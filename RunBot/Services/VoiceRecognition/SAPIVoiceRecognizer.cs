using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Speech.AudioFormat;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Discord.Audio;
using Discord.Audio.Streams;

namespace RunBot.Services.VoiceRecognition
{
    class InputStreamWrapper : Stream
    {
        public InputStreamWrapper(InputStream stream, CancellationToken cancellationToken)
        {
            this.stream = stream;
            this.cancellationToken = cancellationToken;
        }

        readonly InputStream stream;
        readonly CancellationToken cancellationToken;

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => long.MaxValue;

        public override long Position { get => 0; set { } }

        public override int Read(byte[] buffer, int offset, int count)
        {
            byte lastByte = 0;
            int total = 0;
            while (total < count)
            {
                if (cancellationToken.IsCancellationRequested)
                    return 0;

                if (!stream.TryReadFrame(cancellationToken, out var frame))
                {
                    if (lastByte <= byte.MinValue)
                    {
                        const int sampleSize = 2 * 2;
                        for (int i = total; i < total + sampleSize; i++)
                        {
                            buffer[i] = 0;
                        }
                        total += sampleSize;
                    }
                    continue;
                }

                var payload = frame.Payload;
                var writeCount = Math.Min(payload.Length, buffer.Length - total);
                Buffer.BlockCopy(payload, 0, buffer, total, writeCount);
                lastByte = payload[payload.Length - 1];
                total += writeCount;
            }
            return count;
        }

        public override void Flush() { return; }
        public override long Seek(long offset, SeekOrigin origin) => 0;
        public override void SetLength(long value) { return; }
        public override void Write(byte[] buffer, int offset, int count) => throw new NotImplementedException();
    }

    public class SAPIVoiceRecognizer : IVoiceRecognizer
    {
        public SAPIVoiceRecognizer(CancellationToken cancellationToken)
        {
            this.cancellationToken = cancellationToken;
        }

        const float ConfidenceThreshold = 0.25f;

        static readonly CultureInfo culture = new CultureInfo("en-US");

        CancellationToken cancellationToken;

        public async Task RecognizeAsync(string textToRecognize, InputStream audio, Action onRecognized)
        {
            using (var stream = new InputStreamWrapper(audio, cancellationToken))
            using (var speech = new SpeechRecognitionEngine(culture)
            {
                InitialSilenceTimeout = TimeSpan.Zero,
                EndSilenceTimeout = TimeSpan.Zero,
                EndSilenceTimeoutAmbiguous = TimeSpan.Zero
            })
            {
                var gb = new GrammarBuilder(textToRecognize)
                {
                    Culture = culture
                };
                speech.SetInputToAudioStream(stream, new SpeechAudioFormatInfo(48000, AudioBitsPerSample.Sixteen, AudioChannel.Stereo));
                speech.LoadGrammar(new Grammar(gb));

                bool finished = false;

                void OnRecognized()
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        speech.RecognizeAsyncStop();
                        finished = true;
                    }
                    onRecognized();  
                }

                speech.SpeechRecognitionRejected += (obj, args) =>
                {
                    if (args.Result == null)
                        return;
                    if (!(args.Result.Text == textToRecognize && args.Result.Confidence > ConfidenceThreshold))
                        return;
                    OnRecognized();
                };
                speech.SpeechRecognized += (obj, args) =>
                {
                    if (args.Result == null || args.Result.Confidence < ConfidenceThreshold)
                        return;
                    OnRecognized();
                };

                speech.RecognizeAsync(RecognizeMode.Multiple);
                await Task.Run(() => { while (!finished) { Thread.Sleep(100); } }).ConfigureAwait(false);
            }
        }
    }
}

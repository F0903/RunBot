using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
    public class AsyncVoiceRecognizer : IVoiceRecognizer
    {
        class DiscordStreamWrapper : Stream
        {
            public DiscordStreamWrapper(InputStream stream, long streamSize = 5 * 1024)
            {
                this.stream = stream;
                this.streamSize = streamSize;
            }

            private readonly InputStream stream;

            public override bool CanRead => true;

            public override bool CanSeek => true;

            public override bool CanWrite => true;

            readonly long streamSize;
            public override long Length => streamSize;

            public override long Position { get => 0; set { } }

            public override void Flush() { return; }
            public override int Read(byte[] buffer, int offset, int count) //TODO: Fix the indefinite hang here.
            {
                var debugCount = stream.ReadAsync(buffer, offset, count, CancellationToken.None).Result;
                return debugCount;
            }
            public override long Seek(long offset, SeekOrigin origin) => 0;
            public override void SetLength(long value) { return; }
            public override void Write(byte[] buffer, int offset, int count) { return; }
        }

        const float ConfidenceThreshold = 0.7f;

        static readonly CultureInfo culture = new CultureInfo("en-US");

        bool running = false;

        public Task RecognizeAsync(InputStream audio, Action OnRecognized)
        {
            using (var recognizer = new SpeechRecognitionEngine(culture))
            using (audio)
            {
                running = true;
                while (running)
                {
                    var wrapper = new DiscordStreamWrapper(audio);

                    var gb = new GrammarBuilder("run") { Culture = culture };
                    recognizer.LoadGrammar(new Grammar(gb));
                    recognizer.SetInputToAudioStream(wrapper, new SpeechAudioFormatInfo(48000, AudioBitsPerSample.Sixteen, AudioChannel.Stereo));
                    recognizer.SpeechRecognized += (obj, args) =>
                    {
                        if (!running)
                            return;
                        if (args.Result.Confidence < ConfidenceThreshold)
                            return;
                        running = false;
                        OnRecognized();
                    };
                    if (recognizer.Recognize() == null)
                    {
                        running = false;
                        throw new Exception("Recognizer not supported.");
                    }
                }  
            }
            return Task.CompletedTask;
        }

        public void Stop() => running = false;
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
        const float ConfidenceThreshold = 0.7f;

        static readonly CultureInfo culture = new CultureInfo("en-US");

        public async Task RecognizeAsync(InputStream audio, CancellationToken cancellationToken, Action OnRecognized)
        {
            var buffer = new byte[40 * 3840];
            int total = 0;
            int count = 0;
            while ((count = await audio.ReadAsync(buffer, total, buffer.Length)) > 0 && !cancellationToken.IsCancellationRequested)
            {
                total += count;
                if (total < buffer.Length)
                    continue;
                total = 0;

                using (var mem = new MemoryStream(buffer))
                using (var recognizer = new SpeechRecognitionEngine(culture))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var gb = new GrammarBuilder("run") { Culture = culture };
                    recognizer.LoadGrammar(new Grammar(gb));
                    recognizer.SetInputToAudioStream(mem, new SpeechAudioFormatInfo(48000, AudioBitsPerSample.Sixteen, AudioChannel.Stereo));
                    recognizer.SpeechRecognized += (obj, args) =>
                    {
                        if (args.Result.Confidence < ConfidenceThreshold)
                            return;
                        recognizer.RecognizeAsyncStop();
                        OnRecognized();
                    };
                    recognizer.RecognizeAsync(RecognizeMode.Single);

                    bool finished = false;
                    recognizer.RecognizeCompleted += (obj, args) => finished = true;
                    while (!finished && !cancellationToken.IsCancellationRequested) { Thread.Sleep(333); }
                }
            }
        }
    }
}

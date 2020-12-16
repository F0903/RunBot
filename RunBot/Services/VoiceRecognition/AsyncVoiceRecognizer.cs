using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace RunBot.Services.VoiceRecognition
{
    public class AsyncVoiceRecognizer : IVoiceRecognizer
    {
        public AsyncVoiceRecognizer()
        {
            Init();
        }

        ~AsyncVoiceRecognizer()
        {
            recognizer?.Dispose();
        }

        readonly SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine(CultureInfo.GetCultureInfo("en-US"));

        void Init()
        {
            recognizer.LoadGrammar(new Grammar(new GrammarBuilder("Run")));
        }

        public Task RecognizeAsync(Stream audio, Action OnRecognized)
        {
            recognizer.SetInputToAudioStream(audio, new System.Speech.AudioFormat.SpeechAudioFormatInfo(48000, System.Speech.AudioFormat.AudioBitsPerSample.Eight, System.Speech.AudioFormat.AudioChannel.Stereo));
            recognizer.SpeechRecognized += (i, j) => OnRecognized();
            recognizer.Recognize();
            return Task.CompletedTask;
        }
    }
}

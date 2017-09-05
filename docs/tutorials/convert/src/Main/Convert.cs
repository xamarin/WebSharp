using System;
using System.Threading.Tasks;

using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;

namespace Convert
{
    public static class FFMPEGWrapper
    {
        //static Engine engine;
        static string FFMPegPath { get; set; }
        public static bool IsReady { get; set; } = false;

        public static async Task<bool> InitializeEngine(string ffMpegPath)
        {
            Console.WriteLine($"Initializing engine: {ffMpegPath}");

            using (var engine = new Engine(ffMpegPath))
            { }
            FFMPegPath = ffMpegPath;
            IsReady = true;
            return IsReady;
        }

        public static async Task<Metadata> GetMetaData(string input)
        {
            if (!IsReady)
                throw new Exception("Engine has not been initialized.");

            var inputFile = new MediaFile { Filename = input };

            using (var engine = new Engine(FFMPegPath))
            {
                engine.GetMetadata(inputFile);
            }

            return inputFile.Metadata;
        }

        public static async Task Convert(string input, string output, EventHandler<ConvertProgressEventArgs> convertProgress, EventHandler<ConversionCompleteEventArgs> completeHandler)
        {
            if (!IsReady)
                throw new Exception("Engine has not been initialized.");

            var inputFile = new MediaFile { Filename = input };
            var outputFile = new MediaFile { Filename = output };

            var conversionOptions = new ConversionOptions
            {
                VideoFps = 60,
                VideoAspectRatio = VideoAspectRatio.R16_9,
                VideoSize = VideoSize.Hd1080,
                AudioSampleRate = AudioSampleRate.Hz44100,
                Target = Target.Default,
                TargetStandard = TargetStandard.Default
            };

            using (var engine = new Engine(FFMPegPath))
            {
                engine.ConvertProgressEvent += convertProgress;
                engine.ConversionCompleteEvent += completeHandler;
                engine.Convert(inputFile, outputFile);
            }

        }

    }
}
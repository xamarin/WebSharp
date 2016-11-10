using System;
using System.Runtime.InteropServices;

using PepperSharp;

namespace Audio
{
    public class Audio : Instance
    {

        const string PLAY_SOUND_ID = "playSound";
        const string STOP_SOUND_ID = "stopSound";
        const string SET_FREQUENCY_ID = "setFrequency";
        const char MESSAGE_ARGUMENT_SEPARATOR = ':';

        const double DEFAULT_FREQUENCY = 440.0;
        const double PI = 3.141592653589;
        const double TWO_PI = 2.0 * PI;
        // The sample count we will request.
        const uint SAMPLE_FRAME_COUNT = 4096u;
        // Only supporting stereo audio for now.
        const uint CHANNELS = 2u;

        PepperSharp.Audio audio;
        //PPResource audio_;
        double Frequency { get; set; }

        // The last parameter sent to the sin function.  Used to prevent sine wave
        // skips on buffer boundaries.
        double Theta { get; set; }

        // The count of sample frames per channel in an audio buffer.
        uint SampleFrameCount { get; set; }

        public Audio(IntPtr handle) : base(handle)
        {
            SampleFrameCount = SAMPLE_FRAME_COUNT;
            Frequency = DEFAULT_FREQUENCY;

            HandleMessage += OnHandleMessage;
            Initialize += OnInitialize;
        }

        private void OnInitialize(object sender, InitializeEventArgs args)
        {
            // Ask the device for an appropriate sample count size.
            SampleFrameCount = AudioConfig.RecommendSampleFrameCount(this, AudioSampleRate._44100, SAMPLE_FRAME_COUNT);

            var thisHandle = (IntPtr)(GCHandle.Alloc(this, GCHandleType.Normal));

            audio = new PepperSharp.Audio(this, new AudioConfig(this,
                    AudioSampleRate._44100, SampleFrameCount),
                    SineWaveCallback);

        }

        /// <summary>
        /// Called by the browser to handle the postMessage() call in Javascript.
        /// |var_message| is expected to be a string that contains the name of the
        /// method to call.  Note that the setFrequency method takes a single
        /// parameter, the frequency.  The frequency parameter is encoded as a string
        /// and appended to the 'setFrequency' method name after a ':'.  Examples
        /// of possible message strings are:
        ///     playSound
        ///     stopSound
        ///     setFrequency:880
        /// If |var_message| is not a recognized method name, this method does nothing.
        /// </summary>
        /// <param name="varMessage"></param>
        private void OnHandleMessage(object sender, Var varMessage)
        {
            if (!varMessage.IsString)
            {
                return;
            }
            var message = varMessage.AsString();

            if (message == PLAY_SOUND_ID)
            {
                audio.StartPlayback();
            }
            else if (message == STOP_SOUND_ID)
            {
                audio.StopPlayback();
            }
            else if (message.Contains(SET_FREQUENCY_ID))
            {
                // The argument to setFrequency is everything after the first ':'.
                var sep_pos = message.IndexOf(MESSAGE_ARGUMENT_SEPARATOR);
                if (sep_pos > 0)
                {
                    var stringArg = message.Substring(sep_pos + 1);
                    // Got the argument value as a string: try to convert it to a number.
                    double doubleValue = 0;
                    if (Double.TryParse(stringArg, out doubleValue))
                    {
                        Frequency = doubleValue;
                        return;
                    }
                }
            }

        }

        void SineWaveCallback(byte[] samples,
                uint bufferSize,
                double latency,
                object state)
        {

            var frequency = Frequency;
            double delta = TWO_PI * frequency / (int)AudioSampleRate._44100;
            short maxShort = short.MaxValue;

            unsafe
            {
                fixed (byte* pBuffer = samples)
                {
                    short* pSample = (short*)pBuffer;

                    for (int sample_i = 0; sample_i < SampleFrameCount;
                            sample_i++, Theta += delta)
                    {
                        // Keep theta_ from going beyond 2*Pi.
                        if (Theta > TWO_PI)
                        {
                            Theta -= TWO_PI;
                        }

                        var sinValue = Math.Sin(Theta);
                        var scaledValue = (short)((sinValue * maxShort) + (maxShort / 2));
                        for (int channel = 0; channel < CHANNELS; ++channel)
                        {
                            pSample[sample_i] = scaledValue;
                        }
                    }


                }
            }
        }

    }


}

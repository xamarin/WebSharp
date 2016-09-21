using System;

using PepperSharp;
using System.Runtime.InteropServices;

namespace MediaStreamAudio
{
    public class MediaStreamAudio : Instance
    {
        const uint kColorRed = 0xFFFF0000;
        const uint kColorGreen = 0xFF00FF00;
        const uint kColorGrey1 = 0xFF202020;
        const uint kColorGrey2 = 0xFF404040;
        const uint kColorGrey3 = 0xFF606060;

        bool firstBuffer = true;
        uint sampleCount;
        uint channelCount;
        short[] samples_;

        int timer_interval_;

        // Painting stuff.
        PPSize size = PPSize.Zero;
        Graphics2D deviceContext;
        bool pendingPaint;
        bool waitingForFlushCompletion;

        MediaStreamAudioTrack audioTrack;

        public MediaStreamAudio(IntPtr handle) : base(handle)
        {
            Initialize += HandleInitialize;
            ViewChanged += DidChangeView;
            HandleMessage += OnMessage;
        }

        private void OnMessage(object sender, Var var_message)
        {
            if (!var_message.IsDictionary)
                return;
            var var_dictionary_message = new VarDictionary(var_message);
            var var_track = var_dictionary_message.Get("track");
            if (!var_track.IsResource)
                return;

            var resourceTrack = var_track.AsResource();
            audioTrack = new MediaStreamAudioTrack(resourceTrack);
            audioTrack.HandleBuffer += OnGetBuffer;
            audioTrack.GetBuffer();
        }

        void OnGetBuffer (object sender, MediaStreamAudioTrack.AudioBufferInfo audioBufferInfo)
        {
            if (audioBufferInfo.Result != PPError.Ok) {
                Console.WriteLine ($"{audioBufferInfo.Result}");
                return;
            }

            var buffer = new AudioBuffer (audioBufferInfo.AudioBuffer);

            if (buffer.IsEmpty)
               return;
            
            if (buffer.SampleSize != AudioBufferSampleSize._16Bits)
                throw new ArgumentException("Sample size is incorrect");
            
            var data = buffer.DataBuffer;
            var channels = buffer.NumberOfChannels;
            var samples = buffer.NumberOfSamples / channels;
            
            if (channelCount != channels || sampleCount != samples)
            {
                channelCount = channels;
                sampleCount = samples;

                samples_ = new short[sampleCount * channelCount];

                // Try (+ 5) to ensure that we pick up a new set of samples between each
                // timer-generated repaint.
                timer_interval_ = (int)(sampleCount * 1000) / (int)buffer.SampleRate + 5;

                // Start the timer for the first buffer.
                if (firstBuffer)
                {
                    firstBuffer = false;
                    ScheduleNextTimer();
                }
            }

            try
            {
                Marshal.Copy(data, samples_, 0, (int)(sampleCount * channelCount));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            } 

            audioTrack.RecycleBuffer(buffer);
            audioTrack.GetBuffer();
        }

        private void DidChangeView(object sender, View view)
        {
            var position = view.Rect;
            if (position.Size == size)
                return;

            size = position.Size;
            deviceContext = new Graphics2D(this, size, false);
            deviceContext.Flushed += DidFlush;
            if (!BindGraphics(deviceContext))
                return;

            Paint();
        }

        private void DidFlush(object sender, PPError e)
        {
            waitingForFlushCompletion = false;
            if (pendingPaint)
                Paint();
        }

        private void HandleInitialize(object sender, InitializeEventArgs args)
        {
            LogToConsole(PPLogLevel.Log, "Here be dragons");
        }

        void Paint()
        {
            if (waitingForFlushCompletion)
            {
                pendingPaint = true;
                return;
            }

            pendingPaint = false;

            if (size == PPSize.Zero)
                return;  // Nothing to do.

            using (var image = PaintImage (size)) 
            {
                if (!image.IsEmpty)
                {
                    deviceContext.ReplaceContents(image);
                    waitingForFlushCompletion = true;
                    deviceContext.Flush();
                }
            }

        }


        int [] imageDataBuffer = null; // This is our graphics data buffer
        ImageData PaintImage(PPSize imageSize)
        {
            var imageData = new ImageData(this, PPImageDataFormat.BgraPremul, imageSize, false);
            if (imageData.IsEmpty)
              return imageData;

            IntPtr dataPtr = imageData.Data;
            if (dataPtr == IntPtr.Zero)
                return imageData;

            var imageDataSize = imageData.Size.Area;

            if (imageDataBuffer == null || imageDataBuffer.Length != imageDataSize)
                imageDataBuffer = new int[imageDataSize];

            Marshal.Copy(dataPtr, imageDataBuffer, 0, imageDataBuffer.Length);

            var num_pixels = (imageSize.Width * imageSize.Height);
            uint offset = 0;
            var stride = imageData.Stride;

            // Clear to dark grey.
            for (uint i = 0; i < num_pixels; ++i)
            {
                unchecked { imageDataBuffer[offset] = (int)kColorGrey1; }
                offset++;
            }

            int mid_height = imageSize.Height / 2;
            int max_amplitude = imageSize.Height * 4 / 10;

            // Draw some lines.
            for (int x = 0; x<imageSize.Width; x++)
            {
                unchecked { imageDataBuffer[mid_height * imageSize.Width + x] = (int)kColorGrey3; }
                unchecked { imageDataBuffer[(mid_height + max_amplitude) * imageSize.Width + x] = (int)kColorGrey2; }
                unchecked { imageDataBuffer[(mid_height - max_amplitude) * imageSize.Width + x] = (int)kColorGrey2; }
            }

            // Draw our samples.
            for (int x = 0, i = 0;
                x < Math.Min(imageSize.Width, (int)sampleCount);
                     x++, i += (int)channelCount) 
            {
                for (uint ch = 0; ch < Math.Min(channelCount, 2U); ++ch) 
                {
                    int y = samples_[i + ch] * max_amplitude /
                        (short.MaxValue + 1) + mid_height;
                    unchecked { imageDataBuffer[y * imageSize.Width + x] = (ch == 0 ? (int)kColorRed : (int)kColorGreen); }
                }
            }
            Marshal.Copy(imageDataBuffer, 0, dataPtr, imageDataBuffer.Length);

            return imageData;
        }

        // Starts a timer to run Paint() in every |timer_interval_|.
        void ScheduleNextTimer()
        {
            Core.CallOnMainThread(OnTimer, timer_interval_);
        }

        void OnTimer(PPError result)
        {
            ScheduleNextTimer();
            Paint();
        }
    }
}

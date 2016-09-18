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

        bool first_buffer_ = true;
        uint sample_count_;
        uint channel_count_;
        short[] samples_;

        int timer_interval_;

        // Painting stuff.
        PPSize size_ = PPSize.Zero;
        Graphics2D device_context_;
        bool pending_paint_;
        bool waiting_for_flush_completion_;

        MediaStreamAudioTrack audio_track_;

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
            audio_track_ = new MediaStreamAudioTrack(resourceTrack);
            audio_track_.HandleBuffer += OnGetBuffer;
            audio_track_.GetBuffer();
        }

        void OnGetBuffer (object sender, MediaStreamAudioTrack.AudioBufferInfo audioBufferInfo)
        {
            var buffer = audioBufferInfo.AudioBuffer;
            if (buffer.SampleSize != AudioBufferSampleSize._16Bits)
                throw new ArgumentException("Sample size is incorrect");
            
            var data = buffer.DataBuffer;
            var channels = buffer.NumberOfChannels;
            var samples = buffer.NumberOfSamples / channels;
            
            if (channel_count_ != channels || sample_count_ != samples)
            {
                channel_count_ = channels;
                sample_count_ = samples;

                samples_ = new short[sample_count_ * channel_count_];

                // Try (+ 5) to ensure that we pick up a new set of samples between each
                // timer-generated repaint.
                timer_interval_ = (int)(sample_count_ * 1000) / (int)buffer.SampleRate + 5;
                
                // Start the timer for the first buffer.
                if (first_buffer_)
                {
                    first_buffer_ = false;
                    ScheduleNextTimer();
                }
            }

            try
            {
                Marshal.Copy(data, samples_, 0, (int)(sample_count_ * channel_count_));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            } 

            audio_track_.RecycleBuffer(buffer);
            audio_track_.GetBuffer();
        }

        private void DidChangeView(object sender, View view)
        {
            var position = view.Rect;
            if (position.Size == size_)
                return;

            size_ = position.Size;
            device_context_ = new Graphics2D(this, size_, false);
            device_context_.Flushed += DidFlush;
            if (!BindGraphics(device_context_))
                return;

            Paint();
        }

        private void DidFlush(object sender, PPError e)
        {
            waiting_for_flush_completion_ = false;
            if (pending_paint_)
                Paint();
        }

        private void HandleInitialize(object sender, InitializeEventArgs args)
        {
            LogToConsole(PPLogLevel.Log, "Here be dragons");
        }

        void Paint()
        {
            if (waiting_for_flush_completion_)
            {
                pending_paint_ = true;
                return;
            }

            pending_paint_ = false;

            if (size_ == PPSize.Zero)
                return;  // Nothing to do.

            var image = PaintImage(size_);
            if (!image.IsEmpty)
            {
                device_context_.ReplaceContents(image);
                waiting_for_flush_completion_ = true;
                device_context_.Flush();
            }

        }

        ImageData PaintImage(PPSize size)
        {
            var imageData = new ImageData(this, PPImageDataFormat.BgraPremul, size, false);
            if (imageData.IsEmpty)
              return imageData;

            int[] data = null;
            IntPtr dataPtr = imageData.Data;
            if (dataPtr == IntPtr.Zero)
                return imageData;

            data = new int[(imageData.Size.Width * imageData.Size.Height)];
            Marshal.Copy(dataPtr, data, 0, data.Length);

            var num_pixels = (size.Width * size.Height);
            uint offset = 0;
            var stride = imageData.Stride;

            // Clear to dark grey.
            for (uint i = 0; i < num_pixels; ++i)
            {
                unchecked { data[offset] = (int)kColorGrey1; }
                offset++;
            }

            int mid_height = size.Height / 2;
            int max_amplitude = size.Height * 4 / 10;

            // Draw some lines.
            for (int x = 0; x<size.Width; x++)
            {
                unchecked { data[mid_height * size.width + x] = (int)kColorGrey3; }
                unchecked { data[(mid_height + max_amplitude) * size.width + x] = (int)kColorGrey2; }
                unchecked { data[(mid_height - max_amplitude) * size.width + x] = (int)kColorGrey2; }
            }

            // Draw our samples.
            for (int x = 0, i = 0;
                x < Math.Min(size.Width, (int)sample_count_);
                     x++, i += (int)channel_count_) {
                for (uint ch = 0; ch < Math.Min(channel_count_, 2U); ++ch) {
                    int y = samples_[i + ch] * max_amplitude /
                        (short.MaxValue + 1) + mid_height;
                    unchecked { data[y * size.width + x] = (ch == 0 ? (int)kColorRed : (int)kColorGreen); }
                }
            }
            Marshal.Copy(data, 0, dataPtr, data.Length);

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

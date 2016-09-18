using System;

namespace PepperSharp
{
    public class AudioBuffer : Resource
    {
        public AudioBuffer(AudioBuffer other) : base(other)
        { }

        internal AudioBuffer(PPResource resource) : base(resource)
        { }

        internal AudioBuffer(PassRef pasref, PPResource resource) : base(pasref, resource)
        { }

        /// <summary>
        /// Gets or Sets the timestamp of the audio buffer.  A Time Delta containing the timestamp of the audio
        /// buffer. Given in seconds since the start of the containing audio stream.
        /// </summary>
        public double TimeStamp
        {
            get { return PPBAudioBuffer.GetTimestamp(this); }
            set { PPBAudioBuffer.SetTimestamp(this, value); }
        }

        /// <summary>
        /// Gets or Sets the timestamp of the audio buffer.  A DateTime containing the timestamp of the audio
        /// buffer. Given in seconds since the start of the containing audio stream.
        /// </summary>
        public DateTime DateTimeStamp
        {
            get { return PepperSharpUtils.ConvertFromPepperTimestamp( PPBAudioBuffer.GetTimestamp(this));  }
            set { PPBAudioBuffer.SetTimestamp(this, PepperSharpUtils.ConvertToPepperTimestamp(value)); }
        }

        /// <summary>
        /// Gets the sample rate of the audio buffer.
        /// </summary>
        public AudioBufferSampleRate SampleRate
            => (AudioBufferSampleRate)PPBAudioBuffer.GetSampleRate(this);

        /// <summary>
        /// Gets the sample size of the audio buffer.
        /// </summary>
        public AudioBufferSampleSize SampleSize
            => (AudioBufferSampleSize)PPBAudioBuffer.GetSampleSize(this);

        /// <summary>
        /// Gets the number of channels in the audio buffer.
        /// </summary>
        public uint NumberOfChannels
            => PPBAudioBuffer.GetNumberOfChannels(this);

        /// <summary>
        /// Gets the number of samples in the audio buffer.
        /// For example, at a sampling rate of 44,100 Hz in stereo audio, a buffer
        /// containing 4,410 * 2 samples would have a duration of 100 milliseconds.
        /// </summary>
        public uint NumberOfSamples
            => PPBAudioBuffer.GetNumberOfSamples(this);

        /// <summary>
        /// Gets the data buffer containing the audio buffer samples.
        ///
        /// A pointer to the beginning of the data buffer.
        /// </summary>
        public IntPtr DataBuffer
            => PPBAudioBuffer.GetDataBuffer(this);

        /// <summary>
        /// Gets the size of data buffer in bytes.
        /// </summary>
        public uint DataBufferSize
            => PPBAudioBuffer.GetDataBufferSize(this);
    }
}

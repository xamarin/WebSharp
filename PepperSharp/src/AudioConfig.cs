using System;

namespace PepperSharp
{
    public class AudioConfig : Resource
    {
        /// <summary>
        /// Getter function for returning the internal
        /// <code>AudioSampleRate</code> enum.
        /// </summary>
        public AudioSampleRate SampleRate { get; private set; }

        /// <summary>
        /// Getter function for returning the internal sample frame count.
        /// </summary>
        public uint SampleFrameCount { get; private set; }

        /// <summary>
        /// A constructor that creates an audio config based on the given sample rate
        /// and frame count. If the rate and frame count aren't supported, the
        /// resulting resource will be is_null(). You can pass the result of
        /// RecommendSampleFrameCount() as the sample frame count.
        /// </summary>
        /// <param name="instance">The instance associated with this resource.</param>
        /// <param name="sampleRate">The sample rate <see cref="AudioSampleRate"/></param>
        /// <param name="sampleFrameCount">A uint frame count returned from the
        /// <code>RecommendSampleFrameCount</code> function.</param>
        public AudioConfig(Instance instance, AudioSampleRate sampleRate, uint sampleFrameCount)
        {
            handle = PPBAudioConfig.CreateStereo16Bit(instance, (PPAudioSampleRate)sampleRate, sampleFrameCount);
            SampleRate = sampleRate;
            SampleFrameCount = sampleFrameCount;
        }

        public static AudioSampleRate RecommendSampleRate(Instance instance)
            => (AudioSampleRate)PPBAudioConfig.RecommendSampleRate(instance);

        public static uint RecommendSampleFrameCount(Instance instance,
                                                    AudioSampleRate sampleRate,
                                                    uint requestedSampleFrameCount)
            => PPBAudioConfig.RecommendSampleFrameCount(instance, (PPAudioSampleRate)sampleRate, requestedSampleFrameCount);
    }

    /**
    * This enumeration contains audio frame count constants.
    */
    public enum AudioFrameSize
    {
        /// <summary>
        /// Minimum possible frame count.
        /// </summary>
        Minimum = 64,
        /// <summary>
        /// Maximum possible frame count
        /// </summary>
        Maximum = 32768
    }

    /**
     * AudioSampleRate is an enumeration of the different audio sampling rates.
     */
    public enum AudioSampleRate
    {
        None = 0,
        /// <summary>
        /// Sample rate used on CDs
        /// </summary>
        _44100 = 44100,
        /// <summary>
        /// Sample rate used on DVDs and Digital Audio Tapes
        /// </summary>
        _48000 = 48000
    }
}

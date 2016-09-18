/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_audio_buffer.idl modified Thu May 12 07:00:00 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * Defines the <code>PPB_AudioBuffer</code> interface.
 */


/**
 * @addtogroup Enums
 * @{
 */
/**
 * PP_AudioBuffer_SampleRate is an enumeration of the different audio sample
 * rates.
 */
public enum PPAudioBufferSampleRate {
  Unknown = 0,
  _8000 = 8000,
  _16000 = 16000,
  _22050 = 22050,
  _32000 = 32000,
  _44100 = 44100,
  _48000 = 48000,
  _96000 = 96000,
  _192000 = 192000
}

/**
 * PP_AudioBuffer_SampleSize is an enumeration of the different audio sample
 * sizes.
 */
public enum PPAudioBufferSampleSize {
  Unknown = 0,
  _16Bits = 2
}
/**
 * @}
 */

/**
 * @addtogroup Interfaces
 * @{
 */
internal static partial class PPBAudioBuffer {
  [DllImport("PepperPlugin", EntryPoint = "PPB_AudioBuffer_IsAudioBuffer")]
  extern static PPBool _IsAudioBuffer ( PPResource resource);

  /**
   * Determines if a resource is an AudioBuffer resource.
   *
   * @param[in] resource The <code>PP_Resource</code> to test.
   *
   * @return A <code>PP_Bool</code> with <code>PP_TRUE</code> if the given
   * resource is an AudioBuffer resource or <code>PP_FALSE</code> otherwise.
   */
  public static PPBool IsAudioBuffer ( PPResource resource)
  {
  	return _IsAudioBuffer (resource);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_AudioBuffer_GetTimestamp")]
  extern static double _GetTimestamp ( PPResource buffer);

  /**
   * Gets the timestamp of the audio buffer.
   *
   * @param[in] buffer A <code>PP_Resource</code> corresponding to an audio
   * buffer resource.
   *
   * @return A <code>PP_TimeDelta</code> containing the timestamp of the audio
   * buffer. Given in seconds since the start of the containing audio stream.
   */
  public static double GetTimestamp ( PPResource buffer)
  {
  	return _GetTimestamp (buffer);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_AudioBuffer_SetTimestamp")]
  extern static void _SetTimestamp ( PPResource buffer,  double timestamp);

  /**
   * Sets the timestamp of the audio buffer.
   *
   * @param[in] buffer A <code>PP_Resource</code> corresponding to an audio
   * buffer resource.
   * @param[in] timestamp A <code>PP_TimeDelta</code> containing the timestamp
   * of the audio buffer. Given in seconds since the start of the containing
   * audio stream.
   */
  public static void SetTimestamp ( PPResource buffer,  double timestamp)
  {
  	 _SetTimestamp (buffer, timestamp);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_AudioBuffer_GetSampleRate")]
  extern static PPAudioBufferSampleRate _GetSampleRate ( PPResource buffer);

  /**
   * Gets the sample rate of the audio buffer.
   *
   * @param[in] buffer A <code>PP_Resource</code> corresponding to an audio
   * buffer resource.
   *
   * @return The sample rate of the audio buffer.
   */
  public static PPAudioBufferSampleRate GetSampleRate ( PPResource buffer)
  {
  	return _GetSampleRate (buffer);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_AudioBuffer_GetSampleSize")]
  extern static PPAudioBufferSampleSize _GetSampleSize ( PPResource buffer);

  /**
   * Gets the sample size of the audio buffer.
   *
   * @param[in] buffer A <code>PP_Resource</code> corresponding to an audio
   * buffer resource.
   *
   * @return The sample size of the audio buffer.
   */
  public static PPAudioBufferSampleSize GetSampleSize ( PPResource buffer)
  {
  	return _GetSampleSize (buffer);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_AudioBuffer_GetNumberOfChannels")]
  extern static uint _GetNumberOfChannels ( PPResource buffer);

  /**
   * Gets the number of channels in the audio buffer.
   *
   * @param[in] buffer A <code>PP_Resource</code> corresponding to an audio
   * buffer resource.
   *
   * @return The number of channels in the audio buffer.
   */
  public static uint GetNumberOfChannels ( PPResource buffer)
  {
  	return _GetNumberOfChannels (buffer);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_AudioBuffer_GetNumberOfSamples")]
  extern static uint _GetNumberOfSamples ( PPResource buffer);

  /**
   * Gets the number of samples in the audio buffer.
   *
   * @param[in] buffer A <code>PP_Resource</code> corresponding to an audio
   * buffer resource.
   *
   * @return The number of samples in the audio buffer.
   * For example, at a sampling rate of 44,100 Hz in stereo audio, a buffer
   * containing 4410 * 2 samples would have a duration of 100 milliseconds.
   */
  public static uint GetNumberOfSamples ( PPResource buffer)
  {
  	return _GetNumberOfSamples (buffer);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_AudioBuffer_GetDataBuffer")]
  extern static IntPtr _GetDataBuffer ( PPResource buffer);

  /**
   * Gets the data buffer containing the audio samples.
   *
   * @param[in] buffer A <code>PP_Resource</code> corresponding to an audio
   * buffer resource.
   *
   * @return A pointer to the beginning of the data buffer.
   */
  public static IntPtr GetDataBuffer ( PPResource buffer)
  {
  	return _GetDataBuffer (buffer);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_AudioBuffer_GetDataBufferSize")]
  extern static uint _GetDataBufferSize ( PPResource buffer);

  /**
   * Gets the size of the data buffer in bytes.
   *
   * @param[in] buffer A <code>PP_Resource</code> corresponding to an audio
   * buffer resource.
   *
   * @return The size of the data buffer in bytes.
   */
  public static uint GetDataBufferSize ( PPResource buffer)
  {
  	return _GetDataBufferSize (buffer);
  }


}
/**
 * @}
 */


}

/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_audio.idl modified Thu May 12 07:00:00 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * This file defines the <code>PPB_Audio</code> interface, which provides
 * realtime stereo audio streaming capabilities.
 */


/**
 * @addtogroup Typedefs
 * @{
 */
/**
 * <code>PPB_Audio_Callback</code> defines the type of an audio callback
 * function used to fill the audio buffer with data. Please see the
 * Create() function in the <code>PPB_Audio</code> interface for
 * more details on this callback.
 *
 * @param[in] sample_buffer A buffer to fill with audio data.
 * @param[in] buffer_size_in_bytes The size of the buffer in bytes.
 * @param[in] latency How long before the audio data is to be presented.
 * @param[inout] user_data An opaque pointer that was passed into
 * <code>PPB_Audio.Create()</code>.
 */
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void PPBAudioCallback (IntPtr sample_buffer,
                                       uint buffer_size_in_bytes,
                                       double latency,
                                       IntPtr user_data);

/**
 * @}
 */

/**
 * @addtogroup Interfaces
 * @{
 */
/**
 * The <code>PPB_Audio</code> interface contains pointers to several functions
 * for handling audio resources. Refer to the
 * <a href="/native-client/devguide/coding/audio.html">Audio</a>
 * chapter in the Developer's Guide for information on using this interface.
 * Please see descriptions for each <code>PPB_Audio</code> and
 * <code>PPB_AudioConfig</code> function for more details. A C example using
 * <code>PPB_Audio</code> and <code>PPB_AudioConfig</code> follows.
 *
 * <strong>Example: </strong>
 *
 * @code
 * void audio_callback(void* sample_buffer,
 *                     uint32_t buffer_size_in_bytes,
 *                     void* user_data) {
 *   ... quickly fill in the buffer with samples and return to caller ...
 *  }
 *
 * ...Assume the application has cached the audio configuration interface in
 * audio_config_interface and the audio interface in
 * audio_interface...
 *
 * uint32_t count = audio_config_interface->RecommendSampleFrameCount(
 *     PP_AUDIOSAMPLERATE_44100, 4096);
 * PP_Resource pp_audio_config = audio_config_interface->CreateStereo16Bit(
 *     pp_instance, PP_AUDIOSAMPLERATE_44100, count);
 * PP_Resource pp_audio = audio_interface->Create(pp_instance, pp_audio_config,
 *     audio_callback, NULL);
 * audio_interface->StartPlayback(pp_audio);
 *
 * ...audio_callback() will now be periodically invoked on a separate thread...
 * @endcode
 */
public static partial class PPBAudio {
  [DllImport("PepperPlugin", EntryPoint = "PPB_Audio_Create")]
  extern static PPResource _Create ( PPInstance instance,
                                     PPResource config,
                                     PPBAudioCallback audio_callback,
                                    ref IntPtr user_data);

  /**
   * Create() creates an audio resource. No sound will be heard until
   * StartPlayback() is called. The callback is called with the buffer address
   * and given user data whenever the buffer needs to be filled. From within the
   * callback, you should not call <code>PPB_Audio</code> functions. The
   * callback will be called on a different thread than the one which created
   * the interface. For performance-critical applications (i.e. low-latency
   * audio), the callback should avoid blocking or calling functions that can
   * obtain locks, such as malloc. The layout and the size of the buffer passed
   * to the audio callback will be determined by the device configuration and is
   * specified in the <code>AudioConfig</code> documentation.
   *
   * @param[in] instance A <code>PP_Instance</code> identifying one instance
   * of a module.
   * @param[in] config A <code>PP_Resource</code> corresponding to an audio
   * config resource.
   * @param[in] audio_callback A <code>PPB_Audio_Callback</code> callback
   * function that the browser calls when it needs more samples to play.
   * @param[in] user_data A pointer to user data used in the callback function.
   *
   * @return A <code>PP_Resource</code> containing the audio resource if
   * successful or 0 if the configuration cannot be honored or the callback is
   * null.
   */
  public static PPResource Create ( PPInstance instance,
                                    PPResource config,
                                    PPBAudioCallback audio_callback,
                                   ref IntPtr user_data)
  {
  	return _Create (instance, config, audio_callback, ref user_data);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_Audio_IsAudio")]
  extern static PPBool _IsAudio ( PPResource resource);

  /**
   * IsAudio() determines if the provided resource is an audio resource.
   *
   * @param[in] resource A <code>PP_Resource</code> corresponding to a generic
   * resource.
   *
   * @return A <code>PP_Bool</code> containing containing <code>PP_TRUE</code>
   * if the given resource is an Audio resource, otherwise
   * <code>PP_FALSE</code>.
   */
  public static PPBool IsAudio ( PPResource resource)
  {
  	return _IsAudio (resource);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_Audio_GetCurrentConfig")]
  extern static PPResource _GetCurrentConfig ( PPResource audio);

  /**
   * GetCurrrentConfig() returns an audio config resource for the given audio
   * resource.
   *
   * @param[in] config A <code>PP_Resource</code> corresponding to an audio
   * resource.
   *
   * @return A <code>PP_Resource</code> containing the audio config resource if
   * successful.
   */
  public static PPResource GetCurrentConfig ( PPResource audio)
  {
  	return _GetCurrentConfig (audio);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_Audio_StartPlayback")]
  extern static PPBool _StartPlayback ( PPResource audio);

  /**
   * StartPlayback() starts the playback of the audio resource and begins
   * periodically calling the callback.
   *
   * @param[in] config A <code>PP_Resource</code> corresponding to an audio
   * resource.
   *
   * @return A <code>PP_Bool</code> containing <code>PP_TRUE</code> if
   * successful, otherwise <code>PP_FALSE</code>. Also returns
   * <code>PP_TRUE</code> (and be a no-op) if called while playback is already
   * in progress.
   */
  public static PPBool StartPlayback ( PPResource audio)
  {
  	return _StartPlayback (audio);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_Audio_StopPlayback")]
  extern static PPBool _StopPlayback ( PPResource audio);

  /**
   * StopPlayback() stops the playback of the audio resource.
   *
   * @param[in] config A <code>PP_Resource</code> corresponding to an audio
   * resource.
   *
   * @return A <code>PP_Bool</code> containing <code>PP_TRUE</code> if
   * successful, otherwise <code>PP_FALSE</code>. Also returns
   * <code>PP_TRUE</code> (and is a no-op) if called while playback is already
   * stopped. If a callback is in progress, StopPlayback() will block until the
   * callback completes.
   */
  public static PPBool StopPlayback ( PPResource audio)
  {
  	return _StopPlayback (audio);
  }


}
/**
 * @}
 */


}

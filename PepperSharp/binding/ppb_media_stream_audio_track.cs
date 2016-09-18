/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_media_stream_audio_track.idl modified Thu May 12 07:00:02 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * Defines the <code>PPB_MediaStreamAudioTrack</code> interface. Used for
 * receiving audio samples from a MediaStream audio track in the browser.
 */


/**
 * @addtogroup Enums
 * @{
 */
/**
 * This enumeration contains audio track attributes which are used by
 * <code>Configure()</code>.
 */
public enum PPMediaStreamAudioTrackAttrib {
  /**
   * Attribute list terminator.
   */
  None = 0,
  /**
   * The maximum number of buffers to hold audio samples.
   * Note: this is only used as advisory; the browser may allocate more or fewer
   * based on available resources. How many buffers depends on usage -
   * request at least 2 to make sure latency doesn't cause lost samples. If
   * the plugin expects to hold on to more than one buffer at a time (e.g. to do
   * multi-buffer processing), it should request that many more.
   */
  Buffers = 1,
  /**
   * The sample rate of audio data in buffers. The attribute value is a
   * <code>PP_AudioBuffer_SampleRate</code>.
   */
  SampleRate = 2,
  /**
   * The sample size of audio data in buffers in bytes. The attribute value is a
   * <code>PP_AudioBuffer_SampleSize</code>.
   */
  SampleSize = 3,
  /**
   * The number of channels in audio buffers.
   *
   * Supported values: 1, 2
   */
  Channels = 4,
  /**
   * The duration of an audio buffer in milliseconds.
   *
   * Valid range: 10 to 10000
   */
  Duration = 5
}
/**
 * @}
 */

/**
 * @addtogroup Interfaces
 * @{
 */
internal static partial class PPBMediaStreamAudioTrack {
  [DllImport(
      "PepperPlugin",
      EntryPoint = "PPB_MediaStreamAudioTrack_IsMediaStreamAudioTrack")]
  extern static PPBool _IsMediaStreamAudioTrack ( PPResource resource);

  /**
   * Determines if a resource is a MediaStream audio track resource.
   *
   * @param[in] resource The <code>PP_Resource</code> to test.
   *
   * @return A <code>PP_Bool</code> with <code>PP_TRUE</code> if the given
   * resource is a Mediastream audio track resource or <code>PP_FALSE</code>
   * otherwise.
   */
  public static PPBool IsMediaStreamAudioTrack ( PPResource resource)
  {
  	return _IsMediaStreamAudioTrack (resource);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_MediaStreamAudioTrack_Configure")]
  extern static int _Configure ( PPResource audio_track,
                                 int[] attrib_list,
                                 PPCompletionCallback callback);

  /**
   * Configures underlying buffers for incoming audio samples.
   * If the application doesn't want to drop samples, then the
   * <code>PP_MEDIASTREAMAUDIOTRACK_ATTRIB_BUFFERS</code> should be
   * chosen such that inter-buffer processing time variability won't overrun all
   * the input buffers. If all buffers are filled, then samples will be
   * dropped. The application can detect this by examining the timestamp on
   * returned buffers. If <code>Configure()</code> is not called, default
   * settings will be used. Calls to Configure while the plugin holds
   * buffers will fail.
   * Example usage from plugin code:
   * @code
   * int32_t attribs[] = {
   *     PP_MEDIASTREAMAUDIOTRACK_ATTRIB_BUFFERS, 4,
   *     PP_MEDIASTREAMAUDIOTRACK_ATTRIB_DURATION, 10,
   *     PP_MEDIASTREAMAUDIOTRACK_ATTRIB_NONE};
   * track_if->Configure(track, attribs, callback);
   * @endcode
   *
   * @param[in] audio_track A <code>PP_Resource</code> corresponding to an audio
   * resource.
   * @param[in] attrib_list A list of attribute name-value pairs in which each
   * attribute is immediately followed by the corresponding desired value.
   * The list is terminated by
   * <code>PP_MEDIASTREAMAUDIOTRACK_ATTRIB_NONE</code>.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion of <code>Configure()</code>.
   *
   * @return An int32_t containing a result code from <code>pp_errors.h</code>.
   */
  public static int Configure ( PPResource audio_track,
                                int[] attrib_list,
                                PPCompletionCallback callback)
  {
  	return _Configure (audio_track, attrib_list, callback);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_MediaStreamAudioTrack_GetAttrib")]
  extern static int _GetAttrib ( PPResource audio_track,
                                 PPMediaStreamAudioTrackAttrib attrib,
                                out int value);

  /**
   * Gets attribute value for a given attribute name.
   *
   * @param[in] audio_track A <code>PP_Resource</code> corresponding to an audio
   * resource.
   * @param[in] attrib A <code>PP_MediaStreamAudioTrack_Attrib</code> for
   * querying.
   * @param[out] value A int32_t for storing the attribute value on success.
   * Otherwise, the value will not be changed.
   *
   * @return An int32_t containing a result code from <code>pp_errors.h</code>.
   */
  public static int GetAttrib ( PPResource audio_track,
                                PPMediaStreamAudioTrackAttrib attrib,
                               out int value)
  {
  	return _GetAttrib (audio_track, attrib, out value);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_MediaStreamAudioTrack_GetId")]
  extern static PPVar _GetId ( PPResource audio_track);

  /**
   * Returns the track ID of the underlying MediaStream audio track.
   *
   * @param[in] audio_track The <code>PP_Resource</code> to check.
   *
   * @return A <code>PP_Var</code> containing the MediaStream track ID as
   * a string.
   */
  public static PPVar GetId ( PPResource audio_track)
  {
  	return _GetId (audio_track);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_MediaStreamAudioTrack_HasEnded")]
  extern static PPBool _HasEnded ( PPResource audio_track);

  /**
   * Checks whether the underlying MediaStream track has ended.
   * Calls to GetBuffer while the track has ended are safe to make and will
   * complete, but will fail.
   *
   * @param[in] audio_track The <code>PP_Resource</code> to check.
   *
   * @return A <code>PP_Bool</code> with <code>PP_TRUE</code> if the given
   * MediaStream track has ended or <code>PP_FALSE</code> otherwise.
   */
  public static PPBool HasEnded ( PPResource audio_track)
  {
  	return _HasEnded (audio_track);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_MediaStreamAudioTrack_GetBuffer")]
  extern static int _GetBuffer ( PPResource audio_track,
                                out PPResource buffer,
                                 PPCompletionCallback callback);

  /**
   * Gets the next audio buffer from the MediaStream track.
   * If internal processing is slower than the incoming buffer rate, new buffers
   * will be dropped from the incoming stream. Once all buffers are full,
   * audio samples will be dropped until <code>RecycleBuffer()</code> is called
   * to free a slot for another buffer.
   * If there are no audio data in the input buffer,
   * <code>PP_OK_COMPLETIONPENDING</code> will be returned immediately and the
   * <code>callback</code> will be called, when a new buffer of audio samples
   * is received or an error happens.
   *
   * @param[in] audio_track A <code>PP_Resource</code> corresponding to an audio
   * resource.
   * @param[out] buffer A <code>PP_Resource</code> corresponding to
   * an AudioBuffer resource.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion of GetBuffer().
   *
   * @return An int32_t containing a result code from <code>pp_errors.h</code>.
   */
  public static int GetBuffer ( PPResource audio_track,
                               out PPResource buffer,
                                PPCompletionCallback callback)
  {
  	return _GetBuffer (audio_track, out buffer, callback);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_MediaStreamAudioTrack_RecycleBuffer")]
  extern static int _RecycleBuffer ( PPResource audio_track,
                                     PPResource buffer);

  /**
   * Recycles a buffer returned by <code>GetBuffer()</code>, so the track can
   * reuse the buffer. And the buffer will become invalid. The caller should
   * release all references it holds to <code>buffer</code> and not use it
   * anymore.
   *
   * @param[in] audio_track A <code>PP_Resource</code> corresponding to an audio
   * resource.
   * @param[in] buffer A <code>PP_Resource</code> corresponding to
   * an AudioBuffer resource returned by <code>GetBuffer()</code>.
   *
   * @return An int32_t containing a result code from <code>pp_errors.h</code>.
   */
  public static int RecycleBuffer ( PPResource audio_track,  PPResource buffer)
  {
  	return _RecycleBuffer (audio_track, buffer);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_MediaStreamAudioTrack_Close")]
  extern static void _Close ( PPResource audio_track);

  /**
   * Closes the MediaStream audio track and disconnects it from the audio
   * source. After calling <code>Close()</code>, no new buffers will be
   * received.
   *
   * @param[in] audio_track A <code>PP_Resource</code> corresponding to a
   * MediaStream audio track resource.
   */
  public static void Close ( PPResource audio_track)
  {
  	 _Close (audio_track);
  }


}
/**
 * @}
 */


}

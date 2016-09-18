/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_audio_encoder.idl modified Thu May 12 07:00:00 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {

/**
 * @file
 * This file defines the <code>PPB_AudioEncoder</code> interface.
 */


/**
 * @addtogroup Interfaces
 * @{
 */
/**
 * Audio encoder interface.
 *
 * Typical usage:
 * - Call Create() to create a new audio encoder resource.
 * - Call GetSupportedProfiles() to determine which codecs and profiles are
 *   available.
 * - Call Initialize() to initialize the encoder for a supported profile.
 * - Call GetBuffer() to get an empty buffer and fill it in, or get an audio
 *   buffer from another resource, e.g. <code>PPB_MediaStreamAudioTrack</code>.
 * - Call Encode() to push the audio buffer to the encoder. If an external
 *   buffer is pushed, wait for completion to recycle the buffer.
 * - Call GetBitstreamBuffer() continuously (waiting for each previous call to
 *   complete) to pull encoded buffers from the encoder.
 * - Call RecycleBitstreamBuffer() after consuming the data in the bitstream
 *   buffer.
 * - To destroy the encoder, the plugin should release all of its references to
 *   it. Any pending callbacks will abort before the encoder is destroyed.
 *
 * Available audio codecs vary by platform.
 * All: opus.
 */
internal static partial class PPBAudioEncoder { /* dev */
  [DllImport("PepperPlugin", EntryPoint = "PPB_AudioEncoder_Create")]
  extern static PPResource _Create ( PPInstance instance);

  /**
   * Creates a new audio encoder resource.
   *
   * @param[in] instance A <code>PP_Instance</code> identifying the instance
   * with the audio encoder.
   *
   * @return A <code>PP_Resource</code> corresponding to an audio encoder if
   * successful or 0 otherwise.
   */
  public static PPResource Create ( PPInstance instance)
  {
  	return _Create (instance);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_AudioEncoder_IsAudioEncoder")]
  extern static PPBool _IsAudioEncoder ( PPResource resource);

  /**
   * Determines if the given resource is an audio encoder.
   *
   * @param[in] resource A <code>PP_Resource</code> identifying a resource.
   *
   * @return <code>PP_TRUE</code> if the resource is a
   * <code>PPB_AudioEncoder</code>, <code>PP_FALSE</code> if the resource is
   * invalid or some other type.
   */
  public static PPBool IsAudioEncoder ( PPResource resource)
  {
  	return _IsAudioEncoder (resource);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_AudioEncoder_GetSupportedProfiles")]
  extern static int _GetSupportedProfiles ( PPResource audio_encoder,
                                            PPArrayOutput output,
                                            PPCompletionCallback callback);

  /**
   * Gets an array of supported audio encoder profiles.
   * These can be used to choose a profile before calling Initialize().
   *
   * @param[in] audio_encoder A <code>PP_Resource</code> identifying the audio
   * encoder.
   * @param[in] output A <code>PP_ArrayOutput</code> to receive the supported
   * <code>PP_AudioProfileDescription</code> structs.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion.
   *
   * @return If >= 0, the number of supported profiles returned, otherwise an
   * error code from <code>pp_errors.h</code>.
   */
  public static int GetSupportedProfiles ( PPResource audio_encoder,
                                           PPArrayOutput output,
                                           PPCompletionCallback callback)
  {
  	return _GetSupportedProfiles (audio_encoder, output, callback);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_AudioEncoder_Initialize")]
  extern static int _Initialize ( PPResource audio_encoder,
                                  uint channels,
                                  PPAudioBufferSampleRate input_sample_rate,
                                  PPAudioBufferSampleSize input_sample_size,
                                  PPAudioProfile output_profile,
                                  uint initial_bitrate,
                                  PPHardwareAcceleration acceleration,
                                  PPCompletionCallback callback);

  /**
   * Initializes an audio encoder resource. The plugin should call Initialize()
   * successfully before calling any of the functions below.
   *
   * @param[in] audio_encoder A <code>PP_Resource</code> identifying the audio
   * encoder.
   * @param[in] channels The number of audio channels to encode.
   * @param[in] input_sampling_rate The sampling rate of the input audio buffer.
   * @param[in] input_sample_size The sample size of the input audio buffer.
   * @param[in] output_profile A <code>PP_AudioProfile</code> specifying the
   * codec profile of the encoded output stream.
   * @param[in] initial_bitrate The initial bitrate for the encoder.
   * @param[in] acceleration A <code>PP_HardwareAcceleration</code> specifying
   * whether to use a hardware accelerated or a software implementation.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   * Returns PP_ERROR_NOTSUPPORTED if audio encoding is not available, or the
   * requested codec profile is not supported.
   */
  public static int Initialize ( PPResource audio_encoder,
                                 uint channels,
                                 PPAudioBufferSampleRate input_sample_rate,
                                 PPAudioBufferSampleSize input_sample_size,
                                 PPAudioProfile output_profile,
                                 uint initial_bitrate,
                                 PPHardwareAcceleration acceleration,
                                 PPCompletionCallback callback)
  {
  	return _Initialize (audio_encoder,
                       channels,
                       input_sample_rate,
                       input_sample_size,
                       output_profile,
                       initial_bitrate,
                       acceleration,
                       callback);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_AudioEncoder_GetNumberOfSamples")]
  extern static int _GetNumberOfSamples ( PPResource audio_encoder);

  /**
   * Gets the number of audio samples per channel that audio buffers must
   * contain in order to be processed by the encoder. This will be the number of
   * samples per channels contained in buffers returned by GetBuffer().
   *
   * @param[in] audio_encoder A <code>PP_Resource</code> identifying the audio
   * encoder.
   * @return An int32_t containing the number of samples required, or an error
   * code from <code>pp_errors.h</code>.
   * Returns PP_ERROR_FAILED if Initialize() has not successfully completed.
   */
  public static int GetNumberOfSamples ( PPResource audio_encoder)
  {
  	return _GetNumberOfSamples (audio_encoder);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_AudioEncoder_GetBuffer")]
  extern static int _GetBuffer ( PPResource audio_encoder,
                                out PPResource audio_buffer,
                                 PPCompletionCallback callback);

  /**
   * Gets a blank audio buffer (with metadata given by the Initialize()
   * call) which can be filled with audio data and passed to the encoder.
   *
   * @param[in] audio_encoder A <code>PP_Resource</code> identifying the audio
   * encoder.
   * @param[out] audio_buffer A blank <code>PPB_AudioBuffer</code> resource.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   * Returns PP_ERROR_FAILED if Initialize() has not successfully completed.
   */
  public static int GetBuffer ( PPResource audio_encoder,
                               out PPResource audio_buffer,
                                PPCompletionCallback callback)
  {
  	return _GetBuffer (audio_encoder, out audio_buffer, callback);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_AudioEncoder_Encode")]
  extern static int _Encode ( PPResource audio_encoder,
                              PPResource audio_buffer,
                              PPCompletionCallback callback);

  /**
   * Encodes an audio buffer.
   *
   * @param[in] audio_encoder A <code>PP_Resource</code> identifying the audio
   * encoder.
   * @param[in] audio_buffer The <code>PPB_AudioBuffer</code> to be encoded.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion. Plugins that pass <code>PPB_AudioBuffer</code> resources owned
   * by other resources should wait for completion before reusing them.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   * Returns PP_ERROR_FAILED if Initialize() has not successfully completed.
   */
  public static int Encode ( PPResource audio_encoder,
                             PPResource audio_buffer,
                             PPCompletionCallback callback)
  {
  	return _Encode (audio_encoder, audio_buffer, callback);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_AudioEncoder_GetBitstreamBuffer")]
  extern static int _GetBitstreamBuffer (
      PPResource audio_encoder,
      out PPAudioBitstreamBuffer bitstream_buffer,
      PPCompletionCallback callback);

  /**
   * Gets the next encoded bitstream buffer from the encoder.
   *
   * @param[in] audio_encoder A <code>PP_Resource</code> identifying the audio
   * encoder.
   * @param[out] bitstream_buffer A <code>PP_BitstreamBuffer</code> containing
   * encoded audio data.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion. The plugin can call GetBitstreamBuffer from the callback in
   * order to continuously "pull" bitstream buffers from the encoder.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   * Returns PP_ERROR_FAILED if Initialize() has not successfully completed.
   * Returns PP_ERROR_INPROGRESS if a prior call to GetBitstreamBuffer() has
   * not completed.
   */
  public static int GetBitstreamBuffer (
      PPResource audio_encoder,
      out PPAudioBitstreamBuffer bitstream_buffer,
      PPCompletionCallback callback)
  {
  	return _GetBitstreamBuffer (audio_encoder, out bitstream_buffer, callback);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_AudioEncoder_RecycleBitstreamBuffer")]
  extern static void _RecycleBitstreamBuffer (
      PPResource audio_encoder,
      PPAudioBitstreamBuffer bitstream_buffer);

  /**
   * Recycles a bitstream buffer back to the encoder.
   *
   * @param[in] audio_encoder A <code>PP_Resource</code> identifying the audio
   * encoder.
   * @param[in] bitstream_buffer A <code>PP_BitstreamBuffer</code> that is no
   * longer needed by the plugin.
   */
  public static void RecycleBitstreamBuffer (
      PPResource audio_encoder,
      PPAudioBitstreamBuffer bitstream_buffer)
  {
  	 _RecycleBitstreamBuffer (audio_encoder, bitstream_buffer);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_AudioEncoder_RequestBitrateChange")]
  extern static void _RequestBitrateChange ( PPResource audio_encoder,
                                             uint bitrate);

  /**
   * Requests a change to the encoding bitrate. This is only a request,
   * fulfilled on a best-effort basis.
   *
   * @param[in] audio_encoder A <code>PP_Resource</code> identifying the audio
   * encoder.
   * @param[in] bitrate The requested new bitrate, in bits per second.
   */
  public static void RequestBitrateChange ( PPResource audio_encoder,
                                            uint bitrate)
  {
  	 _RequestBitrateChange (audio_encoder, bitrate);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_AudioEncoder_Close")]
  extern static void _Close ( PPResource audio_encoder);

  /**
   * Closes the audio encoder, and cancels any pending encodes. Any pending
   * callbacks will still run, reporting <code>PP_ERROR_ABORTED</code> . It is
   * not valid to call any encoder functions after a call to this method.
   * <strong>Note:</strong> Destroying the audio encoder closes it implicitly,
   * so you are not required to call Close().
   *
   * @param[in] audio_encoder A <code>PP_Resource</code> identifying the audio
   * encoder.
   */
  public static void Close ( PPResource audio_encoder)
  {
  	 _Close (audio_encoder);
  }


}
/**
 * @}
 */


}

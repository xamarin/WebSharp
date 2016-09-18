/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_video_encoder.idl modified Thu May 12 07:00:02 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * This file defines the <code>PPB_VideoEncoder</code> interface.
 */


/**
 * @addtogroup Interfaces
 * @{
 */
/**
 * Video encoder interface.
 *
 * Typical usage:
 * - Call Create() to create a new video encoder resource.
 * - Call GetSupportedFormats() to determine which codecs and profiles are
 *   available.
 * - Call Initialize() to initialize the encoder for a supported profile.
 * - Call GetVideoFrame() to get a blank frame and fill it in, or get a video
 *   frame from another resource, e.g. <code>PPB_MediaStreamVideoTrack</code>.
 * - Call Encode() to push the video frame to the encoder. If an external frame
 *   is pushed, wait for completion to recycle the frame.
 * - Call GetBitstreamBuffer() continuously (waiting for each previous call to
 *   complete) to pull encoded pictures from the encoder.
 * - Call RecycleBitstreamBuffer() after consuming the data in the bitstream
 *   buffer.
 * - To destroy the encoder, the plugin should release all of its references to
 *   it. Any pending callbacks will abort before the encoder is destroyed.
 *
 * Available video codecs vary by platform.
 * All: vp8 (software).
 * ChromeOS, depending on your device: h264 (hardware), vp8 (hardware)
 */
internal static partial class PPBVideoEncoder {
  [DllImport("PepperPlugin", EntryPoint = "PPB_VideoEncoder_Create")]
  extern static PPResource _Create ( PPInstance instance);

  /**
   * Creates a new video encoder resource.
   *
   * @param[in] instance A <code>PP_Instance</code> identifying the instance
   * with the video encoder.
   *
   * @return A <code>PP_Resource</code> corresponding to a video encoder if
   * successful or 0 otherwise.
   */
  public static PPResource Create ( PPInstance instance)
  {
  	return _Create (instance);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_VideoEncoder_IsVideoEncoder")]
  extern static PPBool _IsVideoEncoder ( PPResource resource);

  /**
   * Determines if the given resource is a video encoder.
   *
   * @param[in] resource A <code>PP_Resource</code> identifying a resource.
   *
   * @return <code>PP_TRUE</code> if the resource is a
   * <code>PPB_VideoEncoder</code>, <code>PP_FALSE</code> if the resource is
   * invalid or some other type.
   */
  public static PPBool IsVideoEncoder ( PPResource resource)
  {
  	return _IsVideoEncoder (resource);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_VideoEncoder_GetSupportedProfiles")]
  extern static int _GetSupportedProfiles ( PPResource video_encoder,
                                            PPArrayOutput output,
                                            PPCompletionCallback callback);

  /**
   * Gets an array of supported video encoder profiles.
   * These can be used to choose a profile before calling Initialize().
   *
   * @param[in] video_encoder A <code>PP_Resource</code> identifying the video
   * encoder.
   * @param[in] output A <code>PP_ArrayOutput</code> to receive the supported
   * <code>PP_VideoProfileDescription</code> structs.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion.
   *
   * @return If >= 0, the number of supported profiles returned, otherwise an
   * error code from <code>pp_errors.h</code>.
   */
  public static int GetSupportedProfiles ( PPResource video_encoder,
                                           PPArrayOutput output,
                                           PPCompletionCallback callback)
  {
  	return _GetSupportedProfiles (video_encoder, output, callback);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_VideoEncoder_Initialize")]
  extern static int _Initialize ( PPResource video_encoder,
                                  PPVideoFrameFormat input_format,
                                  PPSize input_visible_size,
                                  PPVideoProfile output_profile,
                                  uint initial_bitrate,
                                  PPHardwareAcceleration acceleration,
                                  PPCompletionCallback callback);

  /**
   * Initializes a video encoder resource. The plugin should call Initialize()
   * successfully before calling any of the functions below.
   *
   * @param[in] video_encoder A <code>PP_Resource</code> identifying the video
   * encoder.
   * @param[in] input_format The <code>PP_VideoFrame_Format</code> of the
   * frames which will be encoded.
   * @param[in] input_visible_size A <code>PP_Size</code> specifying the
   * dimensions of the visible part of the input frames.
   * @param[in] output_profile A <code>PP_VideoProfile</code> specifying the
   * codec profile of the encoded output stream.
   * @param[in] acceleration A <code>PP_HardwareAcceleration</code> specifying
   * whether to use a hardware accelerated or a software implementation.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   * Returns PP_ERROR_NOTSUPPORTED if video encoding is not available, or the
   * requested codec profile is not supported.
   */
  public static int Initialize ( PPResource video_encoder,
                                 PPVideoFrameFormat input_format,
                                 PPSize input_visible_size,
                                 PPVideoProfile output_profile,
                                 uint initial_bitrate,
                                 PPHardwareAcceleration acceleration,
                                 PPCompletionCallback callback)
  {
  	return _Initialize (video_encoder,
                       input_format,
                       input_visible_size,
                       output_profile,
                       initial_bitrate,
                       acceleration,
                       callback);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_VideoEncoder_GetFramesRequired")]
  extern static int _GetFramesRequired ( PPResource video_encoder);

  /**
   * Gets the number of input video frames that the encoder may hold while
   * encoding. If the plugin is providing the video frames, it should have at
   * least this many available.
   *
   * @param[in] video_encoder A <code>PP_Resource</code> identifying the video
   * encoder.
   * @return An int32_t containing the number of frames required, or an error
   * code from <code>pp_errors.h</code>.
   * Returns PP_ERROR_FAILED if Initialize() has not successfully completed.
   */
  public static int GetFramesRequired ( PPResource video_encoder)
  {
  	return _GetFramesRequired (video_encoder);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_VideoEncoder_GetFrameCodedSize")]
  extern static int _GetFrameCodedSize ( PPResource video_encoder,
                                        out PPSize coded_size);

  /**
   * Gets the coded size of the video frames required by the encoder. Coded
   * size is the logical size of the input frames, in pixels.  The encoder may
   * have hardware alignment requirements that make this different from
   * |input_visible_size|, as requested in the call to Initialize().
   *
   * @param[in] video_encoder A <code>PP_Resource</code> identifying the video
   * encoder.
   * @param[in] coded_size A <code>PP_Size</code> to hold the coded size.
   * @return An int32_t containing a result code from <code>pp_errors.h</code>.
   * Returns PP_ERROR_FAILED if Initialize() has not successfully completed.
   */
  public static int GetFrameCodedSize ( PPResource video_encoder,
                                       out PPSize coded_size)
  {
  	return _GetFrameCodedSize (video_encoder, out coded_size);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_VideoEncoder_GetVideoFrame")]
  extern static int _GetVideoFrame ( PPResource video_encoder,
                                    out PPResource video_frame,
                                     PPCompletionCallback callback);

  /**
   * Gets a blank video frame which can be filled with video data and passed
   * to the encoder.
   *
   * @param[in] video_encoder A <code>PP_Resource</code> identifying the video
   * encoder.
   * @param[out] video_frame A blank <code>PPB_VideoFrame</code> resource.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   * Returns PP_ERROR_FAILED if Initialize() has not successfully completed.
   */
  public static int GetVideoFrame ( PPResource video_encoder,
                                   out PPResource video_frame,
                                    PPCompletionCallback callback)
  {
  	return _GetVideoFrame (video_encoder, out video_frame, callback);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_VideoEncoder_Encode")]
  extern static int _Encode ( PPResource video_encoder,
                              PPResource video_frame,
                              PPBool force_keyframe,
                              PPCompletionCallback callback);

  /**
   * Encodes a video frame.
   *
   * @param[in] video_encoder A <code>PP_Resource</code> identifying the video
   * encoder.
   * @param[in] video_frame The <code>PPB_VideoFrame</code> to be encoded.
   * @param[in] force_keyframe A <code>PP_Bool> specifying whether the encoder
   * should emit a key frame for this video frame.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion. Plugins that pass <code>PPB_VideoFrame</code> resources owned
   * by other resources should wait for completion before reusing them.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   * Returns PP_ERROR_FAILED if Initialize() has not successfully completed.
   */
  public static int Encode ( PPResource video_encoder,
                             PPResource video_frame,
                             PPBool force_keyframe,
                             PPCompletionCallback callback)
  {
  	return _Encode (video_encoder, video_frame, force_keyframe, callback);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_VideoEncoder_GetBitstreamBuffer")]
  extern static int _GetBitstreamBuffer (
      PPResource video_encoder,
      out PPBitstreamBuffer bitstream_buffer,
      PPCompletionCallback callback);

  /**
   * Gets the next encoded bitstream buffer from the encoder.
   *
   * @param[in] video_encoder A <code>PP_Resource</code> identifying the video
   * encoder.
   * @param[out] bitstream_buffer A <code>PP_BitstreamBuffer</code> containing
   * encoded video data.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion. The plugin can call GetBitstreamBuffer from the callback in
   * order to continuously "pull" bitstream buffers from the encoder.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   * Returns PP_ERROR_FAILED if Initialize() has not successfully completed.
   * Returns PP_ERROR_INPROGRESS if a prior call to GetBitstreamBuffer() has
   * not completed.
   */
  public static int GetBitstreamBuffer ( PPResource video_encoder,
                                        out PPBitstreamBuffer bitstream_buffer,
                                         PPCompletionCallback callback)
  {
  	return _GetBitstreamBuffer (video_encoder, out bitstream_buffer, callback);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_VideoEncoder_RecycleBitstreamBuffer")]
  extern static void _RecycleBitstreamBuffer (
      PPResource video_encoder,
      PPBitstreamBuffer bitstream_buffer);

  /**
   * Recycles a bitstream buffer back to the encoder.
   *
   * @param[in] video_encoder A <code>PP_Resource</code> identifying the video
   * encoder.
   * @param[in] bitstream_buffer A <code>PP_BitstreamBuffer</code> that is no
   * longer needed by the plugin.
   */
  public static void RecycleBitstreamBuffer (
      PPResource video_encoder,
      PPBitstreamBuffer bitstream_buffer)
  {
  	 _RecycleBitstreamBuffer (video_encoder, bitstream_buffer);
  }


  [DllImport("PepperPlugin",
             EntryPoint = "PPB_VideoEncoder_RequestEncodingParametersChange")]
  extern static void _RequestEncodingParametersChange (
      PPResource video_encoder,
      uint bitrate,
      uint framerate);

  /**
   * Requests a change to encoding parameters. This is only a request,
   * fulfilled on a best-effort basis.
   *
   * @param[in] video_encoder A <code>PP_Resource</code> identifying the video
   * encoder.
   * @param[in] bitrate The requested new bitrate, in bits per second.
   * @param[in] framerate The requested new framerate, in frames per second.
   */
  public static void RequestEncodingParametersChange ( PPResource video_encoder,
                                                       uint bitrate,
                                                       uint framerate)
  {
  	 _RequestEncodingParametersChange (video_encoder, bitrate, framerate);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_VideoEncoder_Close")]
  extern static void _Close ( PPResource video_encoder);

  /**
   * Closes the video encoder, and cancels any pending encodes. Any pending
   * callbacks will still run, reporting <code>PP_ERROR_ABORTED</code> . It is
   * not valid to call any encoder functions after a call to this method.
   * <strong>Note:</strong> Destroying the video encoder closes it implicitly,
   * so you are not required to call Close().
   *
   * @param[in] video_encoder A <code>PP_Resource</code> identifying the video
   * encoder.
   */
  public static void Close ( PPResource video_encoder)
  {
  	 _Close (video_encoder);
  }


}
/**
 * @}
 */


}

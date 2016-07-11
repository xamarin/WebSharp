/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_video_frame.idl modified Thu May 12 07:00:02 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * Defines the <code>PPB_VideoFrame</code> interface.
 */


/**
 * @addtogroup Enums
 * @{
 */
public enum PPVideoFrameFormat {
  /**
   * Unknown format value.
   */
  Unknown = 0,
  /**
   * 12bpp YVU planar 1x1 Y, 2x2 VU samples.
   */
  Yv12 = 1,
  /**
   * 12bpp YUV planar 1x1 Y, 2x2 UV samples.
   */
  I420 = 2,
  /**
   * 32bpp BGRA.
   */
  Bgra = 3,
  /**
   * The last format.
   */
  Last = Bgra
}
/**
 * @}
 */

/**
 * @addtogroup Interfaces
 * @{
 */
public static partial class PPBVideoFrame {
  [DllImport("PepperPlugin", EntryPoint = "PPB_VideoFrame_IsVideoFrame")]
  extern static PPBool _IsVideoFrame ( PPResource resource);

  /**
   * Determines if a resource is a VideoFrame resource.
   *
   * @param[in] resource The <code>PP_Resource</code> to test.
   *
   * @return A <code>PP_Bool</code> with <code>PP_TRUE</code> if the given
   * resource is a VideoFrame resource or <code>PP_FALSE</code> otherwise.
   */
  public static PPBool IsVideoFrame ( PPResource resource)
  {
  	return _IsVideoFrame (resource);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_VideoFrame_GetTimestamp")]
  extern static double _GetTimestamp ( PPResource frame);

  /**
   * Gets the timestamp of the video frame.
   *
   * @param[in] frame A <code>PP_Resource</code> corresponding to a video frame
   * resource.
   *
   * @return A <code>PP_TimeDelta</code> containing the timestamp of the video
   * frame. Given in seconds since the start of the containing video stream.
   */
  public static double GetTimestamp ( PPResource frame)
  {
  	return _GetTimestamp (frame);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_VideoFrame_SetTimestamp")]
  extern static void _SetTimestamp ( PPResource frame,  double timestamp);

  /**
   * Sets the timestamp of the video frame. Given in seconds since the
   * start of the containing video stream.
   *
   * @param[in] frame A <code>PP_Resource</code> corresponding to a video frame
   * resource.
   * @param[in] timestamp A <code>PP_TimeDelta</code> containing the timestamp
   * of the video frame. Given in seconds since the start of the containing
   * video stream.
   */
  public static void SetTimestamp ( PPResource frame,  double timestamp)
  {
  	 _SetTimestamp (frame, timestamp);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_VideoFrame_GetFormat")]
  extern static PPVideoFrameFormat _GetFormat ( PPResource frame);

  /**
   * Gets the format of the video frame.
   *
   * @param[in] frame A <code>PP_Resource</code> corresponding to a video frame
   * resource.
   *
   * @return A <code>PP_VideoFrame_Format</code> containing the format of the
   * video frame.
   */
  public static PPVideoFrameFormat GetFormat ( PPResource frame)
  {
  	return _GetFormat (frame);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_VideoFrame_GetSize")]
  extern static PPBool _GetSize ( PPResource frame, out PPSize size);

  /**
   * Gets the size of the video frame.
   *
   * @param[in] frame A <code>PP_Resource</code> corresponding to a video frame
   * resource.
   * @param[out] size A <code>PP_Size</code>.
   *
   * @return A <code>PP_Bool</code> with <code>PP_TRUE</code> on success or
   * <code>PP_FALSE</code> on failure.
   */
  public static PPBool GetSize ( PPResource frame, out PPSize size)
  {
  	return _GetSize (frame, out size);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_VideoFrame_GetDataBuffer")]
  extern static IntPtr _GetDataBuffer ( PPResource frame);

  /**
   * Gets the data buffer for video frame pixels.
   *
   * @param[in] frame A <code>PP_Resource</code> corresponding to a video frame
   * resource.
   *
   * @return A pointer to the beginning of the data buffer.
   */
  public static IntPtr GetDataBuffer ( PPResource frame)
  {
  	return _GetDataBuffer (frame);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_VideoFrame_GetDataBufferSize")]
  extern static uint _GetDataBufferSize ( PPResource frame);

  /**
   * Gets the size of data buffer.
   *
   * @param[in] frame A <code>PP_Resource</code> corresponding to a video frame
   * resource.
   *
   * @return The size of the data buffer.
   */
  public static uint GetDataBufferSize ( PPResource frame)
  {
  	return _GetDataBufferSize (frame);
  }


}
/**
 * @}
 */


}

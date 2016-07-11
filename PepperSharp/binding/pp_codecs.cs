/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From pp_codecs.idl modified Thu May 12 07:00:00 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {

/**
 * @file
 * Video profiles.
 */


/**
 * @addtogroup Enums
 * @{
 */
public enum PPVideoProfile {
  H264baseline = 0,
  H264main = 1,
  H264extended = 2,
  H264high = 3,
  H264high10profile = 4,
  H264high422profile = 5,
  H264high444predictiveprofile = 6,
  H264scalablebaseline = 7,
  H264scalablehigh = 8,
  H264stereohigh = 9,
  H264multiviewhigh = 10,
  Vp8Any = 11,
  Vp9Any = 12,
  Max = Vp9Any
}

/**
 * Audio profiles.
 */
public enum PPAudioProfile {
  Opus = 0,
  Max = Opus
}

/**
 * Hardware acceleration options.
 */
public enum PPHardwareAcceleration {
  /** Create a hardware accelerated resource only. */
  Only = 0,
  /**
   * Create a hardware accelerated resource if possible. Otherwise, fall back
   * to the software implementation.
   */
  Withfallback = 1,
  /** Create the software implementation only. */
  None = 2,
  Last = None
}
/**
 * @}
 */

/**
 * @addtogroup Structs
 * @{
 */
/**
 * Struct describing a decoded video picture. The decoded picture data is stored
 * in the GL texture corresponding to |texture_id|. The plugin can determine
 * which Decode call generated the picture using |decode_id|.
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPVideoPicture {
  /**
   * |decode_id| parameter of the Decode call which generated this picture.
   * See the PPB_VideoDecoder function Decode() for more details.
   */
  public uint decode_id;
  /**
   * Texture ID in the plugin's GL context. The plugin can use this to render
   * the decoded picture.
   */
  public uint texture_id;
  /**
   * The GL texture target for the decoded picture. Possible values are:
   *   GL_TEXTURE_2D
   *   GL_TEXTURE_RECTANGLE_ARB
   *   GL_TEXTURE_EXTERNAL_OES
   *
   * The pixel format of the texture is GL_RGBA.
   */
  public uint texture_target;
  /**
   * Dimensions of the texture holding the decoded picture.
   */
  public PPSize texture_size;
  /**
   * The visible subrectangle of the picture. The plugin should display only
   * this part of the picture.
   */
  public PPRect visible_rect;
};

/**
 * Struct describing a decoded video picture. The decoded picture data is stored
 * in the GL texture corresponding to |texture_id|. The plugin can determine
 * which Decode call generated the picture using |decode_id|.
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPVideoPicture01 {
  /**
   * |decode_id| parameter of the Decode call which generated this picture.
   * See the PPB_VideoDecoder function Decode() for more details.
   */
  public uint decode_id;
  /**
   * Texture ID in the plugin's GL context. The plugin can use this to render
   * the decoded picture.
   */
  public uint texture_id;
  /**
   * The GL texture target for the decoded picture. Possible values are:
   *   GL_TEXTURE_2D
   *   GL_TEXTURE_RECTANGLE_ARB
   *   GL_TEXTURE_EXTERNAL_OES
   *
   * The pixel format of the texture is GL_RGBA.
   */
  public uint texture_target;
  /**
   * Dimensions of the texture holding the decoded picture.
   */
  public PPSize texture_size;
};

/**
 * Supported video profile information. See the PPB_VideoEncoder function
 * GetSupportedProfiles() for more details.
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPVideoProfileDescription {
  /**
   * The codec profile.
   */
  public PPVideoProfile profile;
  /**
   * Dimensions of the maximum resolution of video frames, in pixels.
   */
  public PPSize max_resolution;
  /**
   * The numerator of the maximum frame rate.
   */
  public uint max_framerate_numerator;
  /**
   * The denominator of the maximum frame rate.
   */
  public uint max_framerate_denominator;
  /**
   * Whether the profile is hardware accelerated.
   */
  public PPBool hardware_accelerated;
};

/**
 * Supported video profile information. See the PPB_VideoEncoder function
 * GetSupportedProfiles() for more details.
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPVideoProfileDescription01 {
  /**
   * The codec profile.
   */
  public PPVideoProfile profile;
  /**
   * Dimensions of the maximum resolution of video frames, in pixels.
   */
  public PPSize max_resolution;
  /**
   * The numerator of the maximum frame rate.
   */
  public uint max_framerate_numerator;
  /**
   * The denominator of the maximum frame rate.
   */
  public uint max_framerate_denominator;
  /**
   * A value indicating if the profile is available in hardware, software, or
   * both.
   */
  public PPHardwareAcceleration acceleration;
};

/**
 * Supported audio profile information. See the PPB_AudioEncoder function
 * GetSupportedProfiles() for more details.
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPAudioProfileDescription {
  /**
   * The codec profile.
   */
  public PPAudioProfile profile;
  /**
   * Maximum number of channels that can be encoded.
   */
  public uint max_channels;
  /**
   * Sample size.
   */
  public uint sample_size;
  /**
   * Sampling rate that can be encoded
   */
  public uint sample_rate;
  /**
   * Whether the profile is hardware accelerated.
   */
  public PPBool hardware_accelerated;
};

/**
 * Struct describing a bitstream buffer.
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPBitstreamBuffer {
  /**
   * The size, in bytes, of the bitstream data.
   */
  public uint size;
  /**
   * The base address of the bitstream data.
   */
  public IntPtr buffer;
  /**
   * Whether the buffer represents a key frame.
   */
  public PPBool key_frame;
};

/**
 * Struct describing an audio bitstream buffer.
 */
[StructLayout(LayoutKind.Sequential)]
public partial struct PPAudioBitstreamBuffer {
  /**
   * The size, in bytes, of the bitstream data.
   */
  public uint size;
  /**
   * The base address of the bitstream data.
   */
  public IntPtr buffer;
};
/**
 * @}
 */


}

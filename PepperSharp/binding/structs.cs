using System;
using System.Runtime.InteropServices;

namespace Pepper
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PPGamepadSampleData
    {
        public uint axes_length;
        public float[] axes; // float[16];
        public uint buttons_length;
        public float[] buttons; // float[32];
        public double timestamp;
        public ushort[] id; // ushort[128];
        public PPBool connected;
        public char[] unused_pad_; // char[4];
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPGamepadsSampleData
    {
        public uint length;
        public char[] unused_pad_; // char[4];
        public PPGamepadSampleData[] items; // PPGamepadSampleData[4];
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPHostResolver_Hint
    {
        public PPNetAddress_Family family;
        public int flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPImageDataDesc
    {
        public PPImageDataFormat format;
        public PPSize size;
        public int stride;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPNetAddress_IPv4
    {
        public ushort port;
        public byte[] addr; // byte[4];
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPNetAddress_IPv6
    {
        public ushort port;
        public byte[] addr; // byte[16];
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPArrayOutput
    {
        public PPArrayOutput_GetDataBuffer GetDataBuffer;
        public IntPtr user_data;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPVideoPicture
    {
        public uint decode_id;
        public uint texture_id;
        public uint texture_target;
        public PPSize texture_size;
        public PPRect visible_rect;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPVideoPicture_0_1
    {
        public uint decode_id;
        public uint texture_id;
        public uint texture_target;
        public PPSize texture_size;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPVideoProfileDescription
    {
        public PPVideoProfile profile;
        public PPSize max_resolution;
        public uint max_framerate_numerator;
        public uint max_framerate_denominator;
        public PPBool hardware_accelerated;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPVideoProfileDescription_0_1
    {
        public PPVideoProfile profile;
        public PPSize max_resolution;
        public uint max_framerate_numerator;
        public uint max_framerate_denominator;
        public PPHardwareAcceleration acceleration;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPAudioProfileDescription
    {
        public PPAudioProfile profile;
        public uint max_channels;
        public uint sample_size;
        public uint sample_rate;
        public PPBool hardware_accelerated;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPBitstreamBuffer
    {
        public uint size;
        public IntPtr buffer;
        public PPBool key_frame;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPAudioBitstreamBuffer
    {
        public uint size;
        public IntPtr buffer;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPCompletionCallback
    {
        public PPCompletionCallback_Func func;
        public IntPtr user_data;
        public int flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPDirectoryEntry
    {
        public PPResource file_ref;
        public PPFileType file_type;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPFileInfo
    {
        public long size;
        public PPFileType type;
        public PPFileSystemType system_type;
        //public PPTime creation_time;
        //public PPTime last_access_time;
        //public PPTime last_modified_time;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPInputEvent_Key
    {
        public uint modifier;
        public uint key_code;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPInputEvent_Character
    {
        public uint modifier;
        public char[] text; // char[5];
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPInputEvent_Mouse
    {
        public uint modifier;
        public PPInputEvent_MouseButton button;
        public float x;
        public float y;
        public int click_count;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPInputEvent_Wheel
    {
        public uint modifier;
        public float delta_x;
        public float delta_y;
        public float wheel_ticks_x;
        public float wheel_ticks_y;
        public PPBool scroll_by_page;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPPoint
    {
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPFloatPoint
    {
        public float x;
        public float y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPRect
    {
        public PPPoint point;
        public PPSize size;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPFloatRect
    {
        public PPFloatPoint point;
        public PPFloatSize size;
    }

    [StructLayout(LayoutKind.Sequential)]
    public partial struct PPSize
    {
        public int width;
        public int height;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPFloatSize
    {
        public float width;
        public float height;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPTouchPoint
    {
        public uint id;
        public PPFloatPoint position;
        public PPFloatPoint radius;
        public float rotation_angle;
        public float pressure;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPVarValue
    {
        public PPBool as_bool;
        public int as_int;
        public double as_double;
        public long as_id;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PPVar
    {
        public PPVarType type;
        public int padding;
        public PPVarValue value;
    }


}

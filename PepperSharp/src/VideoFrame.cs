using System;
using System.Runtime.InteropServices;

namespace PepperSharp
{
    public class VideoFrame : Resource
    {
        public VideoFrame() : base(PPResource.Empty)
        { }

        public VideoFrame(VideoFrame other) : base(other)
        { }

        internal VideoFrame(PPResource resource) : base(resource)
        { }

        public VideoFrame(PassRef pass, PPResource resource)
            : base(pass, resource)
        {
        }

        /// <summary>
        /// Gets or Sets the timestamp of the video frame.  Given in seconds since the start of the 
        /// containing video stream.
        /// </summary>
        public DateTime TimeStamp
        {
            get
            { return PepperSharpUtils.ConvertFromPepperTimestamp(PPBVideoFrame.GetTimestamp(this));  }

            set
            {
                PPBVideoFrame.SetTimestamp(this, PepperSharpUtils.ConvertToPepperTimestamp(value));
            }
        }

        /// <summary>
        /// Gets the format of the video frame.
        /// </summary>
        public VideoFrameFormat Format
            => (VideoFrameFormat)PPBVideoFrame.GetFormat(this);

        /// <summary>
        /// Gets the size of the video frame.
        /// </summary>
        /// <param name="size">A size</param>
        /// <returns>True on success or false on failure.</returns>
        public bool GetSize(out PPSize size)
            => PPBVideoFrame.GetSize(this, out size) == PPBool.True;


        /// <summary>
        /// Gets and Sets the byte[] data buffer for video frame pixels.
        /// </summary>
        public byte[] DataBuffer
        {
            get
            {
                var size = DataBufferSize;
                if (size > 0 )
                {
                    var bufferMap = new byte[size];
                    Marshal.Copy(PPBVideoFrame.GetDataBuffer(this), bufferMap, 0, bufferMap.Length);
                    return bufferMap;

                }
                else
                    return new byte[0];
            }

            set
            {
                var size = DataBufferSize;
                if (size > 0)
                {
                    var length = Math.Min(size, value.Length);
                    Marshal.Copy(value, 0, PPBVideoFrame.GetDataBuffer(this), (int)length);
                }

            }
        }

        /// <summary>
        /// Gets the size of data buffer in bytes.
        /// </summary>
        public uint DataBufferSize
            => PPBVideoFrame.GetDataBufferSize(this);
  

    }

    public enum VideoFrameFormat
    {
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

}

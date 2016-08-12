using System;

namespace PepperSharp
{
    public class ImageData : Resource
    {

        PPImageDataDesc imageDataDesc = new PPImageDataDesc();

        /// <summary>
        /// A constructor used when you have received a <code>PP_Resource</code> as a
        /// return value that has already been reference counted.
        ///
        /// </summary>
        /// <param name="passRef">Marker to pass reference</param>
        /// <param name="resource">A PPResource corresponding to image data.</param>
        public ImageData(PassRef passRef, PPResource resource)
        {

        }

        /// <summary>
        /// The copy constructor for <code>ImageData</code>. This constructor
        /// produces an <code>ImageData</code> object that shares the underlying
        /// <code>Image</code> resource with <code>other</code>.
        /// </summary>
        /// <param name="other">The other image data</param>
        public ImageData(ImageData other)
        {

        }

        /// <summary>
        /// A constructor that allocates a new <code>ImageData</code> in the browser
        /// with the provided parameters. The resulting object will be IsEmpty if
        /// the allocation failed.
        /// </summary>
        /// <param name="instance">The instance with which this resource will be
        /// associated.
        /// </param>
        /// <param name="format">A PP_ImageDataFormat containing desired image format.
        /// PP_ImageDataFormat is an enumeration of the different types of
        /// image data formats.
        /// </param>
        /// <param name="size">the image size.</param>
        /// <param name="init_to_zero">A bool used to determine transparency at
        /// creation. Set the <code>init_to_zero</code> flag if you want the bitmap
        /// initialized to transparent during the creation process. If this flag is
        /// not set, the current contents of the bitmap will be undefined, and the
        /// module should be sure to set all the pixels.
        /// </param>
        public ImageData(Instance instance,
            PPImageDataFormat format,
            PPSize size,
            bool init_to_zero)
        {
            handle = PPBImageData.Create(instance, format, size, init_to_zero ? PPBool.True : PPBool.False);
            if (PPBImageData.IsImageData(handle) == PPBool.True)
            {
                InitData();
            }
        }

        IntPtr imageDataPtr;

        void InitData()
        {
            if (PPBImageData.Describe(this, out imageDataDesc) == PPBool.True)
            {
                imageDataPtr = PPBImageData.Map(this);
            }
        }

        /// <summary>
        ///  Returns a raw pointer to the image pixels.
        /// </summary>
        public IntPtr Data
        {
            get { return imageDataPtr;  }
        }

        /// <summary>
        /// This function is used retrieve the address of the given pixel for 32-bit
        /// pixel formats.
        /// </summary>
        /// <param name="coord">A <code>PPPoint</code> representing the x and y
        /// coordinates for a specific pixel.
        /// </param>
        /// <returns>The IntPtr for the pixel.</returns>
        public IntPtr GetAddr32(PPPoint coord)
        {
            // If we add more image format types that aren't 32-bit, we'd want to check
            // here and fail.
            return imageDataPtr + coord.Y * Stride + coord.X * 4;
        }

    /// <summary>
    /// IsImageDataFormatSupported() returns <code>true</code> if the supplied
    /// format is supported by the browser. Note:
    /// <code>BGRA_PREMUL</code> and
    /// <code>RGBA_PREMUL</code> formats are always supported.
    /// Other image formats do not make this guarantee, and should be checked
    /// first with IsImageDataFormatSupported() before using.
    /// </summary>
    /// <param name="format">Image data format.</param>
    /// <returns></returns>
    public static bool IsImageDataFormatSupported(PPImageDataFormat format)
        {
            return PPBImageData.IsImageDataFormatSupported(format) == PPBool.True ? true : false;
        }

        /// <summary>
        /// NativeImageDataFormat determines the browser's preferred format for
        /// images. Using this format guarantees no extra conversions will occur when
        /// painting.
        /// </summary>
        public static PPImageDataFormat NativeImageDataFormat
        {
            get
            {
                return PPBImageData.GetNativeImageDataFormat();
            }
        }

        /// <summary>
        /// Gets the current format for images.
        /// </summary>
        public PPImageDataFormat Format
        {
            get
            {
                return imageDataDesc.format;
            }
        }

        /// <summary>
        /// Gets the current image size.
        /// </summary>
        public PPSize Size
        {
            get
            {
                return imageDataDesc.size;
            }
        }

        /// <summary>
        /// Gets the row width in bytes.
        /// </summary>
        public int Stride
        {
            get
            {
                return imageDataDesc.stride;
            }
        }
    }
}

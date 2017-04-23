using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using WebSharpJs.NodeJS;
using WebSharpJs.Script;

namespace WebSharpJs.Electron
{
    public class NativeImage : NodeJsObject
    {
        protected override string ScriptProxy =>
                        @"function () { 

                            if (options && options.length > 0)
                            {
                                if (options[0] === 'createEmpty')
                                    return nativeImage.createEmpty();
                                else if (options[0] === 'createFromPath')
                                    return nativeImage.createFromPath(options[1]);
                                else if (options[0] === 'createFromBuffer')
                                    return nativeImage.createFromBuffer(options[1]);
                                else if (options[0] === 'createFromDataURL')
                                    return nativeImage.createFromDataURL(options[1]);
                            }
                            else
                                return nativeImage.createEmpty(); 
                        }()";

        protected override string Requires => @"const {nativeImage} = require('electron')";

        // Save off the ScriptObjectProxy implementation to cut down on bridge calls.
        static NodeObjectProxy scriptProxy;

        public static async Task<NativeImage> CreateEmpty()
        {
            var proxy = new NativeImage();
            if (scriptProxy != null)
                proxy.ScriptObjectProxy = scriptProxy;

            scriptProxy = await proxy.Initialize("createEmpty");
            return proxy;

        }

        public static async Task<NativeImage> CreateFromPath(string path)
        {
            var proxy = new NativeImage();
            if (scriptProxy != null)
                proxy.ScriptObjectProxy = scriptProxy;

            scriptProxy = await proxy.Initialize(new object[] { "createFromPath", path });
            return proxy;

        }

        public static async Task<NativeImage> CreateFromBuffer(byte[] buffer)
        {
            var proxy = new NativeImage();
            if (scriptProxy != null)
                proxy.ScriptObjectProxy = scriptProxy;

            scriptProxy = await proxy.Initialize("createFromBuffer", buffer);
            return proxy;

        }

        public static async Task<NativeImage> CreateFromDataURL(string dataURL)
        {
            var proxy = new NativeImage();
            if (scriptProxy != null)
                proxy.ScriptObjectProxy = scriptProxy;

            scriptProxy = await proxy.Initialize("createFromDataURL", dataURL);
            return proxy;

        }

        private NativeImage() : base() { }
        private NativeImage(object obj) : base(obj) { }

        private NativeImage(ScriptObjectProxy scriptObject) : base(scriptObject)
        { }

        public static explicit operator NativeImage(ScriptObjectProxy sop)
        {
            return new NativeImage(sop);
        }


        public async Task<bool> IsEmpty()
        {
            return await Invoke<bool>("isEmpty");
        }

        public async Task<bool> IsTemplateImage()
        {
            return await Invoke<bool>("isTemplateImage");
        }

        public async Task SetTemplateImage(bool template)
        {
            await Invoke<object>("setTemplateImage", template);
        }

        public async Task<string> ToDataURL()
        {
            return await Invoke<string>("toDataURL");
        }

        public async Task<Size> GetSize()
        {
            return new Size(await Invoke<object>("getSize"));
        }

        public async Task<byte[]> ToPNG()
        {
            return await Invoke<byte[]>("toPNG");
        }

        public async Task<byte[]> ToJPEG(int quality)
        {
            return await Invoke<byte[]>("toJPEG", quality);
        }

        public async Task<byte[]> GetBitmap()
        {
            return await Invoke<byte[]>("getBitmap");
        }

        public async Task<byte[]> GetNativeHandle()
        {
            return await Invoke<byte[]>("getNativeHandle");
        }

        public async Task<NativeImage> Crop(Rectangle rect)
        {
            return await Invoke<NativeImage>("crop", rect);
        }

        public async Task<NativeImage> Resize(int width = int.MinValue, int height = int.MinValue, string quality = null)
        {
            var options = new Dictionary<string, object>();
            if (width != int.MinValue || height != int.MinValue || !string.IsNullOrEmpty(quality))
            {
                if (width != int.MinValue)
                    options["width"] = width;
                if (height != int.MinValue)
                    options["height"] = height;
                if (!string.IsNullOrEmpty(quality))
                    options["quality"] = quality;
            }

            return await Invoke<NativeImage>("resize", options);
        }

        public async Task<float> GetAspectRatio()
        {
            return await Invoke<float>("getAspectRatio");
        }

    }
}

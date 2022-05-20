using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace GameCore.Graphics
{
    internal class Texture
    {
        public int TextureHandle;
        private Image<Rgba32> _image;
        
        public Texture(string path, TextureUnit textureUnit)
        {
            TextureHandle = GL.GenTexture();
            Use(textureUnit);
            SetUpTexParameters();
            ConfigureImage(path);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, _image.Width, _image.Height, 0,
                PixelFormat.Rgba, PixelType.UnsignedByte, GetPixels().ToArray());
        }

        public void Use(TextureUnit textureUnit)
        {
            GL.ActiveTexture(textureUnit);
            GL.BindTexture(TextureTarget.Texture2D, TextureHandle);
        }

        private void SetUpTexParameters()
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        }

        private List<byte> GetPixels()
        {
            if (_image == null) throw new ArgumentNullException("Image Was Not Configured");

            List<byte> pixels = new List<byte>(4 * _image.Width * _image.Height);
            _image.ProcessPixelRows(process =>
            {
                for (int y = 0; y < _image.Height; y++)
                {
                    var pixelColor = process.GetRowSpan(y);
                    for (int x = 0; x < _image.Width; x++)
                    {
                        pixels.Add(pixelColor[x].R);
                        pixels.Add(pixelColor[x].G);
                        pixels.Add(pixelColor[x].B);
                        pixels.Add(pixelColor[x].A);
                    }
                }
            });

            return pixels;
        }

        private void ConfigureImage(string path)
        {
            _image = Image.Load<Rgba32>(path);
            _image.Mutate(x => x.Flip(FlipMode.Vertical));
        }
    }
}



















//Use();
//Image<Rgba32> image = Image.Load<Rgba32>(path);
//image.Mutate(x => x.Flip(FlipMode.Vertical));
// 4 for rgba
//List<byte> pixels = new List<byte>(4 * image.Height * image.Width);



//image.ProcessPixelRows(action =>
//{
//    for(int y = 0; y < image.Height; y++)
//    {
//        var pixelColor = action.GetRowSpan(y);
//        for(int x = 0; x < image.Width; x++)
//        {
//            //pixels.Add(pixelColor[x].R);
//            //pixels.Add(pixelColor[x].G);
//            //pixels.Add(pixelColor[x].B);
//            pixels.Add(5);
//            pixels.Add(7);
//            pixels.Add(12);
//            pixels.Add(1);
//        }
//    }
//});
//GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, 
//    PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
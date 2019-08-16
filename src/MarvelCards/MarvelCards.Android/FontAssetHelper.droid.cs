using System.IO;
using MarvelCards.Droid;
using SkiaSharp;
using Xamarin.Forms;

[assembly: Dependency(typeof(FontAssetHelper))]
namespace MarvelCards.Droid
{
    class FontAssetHelper : IFontAssetHelper
    {
        public SKTypeface GetSkiaTypefaceFromAssetFont(string fontName)
        {
            SKTypeface typeFace;

            using (var asset = Android.App.Application.Context.Assets.Open(fontName))
            {
                var fontStream = new MemoryStream();
                asset.CopyTo(fontStream);
                fontStream.Flush();
                fontStream.Position = 0;
                typeFace = SKTypeface.FromStream(fontStream);
            }

            return typeFace;
        }
    }
}
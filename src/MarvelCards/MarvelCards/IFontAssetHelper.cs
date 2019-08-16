using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarvelCards
{
    public interface IFontAssetHelper
    {
        SKTypeface GetSkiaTypefaceFromAssetFont(string fontName);
    }
}

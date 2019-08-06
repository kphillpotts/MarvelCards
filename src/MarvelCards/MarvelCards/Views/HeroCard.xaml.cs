using MarvelCards.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MarvelCards.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeroCard : ContentView
    {
        private Hero _viewModel;
        private readonly float _density; 
        private readonly float _cardTopMargin; 
        private readonly float _cornerRadius = 60f;

        // cached skia color and paint object
        SKColor _heroColor;
        SKPaint _heroPaint;

        public HeroCard()
        {
            InitializeComponent();

            _density = (float)Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density;
            _cardTopMargin = 200f * _density;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (this.BindingContext == null) return;
            _viewModel = this.BindingContext as Hero;

            // because we can't bind skia drawing using the binding engine
            // we cache the paint objects when the bound character changes
            _heroColor = Color.FromHex(_viewModel.HeroColor).ToSKColor();
            _heroPaint = new SKPaint() { Color = _heroColor };

            // repaint the surface with the new colors
            CardBackground.InvalidateSurface();
        }

        public Image MainImage => HeroImage;

        private void CardBackground_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
        {
            // seems that PaintSurface is called before the 
            // binding context is set sometimes
            if (_viewModel == null) return;

            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            // draw the card background
            canvas.DrawRoundRect(
                rect: new SKRect(0, _cardTopMargin, info.Width, info.Height),
                r: new SKSize(_cornerRadius, _cornerRadius),
                paint: _heroPaint);

        }
    }
}
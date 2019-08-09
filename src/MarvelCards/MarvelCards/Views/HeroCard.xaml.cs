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
        private float _cornerRadius = 60f;
        private CardState _cardState = CardState.Collapsed;
        private float _gradientHeight = 200f;


        // cached skia color and paint object
        SKColor _heroColor;
        SKPaint _heroPaint;
        private double _cardTopAnimPosition;
        private float _gradientTransitionY;

        public HeroCard()
        {
            InitializeComponent();

            _density = (float)Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density;
            _cardTopMargin = 200f * _density;
            _cornerRadius = 30f * _density;
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
            _gradientTransitionY = float.MaxValue;

            // setup initial values
            _cardTopAnimPosition = _cardTopMargin;

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

            // draw top hero color
            canvas.DrawRoundRect(
                rect: new SKRect(0, (float)_cardTopAnimPosition, info.Width, info.Height),
                r: new SKSize(_cornerRadius, _cornerRadius),
                paint: _heroPaint);

            // work out the gradient needs to be
            var gradientRect = new SKRect(0, _gradientTransitionY, info.Width,
                _gradientTransitionY + _gradientHeight);
            // create the gradient
            var gradientPaint = new SKPaint() { Style = SKPaintStyle.Fill };
            gradientPaint.Shader = SKShader.CreateLinearGradient
                (
                    start: new SKPoint(0, _gradientTransitionY),
                    end: new SKPoint(0, _gradientTransitionY + _gradientHeight),
                    colors: new SKColor[] { _heroColor, SKColors.White },
                    colorPos: new float[] { 0, 1 },
                    SKShaderTileMode.Clamp
                );
            // draw the gradient
            canvas.DrawRect(gradientRect, gradientPaint);

            // draw the white bit
            SKRect bottomRect = new SKRect(0, _gradientTransitionY + _gradientHeight,
                info.Width, info.Height);
            canvas.DrawRect(bottomRect, new SKPaint() { Color = SKColors.White });
        }

        private void LearnMoreTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            // go to a state of exanded
            GoToState(CardState.Expanded);
        }

        public  void GoToState(CardState cardState)
        {
            // chec we are actually changing state
            if (_cardState == cardState)
                return;

            MessagingCenter.Send<CardEvent>(new CardEvent(), cardState.ToString());
            AnimateTransition(cardState);
            _cardState = cardState;

        }

        private void AnimateTransition(CardState cardState)
        {
            var parentAnimation = new Animation();

            if (cardState == CardState.Expanded)
            {
                parentAnimation.Add(0.00, 0.10, CreateCardAnimation(cardState));
                parentAnimation.Add(0.00, 0.25, CreateLearnMoreAnimation(cardState));
                parentAnimation.Add(0.00, 0.50, CreateImageAnimation(cardState));
                parentAnimation.Add(0.10, 0.50, CreateHeroNameAnimation(cardState));
                parentAnimation.Add(0.15, 0.50, CreateRealNameAnimation(cardState));
                parentAnimation.Add(0.50, 0.75, CreateGradientAnimation(cardState));
            }
            else
            {
                parentAnimation.Add(0.00, 0.25, CreateGradientAnimation(cardState));
                parentAnimation.Add(0.25, 0.45, CreateImageAnimation(cardState));
                parentAnimation.Add(0.35, 0.45, CreateLearnMoreAnimation(cardState));
                parentAnimation.Add(0.25, 0.45, CreateCardAnimation(cardState));
                parentAnimation.Add(0.30, 0.50, CreateHeroNameAnimation(cardState));
                parentAnimation.Add(0.25, 0.50, CreateRealNameAnimation(cardState));
                
            }

            parentAnimation.Commit(this, "CardExpand", 16, 2000);
        }

        private Animation CreateGradientAnimation(CardState cardState)
        {
            double start;
            double end;
            if (cardState == CardState.Expanded)
            {
                _gradientTransitionY = CardBackground.CanvasSize.Height;
                start = _gradientTransitionY;
                end = -_gradientHeight;
            }
            else
            {
                _gradientTransitionY = -_gradientHeight;
                start = _gradientTransitionY;
                end = CardBackground.CanvasSize.Height;
            }

            var gradientAnimation = new Animation(
                callback: v =>
                {
                    _gradientTransitionY = (float)v;
                    CardBackground.InvalidateSurface();
                },
                start: start,
                end: end,
                easing: Easing.Linear,
                finished: () =>
                {
                    Color fontColor = cardState == CardState.Expanded ? Color.Black : Color.White;
                    HeroNameLabel.TextColor = fontColor;
                    RealNameLabel.TextColor = fontColor;
                }
                );

            return gradientAnimation;
        }

        private Animation CreateLearnMoreAnimation(CardState cardState)
        {
            // work out where the top of the card should be
            var learnMoreAnimationStart = cardState == CardState.Expanded ? 0 : 100;
            var learnMoreAnimationEnd = cardState == CardState.Expanded ? 100 : 0;

            var learnMoreAnim = new Animation(
                v =>
                {
                    LearnMoreLabel.TranslationX = v;
                    LearnMoreLabel.Opacity = 1 - (v / 100);
                },
                learnMoreAnimationStart,
                learnMoreAnimationEnd,
                Easing.SinInOut
                );
            return learnMoreAnim;

        }

        private Animation CreateRealNameAnimation(CardState cardState)
        {
            // work out where the top of the card should be
            var nameAnimationStart = cardState == CardState.Expanded ? 0 : -50;
            var nameAnimationEnd = cardState == CardState.Expanded ? -50 : 0;

            var imageAnim = new Animation(
                v =>
                {
                    RealNameLabel.TranslationY = v;
                },
                nameAnimationStart,
                nameAnimationEnd,
                Easing.SpringOut
                );
            return imageAnim;

        }

        private Animation CreateHeroNameAnimation(CardState cardState)
        {
            // work out where the top of the card should be
            var nameAnimationStart = cardState == CardState.Expanded ? 0 : -50;
            var nameAnimationEnd = cardState == CardState.Expanded ? -50 : 0;

            var imageAnim = new Animation(
                v =>
                {
                    HeroNameLabel.TranslationY = v;
                },
                nameAnimationStart,
                nameAnimationEnd,
                Easing.SpringOut
                );
            return imageAnim;

        }

        private Animation CreateImageAnimation(CardState cardState)
        {
            // work out where the top of the card should be
            var imageAnimationStart = cardState == CardState.Expanded ? 50 : 0;
            var imageAnimationEnd = cardState == CardState.Expanded ? 0 : 50;

            var imageAnim = new Animation(
                v =>
                {
                    HeroImage.TranslationY = v;
                },
                imageAnimationStart,
                imageAnimationEnd,
                Easing.SpringOut
                );
            return imageAnim;

        }

        private Animation CreateCardAnimation(CardState cardState)
        {
            // work out where the top of the card should be
            var cardAnimStart = cardState == CardState.Expanded ? _cardTopMargin : -_cornerRadius;
            var cardAnimEnd = cardState == CardState.Expanded ? -_cornerRadius : _cardTopMargin;

            var cardAnim = new Animation(
                v =>
                {

                    _cardTopAnimPosition = v;
                    CardBackground.InvalidateSurface();
                },
                cardAnimStart,
                cardAnimEnd,
                Easing.SinInOut
                );
            return cardAnim;
        }
    }
}
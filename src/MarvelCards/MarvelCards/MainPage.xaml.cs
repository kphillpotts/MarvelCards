using MarvelCards.ViewModels;
using MarvelCards.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MarvelCards
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = new HeroCardsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MainCardView.UserInteracted += MainCardView_UserInteracted;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MainCardView.UserInteracted -= MainCardView_UserInteracted;
        }


        private void MainCardView_UserInteracted(PanCardView.CardsView view, 
            PanCardView.EventArgs.UserInteractedEventArgs args)
        {
            var card = MainCardView.CurrentView as HeroCard;
            

            if (args.Status == PanCardView.Enums.UserInteractionStatus.Running)
            {
                // work out what percent the swipe is at
                var percentFromCenter = Math.Abs(args.Diff / this.Width);
                Debug.WriteLine($"Percent {percentFromCenter}");

                var opacity = (1 - (percentFromCenter)) * 1.5;
                if (opacity > 1) opacity = 1;
                    
                MainCardView.CurrentView.Opacity = opacity;

                // do the scaling on the main card during swipe
                var scale = (1 - (percentFromCenter) * 1.5);
                if (scale > 1) scale = 1;
                card.MainImage.Scale = scale;

                var imageBaseMargin = -150;
                var movementFactor = 100;

                var translation = imageBaseMargin + (movementFactor * percentFromCenter);
                card.MainImage.TranslationY = translation;

                // adjust opacity of image
                card.MainImage.Opacity = opacity;
                var nextCard = MainCardView.CurrentBackViews.First() as HeroCard;
                
                // adjust opacity of the back image
                nextCard.MainImage.Opacity = LimitToRange(percentFromCenter * 1.5, 0, 1);
                nextCard.MainImage.Scale = LimitToRange(percentFromCenter * 1.5, 0, 1);

                // percent => 0, 0
                // percent => .5 => -75
                // percent => 1, -150
                nextCard.MainImage.TranslationY = LimitToRange((imageBaseMargin * percentFromCenter) * 1.5, -150,0);
            }

            if (args.Status == PanCardView.Enums.UserInteractionStatus.Ended ||
                args.Status == PanCardView.Enums.UserInteractionStatus.Ending)
            {
                card.Opacity = 1;
                card.MainImage.Scale = 1;
                card.MainImage.TranslationY =  -150;
                card.MainImage.Opacity = 1;
            }

        }

        public double LimitToRange(double value, double inclusiveMinimum, double inlusiveMaximum)
        {
            if (value >= inclusiveMinimum)
            {
                if (value <= inlusiveMaximum)
                {
                    return value;
                }

                return inlusiveMaximum;
            }

            return inclusiveMinimum;
        }



    }
}

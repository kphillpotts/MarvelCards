using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MarvelCards.ViewModels
{
    public class HeroCardsViewModel
    {
        public ObservableCollection<Hero> Heroes { get; set; }

        public HeroCardsViewModel()
        {
            Heroes = new ObservableCollection<Hero>()
            {
                new Hero()
                {
                    HeroName = "spider man",
                    RealName = "peter parker",
                    Image = "spiderman.png",
                    HeroColor = "#C51925"
                },
                new Hero()
                {
                    HeroName = "iron man",
                    RealName = "tony stark",
                    Image = "ironman.png",
                    HeroColor = "#DF8E04"
                },
            };
        }
    }

    public class Hero
    {
        public string HeroName { get; set; }
        public string RealName { get; set; }
        public string Image { get; set; }
        public string HeroColor { get; set; }

    }
}

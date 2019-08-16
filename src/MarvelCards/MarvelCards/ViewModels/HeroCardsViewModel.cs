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
                    HeroColor = "#C51925",
                    Posters = new List<string>()
                    {
                        "spiderman_1.png",
                        "spiderman_2.png",
                        "spiderman_3.png",
                        "spiderman_4.png",
                    },
                    Bio = "Peter Benjamin Parker was born to C.I.A. agents Richard and Mary Parker, who were killed when Peter was very young. After the death of his parents, Peter was raised by his Uncle Ben and Aunt May in a modest house in Forest Hills, New York."

                },
                new Hero()
                {
                    HeroName = "iron man",
                    RealName = "tony stark",
                    Image = "ironman.png",
                    HeroColor = "#DF8E04",
                    Posters = new List<string>()
                    {
                        "ironman_1.png",
                        "ironman_2.png",
                        "ironman_3.png",
                        "ironman_4.png",
                    },
                    Bio = "Tony Stark is an eccentric self-described genius, billionaire, playboy and philanthropist and the former head of Stark Industries."
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
        public string Bio { get; set; }
        public List<string> Posters { get; set; }

    }
}

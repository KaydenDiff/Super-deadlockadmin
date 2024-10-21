using System.Collections.ObjectModel;
using System.ComponentModel;
using DeadLockApp.Models;

namespace DeadLockApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Коллекция героев
        public ObservableCollection<Hero> Heroes { get; set; }
        public ObservableCollection<Item> Items { get; set; }
        public ObservableCollection<Tactic> Tactics { get; set; }

        public MainViewModel()
        {
            // Инициализация списка героев
            Heroes = new ObservableCollection<Hero>
            {
                new Hero { Name = "Abrams", ImageUrl = "abrams.png", },
                new Hero { Name = "Bebop", ImageUrl = "bebop.png" },
                new Hero { Name = "Dynamo",ImageUrl = "dynamo.png"},
                new Hero { Name = "Grey Talon",ImageUrl = "greytalon.png"},
                new Hero { Name = "Haze",  ImageUrl = "haze.png"  },
                new Hero { Name = "Infernus",  ImageUrl = "inf.png"},
                new Hero { Name = "Ivy", ImageUrl = "ivy.png"  },
                new Hero { Name = "Kelvin",  ImageUrl = "kelvin.png" },
                new Hero { Name = "Lady Geist", ImageUrl = "ladygeist.png" },
                new Hero { Name = "Lash", ImageUrl = "lash.png" },
                new Hero { Name = "McGinnis", ImageUrl = "mcginnis.png"},
                new Hero { Name = "Mirage", ImageUrl = "mirage.png"},
                new Hero { Name = "Mo & Krill", ImageUrl = "mokrill.png" },
                new Hero { Name = "Paradox", ImageUrl = "paradox.png"},
                new Hero { Name = "Pocket", ImageUrl = "pocket.png"},
                new Hero { Name = "Seven", ImageUrl = "seven.png"},
                new Hero { Name = "Shiv", ImageUrl = "shiv.png" },
                new Hero { Name = "Vindicta", ImageUrl = "vindicta.png"},
                new Hero { Name = "Viscous" , ImageUrl = "viscous.png"},
                new Hero { Name = "Warden", ImageUrl = "warden.png"},
                new Hero { Name = "Wraith", ImageUrl = "wraith.png"},
                new Hero { Name = "Yamato", ImageUrl = "yamat.png" },
            };                                                                                                                    
            Items = new ObservableCollection<Item>
            {
                new Item { Name = "Basic Magazine", Category = "Weapon", Tier = "I",  Price = 500,  ImageUrl = "weapon9.png" },
                new Item { Name = "High-Velocity Mag", Category = "Weapon", Tier = "I",  Price = 500, ImageUrl = "weapon1.png" },
                new Item { Name = "Active Reload", Category = "Weapon", Tier = "II",  Price = 1250, ImageUrl = "weapon2.png" },
                new Item { Name = "Berserker", Category = "Weapon", Tier = "II", Price = 1250, ImageUrl = "weapon8.png" },
                new Item { Name = "Burst Fire", Category = "Weapon", Tier = "III", Price = 3000, ImageUrl = "weapon7.png" },
                new Item { Name = "Escalating Resilience", Category = "Weapon", Tier = "III", Price = 3000, ImageUrl = "weapon5.png" },
                new Item { Name = "Crippling Headshot", Category = "Weapon", Tier = "IV", Price = 6200, ImageUrl = "weapon6.png" },
                new Item { Name = "Frenzy", Category = "Weapon", Tier = "IV", Price = 6200, ImageUrl = "weapon4.png" },
                new Item { Name = "Extra Health", Category = "Vitality", Tier = "I", Price = 500, ImageUrl = "vitality7.png" },
                new Item { Name = "Extra Regen", Category = "Vitality", Tier = "I", Price = 500, ImageUrl = "vitality6.png" },
                new Item { Name = "Bullet Armor", Category = "Vitality", Tier = "IV", Price = 1250, ImageUrl = "vitality1.png" },
                new Item { Name = "Combat Barrier", Category = "Vitality", Tier = "IV", Price = 1250, ImageUrl = "vitality10.png" },
                new Item { Name = "Fortitude", Category = "Vitality", Tier = "IV", Price = 3000, ImageUrl = "vitality5.png" },
                new Item { Name = "Improved Bullet Armor", Category = "Vitality", Tier = "IV", Price = 3000, ImageUrl = "vitality4.png" },
                new Item { Name = "Inhibitor", Category = "Vitality", Tier = "IV", Price = 7450, ImageUrl = "vitality3.png" },
                new Item { Name = "Leech", Category = "Vitality", Tier = "IV", Price = 6200, ImageUrl = "vitality2.png" },
                new Item { Name = "Extra Spirit", Category = "Spirit", Tier = "I", Price = 500, ImageUrl = "spirit5.png" },
                new Item { Name = "Mystic Burst", Category = "Spirit", Tier = "I", Price = 500, ImageUrl = "spirit2.png" },
                new Item { Name = "Bullet Resist Shredder", Category = "Spirit", Tier = "II", Price = 1250, ImageUrl = "spirit1.png" },
                new Item { Name = "Duration Extender", Category = "Spirit", Tier = "II", Price = 1250, ImageUrl = "spirit6.png" },
                new Item { Name = "Improved Burst", Category = "Spirit", Tier = "III", Price = 3000, ImageUrl = "spirit4.png" },
                new Item { Name = "Improved Reach", Category = "Spirit", Tier = "III", Price = 3000, ImageUrl = "spirit3.png" },
                new Item { Name = "Curse", Category = "Spirit", Tier = "IV", Price = 6300, ImageUrl = "spirit7.png" },
            };
           
        }
    }
}
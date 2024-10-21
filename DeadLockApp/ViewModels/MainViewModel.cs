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
                new Hero { Name = "Abrams", Description = "Рвётся в ближний бой.", ImageUrl = "abrams.png", IsFavorite = false},
                new Hero { Name = "Bebop", Description = "Цепляет крюком и добивает комбинацией.", ImageUrl = "bebop.png", IsFavorite = false },
                new Hero { Name = "Dynamo", Description = "Держит врагов на месте.", ImageUrl = "dynamo.png" , IsFavorite = false},
                new Hero { Name = "Grey Talon", Description = "Точно стреляет издалека.", ImageUrl = "greytalon.png" , IsFavorite = false},
                new Hero { Name = "Haze", Description = "Подкрадывается и решетит пулями.", ImageUrl = "haze.png" , IsFavorite = false  },
                new Hero { Name = "Infernus", Description = "Поджигает врагов и наблюдает за их мучениями.", ImageUrl = "inf.png" , IsFavorite = false},
                new Hero { Name = "Ivy", Description = "Отличная напарница для везучих союзников.", ImageUrl = "ivy.png", IsFavorite = false  },
                new Hero { Name = "Kelvin", Description = "Замораживает врагов намертво.", ImageUrl = "kelvin.png", IsFavorite = false },
                new Hero { Name = "Lady Geist", Description = "Жертвует здоровьем, но возвращает себе сторицей.", ImageUrl = "ladygeist.png", IsFavorite = false },
                new Hero { Name = "Lash", Description = "Атакует с воздуха и топчет врагов.", ImageUrl = "lash.png", IsFavorite = false },
                new Hero { Name = "McGinnis", Description = "Контролирует бой с помощью боевых турелей.", ImageUrl = "mcginnis.png" , IsFavorite = false},
                new Hero { Name = "Mirage", Description = "Совершает точные выстрелы и ловит убегающих врагов", ImageUrl = "mirage.png" , IsFavorite = false},
                new Hero { Name = "Mo & Krill", Description = "Копают под своих врагов.", ImageUrl = "mokrill.png", IsFavorite = false },
                new Hero { Name = "Paradox", Description = "Манипулирует пространством и временем.", ImageUrl = "paradox.png" , IsFavorite = false},
                new Hero { Name = "Pocket", Description = "Проникает в тыл врага и наводит хаос.", ImageUrl = "pocket.png" , IsFavorite = false},
                new Hero { Name = "Seven", Description = "Поражает током толпы врагов.", ImageUrl = "seven.png" , IsFavorite = false},
                new Hero { Name = "Shiv", Description = "Изматывает, а затем добивает врагов!", ImageUrl = "shiv.png", IsFavorite = false },
                new Hero { Name = "Vindicta", Description = "Самый тихий и смертоносный снайпер", ImageUrl = "vindicta.png" , IsFavorite = false},
                new Hero { Name = "Viscous", Description = "Катится вокруг, отражая пули", ImageUrl = "viscous.png" , IsFavorite = false},
                new Hero { Name = "Warden", Description = "Страж между двумя мирами — нашим и потусторонним.", ImageUrl = "warden.png" , IsFavorite = false},
                new Hero { Name = "Wraith", Description = "Смертоносная охотница за головами.", ImageUrl = "wraith.png" , IsFavorite = false},
                new Hero { Name = "Yamato", Description = "Самурай бесконечного пути.", ImageUrl = "yamat.png" , IsFavorite = false },
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
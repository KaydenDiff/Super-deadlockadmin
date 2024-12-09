using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DeadLockApp.Models
{
    // Главный класс Item
    public class Item
    {
        // Вложенный класс ItemData
        public class ItemData
        {
            [JsonPropertyName("id")]
            public long Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("description")]
            public string Description { get; set; }

            [JsonPropertyName("cost")]
            public int Cost { get; set; }

            [JsonPropertyName("tier_id")]
            public long? TierId { get; set; }

            [JsonPropertyName("type_id")]
            public long? TypeId { get; set; }

            [JsonPropertyName("image")]
            public string ImageName { get; set; }

            [JsonPropertyName("created_at")]
            public DateTime? CreatedAt { get; set; }

            [JsonPropertyName("updated_at")]
            public DateTime? UpdatedAt { get; set; }

            // Полный путь к картинке
            public string ImageUrl => $"http://course-project-4/public/storage/{ImageName}";
        }

        // Экземпляр класса ItemData
        public ItemData Data { get; set; }

        // Свойства Category и Tier теперь ссылаются на соответствующие поля через экземпляр ItemData
        public string Category => Data.TypeId switch
        {
            1 => "Weapon",
            2 => "Vitality",
            3 => "Spirit",
            _ => "Unknown"
        };

        public string Tier => Data.TierId switch
        {
            1 => "I",
            2 => "II",
            3 => "III",
            4 => "IV",
            _ => "Unknown"
        };

        // Для связи с полями TierId и TypeId
        public long? TierId => Data?.TierId;
        public long? TypeId => Data?.TypeId;

        // События для PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

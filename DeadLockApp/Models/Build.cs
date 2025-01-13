using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DeadLockApp.Models
{
    public class BuildItem
    {
        [JsonPropertyName("part")]
        public int PartId { get; set; }

        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        [JsonPropertyName("item_name")]
        public string ItemName { get; set; }
    }

    public class Build
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("items")]
        public List<BuildItem> Items { get; set; } = new();
    }

    public class BuildResponse
    {
        [JsonPropertyName("character")]
        public Character Character { get; set; }

        [JsonPropertyName("builds")]
        public List<Build> Builds { get; set; }
    }
    public class BuildDetailsResponse
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<BuildItem> Items { get; set; }
    }

}

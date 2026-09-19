using System.ComponentModel.DataAnnotations;

namespace SubTrack.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        // Цвет плашки/текста (например, для CSS)
        public string? ColorHex { get; set; }

        // Навигационное свойство: у одной категории может быть много подписок
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}

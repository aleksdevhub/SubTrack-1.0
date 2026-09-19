using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubTrack.Models
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // Цена подписки
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // Валюта (например, "₽", "$")
        [StringLength(5)]
        public string Currency { get; set; } = "₽";

        // Первая буква или короткое имя для логотипа, если нет картинки
        [StringLength(2)]
        public string LogoText { get; set; } = string.Empty;

        // Класс для кастомного цвета логотипа в CSS (например, "netflix-logo")
        public string? CssClass { get; set; }

        // Дата следующего списания
        [Required]
        public DateTime NextPaymentDate { get; set; }

        // Активна ли подписка в данный момент
        public bool IsActive { get; set; } = true;

        // Связь с категорией (Внешний ключ)
        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
    }
}

using SubTrack.Models;

namespace SubTrack.data
{
    public static class DbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

                // ГАРАНТИРОВАННО СОЗДАЕТ ФАЙЛ БАЗЫ И ВСЕ ТАБЛИЦЫ, ЕСЛИ ИХ НЕТ НА ДИСКЕ
                context.Database.EnsureCreated();

                // Проверяем, есть ли категории. Если нет — добавляем
                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(
                        new Category { Name = "Стриминг", ColorHex = "#ff1717" },
                        new Category { Name = "Музыка", ColorHex = "#00df00" },
                        new Category { Name = "Софт", ColorHex = "#6366f1" }
                    );
                    context.SaveChanges();
                }

                // Проверяем, есть ли подписки. Если нет — добавляем со связью к категориям
                if (!context.Subscriptions.Any())
                {
                    var streamingCat = context.Categories.First(c => c.Name == "Стриминг");
                    var musicCat = context.Categories.First(c => c.Name == "Музыка");

                    context.Subscriptions.AddRange(
                        new Subscription
                        {
                            Name = "Netflix",
                            Price = 700,
                            Currency = "₽",
                            LogoText = "N",
                            CssClass = "netflix-logo",
                            NextPaymentDate = DateTime.Now.AddDays(12),
                            CategoryId = streamingCat.Id
                        },
                        new Subscription
                        {
                            Name = "Яндекс Музыка",
                            Price = 299,
                            Currency = "₽",
                            LogoText = "Y",
                            CssClass = "yandex-logo",
                            NextPaymentDate = DateTime.Now.AddDays(5),
                            CategoryId = musicCat.Id
                        },
                        new Subscription
                        {
                            Name = "Spotify",
                            Price = 169,
                            Currency = "₽",
                            LogoText = "S",
                            CssClass = "spotify-logo",
                            NextPaymentDate = DateTime.Now.AddDays(21),
                            CategoryId = musicCat.Id
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}

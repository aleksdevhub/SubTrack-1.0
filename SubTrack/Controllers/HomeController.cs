using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubTrack.data;
using SubTrack.Models;

namespace SubTrack.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        // Конструктор, который передает базу данных в контроллер
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Загружаем active подписки вместе с их категориями
            var subscriptions = await _context.Subscriptions
                .Include(s => s.Category)
                .Where(s => s.IsActive)
                .ToListAsync();

            // 2. Считаем общую статистику для блока "Всего"
            ViewBag.TotalSum = subscriptions.Sum(s => s.Price);
            ViewBag.TotalCount = subscriptions.Count;
            ViewBag.MaxPrice = subscriptions.Any() ? subscriptions.Max(s => s.Price) : 0;

            // 3. Группируем расходы по категориям для левой панели
            var categoryExpenses = subscriptions
                .Where(s => s.Category != null)
                .GroupBy(s => s.Category!.Name)
                .Select(g => new {
                    CategoryName = g.Key,
                    TotalAmount = g.Sum(s => s.Price)
                })
                .ToDictionary(x => x.CategoryName, x => x.TotalAmount);

            ViewBag.CategoryExpenses = categoryExpenses;

            // 4. Подготавливаем данные для графика (динамика расходов за 6 месяцев)
            ViewBag.ChartMonths = new string[] { "Мар", "Апр", "Май", "Июн", "Июл", "Авг" };

            // Берем текущую сумму трат и симулируем красивые колебания под макет
            decimal currentTotal = ViewBag.TotalSum;
            ViewBag.ChartValues = new decimal[] {
                currentTotal * 0.8m,
                currentTotal * 0.9m,
                currentTotal,
                currentTotal * 1.05m,
                currentTotal * 0.95m,
                currentTotal
            };

            return View(subscriptions);
        }

        // 1. Открытие страницы добавления (GET)
        [HttpGet]
        public async Task<IActionResult> AddSubscription()
        {
            // Загружаем категории из базы, чтобы пользователь мог выбрать их в выпадающем списке
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View();
        }

        // 2. Обработка данных из формы и сохранение в БД (POST)
        [HttpPost]
        public async Task<IActionResult> AddSubscription(Subscription model)
        {
            if (model != null)
            {
                // Автоматическое назначение класса цвета и логотипа по имени
                string nameLower = model.Name.ToLower();
                if (nameLower.Contains("netflix")) { model.CssClass = "netflix-logo"; model.LogoText = "N"; }
                else if (nameLower.Contains("яндекс")) { model.CssClass = "yandex-logo"; model.LogoText = "Y"; }
                else if (nameLower.Contains("spotify")) { model.CssClass = "spotify-logo"; model.LogoText = "S"; }
                else { model.CssClass = "default-logo"; model.LogoText = model.Name.Length > 0 ? model.Name.Substring(0, 1).ToUpper() : "P"; }

                model.NextPaymentDate = DateTime.Now.AddMonths(1); // Ставим дату платежа через месяц
                model.IsActive = true;

                // Сохраняем в базу данных
                _context.Subscriptions.Add(model);
                await _context.SaveChangesAsync();

                // Возвращаем пользователя на главную страницу, где уже появится новая подписка
                return RedirectToAction("Index");
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(model);
        }
    }
}

using SubTrack.data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Подключаем Базу Данных SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=subtrack.db"));

// Подключаем MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Обработка ошибок
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// HTTPS
app.UseHttpsRedirection();

// CSS, JS, изображения из wwwroot
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Основной маршрут MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Автоматически наполняем базу тестовыми данными при старте (ЕСЛИ ОНА ПУСТАЯ)
DbInitializer.Seed(app);

app.Run();

namespace SubTrack.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // Пароли храним только в хэше!

        // Связь: у одного пользователя может быть много подписок
        public List<Subscription> Subscriptions { get; set; } = new();
    }
}

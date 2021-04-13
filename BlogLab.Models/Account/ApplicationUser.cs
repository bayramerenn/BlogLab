namespace BlogLab.Models.Account
{
    public class ApplicationUser
    {
        public int AplicationUserId { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
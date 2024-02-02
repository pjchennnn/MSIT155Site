namespace MSIT155Site.Models.DTO
{
    public class user_DTO
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int? Age { get; set; }
        public IFormFile? Avatar { get; set; }
    }
}

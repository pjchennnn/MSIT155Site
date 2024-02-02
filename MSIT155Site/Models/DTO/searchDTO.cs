namespace MSIT155Site.Models.DTO
{
    public class searchDTO
    {
        public int? categoryId { get; set; }
        public string? keyword { get; set; }
        public int? page { get; set; }
        public int? pageSize { get; set; }
        public string? sortType { get; set; }
        public string? sortBy { get; set; }
    }
}

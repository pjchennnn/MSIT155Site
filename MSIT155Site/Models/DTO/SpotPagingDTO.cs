namespace MSIT155Site.Models.DTO
{
    public class SpotPagingDTO
    {
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public List<SpotImagesSpot>? SpotResult { get; set; }
    }
}

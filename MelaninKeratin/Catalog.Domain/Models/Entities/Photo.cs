namespace Catalog.Domain.Models.Entities
{
    public class Photo
    {
        public int Id { get; private set; }
        public string Url { get; set; } = string.Empty;
        public bool IsMainPhoto { get; set; }
        public string PublicId { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        
    }

}

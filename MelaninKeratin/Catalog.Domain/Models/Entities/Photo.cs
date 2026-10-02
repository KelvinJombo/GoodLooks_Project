namespace Catalog.Domain.Models.Entities
{
    public class Photo
    {
        public int Id { get; private set; }
        public string Url { get; private set; } = string.Empty;
        public bool IsMain { get; set; }
        public string PublicId { get; set; } = string.Empty;
        public int DisplayOrder { get; private set; }
        
    }

}

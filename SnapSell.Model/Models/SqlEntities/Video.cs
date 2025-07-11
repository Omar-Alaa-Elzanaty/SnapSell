namespace SnapSell.Domain.Models.SqlEntities
{
    public class Video : BaseEntity
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public virtual List<ProductVideo> Products { get; set; }
    }
}

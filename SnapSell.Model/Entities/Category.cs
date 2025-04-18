namespace SnapSell.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentCategoryId { get; set; }
    }
}
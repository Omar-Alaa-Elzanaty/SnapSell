namespace SnapSell.Domain.Models
{
    public class Product
    {
        public string EnglishName { get; set; }
        public string ArabicName { get; set; }

        public List<Category> Categories { get; set; }
        public List<Size> Sizes { get; set; }
        public List<Color> Colors { get; set; }
        public List<Variant> Variants { get; set; }
        public List<Review> Reviews { get; set; }

        public int Shipping { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsHidden { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public int MinDeleveryDays { get; set; }
        public int MaxDeleveryDays { get; set; }
    }
}

namespace backend.Models
{
    public class Subcategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Code { get; set; }
        public int Category_id { get; set; }
        public Category Category { get; set; } = null!;
    }
}

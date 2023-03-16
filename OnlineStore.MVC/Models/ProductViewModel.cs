using System.ComponentModel.DataAnnotations;

namespace OnlineStore.MVC.Models
{
    public class ProductViewModel
    {
        public int? Id { get; set; }
        public string Product_Name { get; set; } = null!;
        public decimal ListPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public float Discount { get; set; }
        public string CategoryName { get; set; } = null!;
    }

    public class AddOrUpdateViewModel
    {
        public int? Id { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string Product_Name { get; set; } = null!;

        [Required]
        public decimal ListPrice { get; set; }

        [Required]
        public decimal SellingPrice { get; set; }

        [Required]
        public float Discount { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
    }
}

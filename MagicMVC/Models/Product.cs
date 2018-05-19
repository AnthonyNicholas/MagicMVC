using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MagicMVC.Models
{
    public class Product
    {
        //private readonly MagicMVCContext _context;
        [Display(Name="Product ID")]
        public int ProductID { get; set; }
        [Display(Name="Product Name")]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

    }
    public static class ProductExtensions
    {
        public static Product GetProduct(this ICollection<Product> inventory, int productID)
        {
            return inventory.FirstOrDefault(x => x.ProductID == productID);
        }
    }
}

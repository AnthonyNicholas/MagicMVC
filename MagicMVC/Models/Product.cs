using System.Collections.Generic;
using System.Linq;

namespace MagicMVC.Models
{
    public class Product
    {
        //private readonly MagicMVCContext _context;
        public int ProductID { get; set; }
        public string Name { get; set; }
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

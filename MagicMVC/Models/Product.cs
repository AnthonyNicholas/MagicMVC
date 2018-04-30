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

        //public Product(int productID, string name, decimal price, MagicMVCContext context)
        //public Product(int productID, string name, decimal price)
        // {
        //     ProductID = productID;
        //     Name = name;
        //     Price = price;
        //    //_context = context;
        //}

        //public bool IsBelowThreshold(int threshold)
        // {
        //     return StockLevel < threshold;
        // }
    }
    public static class ProductExtensions
    {
        public static Product GetProduct(this List<Product> inventory, int productID)
        {
            return inventory.FirstOrDefault(x => x.ProductID == productID);
        }
    }
}

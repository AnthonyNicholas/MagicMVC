using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace MagicMVC.Models
{
    public class Owner
    {
        public List<Product> Inventory { get; set; }

        public Product GetProduct(int productID)
        {
            return Inventory.GetProduct(productID);
        }


    }
}

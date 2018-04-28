﻿using System.ComponentModel.DataAnnotations;

namespace Standard_VS_MVC_Project.Models
{
    public class StoreInventory
    {
        public int StoreID { get; set; }
        public Store Store { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }

        public int StockLevel { get; set; }
    }
}

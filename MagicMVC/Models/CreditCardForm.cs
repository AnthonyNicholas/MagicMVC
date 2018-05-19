using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;
using MagicMVC.Utilities;

namespace MagicMVC.Models
{
    public class CreditCardForm
    {
        [Display(Name = "Credit Card Type")]
        [Required]
        public CardType CreditCardType { get; set; }

        [Display(Name = "Credit Card Number")]
        [Required]
        [CreditCard]
        public string CreditCardNumber { get; set; }
    }
}

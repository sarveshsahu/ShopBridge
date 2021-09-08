using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ShopBridge.Model
{
    public class Inventory
    {
        [Key]
        public int ItemID { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name Required")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "Price Required")]
        public decimal Price { get; set; }

        [Display(Name = "Qty")]
        [Required(ErrorMessage = "Qty Required")]
        public int Qty { get; set; }

        [Display(Name = "Make")]
        public string Make { get; set; }

        [Display(Name = "Make Year")]
        public int MakeYear { get; set; }
    }
}

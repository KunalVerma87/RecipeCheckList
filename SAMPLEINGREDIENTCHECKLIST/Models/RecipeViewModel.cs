using SAMPLEINGREDIENTCHECKLIST.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAMPLEINGREDIENTCHECKLIST.Models
{
    public class RecipeDataViewModel
    {
        public int RecipeId { get; set; }
        [Required]
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; }
        public string IngredientName { get; set; }
    }
}
using SAMPLEINGREDIENTCHECKLIST.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAMPLEINGREDIENTCHECKLIST.Models
{
    public class IngredientDataViewModel
    {
        public int IngredientId { get; set; }
        public int RecipeId { get; set; }
        [Required]
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public virtual Recipe Receipe { get; set; }
        public List<RecipeDataViewModel> RecipeList { get; set; }
        public string RecipeName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAMPLEINGREDIENTCHECKLIST.Models
{
    public class DishesViewModel
    {
        public string UserId { get; set; }
        public List<RecipeViewModel> RecipeList { get; set; }
        public List<RecipeViewModel> RecipeCollection { get; set; }

    }

    public class RecipeViewModel
    {
        public int RecipeId { get; set; }
        public string RecipeName { get; set; }
        public List<IngredientsViewModel> IngredientsList { get; set; }
    }

    public class IngredientsViewModel
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }

        public int RecipeId { get; set; }
    }
}
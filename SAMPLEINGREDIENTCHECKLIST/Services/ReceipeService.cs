using SAMPLEINGREDIENTCHECKLIST.Entity;
using SAMPLEINGREDIENTCHECKLIST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAMPLEINGREDIENTCHECKLIST.Services
{
    public class RecipeService
    {
        public DBEntities entities = new DBEntities();
        public List<RecipeDataViewModel> GetReceipeList()
        {
            List<Recipe> obj = entities.Recipes.ToList();
            List<RecipeDataViewModel> _reviewmodel = new List<RecipeDataViewModel>();
            foreach (var item in obj)
            {
                RecipeDataViewModel ob = new RecipeDataViewModel();
                ob.RecipeId = item.RecipeId;
                ob.Name = item.Name;
                ob.CreatedBy = item.CreatedBy;
                ob.CreatedDate = item.CreatedDate;
                ob.ModifiedBy = item.ModifiedBy;
                ob.ModifiedDate = item.ModifiedDate;
                _reviewmodel.Add(ob);
            }
            return _reviewmodel;
        }
    }
}
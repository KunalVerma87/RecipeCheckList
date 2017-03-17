using SAMPLEINGREDIENTCHECKLIST.Entity;
using SAMPLEINGREDIENTCHECKLIST.Models;
using SAMPLEINGREDIENTCHECKLIST.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace SAMPLEINGREDIENTCHECKLIST.Controllers
{
    [Authorize]
    public class IngredientController : Controller
    {
        public DBEntities entities = new DBEntities();
        //
        // GET: /Ingredient/
        public ActionResult Index(int? id)
        {
            List<Ingredient> obj = entities.Ingredients.Where(t=>t.RecipeId==id).ToList();
            List<IngredientDataViewModel> _ingviewmodel = new List<IngredientDataViewModel>();
            foreach (var item in obj)
            {
                IngredientDataViewModel ob = new IngredientDataViewModel();
                ob.IngredientId = item.IngredientId;
                ob.RecipeId = item.RecipeId;
                ob.RecipeName = item.Recipe.Name;
                ob.Name = item.Name;
                ob.CreatedBy = item.CreatedBy;
                ob.CreatedDate = item.CreatedDate;
                ob.ModifiedBy = item.ModifiedBy;
                ob.ModifiedDate = item.ModifiedDate;
                _ingviewmodel.Add(ob);
            }
            return View(_ingviewmodel);
        }
        public ActionResult Create(int? id)
        {
            IngredientDataViewModel _ingviewmodel = new IngredientDataViewModel();
            using (entities = new DBEntities())
            {
                var getRecipeDetails = entities.Recipes.Where(t => t.RecipeId == id).FirstOrDefault();
                if (getRecipeDetails != null)
                    _ingviewmodel.RecipeName = getRecipeDetails.Name;
            }
            _ingviewmodel.RecipeId = id.Value;
            return View(_ingviewmodel);
        }

        public ActionResult Edit(int? id)
        {
            IngredientDataViewModel _ingviewmodel = new IngredientDataViewModel();
            using (entities = new DBEntities())
            {
                var getIngredientsDetails = entities.Ingredients.Where(t => t.IngredientId == id).FirstOrDefault();
                if (getIngredientsDetails != null)
                {
                    _ingviewmodel.RecipeName = getIngredientsDetails.Recipe.Name;
                    _ingviewmodel.Name = getIngredientsDetails.Name;
                    _ingviewmodel.IngredientId = getIngredientsDetails.IngredientId;
                    _ingviewmodel.RecipeId = getIngredientsDetails.RecipeId;
                }
            }
            return View("Create", _ingviewmodel);
        }


        [HttpPost]
        public ActionResult Create(IngredientDataViewModel model)
        {
            var loggedInUser = HttpContext.User.Identity.GetUserId();
            ViewBag.ErrorMessage = string.Empty;
            using (entities = new DBEntities())
            {
                try
                {
                    Ingredient _recobj = new Ingredient();
                    if (model.IngredientId > 0)
                    {
                        //Edit
                        _recobj = entities.Ingredients.Where(m => m.IngredientId == model.IngredientId).FirstOrDefault();
                        if (_recobj != null)
                        {
                            var getNameCount = entities.Ingredients.Where(t => t.Name.Trim().ToLower() == model.Name.Trim().ToLower() && t.IngredientId != _recobj.IngredientId && t.RecipeId==_recobj.RecipeId).Count();
                            if (getNameCount > 0)
                            {
                                TempData["ErrorMessage"] = "Ingredient name already exists.";
                                TempData["Name"] = model.Name;
                                return RedirectToAction("Edit", new { id = _recobj.IngredientId });
                            }
                            _recobj.ModifiedBy = loggedInUser;
                            _recobj.ModifiedDate = DateTime.Now;
                        }
                    }
                    else
                    {
                        //Create 
                        _recobj = new Ingredient();
                        _recobj.RecipeId = model.RecipeId;
                        _recobj.CreatedBy = loggedInUser;
                        _recobj.CreatedDate = DateTime.Now;
                       
                        entities.Ingredients.Add(_recobj);
                    }

                    _recobj.Name = model.Name;
                    entities.SaveChanges();
                    
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {

                        foreach (var validationError in validationErrors.ValidationErrors)
                        {

                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                        }

                    }

                }
            };
            return RedirectToAction("Display","Recipe", new { id = model.RecipeId });
        }

        public ActionResult Delete(int id)
        {
            int recipeId = 0;
            Ingredient _recobj = entities.Ingredients.Where(m => m.IngredientId == id).FirstOrDefault();
            if (_recobj != null)
            {
                recipeId = _recobj.RecipeId;
                entities.Ingredients.Remove(_recobj);
                entities.SaveChanges();
            }
            return RedirectToAction("Display", "Recipe", new { id = recipeId });
        }


    }
}
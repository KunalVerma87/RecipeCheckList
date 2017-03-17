using SAMPLEINGREDIENTCHECKLIST.Entity;
using SAMPLEINGREDIENTCHECKLIST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace SAMPLEINGREDIENTCHECKLIST.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult DishesList()
        {
            DBEntities context = new DBEntities();
            DishesViewModel dishModel = new DishesViewModel();
            var loggedInUser = HttpContext.User.Identity.GetUserId();
            using (context = new DBEntities())
            {
                dishModel.UserId = loggedInUser;
                dishModel.RecipeList = new List<RecipeViewModel>();
                dishModel.RecipeCollection = new List<RecipeViewModel>();
                dishModel.RecipeCollection = context.Recipes.Select(s => new RecipeViewModel() { RecipeId = s.RecipeId, RecipeName = s.Name }).ToList();

                var getDishes = context.Dishes.Where(t => t.UserId == loggedInUser).ToList();

                if (getDishes.Any())
                {
                    foreach (var item in getDishes)
                    {
                        RecipeViewModel recModelObj = new RecipeViewModel();
                        if (item.Recipe != null)
                        {
                            recModelObj.RecipeName = item.Recipe.Name;
                            recModelObj.RecipeId = item.Recipe.RecipeId;

                            recModelObj.IngredientsList = new List<IngredientsViewModel>();
                            recModelObj.IngredientsList = item.Recipe.Ingredients.Select(s => new IngredientsViewModel() { IngredientId = s.IngredientId, Name = s.Name, RecipeId = item.Recipe.RecipeId }).ToList();
                        }

                        var getDishIngredient = context.DishIngredientAssociations.Where(t => t.DishId == item.DishId).ToList();
                        if (getDishIngredient.Any())
                        {
                            foreach (var item1 in getDishIngredient)
                            {
                                var getIntgredient = recModelObj.IngredientsList.Where(t => t.IngredientId == item1.IngredientId).FirstOrDefault();
                                if (getIntgredient != null)
                                {
                                    getIntgredient.Checked = true;
                                }
                            }
                        }
                        dishModel.RecipeList.Add(recModelObj);
                    }
                }
            }
            return Json(dishModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetIngredientList(int recipeId)
        {
            var loggedInUser = HttpContext.User.Identity.GetUserId();
            DBEntities context = new DBEntities();
            List<IngredientsViewModel> ingredientList = new List<IngredientsViewModel>();
            using (context = new DBEntities())
            {
                ingredientList = context.Ingredients.Where(t => t.RecipeId == recipeId).Select(s => new IngredientsViewModel() { IngredientId = s.IngredientId, Name = s.Name, RecipeId = recipeId }).ToList();
                var getDishAssociationList = context.Dishes.Where(t => t.RecipeId == recipeId && t.UserId==loggedInUser).FirstOrDefault();
                if (getDishAssociationList != null)
                {
                    foreach (var item in getDishAssociationList.DishIngredientAssociations)
                    {
                        var getAssociateIngredient = ingredientList.Where(t => t.IngredientId == item.IngredientId).FirstOrDefault();
                        if (getAssociateIngredient != null)
                        {
                            getAssociateIngredient.Checked = true;
                        }
                    }
                }
            }
            return Json(ingredientList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveDish(int recipeId, int ingredientId, bool check)
        {
            var loggedInUser = HttpContext.User.Identity.GetUserId();
            DBEntities context = new DBEntities();
            List<IngredientsViewModel> ingredientList = new List<IngredientsViewModel>();

            try
            {


                using (context = new DBEntities())
                {
                    Dish dishObj = new Dish();
                    var getDish = context.Dishes.Where(t => t.UserId == loggedInUser && t.RecipeId == recipeId).FirstOrDefault();
                    if (getDish != null)
                    {
                        var getAssociation = context.DishIngredientAssociations.Where(t => t.IngredientId == ingredientId).FirstOrDefault();
                        if (getAssociation != null)
                        {
                            context.DishIngredientAssociations.Remove(getAssociation);
                            context.SaveChanges();
                        }
                        else
                        {
                            DishIngredientAssociation dishAssociationObj = new DishIngredientAssociation();
                            dishAssociationObj.DishId = getDish.DishId;
                            dishAssociationObj.IngredientId = ingredientId;
                            context.DishIngredientAssociations.Add(dishAssociationObj);
                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        dishObj.RecipeId = recipeId;
                        dishObj.UserId = loggedInUser;
                        dishObj.CreatedDate = DateTime.Now;

                        DishIngredientAssociation dishAssociationObj = new DishIngredientAssociation();
                        dishAssociationObj.DishId = dishObj.DishId;
                        dishAssociationObj.IngredientId = ingredientId;
                        dishObj.DishIngredientAssociations.Add(dishAssociationObj);

                        context.Dishes.Add(dishObj);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
    }
}
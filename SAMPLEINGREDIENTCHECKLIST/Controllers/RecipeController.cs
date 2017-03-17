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
    public class RecipeController : Controller
    {
        //
        // GET: /Recipe/
        public DBEntities entities = new DBEntities();
        public ActionResult Index()
        {
            RecipeService service = new RecipeService();
            List<RecipeDataViewModel> rec = service.GetReceipeList();
            return View(rec);
        }

        public ActionResult Create(int? id)
        {
            RecipeDataViewModel _recviewmodel = new RecipeDataViewModel();
            int _id = 0;
            if (id>0)
            {
                _id = id.Value;
            }
            if (entities != null)
            {
                Recipe _obj = entities.Recipes.Where(m => m.RecipeId == _id).FirstOrDefault();
                if (_obj != null)
                {
                    _recviewmodel.RecipeId = _obj.RecipeId;
                    _recviewmodel.Name = _obj.Name;
                    _recviewmodel.CreatedBy = _obj.CreatedBy;
                    _recviewmodel.CreatedDate = _obj.CreatedDate;
                    _recviewmodel.ModifiedBy = _obj.ModifiedBy;
                    _recviewmodel.ModifiedDate = _obj.ModifiedDate;
                }
            }
            return View(_recviewmodel);
        }

        [HttpPost]
        public ActionResult Create(RecipeDataViewModel model)
        {
            var loggedInUser = HttpContext.User.Identity.GetUserId();
            using (entities = new DBEntities())
            {
                try
                {
                    Recipe _recobj = new Recipe();
                    if (model.RecipeId > 0)
                    {
                        //Edit
                        _recobj = entities.Recipes.Where(m => m.RecipeId == model.RecipeId).FirstOrDefault();
                        if (_recobj != null)
                        {

                            var getNameCount = entities.Recipes.Where(t => t.Name.Trim().ToLower() == model.Name.Trim().ToLower() && t.RecipeId != _recobj.RecipeId).Count();
                            if (getNameCount > 0)
                            {
                                TempData["ErrorMessage"] = "Recipe name already exists.";
                                return View("Create", model);
                            }
                            _recobj.ModifiedBy = loggedInUser;
                            _recobj.ModifiedDate = DateTime.Now;
                        }
                        
                    }
                    else
                    {
                        //Create
                        _recobj = new Recipe();
                        _recobj.CreatedBy = loggedInUser;
                        _recobj.CreatedDate = DateTime.Now;
                        entities.Recipes.Add(_recobj);

                    }
                    _recobj.Name = model.Name;
                    entities.SaveChanges();

                }
                catch (Exception ex)
                {
                }
            };
            return RedirectToAction("Index");
        }



        public ActionResult Delete(int id)
        {
            var loggedInUser = HttpContext.User.Identity.GetUserId();
            Recipe _recobj = entities.Recipes.Where(m => m.RecipeId == id).FirstOrDefault();
            //_recobj.Ingredients.Clear();            
            //_recobj.Dishes.Clear();

            entities.Ingredients.RemoveRange(_recobj.Ingredients);

            foreach (var item in _recobj.Dishes)
            {
                entities.DishIngredientAssociations.RemoveRange(entities.DishIngredientAssociations.Where(m => m.DishId == item.DishId).ToList());   
            }            

            entities.Dishes.RemoveRange(_recobj.Dishes);
            entities.Recipes.Remove(_recobj);

            entities.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Display(int id)
        {
            Recipe obj = entities.Recipes.Where(m => m.RecipeId == id).FirstOrDefault();
            return View(obj);
        }
    }
}
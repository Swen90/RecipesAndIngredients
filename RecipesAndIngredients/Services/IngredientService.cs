using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Identity.Client.Utils.Windows;
using RecipesAndIngredients.DTO;
using RecipesAndIngredients.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesAndIngredients.Services
{
    public class IngredientService
    {
        public IngredientDto? Add(IngredientDto ingredientDto)
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                Ingredient? ingredient = new Ingredient()
                {
                    Id = ingredientDto.Id,
                    IngName = ingredientDto.IngName,
                    QuantityTypeId = ingredientDto.QuantityType.Id,
                };
                db.Ingredients.Add(ingredient);
                db.SaveChanges();
                Ingredient? newIngredient = Get(ingredient.Id);
                if (newIngredient == null)
                {
                    return null;
                }
                else
                {
                    IngredientDto? newIngredientDto = Utils.ConvertToIngredientDto(newIngredient);
                    return newIngredientDto;
                }
            }
        }



        private Ingredient? Get(int Id)
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                Ingredient? ingredient = db.Ingredients.Include(p => p.QuantityType).Where(i => i.Id == Id).FirstOrDefault();
                return ingredient;
            }
        }



        public IngredientDto? GetDto(int Id)
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                Ingredient? ingredient = Get(Id);
                if (ingredient == null)
                {
                    return null;
                }
                else
                {
                    IngredientDto? ingredientDto = Utils.ConvertToIngredientDto(ingredient);
                    return ingredientDto;
                }
            }
        }



        public List<IngredientDto>? GetAll()
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                List<Ingredient>? ingredients = db.Ingredients.Include(p => p.QuantityType).ToList();
                List<IngredientDto> ingredientsDto = new List<IngredientDto>();
                foreach(Ingredient ingredient in ingredients)
                {
                    IngredientDto? ingredientDto = Utils.ConvertToIngredientDto(ingredient);
                    ingredientsDto.Add(ingredientDto);
                }
                return ingredientsDto;
            }
        }



        public void Remove(int Id)
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                Ingredient? ingredient = Get(Id);
                if(ingredient == null)
                {
                    return;
                }
                else
                {
                    db.Remove(ingredient);
                    db.SaveChanges();
                }
            }

        }
    }
}

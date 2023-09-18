using Microsoft.EntityFrameworkCore;
using RecipesAndIngredients.DTO;
using RecipesAndIngredients.Models;

namespace RecipesAndIngredients.Services
{
    public class IngredientService
    {
        public IngredientDto Add(IngredientDto ingredientDto)
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                Ingredient ingredient = new Ingredient()
                {
                    IngName = ingredientDto.IngName,
                    QuantityTypeId = ingredientDto.QuantityType.Id,
                };
                db.Ingredients.Add(ingredient);
                db.SaveChanges();
                Ingredient newIngredient = GetById(ingredient.Id)!;

                IngredientDto newIngredientDto = Utils.ConvertToIngredientDto(newIngredient);
                return newIngredientDto;
            }
        }



        private Ingredient? GetById(int Id)
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                Ingredient? ingredient = db.Ingredients.Include(p => p.QuantityType).Where(i => i.Id == Id).FirstOrDefault();
                return ingredient;
            }
        }



        public IngredientDto GetByNameDto(string name)
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                /// QUE: по логике убрал многие проверки, так же и здесь, потому что за проверку на null отвечает другой метод checkExistance
                Ingredient ingredient = db.Ingredients.Include(p => p.QuantityType).Where(i => i.IngName == name).FirstOrDefault()!;

                IngredientDto ingredientDto = Utils.ConvertToIngredientDto(ingredient);
                return ingredientDto;
            }
        }



        public Ingredient? GetByName(string name)
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                Ingredient? ingredient = db.Ingredients.Include(p => p.QuantityType)
                    .Where(i => i.IngName == name.ToLower())
                    .Distinct()
                    .FirstOrDefault(); /// QUE: самый нижний уровень, поиск, может быть null, хотел избавиться от firstordefault поставить first, потому что все обязанности null берет на себя check
                return ingredient;
            }
        }



        public IngredientDto? GetByIdDto(int Id)
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                Ingredient? ingredient = GetById(Id);
                if (ingredient == null)
                    return null;

                IngredientDto? ingredientDto = Utils.ConvertToIngredientDto(ingredient);
                return ingredientDto;
            }
        }



        public List<IngredientDto>? GetAll()
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                List<Ingredient>? ingredients = db.Ingredients.Include(p => p.QuantityType).ToList();
                if (ingredients == null || ingredients.Count == 0)
                    return null;

                List<IngredientDto> ingredientsDto = new List<IngredientDto>();
                foreach(Ingredient ingredient in ingredients)
                {
                    IngredientDto ingredientDto = Utils.ConvertToIngredientDto(ingredient);
                    ingredientsDto.Add(ingredientDto);
                }
                return ingredientsDto;
            }
        }



        public bool UpdateIngredient(IngredientDto ingredientDto)
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                Ingredient? ingredient = db.Ingredients.Where(i => i.Id == ingredientDto.Id).FirstOrDefault(); /// исправили вручную чтобы не перезаписывать связующие таблицы (без Include)
                if (ingredient != null)
                {
                    ingredient.Id = ingredientDto.Id;
                    ingredient.IngName = ingredientDto.IngName;
                    ingredient.QuantityTypeId = ingredientDto.QuantityType.Id;
                    db.Update(ingredient);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }



        public void Remove(string name)
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                Ingredient ingredient = GetByName(name)!; /// QUE: явно указал что null здесь невозможен из-за операций в классе? remove
                
                ///if(ingredient == null) временно убрал
                   /// return;

                db.Remove(ingredient);
                db.SaveChanges();
            }
        }


        public bool CheckExistance(int Id)
        {
            Ingredient? ingredient = GetById(Id);
            if (ingredient == null)
                return false;

                return true;
        }



        public bool CheckExistanceByName(string name)
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                Ingredient? ingredient = GetByName(name);
                if (ingredient == null)
                    return false;
                
                return true;
            }
        }



        public QuantityTypeDto? GetQuantityTypeById(int Id)
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                QuantityType? quantityType = db.QuantityTypes.Where(i => i.Id == Id).FirstOrDefault();
                if (quantityType == null)
                    return null;

                QuantityTypeDto quantityTypeDto = Utils.ConvertToQuantityTypeDto(quantityType);
                return quantityTypeDto;
            }
        }



        public List<QuantityTypeDto>? GetAllQuantityTypes()
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                List<QuantityTypeDto> quantityTypeDtos = new List<QuantityTypeDto>();

                QuantityTypeDto? quantityTypeDto = new QuantityTypeDto();
                List<QuantityType>? quantityTypes = db.QuantityTypes.ToList();
                if (quantityTypes == null)
                    return null;

                else
                {
                    foreach (QuantityType quantityType in quantityTypes)
                    {
                        quantityTypeDto = Utils.ConvertToQuantityTypeDto(quantityType);
                        quantityTypeDtos.Add(quantityTypeDto);
                    }
                    return quantityTypeDtos;
                }
            }
        }
    }
}

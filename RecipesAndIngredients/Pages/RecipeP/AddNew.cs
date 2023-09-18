using RecipesAndIngredients.DTO;
using RecipesAndIngredients.Services;

namespace RecipesAndIngredients.Pages.RecipeP
{
    public partial class RecipePage
    {
        public static void AddNewRecipe(RecipeService recipeService, IngredientService ingredientService)
        {
            Dictionary<int, IngredientAndQuantityDto> ingredients = new Dictionary<int, IngredientAndQuantityDto>();
            RecipeDto recipeDto = new RecipeDto();

            Console.WriteLine("Создание нового рецепта");
            while (true)
            {
                Console.WriteLine("Введите название рецепта");
                string recName = Utils.GetAndValidateNullString();
                if (recipeService.CheckExistanceByName(recName) == true)
                {
                    Console.WriteLine($"Рецепт с названием {recName} уже создан");
                    continue;
                }
                recipeDto.RecName = recName;
                break;
            }
            Console.WriteLine("Выберите одну из категорий блюда");

            List<RecipeCategoryDto> categoryList = recipeService.GetAllCategory();
            int countCategory = categoryList.Count;  /// .Count возвращает число записей списка
            for (int i = 1; i <= countCategory; i++)
                Console.WriteLine($"{i} - {categoryList[i - 1].CategName}");   /// [] позволяют обращаться по индексу (количество строк)
                                                                               /// обращаясь по индексу порядок начинается с 0, поэтому указываем i - 1  

            while (true)///здесь желательно избавиться от цикла, но на данный момент неизбежно излишнее использование циклов внутри кода
            {
                Console.WriteLine("Введите цифру категории");

                int category = Utils.GetAndValidateNullInt();
                ///RecipeCategoryDto recipeCategoryDto = categoryList[category - 1];
                RecipeCategoryDto? recipeCategoryDto = categoryList.ElementAtOrDefault(category - 1); /// ElementAtOrDefault позволяет обратиться к выбранной записи по индексу(аналог предыдущей строчки)
                                                                                                      /// также как и FirstOrDefault возвращает либо элемент, либо null
                if (recipeCategoryDto == null)
                {
                    Console.WriteLine("Неверная цифра");
                    continue;
                }
                recipeDto.Category = recipeCategoryDto;
                break;
            }
            while (true)
            {
                Console.WriteLine("Введите название ингредиента");

                string? ingName = Utils.GetAndValidateNullString();
                if (ingredientService.CheckExistanceByName(ingName) == false)
                {
                    Console.WriteLine($"Ингредиента с названием {ingName} не существует");
                    continue;
                }

                IngredientDto ingredientDto = ingredientService.GetByNameDto(ingName);
                if (ingredients.ContainsKey(ingredientDto.Id) == true)
                {
                    Console.WriteLine($"Ингредиент {ingredientDto.IngName} уже добавлен");
                    continue;
                }

                Console.WriteLine($"Введите количество ингредиента ({ingredientDto.QuantityType.Name})");

                int quantity = Utils.GetAndValidateNullInt();
                IngredientAndQuantityDto ingredientAndQuantityDto = new IngredientAndQuantityDto()
                {
                    QuantityCount = quantity,
                    Ingredient = ingredientDto,
                };
                ingredients.Add(ingredientDto.Id, ingredientAndQuantityDto); /// для dictionary писать команду distinct не нужно, т.к. он сам проверяет уникальность записи в коллекции по Key

                Console.WriteLine("Желаете ли продолжить добавление ингредиента?");
                Console.WriteLine("Да - Y   Нет - N");

                ConsoleKey key = Console.ReadKey().Key;
                if (key == ConsoleKey.Y)
                    continue;

                recipeDto.Ingredients = ingredients;
                break;
            }
            recipeService.AddRecipe(recipeDto);

            Console.WriteLine("Создан новый рецепт");
            Console.WriteLine($"{recipeDto.RecName}, {recipeDto.Category.CategName}: ");
            foreach (var ingredient in ingredients)
                Console.WriteLine($"{ingredient.Value.Ingredient.IngName} - {ingredient.Value.QuantityCount}, {ingredient.Value.Ingredient.QuantityType.Name}");
        }
    }
}

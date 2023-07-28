using System;
using System.Collections.Generic;

namespace RecipesAndIngredients.Models;

public partial class RecipeCategory
{
    public int Id { get; set; }

    public string? CategName { get; set; }

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}

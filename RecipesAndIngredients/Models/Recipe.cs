using System;
using System.Collections.Generic;

namespace RecipesAndIngredients.Models;

public partial class Recipe
{
    public Guid Id { get; set; }

    public string RecName { get; set; } = null!;

    public int? RecipeCategoryId { get; set; }

    public virtual RecipeCategory? RecipeCategory { get; set; }

    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}

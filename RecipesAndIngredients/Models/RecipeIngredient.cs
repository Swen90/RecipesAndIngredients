using System;
using System.Collections.Generic;

namespace RecipesAndIngredients.Models;

public partial class RecipeIngredient
{
    public int IngredientId { get; set; }

    public Guid RecipeId { get; set; }

    public int? QuantityCount { get; set; }

    public virtual Ingredient Ingredient { get; set; } = null!;

    public virtual Recipe Recipe { get; set; } = null!;
}

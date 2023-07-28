using System;
using System.Collections.Generic;

namespace RecipesAndIngredients.Models;

public partial class Ingredient
{
    public int Id { get; set; }

    public string IngName { get; set; } = null!;

    public int? QuantityTypeId { get; set; }

    public virtual QuantityType? QuantityType { get; set; }

    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}

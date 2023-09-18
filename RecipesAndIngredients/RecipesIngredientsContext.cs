using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RecipesAndIngredients.Models;

namespace RecipesAndIngredients;

public partial class RecipesIngredientsContext : DbContext
{
    public RecipesIngredientsContext()
    {
    }

    public RecipesIngredientsContext(DbContextOptions<RecipesIngredientsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<QuantityType> QuantityTypes { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RecipeCategory> RecipeCategories { get; set; }

    public virtual DbSet<RecipeIngredient> RecipeIngredients { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-3CP9MR9;Database=RecipesIngredients;Trusted_Connection=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ingredie__3214EC07AAE9A151");

            entity.ToTable("Ingredient");

            entity.Property(e => e.IngName).HasMaxLength(20);

            entity.HasOne(d => d.QuantityType).WithMany(p => p.Ingredients)
                .HasForeignKey(d => d.QuantityTypeId)
                .HasConstraintName("FK__Ingredien__Quant__286302EC");
        });

        modelBuilder.Entity<QuantityType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quantity__3214EC07FE3364EE");

            entity.ToTable("QuantityType");

            entity.Property(e => e.Quantity).HasMaxLength(15);
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recipe__3214EC071F44CB93");

            entity.ToTable("Recipe");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RecName).HasMaxLength(20);

            entity.HasOne(d => d.RecipeCategory).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.RecipeCategoryId)
                .HasConstraintName("FK__Recipe__RecipeCa__2B3F6F97");
        });

        modelBuilder.Entity<RecipeCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RecipeCa__3214EC07421F420A");

            entity.ToTable("RecipeCategory");

            entity.Property(e => e.CategName).HasMaxLength(15);
        });

        modelBuilder.Entity<RecipeIngredient>(entity =>
        {
            entity.HasKey(e => new { e.IngredientId, e.RecipeId }).HasName("PK__RecipeIn__A1732AD1BA960682");

            entity.ToTable("RecipeIngredient");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RecipeIng__Ingre__2E1BDC42");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RecipeIng__Recip__2F10007B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

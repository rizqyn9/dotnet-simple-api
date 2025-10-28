using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using SampleApi.Extensions;

namespace SampleApi.Data.Extensions
{
  public static class ModelBuilderExtensions
  {
    /// <summary>
    /// Convert all table, column, and key names to snake_case.
    /// </summary>
    public static void UseSnakeCaseNames(this ModelBuilder builder)
    {
      foreach (var entity in builder.Model.GetEntityTypes())
      {
        // Table name
        var tableName = entity.GetTableName();
        if (!string.IsNullOrEmpty(tableName))
          entity.SetTableName(tableName.ToSnakeCase());

        // Columns
        foreach (var property in entity.GetProperties())
        {
          var columnName = property.GetColumnName(StoreObjectIdentifier.Table(tableName!, null));
          if (!string.IsNullOrEmpty(columnName))
            property.SetColumnName(columnName.ToSnakeCase());
        }

        // Keys
        foreach (var key in entity.GetKeys())
          key.SetName(key.GetName()?.ToSnakeCase());

        // Foreign keys
        foreach (var fk in entity.GetForeignKeys())
          fk.SetConstraintName(fk.GetConstraintName()?.ToSnakeCase());

        // Indexes
        foreach (var index in entity.GetIndexes())
          index.SetDatabaseName(index.GetDatabaseName()?.ToSnakeCase());
      }
    }
  }
}

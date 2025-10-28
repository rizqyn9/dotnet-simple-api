using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using SampleApi.Extensions;

namespace SampleApi.Data.Converters
{
  public class SnakeCaseEnumConverter<TEnum> : ValueConverter<TEnum, string>
      where TEnum : struct, Enum
  {
    public SnakeCaseEnumConverter() :
        base(
            v => v.ToString().ToSnakeCase(), // Convert enum -> snake_case string
            v => Enum.Parse<TEnum>(v.Replace("_", ""), true) // Convert back to enum
        )
    { }
  }
}

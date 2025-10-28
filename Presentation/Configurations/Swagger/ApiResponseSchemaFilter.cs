using Microsoft.OpenApi.Models;

using SampleApi.Presentation.Common.Responses;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace SampleApi.Presentation.Configurations.Swagger
{
  public class ApiResponseSchemaFilter : ISchemaFilter
  {
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
      // Map generic ApiResponse<T>
      if (context.Type.IsGenericType && context.Type.GetGenericTypeDefinition() == typeof(ApiResponse<>))
      {
        schema.Properties.Clear();

        schema.Properties["success"] = new OpenApiSchema { Type = "boolean", Example = new Microsoft.OpenApi.Any.OpenApiBoolean(true) };
        schema.Properties["message"] = new OpenApiSchema { Type = "string", Example = new Microsoft.OpenApi.Any.OpenApiString("Request successful") };
        schema.Properties["code"] = new OpenApiSchema { Type = "string", Nullable = true, Example = new Microsoft.OpenApi.Any.OpenApiString("SUCCESS") };
        schema.Properties["data"] = new OpenApiSchema { Type = "object", Nullable = true };
      }

      // Map ErrorResponse
      if (context.Type == typeof(ErrorResponse))
      {
        schema.Properties.Clear();

        schema.Properties["success"] = new OpenApiSchema { Type = "boolean", Example = new Microsoft.OpenApi.Any.OpenApiBoolean(false) };
        schema.Properties["message"] = new OpenApiSchema { Type = "string", Example = new Microsoft.OpenApi.Any.OpenApiString("Validation failed") };
        schema.Properties["code"] = new OpenApiSchema { Type = "string", Example = new Microsoft.OpenApi.Any.OpenApiString("NAME_CONFLICT") };
        schema.Properties["fields"] = new OpenApiSchema
        {
          Type = "object",
          AdditionalProperties = new OpenApiSchema { Type = "array", Items = new OpenApiSchema { Type = "string" } },
          Description = "Field-level validation errors (from FluentValidation)"
        };
      }
    }
  }
}

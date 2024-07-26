namespace MicroserviceTemplate.CrossCutting.Extensions;

public static class ReflectionExtensions
{
    public static bool HasGenericParentType(this Type? typeToCheck, Type parentType)
    {
        // parent type is not a generic type i.e. not Something<> or ISomething<>
        if (!parentType.IsGenericType)
            throw new ArgumentException("Parent type must be generic", nameof(parentType));

        // we have reached the end of the line
        if (typeToCheck == null || typeToCheck == typeof(object))
            return false;

        if (typeToCheck.IsGenericType && typeToCheck.GetGenericTypeDefinition() == parentType)
            return true;

        return typeToCheck.BaseType.HasGenericParentType(parentType) || typeToCheck.GetInterfaces().Any(t => t.HasGenericParentType(parentType));
    }

}
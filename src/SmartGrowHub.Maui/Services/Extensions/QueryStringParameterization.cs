namespace SmartGrowHub.Maui.Services.Extensions;

public static class QueryStringParameterization
{
    public static string AppendQueryParameter<T>(this string query, string parameterName, T? value)
    {
        if (value is null) return query;
        char separator = GetFirstSeparator(query);
        return $"{query}{separator}{parameterName}={value}";
    }

    private static char GetFirstSeparator(string query) => query.Contains('?') ? '&' : '?';
}
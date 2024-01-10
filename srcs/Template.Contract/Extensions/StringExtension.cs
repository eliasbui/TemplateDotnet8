namespace Template.Contract.Extensions;

public static class StringExtension
{
    public static string ListToArrayString(this List<string>? inputList)
    {
        if (inputList == null || !inputList.Any()) return "''";
        return string.Join(",", inputList.Select(x => "'" + x + "'"));
    }
    public static string ListToArrayString(this List<Guid>? inputList)
    {
        if (inputList == null || !inputList.Any()) return "''";
        return string.Join(",", inputList.Select(x => "'" + x + "'"));
    }
}

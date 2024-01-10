using System.Text;

namespace Template.Contract.Utility;

public static class StringUtility
{
    public static string GenerateRandomUppercase(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var random = new Random();

        var randomText = new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());

        return randomText;
    }
    public static string CreateMd5(string input)
    {
        // Use input string to calculate MD5 hash
        using var md5 = System.Security.Cryptography.MD5.Create();
        var inputBytes = Encoding.ASCII.GetBytes(input);
        var hashBytes = md5.ComputeHash(inputBytes);

        // Convert the byte array to hexadecimal string
        var sb = new StringBuilder();
        foreach (var t in hashBytes)
        {
            sb.Append(t.ToString("X2"));
        }
        return sb.ToString();
    }
    public static string GetMac<T>(T inputModel, string? secretKey)
    {
        if (inputModel == null)
        {
            return string.Empty;
        }

        var listKey = (from propertyInfo
                    in inputModel.GetType().GetProperties()
                let value = propertyInfo.GetValue(inputModel, null)
                where value != null
                      && !propertyInfo.Name.Equals("mac_type",StringComparison.CurrentCultureIgnoreCase)
                      && !propertyInfo.Name.Equals("mac", StringComparison.CurrentCultureIgnoreCase)
                select propertyInfo.Name)
            .ToList();

        var orderedEnumerable = listKey.OrderBy(x => x).ToList();
        var listKeyValue = orderedEnumerable.Select(key => $"{key}={inputModel.GetType().GetProperty(key)?.GetValue(inputModel, null)}").ToList();

        var inputHash = string.Join("&", listKeyValue);
        return CreateMd5($"{secretKey}{inputHash}");
    }

    public static bool VerifyMac<T>(T? targetModel, string? secretKey, string? targetMac)
    {
        if (string.IsNullOrEmpty(targetMac) || string.IsNullOrEmpty(secretKey) || targetModel == null) return false;
        var mac = GetMac(targetModel, secretKey);
        return mac.Equals(targetMac, StringComparison.CurrentCultureIgnoreCase);
    }
}

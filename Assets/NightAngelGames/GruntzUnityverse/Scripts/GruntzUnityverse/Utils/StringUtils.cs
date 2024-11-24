using System.Text.RegularExpressions;


namespace GruntzUnityverse.Utils {
public static class StringUtils {
    public static string CamelCaseToSpaced(string input) {
        return Regex.Replace(input, "(?<!^)([A-Z])", " $1");
    }
}
}

using System.Globalization;

namespace Pennymatch.Web.Localization;

/// <summary>Supported UI cultures. Value-type key for AOT-friendly dictionary lookups (no reflection).</summary>
public readonly record struct UiCulture(string Code) : IEquatable<UiCulture>
{
    public static readonly UiCulture Es = new("es");
    public static readonly UiCulture En = new("en");

    public static readonly string[] SupportedCodes = ["es", "en"];

    public string ActiveLabel => Code switch
    {
        "en" => "ENG",
        _ => "ESP"
    };

    public string NativeName => Code switch
    {
        "en" => "English",
        _ => "Español"
    };

    public static UiCulture From(CultureInfo? culture)
    {
        var two = culture?.TwoLetterISOLanguageName;
        return string.Equals(two, "en", StringComparison.OrdinalIgnoreCase) ? En : Es;
    }

    public static UiCulture ParseOrDefault(string? code)
    {
        if (string.Equals(code, "en", StringComparison.OrdinalIgnoreCase))
        {
            return En;
        }

        return Es;
    }

    public CultureInfo ToCultureInfo() =>
        Code switch
        {
            "en" => CultureInfo.GetCultureInfo("en"),
            _ => CultureInfo.GetCultureInfo("es")
        };

    public override string ToString() => Code;
}

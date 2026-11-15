namespace Pennymatch.Web.Localization;

public interface ISiteLocalizer
{
    UiCulture Culture { get; }

    string this[string key] { get; }

    string T(string key);
}

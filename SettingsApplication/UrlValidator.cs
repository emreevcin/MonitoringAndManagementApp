using System;

namespace SettingsApplication
{
    public class UrlValidator : IValidator<string>
    {
        public bool Validate(string url)
        {
            Uri resultUri;
            return Uri.TryCreate(url, UriKind.Absolute, out resultUri);
        }
    }
}

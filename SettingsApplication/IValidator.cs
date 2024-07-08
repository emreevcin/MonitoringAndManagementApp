namespace SettingsApplication
{
    public interface IValidator<T>
    {
        bool Validate(T input);
    }
}

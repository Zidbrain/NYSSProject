namespace MobApp;

public interface IAlert
{
    void Show(string message);
}

public interface IFilePickerService
{
    Task<string> PickForSaving(MainPage display);
    Task<string> PickForOpenning(MainPage display);
}

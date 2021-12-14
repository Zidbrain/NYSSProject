namespace NYSSProject;

public interface IAlert
{
    void Show(string message);
}

public interface IFilePickerService
{
    Task<(Stream stream, string fileName)> PickForSaving(MainPage display);
    Task<(Stream stream, string fileName)> PickForOpenning(MainPage display);
}

using Android.Widget;
using Android.App;
using System;
using System.IO;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;

[assembly: Dependency(typeof(NYSSProject.Droid.Alert))]
[assembly: Dependency(typeof(NYSSProject.Droid.FilePicker))]

namespace NYSSProject.Droid
{
    public class Alert : IAlert
    {
        public void Show(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }

    public class FilePicker : IFilePickerService
    {
        public async Task<(Stream stream, string fileName)> PickForSaving(MainPage display)
        {
            var to = await display.DisplayActionSheet("Сохранить как...", "Отмена", null, ".docx", ".txt");

            if (to == "Отмена")
                return (null, null);

            var result = await display.DisplayPromptAsync("Введите назание фалйа", "Файл будет сохранён в локальное хранилище приложения");

            if (result != null)
            {
                result += to;
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), result);

                if (!File.Exists(path) || await display.DisplayAlert("Внимание", $"Файл с именем {result} уже существует\nПерезаписать файл?", "Да", "Нет"))
                    return (File.Create(path), result);
            }

            return (null, null);
        }

        public async Task<(Stream stream, string fileName)> PickForOpenning(MainPage display)
        {
            var from = await display.DisplayActionSheet("Открыть файл из...", "Отмена", null, "Внешнего хранилища", "Локального хранилища приложения");

            string result = null;
            if (from == "Внешнего хранилища")
            {
                result = (await Xamarin.Essentials.FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>()
                        {
                            { DevicePlatform.Android, new[] { "text/plain", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" } },
                        }),
                    PickerTitle = "Выберите файл с текстом"
                }))?.FullPath;
            }
            else if (from != "Отмена")
            {
                var list = new FileOpenList();
                var source = new TaskCompletionSource<string>();

                void Set(object s, EventArgs e) => source.SetResult(list.SelectedFile);
                list.Disappearing += Set;

                await display.Navigation.PushAsync(list);

                var selected = await source.Task;
                if (selected != null)
                    result = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), selected);

                list.Disappearing -= Set;
            }

            if (result is null)
                return (null, null);

            return (File.OpenRead(result), Path.GetFileName(result));
        }
    }
}
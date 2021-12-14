using Microsoft.Toolkit.Uwp.Notifications;
using NYSSProject;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.Collections.Generic;
using System.Windows;
using Windows.Storage.Pickers;
using System;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(NYSSProject.UWP.Alert))]
[assembly: Xamarin.Forms.Dependency(typeof(NYSSProject.UWP.FilePicker))]

namespace NYSSProject.UWP
{
    public class Alert : IAlert
    {
        public void Show(string message) =>
            new ToastContentBuilder().AddText(message).Show();
    }

    public class FilePicker : IFilePickerService
    {
        public async Task<(Stream stream, string fileName)> PickForOpenning(NYSSProject.MainPage display)
        {
            var dialog = new FileOpenPicker();
            dialog.FileTypeFilter.Add(".txt");
            dialog.FileTypeFilter.Add(".docx");

            var res = (await dialog.PickSingleFileAsync());
            return (await res?.OpenStreamForReadAsync(), res?.Name);
        }

        public async Task<(Stream stream, string fileName)> PickForSaving(NYSSProject.MainPage display)
        {
            var dialog = new FileSavePicker();
            dialog.FileTypeChoices.Add("Текст", new List<string>() { ".txt" });
            dialog.FileTypeChoices.Add("Файл Microsoft Word", new List<string>() { ".docx" });
            dialog.SuggestedFileName = "Result";

            var res = await dialog.PickSaveFileAsync();

            return (await res?.OpenStreamForWriteAsync(), res.Name);
        }
    }
}

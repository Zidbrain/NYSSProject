using Microsoft.Toolkit.Uwp.Notifications;
using MobApp;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.Collections.Generic;
using System.Windows;
using Windows.Storage.Pickers;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(MobApp.UWP.Alert))]
[assembly: Xamarin.Forms.Dependency(typeof(MobApp.UWP.FilePicker))]

namespace MobApp.UWP
{
    public class Alert : IAlert
    {
        public void Show(string message) =>
            new ToastContentBuilder().AddText(message).Show();
    }

    public class FilePicker : IFilePickerService
    {
        public async Task<string> PickForOpenning(MobApp.MainPage display)
        {
            var dialog = new FileOpenPicker();
            dialog.FileTypeFilter.Add(".txt");
            dialog.FileTypeFilter.Add(".docx");

            return (await dialog.PickSingleFileAsync())?.Path;
        }

        public async Task<string> PickForSaving(MobApp.MainPage display)
        {
            var dialog = new FileSavePicker();
            dialog.FileTypeChoices.Add("Текст", new List<string>() { ".txt" });
            dialog.FileTypeChoices.Add("Файл Microsoft Word", new List<string>() { ".docx" });
            dialog.SuggestedFileName = "Result";

            return (await dialog.PickSaveFileAsync())?.Path;
        }
    }
}

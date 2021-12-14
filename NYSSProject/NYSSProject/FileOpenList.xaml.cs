using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NYSSProject;
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class FileOpenList : ContentPage
{
    public IEnumerable<string> Items { get; set; }

    public string SelectedFile { get; private set; }

    public FileOpenList()
    {
        InitializeComponent();

        Items = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)).Select(x => Path.GetFileName(x));

        MyListView.ItemsSource = Items;
    }

    async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item == null)
            return;

        SelectedFile = e.Item as string;
        await Navigation.PopAsync();

        ((ListView)sender).SelectedItem = null;
    }

    private void Delete(object sender, EventArgs e)
    {
        var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var path = Path.Combine(folder, (sender as MenuItem).BindingContext as string);
        File.Delete(path);

        Items = Directory.GetFiles(folder).Select(x => Path.GetFileName(x));
        MyListView.ItemsSource = Items;
    }
}

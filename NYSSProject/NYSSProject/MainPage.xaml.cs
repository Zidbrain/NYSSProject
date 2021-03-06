using Xamarin.Forms;

namespace NYSSProject;

public partial class MainPage : ContentPage
{
    public CypherViewModel ViewModel { get; set; }

    public MainPage()
    {
        ViewModel = new CypherViewModel(this);

        InitializeComponent();
    }
}

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;

namespace MobApp;

[ContentProperty(nameof(Source))]
public class ImageResource : IMarkupExtension
{
    public string Source { get; set; }

    public object ProvideValue(IServiceProvider serviceProvider) => 
        ImageSource.FromResource(Source, typeof(ImageResource).Assembly);
}

public class KeyConverter : IValueConverter, INotifyPropertyChanged
{
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public event PropertyChangedEventHandler PropertyChanged;

    private bool _hasErrors;
    public bool HasErrors
    {
        get => _hasErrors;
        private set
        {
            if (_hasErrors != value)
            {
                _hasErrors = value;
                OnPropertyChanged();
            }
        }
    }

    private string _errorMessage;
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value;
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (string.IsNullOrEmpty(value as string))
        {
            ErrorMessage = "Отсутствует ключ";
            HasErrors = true;
            return Binding.DoNothing;
        }

        var regex = new Regex(@"^[а-яА-ЯёЁ]*$");
        if (regex.IsMatch(value as string))
        {
            HasErrors = false;
            return value;
        }
        else
        {
            ErrorMessage = "Ключ содержит недопустимые символы";
            HasErrors = true;
            return Binding.DoNothing;
        }
    }
}
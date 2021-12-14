using NPOI.XWPF.UserModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobApp;

public enum ConversionType
{
    Cypher,
    Decypher
}

public class CypherViewModel : INotifyPropertyChanged
{
    private readonly CypherProvider _cypherProvider = new("скорпион");
    private CancellationTokenSource _tokenSource = new();
    private readonly MainPage _mainPage;

    public CypherViewModel(MainPage page)
    {
        _mainPage = page;

        SwapCommand = new Command(() =>
        {
            _conversionType = _conversionType is ConversionType.Cypher ? ConversionType.Decypher : ConversionType.Cypher;
            (_text, _convertedText) = (_convertedText, _text);

            OnPropertyChanged(nameof(ConversionType));
            OnPropertyChanged(nameof(Text));
            OnPropertyChanged(nameof(ConvertedText));
        });

        OpenCommand = new Command(async () =>
        {
            try
            {
                var result = await DependencyService.Resolve<IFilePickerService>().PickForOpenning(_mainPage);
                if (result is not null)
                {
                    _conversionType = ConversionType.Decypher;
                    Text = await ReadFromFileAsync(result);
                    OnPropertyChanged(nameof(ConversionType));

                    DependencyService.Resolve<IAlert>().Show($"Файл открыт");
                }
            }
            catch (Exception)
            {
                DependencyService.Resolve<IAlert>().Show("Произошла ошибка");
            }
        });

        SaveCommand = new Command(async () =>
        {
            try
            {
                var path = await DependencyService.Resolve<IFilePickerService>().PickForSaving(_mainPage);
                if (path is not null)
                {
                    await SaveToFileAsync(path, ConvertedText);
                    DependencyService.Resolve<IAlert>().Show($"Файл сохранён в:{path}");
                }
            }
            catch (Exception)
            {
                DependencyService.Resolve<IAlert>().Show("Произошла ошибка");
            }
        });
    }

    public static async Task<string> ReadFromFileAsync(string path) =>
        await Task.Run(() =>
        {
            if (path.EndsWith(".txt"))
                return File.ReadAllText(path);

            using var stream = File.OpenRead(path);
            var document = new XWPFDocument(stream);

            var result = new StringBuilder();
            for (int i = 0; i < document.Paragraphs.Count; i++)
            {
                foreach (var run in document.Paragraphs[i].Runs)
                {
                    result.Append(run.Text);
                }

                if (i != document.Paragraphs.Count - 1)
                    result.Append('\n');
            }

            return result.ToString();
        });

    public static async Task SaveToFileAsync(string path, string text) =>
        await Task.Run(() =>
        {
            if (path.EndsWith(".txt"))
            {
                File.WriteAllText(path, text);
                return;
            }

            using var stream = File.Create(path);
            var document = new XWPFDocument();
            foreach (var line in text.Split('\n'))
            {
                var paragraph = document.CreateParagraph();
                paragraph.CreateRun().SetText(line);
            }
            document.Write(stream);
        });

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private async void ConvertText()
    {
        if ((_mainPage.Resources["KeyConverter"] as KeyConverter).HasErrors)
            return;

        try
        {
            _tokenSource.Cancel();
            _tokenSource = new CancellationTokenSource();

            string result;
            if (ConversionType == ConversionType.Cypher)
                result = await _cypherProvider.CypherAsync(_text, _tokenSource.Token);
            else
                result = await _cypherProvider.DecypherAsync(_text, _tokenSource.Token);

            ConvertedText = result;
        }
        catch (OperationCanceledException) { }
    }

    public ICommand SwapCommand { get; private set; }
    public ICommand OpenCommand { get; private set; }
    public ICommand SaveCommand { get; private set; }

    private string _text;
    public string Text
    {
        get => _text;
        set
        {
            if (_text != value)
            {
                _text = value;
                OnPropertyChanged();

                ConvertText();
            }
        }
    }

    private ConversionType _conversionType;
    public ConversionType ConversionType
    {
        get => _conversionType;
        set
        {
            if (_conversionType != value)
            {
                _conversionType = value;
                OnPropertyChanged();

                ConvertText();
            }
        }
    }

    private string _convertedText;
    public string ConvertedText
    {
        get => _convertedText;
        set
        {
            if (_convertedText != value)
            {
                if (value == string.Empty)
                    value = null;

                _convertedText = value;
                OnPropertyChanged();
            }
        }
    }

    public string Key
    {
        get => _cypherProvider.Key;
        set
        {
            if (_cypherProvider.Key != value)
            {
                _cypherProvider.Key = value;
                OnPropertyChanged();

                ConvertText();
            }
        }
    }
}

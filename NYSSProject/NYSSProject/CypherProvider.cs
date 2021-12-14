using System.Text;
using Xamarin.Forms;
using System.Text.RegularExpressions;

namespace NYSSProject;

public class CypherProvider
{
    private string _key;
    public string Key
    {
        get => _key;
        set
        {
            if (value == null)
                throw new ArgumentNullException("value");

            foreach (var c in value)
                if (!TryGetIndex(c, out _))
                    throw new ArgumentException("Ключ содержит недопустимые символы", nameof(value));
            _key = value;
        }
    }

    public CypherProvider(string key) =>
        Key = key;

    private bool TryGetIndex(char value, out int index)
    {
        var c = char.ToLower(value);

        index = -1;
        if (c >= 'а' && c <= 'е')
            index = c - 'а';
        else if (c == 'ё')
            index = 'е' - 'а' + 1;
        else if (c >= 'е' && c <= 'я')
            index = c - 'а' + 1;
        else return false;

        return true;
    }

    private char GetCharByIndex(int index)
    {
        if (index <= 5)
            return (char)('а' + index);
        if (index == 6)
            return 'ё';
        else
            return (char)('а' + index - 1);
    }

    private string Cypher(string value, CancellationToken cancellationToken)
    {
        if (value is null)
            return null;

        var result = new StringBuilder(value.Length);
        var charIndex = 0;

        for (int i = 0; i < value.Length; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (TryGetIndex(value[i], out var index))
            {
                var wasUpper = char.IsUpper(value[i]);

                TryGetIndex(Key[charIndex % Key.Length], out var keyIndex);

                var resChar = GetCharByIndex((index + keyIndex) % 33);
                if (wasUpper)
                    resChar = char.ToUpper(resChar);

                result.Append(resChar);
                charIndex++;
            }
            else
                result.Append(value[i]);
        }

        return result.ToString();
    }

    public string Cypher(string value) => Cypher(value, CancellationToken.None);
    public async Task<string> CypherAsync(string value, CancellationToken cancellationToken) =>
        await Task.Run(() => Cypher(value, cancellationToken));

    private string Decypher(string value, CancellationToken cancellationToken)
    {
        if (value is null)
            return null;

        var result = new StringBuilder(value.Length);
        var charIndex = 0;

        for (int i = 0; i < value.Length; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (TryGetIndex(value[i], out var index))
            {
                var wasUpper = char.IsUpper(value[i]);

                TryGetIndex(Key[charIndex % Key.Length], out var keyIndex);

                var atIndex = (index - keyIndex) % 33;
                if (atIndex < 0)
                    atIndex = 33 + atIndex;

                var resChar = GetCharByIndex(atIndex);
                if (wasUpper)
                    resChar = char.ToUpper(resChar);

                result.Append(resChar);
                charIndex++;
            }
            else
                result.Append(value[i]);
        }

        return result.ToString();
    }

    public string Decypher(string value) => Decypher(value, CancellationToken.None);

    public async Task<string> DecypherAsync(string value, CancellationToken cancellationToken) =>
        await Task.Run(() => Decypher(value, cancellationToken));
}

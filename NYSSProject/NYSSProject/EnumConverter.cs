using System;
using Xamarin.Forms;
using System.Globalization;

namespace NYSSProject;

[ContentProperty(nameof(EnumType))]
public class EnumConverter : IValueConverter
{
    public Type EnumType { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter == null)
            throw new ArgumentException("Null parameter", nameof(parameter));

        if (parameter.GetType() != EnumType)
            throw new ArgumentException("Wrong argument type", nameof(parameter));

        return value?.Equals(parameter);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter == null)
            throw new ArgumentException("Null parameter", nameof(parameter));

        if (parameter.GetType() != EnumType)
            throw new ArgumentException("Wrong argument type", nameof(parameter));

        return (value?.Equals(true) ?? false) ? parameter : Binding.DoNothing;
    }
}

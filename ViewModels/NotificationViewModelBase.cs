using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SampleApp.ViewModels;
public abstract class NotificationViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(PropertyChangedEventArgs? args)
    {
        if (args is null)
        {
            ArgumentNullException.ThrowIfNull(args, nameof(args));
        }

        var propertyChanged = PropertyChanged;
        propertyChanged?.Invoke(this, args);
    }

    protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        return SetProperty(ref field, value, new PropertyChangedEventArgs(propertyName));
    }

    protected virtual bool SetProperty<T>(ref T field, T value, PropertyChangedEventArgs? args)
    {
        if (args is null)
        {
            ArgumentNullException.ThrowIfNull(args, nameof(args));
        }

        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(args);

        return true;
    }
}

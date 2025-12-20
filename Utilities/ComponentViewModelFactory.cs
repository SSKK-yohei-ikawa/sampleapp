using System.Windows.Input;
using SampleApp.ViewModels.ComponentViewModels;

namespace SampleApp.Utilities;
public static class ComponentViewModelFactory
{
    public static IComponentViewModel CreateInstance(string name, string description, object value, ICommand removeCommand)
    {
        return value switch
        {
            string stringValue => new StringComponentViewModel(name, description, stringValue, removeCommand),
            int intValue => new IntComponentViewModel(name, description, intValue, removeCommand),
            float floatValue => new FloatComponentViewModel(name, description, floatValue, removeCommand),
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
}

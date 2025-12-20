using System.Windows.Input;

namespace SampleApp.ViewModels.ComponentViewModels;
public abstract class ComponentViewModelBase<T>(string name, string description, T value, ICommand removeCommand) : NotificationViewModelBase, IComponentViewModel
{
    private T _value = value;

    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; } = name;
    public string Description { get; } = description;

    public T Value
    {
        get => _value;
        set => SetProperty(ref _value, value);
    }

    public ICommand RemoveCommand { get; } = removeCommand;
}

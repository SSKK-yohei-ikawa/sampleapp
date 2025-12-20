using System.Windows.Input;

namespace SampleApp.ViewModels.ComponentViewModels;
public interface IComponentViewModel
{
    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }

    public ICommand RemoveCommand { get; }
}

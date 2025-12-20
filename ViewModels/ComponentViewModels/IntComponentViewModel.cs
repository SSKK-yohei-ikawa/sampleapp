using System.Windows.Input;

namespace SampleApp.ViewModels.ComponentViewModels;
public class IntComponentViewModel(string name, string description, int value, ICommand removeCommand)
    : ComponentViewModelBase<int>(name, description, value, removeCommand);

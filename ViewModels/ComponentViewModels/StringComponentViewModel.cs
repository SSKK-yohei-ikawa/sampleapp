using System.Windows.Input;

namespace SampleApp.ViewModels.ComponentViewModels;

public class StringComponentViewModel(string name, string description, string value, ICommand removeCommand)
    : ComponentViewModelBase<string>(name, description, value, removeCommand);
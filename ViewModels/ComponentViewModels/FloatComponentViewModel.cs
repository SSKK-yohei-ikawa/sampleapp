using System.Windows.Input;

namespace SampleApp.ViewModels.ComponentViewModels;

public class FloatComponentViewModel(string name, string description, float value, ICommand removeCommand)
    : ComponentViewModelBase<float>(name, description, value, removeCommand);
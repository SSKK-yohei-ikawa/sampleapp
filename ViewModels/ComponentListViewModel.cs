using Reactive.Bindings;
using Reactive.Bindings.Disposables;
using Reactive.Bindings.Extensions;
using SampleApp.Utilities;
using SampleApp.ViewModels.ComponentViewModels;
using SampleApp.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace SampleApp.ViewModels;
public class ComponentListViewModel : IDisposable
{
    private readonly CompositeDisposable _compositeDisposable = new();

    private readonly ObservableCollection<IComponentViewModel> _components = [];


    public ComponentListViewModel()
    {
        ComponentsView = CollectionViewSource.GetDefaultView(_components);
        ComponentsView.Filter = ComponentViewFilter;

        _ = Filter.Subscribe(_ =>
        {
            ComponentsView.Refresh();
        }).AddTo(_compositeDisposable);

        AddComponentCommand = new ReactiveCommandSlim<ComponentElement>().WithSubscribe(AddComponent).AddTo(_compositeDisposable);
        RemoveComponentCommand = new ReactiveCommandSlim<Guid?>().WithSubscribe(RemoveComponent).AddTo(_compositeDisposable);
        OpenComponentCreationDialogCommand = new ReactiveCommandSlim().WithSubscribe(OpenComponentCreationDialog).AddTo(_compositeDisposable);
        CreateDummyComponentsCommand = new ReactiveCommandSlim().WithSubscribe(() => DummyComponentFactory.CreateDummyComponents(10, _components.Select(component => component.Name).ToHashSet(), AddComponentCommand)).AddTo(_compositeDisposable);
        ResetFilterCommand = new ReactiveCommandSlim().WithSubscribe(() => Filter.Value = string.Empty).AddTo(_compositeDisposable);

        foreach (var componentElement in DummyComponentFactory.CreateDummyComponentElementsOnInit())
        {
            AddComponentCommand.Execute(componentElement);
        }
    }


    public ICollectionView ComponentsView { get; }

    public ReactiveProperty<string> Filter { get; } = new(string.Empty);

    public ICommand AddComponentCommand { get; }
    public ICommand RemoveComponentCommand { get; }

    public ICommand OpenComponentCreationDialogCommand { get; }
    public ICommand CreateDummyComponentsCommand { get; }

    public ICommand ResetFilterCommand { get; }


    public void Dispose()
    {
        _compositeDisposable.Dispose();

        Filter.Dispose();

        GC.SuppressFinalize(this);
    }

    private void AddComponent(ComponentElement componentElem)
    {
        var (name, description, value) = componentElem;
        _components.Add(ComponentViewModelFactory.CreateInstance(name, description, value, RemoveComponentCommand));
    }

    private void RemoveComponent(Guid? id)
    {
        var removeComponent = _components.FirstOrDefault(component => component.Id == id);
        if (removeComponent is not null)
        {
            _ = _components.Remove(removeComponent);
        }

    }

    private void OpenComponentCreationDialog()
    {
        var componentCreationViewModel = new ComponentCreationViewModel(_components.Select(component => component.Name).ToHashSet());
        var componentCreationDialog = new ComponentCreationDialog(componentCreationViewModel) { Owner = Application.Current.MainWindow };

        if (componentCreationDialog.ShowDialog() is true)
        {
            AddComponentCommand.Execute(componentCreationViewModel.GetResult());
        }
    }

    private bool ComponentViewFilter(object obj)
    {
        return obj is IComponentViewModel component && component.Name.Contains(Filter.Value, StringComparison.InvariantCultureIgnoreCase);
    }
}

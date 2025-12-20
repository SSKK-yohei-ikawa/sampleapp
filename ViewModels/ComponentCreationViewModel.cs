using Reactive.Bindings;
using Reactive.Bindings.Disposables;
using Reactive.Bindings.Extensions;
using SampleApp.Resources;
using SampleApp.Types;
using SampleApp.Utilities;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reactive.Linq;

namespace SampleApp.ViewModels;
public class ComponentCreationViewModel : IDisposable
{
    private readonly CompositeDisposable _compositeDisposable = new();

    private readonly HashSet<string> _registeredComponentNames;


    public ComponentCreationViewModel(HashSet<string> registeredComponentNames)
    {
        _registeredComponentNames = registeredComponentNames;

        _ = CanCreateComponent.Subscribe(canCreateComponent =>
        {
            IsWarningVisible.Value = !canCreateComponent;
        }).AddTo(_compositeDisposable);

        _ = Name.CombineLatest(SelectedType, ValueText).Subscribe(_ =>
        {
            CanCreateComponent.Value = CheckCanCreateComponent();
        }).AddTo(_compositeDisposable);
    }


    public ReactiveProperty<string> Name { get; } = new(string.Empty);
    public ReactiveProperty<string> Description { get; } = new(string.Empty);

    public ReadOnlyObservableCollection<ComponentTypes> Types { get; } = new(new ObservableCollection<ComponentTypes>(Enum.GetValues<ComponentTypes>()));
    public ReactiveProperty<ComponentTypes> SelectedType { get; } = new(ComponentTypes.String);

    public ReactiveProperty<string> ValueText { get; } = new(string.Empty);

    public ReactiveProperty<bool> CanCreateComponent { get; } = new(false);

    public ReactiveProperty<bool> IsWarningVisible { get; } = new(false);

    public ReactiveProperty<string> WarningToolTip { get; } = new("");


    public void Dispose()
    {
        _compositeDisposable.Dispose();

        Name.Dispose();
        Description.Dispose();
        SelectedType.Dispose();
        ValueText.Dispose();
        CanCreateComponent.Dispose();
        IsWarningVisible.Dispose();
        WarningToolTip.Dispose();

        GC.SuppressFinalize(this);
    }

    private bool CheckCanCreateComponent()
    {
        var invalidReasons = new List<string>();
        WarningToolTip.Value = string.Empty;

        if (string.IsNullOrEmpty(Name.Value))
        {
            invalidReasons.Add(StringResources.ComponentCreationDialog_Warning_NoName);
        }
        else if (_registeredComponentNames.Contains(Name.Value))
        {
            invalidReasons.Add(StringResources.ComponentCreationDialog_Warning_RegisteredName);
        }

        if (string.IsNullOrEmpty(ValueText.Value))
        {
            invalidReasons.Add(StringResources.ComponentCreationDialog_Warning_NoValue);
        }
        else
        {
            switch (SelectedType.Value)
            {
                case ComponentTypes.String:
                    break;
                case ComponentTypes.Int:
                    if (!int.TryParse(ValueText.Value, CultureInfo.InvariantCulture, out _))
                    {
                        invalidReasons.Add(StringResources.ComponentCreationDialog_Warning_DifferentTypeValue);
                    }
                    break;
                case ComponentTypes.Float:
                    if (!float.TryParse(ValueText.Value, CultureInfo.InvariantCulture, out _))
                    {
                        invalidReasons.Add(float.TryParse(ConvertFloatString(ValueText.Value), CultureInfo.InvariantCulture, out _)
                            ? StringResources.ComponentCreationDialog_Warning_NoNeedFloatSuffix
                            : StringResources.ComponentCreationDialog_Warning_DifferentTypeValue);
                    }
                    break;
                default:
                    break;
            }
        }

        if (invalidReasons.Count > 0)
        {
            WarningToolTip.Value = string.Join("\n", invalidReasons);
            return false;
        }

        return true;
    }

    private static string ConvertFloatString(string input)
    {
        var output = input;
        if (input.EndsWith("f", StringComparison.InvariantCultureIgnoreCase))
        {
            output = output[..^1];
        }

        return output;
    }


    public ComponentElement GetResult()
    {
        return ComponentElement.CreateInstance(Name.Value, Description.Value, GetValue());
    }

    private object GetValue()
    {
        return SelectedType.Value switch
        {
            ComponentTypes.String => ValueText.Value,
            ComponentTypes.Int => int.TryParse(ValueText.Value, CultureInfo.InvariantCulture, out var result) ? result : 0,
            ComponentTypes.Float => float.TryParse(ValueText.Value, CultureInfo.InvariantCulture, out var result) ? result : 0,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}

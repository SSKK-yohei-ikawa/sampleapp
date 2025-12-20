using SampleApp.Types;
using System.Windows.Input;

namespace SampleApp.Utilities;

public static class DummyComponentFactory
{
    private static readonly Random SRandom = new();

    private const string DummyComponentNamePreFix = "Dummy";
    private const string DummyComponentNameSuffixFormat = "_{0}";

    private const string DummyStringComponentName = DummyComponentNamePreFix + "String";
    private const string DummyStringComponentNameFormat = DummyStringComponentName + DummyComponentNameSuffixFormat;

    private const string DummyIntComponentName = DummyComponentNamePreFix + "Int";
    private const string DummyIntComponentNameFormat = DummyIntComponentName + DummyComponentNameSuffixFormat;

    private const string DummyFloatComponentName = DummyComponentNamePreFix + "Float";
    private const string DummyFloatComponentNameFormat = DummyFloatComponentName + DummyComponentNameSuffixFormat;

    private const string DummyComponentDescriptionFormat = "Description of {0}";

    private const string StringValueElemChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public static void CreateDummyComponents(int num, HashSet<string> registeredNames, ICommand addComponentCommand)
    {
        var stringNewIndex = GetComponentIndex(DummyStringComponentName);
        var intNewIndex = GetComponentIndex(DummyIntComponentName);
        var floatNewIndex = GetComponentIndex(DummyFloatComponentName);

        for (var i = 0; i < num; i++)
        {
            var type = (ComponentTypes)SRandom.Next(Enum.GetValues(typeof(ComponentTypes)).Length);
            var componentElement = type switch
            {
                ComponentTypes.String => CreateDummyStringComponentElement(),
                ComponentTypes.Int => CreateDummyIntComponentElement(),
                ComponentTypes.Float => CreateDummyFloatComponentElement(),
                _ => CreateDummyStringComponentElement()
            };

            addComponentCommand.Execute(componentElement);
            registeredNames.Add(componentElement.Name);
        }

        return;

        int GetComponentIndex(string componentName)
        {
            var newIndex = 0;
            foreach (var suffix in registeredNames.Where(name => name.StartsWith(componentName)).Select(name => name[(componentName.Length + 1)..]))
            {
                if (int.TryParse(suffix, out var index))
                {
                    newIndex = Math.Max(newIndex, index);
                }
            }
            return newIndex;
        }

        ComponentElement CreateDummyStringComponentElement()
        {
            var name = CreateNewName(DummyStringComponentNameFormat, ref stringNewIndex);
            var value = string.Empty;
            var valueLength = SRandom.Next(5, 10);
            for (var j = 0; j < valueLength; j++)
            {
                value += StringValueElemChars[SRandom.Next(StringValueElemChars.Length)];
            }
            return ComponentElement.CreateInstance(name, GetDescription(name), value);
        }

        ComponentElement CreateDummyIntComponentElement()
        {
            var name = CreateNewName(DummyIntComponentNameFormat, ref intNewIndex);
            var value = SRandom.Next(10000);
            return ComponentElement.CreateInstance(name, GetDescription(name), value);
        }

        ComponentElement CreateDummyFloatComponentElement()
        {
            var name = CreateNewName(DummyFloatComponentNameFormat, ref floatNewIndex);
            var value = SRandom.NextSingle() * SRandom.Next(1, 20);

            return ComponentElement.CreateInstance(name, GetDescription(name), value);
        }

        string CreateNewName(string componentNameFormat, ref int index)
        {
            while (true)
            {
                var newName = string.Format(componentNameFormat, index);
                if (!registeredNames.Contains(newName))
                {
                    return newName;
                }
                index++;
            }
        }
    }

    public static IEnumerable<ComponentElement> CreateDummyComponentElementsOnInit()
    {
        var componentElems = new List<ComponentElement>();
        var name = string.Format(DummyStringComponentNameFormat, 0);
        componentElems.Add(ComponentElement.CreateInstance(name, GetDescription(name), "TEST"));
        name = string.Format(DummyIntComponentNameFormat, 0);
        componentElems.Add(ComponentElement.CreateInstance(name, GetDescription(name), 1));
        name = string.Format(DummyFloatComponentNameFormat, 0);
        componentElems.Add(ComponentElement.CreateInstance(name, GetDescription(name), 2.5f));

        return componentElems;
    }

    private static string GetDescription(string name)
    {
        return string.Format(DummyComponentDescriptionFormat, name);
    }
}
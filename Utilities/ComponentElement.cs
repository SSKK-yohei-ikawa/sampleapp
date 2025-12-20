namespace SampleApp.Utilities;

public readonly record struct ComponentElement
{
    private ComponentElement(string Name, string Description, object Value)
    {
        this.Name = Name;
        this.Description = Description;
        this.Value = Value;
    }

    public string Name { get; }
    public string Description { get; }
    public object Value { get; }

    public void Deconstruct(out string name, out string description, out object value)
    {
        name = Name;
        description = Description;
        value = Value;
    }

    public static ComponentElement CreateInstance(string name, string description, object value)
    {
        return new ComponentElement(name, description, value);
    }
}

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Calculator.Annotations;

namespace Calculator.Models;

public class OperandModel : INotifyPropertyChanged
{
    private string _value;

    public OperandModel(string value)
    {
        Value = value ?? "";
    }

    public string Value
    {
        get => _value;
        set
        {
            _value = value;
            OnPropertyChanged(nameof(Value));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Calculator.Annotations;

namespace Calculator.Models;

public class OperationModel : INotifyPropertyChanged
{
    private Operation _operation;

    public OperationModel(Operation operation)
    {
        Operation = operation;
    }
    
    public Operation Operation
    {
        get => _operation;
        set
        {
            _operation = value;
            OnPropertyChanged(nameof(Operation));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
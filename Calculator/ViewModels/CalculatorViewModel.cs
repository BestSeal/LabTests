using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Calculator.Annotations;
using Calculator.Command;
using Calculator.Models;

namespace Calculator.ViewModels;

public class CalculatorViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public ObservableCollection<OperandModel> OperandModels { get; }
    public ObservableCollection<OperationModel> OperationModels { get; }

    public OperationCommand OperationCommand =>
        new OperationCommand(x =>
        {
            switch ((Operation)x)
            {
                case Operation.Sum:
                    OperationModel = OperationModels.First(x => x.Operation == Operation.Sum);
                    break;
                case Operation.Subtract:
                    OperationModel = OperationModels.First(x => x.Operation == Operation.Subtract);
                    break;
                case Operation.Divide:
                    OperationModel = OperationModels.First(x => x.Operation == Operation.Divide);
                    break;
                case Operation.Multiply:
                    OperationModel = OperationModels.First(x => x.Operation == Operation.Multiply);
                    break;
                case Operation.Result:
                    ExecuteResultOperation();
                    break;
                case Operation.Backspace:
                    switch (SelectedOperand)
                    {
                        case SelectedOperand.Left:
                            LeftOperand = string.IsNullOrEmpty(_leftOperand?.Value)
                                ? new OperandModel("")
                                : new OperandModel(LeftOperand.Value.Remove(LeftOperand.Value.Length - 1));
                            break;
                        case SelectedOperand.Right:
                            RightOperand = string.IsNullOrEmpty(_rightOperand?.Value)
                                ? new OperandModel("")
                                : new OperandModel(RightOperand.Value.Remove(RightOperand.Value.Length - 1));
                            break;
                    }

                    break;
            }
        });

    public OperationCommand OperandCommand =>
        new OperationCommand(x =>
        {
            switch (SelectedOperand)
            {
                case SelectedOperand.Left:
                    if ((string)x == "," && _leftOperand?.Value.Contains(',') == true) return;
                    LeftOperand = _leftOperand is null
                        ? new OperandModel(x.ToString())
                        : new OperandModel(LeftOperand.Value + x);
                    break;
                case SelectedOperand.Right:
                    if ((string)x == "," && _rightOperand?.Value.Contains(',') == true) return;
                    RightOperand = _rightOperand is null
                        ? new OperandModel(x.ToString())
                        : new OperandModel(RightOperand.Value + x);
                    break;
            }
        });

    public OperationCommand TextBoxCommand =>
        new OperationCommand(x =>
        {
            SelectedOperand = (SelectedOperand)x;
        });

    private OperandModel? _leftOperand;
    private OperandModel? _rightOperand;
    private OperandModel? _result;
    private OperationModel? _operationModel;
    private SelectedOperand SelectedOperand { get; set; }

    public CalculatorViewModel()
    {
        OperandModels = new ObservableCollection<OperandModel>
        {
            new("0"),
            new("1"),
            new("2"),
            new("3"),
            new("4"),
            new("5"),
            new("6"),
            new("7"),
            new("8"),
            new("9"),
            new(",")
        };
        OperationModels = new ObservableCollection<OperationModel>
        {
            new(Operation.Divide),
            new(Operation.Multiply),
            new(Operation.Sum),
            new(Operation.Subtract),
            new(Operation.Result),
            new(Operation.Backspace)
        };

        Result = new OperandModel("0");
        OperationModel = new OperationModel(Operation.Sum);
        SelectedOperand = SelectedOperand.Left;
    }

    public OperandModel LeftOperand
    {
        get => _leftOperand;
        set
        {
            _leftOperand = value;
            OnPropertyChanged(nameof(LeftOperand));
        }
    }

    public OperandModel RightOperand
    {
        get => _rightOperand;
        set
        {
            _rightOperand = value;
            OnPropertyChanged(nameof(RightOperand));
        }
    }

    public OperandModel Result
    {
        get => _result;
        set
        {
            _result = value;
            OnPropertyChanged(nameof(Result));
        }
    }

    public OperationModel OperationModel
    {
        get => _operationModel;
        set
        {
            _operationModel = value;
            OnPropertyChanged(nameof(OperationModel));
            OnPropertyChanged(nameof(StringOperandValue));
        }
    }

    public string StringOperandValue
    {
        get
        {
            switch (_operationModel.Operation)
            {
                case Operation.Sum:
                    return "+";
                case Operation.Subtract:
                    return "-";
                case Operation.Divide:
                    return "/";
                case Operation.Multiply:
                    return "*";
            }

            return "";
        }
    }

    public void ExecuteResultOperation()
    {
        switch (OperationModel.Operation)
        {
            case Operation.Sum:
                Result = new OperandModel(ExecuteSumOperation(LeftOperand.Value, RightOperand.Value)?.ToString());
                break;
            case Operation.Subtract:
                Result = new OperandModel(ExecuteSubtractOperation(LeftOperand.Value, RightOperand.Value)?.ToString());
                break;
            case Operation.Divide:
                Result = new OperandModel(ExecuteDivideOperation(LeftOperand.Value, RightOperand.Value)?.ToString());
                break;
            case Operation.Multiply:
                Result = new OperandModel(ExecuteMultiplyOperation(LeftOperand.Value, RightOperand.Value)?.ToString());
                break;
        }
    }

    public double? ExecuteMultiplyOperation(string leftS, string rightS)
    {
        if (double.TryParse(leftS, out var left) && double.TryParse(rightS, out var right))
        {
            return left * right;
        }
        
        return null;
    }

    public double? ExecuteDivideOperation(string leftS, string rightS)
    {
        if (double.TryParse(leftS, out var left) && double.TryParse(rightS, out var right))
        {
            if (right == 0) return null;

            return left / right;
        }
        
        return null;
    }

    public double? ExecuteSubtractOperation(string leftS, string rightS)
    {
        if (double.TryParse(leftS, out var left) && double.TryParse(rightS, out var right))
        {

            return left - right;
        }
        
        return null;
    }

    public double? ExecuteSumOperation(string leftS, string rightS)
    {
        if (double.TryParse(leftS, out var left) && double.TryParse(rightS, out var right))
        {

            return left + right;
        }

        return null;
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
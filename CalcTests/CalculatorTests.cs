using Calculator.ViewModels;
using Xunit;

namespace CalcTests;

public class CalculatorTests
{
    private static readonly CalculatorViewModel _calculatorViewModel = new();

    [Fact]
    public void DivisionByZero() => Assert.Null(_calculatorViewModel.ExecuteDivideOperation("8", "0"));
    
    [Fact]
    public void DivisionZeroByZero() => Assert.Null(_calculatorViewModel.ExecuteDivideOperation("0", "0"));
    
    [Fact]
    public void InvalidLeftInput() => Assert.Null(_calculatorViewModel.ExecuteDivideOperation("asdasf", "0"));
    
    [Fact]
    public void InvalidRightInput() => Assert.Null(_calculatorViewModel.ExecuteDivideOperation("124123", "asfasd"));
    
    [Fact]
    public void InvalidBothInputs() => Assert.Null(_calculatorViewModel.ExecuteDivideOperation("asfasd", "asfasd"));
    
    [Fact]
    public void MultiplyInt() => Assert.Equal(1000.0 ,_calculatorViewModel.ExecuteMultiplyOperation("10", "100"));
    
    [Fact]
    public void MultiplyDouble() => Assert.Equal(1050.0 ,_calculatorViewModel.ExecuteMultiplyOperation("10,5", "100"));
    
    [Fact]
    public void MultiplyByZero() => Assert.Equal(0 ,_calculatorViewModel.ExecuteMultiplyOperation("10,5", "0"));
    
    [Fact]
    public void MultiplyDoubleInvalidSeparator() => Assert.Null(_calculatorViewModel.ExecuteMultiplyOperation("10.5", "0"));
    
    [Fact]
    public void DivideDoubleInvalidSeparator() => Assert.Null(_calculatorViewModel.ExecuteMultiplyOperation("10.5", "0"));
    
    [Fact]
    public void SubtractDoubleInvalidSeparator() => Assert.Null(_calculatorViewModel.ExecuteMultiplyOperation("10.5", "0"));
    
    [Fact]
    public void MinusDoubleInvalidSeparator() => Assert.Null(_calculatorViewModel.ExecuteMultiplyOperation("10.5", "0"));
    
    [Fact]
    public void SubtractResultNegative() => Assert.Equal(-12329.5 ,_calculatorViewModel.ExecuteSubtractOperation("10,5", "12340"));
    
    [Fact]
    public void MultiplyOutOfRange() => Assert.Equal(0, _calculatorViewModel.ExecuteSubtractOperation("10000000000000000000000000000000000000", 
        "10000000000000000000000000000000000000"));
}
using System.Globalization;
using CalcTests;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using Xunit;

namespace CalcTestsFluent.Steps;

[Binding]
public sealed class CalculatorStepDefinitions
{
    private readonly ScenarioContext _scenarioContext;

    private readonly FlaUI.Core.Application _calculator =
        FlaUI.Core.Application.Launch(@"D:\dev\c#\Tests\Calculator\bin\Debug\net6.0-windows\Calculator.exe");

    private readonly Window _window;

    private string _result;

    public CalculatorStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
        _window = _calculator.GetMainWindow(new UIA3Automation());
    }

    [Given("the first number is (.*)")]
    public void GivenTheFirstNumberIs(double number)
    {
        foreach (var symbol in number.ToString(CultureInfo.InvariantCulture))
        {
            var btn = CalculatorUiTest.GetButtonByName(symbol.ToString(), _window);
            Mouse.Click(btn.GetClickablePoint());
        }

        var values = CalculatorUiTest.GetValues(_window);
        Mouse.Click(values.right.GetClickablePoint());
    }

    [Given("the second number is (.*)")]
    public void GivenTheSecondNumberIs(double number)
    {
        foreach (var symbol in number.ToString(CultureInfo.InvariantCulture))
        {
            var btn = CalculatorUiTest.GetButtonByName(symbol.ToString(), _window);
            Mouse.Click(btn.GetClickablePoint());
        }
    }

    [When("the two numbers are (.*)")]
    public void WhenOperation(string operation)
    {
        var btn = CalculatorUiTest.GetButtonByName(operation, _window);
        Mouse.Click(btn.GetClickablePoint());
        btn = CalculatorUiTest.GetButtonByName("Result", _window);
        Mouse.Click(btn.GetClickablePoint());
        
        var values = CalculatorUiTest.GetValues(_window);
        _result = values.result.Name;
    }

    [Then("the result should be (.*)")]
    public void ThenTheResultShouldBe(string result)
    {
        _calculator.Kill();
        Assert.Equal(result, _result);
    }
}
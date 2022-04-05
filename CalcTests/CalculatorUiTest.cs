using System.Collections.Generic;
using System.Threading;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using Xunit;

namespace CalcTests;

public class CalculatorUiTest
{
    private static FlaUI.Core.Application _calculator;

    [Fact]
    public void InitStart()
    {
        Retry.WhileException(() =>
        {
            Start();
            using var automation = new UIA3Automation();
            var window = _calculator.GetMainWindow(automation);

            var boxes = new List<string>();

            var left = window.FindFirstDescendant(x => x.ByAutomationId("LeftOperand")).AsTextBox()?.Text;
            var right = window.FindFirstDescendant(x => x.ByAutomationId("RightOperand")).AsTextBox()?.Text;
            var result = window.FindFirstDescendant(x => x.ByAutomationId("OperationResult")).Name;

            boxes.AddRange(new[]
            {
                left,
                right,
                result
            }!);

            Assert.All(boxes, x => Assert.True(string.IsNullOrEmpty(x) || x == "0"));

            Stop();
        });
    }

    [Fact]
    public void SumTwoNums()
    {
        Retry.WhileException(() =>
        {
            Start();
            using var automation = new UIA3Automation();
            var window = _calculator.GetMainWindow(automation);

            var btn1 = GetButtonByName("1", window);
            var btn0 = GetButtonByName("0", window);
            var sumBtn = GetButtonByName("Sum", window);
            var resultBtn = GetButtonByName("Result", window);

            Mouse.Click(btn1.GetClickablePoint());
            Mouse.Click(btn0.GetClickablePoint());
            Thread.Sleep(500);

            var (left, right, result) = GetValues(window);

            Mouse.Click(right.GetClickablePoint());
            Mouse.Click(btn1.GetClickablePoint());
            Mouse.Click(btn0.GetClickablePoint());
            Thread.Sleep(500);
            Mouse.Click(sumBtn.GetClickablePoint());
            Mouse.Click(resultBtn.GetClickablePoint());
            Thread.Sleep(1000);

            Assert.Equal("20", result.Name);

            Stop();
        });
    }

    [Fact]
    public void DivisionByZero()
    {
        Retry.WhileException(() =>
        {
            Start();
            using var automation = new UIA3Automation();
            var window = _calculator.GetMainWindow(automation);

            var btn1 = GetButtonByName("1", window);
            var btn0 = GetButtonByName("0", window);
            var divideBtn = GetButtonByName("Divide", window);
            var resultBtn = GetButtonByName("Result", window);

            Mouse.Click(btn1.GetClickablePoint());
            Thread.Sleep(500);

            var (left, right, result) = GetValues(window);

            Mouse.Click(right.GetClickablePoint());
            Mouse.Click(btn0.GetClickablePoint());
            Thread.Sleep(500);
            Mouse.Click(divideBtn.GetClickablePoint());
            Mouse.Click(resultBtn.GetClickablePoint());
            Thread.Sleep(1000);

            Assert.Equal("", result.Name);

            Stop();
        });
    }

    [Fact]
    public void Multiply()
    {
        Retry.WhileException(() =>
        {
            Start();
            using var automation = new UIA3Automation();
            var window = _calculator.GetMainWindow(automation);

            var btn1 = GetButtonByName("1", window);
            var btn0 = GetButtonByName("0", window);
            var divideBtn = GetButtonByName("Multiply", window);
            var resultBtn = GetButtonByName("Result", window);

            Mouse.Click(btn1.GetClickablePoint());
            Thread.Sleep(500);

            var (left, right, result) = GetValues(window);

            Mouse.Click(right.GetClickablePoint());
            Mouse.Click(btn0.GetClickablePoint());
            Thread.Sleep(500);
            Mouse.Click(divideBtn.GetClickablePoint());
            Mouse.Click(resultBtn.GetClickablePoint());
            Thread.Sleep(1000);

            Assert.Equal("0", result.Name);

            Stop();
        });
    }

    [Fact]
    public void Subtract()
    {
        Retry.WhileException(() =>
        {
            Start();
            using var automation = new UIA3Automation();
            var window = _calculator.GetMainWindow(automation);

            var btn1 = GetButtonByName("1", window);
            var btn0 = GetButtonByName("0", window);
            var subtractBtn = GetButtonByName("Subtract", window);
            var resultBtn = GetButtonByName("Result", window);

            Mouse.Click(btn1.GetClickablePoint());
            Mouse.Click(btn1.GetClickablePoint());
            Mouse.Click(btn1.GetClickablePoint());
            Thread.Sleep(500);

            var (left, right, result) = GetValues(window);

            Mouse.Click(right.GetClickablePoint());
            Mouse.Click(btn1.GetClickablePoint());
            Mouse.Click(btn1.GetClickablePoint());
            Thread.Sleep(500);
            Mouse.Click(subtractBtn.GetClickablePoint());
            Mouse.Click(resultBtn.GetClickablePoint());
            Thread.Sleep(1000);

            Assert.Equal("100", result.Name);

            Stop();
        });
    }

    private bool Start()
    {
        if (!(_calculator?.Close() ?? true)) return false;

        _calculator =
            FlaUI.Core.Application.Launch(@"D:\dev\c#\Tests\Calculator\bin\Debug\net6.0-windows\Calculator.exe");

        return !_calculator.HasExited;
    }

    private bool Stop()
    {
        _calculator.Kill();
        return _calculator.HasExited;
    }

    public static (TextBox left, TextBox right, AutomationElement result) GetValues(Window window) =>
        (window.FindFirstDescendant(x => x.ByAutomationId("LeftOperand")).AsTextBox(),
            window.FindFirstDescendant(x => x.ByAutomationId("RightOperand")).AsTextBox(),
            window.FindFirstDescendant(x => x.ByAutomationId("OperationResult")))!;

    public static Button GetButtonByName(string name, Window window) =>
        window.FindFirstDescendant(cf => cf.ByName(name)).AsButton();
}
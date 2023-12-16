using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using CalculatorExample.Logic;
using CalculatorExample.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace CalculatorExample.ViewModels;

public class CalculatorVM : INotifyPropertyChanged {
    private string expressionText;
    public string ExpressionText {
        get => expressionText;
        set => SetField(ref expressionText, value);
    }

    #region Commands

    /// <summary>
    /// Команда добавления символа к выражению.
    /// </summary>
    public RelayCommand AddCharToExpressionCommand { get; }
    
    /// <summary>
    /// Команда добавления знака "минус" к числу в выражении.
    /// </summary>
    public RelayCommand NegateCommand { get; }
    
    /// <summary>
    /// Команда убирания последнего символа в выражении.
    /// </summary>
    public RelayCommand BackspaceCommand { get; }
    
    /// <summary>
    /// Команда очистки поля выражения.
    /// </summary>
    public RelayCommand ClearFieldCommand { get; }
    
    /// <summary>
    /// Команда подсчёта выражения.
    /// </summary>
    public RelayCommand EvaluateExpressionCommand { get; }

    #endregion

    /// <summary>
    /// Конструктор.
    /// </summary>
    public CalculatorVM() {
        EvaluateExpressionCommand = new RelayCommand(_ => ExpressionText = Calculator.EvaluateExpression(ExpressionText).ToString(CultureInfo.CurrentUICulture) ?? string.Empty);
        ClearFieldCommand = new RelayCommand(_ => ExpressionText = string.Empty);
        BackspaceCommand = new RelayCommand(_ => {
            if (!string.IsNullOrEmpty(expressionText)) {
                ExpressionText = ExpressionText[..^1];
            }
        });
        NegateCommand = new RelayCommand(_ => {
            var lastChar = !string.IsNullOrEmpty(expressionText) ? expressionText.LastOrDefault() : default;
                
            if (string.IsNullOrEmpty(expressionText) || lastChar == '(') {
                ExpressionText += '-';
            }
        });
        AddCharToExpressionCommand = new RelayCommand(AddCharToExpression);
    }

    /// <summary>
    /// Добавляет символ к текущему выражению.
    /// </summary>
    /// <param name="obj">Символ для добавления.</param>
    private void AddCharToExpression(object? obj) {
        var lastChar = !string.IsNullOrEmpty(expressionText) ? expressionText.LastOrDefault() : default;

        if (lastChar == default && !char.IsDigit(obj?.ToString()?.FirstOrDefault() ?? default)
                                && (obj?.ToString()?.FirstOrDefault() ?? default) is not ('(' or ')')) {
            return;
        }

        if (lastChar != default && !char.IsDigit(obj?.ToString()?.FirstOrDefault() ?? default) &&
            !char.IsDigit(lastChar)) {
            switch (lastChar) {
                case '(' or ')': {
                    if (lastChar == (obj?.ToString()?.FirstOrDefault() ?? default)) {
                        ExpressionText += obj?.ToString();
                    }

                    return;
                }
                case '-' when ExpressionText.Length == 1:
                    return;
            }

            if (lastChar is not ('(' or ')')) {
                if ((obj?.ToString()?.FirstOrDefault() ?? default) is '(') {
                    ExpressionText += obj?.ToString();
                }

                if ((obj?.ToString()?.FirstOrDefault() ?? default) is ')') {
                    return;
                }
            }

            var replacedText = new StringBuilder(expressionText);
            replacedText[^1] = obj?.ToString()?.FirstOrDefault() ?? default;

            ExpressionText = replacedText.ToString();
            return;
        }

        ExpressionText += obj?.ToString();
    }

    #region INPC

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null) {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    #endregion
}
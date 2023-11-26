using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Calculator.Commands;
using Calculator.Logic;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.ViewModels;

public class CalculatorVM : INotifyPropertyChanged {
    private CalculatorLogic logic;
    
    private string expressionText;
    public string ExpressionText {
        get => expressionText;
        set => SetField(ref expressionText, value);
    }

    private RelayCommand? addTextToExpressionCommand;

    public RelayCommand AddTextToExpressionCommand {
        get {
            return addTextToExpressionCommand ??= new RelayCommand(obj => {
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
            });
        }
    }

    private RelayCommand? negateCommand;

    public RelayCommand NegateCommand {
        get {
            return negateCommand ??= new RelayCommand(_ => {
                var lastChar = !string.IsNullOrEmpty(expressionText) ? expressionText.LastOrDefault() : default;
                
                if (string.IsNullOrEmpty(expressionText) || lastChar == '(') {
                    ExpressionText += '-';
                }
            });
        }
    }

    private RelayCommand? backspaceCommand;

    public RelayCommand BackspaceCommand {
        get {
            return backspaceCommand ??= new RelayCommand(_ => {
                if (!string.IsNullOrEmpty(expressionText)) {
                    ExpressionText = ExpressionText[..^1];
                }
            });
        }
    }

    private RelayCommand? clearFieldCommand;

    public RelayCommand ClearFieldCommand {
        get {
            return clearFieldCommand ??= new RelayCommand(_ => {
                ExpressionText = string.Empty;
            });
        }
    }

    private RelayCommand? evaluateExpressionCommand;

    public RelayCommand EvaluateExpressionCommand {
        get {
            return evaluateExpressionCommand ??= new RelayCommand(_ => {
                
            });
        }
    }

    public CalculatorVM() {
        logic = App.ServiceProvider.GetService<CalculatorLogic>();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null) {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
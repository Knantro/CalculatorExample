using Calculator.Models;

namespace Calculator.Logic; 

public class CalculatorLogic {
    private const char NEGATE_MINUS = '-';
    private const string OPERATIONS = "+-×/";
    private const char OPEN_BRACE = '(';
    private const char CLOSE_BRACE = ')';
    
    private static void ValidateExpression(string expression) {
        ExpressionValidation.EmptyExpression(expression);
        ExpressionValidation.FirstSymbol(expression);
        ExpressionValidation.LastSymbol(expression);
        ExpressionValidation.EqualBraces(expression);
        ExpressionValidation.BracesOpenCloseSequence(expression);
        ExpressionValidation.OperationSymbols(expression);
    }
    
    /// <summary>
    /// Вычислить выражение с валидацией
    /// </summary>
    /// <param name="expression">Исходное выражение</param>
    /// <returns>Результат вычисления</returns>
    public decimal EvaluateExpression(string expression) {
        ValidateExpression(expression);
        
        return EvaluateOperation(expression);
    }

    private decimal EvaluateOperation(string expression) {
        if (expression.Length == 0) {
            return 0;
        }
        
        var operations = new List<Operation>();
        var currNumber = string.Empty;
        var leftNumber = (decimal?)null;
        var rightNumber = (decimal?)null;
        var lastCharIsDigit = false;
        var currOperation = OperationType.None;
        
        for (var i = 0; i < expression.Length; i++) {
            var ch = expression[i];
            
            // Собираем число из цифр.
            if (char.IsDigit(ch)) {
                currNumber += ch;
                lastCharIsDigit = true;
            }

            // Если последний символ был цифрой, а сейчас не цифра, значит, мы собрали число.
            if (!char.IsDigit(ch) && lastCharIsDigit || i == expression.Length - 1 && ch != CLOSE_BRACE) {
                if (leftNumber == null) {
                    leftNumber = decimal.Parse(currNumber!);
                }
                else {
                    rightNumber = decimal.Parse(currNumber!);
                }

                currNumber = null;
                lastCharIsDigit = false;
            }

            if (leftNumber != null && currOperation != OperationType.None && rightNumber != null) {
                var leftOperation = operations.LastOrDefault() ?? new Operation {
                    Type = OperationType.None,
                    Value = leftNumber.Value
                };
                
                var centralOperation = new Operation {
                    Type = currOperation
                };

                leftOperation.Right = centralOperation;
                
                var rightOperation = new Operation {
                    Type = OperationType.None,
                    Value = rightNumber.Value
                };
                
                if (operations.Count > 0) {
                }
                
                centralOperation.Left = leftOperation;
                centralOperation.Right = rightOperation;
                rightOperation.Left = centralOperation;

                if (operations.Count == 0) {
                    operations.Add(leftOperation);
                }
                operations.Add(centralOperation);
                operations.Add(rightOperation);

                leftNumber = rightNumber;
                currOperation = OperationType.None;
                rightNumber = null;
            }

            // Если нашли знак, значит это операция.
            if (OPERATIONS.Contains(ch)) {
                // Проверяем, что минус в выражении - не знак числа.
                if (ch == NEGATE_MINUS && (i == 0 || expression[i - 1] == OPEN_BRACE)) {
                    currNumber += ch;
                    continue;
                }
                
                currOperation = MapOperation(ch);
            }
            
            // Нашли открывающую скобку, входим в рекурсию.
            if (ch == OPEN_BRACE) {
                var index = i + 1;
                var length = IndexOfLastClosingBrace(expression[i..]);
                rightNumber = EvaluateOperation(expression.Substring(index, length - 1));
                i = index + length - 2;
            }

            // Достигли конца закрывающей скобки или  конца выражения - начинаем считать выражение.
            if (i == expression.Length - 1) {
                leftNumber = ComputeOperations(operations);
            }
        }

        return leftNumber ?? 0;
    }

    private OperationType MapOperation(char ch) => ch switch {
            '+' => OperationType.Add,
            '-' => OperationType.Subtract,
            '×' => OperationType.Multiply,
            '/' => OperationType.Divide,
            _ => OperationType.None
        };

    private static int IndexOfLastClosingBrace(string expression) {
        if (!expression.StartsWith('(')) {
            throw new ArgumentException("Expression should starts with opening brace", nameof(expression));
        }
        
        var counter = 1;
        for (var i = 1; i < expression.Length; i++) {
            if (expression[i] == '(') {
                counter++;
            }

            if (expression[i] == ')') {
                counter--;
            }

            if (counter == 0) {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Посчитать текущие операции.
    /// </summary>
    /// <param name="operations">Список операций.</param>
    /// <returns>Результат выполнения операций</returns>
    /// <remarks>
    /// Считаются все операции внутри скобок или просто все операции выражения, если скобок нет.
    /// </remarks>
    private decimal ComputeOperations(List<Operation> operations) {
        // Нам нужна стабильная сортировка, Array.Sort() - нестабильная сортировка.
        // Стабильная сортировка не меняет местами одинаковые по значению данные.
        operations = operations.Where(x => x.Type is not OperationType.None) // Просто числа не пытаемся прогонять через операции.
                               .OrderByDescending(x => x.Type is OperationType.Multiply or OperationType.Divide)
                               .ToList();
        foreach (var op in operations) {
            op.Value = ComputeOperation(op.Type, op.Left!.Value, op.Right!.Value);
            op.Type = OperationType.None;
            
            if (op.Left.Left != null) {
                op.Left.Left!.Right = op;
                op.Left = op.Left.Left;
            }

            if (op.Right.Right != null) {
                op.Right.Right!.Left = op;
                op.Right = op.Right.Right;
            }
        }

        return operations.Last().Value;
    }

    private static decimal ComputeOperation(OperationType type, decimal left, decimal right) => type switch {
        OperationType.Add => left + right,
        OperationType.Subtract => left - right,
        OperationType.Multiply => left * right,
        OperationType.Divide => left / right, // И на ноль поделим, не сомневайтесь.
        _ => 0
    };
}
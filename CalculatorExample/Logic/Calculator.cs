using CalculatorExample.Models;

namespace CalculatorExample.Logic; 

public static class Calculator {
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
    /// Вычисляет выражение с валидацией
    /// </summary>
    /// <param name="expression">Исходное выражение</param>
    /// <returns>Результат вычисления</returns>
    public static decimal EvaluateExpression(string expression) {
        ValidateExpression(expression);
        
        return EvaluateExpressionInternal(expression);
    }

    /// <summary>
    /// Вычисляет выражение (внутренний метод)
    /// </summary>
    /// <param name="expression">Исходное выражение</param>
    /// <returns>Результат вычисления</returns>
    private static decimal EvaluateExpressionInternal(string expression) {
        if (expression.Length == 0) {
            return 0;
        }

        expression = EraseExtraBraces(expression);
        
        var operations = new List<Operation>();
        var currNumber = string.Empty;
        var leftNumber = (decimal?)null;
        var rightNumber = (decimal?)null;
        var lastCharIsDigit = false; // Чтобы определить конец числа в строке.
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
                /*
                 * Чтобы избежать задвоения, мы берём последнюю операцию из списка, так как после итерации
                 * мы кладём то, что было справа - налево (ниже это показано).
                 */ 
                
                // Левое число.
                var leftOperation = operations.LastOrDefault() ?? new Operation {
                    Type = OperationType.None,
                    Value = leftNumber.Value
                };
                
                // Сама операция.
                var centralOperation = new Operation {
                    Type = currOperation
                };
                
                // У левого числа справа от него будет идти операция.
                leftOperation.Right = centralOperation;
                
                // Правое число. 
                var rightOperation = new Operation {
                    Type = OperationType.None,
                    Value = rightNumber.Value
                };
                
                // Слева у операции стоит левое число, справа - правое.
                // У правого числа слева стоит операция.
                centralOperation.Left = leftOperation;
                centralOperation.Right = rightOperation;
                rightOperation.Left = centralOperation;

                /*
                 * Не добавляем в список левую операцию, если список операций не пуст, т.к. может быть задвоение из-за того,
                 * что мы берём в качестве левой операции ссылку на последнюю операцию из списка.
                 */
                if (operations.Count == 0) {
                    operations.Add(leftOperation);
                }
                // Добавляем операции в список.
                operations.Add(centralOperation);
                operations.Add(rightOperation);

                // Перекладываем числа справа налево, добираем ещё одну операцию и одно число - снова создаём 2 новые операции в списке.
                leftNumber = rightNumber;
                currOperation = OperationType.None;
                rightNumber = null;
            }

            // Если нашли знак, значит это операция.
            if (OPERATIONS.Contains(ch)) {
                // Проверяем, что минус в выражении - не знак числа.
                if (ch == NEGATE_MINUS && (i == 0 || expression[i - 1] == OPEN_BRACE)) {
                    leftNumber = 0;
                    currOperation = OperationType.Subtract;
                    // currNumber += ch;
                    // if (expression[i + 1] == OPEN_BRACE) {
                    //     currNumber += 0;
                    //     lastCharIsDigit = true;
                    // }
                    continue;
                }
                
                currOperation = MapOperation(ch);
            }
            
            // Нашли открывающую скобку, входим в рекурсию.
            if (ch == OPEN_BRACE) {
                var index = i + 1;
                var length = IndexOfLastClosingBrace(expression[i..]);
                rightNumber = EvaluateExpressionInternal(expression.Substring(index, length - 1));
                i = index + length - 2;
            }

            // Достигли конца закрывающей скобки или  конца выражения - начинаем считать выражение.
            if (i == expression.Length - 1) {
                leftNumber = ComputeOperations(operations);
            }
        }

        return leftNumber ?? 0;
    }

    private static string EraseExtraBraces(string expression) {
        var counter = 1;
        bool foundExtraBraces;
        do {
            if (expression.First() != OPEN_BRACE || expression.Last() != CLOSE_BRACE) {
                return expression;
            }
            
            foundExtraBraces = false;
            
            for (var i = 1; i < expression.Length; i++) {
                var ch = expression[i];
                
                if (ch == OPEN_BRACE) {
                    counter++;
                }

                if (ch == CLOSE_BRACE) {
                    counter--;
                }

                // Лишние скобки могут быть только по краям выражения, которые относятся друг к другу
                if (counter == 0) {
                    if (i + 1 == expression.Length) {
                        foundExtraBraces = true;
                        expression = expression[1..^1];
                    }
                    else {
                        break;
                    }
                }
            }
        } while (foundExtraBraces);

        return expression;
    }

    private static OperationType MapOperation(char ch) => ch switch {
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
    private static decimal ComputeOperations(List<Operation> operations) {
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
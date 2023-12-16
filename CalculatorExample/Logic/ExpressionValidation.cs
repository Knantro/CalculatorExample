using CalculatorExample.Exceptions;
using CalculatorExample.Models;

namespace CalculatorExample.Logic; 

public static class ExpressionValidation {
    private const char NEGATE_MINUS = '-';
    private const string OPERATIONS = "+-×/";
    private const char OPEN_BRACE = '(';
    private const char CLOSE_BRACE = ')';
    
    public static void EmptyExpression(string expression) {
        if (string.IsNullOrEmpty(expression)) {
            throw new ExpressionException("Expression should not be empty", ExpressionErrorCode.EmptyExpression);
        }
    }
    
    public static void EqualBraces(string expression) {
        EmptyExpression(expression);
        
        if (expression.Count(x => x == OPEN_BRACE) != expression.Count(x => x == CLOSE_BRACE)) {
            throw new ExpressionException("Expression should have equal quantity of braces", ExpressionErrorCode.BracesCountNotEqual);
        }
    }

    public static void BracesOpenCloseSequence(string expression) {
        EmptyExpression(expression);
        
        var counter = 0;
        foreach (var ch in expression) {
            if (ch == OPEN_BRACE) {
                counter++;
            }

            if (ch == CLOSE_BRACE) {
                counter--;
            }

            if (counter < 0) {
                throw new ExpressionException("Expression cannot have a closing parenthesis without a corresponding opening", ExpressionErrorCode.InvalidBracesSequence);
            }
        }
    }

    public static void FirstSymbol(string expression) {
        EmptyExpression(expression);
        
        if (expression.FirstOrDefault() != OPEN_BRACE && expression.FirstOrDefault() != NEGATE_MINUS && !char.IsDigit(expression.FirstOrDefault())) {
            throw new ExpressionException("Expression can be started with a opening parenthesis, minus or digit", ExpressionErrorCode.InvalidFirstSymbol);
        }
    }

    public static void LastSymbol(string expression) {
        EmptyExpression(expression);
        
        if (expression.LastOrDefault() != CLOSE_BRACE && !char.IsDigit(expression.LastOrDefault())) {
            throw new ExpressionException("Expression can be completed with a closing parenthesis or digit", ExpressionErrorCode.InvalidLastSymbol);
        }
    }

    public static void OperationSymbols(string expression) {
        EmptyExpression(expression);

        for (var i = 0; i < expression.Length; i++) {
            var ch = expression[i];
            
            if (i < expression.Length - 1) {
                if (OPERATIONS.Contains(ch) && OPERATIONS.Contains(expression[i + 1])) {
                    throw new ExpressionException("Expression can not contain two operations nearby", ExpressionErrorCode.TwoOperationsNearby);
                }
                
                if (ch == OPEN_BRACE && OPERATIONS.Contains(expression[i + 1]) && expression[i + 1] != NEGATE_MINUS) {
                    throw new ExpressionException("Expression can not contain opening brace with operation following it", ExpressionErrorCode.BraceOperationInvalidOrder);
                }
                
                if (OPERATIONS.Contains(ch) && expression[i + 1] == CLOSE_BRACE) {
                    throw new ExpressionException("Expression can not contain closing brace with operation before", ExpressionErrorCode.BraceOperationInvalidOrder);
                }

                if (ch == OPEN_BRACE && expression[i + 1] == CLOSE_BRACE) {
                    throw new ExpressionException("Expression can not contain empty braces", ExpressionErrorCode.EmptyBraces); 
                }
            }
        }
    }
}
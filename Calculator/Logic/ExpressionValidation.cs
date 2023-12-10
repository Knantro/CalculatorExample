using Calculator.Exceptions;

namespace Calculator.Logic; 

public static class ExpressionValidation {
    private const string DIGITS = "0123456789";
    private const char NEGATE_MINUS = '-';
    private const string OPERATIONS = "+-×/";
    private const char OPEN_BRACE = '(';
    private const char CLOSE_BRACE = ')';
    
    public static void EmptyExpression(string expression) {
        if (string.IsNullOrEmpty(expression)) {
            throw new ExpressionException("Expression should not be empty");
        }
    }
    
    public static void EqualBraces(string expression) {
        EmptyExpression(expression);
        
        if (expression.Count(x => x == '(') != expression.Count(x => x == ')')) {
            throw new ExpressionException("Expression should have equal quantity of braces");
        }
    }

    public static void BracesOpenCloseSequence(string expression) {
        EmptyExpression(expression);
        
        var counter = 0;
        foreach (var ch in expression) {
            if (ch == '(') {
                counter++;
            }

            if (ch == ')') {
                counter--;
            }

            if (counter < 0) {
                throw new ExpressionException("Expression cannot have a closing parenthesis without a corresponding opening");
            }
        }
    }

    public static void FirstSymbol(string expression) {
        EmptyExpression(expression);
        
        if (expression.FirstOrDefault() != '(' && expression.FirstOrDefault() != '-' && !char.IsDigit(expression.FirstOrDefault())) {
            throw new ExpressionException("Expression can be started with a opening parenthesis, minus or digit");
        }
    }

    public static void LastSymbol(string expression) {
        EmptyExpression(expression);
        
        if (expression.LastOrDefault() != ')' && !char.IsDigit(expression.LastOrDefault())) {
            throw new ExpressionException("Expression can be completed with a closing parenthesis or digit");
        }
    }

    public static void OperationSymbols(string expression) {
        EmptyExpression(expression);

        for (var i = 0; i < expression.Length; i++) {
            var ch = expression[i];
            
            if (i < expression.Length - 1) {
                if (OPERATIONS.Contains(ch) && OPERATIONS.Contains(expression[i + 1])) {
                    throw new ExpressionException("Expression can not contain two operations nearby");
                }
                
                if (ch == '(' && OPERATIONS.Contains(expression[i + 1])) {
                    throw new ExpressionException("Expression can not contain opening brace with operation following it");
                }
                
                if (OPERATIONS.Contains(ch) && expression[i + 1] == ')') {
                    throw new ExpressionException("Expression can not contain closing brace with operation before");
                }

                if (ch == '(' && expression[i + 1] == ')') {
                    throw new ExpressionException("Expression can not contain empty braces"); 
                }
            }
        }
        
        // if (expression.LastOrDefault() != ')' && !char.IsDigit(expression.LastOrDefault())) {
        //     throw new ExpressionException("Expression can be completed with a closing parenthesis or digit");
        // }
    }
}
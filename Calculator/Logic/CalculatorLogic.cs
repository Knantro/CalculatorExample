namespace Calculator.Logic; 

public class CalculatorLogic {
    private const string DIGITS = "0123456789";
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
    }

    private decimal EvaluateOperation(string operation) {
        return 0;
    }
    
    public decimal EvaluateExpression(string expression) {
        ValidateExpression(expression);

        foreach (var ch in expression) {
            
        }
        
        return 0;
    }
}
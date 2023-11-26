namespace Calculator.Exceptions;

public class ExpressionException : Exception {
    public ExpressionException() { }
    public ExpressionException(string message) : base(message) { }
    public ExpressionException(string message, Exception inner) : base(message, inner) { }
}
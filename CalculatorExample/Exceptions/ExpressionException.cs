using CalculatorExample.Models;

namespace CalculatorExample.Exceptions;

public class ExpressionException : Exception {
    public ExpressionErrorCode ErrorCode { get; private set; }
    
    public ExpressionException(ExpressionErrorCode code) {
        ErrorCode = code;
    }

    public ExpressionException(string message, ExpressionErrorCode code = ExpressionErrorCode.UnexpectedError) : base(message) {
        ErrorCode = code;
    }
    
    public ExpressionException(string message, Exception inner) : base(message, inner) { }
}
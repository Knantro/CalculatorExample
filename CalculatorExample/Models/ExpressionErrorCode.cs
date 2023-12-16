namespace CalculatorExample.Models; 

public enum ExpressionErrorCode {
    UnexpectedError,
    BracesCountNotEqual,
    TwoOperationsNearby,
    BraceOperationInvalidOrder,
    InvalidFirstSymbol,
    InvalidLastSymbol,
    EmptyExpression,
    InvalidBracesSequence,
    EmptyBraces,
}
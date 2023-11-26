namespace Calculator.Models; 

public class Operation {
    public OperationType OperationType { get; set; }
    public Operation? LeftOperation { get; set; }
    public Operation? RightOperation { get; set; }
    public decimal? Value { get; set; }
}
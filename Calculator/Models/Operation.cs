namespace Calculator.Models; 

/// <summary>
/// Модель операции.
/// </summary>
public class Operation {
    
    /// <summary>
    /// Тип операции.
    /// </summary>
    public OperationType Type { get; set; }
    
    /// <summary>
    /// Операция слева.
    /// </summary>
    public Operation? Left { get; set; }
    
    /// <summary>
    /// Операция справа.
    /// </summary>
    public Operation? Right { get; set; }
    
    /// <summary>
    /// Значение уже посчитанной текущей операции.
    /// </summary>
    public decimal Value { get; set; }
}
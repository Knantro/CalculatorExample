namespace CalculatorExample.Models; 

/// <summary>
/// Обёртка для числа, чтобы число было ссылочным типом
/// </summary>
public class Number {
    
    /// <summary>
    /// Исходное значение
    /// </summary>
    public decimal Value { get; set; }
}
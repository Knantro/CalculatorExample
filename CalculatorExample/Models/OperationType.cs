namespace CalculatorExample.Models; 

public enum OperationType {
    /// <summary>
    /// Операции нет. Используется для чисел
    /// </summary>
    None,
    
    /// <summary>
    /// Сложение
    /// </summary>
    /// <remarks>
    /// Операция Add и Subtract равнозначны по порядку вычисления.
    /// </remarks>
    Add,
    
    /// <summary>
    /// Вычитание
    /// </summary>
    /// <remarks>
    /// Операция Add и Subtract равнозначны по порядку вычисления.
    /// </remarks>
    Subtract, // Операция Add и Subtract равнозначны по порядку вычисления.
    
    /// <summary>
    /// Умножение
    /// </summary>
    Multiply, // Операция Multiply и Divide равнозначны по порядку вычисления.
    
    /// <summary>
    /// Деление
    /// </summary>
    Divide // Операция Multiply и Divide равнозначны по порядку вычисления.
}
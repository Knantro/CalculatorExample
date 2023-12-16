using CalculatorExample.Logic;

namespace CalculatorExample.Tests;

public class ExpressionTests {
    
    [Theory]
    [InlineData("2+2×2-3", 3)]
    [InlineData("2×2-3", 1)]
    [InlineData("10-8+9×3-30/10", 26)]
    [InlineData("800-100/5×20+3", 403)]
    [InlineData("-29+30", 1)]
    [InlineData("-29×30", -870)]
    public void EvaluateExpression_Simple(string expression, decimal expected) {
        Assert.Equal(expected, Calculator.EvaluateExpression(expression));
    }
    
    [Theory]
    [InlineData("5+10×(50-20×2)", 105)]
    [InlineData("5+10×(50-20×2)+30", 135)]
    [InlineData("5+10×(50-20×2-30)×3-20+(1+12×10)/11", -604)]
    [InlineData("(50-20×2-30)×3-20+(1+10)/11", -79)]
    public void EvaluateExpression_OnePairBraces(string expression, decimal expected) {
        Assert.Equal(expected, Calculator.EvaluateExpression(expression));
    }
    
    [Theory]
    [InlineData("5+10×(50-20×(10-5)-13)", -625)]
    [InlineData("5+10×(50-20×(10-5)-13)×3-20+(1+12×10/(10-5))/10", -1902.5)]
    [InlineData("5+10×(50-(50-20×2-30)+(800-(2×2-3)+100/5×20+3)×30×(10-5)-13)", 1803575)]
    [InlineData("5+10×(50-(50-20×2-30)+(800-(2×2-3)+100/5×20+3)×30×(10-5)-13)/(5+10×(50-20×2-30)×3-20+(1+12×10)/11)", -2981.04304636)]
    [InlineData("((50-20×2-30)×3-20+(1+10)/11)", -79)]
    [InlineData("(-(-(-(-30))))", 30)]
    public void EvaluateExpression_NestedLevelBraces(string expression, decimal expected) {
        var result = Math.Round(Calculator.EvaluateExpression(expression), 8, MidpointRounding.AwayFromZero);
        Assert.Equal(expected, result);
    }
}
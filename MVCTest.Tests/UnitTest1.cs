using MVCTest.Helpers;

namespace MVCTest.Tests
{
    public class VATCalculatorTests
    {
        [Theory]
        [InlineData(5, 100, 0.21, 605)]
        public void CalculateTotalWithVAT_ShouldReturnExpectedValue(
            int quantity, decimal price, decimal vat, decimal expected)
        {
            // Act
            var result = VATCalculator.CalculateTotalWithVAT(quantity, price, vat);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
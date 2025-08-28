namespace MVCTest.Helpers
{
    public static class VATCalculator
    {
        public static decimal CalculateTotalWithVAT(int quantity, decimal price, decimal vat)
        {
            return (quantity * price) * (1 + vat);
        }
    }
}

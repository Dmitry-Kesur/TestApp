namespace Infrastructure.Services.InAppPurchase
{
    public interface IPurchaseValidator
    {
        bool Validate(string receipt);
    }
}
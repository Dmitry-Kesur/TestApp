namespace Infrastructure.Services.Progress.PlayerProgressUpdaters
{
    public class ResourceProgressUpdater : ProgressUpdater
    {
        public int GetActiveBoosterId() =>
            progress.ActiveBoosterId;

        public int GetCurrencyAmount() =>
            progress.CurrencyAmount;

        public void SetActiveBoosterId(int boosterId)
        {
            progress.ActiveBoosterId = boosterId;
        }

        public void UpdateCurrencyAmount(int currencyAmount)
        {
            progress.CurrencyAmount = currencyAmount;
        }
    }
}
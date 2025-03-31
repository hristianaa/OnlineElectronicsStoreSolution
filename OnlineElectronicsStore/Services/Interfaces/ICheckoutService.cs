namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface ICheckoutService
    {
        Task<bool> PlaceOrderAsync(int userId);
    }
}

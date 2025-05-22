using Infrastructure.Services.Log;
using UnityEngine;
using UnityEngine.Purchasing.Security;

namespace Infrastructure.Services.InAppPurchase
{
    public class CrossPlatformPurchaseValidator : IPurchaseValidator
    {
        private readonly CrossPlatformValidator _validator;
        private readonly IExceptionLoggerService _exceptionLoggerService;

        public CrossPlatformPurchaseValidator(IExceptionLoggerService exceptionLoggerService)
        {
            _exceptionLoggerService = exceptionLoggerService;
            
#if !UNITY_EDITOR
                _validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), UnityEngine.Application.identifier);
#endif
        }
        
        public bool Validate(string receipt)
        {
#if !UNITY_EDITOR
                 try
            {
                var result = _validator.Validate(receipt);
                foreach (var productReceipt in result)
                {
                    _exceptionLoggerService.Log($"Product ID: {productReceipt.productID}, Purchase Date: {productReceipt.purchaseDate}");
                }
                
                return true;
            }
            catch (IAPSecurityException ex)
            {
                _exceptionLoggerService.LogError($"Invalid receipt: {ex.Message}");
                return false;
            }
#else
                Debug.Log("[InAppPurchase] ValidatePurchase always returns true in Unity Editor.");
                return true;
#endif
        }
    }
}
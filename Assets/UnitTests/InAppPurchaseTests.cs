using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure.Data.PlayerProgress;
using Infrastructure.Data.Products;
using Infrastructure.Enums;
using Infrastructure.Providers.InAppPurchase;
using Infrastructure.Services.Analytics;
using Infrastructure.Services.InAppPurchase;
using Infrastructure.Services.Log;
using Infrastructure.Services.Progress;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace UnitTests
{
    [TestFixture]
    public class InAppPurchaseTests : ZenjectUnitTestFixture
    {
        private const string FakeProductId = "product1";
        
        private IExceptionLoggerService _exceptionLogger;
        private ISaveLoadProgressService _saveLoadProgressService;
        private IPurchaseValidator _purchaseValidator;
        private IInAppProductsSource _inAppProductsSource;
        private InAppPurchaseProvider _inAppPurchaseProvider;
        private InAppPurchaseService _inAppPurchaseService;
        private IAnalyticsService _analyticsService;
        
        public override void Setup()
        {
            base.Setup();
            _exceptionLogger = Substitute.For<IExceptionLoggerService>();
            _saveLoadProgressService = Substitute.For<ISaveLoadProgressService>();
            _purchaseValidator = Substitute.For<IPurchaseValidator>();
            _inAppProductsSource = Substitute.For<IInAppProductsSource>();
            _analyticsService = Substitute.For<IAnalyticsService>();

            Container.Bind<IExceptionLoggerService>().FromInstance(_exceptionLogger);
            Container.Bind<ISaveLoadProgressService>().FromInstance(_saveLoadProgressService);
            Container.Bind<IPurchaseValidator>().FromInstance(_purchaseValidator);
            Container.Bind<IInAppProductsSource>().FromInstance(_inAppProductsSource);
            Container.Bind<IAnalyticsService>().FromInstance(_analyticsService);
            Container.BindInterfacesAndSelfTo<InAppPurchaseProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<InAppPurchaseService>().AsSingle();
            
            _inAppPurchaseProvider = Container.Resolve<InAppPurchaseProvider>();
            _inAppPurchaseService = Container.Resolve<InAppPurchaseService>();
        }

        [UnityTest]
        public IEnumerator PurchaseProduct_WhenSuccess()
        {
            // Arrange
            _inAppProductsSource.GetProducts().Returns(new List<InAppProductData> { CreateFakeProductData() });
            _purchaseValidator.Validate(Arg.Any<string>()).Returns(true);
            bool purchaseComplete = false;
            
            _inAppPurchaseService.OnCompletePurchaseAction = _ => { purchaseComplete = true; };
            
            // Act
            _inAppPurchaseService.Initialize();
            var purchaseTask = _inAppPurchaseService.PurchaseProduct(FakeProductId);
            while (!purchaseTask.IsCompleted) yield return null;
            
            // Assert
            Assert.IsTrue(purchaseComplete);
        }
        
        [UnityTest]
        public IEnumerator PurchaseProduct_WhenPurchaseValidationFailed()
        {
            // Arrange
            _inAppProductsSource.GetProducts().Returns(new List<InAppProductData> { CreateFakeProductData() });
            _purchaseValidator.Validate(Arg.Any<string>()).Returns(false);
            
            // Act
            _inAppPurchaseService.Initialize();
            var purchaseTask = _inAppPurchaseService.PurchaseProduct(FakeProductId);
            while (!purchaseTask.IsCompleted) yield return null;
            
            // Assert
            _analyticsService.Received(1).LogFailedInAppPurchaseProduct(FakeProductId);
        }
        
        [Test]
        public void RestorePendingPurchases_WithValidReceipt_RestoresProduct()
        {
            // Arrange
            var fakeProgress = new ProgressData
            {
                PendingInAppProducts = new List<string> { FakeProductId }
            };
            
            _saveLoadProgressService
                .Read(Arg.Any<Func<ProgressData, bool>>())
                .Returns(ci => ci.Arg<Func<ProgressData, bool>>()(fakeProgress));
            
            _saveLoadProgressService
                .When(x => x.Write(Arg.Any<Action<ProgressData>>()))
                .Do(ci => ci.Arg<Action<ProgressData>>()(fakeProgress));
            
            _inAppProductsSource.GetProducts().Returns(new List<InAppProductData> { CreateFakeProductData() });
            _purchaseValidator.Validate(Arg.Any<string>()).Returns(true);
            
            bool restoreComplete = false;
            _inAppPurchaseProvider.OnRestoreCompletePurchase = _ => {restoreComplete = true;}; 
            
            // Act
            _inAppPurchaseService.Initialize();

            // Assert
            Assert.IsTrue(restoreComplete);
            CollectionAssert.DoesNotContain(fakeProgress.PendingInAppProducts, FakeProductId);
        }

        private InAppProductData CreateFakeProductData()
        {
            var payout = ScriptableObject.CreateInstance<PurchaseReward>();
            payout.Type = PurchaseRewardType.Resource;
            return ScriptableObject.CreateInstance<InAppProductData>();
        }
    }
}
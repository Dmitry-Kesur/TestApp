using System.Collections.Generic;
using Zenject;

namespace Infrastructure.Services
{
    public class FirebaseInitializer : IInitializable
    {
        private readonly List<IInitializeAsync> _initializeServices;

        public FirebaseInitializer(List<IInitializeAsync> initializeServices)
        {
            _initializeServices = initializeServices;
        }

        public async void Initialize()
        {
            foreach (var initializeService in _initializeServices)
            {
                await initializeService.InitializeAsync();
            }
        }
    }
}
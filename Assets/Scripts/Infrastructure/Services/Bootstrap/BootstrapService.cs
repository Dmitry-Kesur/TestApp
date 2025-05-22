using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services.Log;
using UnityEngine;

namespace Infrastructure.Services.Bootstrap
{
    public class BootstrapService
    {
        private readonly List<IBootstrapTarget> _targets;
        
        private readonly IExceptionLoggerService _loggerService;

        public BootstrapService(List<IBootstrapTarget> targets, IExceptionLoggerService loggerService)
        {
            _targets = targets.OrderBy(target => target.InitializationOrder).ToList();

            _loggerService = loggerService;
        }

        public void Initialize()
        {
            foreach (var target in _targets)
            {
                try
                {
                    target.Initialize();
                    Debug.Log($"[Bootstrap] Initializing {target.GetType().Name} (Order: {target.InitializationOrder})");
                }
                catch (Exception e)
                {
                    _loggerService.LogException(e);
                    throw;
                }
            }
        }
    }
}
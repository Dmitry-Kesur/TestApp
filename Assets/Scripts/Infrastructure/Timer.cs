using System;
using System.Collections;
using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure
{
    public class Timer
    {
        public event Action OnTimerEnd;
        
        private readonly CoroutineRunner _coroutineRunner;

        private float _remainingSeconds;

        private Coroutine _routine;

        public Timer(CoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }
        
        public void Start(float remainingSeconds)
        {
            _remainingSeconds = remainingSeconds;
            _routine = _coroutineRunner.StartCoroutine(TimerRoutine());
        }

        public void Stop()
        {
            if (_routine == null)
                return;
            
            _remainingSeconds = 0;
            _coroutineRunner.StopCoroutine(_routine);
        }
        
        private IEnumerator TimerRoutine()
        {
            yield return new WaitForSeconds(_remainingSeconds);
            Stop();
            OnTimerEnd?.Invoke();
        }
    }
}
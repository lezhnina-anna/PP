using System;
using System.Diagnostics;
using System.Threading;

namespace lab3.CriticalSection
{
    public class CriticalSection : ICriticalSection
    {
        private int _spinCount = 1;
        private int _currentState;
        private const int LockedState = 1;
        private const int UnlockedState = 0;

        public CriticalSection()
        {
            _currentState = UnlockedState;
        }

        public void SetSpinCount(int count)
        {
            _spinCount = count;
        }
        
        public void Enter()
        {
            while (true)
            {
                for(var i = 0; i < _spinCount; ++i)
                {
                    // Если значением current является unlocked, изменить его на locked.
                    if (Interlocked.CompareExchange(ref _currentState, LockedState, UnlockedState) == UnlockedState)
                    { 
                        return;
                    }
                }
                Thread.Sleep(10);
            }
        }

        public void Leave()
        {
            // Если значением current является locked, изменить его на unlocked.
            if (Interlocked.CompareExchange(ref _currentState, UnlockedState, LockedState) == UnlockedState)
            {
                Console.WriteLine("Critical section is already unlocked");
            }
        }

        public bool TryEnter(int timeout)
        {
            var watch = new Stopwatch();
            watch.Start();
            while (watch.ElapsedMilliseconds < timeout)
            {
                for(var i = 0; i < _spinCount; ++i)
                {
                    if (Interlocked.CompareExchange(ref _currentState, LockedState, UnlockedState) == UnlockedState)
                    { 
                        return true;
                    }
                }
                Thread.Sleep(10);
            }
            return false;
        }
    }
}
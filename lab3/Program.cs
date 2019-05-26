using System;
using System.Diagnostics;
using System.Threading;

namespace lab3
{
    class Program
    {
        private const int ThreadsCount = 8;
        private const int StrictMode = 0;
        private const int TryMode = 1;
        private static int _stepCount;
        private static int _timeout;
        private static int _spinCount;
        private static double _pi;
        private static readonly CriticalSection.CriticalSection CriticalSection = new CriticalSection.CriticalSection();
        
        private readonly struct ThreadArguments
        {
            public long Left { get; }
            public long Right { get; }
            public double Step { get; }
            public ThreadArguments(long left, long right, double step)
            {
                Left = left;
                Right = right;
                Step = step;
            }
        }
        
        static void Main(string[] args)
        {
            _stepCount = int.Parse(args[0]);
            _timeout = int.Parse(args[1]);
            _spinCount = int.Parse(args[2]);
            
            var watch = new Stopwatch();
            watch.Start();
            CalculatePi(StrictMode);
            watch.Stop();
            Console.WriteLine("Mode: Strict, Time: " + watch.ElapsedMilliseconds + "ms, Pi: " + _pi);

            _pi = 0;
            watch.Reset();
            
            watch.Start();
            CalculatePi(TryMode);
            watch.Stop();
            Console.WriteLine("Mode: Try, Time: " + watch.ElapsedMilliseconds + "ms, Pi: " + _pi);
        }

        private static void CalculatePi(int mode)
        {
            var threads = new Thread[ThreadsCount];
            var stepsPerThread = _stepCount / ThreadsCount;
            var step = 1.0 / _stepCount;
            CriticalSection.SetSpinCount(_spinCount);

            for(var i = 0; i < threads.Length; ++i)
            {
                var thread = mode == StrictMode ? new Thread(Calculate) : new Thread(CalculateWithTry);
                threads[i] = thread;
                var threadArguments = new ThreadArguments(i * stepsPerThread, (i + 1) * stepsPerThread, step);
                thread.Start(threadArguments);
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
        }
        
        private static void Calculate(object argument)
        {
            
            var args = (ThreadArguments)argument;
            for(var i = args.Left; i < args.Right; ++i)
            {
                var x = (i + 0.5) * args.Step;
                var value = 4.0 / (1 + x * x) * args.Step;
                CriticalSection.Enter();
                _pi += value;
                CriticalSection.Leave();
            }
        }
        
        private static void CalculateWithTry(object argument)
        {
            
            var args = (ThreadArguments)argument;
            var i = args.Left;
            while(i < args.Right)
            {
                var x = (i + 0.5) * args.Step;
                var value = 4.0 / (1 + x * x) * args.Step;
                if (!CriticalSection.TryEnter(_timeout)) continue;
                _pi += value;
                CriticalSection.Leave();
                ++i;
            }
        }
    }
}

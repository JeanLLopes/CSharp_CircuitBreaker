using CSharp_CircuitBreaker.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_CircuitBreaker.Interface
{
    interface ICircuitBreaker
    {
        CircuitBreakerStateEnum State { get; }
        void Reset();
        void Execute(Action action);
        bool IsClosed { get; }
        bool IsOpen { get; }
    }
}

using CSharp_CircuitBreaker.Interface;
using CSharp_CircuitBreaker.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_CircuitBreaker.Implementation
{
    public class CircuitBreaker : ICircuitBreaker
    {
        public CircuitBreakerStateEnum State => throw new NotImplementedException();

        public bool IsClosed => throw new NotImplementedException();

        public bool IsOpen => throw new NotImplementedException();

        public void Execute(Action action)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}

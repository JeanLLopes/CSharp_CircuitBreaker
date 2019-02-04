using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_CircuitBreaker.Model
{
    public enum CircuitBreakerStateEnum
    {
        Closed,
        Open,
        HalfOpen
    }
}

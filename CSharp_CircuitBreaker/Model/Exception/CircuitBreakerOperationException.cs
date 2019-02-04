using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_CircuitBreaker.Model.Exception
{
    public class CircuitBreakerOperationException : SystemException
    {
        public CircuitBreakerOperationException(string message) : base(message)
        {

        }
    }
}

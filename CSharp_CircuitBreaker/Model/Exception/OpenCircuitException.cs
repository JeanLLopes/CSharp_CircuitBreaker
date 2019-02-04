using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_CircuitBreaker.Model.Exception
{
    public class OpenCircuitException : SystemException
    {
        public OpenCircuitException(string message) : base(message)
        {

        }
    }
}

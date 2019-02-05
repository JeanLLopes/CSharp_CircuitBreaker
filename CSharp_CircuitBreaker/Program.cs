using CSharp_CircuitBreaker.Implementation;
using CSharp_CircuitBreaker.Model.Exception;
using System;
using System.Diagnostics;

namespace CSharp_CircuitBreaker
{
    class Program
    {
        static void Main(string[] args)
        {
            var cb = new CircuitBreaker(5,5000);
            try
            {
                cb.Execute(() =>
                {
                    throw new Exception();
                });
            }
            catch (CircuitBreakerOperationException ex)
            {
                Trace.Write(ex);
            }
            catch (OpenCircuitException )
            {
                Console.Write(cb.IsOpen);
            }
            Console.Write(cb.IsClosed);
            Console.Read();
        }
    }
}

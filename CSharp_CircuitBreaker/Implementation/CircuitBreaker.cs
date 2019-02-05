using CSharp_CircuitBreaker.Interface;
using CSharp_CircuitBreaker.Model;
using CSharp_CircuitBreaker.Model.Exception;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Timers;

namespace CSharp_CircuitBreaker.Implementation
{
    public class CircuitBreaker : ICircuitBreaker
    {
        public event EventHandler StateChanged;
        private object monitor = new object();
        public int Timeout { get; private set; }
        public int Threshold { get; private set; }
        private Timer Timer { get; set; }
        private int FailureCount { get; set; }



        public CircuitBreakerStateEnum State { get; private set; }
        private Action Action { get; set; }
        public bool IsClosed { get { return State == CircuitBreakerStateEnum.Closed; } }
        public bool IsOpen { get { return State == CircuitBreakerStateEnum.Open; } }



        public CircuitBreaker(int threshold = 5, int timeout = 60000)
        {
            if (threshold <= 0)
            {
                throw new ArgumentOutOfRangeException($"{threshold} deve ser maior que zero");
            }
                
            if (timeout <= 0)
            {
                throw new ArgumentOutOfRangeException($"{timeout} deve ser maior que zero");
            }
                
            this.Threshold = threshold;
            this.Timeout = timeout;
            this.State = CircuitBreakerStateEnum.Closed;
            this.Timer = new Timer(timeout);
            this.Timer.Enabled = false;
            this.Timer.Elapsed += Timer_Elapsed;
        }

        public void Execute(Action action)
        {
            if (this.State == CircuitBreakerStateEnum.Open)
            {
                throw new OpenCircuitException("Circuit breaker is currently open");
            }
            lock (monitor)
            {
                try
                {
                    this.Action = action;
                    this.Action();
                }
                catch (Exception ex)
                {
                    if (this.State == CircuitBreakerStateEnum.HalfOpen)
                    {
                        Trip();
                    }
                    else if (this.FailureCount <= this.Threshold)
                    {
                        this.FailureCount++;

                        //Ativa o Retry
                        if (this.Timer.Enabled == false)
                        {
                            this.Timer.Enabled = true;
                        }
                            
                    }
                    else if (this.FailureCount >= this.Threshold)
                    {
                        Trip();
                    }

                    throw new CircuitBreakerOperationException("Operation failed", ex);
                }
                if (this.State == CircuitBreakerStateEnum.HalfOpen)
                {
                    Reset();
                }
                if (this.FailureCount > 0)
                {
                    this.FailureCount--;
                }
            }
        }


        public void Reset()
        {
            if (this.State != CircuitBreakerStateEnum.Closed)
            {
                Trace.WriteLine($"Circuito Closed");
                ChangeState(CircuitBreakerStateEnum.Closed);
                this.Timer.Stop();
            }
        }

        #region Machine State Handles
        private void Trip()
        {
            if (this.State != CircuitBreakerStateEnum.Open)
            {
                Trace.WriteLine($"Circuito Aberto");
                ChangeState(CircuitBreakerStateEnum.Open);
            }
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (monitor)
            {
                try
                {
                    Trace.WriteLine($"Retry, Execução nº { this.FailureCount}");
                    Execute(this.Action);
                    Reset();
                }
                catch
                {
                    if (this.FailureCount > this.Threshold)
                    {
                        Trip();
                        this.Timer.Elapsed -= Timer_Elapsed;
                        this.Timer.Enabled = false;
                        this.Timer.Stop();

                    }
                }
            }
        }
        private void ChangeState(CircuitBreakerStateEnum state)
        {
            this.State = state;
            this.OnCircuitBreakerStateChanged(new EventArgs() { });
        }
        private void OnCircuitBreakerStateChanged(EventArgs e)
        {
            if (this.StateChanged != null)
            {
                StateChanged(this, e);
            }
        }
        #endregion
    }
}

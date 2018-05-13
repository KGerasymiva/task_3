using System;

namespace Parking
{
    internal class InternalTimer
    {
        private System.Timers.Timer Timer { get; }

        private readonly Action _action;


        public InternalTimer(double interval, Action action)
        {
            _action = action;
            Timer = new System.Timers.Timer(interval);
            Timer.Elapsed += OnTimer1Elapsed;
            Timer.Enabled = true;

        }

        private void OnTimer1Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _action();
        }

    }
}

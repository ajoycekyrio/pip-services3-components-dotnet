﻿using System;

namespace PipServices3.Components.Count
{
    /// <summary>
    /// Callback object returned by <see cref="ICounters.BeginTiming(string)"/> to end timing
    /// of execution block and update the associated counter.
    /// </summary>
    /// <example>
    /// <code>
    /// var timing = counters.beginTiming("mymethod.exec_time");
    /// try {
    /// ...
    /// } finally {
    /// timing.endTiming();
    /// }
    /// </code>
    /// </example>
    public class Timing : IDisposable
    {
        private readonly int _start;
        private readonly ITimingCallback _callback;
        private readonly string _counter;

        /// <summary>
        /// Creates a new instance of the timing callback object.
        /// </summary>
        public Timing() { }

        /// <summary>
        /// Creates a new instance of the timing callback object.
        /// </summary>
        /// <param name="counter">an associated counter name</param>
        /// <param name="callback">a callback that shall be called when endTiming is called.</param>
        public Timing(string counter, ITimingCallback callback)
        {
            _counter = counter;
            _callback = callback;
            _start = Environment.TickCount;
        }

        /// <summary>
        /// Ends timing of an execution block, calculates elapsed time and updates the associated counter.
        /// </summary>
        public void EndTiming()
        {
            if (_callback == null)
                return;

            double elapsed = Environment.TickCount - _start;

            _callback.EndTiming(_counter, elapsed);
        }

        public void Dispose()
        {
            EndTiming();
        }
    }
}

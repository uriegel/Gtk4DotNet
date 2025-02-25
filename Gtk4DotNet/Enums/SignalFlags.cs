namespace GtkDotNet;

[Flags]
public enum SignalFlags
{
    /// <summary>
    /// Invoke the object method handler in the first emission stage.
    /// </summary>
    RunFirst = 1,
    /// <summary>
    /// Invoke the object method handler in the third emission stage.
    /// </summary>
    RunLast = 2, 
    /// <summary>
    /// Invoke the object method handler in the last emission stage.
    /// </summary>
    RunCleanUp = 4,
    /// <summary>
    /// Signals being emitted for an object while currently being in emission for this very object will not be emitted recursively, but instead cause the first emission to be restarted.
    /// </summary>
    NoRecurse = 8,
    /// <summary>
    /// This signal supports “::detail” appendices to the signal name upon handler connections and emissions.
    /// </summary>
    Detailed = 16,
    /// <summary>
    /// Action signals are signals that may freely be emitted on alive objects from user code via g_signal_emit() and friends, without the need of being embedded into extra code that performs pre or post emission adjustments on the object. They can also be thought of as object methods which can be called generically by third-party code.
    /// </summary>
    Action = 32,
    /// <summary>
    /// No emissions hooks are supported for this signal.
    /// </summary>
    NoHooks = 64,
    /// <summary>
    /// Varargs signal emission will always collect the arguments, even if there are no signal handlers connected. Since 2.30.
    /// </summary>
    MustCollect = 128,
    /// <summary>
    /// The signal is deprecated and will be removed in a future version. A warning will be generated if it is connected while running with G_ENABLE_DIAGNOSTIC=1. Since 2.32.
    /// </summary>
    Deprecated = 256,
    /// <summary>
    /// Only used in GSignalAccumulator accumulator functions for the GSignalInvocationHint::run_type field to mark the first call to the accumulator function for a signal emission. Since 2.68.
    /// </summary>
    AccumulatorFirstRun = 131072
}

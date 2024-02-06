namespace TcHaxx.Snappy.CLI;

/// <summary>
/// Exit codes for this application.
/// </summary>
internal enum ExitCodes : int
{
    /// <summary>
    /// Exit code: No error.
    /// </summary>
    E_NOERROR = 0,

    /// <summary>
    /// Exit code: General exception occured.
    /// </summary>
    E_EXCEPTION = 1,

    /// <summary>
    /// Exit code: Parsing arguments and/or arguments missing/wrong.
    /// </summary>
    E_CLIOPTIONS = 2

}

namespace Bemo

open System
open System.IO
open System.Text

module Logger =

    let private logDirectory =
        let appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
        let logDir = Path.Combine(appData, "WindowTabs", "logs")
        if not (Directory.Exists(logDir)) then
            Directory.CreateDirectory(logDir) |> ignore
        logDir

    let private logFilePath =
        let timestamp = DateTime.Now.ToString("yyyy-MM-dd")
        Path.Combine(logDirectory, sprintf "WindowTabs-%s.log" timestamp)

    let private lockObj = obj()

    let private writeLog level (message: string) =
        try
            lock lockObj (fun () ->
                let timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                let logLine = sprintf "[%s] [%s] %s" timestamp level message
                File.AppendAllText(logFilePath, logLine + Environment.NewLine)
            )
        with
        | ex ->
            // If logging fails, write to debug output as fallback
            System.Diagnostics.Debug.WriteLine(sprintf "Logger failed: %s" ex.Message)

    let info (message: string) =
        writeLog "INFO" message

    let warning (message: string) =
        writeLog "WARN" message

    let error (message: string) =
        writeLog "ERROR" message

    let errorWithException (message: string) (ex: Exception) =
        let sb = StringBuilder()
        sb.AppendLine(message) |> ignore
        sb.AppendLine(sprintf "Exception: %s" ex.Message) |> ignore
        sb.AppendLine(sprintf "Type: %s" (ex.GetType().FullName)) |> ignore
        sb.AppendLine(sprintf "Stack Trace:\n%s" ex.StackTrace) |> ignore

        // Log inner exceptions
        let mutable innerEx = ex.InnerException
        let mutable level = 1
        while innerEx <> null do
            sb.AppendLine(sprintf "Inner Exception [%d]: %s" level innerEx.Message) |> ignore
            sb.AppendLine(sprintf "Stack Trace:\n%s" innerEx.StackTrace) |> ignore
            innerEx <- innerEx.InnerException
            level <- level + 1

        writeLog "ERROR" (sb.ToString())

    let fatal (message: string) (ex: Exception) =
        let sb = StringBuilder()
        sb.AppendLine("=== FATAL ERROR ===") |> ignore
        sb.AppendLine(message) |> ignore
        sb.AppendLine(sprintf "Exception: %s" ex.Message) |> ignore
        sb.AppendLine(sprintf "Type: %s" (ex.GetType().FullName)) |> ignore
        sb.AppendLine(sprintf "Stack Trace:\n%s" ex.StackTrace) |> ignore

        // Add system information
        sb.AppendLine("") |> ignore
        sb.AppendLine("=== System Information ===") |> ignore
        sb.AppendLine(sprintf "OS: %s" (Environment.OSVersion.ToString())) |> ignore
        sb.AppendLine(sprintf "CLR: %s" (Environment.Version.ToString())) |> ignore
        sb.AppendLine(sprintf "64-bit OS: %b" Environment.Is64BitOperatingSystem) |> ignore
        sb.AppendLine(sprintf "64-bit Process: %b" Environment.Is64BitProcess) |> ignore
        sb.AppendLine(sprintf "Working Set: %d MB" (Environment.WorkingSet / 1024L / 1024L)) |> ignore
        sb.AppendLine(sprintf "Processor Count: %d" Environment.ProcessorCount) |> ignore

        // Log inner exceptions
        let mutable innerEx = ex.InnerException
        let mutable level = 1
        while innerEx <> null do
            sb.AppendLine("") |> ignore
            sb.AppendLine(sprintf "Inner Exception [%d]: %s" level innerEx.Message) |> ignore
            sb.AppendLine(sprintf "Type: %s" (innerEx.GetType().FullName)) |> ignore
            sb.AppendLine(sprintf "Stack Trace:\n%s" innerEx.StackTrace) |> ignore
            innerEx <- innerEx.InnerException
            level <- level + 1

        sb.AppendLine("=== END FATAL ERROR ===") |> ignore
        sb.AppendLine("") |> ignore

        writeLog "FATAL" (sb.ToString())

    let getLogDirectory() = logDirectory

    let getLogFilePath() = logFilePath

    /// Gets the last N lines from the current log file
    let getRecentLogs lineCount =
        try
            if File.Exists(logFilePath) then
                let lines = File.ReadAllLines(logFilePath)
                let count = min lineCount lines.Length
                let startIndex = lines.Length - count
                lines.[startIndex..]
            else
                [||]
        with
        | _ -> [||]

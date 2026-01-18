namespace Bemo
open System
open System.Windows.Forms

type ExceptionHandlerPlugin() as this =

    member this.onException(e:UnhandledExceptionEventArgs) =
        try
            match e.ExceptionObject with
            | :? Exception as ex ->
                // Log the fatal error with full details
                Logger.fatal "Unhandled exception caught by ExceptionHandlerPlugin" ex

                // Show user-friendly error dialog
                let errorMessage =
                    sprintf "WindowTabs encountered an unexpected error and needs to close.\n\n" +
                    sprintf "Error: %s\n\n" ex.Message +
                    sprintf "A detailed error log has been saved to:\n%s\n\n" (Logger.getLogFilePath()) +
                    sprintf "Please report this issue at: https://github.com/leafOfTree/WindowTabs/issues"

                MessageBox.Show(
                    errorMessage,
                    "WindowTabs - Fatal Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error) |> ignore

                // If terminating, give user a chance to read the message
                if e.IsTerminating then
                    System.Threading.Thread.Sleep(500)
            | _ ->
                // Non-exception object thrown (rare but possible)
                Logger.error (sprintf "Non-exception object thrown: %A" e.ExceptionObject)
                MessageBox.Show(
                    "WindowTabs encountered an unexpected error (non-exception object).\n\n" +
                    "A log has been saved to:\n" + Logger.getLogFilePath(),
                    "WindowTabs - Fatal Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error) |> ignore
        with
        | ex ->
            // Last resort: if error handling itself fails
            System.Diagnostics.Debug.WriteLine(sprintf "Exception handler failed: %s" ex.Message)
            MessageBox.Show(
                "WindowTabs encountered a critical error. Please check the logs.",
                "WindowTabs - Critical Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error) |> ignore

    interface IPlugin with
        member x.init() =
            // Log plugin initialization
            Logger.info "ExceptionHandlerPlugin initialized - unhandled exception monitoring started"
            AppDomain.CurrentDomain.UnhandledException.Add this.onException
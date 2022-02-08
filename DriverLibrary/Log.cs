using System;
using System.IO;

namespace DriverLibrary
{
    
    public static class Log
    {
        /// <summary>
        /// Write messages and exceptions to the log file
        /// Log file will be saved the C drive
        /// </summary>
        /// <param name="message">Log message to be recorded</param>
        /// <param name="functionName">Function name that is writing to the log, defaults to an empty string</param>
        /// <param name="exception">Exception to be recorded in the log, defaults to an empty string</param>
        public static void Write(string message, string functionName = "", string exception = "")
        {
            //Declare the path where log will be saved
            string path = @"C:\Logs\";

            //Check if directory exists
            if (!Directory.Exists(path))
            {
                //Create the directory if it does not exist
                Directory.CreateDirectory(path);
            }
            using (StreamWriter file = new StreamWriter(Path.Combine(path, "log.txt"), true))
            {
                //Write the message, function and exception to the log file
                file.WriteLine(AddLogMessage(message, functionName, exception));
            }

            //Write the message to the Console window
            Console.WriteLine(message);
        }

        /// <summary>
        /// Formats the log message to be added to the log file
        /// </summary>
        /// <param name="message">Log message to be recorded</param>
        /// <param name="functionName">Function name that is writing to the log, defaults to an empty string</param>
        /// <param name="exception">Exception to be recorded in the log, defaults to an empty string</param>
        /// <returns>return timestamp, message, functionName and exception as one string to be added to the log file</returns>
        public static string AddLogMessage(string message, string functionName = "", string exception = "")
        {
            //Get current time stamp
            string timeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff");

            //return timestamp, message, functionName and exception as one string
            return (timeStamp) + ": " + message + " " + functionName + " " + exception;
        }
    }
}

/*
MIT License

Copyright (c) 2022 WeiJun Syu

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;

// Perform a mathematical modulo operation: a modulo b.
// NOT to be confused with the built in % operator in C# which is a REMAINDER function.
public static int Modulo(int a, int b)
{
    return (a % b + b) % b;
}

// Converts a percentage value of audio to decibels given a min and max decibel value.
// Uses log to calculate decibels to beter simulate human hearing of audio levels. (Rather than a linear path)
public static double ConvertPercentageRawToDecibels(double percentValue, double max, double min)
{
    if (percentValue == 0)
    {
        return min;
    }
    return Math.Clamp(Math.Log(percentValue, 10) * 20, max, min);
}

// Simple function to log out some message to some file.
// Does NOT check for invalid message, logPath, etc.
// Date and time is given in UTC+0 NOT local time.
// mode=-1: Do not overwrite existing file; do nothing if log file already exists on disk.
// mode=0:  Append to existing log file.
// mode=1:  Overwrite if file already exists on disk.
public static bool LogToFileSimple(string message,
                                   int mode=0, string logPath="",
                                   bool useGUID=false, bool usePMID=false,                              // File name options
                                   bool fullSourcePath=false, bool doTrace=false, bool logTime=true,    // Log message options
                                   [System.Runtime.CompilerServices.CallerFilePathAttribute] string sourceFile=null,
                                   [System.Runtime.CompilerServices.CallerMemberNameAttribute] string methodName=null,
                                   [System.Runtime.CompilerServices.CallerLineNumberAttribute] int lineNumber=-1,)
{
    const string DEFAULT_LOG_PATH = "C:\\Logs\\LogFile"; // Given without file extension (.log to be appended later)
    const string EXT = ".log";

    // Init logPath
    if (System.String.IsNullOrEmpty(logPath))
    {
        logPath = DEFAULT_LOG_PATH;
    }
    // Build final message string
    if (!fullSourcePath)
    {
        sourceFile = System.IO.Path.GetFileName(sourceFile);
    }
    if (doTrace)
    {
        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        message += "\nStackTrace:\n" + stackTrace.ToString();
    }
    message = "'" + sourceFile + "' : " + methodName + " @ Line: " + lineNumber + "\n  " + message + "\n";
    if (logTime)
    {
        message = System.DateTime.UtcNow.ToString("yyyy-MM-dd;HH:mm:ss:fff") + " " + message;
    }
    // Build final logPath with extension
    if (useGUID)
    {
        logPath += System.Guid.NewGuid().ToString();
    }
    if (usePMID)
    {
        logPath += "." + System.Diagnostics.Process.GetCurrentProcess().Id.ToString() + "." + System.Environment.CurrentManagedThreadId.ToString();
    }
    logPath += EXT;
    // Try log out
    if (mode == 0 || !System.IO.File.Exists(logPath))
    {
        return _LogOut(message, logPath, true);
    }
    else if (mode > 0)
    {
        return _LogOut(message, logPath, false);
    }
    return false;

    // Helper function to log out to file
    bool _LogOut(string finalMessage, string finalLogPath, bool append)
    {
        Try
        {
            using (System.IO.StreamWriter output = new System.IO.StreamWriter(finalLogPath, append))
            {
                output.Write(finalMessage);
            }
            return true;
        }
        catch(System.Exception e)
        {
            // Do not halt execution
            return false;
        }
    }
}
/**
 * MDCTool
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@jmp.cx>
 */
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace MDCTool
{
    /// <summary>
    /// Implements simple handler to dump an exception.
    /// </summary>
    public class Messages
    {
        /**
         * Fields
         */
        private static TextWriter tw = new StreamWriter("Trace.log", false);

        /**
         * Methods
         */
        /// <summary>
        /// Writes the exception stack trace to the received stream
        /// </summary>
        /// <param name="errorMessage">Error message</param>
        /// <param name="throwable">Exception to obtain information from</param>
        public static void Write(string errorMessage, Exception throwable)
        {
#if DEBUG
            StringBuilder sb = new StringBuilder();
            MethodBase mb = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod();
            ParameterInfo[] param = mb.GetParameters();
            string funcParams = string.Empty;
            for (int i = 0; i < param.Length; i++)
                if (i < param.Length - 1)
                    funcParams += param[i].ParameterType.Name + ", ";
                else
                    funcParams += param[i].ParameterType.Name;

            sb.AppendLine(errorMessage + " " + throwable.Message + "\n");
            sb.AppendLine("<" + mb.ReflectedType.Name + "::" + mb.Name + "(" + funcParams + ")>");
            sb.AppendLine(throwable.GetType().ToString());
            sb.AppendLine(throwable.Source);
            sb.AppendLine("\n" + throwable.StackTrace);
            sb.AppendLine("\n");

            string trace = "---- SNIP ----\n" + sb.ToString() + "\n" + "---- SNIP ----";
#if TRACE
            System.Diagnostics.Trace.WriteLine(trace);
#endif
            tw.WriteLine(trace);
            tw.Flush();
            MessageBox.Show(sb.ToString(), "MDCTool Exception Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
        }

        /// <summary>
        /// Writes the exception stack trace to the received stream
        /// </summary>
        /// <param name="errorMessage">Error message</param>
        /// <param name="throwable">Exception to obtain information from</param>
        public static void WriteNoReport(string errorMessage, Exception throwable)
        {
#if DEBUG
            StringBuilder sb = new StringBuilder();
            MethodBase mb = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod();
            ParameterInfo[] param = mb.GetParameters();
            string funcParams = string.Empty;
            for (int i = 0; i < param.Length; i++)
                if (i < param.Length - 1)
                    funcParams += param[i].ParameterType.Name + ", ";
                else
                    funcParams += param[i].ParameterType.Name;

            sb.AppendLine(errorMessage + "\n" + throwable.Message + "\n");
            sb.AppendLine("<" + mb.ReflectedType.Name + "::" + mb.Name + "(" + funcParams + ")>");
            sb.AppendLine(throwable.GetType().ToString());
            sb.AppendLine(throwable.Source);
            sb.AppendLine("\n" + throwable.StackTrace);

            string trace = "---- SNIP ----\n" + sb.ToString() + "\n" + "---- SNIP ----";
#if TRACE
            System.Diagnostics.Trace.WriteLine(trace);
#endif
            tw.WriteLine(trace);
            tw.Flush();
#endif
        }

        /// <summary>
        /// Writes a trace message w/ calling function information.
        /// </summary>
        /// <param name="message">Message to print to debug window</param>
        public static void Trace(string message)
        {
#if DEBUG
            string timeString = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            MethodBase mb = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod();
            ParameterInfo[] param = mb.GetParameters();
            string funcParams = string.Empty;
            for (int i = 0; i < param.Length; i++)
                if (i < param.Length - 1)
                    funcParams += param[i].ParameterType.Name + ", ";
                else
                    funcParams += param[i].ParameterType.Name;

            string trace = timeString + " <" + mb.ReflectedType.Name + "::" + mb.Name + "(" + funcParams + ")> " + message;
#if TRACE
            System.Diagnostics.Trace.WriteLine(trace);
#endif
            tw.WriteLine(trace);
            tw.Flush();
#endif
        }

        public static void TraceHex(string message, byte[] buffer, int maxLength = 32, int startOffset = 0)
        {
            int bCount = 0, j = 0, lenCount = 0;

            // iterate through buffer printing all the stored bytes
            string traceMsg = message + " Off [" + j.ToString("X4") + "] -> [";
            for (int i = startOffset; i < buffer.Length; i++)
            {
                byte b = buffer[i];

                // split the message every 16 bytes...
                if (bCount == 16)
                {
                    traceMsg += "]";
                    Console.WriteLine(traceMsg);

                    bCount = 0;
                    j += 16;
                    traceMsg = message + " Off [" + j.ToString("X4") + "] -> [";
                }
                else
                    traceMsg += (bCount > 0) ? " " : "";

                traceMsg += b.ToString("X2");

                bCount++;

                // increment the length counter, and check if we've exceeded the specified
                // maximum, then break the loop
                lenCount++;
                if (lenCount > maxLength)
                    break;
            }

            // if the byte count at this point is non-zero print the message
            if (bCount != 0)
            {
                traceMsg += "]";
                Console.WriteLine(traceMsg);
            }
        }
    } // public class Messages
} // namespace MDCTool

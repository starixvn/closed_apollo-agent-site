// <copyright file="EntLibLogger.cs" company="IBC">
// Copyright (c) Sunplus. All rights reserved. 
// </copyright>
//
// <summary>
//  Implement EntLibLoggingService
// </summary>
//
// <history>
//  <change who="Lewin" date="2010.01.14">Created</change>
//  <change who="Lewin" date="2010.06.24">Change filename</change>
// </history>
namespace Sunplus.Core.LoggingLib
{
    using System;
    using System.Diagnostics;
    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

    /// <summary>
    /// EntLibLogger inherit from ILogger
    /// </summary>
    public class EntLibLogger : ILogger
    {
        private const string DefaultLogCategory = "General";
        
        #region Debug functions
        /// <summary>
        /// Debug the specified format and category
        /// </summary>
        /// <param name="message">the message</param>
        /// <param name="category">the category</param>
        public void Debug(string message, string category)
        {
            WriteLog(message, category, LogLevel.Debug);
        }

        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Debug(string message)
        {
            this.Debug(message, DefaultLogCategory);
        }

        /// <summary>
        /// Debugs the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void Debug(string format, params object[] args)
        {
            Debug(string.Format(format, args));
        }

        /// <summary>
        /// Debug specified format and cateogry
        /// </summary>
        /// <param name="format">the format</param>
        /// <param name="category">the category</param>
        /// <param name="args">The args.</param>
        public void Debug(string format, string category, params object[] args)
        {
            Debug(string.Format(format, args), category);
        }
        #endregion

        #region Info functions
        /// <summary>
        /// Info the specified message and category
        /// </summary>
        /// <param name="mesaage">the message</param>
        /// <param name="category">the category</param>
        public void Info(string mesaage, string category)
        {
            WriteLog(mesaage, category, LogLevel.Info);
        }

        /// <summary>
        /// Infoes the specified message and default log category
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(string message)
        {
            Info(message, DefaultLogCategory);
        }

        /// <summary>
        /// Infoes the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void Info(string format, params object[] args)
        {
            Info(string.Format(format, args));
        }

        public void Info(string format, string category, params object[] args)
        {
            Info(string.Format(format, args), category);
        }
        #endregion

        #region Warning functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        public void Warn(string message, string category)
        {
            WriteLog(message, category, LogLevel.Warn);
        }

        /// <summary>
        /// Warns the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warn(string message)
        {
            Warn(message, DefaultLogCategory);
        }

        /// <summary>
        /// Warns the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void Warn(string format, params object[] args)
        {
            Warn(string.Format(format, args));
        }

        public void Warn(string format, string category, params object[] args)
        {
            Warn(string.Format(format, args), category);
        }

        #endregion

        #region Error functions

        /// <summary>
        /// Wrire error with specified message and category
        /// </summary>
        /// <param name="message">the message</param>
        /// <param name="category">the category</param>
        public void Error(string message, string category)
        {
            WriteLog(message, category, LogLevel.Error);
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error(string message)
        {
            Error(message, DefaultLogCategory);
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Error(string message, Exception exception)
        {
            Error(message, DefaultLogCategory, exception);
        }

        /// <summary>
        /// Error the specifed message and category
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// <param name="exception"></param>
        public void Error(string message, string category, Exception exception)
        {
            Error(string.Format("{0}\n{1}", message, exception), category);
        }

        #endregion

        #region Critical

        public void Critical(string message, string category) 
        {
            WriteLog(message, category, LogLevel.Critical);
        }

        /// <summary>
        /// Fatals the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Critical(string message)
        {
            Critical(message, DefaultLogCategory);
        }

        /// <summary>
        /// Fatals the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void Critical(string format, params object[] args)
        {
            Critical(string.Format(format, args));
        }

        /// <summary>
        /// Fatals the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="category">The category.</param>
        /// <param name="args">The args</param>
        public void Critical(string format, string category, params object[] args)
        {
            Critical(string.Format(format, args), category);
        }

        #endregion

        #region WriteLog functions
        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="level">The level.</param>
        private static void WriteLog(string message, string category, LogLevel level)
        {
            try
            {
                var severity = TraceEventType.Information;
                switch (level)
                {
                    case (LogLevel.Debug):
                        severity = TraceEventType.Verbose;
                        break;
                    case (LogLevel.Warn):
                        severity = TraceEventType.Warning;
                        break;
                    case (LogLevel.Critical):
                        severity = TraceEventType.Critical;
                        break;
                    case (LogLevel.Error):
                        severity = TraceEventType.Error;
                        break;
                }

                //create log entry instance title temporarily set by value category
                var logEntry = new LogEntry {Severity = severity, Message = message, Title = category };
                if (string.IsNullOrEmpty(category))
                {
                    WriteLog(message, level);
                }
                else 
                {
                    logEntry.Categories.Add(category);
                }

                //LogWriter writer = EnterpriseLibraryContainer.Current.GetInstance<LogWriter>();
                //writer.Write(logEntry);

                Logger.Write(logEntry);
                

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Write log with default category
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        private static void WriteLog(string message, LogLevel level)
        {
            WriteLog(message, DefaultLogCategory, level);
        }
        #endregion
    }
}
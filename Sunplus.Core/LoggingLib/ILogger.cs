// <copyright file="ILogger.cs" company="IBC">
// Copyright (c) Sunplus. All rights reserved. 
// </copyright>
//
// <summary>
//  Interface of ILogger
// </summary>
//
// <history>
//  <change who="Lewin" date="2010.02.29">Create</change>
//  <change who="Lewin" date="2010.06.22">Upgrade (Using Entlib 5.0)</change>
//  <change who="Lewin" date="2010.06.23">Change file name</change>
// </history>
namespace Sunplus.Core.LoggingLib
{
    using System;
    using Microsoft.Practices.EnterpriseLibrary.Logging;


    /// <summary>
    /// LogLevel enum Debug, Info, Warn, Error, Critical
    /// </summary>
    internal enum LogLevel
    {
        /// <summary>
        /// Debug enum
        /// </summary>
        Debug,
        /// <summary>
        /// Info enum
        /// </summary>
        Info,
        /// <summary>
        /// Warn enum
        /// </summary>
        Warn,
        /// <summary>
        /// Error enum
        /// </summary>
        Error,
        /// <summary>
        /// Critical enum
        /// </summary>
        Critical
    }

    /// <summary>
    /// Interface ILogger
    /// </summary>
    public interface ILogger
    {

        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Debug(string message);
        void Debug(string message, string category);
        void Debug(string format, params object[] args);
        void Debug(string format, string category, params object[] args);

        void Info(string message);
        void Info(string mesaage, string category);
        void Info(string format, params object[] args);
        void Info(string format, string category, params object[] args);

        void Warn(string message);
        void Warn(string message, string category);
        void Warn(string format, params object[] args);
        void Warn(string format, string category, params object[] args);

        void Error(string message);
        void Error(string message, string category);
        void Error(string message, Exception exception);
        void Error(string message, string category, Exception exception);

        void Critical(string message);
        void Critical(string message, string category);
        void Critical(string format, params object[] args);
        void Critical(string format, string category, params object[] args);
    }

   
}
// <copyright file="XmlFormatter.cs" company="IBC">
// Copyright (c) Sunplus. All rights reserved. 
// </copyright>
//
// <summary>
//  Create custom log data format - xml format
// </summary>
//
// <history>
//  <change who="Lewin" date="2010.06.23">Created</change>
// </history>

namespace Sunplus.Core.LoggingLib.Formatter
{
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Xml.Linq;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

    /// <summary>
    /// XmlFormatter is a custom log formatter
    /// </summary>
    [ConfigurationElementType(typeof(CustomFormatterData))]    
    public class XmlFormatter : LogFormatter
    {
        /// <summary>
        /// NameValueCollection m_Attributes
        /// </summary>
        private NameValueCollection attributes = null;

        /// <summary>
        /// Initializes a new instance of the XmlFormatter class
        /// Author: Lewin
        /// </summary>
        /// <param name="attrs">NameValueCollection attributes</param>
        public XmlFormatter(NameValueCollection attrs)
        {
            this.attributes = attrs;
        }

        /// <summary>
        /// override format function
        /// </summary>
        /// <param name="log">valid LogEntry object</param>
        /// <returns>return an xml string</returns>
        public override string Format(LogEntry log)
        {
            XElement xelement = new XElement(
                "LogEntry",
                new XElement("Timestamp", log.TimeStampString),
                new XElement("Message", string.Format("<![CDATA[{0}]]>", log.Message)),
                new XElement("Priority", log.Priority.ToString(CultureInfo.InvariantCulture)),
                new XElement("EventId", log.EventId),
                new XElement("Severity", log.Severity),
                new XElement("Title", string.Format("<![CDATA[{0}]]>", log.Title)),
                new XElement("Machine", log.MachineName),
                new XElement("AppDomain", log.AppDomainName),
                new XElement("ProcessId", log.ProcessId),
                new XElement("ProcessName", log.ProcessName),
                new XElement("ThreadName", log.ManagedThreadName));
            
            foreach (string key in log.ExtendedProperties.Keys)
            {
                xelement.Add(new XElement(key, log.ExtendedProperties[key]));
            }

            return xelement.ToString();
        }
    }
}

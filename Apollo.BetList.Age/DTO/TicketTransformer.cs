// ---------------------------------------------------------------------------------
// <copyright file="TicketTransformer.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.18">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Age.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Apollo.BetList.Core.Entities;

    /// <summary>
    /// Ticket transformer.
    /// </summary>
    public static class TicketTransformer
    {
        /// <summary>
        /// Transforms the specified data.
        /// </summary>
        /// <param name="data">The data table.</param>
        /// <returns>List of tickets.</returns>   
        public static IList<ITicket> Transform(DataTable data)
        {
            IList<ITicket> tickets = new List<ITicket>();
            Type typeTicket = typeof(ITicket);
            PropertyInfo[] typeTicketProps = typeTicket.GetProperties();
            string mapKey = string.Empty;

            foreach (DataRow row in data.Rows)
            {
                ITicket t = new Ticket();
                foreach (PropertyInfo prop in typeTicketProps)
                {
                    mapKey = string.Empty;
                    if (TicketMapper.Dict.TryGetValue(prop.Name, out mapKey) && mapKey.Length > 0)
                    {
                        if (data.Columns.Contains(mapKey))
                        {
                            object value;
                           
                            try
                            {
                                if (row[mapKey] == DBNull.Value)
                                {
                                    value = null;
                                }
                                else
                                {                                    
                                    Type underlyingType = Nullable.GetUnderlyingType(prop.PropertyType);

                                    if (underlyingType == null)
                                    {
                                        underlyingType = prop.PropertyType;
                                    }

                                    value = Convert.ChangeType(row[mapKey], underlyingType);
                                }

                                prop.SetValue(t, value, null);
                            }
                            catch
                            {
                            }
                        }
                    }
                }

                tickets.Add(t);
            }

            return tickets;
        }
    }
}
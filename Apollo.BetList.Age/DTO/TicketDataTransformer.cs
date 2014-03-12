// --------------------------------------------------------------------------
// <copyright file="TicketDataTransformer.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.11.1">Created</change>
// </history>

namespace Apollo.BetList.Age.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;
    using Apollo.BetList.Core.Entities;

    /// <summary>
    /// Ticket data transformer.
    /// </summary>
    public static class TicketDataTransformer
    {
        /// <summary>
        /// Transforms the specified data.
        /// </summary>
        /// <param name="data">The data table.</param>
        /// <returns>List of tickets.</returns>
        public static List<ITicketData> Transform(DataTable data)
        {
            List<ITicketData> tickets = new List<ITicketData>();
            if (data == null)
            {
                return tickets;
            }

            Type typeTicket = typeof(ITicketData);
            PropertyInfo[] typeTicketProps = typeTicket.GetProperties();
            string mapKey = string.Empty;
            foreach (DataRow row in data.Rows)
            {
                ITicketData t = new TicketData();
                foreach (PropertyInfo prop in typeTicketProps)
                {
                    mapKey = string.Empty;
                    if (TicketDataMapper.Dict.TryGetValue(prop.Name, out mapKey) && mapKey.Length > 0)
                    {
                        if (data.Columns.Contains(mapKey))
                        {
                            object value;

                            // Determine if the property is DBNull
                            if (row[mapKey] == DBNull.Value)
                            {
                                value = null;
                            }
                            else
                            {
                                // Determine if the property is nullable
                                Type underlyingType = Nullable.GetUnderlyingType(prop.PropertyType);

                                if (underlyingType == null)
                                {
                                    underlyingType = prop.PropertyType;
                                }

                                value = Convert.ChangeType(row[mapKey], underlyingType);
                            }

                            prop.SetValue(t, value, null);
                        }
                    }
                }

                tickets.Add(t);
            }

            return tickets;
        }
    }
}
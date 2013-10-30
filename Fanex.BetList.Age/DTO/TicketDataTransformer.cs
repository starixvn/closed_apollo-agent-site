// -----------------------------------------------------------------------
// <copyright file="TicketDataTransformer.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.BetList.Age.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class TicketDataTransformer
    {
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
                    if (TicketDataMapper.dict.TryGetValue(prop.Name, out mapKey) && mapKey.Length > 0)
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

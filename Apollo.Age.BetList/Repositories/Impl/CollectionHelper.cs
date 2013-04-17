// -----------------------------------------------------------------------
// <copyright file="CollectionHelper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Collection Helper convert to Object
// </summary>
//
// <history>
//  <change who="Kaden.Trinh" date="2013.04.17">Created</change>
// </history>

namespace Apollo.Age.BetList.Repositories.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;

    public class CollectionHelper
    {
        private CollectionHelper()
        {
        }

        public static T CreateItem<T>(DataRow row)
        {
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();
                var columnName = string.Empty;

                foreach (DataColumn column in row.Table.Columns)
                {
                    foreach (System.Reflection.PropertyInfo info in obj.GetType().GetProperties())
                    {
                        if (info.CanRead)
                        {
                            string propertyName = info.Name.ToLower();
                            if (propertyName == column.ColumnName.ToLower())
                            {
                                PropertyInfo prop = obj.GetType().GetProperty(info.Name);
                                object value = row[column.ColumnName];
                                if (value != DBNull.Value)
                                {
                                    prop.SetValue(obj, value, null);
                                }

                                break;
                            }
                        }
                    }
                }
            }

            return obj;
        }

        /// <summary>
        /// Converting Custom Collections
        /// </summary>
        /// <typeparam name="T">type parameter T</typeparam>
        /// <param name="rows">Row</param>
        /// <returns>Return List T</returns>
        public static IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }

            return list;
        }

        /// <summary>
        /// Converting Custom Collections To and From DataTable
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="table">Table</param>
        /// <returns>Return list</returns>
        public static IList<T> ConvertTo<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }

            List<DataRow> rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertTo<T>(rows);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Fanex.Age.Utility
{
    public static class ReflectionUtil
    {
        public static object GetValue(object obj, string propertyName)
        {
            var propertyInfo = GetPropertyInfo(obj, propertyName);
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(obj, null);
            }
            return null;
        }

        public static PropertyInfo GetPropertyInfo(object obj, string propertyName)
        {
            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                if (string.Compare(propertyInfo.Name, propertyName, true) == 0)
                {
                    return propertyInfo;
                }
            }
            return null;
        }

        public static bool HasPropertyInfo(object obj, string propertyName)
        {
            var propertyInfo = GetPropertyInfo(obj, propertyName);
            if (propertyInfo != null)
                return true;
            return false;
        }
    }
}

// --------------------------------------------------------------------------
// <copyright file="ServiceLayerException.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.7.5">Created</change>
// </history>

namespace Apollo.Age.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class ServiceLayerException : Exception
    {
        #region ctor
        public ServiceLayerException()
            : base()
        {
        }

        public ServiceLayerException(string message)
            : base(message)
        {
        }

        public ServiceLayerException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        public ServiceLayerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ServiceLayerException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        protected ServiceLayerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info != null)
            {
                this.SettingKey = info.GetString("SettingKey");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            if (info != null)
            {
                info.AddValue("SettingKey", this.SettingKey);
            }
        }
        #endregion

        public string SettingKey 
        { 
            get; 
            set; 
        }

        public override string Message
        {
            get
            {
                var dbSetting = Fanex.Data.Configuration.DbSettingProviderManager.Current.GetSetting(SettingKey);
                if (dbSetting != null)
                {
                    return base.Message +
                        "\nSettingKey:" + SettingKey +
                        "\nDBName:" + dbSetting.DbName + 
                        "\nConnectionString:" + dbSetting.ConnectionString;
                }

                return base.Message + "\nSettingKey:" + SettingKey;
            }
        }        
    }
}

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
    using System.Runtime.Serialization;

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
       
        #endregion      
       
        public string SettingKey { get; set; }

        public override string Message
        {
            get
            {
                var databaseSetting = Fanex.Data.Configuration.DbSettingProviderManager.Current.GetSetting(SettingKey);
                if (databaseSetting != null)
                {
                    return base.Message +
                        "\nSettingKey:" + SettingKey +
                        "\nDBName:" + databaseSetting.DbName +
                        "\nConnectionString:" + databaseSetting.ConnectionString;
                }

                return base.Message + "\nSettingKey:" + SettingKey;
            }
        }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is a null reference (Nothing in Visual Basic). </exception>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter"/>
        /// </PermissionSet>        
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            if (info != null)
            {
                info.AddValue("SettingKey", this.SettingKey);
            }
        }
    }
}

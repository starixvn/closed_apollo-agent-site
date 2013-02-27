using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects.DataClasses;

namespace Sunplus.Core.Common.IPs.DAL
{
    public partial class IPTable : EntityObject
    {
        public string Region
        {
            get {
                return ipRegion;
            }
        }

        public string City
        {
            get {
                return ipCity;
            }
        }

        public string FlagPath
        {
            get;
            set;
        }
    }
}

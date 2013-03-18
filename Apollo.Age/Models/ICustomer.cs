
namespace Apollo.Age.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICustomer : IEntity<int, string>
    {
        int Currency { get; set; }

        decimal MatchLimit { get; set; }

        decimal VirtualMultiplier { get; set; }

        bool DoubleCommLosing { get; set; }

        Agent Agent { get; set; }
    }
}

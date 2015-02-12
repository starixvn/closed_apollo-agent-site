namespace Apollo.Age.Reports.Models
{
    using System;
    using Apollo.Age.Web;

    /// <summary>
    /// Transaction Credit model class.
    /// </summary>
    [Serializable]
    public class TransactionCreditModel
    {
        private string _creatorName;

        public int TransId { get; set; }

        public DateTime TransDate { get; set; }

        public string TransType { get; set; }

        public decimal Amount { get; set; }

        public decimal BalanceAmount { get; set; }

        public string TransDesc { get; set; }

        public short CreatorLevel { get; set; }

        public string CreatorName
        {
            get
            {
                var loginLevel = SessionManager.CurrentSession.AgentLevel;

                if (CreatorLevel < loginLevel)
                {
                    return Encrypt(_creatorName);
                }
                else
                {
                    return _creatorName;
                }
            }

            set
            {
                _creatorName = value;
            }
        }

        /// <summary>
        /// Encrypts the specified name.
        /// </summary>
        /// <param name="name">The name input.</param>
        /// <returns>Name after Encrypt.</returns>
        private string Encrypt(string name)
        {
            if (name.Length <= 3)
            {
                return "***";
            }
            else
            {
                var first = name.ToCharArray()[0];
                var last = name.ToCharArray()[name.Length - 1];

                return name.Length == 4 ? first + "***" : first + "***" + last;
            }
        }
    }
}
/*
 * Created 20091229@Lee - Sub-account entity
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.Model.Entities
{
    using System;
    using System.Data;
    using Sunplus.Agent.Model.Entities.Base;

    // SP return subaccid, username, firstname, lastname, createdate, isnull(closed, 0) closed, PageAccess
    public class SubAccount : IEntity
    {        
        private int asId;
        private string asUserName;
        private string asPassword;
        private string asFirstName;
        private string asLastName;
        private DateTime asCreateDate;
        private bool asIsClosed;
        private PageAccess asPageAccess;

        public SubAccount(DataRow dataRow)
            : this(dataRow, true)
        {
        }

        public SubAccount(DataRow dataRow, bool isGetCreatedDate)
        {
            this.asId = (int)dataRow["subaccid"];
            this.asUserName = dataRow["username"] as string;
            this.asFirstName = dataRow["firstname"] as string;
            this.asLastName = dataRow["lastname"] as string;
            if (isGetCreatedDate)
            {
                this.asCreateDate = (DateTime)dataRow["createdate"];
            }

            this.asIsClosed = (bool)dataRow["closed"];
            this.asPageAccess = dataRow["PageAccess"] as string;
        }

        public SubAccount(IDataReader reader)
            : this(reader, true)
        {
        }

        public SubAccount(IDataReader reader, bool isGetCreatedDate)
        {
            this.asId = (int)reader["subaccid"];
            this.asUserName = reader["username"] as string;
            this.asFirstName = reader["firstname"] as string;
            this.asLastName = reader["lastname"] as string;
            if (isGetCreatedDate)
            {
                this.asCreateDate = (DateTime)reader["createdate"];
            }

            this.asIsClosed = (bool)reader["closed"];
            this.asPageAccess = reader["PageAccess"] as string;
        }

        public SubAccount()
        {
            this.asPageAccess = new PageAccess();
        }

        public int Id
        {
            get { return this.asId; }
            set { this.asId = value; }
        }

        public string UserName
        {
            get { return this.asUserName; }
            set { this.asUserName = value; }
        }

        public string PassWord
        {
            get { return this.asPassword; }
            set { this.asPassword = value; }
        }

        public string FirstName
        {
            get { return this.asFirstName; }
            set { this.asFirstName = value; }
        }

        public string LastName
        {
            get { return this.asLastName; }
            set { this.asLastName = value; }
        }

        public DateTime CreateDate
        {
            get { return this.asCreateDate; }
            set { this.asCreateDate = value; }
        }        

        public bool IsClosed
        {
            get { return this.asIsClosed; }
            set { this.asIsClosed = value; }
        }        

        public PageAccess PageAccess
        {
            get { return this.asPageAccess; }
            set { this.asPageAccess = value; }
        }

        public static implicit operator SubAccount(DataRow dataRow)
        {
            return new SubAccount(dataRow);
        }                
    }
}
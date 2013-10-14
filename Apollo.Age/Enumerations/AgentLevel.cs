// --------------------------------------------------------------------------
// <copyright file="AgentLevel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2013.5.17">Created</change>
// </history>

namespace Apollo.Age.Enumerations
{
    /// <summary>
    /// This class is hard code for Agent level.
    /// </summary>
    public static class AgentLevel
    {
        private static int _super = 0;
        private static int _mainshareholder = 1;
        private static int _shareholder = 2;
        private static int _mainagent = 3;

        public static int Super
        {
            get
            {
                return _super;
            }
        }

        public static int MainShareholder
        {
            get
            {
                return _mainshareholder;
            }
        }

        public static int Shareholder
        {
            get
            {
                return _shareholder;
            }
        }

        public static int MainAgent
        {
            get
            {
                return _mainagent;
            }
        }
    }
}
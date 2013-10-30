// ---------------------------------------------------------------------------------
// <copyright file="ErrorPag.aspx.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Kaden.Trinh" date="2013.05.08">Apply Error page</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.Age.UI.Components
{
    using Fanex.Age.UI.Components.ErrorPage;
    public class ErrorPage_Page
    {
        private string _error;
        ErrorPage_Template tpl = new ErrorPage_Template();

        public string Error
        {
            get { return _error; }
            set { _error = value; }
        }

        public ErrorPage_Page(string error)
        {
            _error = error;
        }

        public static implicit operator string(ErrorPage_Page value)
        {
            return value.ToString();
        }

        public override string ToString()
        {
            tpl.errorMsg = _error;
            return tpl.ToString();
        }
    }
}
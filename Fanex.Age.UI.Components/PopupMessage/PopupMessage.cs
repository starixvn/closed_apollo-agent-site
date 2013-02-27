using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;

using Fanex.Age.UI.Components.PopupMessage;

/*
 * Created 20100220@Lee - Popup message page
 * Revision:
 * 
 */


namespace Fanex.Age.UI.Components
{
    public class PopupMessage_Page
    {
        private string _content;

        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public PopupMessage_Page(string content)
        {
            _content = content;
        }

        public static implicit operator string(PopupMessage_Page value)
        {
            PopupMessage_Template tpl = new PopupMessage_Template();
            tpl.content = value._content;
            return tpl;
        }

        public string ToString()
        {
            PopupMessage_Template tpl = new PopupMessage_Template();
            tpl.content = _content;
            return tpl.ToString();
        }
    }

}
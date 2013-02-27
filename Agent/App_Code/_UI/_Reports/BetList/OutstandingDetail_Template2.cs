using System;
using System.Data;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace Fanex.Age.UI.Reports.BetList
{

    public class OutstandingDetail_Template2 : OutstandingDetail_Template
    {
        public OutstandingDetail_Template2()
        {

        }

        public void ToString(HtmlTextWriter writer)
        {
            if (!Visible) return;
            writer.Write(s0);
            writer.Write(s1);
            writer.Write(s2);
            writer.Write(s3);
            writer.Write(s4);
            writer.Write(s5);
            writer.Write(s6);
            writer.Write(s7);
            writer.Write(s8);
            writer.Write(title);
            writer.Write(s9);
            writer.Write(SearchPanel);
            writer.Write(s10);
            writer.Write(content);
            writer.Write(s11);
            writer.Write(s12);
            writer.Write(s13);
            writer.Write(s14);
            writer.Write(s15);
            writer.Write(s16);
            writer.Write(s17);
            writer.Write(s18);
            writer.Write(s19);
            writer.Write(s20);
            writer.Write(s21);
            writer.Write(Value);
        }
    }

}
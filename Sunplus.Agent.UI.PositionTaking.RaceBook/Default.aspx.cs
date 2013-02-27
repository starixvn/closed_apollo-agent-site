using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sunplus.Agent.UI.PositionTaking.RaceBook
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Super.AddNewMaster_Template tpl = new Sunplus.Agent.UI.PositionTaking.RaceBook.Super.AddNewMaster_Template();

            Response.Write(tpl);
        }
    }
}

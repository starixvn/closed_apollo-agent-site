using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Sunplus.Agent.UI.ScoreMap.UI
{
    public abstract class ScoreMapBase : ColumnScoreMap_Template, IScoreMapItem
    {
        public abstract string Render();
        public abstract void Assign(DataRow row);
        
        #region IComponent Members
        public abstract void HideLinkedResources();
        #endregion
    }
}
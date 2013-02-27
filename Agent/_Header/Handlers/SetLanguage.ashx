<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._Header.SetLanguage_Handler" %>

using System;
using System.Web;
using System.Collections;

using Fanex.Age.UI;

/*
 * Created 20100126@Lee - Set language handler
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.UI._Header
{
    public class SetLanguage_Handler : Fanex.Age.UI.TplHandler
    {
        const string LANG_PARAM = "lang";
        private string culture = "en-US";
        
        public override void ProcessRequest(ref Hashtable p)
        {
            p.AddHttpParameters(LANG_PARAM, Request);
        }

        public override object ValidateRequest(Hashtable p)
        {
            string language = p[LANG_PARAM] as string;
            
            if (!Validation.CheckValidLanguage(language))
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.strNotAccessPage);
            }

            culture = language;
            
            return null;
        }

        public override object InvokeData(Hashtable p)
        {
            return null;
        }
        
        public override void RenderResult(ref JSONData data)
        {
            CurrentUserSession.Language = culture;
            CurrentUserSession.LangKey = Sunplus.Agent.Model.Common.Language.LangKeyFromCulture(culture);
            
            data.AsSuccessResult();
        }
    }

}
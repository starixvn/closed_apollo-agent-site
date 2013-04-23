using System;
using System.Text;
using DAL;
using DNAide.Core;
using DNAide.Web;

/// <summary>
/// Summary description for Helper
/// </summary>
public static class Helper
{
    //Reset AGE setting
    public static void ResetCasinoSetting(bool useWebConfigProvider, string newProvider)
    {
        Globals.IBCSetting.Casino.UseWebConfigProviderType = useWebConfigProvider;
        CasinoProvider.Instance = null;
        if (!useWebConfigProvider)
        {
            Globals.IBCSetting.Casino.ProviderType = newProvider;
        }
    }

    public static void ResetCasinoSetting(bool useWebConfigProvider)
    {
        Globals.IBCSetting.Casino.UseWebConfigProviderType = useWebConfigProvider;
        CasinoProvider.Instance = null;
    }

    public static string GenSelectOption(double maxValue, double currentVal, int interval, bool disabled, string id, string onChangeFunc)
    {
        return GenSelectOption(maxValue, 0, currentVal, interval, disabled, id, onChangeFunc);
    }

    public static string GenSelectOption(double maxValue, double minValue, double currentVal, int interval, bool disabled, string id, string onChangeFunc)
    {
        maxValue = Math.Max(maxValue, 0); maxValue = Math.Min(maxValue, 0.9);
        minValue = Math.Max(minValue, 0); minValue = Math.Min(minValue, 0.9);

        string tplSelect = TemplateHandler.LoadTemplate(Globals.TPL_CTR_SELECTOPTION);
        StringBuilder options = new StringBuilder();
        minValue = Math.Min(maxValue, minValue);
        //int groupValue = Convert.ToInt32(maxValue * 100000);
        int groupValue = Convert.ToInt32(maxValue * 10000);
        int min = CastAide.AsInt32(minValue * 10000);

        currentVal = Math.Round(currentVal * 10000) / 10000;
        maxValue = Math.Round(maxValue * 10000) / 10000;
        minValue = Math.Round(minValue * 10000) / 10000;
        if (currentVal > maxValue || currentVal < 0)
        {
            //currentVal = maxValue;
            options.Append("<option value='" + currentVal + "' id='" + id + Globals.SEP_GAMEKIND_GAMETYPE + Globals.CTR_OPTIONS_PREFIX + Globals.SEP_GAMEKIND_GAMETYPE + currentVal + "' selected='true'>" + currentVal + "</option>");
        }
        while (groupValue >= min)
        {
            //double selValue = Convert.ToDouble(groupValue) / 100000;
            double selValue = Convert.ToDouble(groupValue) / 10000;
            if (currentVal == selValue)
                options.Append("<option value='" + selValue + "' id='" + id + Globals.SEP_GAMEKIND_GAMETYPE + Globals.CTR_OPTIONS_PREFIX + Globals.SEP_GAMEKIND_GAMETYPE + selValue + "' selected='true'>" + selValue + "</option>");
            else
                options.Append("<option value='" + selValue + "' id='" + id + Globals.SEP_GAMEKIND_GAMETYPE + Globals.CTR_OPTIONS_PREFIX + Globals.SEP_GAMEKIND_GAMETYPE + selValue + "'>" + selValue + "</option>");
            //groupValue = groupValue - 25;
            groupValue = groupValue - interval;
        }

        TemplateHandler.Assign(ref tplSelect, "ddl-Id", id);
        TemplateHandler.Assign(ref tplSelect, "onchange", onChangeFunc);
        TemplateHandler.Assign(ref tplSelect, "class", "");
        TemplateHandler.Assign(ref tplSelect, "options-val", options.ToString());
        TemplateHandler.Assign(ref tplSelect, "disabled", (disabled) ? "disabled='disabled'" : "");
        return tplSelect;
    }

    public static string GenSelectOption(double maxValue, double currentVal, int interval, bool disabled, string id, string onChangeFunc, string classStyle)
    {
        maxValue = Math.Max(maxValue, 0); maxValue = Math.Min(maxValue, 0.9);

        string tplSelect = TemplateHandler.LoadTemplate(Globals.TPL_CTR_SELECTOPTION);
        StringBuilder options = new StringBuilder();
        //int groupValue = Convert.ToInt32(maxValue * 100000);
        int groupValue = Convert.ToInt32(maxValue * 10000);

        currentVal = Math.Round(currentVal * 10000) / 10000;
        maxValue = Math.Round(maxValue * 10000) / 10000;
        if (currentVal > maxValue || currentVal < 0)
        {
            //currentVal = maxValue;
            options.Append("<option value='" + currentVal + "' id='" + id + Globals.SEP_GAMEKIND_GAMETYPE + Globals.CTR_OPTIONS_PREFIX + Globals.SEP_GAMEKIND_GAMETYPE + currentVal + "' selected='true'>" + currentVal + "</option>");
        }
        while (groupValue >= 0)
        {
            //double selValue = Convert.ToDouble(groupValue) / 100000;
            double selValue = Convert.ToDouble(groupValue) / 10000;
            if (currentVal == selValue)
                options.Append("<option value='" + selValue + "' id='" + id + Globals.SEP_GAMEKIND_GAMETYPE + Globals.CTR_OPTIONS_PREFIX + Globals.SEP_GAMEKIND_GAMETYPE + selValue + "' selected='true'>" + selValue + "</option>");
            else
                options.Append("<option value='" + selValue + "' id='" + id + Globals.SEP_GAMEKIND_GAMETYPE + Globals.CTR_OPTIONS_PREFIX + Globals.SEP_GAMEKIND_GAMETYPE + selValue + "'>" + selValue + "</option>");
            //groupValue = groupValue - 25;
            groupValue = groupValue - interval;
        }

        TemplateHandler.Assign(ref tplSelect, "ddl-Id", id);
        TemplateHandler.Assign(ref tplSelect, "class", classStyle);
        TemplateHandler.Assign(ref tplSelect, "onchange", onChangeFunc);
        TemplateHandler.Assign(ref tplSelect, "options-val", options.ToString());
        TemplateHandler.Assign(ref tplSelect, "disabled", (disabled) ? "disabled='disabled'" : "");
        return tplSelect;
    }

    public static string GenCheckBox(string id, bool disabled, bool check, string funcClick)
    {

        /*string tplCheckBox = TemplateHandler.LoadTemplate(Globals.TPL_CTR_CHECKBOX);
        TemplateHandler.Assign(ref tplCheckBox, "chkId", id);
        TemplateHandler.Assign(ref tplCheckBox, "onclick", funcClick);
        TemplateHandler.Assign(ref tplCheckBox, "disabled", (disabled) ? "disabled='disabled'" : "");
        TemplateHandler.Assign(ref tplCheckBox, "checked", (check) ? "checked='checked'" : "");
        */

        return GenInput(Globals.InputType.CheckBox, id, "", "", disabled, check, funcClick);
    }

    public static string GenCheckBox(string id, bool disabled, bool check, string classStyle, string funcClick)
    {

        /*string tplCheckBox = TemplateHandler.LoadTemplate(Globals.TPL_CTR_CHECKBOX);
        TemplateHandler.Assign(ref tplCheckBox, "chkId", id);
        TemplateHandler.Assign(ref tplCheckBox, "onclick", funcClick);
        TemplateHandler.Assign(ref tplCheckBox, "disabled", (disabled) ? "disabled='disabled'" : "");
        TemplateHandler.Assign(ref tplCheckBox, "checked", (check) ? "checked='checked'" : "");
        */

        return GenInput(Globals.InputType.CheckBox, id, "", classStyle, disabled, check, funcClick);

    }

    public static string GenHidden(string id, string value)
    {

        /*string tplHidden = TemplateHandler.LoadTemplate(Globals.TPL_CTR_HIDDEN);
        TemplateHandler.Assign(ref tplHidden, "name", id);
        TemplateHandler.Assign(ref tplHidden, "strValue", value);
        */

        return GenInput(Globals.InputType.Hidden, id, value, "", false, false, "");

    }

    public static string GenButton(string id, string value, string classStyle, bool disabled, string funcClick)
    {

        /*string tplButton = TemplateHandler.LoadTemplate(Globals.TPL_CTR_BUTTON);
        TemplateHandler.Assign(ref tplButton, "btnId", id);
        TemplateHandler.Assign(ref tplButton, "onclick", funcClick);
        TemplateHandler.Assign(ref tplButton, "class", classStyle);
        TemplateHandler.Assign(ref tplButton, "value", value);
        TemplateHandler.Assign(ref tplButton, "disabled", (disabled) ? "disabled='disabled'" : "");
        */
        return GenInput(Globals.InputType.Button, id, value, classStyle, disabled, false, funcClick);

    }

    public static string GenTextBox(string id, string value, string classStyle, bool disabled, string funcClick)
    {

        return GenInput(Globals.InputType.Text, id, value, classStyle, disabled, false, funcClick);

    }

    public static string GenTextBox(string id, string name, string value, string classStyle, bool disabled, string funcClick, string funcKeydown)
    {

        return GenInput(Globals.InputType.Text, id, name, value, classStyle, disabled, false, funcClick, funcKeydown);

    }

    public static string GenTextBox(string id, string name, string value, string classStyle, bool disabled, string funcClick)
    {

        return GenInput(Globals.InputType.Text, id, name, value, classStyle, disabled, false, funcClick);

    }

    public static string GenChangePTButtons(string id, bool disabled)
    {

        string btn = "";
        btn += Helper.GenButton(id + Globals.SEP_GAMEKIND_GAMETYPE + Globals.BTN_DEC1_SUFFIX, "",
            (disabled) ? Globals.BTN_DEC1_DISABLE_CLASS : Globals.BTN_DEC1_CLASS, disabled, string.Format(Globals.BTN_CHANGEPT_CLICK, "this", -Globals.CONST_CHANGEPT_1));
        btn += Helper.GenButton(id + Globals.SEP_GAMEKIND_GAMETYPE + Globals.BTN_INC1_SUFFIX, "",
            (disabled) ? Globals.BTN_INC1_DISABLE_CLASS : Globals.BTN_INC1_CLASS, disabled, string.Format(Globals.BTN_CHANGEPT_CLICK, "this", Globals.CONST_CHANGEPT_1));
        btn += Helper.GenButton(id + Globals.SEP_GAMEKIND_GAMETYPE + Globals.BTN_DEC10_SUFFIX, "",
            (disabled) ? Globals.BTN_DEC10_DISABLE_CLASS : Globals.BTN_DEC10_CLASS, disabled, string.Format(Globals.BTN_CHANGEPT_CLICK, "this", -Globals.CONST_CHANGEPT_10));
        btn += Helper.GenButton(id + Globals.SEP_GAMEKIND_GAMETYPE + Globals.BTN_INC10_SUFFIX, "",
            (disabled) ? Globals.BTN_INC10_DISABLE_CLASS : Globals.BTN_INC10_CLASS, disabled, string.Format(Globals.BTN_CHANGEPT_CLICK, "this", Globals.CONST_CHANGEPT_10));
        return btn;

    }

    public static string GenInput(Globals.InputType input, string id, string name, string value, string classStyle, bool disabled, bool check, string funcClick, string onkeydown)
    {

        string tplInput = TemplateHandler.LoadTemplate("Controls/Input1.htm");

        switch (input)
        {
            case Globals.InputType.Button:
                TemplateHandler.Assign(ref tplInput, "type", Globals.GetInputType(Globals.InputType.Button));
                TemplateHandler.Assign(ref tplInput, "inputId", id);
                TemplateHandler.Assign(ref tplInput, "inputName", id);
                TemplateHandler.Assign(ref tplInput, "value", value);
                TemplateHandler.Assign(ref tplInput, "disabled", (disabled) ? "disabled='disabled'" : "");
                TemplateHandler.Assign(ref tplInput, "checked", "");

                TemplateHandler.Assign(ref tplInput, "onclick", funcClick);
                TemplateHandler.Assign(ref tplInput, "class", classStyle);
                break;
            case Globals.InputType.Hidden:
                TemplateHandler.Assign(ref tplInput, "type", Globals.GetInputType(Globals.InputType.Hidden));
                TemplateHandler.Assign(ref tplInput, "inputId", id);
                TemplateHandler.Assign(ref tplInput, "inputName", id);
                TemplateHandler.Assign(ref tplInput, "value", value);
                TemplateHandler.Assign(ref tplInput, "disabled", "");

                TemplateHandler.Assign(ref tplInput, "onclick", funcClick);
                TemplateHandler.Assign(ref tplInput, "checked", "");
                TemplateHandler.Assign(ref tplInput, "class", "");
                break;
            case Globals.InputType.Text:
                TemplateHandler.Assign(ref tplInput, "type", Globals.GetInputType(Globals.InputType.Text));
                TemplateHandler.Assign(ref tplInput, "inputId", id);
                TemplateHandler.Assign(ref tplInput, "inputName", id);
                TemplateHandler.Assign(ref tplInput, "value", value);
                TemplateHandler.Assign(ref tplInput, "disabled", (disabled) ? "disabled='disabled'" : "");

                TemplateHandler.Assign(ref tplInput, "onclick", funcClick);

                TemplateHandler.Assign(ref tplInput, "onkeydown", onkeydown);
                TemplateHandler.Assign(ref tplInput, "checked", "");
                TemplateHandler.Assign(ref tplInput, "class", classStyle);
                break;
            case Globals.InputType.CheckBox:
                TemplateHandler.Assign(ref tplInput, "type", Globals.GetInputType(Globals.InputType.CheckBox));
                TemplateHandler.Assign(ref tplInput, "inputId", id);
                TemplateHandler.Assign(ref tplInput, "inputName", id);
                TemplateHandler.Assign(ref tplInput, "value", "");
                TemplateHandler.Assign(ref tplInput, "disabled", (disabled) ? "disabled='disabled'" : "");

                TemplateHandler.Assign(ref tplInput, "onclick", funcClick);
                TemplateHandler.Assign(ref tplInput, "class", classStyle);
                TemplateHandler.Assign(ref tplInput, "checked", (check) ? "checked='checked'" : "");
                break;
        }

        return tplInput;

    }

    public static string GenInput(Globals.InputType input, string id, string value, string classStyle, bool disabled, bool check, string funcClick)
    {

        string tplInput = TemplateHandler.LoadTemplate(Globals.TPL_CTR_INPUT);

        switch (input)
        {
            case Globals.InputType.Button:
                TemplateHandler.Assign(ref tplInput, "type", Globals.GetInputType(Globals.InputType.Button));
                TemplateHandler.Assign(ref tplInput, "inputId", id);
                TemplateHandler.Assign(ref tplInput, "inputName", id);
                TemplateHandler.Assign(ref tplInput, "value", value);
                TemplateHandler.Assign(ref tplInput, "disabled", (disabled) ? "disabled='disabled'" : "");
                TemplateHandler.Assign(ref tplInput, "checked", "");

                TemplateHandler.Assign(ref tplInput, "onclick", funcClick);

                TemplateHandler.Assign(ref tplInput, "class", classStyle);
                break;
            case Globals.InputType.Hidden:
                TemplateHandler.Assign(ref tplInput, "type", Globals.GetInputType(Globals.InputType.Hidden));
                TemplateHandler.Assign(ref tplInput, "inputId", id);
                TemplateHandler.Assign(ref tplInput, "inputName", id);
                TemplateHandler.Assign(ref tplInput, "value", value);
                TemplateHandler.Assign(ref tplInput, "disabled", "");

                TemplateHandler.Assign(ref tplInput, "onclick", funcClick);

                TemplateHandler.Assign(ref tplInput, "checked", "");
                TemplateHandler.Assign(ref tplInput, "class", "");
                break;
            case Globals.InputType.Text:
                TemplateHandler.Assign(ref tplInput, "type", Globals.GetInputType(Globals.InputType.Text));
                TemplateHandler.Assign(ref tplInput, "inputId", id);
                TemplateHandler.Assign(ref tplInput, "inputName", id);
                TemplateHandler.Assign(ref tplInput, "value", value);
                TemplateHandler.Assign(ref tplInput, "disabled", (disabled) ? "disabled='disabled'" : "");

                TemplateHandler.Assign(ref tplInput, "onclick", funcClick);

                TemplateHandler.Assign(ref tplInput, "checked", "");
                TemplateHandler.Assign(ref tplInput, "class", classStyle);
                break;
            case Globals.InputType.CheckBox:
                TemplateHandler.Assign(ref tplInput, "type", Globals.GetInputType(Globals.InputType.CheckBox));
                TemplateHandler.Assign(ref tplInput, "inputId", id);
                TemplateHandler.Assign(ref tplInput, "inputName", id);
                TemplateHandler.Assign(ref tplInput, "value", "");
                TemplateHandler.Assign(ref tplInput, "disabled", (disabled) ? "disabled='disabled'" : "");

                TemplateHandler.Assign(ref tplInput, "onclick", funcClick);

                TemplateHandler.Assign(ref tplInput, "class", classStyle);
                TemplateHandler.Assign(ref tplInput, "checked", (check) ? "checked='checked'" : "");
                break;
        }

        return tplInput;

    }

    public static string GenInput(Globals.InputType input, string id, string name, string value, string classStyle, bool disabled, bool check, string funcClick)
    {

        string tplInput = TemplateHandler.LoadTemplate(Globals.TPL_CTR_INPUT);

        switch (input)
        {
            case Globals.InputType.Button:
                TemplateHandler.Assign(ref tplInput, "type", Globals.GetInputType(Globals.InputType.Button));
                TemplateHandler.Assign(ref tplInput, "inputId", id);
                TemplateHandler.Assign(ref tplInput, "inputName", name);
                TemplateHandler.Assign(ref tplInput, "value", value);
                TemplateHandler.Assign(ref tplInput, "disabled", (disabled) ? "disabled='disabled'" : "");
                TemplateHandler.Assign(ref tplInput, "checked", "");

                TemplateHandler.Assign(ref tplInput, "onclick", funcClick);

                TemplateHandler.Assign(ref tplInput, "class", classStyle);
                break;
            case Globals.InputType.Hidden:
                TemplateHandler.Assign(ref tplInput, "type", Globals.GetInputType(Globals.InputType.Hidden));
                TemplateHandler.Assign(ref tplInput, "inputId", id);
                TemplateHandler.Assign(ref tplInput, "inputName", name);
                TemplateHandler.Assign(ref tplInput, "value", value);
                TemplateHandler.Assign(ref tplInput, "disabled", "");

                TemplateHandler.Assign(ref tplInput, "onclick", funcClick);

                TemplateHandler.Assign(ref tplInput, "checked", "");
                TemplateHandler.Assign(ref tplInput, "class", "");
                break;
            case Globals.InputType.Text:
                TemplateHandler.Assign(ref tplInput, "type", Globals.GetInputType(Globals.InputType.Text));
                TemplateHandler.Assign(ref tplInput, "inputId", id);
                TemplateHandler.Assign(ref tplInput, "inputName", name);
                TemplateHandler.Assign(ref tplInput, "value", value);
                TemplateHandler.Assign(ref tplInput, "disabled", (disabled) ? "disabled='disabled'" : "");

                TemplateHandler.Assign(ref tplInput, "onclick", funcClick);

                TemplateHandler.Assign(ref tplInput, "checked", "");
                TemplateHandler.Assign(ref tplInput, "class", classStyle);
                break;
            case Globals.InputType.CheckBox:
                TemplateHandler.Assign(ref tplInput, "type", Globals.GetInputType(Globals.InputType.CheckBox));
                TemplateHandler.Assign(ref tplInput, "inputId", id);
                TemplateHandler.Assign(ref tplInput, "inputName", name);
                TemplateHandler.Assign(ref tplInput, "value", "");
                TemplateHandler.Assign(ref tplInput, "disabled", (disabled) ? "disabled='disabled'" : "");

                TemplateHandler.Assign(ref tplInput, "onclick", funcClick);

                TemplateHandler.Assign(ref tplInput, "class", classStyle);
                TemplateHandler.Assign(ref tplInput, "checked", (check) ? "checked='checked'" : "");
                break;
        }

        return tplInput;

    }

    public static string GenLabel(string id, string value, string classStyle)
    {

        string tplLabel = TemplateHandler.LoadTemplate(Globals.TPL_CTR_LABEL);
        TemplateHandler.Assign(ref tplLabel, "lblid", id);
        TemplateHandler.Assign(ref tplLabel, "class", classStyle);
        TemplateHandler.Assign(ref tplLabel, "value", value);

        return tplLabel;

    }

    public static string GenLink(string label, string href, string classStyle, string funcOnclick)
    {

        string tplLink = TemplateHandler.LoadTemplate(Globals.TPL_CTR_LINK);
        TemplateHandler.Assign(ref tplLink, "label", label);
        TemplateHandler.Assign(ref tplLink, "href", href);
        TemplateHandler.Assign(ref tplLink, "class", classStyle);
        if (!string.IsNullOrEmpty(funcOnclick))
            TemplateHandler.Assign(ref tplLink, "onclick", funcOnclick);

        return tplLink;
    }
}
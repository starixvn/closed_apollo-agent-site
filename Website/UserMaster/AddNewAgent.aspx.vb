
'Imports System.Resources
Imports DNAide.Web
Imports Fanex.Data.DNAide
Imports DNAide.Utils
Imports DNAide.Core
Imports System.Text
Imports System.Data
Imports Sunplus.Agent.Model.Entities.Permission

Public Class AddNewAgent
    Inherits IbcPage2
    Public style As String

    Dim bodyTpl, tennisTpl, basketballTpl, footballTpl, otherSportsTpl, golfTpl As String
    Dim strHidSoccer, strHidBasket, strHidFootball, strHidTennis, strHidNumberGame, strHidMixSports As String
    Dim strHidBaseball, strHidGolf, strHidMoto, strHidOther, strTranferTime As String
    Dim groupBtn As String
    Dim selectTpl As String
    Dim selDisabledTpl As String
    Dim strContent As String
    Protected WithEvents isDirect As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected strDirect As String
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Public script As String
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        bodyTpl = TemplateHandler.LoadTemplate("MemberInfo/MenuPT.html")

        If (Sunplus.Agent.Model.Entities.UserSession.Current.IsInternal) Then
            TemplateHandler.Assign(bodyTpl, "clickable", "return false")
        Else
            TemplateHandler.Assign(bodyTpl, "clickable", String.Empty)
        End If

        If (Not UserPermission.IsShowNumberGame) Then
            TemplateHandler.Assign(bodyTpl, "hideClss", "hidden")
        Else
            TemplateHandler.Assign(bodyTpl, "hideClss", String.Empty)
        End If

        If (Not UserPermission.IsShowMixedSport) Then
            TemplateHandler.Assign(bodyTpl, "hideClssMP", "hidden")
        Else
            TemplateHandler.Assign(bodyTpl, "hideClssMP", String.Empty)
        End If

        groupBtn = TemplateHandler.LoadTemplate("MemberInfo/BtnGroup.html")
        selectTpl = TemplateHandler.LoadTemplate("MemberInfo/DdlPT.html")
        tennisTpl = TemplateHandler.LoadTemplate("MemberInfo/TennisPT.html")
        basketballTpl = TemplateHandler.LoadTemplate("MemberInfo/BasketballPT.html")
        footballTpl = TemplateHandler.LoadTemplate("MemberInfo/FootballPT.html")
        otherSportsTpl = TemplateHandler.LoadTemplate("MemberInfo/OtherSportsPT.html")
        golfTpl = TemplateHandler.LoadTemplate("MemberInfo/GolfPT.html")
        strTranferTime = TemplateHandler.LoadTemplate("MemberInfo/EditTranferTime.html")
        selDisabledTpl = TemplateHandler.LoadTemplate("MemberInfo/DdlDisablePT.html")

        strContent = LoadData()
        'TemplateHandler.RemoveComment(strContent)
        htmlAddAgentContent.Text = strContent


    End Sub
#Region "Declare parameter for Script"
    Sub VariableOfScript()
        Dim script As String = ""
        script += "<script language='javascript'>" & vbNewLine
        script += " var alertcredit='" & Resources.Lang.alertIncorrectAmaxcredit & "';" & vbNewLine
        script += " var alertMinbet='" & Resources.Lang.alertIncorrectMinBet & "'; " & vbNewLine
        script += " var alertMaxbet='" & Resources.Lang.alertIncorrectMaxBet & "';" & vbNewLine
        script += " var alertMaxpermatch='" & Resources.Lang.alertIncorrectMaxPerMatch & "';" & vbNewLine
        script += " var alertCompare3='" & Resources.Lang.alertMaxLessMin & "'; " & vbNewLine
        script += " var alertCompare='" & Resources.Lang.alertMaxLessMaxPer & "'; " & vbNewLine
        script += " var alertminbetC='" & Resources.Lang.lblalertMinbetCompare & "';" & vbNewLine
        script += " var alertMaxbetC='" & Resources.Lang.lblalerMaxbetCompare & "';" & vbNewLine
        script += " var alertMaxpermatchC='" & Resources.Lang.lblalertMaxPerMatchCompare & "';" & vbNewLine
        script += " var alertMaxpermatchCMin='" & Resources.Lang.lblalertMaxPerMatchCompareMin & "';" & vbNewLine
        script += " var alertMaxpermatchCMax='" & Resources.Lang.lblalertMaxPerMatchCompareMax & "';" & vbNewLine
        script += " var alertUserName='" & Resources.Lang.alertUserName & "';" & vbNewLine
        script += " var lblPasswordRequire='" & Resources.Lang.lblPasswordRequire & "';" & vbNewLine
        script += " var alertPasswordEmpty='" & Resources.Lang.alertPasswordEmpty & "';" & vbNewLine
        script += " var alertIncorrectCredit='" & Resources.Lang.alertAgentCredit & "';" & vbNewLine
        script += " var alertMaxperMatchNegative ='" & Resources.Lang.lblmaxperballgreaterhanzero & "';" & vbNewLine
        script += " var alertMaxperball='" & Resources.Lang.alertIncorrectMaxPerBall & "';" & vbNewLine
        script += " var alertMaxperballCMax='" & Resources.Lang.lblalertMaxPerBallCompareMax & "';" & vbNewLine
        script += " var alertMaxperballC='" & Resources.Lang.lblalertMaxPerBallCompare & "';" & vbNewLine
        script += " var alertMaxPerMatchLessMaxPerBall='" & Resources.Lang.alertMaxPerMatchLessMaxPerBall & "';" & vbNewLine
        script += " var alertMaxPerBallLessMaxBet='" & Resources.Lang.alertMaxPerBallLessMaxBet & "';" & vbNewLine

        script += " var lblChooseDaily='" & Resources.Lang.choonsedaily & "';" & vbNewLine
        script += " var lblChooseOneDay='" & Resources.Lang.chooseoneday & "';" & vbNewLine
        Dim lblCreditInvalid As String
        lblCreditInvalid = Resources.Lang.lblCreditInvalid.Replace("{0}", Resources.Lang.lblAgentMaxCredit)

        Dim lblMemberCreditInvalid As String
        lblMemberCreditInvalid = Resources.Lang.lblCreditInvalid.Replace("{0}", Resources.Lang.lblMemberMaxCredit)


        script += " var alertMemberCreditInvalid='" & lblMemberCreditInvalid & "';" & vbNewLine
        script += " var alertCreditInvalid='" & lblCreditInvalid & "';" & vbNewLine

        Dim strAlertMinPT As String = Resources.Lang.alertLessthanMinPT
        strAlertMinPT = strAlertMinPT.Replace("{1}", Resources.Lang.lblMasterPresetPosition)
        strAlertMinPT = strAlertMinPT.Replace("{2}", Resources.Lang.lblSuperPresetPosition)
        script += " var alertLessthanMinPT='" & strAlertMinPT & "';" & vbNewLine
        script += "</script>"
        Response.Write(script)

    End Sub
#End Region

#Region "Load Data"
    Function LoadData() As String
        Dim html As New StringBuilder

        Dim rs As DbResult
        Dim hash As New Hashtable
        hash.Add("custid", Sunplus.Agent.Model.Entities.UserSession.Current.CustId)
        rs = AccountInfo.Acc_CustomerSelMAgentInfo(hash)
        If Not rs Is Nothing Then
            If rs.ResultSet.Tables(0).Rows.Count > 0 Then
                Dim row As DataRow = rs.ResultSet.Tables(0).Rows(0)
                html.Append(GetDataInfo(row))
                Common.GetSportTitle(bodyTpl, False)
                TemplateHandler.Assign(bodyTpl, "add-text", Resources.Lang.btnAdd)
                TemplateHandler.Assign(bodyTpl, "add-id", "btnAdd")
                TemplateHandler.Assign(bodyTpl, "soccerPT", GetDataSoccer(row))
                TemplateHandler.Assign(bodyTpl, "basketPT", GetDataBasketball(row, 2, basketballTpl))
                TemplateHandler.Assign(bodyTpl, "footballPT", GetData4DDl(row, 3, footballTpl))
                TemplateHandler.Assign(bodyTpl, "tennisPT", GetData4DDl(row, 5, tennisTpl))
                TemplateHandler.Assign(bodyTpl, "baseballPT", GetDataBaseball(row, 8))
                TemplateHandler.Assign(bodyTpl, "golfPT", GetData4DDl(row, 10, golfTpl))
                TemplateHandler.Assign(bodyTpl, "motoPT", GetDataMotoSports(row, 11))
                TemplateHandler.Assign(bodyTpl, "otherPT", GetData4DDl(row, 99, otherSportsTpl))

                If UserPermission.IsShowNumberGame Then
                    TemplateHandler.Assign(bodyTpl, "numbergamePT", GetDataNumberGame(row))
                Else
                    TemplateHandler.Assign(bodyTpl, "numbergamePT", "")
                End If

                If UserPermission.IsShowMixedSport Then
                    TemplateHandler.Assign(bodyTpl, "mixSportPT", GetDataMixSports(row))
                End If

                If False Or CastAide.AsBoolean(row("disableautompt"), False) Then
                    TemplateHandler.Assign(bodyTpl, "hdauto", "hidden")
                Else
                    TemplateHandler.Assign(bodyTpl, "hdauto", String.Empty)
                End If

                bodyTpl &= strHidSoccer
                bodyTpl &= strHidBasket
                bodyTpl &= strHidFootball
                bodyTpl &= strHidTennis
                bodyTpl &= strHidBaseball
                bodyTpl &= strHidGolf
                bodyTpl &= strHidMoto
                bodyTpl &= strHidOther
                bodyTpl &= strHidNumberGame
                bodyTpl &= strHidMixSports

                html.Append(bodyTpl)
            End If
        End If

        LoadData = html.ToString()
    End Function
#End Region

#Region "Assign info about UNam, Max Credit, Commission ..."
    Function GetDataInfo(ByVal dr As DataRow) As String
        Dim strAccInfo As String = String.Empty

        strAccInfo = TemplateHandler.LoadTemplate("MemberInfo/AccInfo.html")

        If (Sunplus.Agent.Model.Entities.UserSession.Current.IsInternal) Then
            TemplateHandler.Assign(strAccInfo, "clickable", "return false")
        Else
            TemplateHandler.Assign(strAccInfo, "clickable", String.Empty)
        End If

        TemplateHandler.RemoveBlock(strAccInfo, "none-group")
        TemplateHandler.RemoveBlock(strAccInfo, "none-group")
        TemplateHandler.RemoveBlock(strAccInfo, "none-single")
        TemplateHandler.RemoveBlock(strAccInfo, "none-single")
        TemplateHandler.RemoveBlock(strAccInfo, "none-single")
        TemplateHandler.RemoveBlock(strAccInfo, "none-edit-member")
        TemplateHandler.RemoveBlock(strAccInfo, "none-member")
        TemplateHandler.RemoveBlock(strAccInfo, "none-super-master")
        TemplateHandler.RemoveBlock(strAccInfo, "none-member")
        TemplateHandler.RemoveBlock(strAccInfo, "none-add")

        TemplateHandler.Assign(strAccInfo, "add-text", Resources.Lang.btnAdd)
        TemplateHandler.Assign(strAccInfo, "add-id", "btnAdd")

        TemplateHandler.RemoveBlock(strAccInfo, "closed")
        TemplateHandler.Assign(strAccInfo, "status-text", "")

        TemplateHandler.Assign(strAccInfo, "IsSuspended", CastAide.AsInt32(dr("suspended"), 0))

        TemplateHandler.Assign(strAccInfo, "cols-untext", "4")
        TemplateHandler.Assign(strAccInfo, "title", Resources.Lang.lblAddNewAgent)
        TemplateHandler.Assign(strAccInfo, "max-credit-text", Resources.Lang.lblAgentMaxCredit)

        If (AppSettings.IsMasterSite) Then

            TemplateHandler.Assign(strAccInfo, "mMinCredit", "hidden")
            TemplateHandler.Assign(strAccInfo, "mMaxCredit", "minmaxbet")
            TemplateHandler.Assign(strAccInfo, "member-max-credit", Resources.Lang.lblMemberMaxCredit)
            TemplateHandler.Assign(strAccInfo, "MemberMaxCredit-val", "0")
            TemplateHandler.Assign(strAccInfo, "member-credit-max", "0")

            'Terry Nguyen modify: Disable Casino and Suspend
            Dim htbDis As Hashtable = New Hashtable()
            htbDis.Add("Suspend", True)
            Dim mytemplate As String = TemplateHandler.LoadTemplate("Memberinfo/DisableStatus.html")
            Dim mycolspan As String = "<td colspan='2'><td>"
            TemplateHandler.RemoveBlock(mytemplate, "Captchar")
            TemplateHandler.ReplaceBlock(strAccInfo, "Status-Disable", StatusSetting.StatusDisable(mycolspan, htbDis, mytemplate))

            TemplateHandler.Assign(strAccInfo, "suspend-text", Resources.Lang.suspended)

            If CastAide.AsBoolean(dr("suspended"), False) Then
                TemplateHandler.Assign(strAccInfo, "suspend-check", "checked='checked' disabled='disabled'")
            Else
                TemplateHandler.Assign(strAccInfo, "suspend-check", "")
            End If
            TemplateHandler.Assign(strAccInfo, "IsSuspended", CastAide.AsInt32(dr("suspended"), 0))

            TemplateHandler.RemoveBlock(strAccInfo, "downline")

            ''--              
            TemplateHandler.Assign(strAccInfo, "hdAllowedCurr-value", CastAide.AsInt16(dr("AllowedCurr")))
            '-------

        ElseIf (AppSettings.IsSuperSite) Then
            TemplateHandler.RemoveBlock(strAccInfo, "for-agent-site")
            TemplateHandler.RemoveBlock(strAccInfo, "for-master-site")
        Else
            TemplateHandler.RemoveBlock(strAccInfo, "for-super-site")
            TemplateHandler.RemoveBlock(strAccInfo, "for-master-site")
        End If



        TemplateHandler.Assign(strAccInfo, "uname-parent", Sunplus.Agent.Model.Entities.UserSession.Current.UserName)
        Dim selectNoTpl As String = TemplateHandler.LoadTemplate("MemberInfo/DdlMemPT.html")
        Dim ddlNo As String = selectNoTpl
        Dim number() As String = GetListNumber(CastAide.AsString(dr("NewUserName")))
        Dim no1 As String = CStr(dr("NewUserName")).Substring(0, 1)
        Dim no2 As String = CStr(dr("NewUserName")).Substring(1, 1)

        TemplateHandler.Assign(ddlNo, "ddl-Id", "number1")
        TemplateHandler.Assign(ddlNo, "ddl-val", number(0))
        TemplateHandler.Assign(strAccInfo, "number1-val", ddlNo)
        ddlNo = selectNoTpl
        TemplateHandler.Assign(ddlNo, "ddl-Id", "number2")
        TemplateHandler.Assign(ddlNo, "ddl-val", number(1))
        TemplateHandler.Assign(strAccInfo, "number2-val", ddlNo)

        TemplateHandler.Assign(strAccInfo, "fname-val", "")
        TemplateHandler.Assign(strAccInfo, "lname-val", "")
        TemplateHandler.Assign(strAccInfo, "phone-val", "")
        TemplateHandler.Assign(strAccInfo, "mobile-val", "")
        GetCredit(dr, strAccInfo)
        TemplateHandler.Assign(strAccInfo, "check-val", "")
        TemplateHandler.Assign(strAccInfo, "fax-val", "")

        TemplateHandler.Assign(strAccInfo, "groupa-val", GetListGroup(CastAide.AsDouble(dr("groupa")), CastAide.AsDouble(dr("groupa"))))
        TemplateHandler.Assign(strAccInfo, "groupb-val", GetListGroup(CastAide.AsDouble(dr("groupb")), CastAide.AsDouble(dr("groupb"))))
        TemplateHandler.Assign(strAccInfo, "groupc-val", GetListGroup(CastAide.AsDouble(dr("groupc")), CastAide.AsDouble(dr("groupc"))))
        TemplateHandler.Assign(strAccInfo, "groupd-val", GetListGroup(CastAide.AsDouble(dr("groupd")), CastAide.AsDouble(dr("groupd"))))
        TemplateHandler.Assign(strAccInfo, "discount1x2-val", GetListGroup(CastAide.AsDouble(dr("discount1x2")), CastAide.AsDouble(dr("discount1x2"))))
        TemplateHandler.Assign(strAccInfo, "discountcs-val", GetListOtherComm(CastAide.AsDouble(dr("discountcs")), CastAide.AsDouble(dr("discountcs"))))
        If UserPermission.IsShowNumberGame Then
            TemplateHandler.Assign(strAccInfo, "disnumbergame-val", GetListOtherComm(CastAide.AsDouble(dr("discountnumber")), CastAide.AsDouble(dr("discountnumber"))))
        End If


        '' if master can transfer everyday, he can set transfer condition for his agent
        Dim strTransferCondition As String = LoadTranferCondition(True)
        If strTransferCondition = "" Then
            dirMaster.Value = "1"
            strDirect = "selectTranferTime();"
            GetTranferTime(strTranferTime)
            TemplateHandler.Assign(strAccInfo, "tranfer-time", strTranferTime)
        Else
            strDirect = ""
            dirMaster.Value = "0"
            TemplateHandler.Assign(strAccInfo, "tranfer-time", "")
        End If

        GetTitleInfo(strAccInfo)






        GetDataInfo = strAccInfo
    End Function
#End Region

#Region "Assign Info for Soccer"
    'this function is used to assign data into template for Soccer
    'dr: row contains data get from db
    Function GetDataSoccer(ByVal dr As DataRow) As String
        Dim soccerTpl As String = String.Empty
        Dim headerSTpl As String = String.Empty
        Dim ptAuto As String = String.Empty
        Dim hiddenHash As New Hashtable

        soccerTpl = TemplateHandler.LoadTemplate("MemberInfo/SoccerPT.html")
        headerSTpl = TemplateHandler.LoadTemplate("MemberInfo/SBettypeHeader.html")
        ' assign all text according language
        TemplateHandler.RemoveBlock(soccerTpl, "none-edit")
        GetTitleCommon(soccerTpl)
        TemplateHandler.Assign(soccerTpl, "col-title", "12")
        TemplateHandler.Assign(soccerTpl, "bettype-header", LoadSoccerPTHeader())
        TemplateHandler.Assign(soccerTpl, "deadball-text", Resources.Lang.lblDeadball)
        TemplateHandler.Assign(soccerTpl, "live-text", Resources.Lang.lblLive)
        TemplateHandler.Assign(soccerTpl, "a-preset-live", Resources.Lang.agentposition)
        TemplateHandler.Assign(soccerTpl, "p-preset-live", Resources.Lang.lblMasterPresetPosition)
        If False Or CastAide.AsBoolean(dr("disableautompt"), False) Then
            TemplateHandler.Assign(soccerTpl, "dead-row", "3")
            TemplateHandler.Assign(soccerTpl, "live-row", "2")
            TemplateHandler.Assign(soccerTpl, "copy-live-col", "2")
            TemplateHandler.Assign(soccerTpl, "hdauto", "hidden")
            ''TemplateHandler.RemoveBlock(soccerTpl, "auto-preset")
        Else
            TemplateHandler.Assign(soccerTpl, "dead-row", "5")
            TemplateHandler.Assign(soccerTpl, "live-row", "4")
            TemplateHandler.Assign(soccerTpl, "copy-live-col", "4")
            TemplateHandler.Assign(soccerTpl, "hdauto", String.Empty)
        End If
        TemplateHandler.Assign(soccerTpl, "copy-1x2-text", Resources.Lang.lblCopyFrom1x2)

        TemplateHandler.Assign(soccerTpl, "chkAuto-1", "chkAuto1HdpD")
        TemplateHandler.Assign(soccerTpl, "chkAuto-2", "chkAuto11x2")
        TemplateHandler.Assign(soccerTpl, "chkAuto-3", "chkAuto1HdpL")

        TemplateHandler.Assign(soccerTpl, "chk-hdpd", "")
        TemplateHandler.Assign(soccerTpl, "chk-1x2", "")
        TemplateHandler.Assign(soccerTpl, "chk-hdpl", "")

        'assign value of Minbet, Maxbet , ...
        GetMinMaxAddValue(soccerTpl, 1, dr)
        GetLimitMinMaxBet(dr, soccerTpl, "")

        'assign value position taking for template
        Dim mHdpMin, mOUMin, m1stHdpMin, m1stOUMin, mOEMin, m1x2Min, mCSMin, mTGMin, mMPMin, mORMin As Double
        Dim mLHdpMin, mLOUMin, mL1stHdpMin, mL1stOUMin As Double

        Dim maHdpMin, maOUMin, ma1stHdpMin, ma1stOUMin, maOEMin, ma1x2Min, maCSMin, maTGMin, maMPMin, maORMin As Double
        Dim maLHdpMin, maLOUMin, maL1stHdpMin, maL1stOUMin As Double

        Dim mHdpMax, mOUMax, m1stHdpMax, m1stOUMax, mOEMax, m1x2Max, mCSMax, mTGMax, mMPMax, mORMax As Double
        Dim mLHdpMax, mLOUMax, mL1stHdpMax, mL1stOUMax As Double

        Dim mHdp, mOU, m1stHdp, m1stOU, mOE, m1x2, mCS, mTG, mMP, mOR As Double
        Dim mLHdp, mLOU, mL1stHdp, mL1stOU As Double

        Dim aHdpMax2, aOUMax2, a1stHdpMax2, a1stOUMax2, aOEMax2, a1x2Max2, aCSMax2, aTGMax2, aMPMax2, aORMax2 As Double
        Dim aLHdpMax2, aLOUMax2, aL1stHdpMax2, aL1stOUMax2 As Double

        Dim aHdpMax, aOUMax, a1stHdpMax, a1stOUMax, aOEMax, a1x2Max, aCSMax, aTGMax, aMPMax, aORMax As Double
        Dim aLHdpMax, aLOUMax, aL1stHdpMax, aL1stOUMax As Double

        Dim aHdpMin, aOUMin, a1stHdpMin, a1stOUMin, aOEMin, a1x2Min, aCSMin, aTGMin, aMPMin, aORMin As Double
        Dim aLHdpMin, aLOUMin, aL1stHdpMin, aL1stOUMin As Double

        Dim aHdp, aOU, a1stHdp, a1stOU, aOE, a1x2, aCS, aTG, aMP, aOR As Double
        Dim aLHdp, aLOU, aL1stHdp, aL1stOU As Double



        aHdpMax2 = CastAide.AsDouble(dr("apositiontaking_1_max")) ' a4
        aOUMax2 = CastAide.AsDouble(dr("apositiontaking_3_max"))
        a1stHdpMax2 = CastAide.AsDouble(dr("apositiontaking_7_max"))
        a1stOUMax2 = CastAide.AsDouble(dr("apositiontaking_8_max"))
        aOEMax2 = CastAide.AsDouble(dr("apositiontaking_2_max"))
        a1x2Max2 = CastAide.AsDouble(dr("apositiontaking_5_max"))
        aCSMax2 = CastAide.AsDouble(dr("apositiontaking_4_max"))
        aTGMax2 = CastAide.AsDouble(dr("apositiontaking_6_max"))
        aMPMax2 = CastAide.AsDouble(dr("apositiontaking_9_max"))
        aORMax2 = CastAide.AsDouble(dr("apositiontaking_10_max"))
        aLHdpMax2 = CastAide.AsDouble(dr("alivepositiontaking_1_max"))
        aLOUMax2 = CastAide.AsDouble(dr("alivepositiontaking_3_max"))
        aL1stHdpMax2 = CastAide.AsDouble(dr("alivepositiontaking_7_max"))
        aL1stOUMax2 = CastAide.AsDouble(dr("alivepositiontaking_8_max"))

        mHdpMax = CastAide.AsDouble(dr("mpositiontaking_1")) ' min(l3,m3)
        mOUMax = CastAide.AsDouble(dr("mpositiontaking_3"))
        m1stHdpMax = CastAide.AsDouble(dr("mpositiontaking_7"))
        m1stOUMax = CastAide.AsDouble(dr("mpositiontaking_8"))
        mOEMax = CastAide.AsDouble(dr("mpositiontaking_2"))
        m1x2Max = CastAide.AsDouble(dr("mpositiontaking_5"))
        mCSMax = CastAide.AsDouble(dr("mpositiontaking_4"))
        mTGMax = CastAide.AsDouble(dr("mpositiontaking_6"))
        mMPMax = CastAide.AsDouble(dr("mpositiontaking_9"))
        mORMax = CastAide.AsDouble(dr("mpositiontaking_10"))
        mLHdpMax = CastAide.AsDouble(dr("mlivepositiontaking_1"))
        mLOUMax = CastAide.AsDouble(dr("mlivepositiontaking_3"))
        mL1stHdpMax = CastAide.AsDouble(dr("mlivepositiontaking_7"))
        mL1stOUMax = CastAide.AsDouble(dr("mlivepositiontaking_8"))
        mHdp = mHdpMax
        mOU = mOUMax
        m1stHdp = m1stHdpMax
        m1stOU = m1stOUMax
        mOE = mOEMax
        m1x2 = m1x2Max
        mCS = mCSMax
        mTG = mTGMax
        mMP = mMPMax
        mOR = mORMax
        mLHdp = mLHdpMax
        mLOU = mLOUMax
        mL1stHdp = mL1stHdpMax
        mL1stOU = mL1stOUMax

        maHdpMin = CastAide.AsDouble(dr("mpositiontaking_1_min"))
        maOUMin = CastAide.AsDouble(dr("mpositiontaking_3_min"))
        ma1stHdpMin = CastAide.AsDouble(dr("mpositiontaking_7_min"))
        ma1stOUMin = CastAide.AsDouble(dr("mpositiontaking_8_min"))
        maOEMin = CastAide.AsDouble(dr("mpositiontaking_2_min"))
        ma1x2Min = CastAide.AsDouble(dr("mpositiontaking_5_min"))
        maCSMin = CastAide.AsDouble(dr("mpositiontaking_4_min"))
        maTGMin = CastAide.AsDouble(dr("mpositiontaking_6_min"))
        maMPMin = CastAide.AsDouble(dr("mpositiontaking_9_min"))
        maORMin = CastAide.AsDouble(dr("mpositiontaking_10_min"))
        maLHdpMin = CastAide.AsDouble(dr("mlivepositiontaking_1_min"))
        maLOUMin = CastAide.AsDouble(dr("mlivepositiontaking_3_min"))
        maL1stHdpMin = CastAide.AsDouble(dr("mlivepositiontaking_7_min"))
        maL1stOUMin = CastAide.AsDouble(dr("mlivepositiontaking_8_min"))
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        aHdpMax = IIf(mHdpMax - mHdp > aHdpMax2, aHdpMax2, mHdpMax - mHdp)


        aOUMax = IIf(mOUMax - mOU > aOUMax2, aOUMax2, mOUMax - mOU)
        a1stHdpMax = IIf(m1stHdpMax - m1stHdp > a1stHdpMax2, a1stHdpMax2, m1stHdpMax - m1stHdp)
        a1stOUMax = IIf(m1stOUMax - m1stOU > a1stOUMax2, a1stOUMax2, m1stOUMax - m1stOU)
        aOEMax = IIf(mOEMax - mOE > aOEMax2, aOEMax2, mOEMax - mOE)
        a1x2Max = IIf(m1x2Max - m1x2 > a1x2Max2, a1x2Max2, m1x2Max - m1x2)
        aCSMax = IIf(mCSMax - mCS > aCSMax2, aCSMax2, mCSMax - mCS)
        aTGMax = IIf(mTGMax - mTG > aTGMax2, aTGMax2, mTGMax - mTG)
        aMPMax = IIf(mMPMax - mMP > aMPMax2, aMPMax2, mMPMax - mMP)
        aORMax = IIf(mORMax - mOR > aORMax2, aORMax2, mORMax - mOR)
        aLHdpMax = IIf(mLHdpMax - mLHdp > aLHdpMax2, aLHdpMax2, mLHdpMax - mLHdp)
        aLOUMax = IIf(mLOUMax - mLOU > aLOUMax2, aLOUMax2, mLOUMax - mLOU)
        aL1stHdpMax = IIf(mL1stHdpMax - mL1stHdp > aL1stHdpMax2, aL1stHdpMax2, mL1stHdpMax - mL1stHdp)
        aL1stOUMax = IIf(mL1stOUMax - mL1stOU > aL1stOUMax2, aL1stOUMax2, mL1stOUMax - mL1stOU)
        aHdp = aHdpMax
        aOU = aOUMax
        a1stHdp = a1stHdpMax
        a1stOU = a1stOUMax
        aOE = aOEMax
        a1x2 = a1x2Max
        aCS = aCSMax
        aTG = aTGMax
        aMP = aMPMax
        aOR = aORMax
        aLHdp = aLHdpMax
        aLOU = aLOUMax
        aL1stHdp = aL1stHdpMax
        aL1stOU = aL1stOUMax

        hiddenHash.Add("minM1HdpD", mHdpMin)
        hiddenHash.Add("minM1OUD", mOUMin)
        hiddenHash.Add("minM11stHdpD", m1stHdpMin)
        hiddenHash.Add("minM11stOUD", m1stOUMin)
        hiddenHash.Add("minM1OE", mOEMin)

        hiddenHash.Add("minM11x2", m1x2Min)
        hiddenHash.Add("minM1CS", mCSMin)
        hiddenHash.Add("minM1TG", mTGMin)
        hiddenHash.Add("minM1MP", mMPMin)
        hiddenHash.Add("minM1OR", mORMin)

        hiddenHash.Add("minM1HdpL", mLHdpMin)
        hiddenHash.Add("minM1OUL", mLOUMin)
        hiddenHash.Add("minM11stHdpL", mL1stHdpMin)
        hiddenHash.Add("minM11stOUL", mL1stOUMin)

        hiddenHash.Add("maxA1HdpD", IIf(mHdpMax > aHdpMax2, aHdpMax2, mHdpMax))
        hiddenHash.Add("maxA1OUD", IIf(mOUMax > aOUMax2, aOUMax2, mOUMax))
        hiddenHash.Add("maxA11stHdpD", IIf(m1stHdpMax > a1stHdpMax2, a1stHdpMax2, m1stHdpMax))
        hiddenHash.Add("maxA11stOUD", IIf(m1stOUMax > a1stOUMax2, a1stOUMax2, m1stOUMax))
        hiddenHash.Add("maxA1OE", IIf(mOEMax > aOEMax2, aOEMax2, mOEMax))

        hiddenHash.Add("maxA11x2", IIf(m1x2Max > a1x2Max2, a1x2Max2, m1x2Max))
        hiddenHash.Add("maxA1CS", IIf(mCSMax > aCSMax2, aCSMax2, mCSMax))
        hiddenHash.Add("maxA1TG", IIf(mTGMax > aTGMax2, aTGMax2, mTGMax))
        hiddenHash.Add("maxA1MP", IIf(mMPMax > aMPMax2, aMPMax2, mMPMax))
        hiddenHash.Add("maxA1OR", IIf(mORMax > aORMax2, aORMax2, mORMax))

        hiddenHash.Add("maxA1HdpL", IIf(mLHdpMax > aLHdpMax2, aLHdpMax2, mLHdpMax))
        hiddenHash.Add("maxA1OUL", IIf(mLOUMax > aLOUMax2, aLOUMax2, mLOUMax))
        hiddenHash.Add("maxA11stHdpL", IIf(mL1stHdpMax > aL1stHdpMax2, aL1stHdpMax2, mL1stHdpMax))
        hiddenHash.Add("maxA11stOUL", IIf(mL1stOUMax > aL1stOUMax2, aL1stOUMax2, mL1stOUMax))

        hiddenHash.Add("maxM1HdpD", mHdpMax)
        hiddenHash.Add("maxM1OUD", mOUMax)
        hiddenHash.Add("maxM11stHdpD", m1stHdpMax)
        hiddenHash.Add("maxM11stOUD", m1stOUMax)
        hiddenHash.Add("maxM1OE", mOEMax)

        hiddenHash.Add("maxM11x2", m1x2Max)
        hiddenHash.Add("maxM1CS", mCSMax)
        hiddenHash.Add("maxM1TG", mTGMax)
        hiddenHash.Add("maxM1MP", mMPMax)
        hiddenHash.Add("maxM1OR", mORMax)

        hiddenHash.Add("maxM1HdpL", mLHdpMax)
        hiddenHash.Add("maxM1OUL", mLOUMax)
        hiddenHash.Add("maxM11stHdpL", mL1stHdpMax)
        hiddenHash.Add("maxM11stOUL", mL1stOUMax)

        strHidSoccer = PositionTaking.AssignMulipleHidden(hiddenHash)

        'assign Increase and Reduce button for template
        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM1HdpD")
        TemplateHandler.Assign(soccerTpl, "a-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM11x2")
        TemplateHandler.Assign(soccerTpl, "a-1x2-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listS1HdpD")
        TemplateHandler.Assign(soccerTpl, "p-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listS11x2")
        TemplateHandler.Assign(soccerTpl, "p-1x2-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM1HdpL")
        TemplateHandler.Assign(soccerTpl, "a-Lhdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listS1HdpL")
        TemplateHandler.Assign(soccerTpl, "p-Lhdp-btn", btnTpl)

        'get PT deadball value for master preset PT
        Dim pt As String = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1HdpD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aHdpMin, aHdpMax, aHdp))
        TemplateHandler.Assign(soccerTpl, "a-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1OUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aOUMin, aOUMax, aOU))
        TemplateHandler.Assign(soccerTpl, "a-ou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM11stHdpD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(a1stHdpMin, a1stHdpMax, a1stHdp))
        TemplateHandler.Assign(soccerTpl, "a-1sthdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM11stOUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(a1stOUMin, a1stOUMax, a1stOU))
        TemplateHandler.Assign(soccerTpl, "a-1stou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1OE")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aOEMin, aOEMax, aOE))
        TemplateHandler.Assign(soccerTpl, "a-oe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM11x2")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(a1x2Min, a1x2Max, a1x2))
        TemplateHandler.Assign(soccerTpl, "a-1x2-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1CS")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aCSMin, aCSMax, aCS))
        TemplateHandler.Assign(soccerTpl, "a-cs-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1TG")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aTGMin, aTGMax, aTG))
        TemplateHandler.Assign(soccerTpl, "a-tg-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1MP")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aMPMin, aMPMax, aMP))
        TemplateHandler.Assign(soccerTpl, "a-mp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1OR")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aORMin, aORMax, aOR))
        TemplateHandler.Assign(soccerTpl, "a-or-val", pt)

        'get PT deadball value for super preset PT
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS1HdpD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
        TemplateHandler.Assign(soccerTpl, "p-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS1OUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOU))
        TemplateHandler.Assign(soccerTpl, "p-ou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS11stHdpD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1stHdpMin, m1stHdpMax, m1stHdp))
        TemplateHandler.Assign(soccerTpl, "p-1sthdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS11stOUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1stOUMin, m1stOUMax, m1stOU))
        TemplateHandler.Assign(soccerTpl, "p-1stou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS1OE")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOE))
        TemplateHandler.Assign(soccerTpl, "p-oe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS11x2")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1x2Min, m1x2Max, m1x2))
        TemplateHandler.Assign(soccerTpl, "p-1x2-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS1CS")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mCSMin, mCSMax, mCS))
        TemplateHandler.Assign(soccerTpl, "p-cs-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS1TG")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mTGMin, mTGMax, mTG))
        TemplateHandler.Assign(soccerTpl, "p-tg-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS1MP")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMPMin, mMPMax, mMP))
        TemplateHandler.Assign(soccerTpl, "p-mp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS1OR")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mORMin, mORMax, mOR))
        TemplateHandler.Assign(soccerTpl, "p-or-val", pt)

        'get PT value for super preset PT
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1HdpL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aLHdpMin, aLHdpMax, aLHdp))
        TemplateHandler.Assign(soccerTpl, "a-Lhdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1OUL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aLOUMin, aLOUMax, aLOU))
        TemplateHandler.Assign(soccerTpl, "a-Lou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM11stHdpL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aL1stHdpMin, aL1stHdpMax, aL1stHdp))
        TemplateHandler.Assign(soccerTpl, "a-L1stHdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM11stOUL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aL1stOUMin, aL1stOUMax, aL1stOU))
        TemplateHandler.Assign(soccerTpl, "a-L1stou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS1HdpL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLHdpMin, mLHdpMax, mLHdp))
        TemplateHandler.Assign(soccerTpl, "p-Lhdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS1OUL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLOUMin, mLOUMax, mLOU))
        TemplateHandler.Assign(soccerTpl, "p-Lou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS11stHdpL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mL1stHdpMin, mL1stHdpMax, mL1stHdp))
        TemplateHandler.Assign(soccerTpl, "p-L1stHdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS11stOUL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mL1stOUMin, mL1stOUMax, mL1stOU))
        TemplateHandler.Assign(soccerTpl, "p-L1stou-val", pt)
        ' Get value for Auto Postion taking-------------------------------------------------------------------------------

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS1HdpD")
        TemplateHandler.Assign(soccerTpl, "au-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS11x2")
        TemplateHandler.Assign(soccerTpl, "au-1x2-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS1HdpL")
        TemplateHandler.Assign(soccerTpl, "au-Lhdp-btn", btnTpl)

        'For Deadball

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1HdpD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mHdpMax, 0))
        TemplateHandler.Assign(soccerTpl, "au-hdp-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OUD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mOUMax, 0))
        TemplateHandler.Assign(soccerTpl, "au-ou-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stHdpD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, m1stHdpMax, 0))
        TemplateHandler.Assign(soccerTpl, "au-1sthdp-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stOUD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, m1stOUMax, 0))
        TemplateHandler.Assign(soccerTpl, "au-1stou-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OE")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mOEMax, 0))
        TemplateHandler.Assign(soccerTpl, "au-oe-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11x2")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, m1x2Max, 0))
        TemplateHandler.Assign(soccerTpl, "au-1x2-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1CS")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mCSMax, 0))
        TemplateHandler.Assign(soccerTpl, "au-cs-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1TG")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mTGMax, 0))
        TemplateHandler.Assign(soccerTpl, "au-tg-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1MP")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mMPMax, 0))
        TemplateHandler.Assign(soccerTpl, "au-mp-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OR")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mORMax, 0))
        TemplateHandler.Assign(soccerTpl, "au-or-val", ptAuto)




        'For live ball
        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1HdpL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mLHdpMax, 0))
        TemplateHandler.Assign(soccerTpl, "au-Lhdp-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OUL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mLOUMax, 0))
        TemplateHandler.Assign(soccerTpl, "au-Lou-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stHdpL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mL1stHdpMax, 0))
        TemplateHandler.Assign(soccerTpl, "au-L1stHdp-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stOUL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mL1stOUMax, 0))
        TemplateHandler.Assign(soccerTpl, "au-L1stou-val", ptAuto)

        '-------------------------------------------------------------------------------------------------------------------------


        GetDataSoccer = soccerTpl
    End Function
#End Region

#Region "Assign info for  Basketball"
    'this function is used to assign data into template for all sporttype that have 4 bettype
    '(1:Handicap, 3:Over/Under, 2:Odd/Even, 9:Mix Parlay, 20:Money Line)
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (2: Basketball )
    Function GetDataBasketball(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String) As String
        Dim pt As String
        Dim ptAuto As String = String.Empty
        Dim hiddenHash As New Hashtable

        'assign text for position taking
        GetTitleCommon(tpl)
        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "oe-text", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(tpl, "mp-text", Resources.Lang.lblMixParlay)
        TemplateHandler.Assign(tpl, "title-col", "7")
        TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "chk-hdp", "")
        TemplateHandler.RemoveBlock(tpl, "none-edit")
        'assign value for position taking
        GetMinMaxAddValue(tpl, sportTypeId, dr)
        GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

        Dim mHdpMin, mOUMin, mOEMin, mMLMin As Double
        Dim mHdpMax, mOUMax, mOEMax, mMLMax As Double
        Dim mHdp, mOU, mOE, mML As Double

        Dim aHdpMin, aOUMin, aOEMin, aMLMin As Double
        Dim aHdpMax, aOUMax, aOEMax, aMLMax As Double
        Dim aHdp, aOU, aOE, aML As Double
        Dim aHdpMax2, aOUMax2, aOEMax2, aMLMax2 As Double
        Dim maHdpMin, maOUMin, maOEMin, maMLMin As Double

        aHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_max")) 'a4
        aOUMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3_max"))
        aOEMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2_max"))
        aMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_max"))

        mHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1")) 'min(m3,l3)
        mOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
        mOEMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))
        mMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))
        mHdp = mHdpMax
        mOU = mOUMax
        mOE = mOEMax
        mML = mMLMax

        maHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_min"))
        maOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_min"))
        maOEMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2_min"))
        maMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_min"))

        aHdpMax = IIf(mHdpMax - mHdp > aHdpMax2, aHdpMax2, mHdpMax - mHdp)
        aOUMax = IIf(mOUMax - mOU > aOUMax2, aOUMax2, mOUMax - mOU)
        aOEMax = IIf(mOEMax - mOE > aOEMax2, aOEMax2, mOEMax - mOE)
        aMLMax = IIf(mMLMax - mML > aMLMax2, aMLMax2, mMLMax - mML)
        aHdp = aHdpMax
        aOU = aOUMax
        aOE = aOEMax
        aML = aMLMax

        hiddenHash.Add("minM" & sportTypeId & "Hdp", maHdpMin)
        hiddenHash.Add("minM" & sportTypeId & "OU", maOUMin)
        hiddenHash.Add("minM" & sportTypeId & "OE", maOEMin)
        hiddenHash.Add("minM" & sportTypeId & "ML", maMLMin)

        hiddenHash.Add("maxA" & sportTypeId & "Hdp", IIf(mHdpMax > aHdpMax2, aHdpMax2, mHdpMax))
        hiddenHash.Add("maxA" & sportTypeId & "OU", IIf(mOUMax > aOUMax2, aOUMax2, mOUMax))
        hiddenHash.Add("maxA" & sportTypeId & "OE", IIf(mOEMax > aOEMax2, aOEMax2, mOEMax))
        hiddenHash.Add("maxA" & sportTypeId & "ML", IIf(mMLMax > aMLMax2, aMLMax2, mMLMax))

        hiddenHash.Add("maxM" & sportTypeId & "Hdp", mHdpMax)
        hiddenHash.Add("maxM" & sportTypeId & "OU", mOUMax)
        hiddenHash.Add("maxM" & sportTypeId & "OE", mOEMax)
        hiddenHash.Add("maxM" & sportTypeId & "ML", mMLMax)
        strHidBasket = PositionTaking.AssignMulipleHidden(hiddenHash)

        'assign data for Position taking dropdowns
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aHdpMin, aHdpMax, aHdpMax))
        TemplateHandler.Assign(tpl, "a-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aOUMin, aOUMax, aOUMax))
        TemplateHandler.Assign(tpl, "a-ou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OE")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aOEMin, aOEMax, aOEMax))
        TemplateHandler.Assign(tpl, "a-oe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aMLMin, aMLMax, aMLMax))
        TemplateHandler.Assign(tpl, "a-ml-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdpMax))
        TemplateHandler.Assign(tpl, "p-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OU")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOUMax))
        TemplateHandler.Assign(tpl, "p-ou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OE")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOEMax))
        TemplateHandler.Assign(tpl, "p-oe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mMLMax))
        TemplateHandler.Assign(tpl, "p-ml-val", pt)

        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)

        '----------------------------------------------------------------------------------------------------------------
        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mHdpMax, 0))
        TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-Hdp", String.Empty)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mOUMax, 0))
        TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-OU", String.Empty)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OE")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mOEMax, 0))
        TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-OE", String.Empty)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mMLMax, 0))
        TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-ML", String.Empty)


        GetDataBasketball = tpl
    End Function
#End Region

#Region "Assign info for Football, Tennis, Golf, OtherSports"
    'this function is used to assign data into template for all sporttype that have 4 bettype
    '(1:Handicap, 3:Over/Under, 2:Odd/Even, 20:Money Line)
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (3:Football, 5:Tennis, 10:Golf, 99:OtherSports )
    Function GetData4DDl(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String) As String
        Dim pt As String
        Dim ptAuto As String = String.Empty
        Dim hiddenHash As New Hashtable

        'assign text for position taking
        GetTitleCommon(tpl)
        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "oe-text", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(tpl, "title-col", "6")
        TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "chk-hdp", "")
        TemplateHandler.RemoveBlock(tpl, "none-edit")
        'assign value for position taking
        GetMinMaxAddValue(tpl, sportTypeId, dr)
        GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

        Dim mHdpMin, mOUMin, mOEMin, mMLMin As Double
        Dim mHdpMax, mOUMax, mOEMax, mMLMax As Double
        Dim mHdp, mOU, mOE, mML As Double

        Dim aHdpMin, aOUMin, aOEMin, aMLMin As Double
        Dim aHdpMax, aOUMax, aOEMax, aMLMax As Double
        Dim aHdp, aOU, aOE, aML As Double
        Dim aHdpMax2, aOUMax2, aOEMax2, aMLMax2 As Double
        Dim maHdpMin, maOUMin, maOEMin, maMLMin As Double

        aHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_max")) 'a4
        aOUMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3_max"))
        aOEMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2_max"))
        aMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_max"))

        mHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1")) 'min(m3,l3)
        mOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
        mOEMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))
        mMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))
        mHdp = mHdpMax
        mOU = mOUMax
        mOE = mOEMax
        mML = mMLMax

        maHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_min"))
        maOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_min"))
        maOEMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2_min"))
        maMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_min"))

        aHdpMax = IIf(mHdpMax - mHdp > aHdpMax2, aHdpMax2, mHdpMax - mHdp)
        aOUMax = IIf(mOUMax - mOU > aOUMax2, aOUMax2, mOUMax - mOU)
        aOEMax = IIf(mOEMax - mOE > aOEMax2, aOEMax2, mOEMax - mOE)
        aMLMax = IIf(mMLMax - mML > aMLMax2, aMLMax2, mMLMax - mML)
        aHdp = aHdpMax
        aOU = aOUMax
        aOE = aOEMax
        aML = aMLMax

        hiddenHash.Add("minM" & sportTypeId & "Hdp", maHdpMin)
        hiddenHash.Add("minM" & sportTypeId & "OU", maOUMin)
        hiddenHash.Add("minM" & sportTypeId & "OE", maOEMin)
        hiddenHash.Add("minM" & sportTypeId & "ML", maMLMin)

        hiddenHash.Add("maxA" & sportTypeId & "Hdp", IIf(mHdpMax > aHdpMax2, aHdpMax2, mHdpMax))
        hiddenHash.Add("maxA" & sportTypeId & "OU", IIf(mOUMax > aOUMax2, aOUMax2, mOUMax))
        hiddenHash.Add("maxA" & sportTypeId & "OE", IIf(mOEMax > aOEMax2, aOEMax2, mOEMax))
        hiddenHash.Add("maxA" & sportTypeId & "ML", IIf(mMLMax > aMLMax2, aMLMax2, mMLMax))

        hiddenHash.Add("maxM" & sportTypeId & "Hdp", mHdpMax)
        hiddenHash.Add("maxM" & sportTypeId & "OU", mOUMax)
        hiddenHash.Add("maxM" & sportTypeId & "OE", mOEMax)
        hiddenHash.Add("maxM" & sportTypeId & "ML", mMLMax)

        Select Case sportTypeId
            Case 3
                strHidFootball = PositionTaking.AssignMulipleHidden(hiddenHash)
            Case 5
                strHidTennis = PositionTaking.AssignMulipleHidden(hiddenHash)
            Case 10
                strHidGolf = PositionTaking.AssignMulipleHidden(hiddenHash)
            Case 99
                strHidOther = PositionTaking.AssignMulipleHidden(hiddenHash)
        End Select

        'assign data for Position taking dropdowns
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aHdpMin, aHdpMax, aHdpMax))
        TemplateHandler.Assign(tpl, "a-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aOUMin, aOUMax, aOUMax))
        TemplateHandler.Assign(tpl, "a-ou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OE")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aOEMin, aOEMax, aOEMax))
        TemplateHandler.Assign(tpl, "a-oe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aMLMin, aMLMax, aMLMax))
        TemplateHandler.Assign(tpl, "a-ml-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdpMax))
        TemplateHandler.Assign(tpl, "p-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OU")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOUMax))
        TemplateHandler.Assign(tpl, "p-ou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OE")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOEMax))
        TemplateHandler.Assign(tpl, "p-oe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mMLMax))
        TemplateHandler.Assign(tpl, "p-ml-val", pt)

        '----------------------------------------------------------------------------------------------------------------
        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mHdpMax, 0))
        TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-Hdp", String.Empty)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mOUMax, 0))
        TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-OU", String.Empty)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OE")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mOEMax, 0))
        TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-OE", String.Empty)


        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mMLMax, 0))
        TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-ML", String.Empty)
        '/--------------------------------------------------------------------------------------------------------------
        Dim btnTpl As String = groupBtn
        '--------------------------------------------------------------------------------------------------------------
        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)
        '/--------------------------------------------------------------------------------------------------------------
        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)



        GetData4DDl = tpl
    End Function
#End Region

#Region "Assign info for  Baseball"
    'this function is used to assign data into template for Baseball
    'dr: row contains data get from db
    'sportTypeId: id of sporttype
    'Baseball have only Handicap, Over/Under & Money Line bettype
    Function GetDataBaseball(ByVal dr As DataRow, ByVal sportTypeId As Integer) As String
        Dim pt As String
        Dim tpl As String
        Dim ptAuto As String = String.Empty
        Dim hiddenHash As New Hashtable

        'assign text for position taking
        tpl = TemplateHandler.LoadTemplate("MemberInfo/BaseballPT.html")
        GetTitleCommon(tpl)
        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "title-col", "5")
        TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "chk-hdp", "")
        TemplateHandler.RemoveBlock(tpl, "none-edit")
        'assign value for position taking
        GetMinMaxAddValue(tpl, sportTypeId, dr)
        GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

        Dim mHdpMin, mOUMin, mMLMin As Double
        Dim mHdpMax, mOUMax, mMLMax As Double
        Dim mHdp, mOU, mML As Double

        Dim aHdpMax, aOUMax, aMLMax As Double
        Dim aHdpMin, aOUMin, aMLMin As Double
        Dim aHdp, aOU, aML As Double
        Dim aHdpMax2, aOUMax2, aMLMax2 As Double
        Dim maHdpMin, maOUMin, maMLMin As Double

        aHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_max")) 'a4
        aOUMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3_max"))
        aMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_max"))

        mHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1")) 'min(m3,l3)
        mOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
        mMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))
        mHdp = mHdpMax
        mOU = mOUMax
        mML = mMLMax

        maHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_min"))
        maOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_min"))
        maMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_min"))

        aHdpMax = IIf(mHdpMax - mHdp > aHdpMax2, aHdpMax2, mHdpMax - mHdp)
        aOUMax = IIf(mOUMax - mOU > aOUMax2, aOUMax2, mOUMax - mOU)
        aMLMax = IIf(mMLMax - mML > aMLMax2, aMLMax2, mMLMax - mML)
        aHdp = aHdpMax
        aOU = aOUMax
        aML = aMLMax

        hiddenHash.Add("minM" & sportTypeId & "Hdp", maHdpMin)
        hiddenHash.Add("minM" & sportTypeId & "OU", maOUMin)
        hiddenHash.Add("minM" & sportTypeId & "ML", maMLMin)

        hiddenHash.Add("maxA" & sportTypeId & "Hdp", IIf(mHdpMax > aHdpMax2, aHdpMax2, mHdpMax))
        hiddenHash.Add("maxA" & sportTypeId & "OU", IIf(mOUMax > aOUMax2, aOUMax2, mOUMax))
        hiddenHash.Add("maxA" & sportTypeId & "ML", IIf(mMLMax > aMLMax2, aMLMax2, mMLMax))

        hiddenHash.Add("maxM" & sportTypeId & "Hdp", mHdpMax)
        hiddenHash.Add("maxM" & sportTypeId & "OU", mOUMax)
        hiddenHash.Add("maxM" & sportTypeId & "ML", mMLMax)
        strHidBaseball = PositionTaking.AssignMulipleHidden(hiddenHash)

        'assign data for Position taking dropdowns
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aHdpMin, aHdpMax, aHdp))
        TemplateHandler.Assign(tpl, "a-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aOUMin, aOUMax, aOU))
        TemplateHandler.Assign(tpl, "a-ou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aMLMin, aMLMax, aML))
        TemplateHandler.Assign(tpl, "a-ml-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
        TemplateHandler.Assign(tpl, "p-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OU")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOU))
        TemplateHandler.Assign(tpl, "p-ou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mML))
        TemplateHandler.Assign(tpl, "p-ml-val", pt)

        '----------------------------------------------------------------------------------------------------------------

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mHdpMax, 0))
        TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-Hdp", String.Empty)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mOUMax, 0))
        TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-OU", String.Empty)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mMLMax, 0))
        TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-ML", String.Empty)


        Dim btnTpl As String = groupBtn

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)
        '/-------------------------------------------------------------------------------------------------------------------

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)

        GetDataBaseball = tpl
    End Function
#End Region

#Region "Assign info for MotoSports"
    'this function is used to assign data into template for MotoSports
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (11: motosports)
    'Motosports have only Handicap & Money Line bettype
    Function GetDataMotoSports(ByVal dr As DataRow, ByVal sportTypeId As Integer) As String
        Dim pt As String
        Dim tpl As String
        Dim ptAuto As String = String.Empty
        Dim hiddenHash As New Hashtable

        'assign text for position taking
        tpl = TemplateHandler.LoadTemplate("MemberInfo/MotorSportsPT.html")
        GetTitleCommon(tpl)
        TemplateHandler.Assign(tpl, "title-col", "4")
        TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "chk-hdp", "")
        TemplateHandler.RemoveBlock(tpl, "none-edit")
        'assign value for minbet, maxbet, maxpermatch (all are 0 for adding case)            
        GetMinMaxAddValue(tpl, 11, dr)
        GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

        'get data and assign for template Position taking
        Dim mHdpMin, mMLMin As Double
        Dim mHdpMax, mMLMax As Double
        Dim mHdp, mML As Double

        Dim aHdpMax, aMLMax As Double
        Dim aHdpMin, aMLMin As Double
        Dim aHdp, aML As Double
        Dim aHdpMax2, aMLMax2 As Double
        Dim maHdpMin, maMLMin As Double

        aHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_max")) 'a4
        aMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_max"))

        mHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1")) 'min(m3,l3)
        mMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))
        mHdp = mHdpMax
        mML = mMLMax

        maHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_min"))
        maMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_min"))

        aHdpMax = IIf(mHdpMax - mHdp > aHdpMax2, aHdpMax2, mHdpMax - mHdp)
        aMLMax = IIf(mMLMax - mML > aMLMax2, aMLMax2, mMLMax - mML)
        aHdp = aHdpMax
        aML = aMLMax

        hiddenHash.Add("minM" & sportTypeId & "Hdp", maHdpMin)
        hiddenHash.Add("minM" & sportTypeId & "ML", maMLMin)

        hiddenHash.Add("maxA" & sportTypeId & "Hdp", IIf(mHdpMax > aHdpMax2, aHdpMax2, mHdpMax))
        hiddenHash.Add("maxA" & sportTypeId & "ML", IIf(mMLMax > aMLMax2, aMLMax2, mMLMax))

        hiddenHash.Add("maxM" & sportTypeId & "Hdp", mHdpMax)
        hiddenHash.Add("maxM" & sportTypeId & "ML", mMLMax)
        strHidMoto = PositionTaking.AssignMulipleHidden(hiddenHash)

        'assign data for Position taking dropdowns
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aHdpMin, aHdpMax, aHdp))
        TemplateHandler.Assign(tpl, "a-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aMLMin, aMLMax, aML))
        TemplateHandler.Assign(tpl, "a-ml-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
        TemplateHandler.Assign(tpl, "p-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mML))
        TemplateHandler.Assign(tpl, "p-ml-val", pt)

        '----------------------------------------------------------------------------------------------------------------

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mHdpMax, 0))
        TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-Hdp", String.Empty)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mMLMax, 0))
        TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-ML", String.Empty)


        Dim btnTpl As String = groupBtn

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)
        '----------------------------------------------------------------------------------------------------------------
        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)

        GetDataMotoSports = tpl
    End Function
#End Region

#Region "Assign Info for Number Game"

    Function GetDataNumberGame(ByVal dr As DataRow) As String
        Dim numbergameTpl As String = String.Empty
        Dim headerSTpl As String = String.Empty
        Dim ptAuto As String = String.Empty
        Dim hiddenHash As New Hashtable

        numbergameTpl = TemplateHandler.LoadTemplate("MemberInfo/NumberGamePT.html")


        If (Not UserPermission.IsShowNumberGame Or Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission) Then
            TemplateHandler.Assign(numbergameTpl, "hideClss", "hidden")
        Else
            TemplateHandler.Assign(numbergameTpl, "hideClss", String.Empty)
        End If

        If UserPermission.IsShowNumberGame Then
            TemplateHandler.Assign(numbergameTpl, "othersetting", Resources.Lang.othersetting)
            TemplateHandler.Assign(numbergameTpl, "lblMaxPerBall", Resources.Lang.maxperball)
            '
            Dim maxperballmax As Double = DataFormat.FormatNumber1(dr("MaxPerBallv161_Max"))
            TemplateHandler.Assign(numbergameTpl, "MaxPerBall-val", NumberFormat.DoubleFormat(maxperballmax, 0))
            TemplateHandler.Assign(numbergameTpl, "MaxPerBall-max", NumberFormat.DoubleFormat(maxperballmax, 0))
            If maxperballmax = 0 Then
                TemplateHandler.Assign(numbergameTpl, "maxperball_clss", "hidden")
            Else
                TemplateHandler.Assign(numbergameTpl, "maxperball_clss", "minmaxbet")
            End If
        Else
            TemplateHandler.RemoveBlock(numbergameTpl, "none-maxperball")
        End If

        ' assign all text according language
        TemplateHandler.RemoveBlock(numbergameTpl, "none-edit")
        GetTitleCommon(numbergameTpl)
        TemplateHandler.Assign(numbergameTpl, "col-title", "12")
        TemplateHandler.Assign(numbergameTpl, "bettype-header", LoadNumberGameHeader())
        TemplateHandler.Assign(numbergameTpl, "deadball-text", Resources.Lang.lblDeadball)
        TemplateHandler.Assign(numbergameTpl, "live-text", Resources.Lang.lblLive)
        TemplateHandler.Assign(numbergameTpl, "a-preset-live", Resources.Lang.agentposition)
        TemplateHandler.Assign(numbergameTpl, "p-preset-live", Resources.Lang.lblMasterPresetPosition)
        TemplateHandler.Assign(numbergameTpl, "copy-1stlastou-text", Resources.Lang.lbl1stcopyfromoverunder)
        TemplateHandler.Assign(numbergameTpl, "copy-oulive-text", Resources.Lang.lblcopyfromoverunder)

        TemplateHandler.Assign(numbergameTpl, "overunder-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(numbergameTpl, "oddeven-text", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(numbergameTpl, "highlow-text", Resources.Lang.highlow)
        TemplateHandler.Assign(numbergameTpl, "next-combo-text", Resources.Lang.nextcombo)

        If False Or CastAide.AsBoolean(dr("disableautompt"), False) Then
            TemplateHandler.Assign(numbergameTpl, "dead-row", "2")
            TemplateHandler.Assign(numbergameTpl, "live-row", "2")
            TemplateHandler.Assign(numbergameTpl, "copy-live-col", "2")
            TemplateHandler.Assign(numbergameTpl, "hdauto", "hidden")
        Else
            TemplateHandler.Assign(numbergameTpl, "dead-row", "4")
            TemplateHandler.Assign(numbergameTpl, "live-row", "4")
            TemplateHandler.Assign(numbergameTpl, "copy-live-col", "2")
            TemplateHandler.Assign(numbergameTpl, "hdauto", String.Empty)
        End If


        'assign value of Minbet, Maxbet , ...
        GetMinMaxAddValue(numbergameTpl, 161, dr)
        GetLimitMinMaxBet(dr, numbergameTpl, "161")

        If (Not UserPermission.IsShowNumberGroup) Then
            TemplateHandler.Assign(numbergameTpl, "hide", "hide")
        End If

        'assign value position taking for template
        Dim m1stOUMin, m1stOEMin, mOEMin, mWRMin As Double
        Dim mLOUMin, mLOEMin, mLHLMin, mLNCMin, mLNGMin As Double

        Dim ma1stOUMin, ma1stOEMin, maOEMin, maWRMin As Double
        Dim maLOUMin, maLOEMin, maLHLMin, maLNCMin, maLNGMin As Double

        Dim m1stOUMax, m1stOEMax, mOEMax, mWRMax As Double
        Dim mLOUMax, mLOEMax, mLHLMax, mLNCMax, mLNGMax As Double

        Dim m1stOU, m1stOE, mOE, mWR As Double
        Dim mLOU, mLOE, mLHL, mLNC, mLNG As Double

        Dim a1stOUMax2, a1stOEMax2, aOEMax2, aWRMax2 As Double
        Dim aLOUMax2, aLOEMax2, aLHLMax2, aLNCMax2, aLNGMax2 As Double

        Dim a1stOUMax, a1stOEMax, aOEMax, aWRMax As Double
        Dim aLOUMax, aLOEMax, aLHLMax, aLNCMax, aLNGMax As Double

        Dim a1stOUMin, a1stOEMin, aOEMin, aWRMin As Double
        Dim aLOUMin, aLOEMin, aLHLMin, aLNCMin, aLNGMin As Double

        Dim a1stOU, a1stOE, aOE, aWR As Double
        Dim aLOU, aLOE, aLHL, aLNC, aLNG As Double



        a1stOUMax2 = CastAide.AsDouble(dr("v161apositiontaking_81_max")) ' a4
        a1stOEMax2 = CastAide.AsDouble(dr("v161apositiontaking_83_max"))
        aOEMax2 = CastAide.AsDouble(dr("v161apositiontaking_86_max"))
        aWRMax2 = CastAide.AsDouble(dr("v161apositiontaking_88_max"))

        aLOUMax2 = CastAide.AsDouble(dr("v161alivepositiontaking_85_max"))
        aLOEMax2 = CastAide.AsDouble(dr("v161alivepositiontaking_86_max"))
        aLHLMax2 = CastAide.AsDouble(dr("v161alivepositiontaking_87_max"))
        aLNCMax2 = CastAide.AsDouble(dr("v161alivepositiontaking_89_max"))
        aLNGMax2 = CastAide.AsDouble(dr("v161alivepositiontaking_90_max"))

        m1stOUMax = CastAide.AsDouble(dr("v161mpositiontaking_81")) ' min(l3,m3)
        m1stOEMax = CastAide.AsDouble(dr("v161mpositiontaking_83"))
        mOEMax = CastAide.AsDouble(dr("v161mpositiontaking_86"))
        mWRMax = CastAide.AsDouble(dr("v161mpositiontaking_88"))

        mLOUMax = CastAide.AsDouble(dr("v161mlivepositiontaking_85"))
        mLOEMax = CastAide.AsDouble(dr("v161mlivepositiontaking_86"))
        mLHLMax = CastAide.AsDouble(dr("v161mlivepositiontaking_87"))
        mLNCMax = CastAide.AsDouble(dr("v161mlivepositiontaking_89"))
        mLNGMax = CastAide.AsDouble(dr("v161mlivepositiontaking_90"))

        m1stOU = m1stOUMax
        m1stOE = m1stOEMax
        mOE = mOEMax
        mWR = mWRMax

        mLOU = mLOUMax
        mLOE = mLOEMax
        mLHL = mLHLMax
        mLNC = mLNCMax
        mLNG = mLNGMax

        ma1stOUMin = CastAide.AsDouble(dr("v161mpositiontaking_81_min"))
        ma1stOEMin = CastAide.AsDouble(dr("v161mpositiontaking_83_min"))
        maOEMin = CastAide.AsDouble(dr("v161mpositiontaking_86_min"))
        maWRMin = CastAide.AsDouble(dr("v161mpositiontaking_88_min"))

        maLOUMin = CastAide.AsDouble(dr("v161mlivepositiontaking_85_min"))
        maLOEMin = CastAide.AsDouble(dr("v161mlivepositiontaking_86_min"))
        maLHLMin = CastAide.AsDouble(dr("v161mlivepositiontaking_87_min"))
        maLNCMin = CastAide.AsDouble(dr("v161mlivepositiontaking_89_min"))
        maLNGMin = CastAide.AsDouble(dr("v161mlivepositiontaking_90_min"))

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        a1stOUMax = IIf(m1stOUMax - m1stOU > a1stOUMax2, a1stOUMax2, m1stOUMax - m1stOU)
        a1stOEMax = IIf(m1stOEMax - m1stOE > a1stOEMax2, a1stOEMax2, m1stOEMax - m1stOE)
        aOEMax = IIf(mOEMax - mOE > aOEMax2, aOEMax2, mOEMax - mOE)
        aWRMax = IIf(mWRMax - mWR > aWRMax2, aWRMax2, mWRMax - mWR)

        aLOUMax = IIf(mLOUMax - mLOU > aLOUMax2, aLOUMax2, mLOUMax - mLOU)
        aLOEMax = IIf(mLOEMax - mLOE > aLOEMax2, aLOEMax2, mLOEMax - mLOE)
        aLHLMax = IIf(mLHLMax - mLHL > aLHLMax2, aLHLMax2, mLHLMax - mLHL)
        aLNCMax = IIf(mLNCMax - mLNC > aLNCMax2, aLNCMax2, mLNCMax - mLNC)
        aLNGMax = IIf(mLNGMax - mLNG > aLNGMax2, aLNGMax2, mLNGMax - mLNG)

        a1stOU = a1stOUMax
        a1stOE = a1stOEMax
        aOE = aOEMax
        aWR = aWRMax

        aLOU = aLOUMax
        aLOE = aLOEMax
        aLHL = aLHLMax
        aLNC = aLNCMax
        aLNG = aLNGMax

        hiddenHash.Add("minM1611stOUD", m1stOUMin)
        hiddenHash.Add("minM1611stOED", m1stOEMin)
        hiddenHash.Add("minM161OED", mOEMin)
        hiddenHash.Add("minM161WRD", mWRMin)

        hiddenHash.Add("minM161OUL", mLOUMin)
        hiddenHash.Add("minM161OEL", mLOEMin)
        hiddenHash.Add("minM161HLL", mLHLMin)
        hiddenHash.Add("minM161NCL", mLNCMin)
        hiddenHash.Add("minM161NGL", mLNGMin)

        hiddenHash.Add("maxA1611stOUD", IIf(m1stOUMax > a1stOUMax2, a1stOUMax2, m1stOUMax))
        hiddenHash.Add("maxA1611stOED", IIf(m1stOEMax > a1stOEMax2, a1stOEMax2, m1stOEMax))
        hiddenHash.Add("maxA161OED", IIf(mOEMax > aOEMax2, aOEMax2, mOEMax))
        hiddenHash.Add("maxA161WRD", IIf(mWRMax > aWRMax2, aWRMax2, mWRMax))

        hiddenHash.Add("maxA161OUL", IIf(mLOUMax > aLOUMax2, aLOUMax2, mLOUMax))
        hiddenHash.Add("maxA161OEL", IIf(mLOEMax > aLOEMax2, aLOEMax2, mLOEMax))
        hiddenHash.Add("maxA161HLL", IIf(mLHLMax > aLHLMax2, aLHLMax2, mLHLMax))
        hiddenHash.Add("maxA161NCL", IIf(mLNCMax > aLNCMax2, aLNCMax2, mLNCMax))
        hiddenHash.Add("maxA161NGL", IIf(mLNGMax > aLNGMax2, aLNGMax2, mLNGMax))

        hiddenHash.Add("maxM1611stOUD", m1stOUMax)
        hiddenHash.Add("maxM1611stOED", m1stOEMax)
        hiddenHash.Add("maxM161OED", mOEMax)
        hiddenHash.Add("maxM161WRD", mWRMax)

        hiddenHash.Add("maxM161OUL", mLOUMax)
        hiddenHash.Add("maxM161OEL", mLOEMax)
        hiddenHash.Add("maxM161HLL", mLHLMax)
        hiddenHash.Add("maxM161NCL", mLNCMax)
        hiddenHash.Add("maxM161NGL", mLNGMax)

        strHidNumberGame = PositionTaking.AssignMulipleHidden(hiddenHash)

        'assign Increase and Reduce button for template
        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM1611stOUD")
        TemplateHandler.Assign(numbergameTpl, "a-1stlastou-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listS1611stOUD")
        TemplateHandler.Assign(numbergameTpl, "p-1stlastou-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM161OUL")
        TemplateHandler.Assign(numbergameTpl, "a-Lou-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listS161OUL")
        TemplateHandler.Assign(numbergameTpl, "p-Lou-btn", btnTpl)

        'get PT deadball value for master preset PT
        Dim pt As String = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1611stOUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(a1stOUMin, a1stOUMax, a1stOU))
        TemplateHandler.Assign(numbergameTpl, "a-1stlastou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1611stOED")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(a1stOEMin, a1stOEMax, a1stOE))
        TemplateHandler.Assign(numbergameTpl, "a-1stlastoe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161OED")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aOEMin, aOEMax, aOE))
        TemplateHandler.Assign(numbergameTpl, "a-oe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161WRD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aWRMin, aWRMax, aWR))
        TemplateHandler.Assign(numbergameTpl, "a-warrior-val", pt)
        TemplateHandler.Assign(numbergameTpl, "checked-w", String.Empty)


        'get PT deadball value for super preset PT
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS1611stOUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1stOUMin, m1stOUMax, m1stOU))
        TemplateHandler.Assign(numbergameTpl, "p-1stlastou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS1611stOED")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1stOEMin, m1stOEMax, m1stOE))
        TemplateHandler.Assign(numbergameTpl, "p-1stlastoe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS161OED")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOE))
        TemplateHandler.Assign(numbergameTpl, "p-oe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS161WRD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mWRMin, mWRMax, mWR))
        TemplateHandler.Assign(numbergameTpl, "p-warrior-val", pt)
        TemplateHandler.Assign(numbergameTpl, "checked-w", String.Empty)


        'get PT value for super preset PT
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161OUL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aLOUMin, aLOUMax, aLOU))
        TemplateHandler.Assign(numbergameTpl, "a-Lou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161OEL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aLOEMin, aLOEMax, aLOE))
        TemplateHandler.Assign(numbergameTpl, "a-Loe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161HLL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aLHLMin, aLHLMax, aLHL))
        TemplateHandler.Assign(numbergameTpl, "a-Lhl-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161NCL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aLNCMin, aLNCMax, aLNC))
        TemplateHandler.Assign(numbergameTpl, "a-nextCombo-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161NGL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aLNGMin, aLNGMax, aLNG))
        TemplateHandler.Assign(numbergameTpl, "a-NumGr-val", pt)

        ''----------------------------------------

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS161OUL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLOUMin, mLOUMax, mLOU))
        TemplateHandler.Assign(numbergameTpl, "p-Lou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS161OEL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLOEMin, mLOEMax, mLOE))
        TemplateHandler.Assign(numbergameTpl, "p-Loe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS161HLL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLHLMin, mLHLMax, mLHL))
        TemplateHandler.Assign(numbergameTpl, "p-Lhl-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS161NCL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLNCMin, mLNCMax, mLNC))
        TemplateHandler.Assign(numbergameTpl, "p-nextCombo-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS161NGL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLNGMin, mLNGMax, mLNG))
        TemplateHandler.Assign(numbergameTpl, "p-NumGr-val", pt)

        '' Get value for Auto Postion taking-------------------------------------------------------------------------------

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS1611stOUD")
        TemplateHandler.Assign(numbergameTpl, "au-1stlastou-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS161OUL")
        TemplateHandler.Assign(numbergameTpl, "au-Lou-btn", btnTpl)

        ''For Deadball

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1611stOUD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, m1stOUMax, 0))
        TemplateHandler.Assign(numbergameTpl, "au-1stlastou-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1611stOED")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, m1stOEMax, 0))
        TemplateHandler.Assign(numbergameTpl, "au-1stlastoe-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161OED")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mOEMax, 0))
        TemplateHandler.Assign(numbergameTpl, "au-oe-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161WRD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mWRMax, 0))
        TemplateHandler.Assign(numbergameTpl, "au-warrior-val", ptAuto)

        ''For live ball
        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161OUL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mLOUMax, 0))
        TemplateHandler.Assign(numbergameTpl, "au-Lou-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161OEL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mLOEMax, 0))
        TemplateHandler.Assign(numbergameTpl, "au-Loe-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161HLL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mLHLMax, 0))
        TemplateHandler.Assign(numbergameTpl, "au-Lhl-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161NCL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mLNCMax, 0))
        TemplateHandler.Assign(numbergameTpl, "au-nextCombo-val", ptAuto)
        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161NGL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mLNGMax, 0))
        TemplateHandler.Assign(numbergameTpl, "au-NumGr-val", ptAuto)
        '-------------------------------------------------------------------------------------------------------------------------


        Return numbergameTpl
    End Function
#End Region

#Region "Assign info for MixSports"
    Function GetDataMixSports(ByVal dr As DataRow) As String
        Dim pt As String
        Dim ptAuto As String = String.Empty
        Dim hiddenHash As New Hashtable
        Dim tpl As String = TemplateHandler.LoadTemplate("MemberInfo/MixSportsPT.html")


        'assign text for position taking
        GetTitleCommon(tpl)
        TemplateHandler.Assign(tpl, "mp-text", Resources.Lang.lblMixParlay)
        TemplateHandler.Assign(tpl, "title-col", "2")
        TemplateHandler.RemoveBlock(tpl, "none-edit")

        'assign value for position taking
        Dim minbetMin As Double
        Dim maxbetMax As Double
        Dim maxpermatchMax As Double
        Dim maxpermatchMin As Double

        minbetMin = DataFormat.FormatNumber1(dr("minbetv99MP_max"))
        maxbetMax = DataFormat.FormatNumber1(dr("maxbetv99MP_min"))
        maxpermatchMax = DataFormat.FormatNumber1(dr("maxpermatchv99MP_max"))
        maxpermatchMin = DataFormat.FormatNumber1(dr("MaxPerMatchv99MP_Min"))

        TemplateHandler.Assign(tpl, "minbet99_M-val", NumberFormat.DoubleFormat(minbetMin, 0))
        TemplateHandler.Assign(tpl, "maxbet99_M-val", NumberFormat.DoubleFormat(maxbetMax, 0))
        TemplateHandler.Assign(tpl, "maxpermatch99_M-val", NumberFormat.DoubleFormat(maxpermatchMax, 0))
        TemplateHandler.Assign(tpl, "minbet-min-99_M", NumberFormat.DoubleFormat(minbetMin, 0))
        TemplateHandler.Assign(tpl, "maxbet-max-99_M", NumberFormat.DoubleFormat(maxbetMax, 0))
        TemplateHandler.Assign(tpl, "maxpermatch-min-99_M", NumberFormat.DoubleFormat(maxpermatchMin, 0))
        TemplateHandler.Assign(tpl, "maxpermatch-max-99_M", NumberFormat.DoubleFormat(maxpermatchMax, 0))

        If minbetMin = 0 Then
            TemplateHandler.Assign(tpl, "mbMin", "hidden")
        Else
            TemplateHandler.Assign(tpl, "mbMin", "minmaxbet")
        End If
        If maxbetMax = 0 Then
            TemplateHandler.Assign(tpl, "mbMax", "hidden")
        Else
            TemplateHandler.Assign(tpl, "mbMax", "minmaxbet")
        End If
        If maxpermatchMin = 0 Then
            TemplateHandler.Assign(tpl, "mpmMin", "hidden")
        Else
            TemplateHandler.Assign(tpl, "mpmMin", "minmaxbet")
        End If
        If maxpermatchMax = 0 Then
            TemplateHandler.Assign(tpl, "mpmMax", "hidden")
        Else
            TemplateHandler.Assign(tpl, "mpmMax", "minmaxbet")
        End If

        Dim mMPMin As Double
        Dim mMPMax As Double
        Dim mMP As Double

        Dim aMPMin As Double
        Dim aMPMax As Double
        Dim aMP As Double
        Dim aMPMax2 As Double
        Dim maMPMin As Double

        aMPMax2 = CastAide.AsDouble(dr("v99apositiontaking_9_max"))
        mMPMax = CastAide.AsDouble(dr("v99mpositiontaking_9"))
        mMP = mMPMax
        maMPMin = CastAide.AsDouble(dr("v99mpositiontaking_9_min"))
        aMPMax = IIf(mMPMax - mMP > aMPMax2, aMPMax2, mMPMax - mMP)
        aMP = aMPMax

        hiddenHash.Add("minM99_M", maMPMin)
        hiddenHash.Add("maxA99_M", IIf(mMPMax > aMPMax2, aMPMax2, mMPMax))
        hiddenHash.Add("maxM99_M", mMPMax)
        strHidMixSports = PositionTaking.AssignMulipleHidden(hiddenHash)

        ''assign data for Position taking dropdowns
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM99_M")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aMPMin, aMPMax, aMPMax))
        TemplateHandler.Assign(tpl, "a-mp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS99_M")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMPMin, mMPMax, mMPMax))
        TemplateHandler.Assign(tpl, "p-mp-val", pt)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS99_M")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mMPMax, 0))
        TemplateHandler.Assign(tpl, "au-mp-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-MP", String.Empty)

        Dim btnTpl As String = groupBtn

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS99_M")
        TemplateHandler.Assign(tpl, "au-mp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM99_M")
        TemplateHandler.Assign(tpl, "a-mp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listS99_M")
        TemplateHandler.Assign(tpl, "p-mp-btn", btnTpl)



        Return tpl
    End Function
#End Region

#Region "Get Title Common"
    Sub GetTitleCommon(ByRef tpl As String)
        'TemplateHandler.RemoveBlock(tpl, "auto-preset")
        TemplateHandler.Assign(tpl, "bet-text", Resources.Lang.lblBetSetting)
        TemplateHandler.Assign(tpl, "minbet-text", Resources.Lang.lblMinBet)
        TemplateHandler.Assign(tpl, "maxbet-text", Resources.Lang.lblMaxBet)
        TemplateHandler.Assign(tpl, "maxpermatch-text", Resources.Lang.lblMaxPerMatch)
        TemplateHandler.Assign(tpl, "pt-text", Resources.Lang.lblPositionTaking)
        TemplateHandler.Assign(tpl, "a-presetpt-text", Resources.Lang.agentposition)
        TemplateHandler.Assign(tpl, "p-presetpt-text", Resources.Lang.lblMasterPresetPosition)
        TemplateHandler.Assign(tpl, "hdp-text", Resources.Lang.lblHandicapFC)
        TemplateHandler.Assign(tpl, "ml-text", Resources.Lang.moneyline)
        TemplateHandler.Assign(tpl, "copy-hdp-text", Resources.Lang.lblCopyFromHandicap)
        TemplateHandler.Assign(tpl, "copy-hdp-col", "2")
        TemplateHandler.Assign(tpl, "au-presetpt-text", Resources.Lang.lblAutoMasterResestPosition)
        TemplateHandler.Assign(tpl, "au-preset-live", Resources.Lang.lblAutoMasterResestPosition)
        TemplateHandler.Assign(tpl, "numberGroup-text", Resources.Lang.numberGroup)


    End Sub
#End Region

#Region "GetListNumber"
    Shared Function GetListNumber(ByVal currentVal As String, Optional ByVal isMember As Boolean = False) As String()
        Dim result() As String = {"", ""}

        Dim htmlNo1 As New StringBuilder
        Dim htmlNo2 As New StringBuilder
        Dim htmlNo3 As New StringBuilder
        ''ascii 48-->57, 65-->90, 97-->122
        Dim no1 As String = currentVal.Substring(0, 1).ToUpper()
        Dim no2 As String = currentVal.Substring(1, 1).ToUpper()
        Dim no3 As String = no2
        Dim strNextNo1 As String = no1
        Dim strNextNo2 As String = ""
        Dim strNextNo3 As String = ""
        If isMember Then
            no3 = currentVal.Substring(2, 1).ToUpper()
            strNextNo2 = no2
            result = New String() {"", "", ""}
        End If
        Dim arr() As String = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", _
                               "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", _
                               "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", _
                               "U", "V", "W", "X", "Y", "Z"}
        Dim index As Integer
        If isMember Then
            If no3 = arr(arr.Length - 1) Then
                strNextNo3 = arr(0)
                If no2 = arr(arr.Length - 1) Then
                    strNextNo2 = arr(0)
                    index = Array.IndexOf(arr, no1)
                    strNextNo1 = arr(index + 1)
                Else
                    index = Array.IndexOf(arr, no2)
                    strNextNo2 = arr(index + 1)
                End If
            Else
                index = Array.IndexOf(arr, no3)
                strNextNo3 = arr(index + 1)
            End If
        Else
            If no2 = arr(arr.Length - 1) Then
                strNextNo2 = arr(0)
                index = Array.IndexOf(arr, no1)
                If index = arr.Length - 1 Then
                    strNextNo1 = arr(0)
                Else
                    strNextNo1 = arr(index + 1)
                End If
            Else
                index = Array.IndexOf(arr, no2)
                If index = arr.Length - 1 Then
                    strNextNo2 = arr(0)
                Else
                    strNextNo2 = arr(index + 1)
                End If


            End If
        End If

        For i As Integer = 0 To arr.Length - 1 Step 1
            If strNextNo2 = arr(i) Then
                htmlNo2.Append("<option selected='true' value='" & arr(i) & "'>" & arr(i) & "</option>")
            Else
                htmlNo2.Append("<option value='" & arr(i) & "'>" & arr(i) & "</option>")
            End If
            If strNextNo1 = arr(i) Then
                htmlNo1.Append("<option selected='true' value='" & arr(i) & "'>" & arr(i) & "</option>")
            Else
                htmlNo1.Append("<option value='" & arr(i) & "'>" & arr(i) & "</option>")
            End If
            If isMember Then
                If strNextNo3 = arr(i) Then
                    htmlNo3.Append("<option selected='true' value='" & arr(i) & "'>" & arr(i) & "</option>")
                Else
                    htmlNo3.Append("<option value='" & arr(i) & "'>" & arr(i) & "</option>")
                End If
            End If
        Next
        result(0) = htmlNo1.ToString()
        result(1) = htmlNo2.ToString()
        If isMember Then
            result(2) = htmlNo3.ToString()
        End If

        GetListNumber = result
    End Function
#End Region

#Region "GetCredit"
    Sub GetCredit(ByVal dr As DataRow, ByRef tbl As String, Optional ByVal isEdit As Boolean = False)
        If isEdit Then
            TemplateHandler.Assign(tbl, "credit-val", DataFormat.FormatNumber1(dr("credit")))
            TemplateHandler.Assign(tbl, "mpmMin", "minmaxbet")
            TemplateHandler.Assign(tbl, "credit-min", DataFormat.FormatNumber1(dr("Credit_Min")))
        Else
            TemplateHandler.Assign(tbl, "credit-val", "0")
            TemplateHandler.Assign(tbl, "mpmMin", "hidden")
        End If
        TemplateHandler.Assign(tbl, "mpmMax", "minmaxbet")
        TemplateHandler.Assign(tbl, "credit-max", DataFormat.FormatNumber1(dr("Credit_Max")))
    End Sub
#End Region

#Region "GetListGroup"
    Shared Function GetListGroup(ByVal maxVal As Double, Optional ByVal currentVal As Double = 0) As String
        Dim html As New StringBuilder

        Dim groupValue As Integer = CInt(maxVal * 10000)
        If currentVal > maxVal Then
            html.Append("<option>" & currentVal & "</option>")
        End If
        While groupValue >= 0
            Dim selValue As Double = CDbl(groupValue) / 10000
            If selValue = currentVal Then
                html.Append("<option selected='true'>" & selValue & "</option>")
            Else
                html.Append("<option>" & selValue & "</option>")
            End If
            groupValue = groupValue - 5
        End While

        GetListGroup = html.ToString()
    End Function
#End Region

#Region "GetListOtherComm"
    Shared Function GetListOtherComm(ByVal maxVal As Double, Optional ByVal currentVal As Double = 0) As String
        Dim html As New StringBuilder

        'Dim groupValue As Integer = CInt(maxVal * 1000)
        Dim groupValue As Integer = CInt(maxVal * 10000)
        If currentVal > maxVal Then
            html.Append("<option>" & currentVal & "</option>")
        End If
        While groupValue >= 0
            Dim selValue As Double = CDbl(groupValue) / 10000
            If selValue = currentVal Then
                html.Append("<option selected='true'>" & selValue & "</option>")
            Else
                html.Append("<option>" & selValue & "</option>")
            End If
            groupValue = groupValue - 5
        End While

        GetListOtherComm = html.ToString()
    End Function
    Shared Function GetListOtherComm(ByVal maxVal As Double, ByVal currentVal As Double, ByVal interval As Integer) As String
        Dim html As New StringBuilder

        'Dim groupValue As Integer = CInt(maxVal * 1000)
        Dim groupValue As Integer = CInt(maxVal * 10000)
        If currentVal > maxVal Then
            html.Append("<option>" & currentVal & "</option>")
        End If
        While groupValue >= 0
            Dim selValue As Double = CDbl(groupValue) / 10000
            If selValue = currentVal Then
                html.Append("<option selected='true'>" & selValue & "</option>")
            Else
                html.Append("<option>" & selValue & "</option>")
            End If
            groupValue = groupValue - interval
        End While

        GetListOtherComm = html.ToString()
    End Function
#End Region

#Region "LoadTranferCondition"
    Shared Function LoadTranferCondition(Optional ByVal isMaster As Boolean = False) As String

        Dim strTransferTimeTpl As String = String.Empty


        strTransferTimeTpl &= Resources.Lang.lblAllowTransfer
        Dim strHtml As String = String.Empty
        Dim strTextRe() As String = {Resources.Lang.sunday, Resources.Lang.monday, Resources.Lang.tuesday, Resources.Lang.wednesday, Resources.Lang.thursday, Resources.Lang.friday, Resources.Lang.saturday}
        Dim rs As DbResult
        Dim hash As New Hashtable
        hash.Add("custid", Sunplus.Agent.Model.Entities.UserSession.Current.CustId)
        hash.Add("transferoption", "")
        rs = AccountInfo.Acc_CustomerSelTransferOption(hash)
        If Not rs Is Nothing Then
            If Not rs.Outputs("transferoption") Is Nothing Then
                Dim arrChar() As Char = CType(rs.Outputs("transferoption"), Char())

                'this case for master level and he can transfer everyday
                If isMaster And arrChar(7).ToString().Equals("0") Then
                    Return ""
                End If
                For i As Integer = 1 To arrChar.Length - 2 Step 1
                    If arrChar(i).ToString().Equals("1") Then
                        strHtml &= " - " & strTextRe(i)
                    End If
                Next
                If arrChar(0).ToString().Equals("1") Then
                    strHtml &= " - " & strTextRe(0)
                End If

                If strHtml.Length > 0 Then
                    strHtml = strHtml.Substring(3)
                End If
                If arrChar(7).ToString().Equals("1") Then
                    strTransferTimeTpl &= " " & strHtml
                Else
                    strTransferTimeTpl &= " " & Resources.Lang.daily
                End If
            End If
        End If

        LoadTranferCondition = strTransferTimeTpl
    End Function
#End Region

#Region "GetTranferTime"
    Shared Function GetTranferTime(ByRef tpl As String, Optional ByVal isEdit As Boolean = False, Optional ByVal isDisplay As Boolean = False)
        If Not isEdit Then
            TemplateHandler.RemoveBlock(tpl, "none-add")
            TemplateHandler.Assign(tpl, "daily-chk", "checked")
            TemplateHandler.Assign(tpl, "weekly-chk", "")
            TemplateHandler.Assign(tpl, "mon-chk", "")
            TemplateHandler.Assign(tpl, "tue-chk", "")
            TemplateHandler.Assign(tpl, "wed-chk", "")
            TemplateHandler.Assign(tpl, "thurs-chk", "")
            TemplateHandler.Assign(tpl, "fri-chk", "")
            TemplateHandler.Assign(tpl, "sat-chk", "")
            TemplateHandler.Assign(tpl, "sun-chk", "")
            TemplateHandler.Assign(tpl, "padd-left", "paddleft200")
        Else
            TemplateHandler.Assign(tpl, "submit-text", Resources.Lang.submit)
            TemplateHandler.Assign(tpl, "padd-left", "left")
        End If
        TemplateHandler.Assign(tpl, "acc-balance-tranfer", Resources.Lang.accbalancetrancondition)
        TemplateHandler.Assign(tpl, "daily", Resources.Lang.daily)
        TemplateHandler.Assign(tpl, "weekly", Resources.Lang.weekly)
        TemplateHandler.Assign(tpl, "mon", Resources.Lang.mon)
        TemplateHandler.Assign(tpl, "tue", Resources.Lang.tue)
        TemplateHandler.Assign(tpl, "wed", Resources.Lang.wed)
        TemplateHandler.Assign(tpl, "thurs", Resources.Lang.thurs)
        TemplateHandler.Assign(tpl, "fri", Resources.Lang.fri)
        TemplateHandler.Assign(tpl, "sat", Resources.Lang.sat)
        TemplateHandler.Assign(tpl, "sun", Resources.Lang.sun)
    End Function
#End Region

#Region "GetTitleInfo"
    Shared Sub GetTitleInfo(ByRef strHtml As String, Optional ByVal isMult As Boolean = False)

        TemplateHandler.Assign(strHtml, "{uname-text}", Resources.Lang.username)
        TemplateHandler.Assign(strHtml, "{pwd-text}", Resources.Lang.password)
        TemplateHandler.Assign(strHtml, "{fname-text}", Resources.Lang.firstname)
        TemplateHandler.Assign(strHtml, "{lname-text}", Resources.Lang.lastname)
        TemplateHandler.Assign(strHtml, "{phone-text}", Resources.Lang.lblPhone)
        TemplateHandler.Assign(strHtml, "{mobile-text}", Resources.Lang.lblMobilePhone)
        TemplateHandler.Assign(strHtml, "{fax-text}", Resources.Lang.lblFax)
        TemplateHandler.Assign(strHtml, "{closed-text}", Resources.Lang.lblStatusClosed)
        Dim str As String = String.Empty
        Dim strSS As String = String.Empty
        If isMult Then
            str = "<input type=""checkbox"" id=""chkUpdComm"" onclick=""AllowUpdate(this,false);"" name=""chkUpdComm""/>"
            strSS = "<input type=""checkbox"" id=""chkUpdStatus"" onclick=""AllowUpdate(this,true);"" name=""chkUpdStatus""/>"
        End If
        TemplateHandler.Assign(strHtml, "{status-text}", strSS & Resources.Lang.status)
        TemplateHandler.Assign(strHtml, "{commission-text}", str & Resources.Lang.commission)
        TemplateHandler.Assign(strHtml, "{groupa-text}", Resources.Lang.lblGroupa)
        TemplateHandler.Assign(strHtml, "{groupb-text}", Resources.Lang.lblGroupb)
        TemplateHandler.Assign(strHtml, "{groupc-text}", Resources.Lang.lblGroupc)
        TemplateHandler.Assign(strHtml, "{groupd-text}", Resources.Lang.lblGroupd)
        TemplateHandler.Assign(strHtml, "{discountcs-text}", Resources.Lang.lblOtherComm)
        TemplateHandler.Assign(strHtml, "{discount1x2-text}", Resources.Lang.lbl1x2Comm)
        TemplateHandler.Assign(strHtml, "{discountnumbergame-text}", Resources.Lang.numbergamecomm)
    End Sub
#End Region

#Region "LoadSoccerPTHeader"
    Shared Function LoadSoccerPTHeader(Optional ByVal isMember As Boolean = False) As String
        Dim strHtml As String = TemplateHandler.LoadTemplate("MemberInfo/SBettypeHeader.html")
        Dim col As Integer = 2
        If (isMember) Then
            col = 1
        End If
        TemplateHandler.Assign(strHtml, "{col}", col)
        TemplateHandler.Assign(strHtml, "{hdp-text}", Resources.Lang.lblHandicapFC & "/" & Resources.Lang.nextGoal)
        TemplateHandler.Assign(strHtml, "{ou-text}", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(strHtml, "{1sthdp-text}", Resources.Lang.lbl1stHdp)
        TemplateHandler.Assign(strHtml, "{1stou-text}", Resources.Lang.lbl1stOU)
        TemplateHandler.Assign(strHtml, "{oe-text}", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(strHtml, "{1x2-text}", Resources.Lang.lbl1x2)
        TemplateHandler.Assign(strHtml, "{cs-text}", Resources.Lang.correctscore)
        TemplateHandler.Assign(strHtml, "{tg-text}", Resources.Lang.totalgoal)
        TemplateHandler.Assign(strHtml, "{mp-text}", Resources.Lang.lblMixParlay)
        TemplateHandler.Assign(strHtml, "{or-text}", Resources.Lang.lblOutRight)
        LoadSoccerPTHeader = strHtml
    End Function
#End Region

#Region "Load Number Game Header"
    Shared Function LoadNumberGameHeader(Optional ByVal isMember As Boolean = False) As String
        Dim strHtml As String = TemplateHandler.LoadTemplate("MemberInfo/HeaderNumberGame.html")
        Dim col As Integer = 2
        If (isMember) Then
            col = 1
        End If
        TemplateHandler.Assign(strHtml, "{col}", col)
        TemplateHandler.Assign(strHtml, "{1stlastou-text}", Resources.Lang.firstlastoverunder)
        TemplateHandler.Assign(strHtml, "{1stlastoe-text}", Resources.Lang.firstlastoddeven)
        TemplateHandler.Assign(strHtml, "{ou-text}", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(strHtml, "{oe-text}", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(strHtml, "{warrior-text}", Resources.Lang.warrior)
        TemplateHandler.Assign(strHtml, "{numberGroup-text}", Resources.Lang.numberGroup)
        Return strHtml
    End Function

#End Region

#Region "GetMinMaxAddValue"
    Shared Sub GetMinMaxAddValue(ByRef tpl As String, ByVal sportTypeId As Integer, ByVal dr As DataRow)
        'TemplateHandler.Assign(tpl, "minbet" & sportTypeId & "-val", strReplace)
        'TemplateHandler.Assign(tpl, "maxbet" & sportTypeId & "-val", strReplace)
        'TemplateHandler.Assign(tpl, "maxpermatch" & sportTypeId & "-val", strReplace)
        Dim minbetMin As Double
        Dim maxbetMax As Double
        Dim maxpermatchMax As Double
        If sportTypeId = 1 Then
            minbetMin = DataFormat.FormatNumber1(dr("minbet_max"))
            maxbetMax = DataFormat.FormatNumber1(dr("maxbet_min"))
            maxpermatchMax = DataFormat.FormatNumber1(dr("maxpermatch_max"))
        Else
            minbetMin = DataFormat.FormatNumber1(dr("minbetv" & sportTypeId & "_max"))
            maxbetMax = DataFormat.FormatNumber1(dr("maxbetv" & sportTypeId & "_min"))
            maxpermatchMax = DataFormat.FormatNumber1(dr("maxpermatchv" & sportTypeId & "_max"))
        End If
        TemplateHandler.Assign(tpl, "minbet" & sportTypeId & "-val", NumberFormat.DoubleFormat(minbetMin, 0))
        TemplateHandler.Assign(tpl, "maxbet" & sportTypeId & "-val", NumberFormat.DoubleFormat(maxbetMax, 0))
        TemplateHandler.Assign(tpl, "maxpermatch" & sportTypeId & "-val", NumberFormat.DoubleFormat(maxpermatchMax, 0))
    End Sub
#End Region

#Region "GetLimitMinMaxBet"
    'this function is used to display or not limit values of Min Bet, Max Bet, Max Per Match 
    'dr: row contains data get from db
    'tpl: template for each sporttype
    'sportTypeId: id of sporttype (same to Sport table in db)
    Shared Sub GetLimitMinMaxBet(ByVal dr As DataRow, ByRef tpl As String, ByVal sportTypeId As String)

        Dim minbetMin As Double
        Dim maxbetMax As Double
        Dim maxpermatchMin As Double
        Dim maxpermatchMax As Double

        If sportTypeId = String.Empty Then 'this case for soccer
            minbetMin = DataFormat.FormatNumber1(dr("MinBet" & sportTypeId & "_Max"))
            maxbetMax = DataFormat.FormatNumber1(dr("MaxBet" & sportTypeId & "_Min"))
            maxpermatchMin = DataFormat.FormatNumber1(dr("MaxPerMatch" & sportTypeId & "_Min"))
            maxpermatchMax = DataFormat.FormatNumber1(dr("MaxPerMatch" & sportTypeId & "_Max"))
            sportTypeId = "1"
        Else
            minbetMin = DataFormat.FormatNumber1(dr("MinBetv" & sportTypeId & "_Max"))
            maxbetMax = DataFormat.FormatNumber1(dr("MaxBetv" & sportTypeId & "_Min"))
            maxpermatchMin = DataFormat.FormatNumber1(dr("MaxPerMatchv" & sportTypeId & "_Min"))
            maxpermatchMax = DataFormat.FormatNumber1(dr("MaxPerMatchv" & sportTypeId & "_Max"))
        End If
        TemplateHandler.Assign(tpl, "minbet-min-" & sportTypeId & "", NumberFormat.DoubleFormat(minbetMin, 0))
        TemplateHandler.Assign(tpl, "maxbet-max-" & sportTypeId & "", NumberFormat.DoubleFormat(maxbetMax, 0))
        TemplateHandler.Assign(tpl, "maxpermatch-min-" & sportTypeId & "", NumberFormat.DoubleFormat(maxpermatchMin, 0))
        TemplateHandler.Assign(tpl, "maxpermatch-max-" & sportTypeId & "", NumberFormat.DoubleFormat(maxpermatchMax, 0))
        If minbetMin = 0 Then
            TemplateHandler.Assign(tpl, "mbMin", "hidden")
        Else
            TemplateHandler.Assign(tpl, "mbMin", "minmaxbet")
        End If
        If maxbetMax = 0 Then
            TemplateHandler.Assign(tpl, "mbMax", "hidden")
        Else
            TemplateHandler.Assign(tpl, "mbMax", "minmaxbet")
        End If
        If maxpermatchMin = 0 Then
            TemplateHandler.Assign(tpl, "mpmMin", "hidden")
        Else
            TemplateHandler.Assign(tpl, "mpmMin", "minmaxbet")
        End If
        If maxpermatchMax = 0 Then
            TemplateHandler.Assign(tpl, "mpmMax", "hidden")
        Else
            TemplateHandler.Assign(tpl, "mpmMax", "minmaxbet")
        End If

    End Sub
#End Region

#Region "GetListPositionTaking"
    'Shared Function GetListPositionTaking(ByVal minVal As Double, ByVal maxVal As Double, Optional ByVal currentVal As Double = 0) As String
    '    Dim html As String = String.Empty
    '    Dim strBody As New StringBuilder
    '    
    '        Dim isInRange As Boolean = False
    '        Dim intToValue As Integer = CInt(maxVal * 100)
    '        Dim intFromValue As Integer = CInt(minVal * 100)
    '        While intToValue >= intFromValue
    '            Dim ptValue As Double = CDbl(intToValue) / 100
    '            If ptValue = currentVal Then
    '                isInRange = True
    '                strBody.Append("<option selected='true' value='" & ptValue & "'>" & ptValue & "</option>")
    '            Else
    '                strBody.Append("<option value='" & ptValue & "'>" & ptValue & "</option>")
    '            End If
    '            intToValue = intToValue - 1
    '        End While
    '        If Not isInRange Then
    '            html &= "<option selected='true' value='" & currentVal & "'>" & currentVal & "</option>"
    '        End If
    '        html &= strBody.ToString()
    '    
    '    GetListPositionTaking = html
    'End Function
#End Region
End Class

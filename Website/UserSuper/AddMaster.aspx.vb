'Imports IBCDB
Imports System.Resources
Imports DNAide.Web
Imports System.Text
Imports DNAide.Core
Imports Fanex.Data.DNAide
Imports System.Data
Imports DNAide.Utils
Imports Sunplus.Agent.Model.Entities.Permission

Public Class AddNewMaster
    Inherits IbcPage2
    Public style As String
    Dim strContent As String
    Dim bodyTpl, tennisTpl, basketballTpl, footballTpl, otherSportsTpl, golfTpl As String
    Dim strHidSoccer, strHidBasket, strHidFootball, strHidTennis, strHidNumberGame As String
    Dim strHidBaseball, strHidGolf, strHidMoto, strHidOther, strHidMixSports, strTranferTime As String
    Dim groupBtn As String
    Dim selectTpl As String
    Dim selDisabledTpl As String

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
        ''TemplateHandler.RemoveComment(strContent)
        htmlAddMasterContent.Text = strContent



    End Sub

#Region "Declare parameter for Script"
    'parameters is used in client script functions
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
        script += " var alertIncorrectCredit='" & Resources.Lang.alertMasterCredit & "';" & vbNewLine
        script += " var alertMaxperMatchNegative ='" & Resources.Lang.lblmaxperballgreaterhanzero & "';" & vbNewLine
        script += " var alertMaxperball='" & Resources.Lang.alertIncorrectMaxPerBall & "';" & vbNewLine
        script += " var alertMaxperballCMax='" & Resources.Lang.lblalertMaxPerBallCompareMax & "';" & vbNewLine
        script += " var alertMaxperballC='" & Resources.Lang.lblalertMaxPerBallCompare & "';" & vbNewLine

        script += " var alertMaxPerMatchLessMaxPerBall='" & Resources.Lang.alertMaxPerMatchLessMaxPerBall & "';" & vbNewLine
        script += " var alertMaxPerBallLessMaxBet='" & Resources.Lang.alertMaxPerBallLessMaxBet & "';" & vbNewLine


        script += " var lblChooseDaily='" & Resources.Lang.choonsedaily & "';" & vbNewLine
        script += " var lblChooseOneDay='" & Resources.Lang.chooseoneday & "';" & vbNewLine
        Dim lblCreditInvalid As String
        lblCreditInvalid = Resources.Lang.lblCreditInvalid.Replace("{0}", Resources.Lang.lblMasterMaxCredit)
        script += " var alertCreditInvalid='" & lblCreditInvalid & "';" & vbNewLine
        Dim strAlertMinPT As String = Resources.Lang.alertLessthanMinPT
        strAlertMinPT = strAlertMinPT.Replace("{1}", Resources.Lang.lblMasterPresetPosition)
        strAlertMinPT = strAlertMinPT.Replace("{2}", Resources.Lang.lblSuperPresetPosition)
        script += " var alertLessthanMinPT='" & strAlertMinPT & "';" & vbNewLine
        script += "</script>"
        Response.Write(script)

    End Sub
#End Region

#Region "Load Information for Add Account"
    Function LoadData() As String
        Dim strAddContent As New StringBuilder

        Dim hash As New Hashtable
        hash.Add("custid", Sunplus.Agent.Model.Entities.UserSession.Current.CustId)

        Dim rs As DbResult = AccountInfo.Acc_CustomerSelSAgentInfo(hash)

        If Not rs Is Nothing Then
            If rs.ResultSet.Tables(0).Rows.Count > 0 Then
                Dim row As DataRow = rs.ResultSet.Tables(0).Rows(0)
                strAddContent.Append(GetDataInfo(row))
                Common.GetSportTitle(bodyTpl, False)
                TemplateHandler.Assign(bodyTpl, "add-text", Resources.Lang.btnAdd)
                TemplateHandler.Assign(bodyTpl, "add-id", "btnAdd")
                TemplateHandler.Assign(bodyTpl, "soccerPT", GetDataSoccer(row))
                TemplateHandler.Assign(bodyTpl, "basketPT", GetBasketball(row, 2, basketballTpl))
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
                    TemplateHandler.Assign(bodyTpl, "mixSportPT", GetDataMixSport(row))
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

                strAddContent.Append(bodyTpl)
            End If
            End If

            LoadData = strAddContent.ToString()
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

        TemplateHandler.RemoveBlock(strAccInfo, "none-member")
        TemplateHandler.RemoveBlock(strAccInfo, "none-member")
        TemplateHandler.RemoveBlock(strAccInfo, "none-member")
        TemplateHandler.RemoveBlock(strAccInfo, "none-edit-member")

        TemplateHandler.RemoveBlock(strAccInfo, "none-super-master")
        TemplateHandler.RemoveBlock(strAccInfo, "none-add")
        TemplateHandler.Assign(strAccInfo, "status-text", "")

        TemplateHandler.Assign(strAccInfo, "add-text", Resources.Lang.btnAdd)
        TemplateHandler.Assign(strAccInfo, "add-id", "btnAdd")


        '--------------------------------------------------------------
        'Terry Nguyen modify: Disable FI,Disable HR, Disable Casino and Suspend
        Dim htbDis As Hashtable = New Hashtable()
        htbDis.Add("Suspend", True)
        htbDis.Add("AllowAutoMPT", True)


        Dim mytemplate As String = TemplateHandler.LoadTemplate("Memberinfo/DisableStatus.html")
        Dim mycolspan As String = "<td colspan='2'><td>"
        TemplateHandler.RemoveBlock(mytemplate, "Captchar")
        TemplateHandler.ReplaceBlock(strAccInfo, "Status-Disable", StatusSetting.StatusDisable(mycolspan, htbDis, mytemplate))
        TemplateHandler.Assign(strAccInfo, "suspend-text", Resources.Lang.suspended)
        TemplateHandler.Assign(strAccInfo, "allowAuto-MasterPT-text", Resources.Lang.allowautopt)

        '--            

        TemplateHandler.Assign(strAccInfo, "hdAllowedCurr-value", CastAide.AsInt16(dr("AllowedCurr")))
        '--------------------------------------------------------------
        TemplateHandler.Assign(strAccInfo, "checked-autoMPT", "checked")

        'Terry Nguyen modify: Disabled FI
        TemplateHandler.RemoveBlock(strAccInfo, "downline")


        If CastAide.AsBoolean(dr("suspended"), False) Then
            TemplateHandler.Assign(strAccInfo, "suspend-check", "checked='checked' disabled='disabled'")
        Else
            TemplateHandler.Assign(strAccInfo, "suspend-check", "")
        End If

        TemplateHandler.Assign(strAccInfo, "IsSuspended", CastAide.AsInt32(dr("suspended"), 0))
        TemplateHandler.Assign(strAccInfo, "cols-untext", "4")
        TemplateHandler.Assign(strAccInfo, "title", Resources.Lang.lblAddNewMaster)
        TemplateHandler.Assign(strAccInfo, "max-credit-text", Resources.Lang.lblMasterMaxCredit)
        TemplateHandler.Assign(strAccInfo, "uname-parent", Sunplus.Agent.Model.Entities.UserSession.Current.UserName)

        If (AppSettings.IsMasterSite) Then
            TemplateHandler.RemoveBlock(strAccInfo, "for-agent-site")
            TemplateHandler.RemoveBlock(strAccInfo, "for-super-site")
            TemplateHandler.Assign(strAccInfo, "member-max-credit", Resources.Lang.lblMemberMaxCredit)
        ElseIf (AppSettings.IsSuperSite) Then
            TemplateHandler.RemoveBlock(strAccInfo, "for-agent-site")
            TemplateHandler.RemoveBlock(strAccInfo, "for-master-site")
        Else
            TemplateHandler.RemoveBlock(strAccInfo, "for-super-site")
            TemplateHandler.RemoveBlock(strAccInfo, "for-master-site")
        End If

        Dim selectNoTpl As String = TemplateHandler.LoadTemplate("MemberInfo/DdlMemPT.html")
        Dim ddlNo As String = selectNoTpl

        Dim number() As String = GetListNumber(dr("NewUserName"))
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

        'display value,limit of credit
        GetCredit(dr, strAccInfo)
        TemplateHandler.Assign(strAccInfo, "check-val", "")
        TemplateHandler.Assign(strAccInfo, "fax-val", "")

        TemplateHandler.Assign(strAccInfo, "groupa-val", PositionTaking.GetListGroup(CastAide.AsDouble(dr("groupa")), CastAide.AsDouble(dr("groupa"))))
        TemplateHandler.Assign(strAccInfo, "groupb-val", PositionTaking.GetListGroup(CastAide.AsDouble(dr("groupb")), CastAide.AsDouble(dr("groupb"))))
        TemplateHandler.Assign(strAccInfo, "groupc-val", PositionTaking.GetListGroup(CastAide.AsDouble(dr("groupc")), CastAide.AsDouble(dr("groupc"))))
        TemplateHandler.Assign(strAccInfo, "groupd-val", PositionTaking.GetListGroup(CastAide.AsDouble(dr("groupd")), CastAide.AsDouble(dr("groupd"))))
        TemplateHandler.Assign(strAccInfo, "discount1x2-val", PositionTaking.GetListGroup(CastAide.AsDouble(dr("discount1x2")), CastAide.AsDouble(dr("discount1x2"))))
        TemplateHandler.Assign(strAccInfo, "discountcs-val", PositionTaking.GetListGroup(CastAide.AsDouble(dr("discountcs")), CastAide.AsDouble(dr("discountcs"))))


        If UserPermission.IsShowNumberGame Or Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission = False Then
            TemplateHandler.Assign(strAccInfo, "disnumbergame-val", PositionTaking.GetListGroup(CastAide.AsDouble(dr("discountnumber")), CastAide.AsDouble(dr("discountnumber"))))
        Else
            TemplateHandler.RemoveBlock(strAccInfo, "none-text-numbergame")
        End If


        Dim strTransferCondition As String = LoadTranferCondition(True)
        If strTransferCondition = "" Then
            TemplateHandler.Assign(strAccInfo, "isTransferOption", "1")
            dirMaster.Value = "1"
            GetTranferTime(strTranferTime)
            TemplateHandler.Assign(strAccInfo, "tranfer-time", strTranferTime)
        Else
            TemplateHandler.Assign(strAccInfo, "isTransferOption", "0")
            dirMaster.Value = "0"
            TemplateHandler.Assign(strAccInfo, "tranfer-time", "")
        End If

        'GetTranferTime(strTranferTime)
        'TemplateHandler.Assign(strAccInfo, "tranfer-time", strTranferTime)
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
        Dim hiddenHash As New Hashtable
        Dim ptAuto As String = String.Empty

        soccerTpl = TemplateHandler.LoadTemplate("MemberInfo/SoccerPT.html")
        headerSTpl = TemplateHandler.LoadTemplate("MemberInfo/SBettypeHeader.html")
        ' assign all text according language
        'TemplateHandler.RemoveBlock(soccerTpl, "auto-preset")
        TemplateHandler.RemoveBlock(soccerTpl, "none-edit")
        GetTitleCommon(soccerTpl)
        TemplateHandler.Assign(soccerTpl, "col-title", "12")
        TemplateHandler.Assign(soccerTpl, "bettype-header", LoadSoccerPTHeader())
        TemplateHandler.Assign(soccerTpl, "deadball-text", Resources.Lang.lblDeadball)
        TemplateHandler.Assign(soccerTpl, "live-text", Resources.Lang.lblLive)
        TemplateHandler.Assign(soccerTpl, "a-preset-live", Resources.Lang.lblMasterPresetPosition)
        TemplateHandler.Assign(soccerTpl, "p-preset-live", Resources.Lang.lblSuperPresetPosition)
        TemplateHandler.Assign(soccerTpl, "dead-row", "5")
        TemplateHandler.Assign(soccerTpl, "live-row", "4")
        TemplateHandler.Assign(soccerTpl, "copy-live-col", "4")
        TemplateHandler.Assign(soccerTpl, "copy-1x2-text", Resources.Lang.lblCopyFrom1x2)
        TemplateHandler.Assign(soccerTpl, "au-preset-live", Resources.Lang.lblAutoSuperPresetPosition)

        TemplateHandler.Assign(soccerTpl, "chkAuto-1", "chkAuto1HdpD")
        TemplateHandler.Assign(soccerTpl, "chkAuto-2", "chkAuto11x2")
        TemplateHandler.Assign(soccerTpl, "chkAuto-3", "chkAuto1HdpL")

        'assign value of Minbet, Maxbet , ...
        GetMinMaxAddValue(soccerTpl, 1, dr)
        GetLimitMinMaxBet(dr, soccerTpl, "")

        'assign value position taking for template
        Dim mHdpMin, mOUMin, m1stHdpMin, m1stOUMin, mOEMin, m1x2Min, mCSMin, mTGMin, mMPMin, mORMin As Double
        Dim mLHdpMin, mLOUMin, mL1stHdpMin, mL1stOUMin As Double

        Dim sHdpMin, sOUMin, s1stHdpMin, s1stOUMin, sOEMin, s1x2Min, sCSMin, sTGMin, sMPMin, sORMin As Double
        Dim sLHdpMin, sLOUMin, sL1stHdpMin, sL1stOUMin As Double

        Dim mHdpMax, mOUMax, m1stHdpMax, m1stOUMax, mOEMax, m1x2Max, mCSMax, mTGMax, mMPMax, mORMax As Double
        Dim mLHdpMax, mLOUMax, mL1stHdpMax, mL1stOUMax As Double

        Dim mHdpMax2, mOUMax2, m1stHdpMax2, m1stOUMax2, mOEMax2, m1x2Max2, mCSMax2, mTGMax2, mMPMax2, mORMax2 As Double
        Dim mLHdpMax2, mLOUMax2, mL1stHdpMax2, mL1stOUMax2 As Double

        Dim sHdpMax, sOUMax, s1stHdpMax, s1stOUMax, sOEMax, s1x2Max, sCSMax, sTGMax, sMPMax, sORMax As Double
        Dim sLHdpMax, sLOUMax, sL1stHdpMax, sL1stOUMax As Double

        Dim msHdpMax, msOUMax, ms1stHdpMax, ms1stOUMax, msOEMax, ms1x2Max, msCSMax, msTGMax, msMPMax, msORMax As Double
        Dim msLHdpMax, msLOUMax, msL1stHdpMax, msL1stOUMax As Double

        Dim msHdpMin, msOUMin, ms1stHdpMin, ms1stOUMin, msOEMin, ms1x2Min, msCSMin, msTGMin, msMPMin, msORMin As Double
        Dim msLHdpMin, msLOUMin, msL1stHdpMin, msL1stOUMin As Double

        Dim sAutoHdp, sAutoOU, sAuto1stHdp, sAuto1stOU, sAutoOE, sAuto1x2, sAutoCS, sAutoTG, sAutoMP, sAutoOR As Double
        Dim sAutoLHdp, sAutoLOU, sAutoL1stHdp, sAutoL1stOU As Double

        mHdpMax2 = CastAide.AsDouble(dr("mpositiontaking_1_max"))
        mOUMax2 = CastAide.AsDouble(dr("mpositiontaking_3_max"))
        m1stHdpMax2 = CastAide.AsDouble(dr("mpositiontaking_7_max"))
        m1stOUMax2 = CastAide.AsDouble(dr("mpositiontaking_8_max"))
        mOEMax2 = CastAide.AsDouble(dr("mpositiontaking_2_max"))
        m1x2Max2 = CastAide.AsDouble(dr("mpositiontaking_5_max"))
        mCSMax2 = CastAide.AsDouble(dr("mpositiontaking_4_max"))
        mTGMax2 = CastAide.AsDouble(dr("mpositiontaking_6_max"))
        mMPMax2 = CastAide.AsDouble(dr("mpositiontaking_9_max"))
        mORMax2 = CastAide.AsDouble(dr("mpositiontaking_10_max"))
        mLHdpMax2 = CastAide.AsDouble(dr("mlivepositiontaking_1_max"))
        mLOUMax2 = CastAide.AsDouble(dr("mlivepositiontaking_3_max"))
        mL1stHdpMax2 = CastAide.AsDouble(dr("mlivepositiontaking_7_max"))
        mL1stOUMax2 = CastAide.AsDouble(dr("mlivepositiontaking_8_max"))

        sHdpMax = CastAide.AsDouble(dr("spositiontaking_1"))
        sOUMax = CastAide.AsDouble(dr("spositiontaking_3"))
        s1stHdpMax = CastAide.AsDouble(dr("spositiontaking_7"))
        s1stOUMax = CastAide.AsDouble(dr("spositiontaking_8"))
        sOEMax = CastAide.AsDouble(dr("spositiontaking_2"))
        s1x2Max = CastAide.AsDouble(dr("spositiontaking_5"))
        sCSMax = CastAide.AsDouble(dr("spositiontaking_4"))
        sTGMax = CastAide.AsDouble(dr("spositiontaking_6"))
        sMPMax = CastAide.AsDouble(dr("spositiontaking_9"))
        sORMax = CastAide.AsDouble(dr("spositiontaking_10"))
        sLHdpMax = CastAide.AsDouble(dr("slivepositiontaking_1"))
        sLOUMax = CastAide.AsDouble(dr("slivepositiontaking_3"))
        sL1stHdpMax = CastAide.AsDouble(dr("slivepositiontaking_7"))
        sL1stOUMax = CastAide.AsDouble(dr("slivepositiontaking_8"))

        'this is used to save into hidden fields to check in client script '''''''
        msHdpMax = CastAide.AsDouble(dr("mspositiontaking_1_max"))
        msOUMax = CastAide.AsDouble(dr("mspositiontaking_3_max"))
        ms1stHdpMax = CastAide.AsDouble(dr("mspositiontaking_7_max"))
        ms1stOUMax = CastAide.AsDouble(dr("mspositiontaking_8_max"))
        msOEMax = CastAide.AsDouble(dr("mspositiontaking_2_max"))
        ms1x2Max = CastAide.AsDouble(dr("mspositiontaking_5_max"))
        msCSMax = CastAide.AsDouble(dr("mspositiontaking_4_max"))
        msTGMax = CastAide.AsDouble(dr("mspositiontaking_6_max"))
        msMPMax = CastAide.AsDouble(dr("mspositiontaking_9_max"))
        msORMax = CastAide.AsDouble(dr("mspositiontaking_10_max"))
        msLHdpMax = CastAide.AsDouble(dr("mslivepositiontaking_1_max"))
        msLOUMax = CastAide.AsDouble(dr("mslivepositiontaking_3_max"))
        msL1stHdpMax = CastAide.AsDouble(dr("mslivepositiontaking_7_max"))
        msL1stOUMax = CastAide.AsDouble(dr("mslivepositiontaking_8_max"))

        msHdpMin = CastAide.AsDouble(dr("mspositiontaking_1_min"))
        msOUMin = CastAide.AsDouble(dr("mspositiontaking_3_min"))
        ms1stHdpMin = CastAide.AsDouble(dr("mspositiontaking_7_min"))
        ms1stOUMin = CastAide.AsDouble(dr("mspositiontaking_8_min"))
        msOEMin = CastAide.AsDouble(dr("mspositiontaking_2_min"))
        ms1x2Min = CastAide.AsDouble(dr("mspositiontaking_5_min"))
        msCSMin = CastAide.AsDouble(dr("mspositiontaking_4_min"))
        msTGMin = CastAide.AsDouble(dr("mspositiontaking_6_min"))
        msMPMin = CastAide.AsDouble(dr("mspositiontaking_9_min"))
        msORMin = CastAide.AsDouble(dr("mspositiontaking_10_min"))
        msLHdpMin = CastAide.AsDouble(dr("mslivepositiontaking_1_min"))
        msLOUMin = CastAide.AsDouble(dr("mslivepositiontaking_3_min"))
        msL1stHdpMin = CastAide.AsDouble(dr("mslivepositiontaking_7_min"))
        msL1stOUMin = CastAide.AsDouble(dr("mslivepositiontaking_8_min"))

        sAutoHdp = CastAide.AsDouble(dr("auto_spositiontaking_1"))
        sAutoOU = CastAide.AsDouble(dr("auto_spositiontaking_3"))
        sAuto1stHdp = CastAide.AsDouble(dr("auto_spositiontaking_7"))
        sAuto1stOU = CastAide.AsDouble(dr("auto_spositiontaking_8"))
        sAutoOE = CastAide.AsDouble(dr("auto_spositiontaking_2"))
        sAuto1x2 = CastAide.AsDouble(dr("auto_spositiontaking_5"))
        sAutoCS = CastAide.AsDouble(dr("auto_spositiontaking_4"))
        sAutoTG = CastAide.AsDouble(dr("auto_spositiontaking_6"))
        sAutoMP = CastAide.AsDouble(dr("auto_spositiontaking_9"))
        sAutoOR = CastAide.AsDouble(dr("auto_spositiontaking_10"))
        sAutoLHdp = CastAide.AsDouble(dr("auto_slivepositiontaking_1"))
        sAutoLOU = CastAide.AsDouble(dr("auto_slivepositiontaking_3"))
        sAutoL1stHdp = CastAide.AsDouble(dr("auto_slivepositiontaking_7"))
        sAutoL1stOU = CastAide.AsDouble(dr("auto_slivepositiontaking_8"))
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        mHdpMax = msHdpMax - sHdpMax
        mOUMax = msOUMax - sOUMax
        m1stHdpMax = ms1stHdpMax - s1stHdpMax
        m1stOUMax = ms1stOUMax - s1stOUMax
        mOEMax = msOEMax - sOEMax
        m1x2Max = ms1x2Max - s1x2Max
        mCSMax = msCSMax - sCSMax
        mTGMax = msTGMax - sTGMax
        mMPMax = msMPMax - sMPMax
        mORMax = msORMax - sORMax
        mLHdpMax = msLHdpMax - sLHdpMax
        mLOUMax = msLOUMax - sLOUMax
        mL1stHdpMax = msL1stHdpMax - sL1stHdpMax
        mL1stOUMax = msL1stOUMax - sL1stOUMax

        hiddenHash.Add("maxMS1HdpD", msHdpMax)
        hiddenHash.Add("maxMS1OUD", msOUMax)
        hiddenHash.Add("maxMS11stHdpD", ms1stHdpMax)
        hiddenHash.Add("maxMS11stOUD", ms1stOUMax)
        hiddenHash.Add("maxMS1OE", msOEMax)

        hiddenHash.Add("maxMS11x2", ms1x2Max)
        hiddenHash.Add("maxMS1CS", msCSMax)
        hiddenHash.Add("maxMS1TG", msTGMax)
        hiddenHash.Add("maxMS1MP", msMPMax)
        hiddenHash.Add("maxMS1OR", msORMax)

        hiddenHash.Add("maxMS1HdpL", msLHdpMax)
        hiddenHash.Add("maxMS1OUL", msLOUMax)
        hiddenHash.Add("maxMS11stHdpL", msL1stHdpMax)
        hiddenHash.Add("maxMS11stOUL", msL1stOUMax)

        hiddenHash.Add("minMS1HdpD", msHdpMin)
        hiddenHash.Add("minMS1OUD", msOUMin)
        hiddenHash.Add("minMS11stHdpD", ms1stHdpMin)
        hiddenHash.Add("minMS11stOUD", ms1stOUMin)
        hiddenHash.Add("minMS1OE", msOEMin)

        hiddenHash.Add("minMS11x2", ms1x2Min)
        hiddenHash.Add("minMS1CS", msCSMin)
        hiddenHash.Add("minMS1TG", msTGMin)
        hiddenHash.Add("minMS1MP", msMPMin)
        hiddenHash.Add("minMS1OR", msORMin)

        hiddenHash.Add("minMS1HdpL", msLHdpMin)
        hiddenHash.Add("minMS1OUL", msLOUMin)
        hiddenHash.Add("minMS11stHdpL", msL1stHdpMin)
        hiddenHash.Add("minMS11stOUL", msL1stOUMin)

        hiddenHash.Add("maxM1HdpD", mHdpMax2)
        hiddenHash.Add("maxM1OUD", mOUMax2)
        hiddenHash.Add("maxM11stHdpD", m1stHdpMax2)
        hiddenHash.Add("maxM11stOUD", m1stOUMax2)
        hiddenHash.Add("maxM1OE", mOEMax2)

        hiddenHash.Add("maxM11x2", m1x2Max2)
        hiddenHash.Add("maxM1CS", mCSMax2)
        hiddenHash.Add("maxM1TG", mTGMax2)
        hiddenHash.Add("maxM1MP", mMPMax2)
        hiddenHash.Add("maxM1OR", mORMax2)

        hiddenHash.Add("maxM1HdpL", mLHdpMax2)
        hiddenHash.Add("maxM1OUL", mLOUMax2)
        hiddenHash.Add("maxM11stHdpL", mL1stHdpMax2)
        hiddenHash.Add("maxM11stOUL", mL1stOUMax2)

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
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdpMax))
        TemplateHandler.Assign(soccerTpl, "a-hdp-val", pt)
        TemplateHandler.Assign(soccerTpl, "checked-Hdp", String.Empty)


        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1OUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOUMax))
        TemplateHandler.Assign(soccerTpl, "a-ou-val", pt)
        TemplateHandler.Assign(soccerTpl, "checked-OU", String.Empty)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM11stHdpD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1stHdpMin, m1stHdpMax, m1stHdpMax))
        TemplateHandler.Assign(soccerTpl, "a-1sthdp-val", pt)
        TemplateHandler.Assign(soccerTpl, "checked-1stHdp", String.Empty)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM11stOUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1stOUMin, m1stOUMax, m1stOUMax))
        TemplateHandler.Assign(soccerTpl, "a-1stou-val", pt)
        TemplateHandler.Assign(soccerTpl, "checked-1stOU", String.Empty)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1OE")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOEMax))
        TemplateHandler.Assign(soccerTpl, "a-oe-val", pt)
        TemplateHandler.Assign(soccerTpl, "checked-OE", String.Empty)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM11x2")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1x2Min, m1x2Max, m1x2Max))
        TemplateHandler.Assign(soccerTpl, "a-1x2-val", pt)
        TemplateHandler.Assign(soccerTpl, "checked-1x2", String.Empty)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1CS")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mCSMin, mCSMax, mCSMax))
        TemplateHandler.Assign(soccerTpl, "a-cs-val", pt)
        TemplateHandler.Assign(soccerTpl, "checked-CS", String.Empty)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1TG")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mTGMin, mTGMax, mTGMax))
        TemplateHandler.Assign(soccerTpl, "a-tg-val", pt)
        TemplateHandler.Assign(soccerTpl, "checked-TG", String.Empty)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1MP")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMPMin, mMPMax, mMPMax))
        TemplateHandler.Assign(soccerTpl, "a-mp-val", pt)
        TemplateHandler.Assign(soccerTpl, "checked-MP", String.Empty)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1OR")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mORMin, mORMax, mORMax))
        TemplateHandler.Assign(soccerTpl, "a-or-val", pt)
        TemplateHandler.Assign(soccerTpl, "checked-OR", String.Empty)

        'get PT deadball value for super preset PT
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS1HdpD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sHdpMin, sHdpMax, sHdpMax))
        TemplateHandler.Assign(soccerTpl, "p-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS1OUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOUMin, sOUMax, sOUMax))
        TemplateHandler.Assign(soccerTpl, "p-ou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS11stHdpD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(s1stHdpMin, s1stHdpMax, s1stHdpMax))
        TemplateHandler.Assign(soccerTpl, "p-1sthdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS11stOUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(s1stOUMin, s1stOUMax, s1stOUMax))
        TemplateHandler.Assign(soccerTpl, "p-1stou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS1OE")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOEMin, sOEMax, sOEMax))
        TemplateHandler.Assign(soccerTpl, "p-oe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS11x2")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(s1x2Min, s1x2Max, s1x2Max))
        TemplateHandler.Assign(soccerTpl, "p-1x2-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS1CS")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sCSMin, sCSMax, sCSMax))
        TemplateHandler.Assign(soccerTpl, "p-cs-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS1TG")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sTGMin, sTGMax, sTGMax))
        TemplateHandler.Assign(soccerTpl, "p-tg-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS1MP")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sMPMin, sMPMax, sMPMax))
        TemplateHandler.Assign(soccerTpl, "p-mp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS1OR")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sORMin, sORMax, sORMax))
        TemplateHandler.Assign(soccerTpl, "p-or-val", pt)

        'get PT value for super preset PT


        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1HdpL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLHdpMin, mLHdpMax, mLHdpMax))
        TemplateHandler.Assign(soccerTpl, "a-Lhdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1OUL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLOUMin, mLOUMax, mLOUMax))
        TemplateHandler.Assign(soccerTpl, "a-Lou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM11stHdpL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mL1stHdpMin, mL1stHdpMax, mL1stHdpMax))
        TemplateHandler.Assign(soccerTpl, "a-L1stHdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM11stOUL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mL1stOUMin, mL1stOUMax, mL1stOUMax))
        TemplateHandler.Assign(soccerTpl, "a-L1stou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS1HdpL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sLHdpMin, sLHdpMax, sLHdpMax))
        TemplateHandler.Assign(soccerTpl, "p-Lhdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS1OUL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sLOUMin, sLOUMax, sLOUMax))
        TemplateHandler.Assign(soccerTpl, "p-Lou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS11stHdpL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sL1stHdpMin, sL1stHdpMax, sL1stHdpMax))
        TemplateHandler.Assign(soccerTpl, "p-L1stHdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS11stOUL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sL1stOUMin, sL1stOUMax, sL1stOUMax))
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
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoHdp, 0))
        TemplateHandler.Assign(soccerTpl, "au-hdp-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OUD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOU, 0))
        TemplateHandler.Assign(soccerTpl, "au-ou-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stHdpD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAuto1stHdp, 0))
        TemplateHandler.Assign(soccerTpl, "au-1sthdp-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stOUD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAuto1stOU, 0))
        TemplateHandler.Assign(soccerTpl, "au-1stou-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OE")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOE, 0))
        TemplateHandler.Assign(soccerTpl, "au-oe-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11x2")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAuto1x2, 0))
        TemplateHandler.Assign(soccerTpl, "au-1x2-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1CS")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoCS, 0))
        TemplateHandler.Assign(soccerTpl, "au-cs-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1TG")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoTG, 0))
        TemplateHandler.Assign(soccerTpl, "au-tg-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1MP")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoMP, 0))
        TemplateHandler.Assign(soccerTpl, "au-mp-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OR")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOR, 0))
        TemplateHandler.Assign(soccerTpl, "au-or-val", ptAuto)




        'For live ball
        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1HdpL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoLHdp, 0))
        TemplateHandler.Assign(soccerTpl, "au-Lhdp-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OUL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoLOU, 0))
        TemplateHandler.Assign(soccerTpl, "au-Lou-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stHdpL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoL1stHdp, 0))
        TemplateHandler.Assign(soccerTpl, "au-L1stHdp-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stOUL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoL1stOU, 0))
        TemplateHandler.Assign(soccerTpl, "au-L1stou-val", ptAuto)

        '-------------------------------------------------------------------------------------------------------------------------


        GetDataSoccer = soccerTpl
    End Function
#End Region

#Region "Assign Info for Mix Sports"
    Function GetDataMixSport(ByVal dr As DataRow) As String
        Dim mixsportsTpl As String = String.Empty
        Dim hiddenHash As New Hashtable
        Dim pt As String = String.Empty
        Dim ptAuto As String = String.Empty

        mixsportsTpl = TemplateHandler.LoadTemplate("MemberInfo/MixSportsPT.html")
        GetTitleCommon(mixsportsTpl)
        TemplateHandler.Assign(mixsportsTpl, "mp-text", Resources.Lang.lblMixParlay)
        TemplateHandler.Assign(mixsportsTpl, "title-col", "6")
        TemplateHandler.RemoveBlock(mixsportsTpl, "none-edit")


        'assign value for position taking
        Dim minbetMin As Double
        Dim maxbetMax As Double
        Dim maxpermatchMax As Double
        Dim maxpermatchMin As Double

        minbetMin = DataFormat.FormatNumber1(dr("minbetv99MP_Max"))
        maxbetMax = DataFormat.FormatNumber1(dr("maxbetv99MP_Min"))
        maxpermatchMax = DataFormat.FormatNumber1(dr("MaxPerMatchv99MP_Max"))
        maxpermatchMin = DataFormat.FormatNumber1(dr("MaxPerMatchv99MP_Min"))

        TemplateHandler.Assign(mixsportsTpl, "minbet99_M-val", NumberFormat.DoubleFormat(minbetMin, 0))
        TemplateHandler.Assign(mixsportsTpl, "maxbet99_M-val", NumberFormat.DoubleFormat(maxbetMax, 0))
        TemplateHandler.Assign(mixsportsTpl, "maxpermatch99_M-val", NumberFormat.DoubleFormat(maxpermatchMax, 0))


        TemplateHandler.Assign(mixsportsTpl, "minbet-min-99_M", NumberFormat.DoubleFormat(minbetMin, 0))
        TemplateHandler.Assign(mixsportsTpl, "maxbet-max-99_M", NumberFormat.DoubleFormat(maxbetMax, 0))
        TemplateHandler.Assign(mixsportsTpl, "maxpermatch-min-99_M", NumberFormat.DoubleFormat(maxpermatchMin, 0))
        TemplateHandler.Assign(mixsportsTpl, "maxpermatch-max-99_M", NumberFormat.DoubleFormat(maxpermatchMax, 0))
        If minbetMin = 0 Then
            TemplateHandler.Assign(mixsportsTpl, "mbMin", "hidden")
        Else
            TemplateHandler.Assign(mixsportsTpl, "mbMin", "minmaxbet")
        End If
        If maxbetMax = 0 Then
            TemplateHandler.Assign(mixsportsTpl, "mbMax", "hidden")
        Else
            TemplateHandler.Assign(mixsportsTpl, "mbMax", "minmaxbet")
        End If
        If maxpermatchMin = 0 Then
            TemplateHandler.Assign(mixsportsTpl, "mpmMin", "hidden")
        Else
            TemplateHandler.Assign(mixsportsTpl, "mpmMin", "minmaxbet")
        End If
        If maxpermatchMax = 0 Then
            TemplateHandler.Assign(mixsportsTpl, "mpmMax", "hidden")
        Else
            TemplateHandler.Assign(mixsportsTpl, "mpmMax", "minmaxbet")
        End If

        Dim mMp_MMin As Double
        Dim sMp_MMin As Double

        Dim mMp_MMax As Double
        Dim mMp_MMax2 As Double
        Dim sMp_MMax As Double

        Dim msMp_MMax As Double
        Dim msMp_MMin As Double
        Dim sAutoMp_M As Double

        mMp_MMax2 = CastAide.AsDouble(dr("v99mpositiontaking_9_max"))
        sMp_MMax = CastAide.AsDouble(dr("v99spositiontaking_9"))
        msMp_MMax = CastAide.AsDouble(dr("v99mspositiontaking_9_max"))
        msMp_MMin = CastAide.AsDouble(dr("v99mspositiontaking_9_min"))
        sAutoMp_M = CastAide.AsDouble(dr("auto_v99spositiontaking_9"))

        mMp_MMax = msMp_MMax - sMp_MMax

        hiddenHash.Add("maxMS99_M", msMp_MMax)
        hiddenHash.Add("minMS99_M", msMp_MMin)
        hiddenHash.Add("maxM99_M", mMp_MMax2)

        strHidMixSports = PositionTaking.AssignMulipleHidden(hiddenHash)

        ''assign data for Position taking dropdowns
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM99_M")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMp_MMin, mMp_MMax, mMp_MMax))
        TemplateHandler.Assign(mixsportsTpl, "a-mp-val", pt)


        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS99_M")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sMp_MMin, sMp_MMax, sMp_MMax))
        TemplateHandler.Assign(mixsportsTpl, "p-mp-val", pt)

        ''----------------------------------------------------------------------------------------------------------------


        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS99_M")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoMp_M, 0))
        TemplateHandler.Assign(mixsportsTpl, "au-mp-val", ptAuto)
        TemplateHandler.Assign(mixsportsTpl, "checked-mp", String.Empty)

        Dim btnTpl As String = groupBtn
        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS99_M")
        TemplateHandler.Assign(mixsportsTpl, "au-mp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM99_M")
        TemplateHandler.Assign(mixsportsTpl, "a-mp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listS99_M")
        TemplateHandler.Assign(mixsportsTpl, "p-mp-btn", btnTpl)

        Return mixsportsTpl
    End Function
#End Region


#Region "Assign Info for Number Game"
    Function GetDataNumberGame(ByVal dr As DataRow) As String
        Dim numbergameTpl As String = String.Empty
        Dim headerSTpl As String = String.Empty
        Dim hiddenHash As New Hashtable
        Dim ptAuto As String = String.Empty


        numbergameTpl = TemplateHandler.LoadTemplate("MemberInfo/NumberGamePT.html")


        


        If UserPermission.IsShowNumberGame Then

            ' Max per ball
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
            '---
        Else
            TemplateHandler.RemoveBlock(numbergameTpl, "none-maxperball")
        End If


        TemplateHandler.RemoveBlock(numbergameTpl, "none-edit")
        GetTitleCommon(numbergameTpl)
        TemplateHandler.Assign(numbergameTpl, "col-title", "12")
        TemplateHandler.Assign(numbergameTpl, "bettype-header", LoadNumberGameHeader())

        TemplateHandler.Assign(numbergameTpl, "overunder-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(numbergameTpl, "oddeven-text", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(numbergameTpl, "highlow-text", Resources.Lang.highlow)

        TemplateHandler.Assign(numbergameTpl, "deadball-text", Resources.Lang.lblDeadball)
        TemplateHandler.Assign(numbergameTpl, "live-text", Resources.Lang.lblLive)
        TemplateHandler.Assign(numbergameTpl, "a-preset-live", Resources.Lang.lblMasterPresetPosition)
        TemplateHandler.Assign(numbergameTpl, "p-preset-live", Resources.Lang.lblSuperPresetPosition)
        TemplateHandler.Assign(numbergameTpl, "dead-row", "4")
        TemplateHandler.Assign(numbergameTpl, "live-row", "4")
        TemplateHandler.Assign(numbergameTpl, "copy-1stlastou-text", Resources.Lang.lbl1stcopyfromoverunder)
        TemplateHandler.Assign(numbergameTpl, "copy-oulive-text", Resources.Lang.lblcopyfromoverunder)
        TemplateHandler.Assign(numbergameTpl, "au-preset-live", Resources.Lang.lblAutoSuperPresetPosition)
        TemplateHandler.Assign(numbergameTpl, "next-combo-text", Resources.Lang.nextcombo)
        TemplateHandler.Assign(numbergameTpl, "{numberGroup-text}", Resources.Lang.numberGroup)
        GetMinMaxAddValue(numbergameTpl, 161, dr)
        GetLimitMinMaxBet(dr, numbergameTpl, "161")

        If (Not UserPermission.IsShowNumberGroup) Then
            TemplateHandler.Assign(numbergameTpl, "hide", "hide")
        End If

        'assign value position taking for template
        Dim m1stOUMin, m1stOEMin, mOEMin, mWRMin As Double
        Dim mLOUMin, mLOEMin, mLHLMin, mLNCMin, mLNGMin As Double

        Dim s1stOUMin, s1stOEMin, sOEMin, sWRMin As Double
        Dim sLOUMin, sLOEMin, sLHLMin, sLNCMin, sLNGMin As Double

        Dim m1stOUMax, m1stOEMax, mOEMax, mWRMax As Double
        Dim mLOUMax, mLOEMax, mLHLMax, mLNCMax, mLNGMax As Double

        Dim m1stOUMax2, m1stOEMax2, mOEMax2, mWRMax2 As Double
        Dim mLOUMax2, mLOEMax2, mLHLMax2, mLNCMax2, mLNGMax2 As Double

        Dim s1stOUMax, s1stOEMax, sOEMax, sWRMax As Double
        Dim sLOUMax, sLOEMax, sLHLMax, sLNCMax, sLNGMax As Double

        Dim ms1stOUMax, ms1stOEMax, msOEMax, msWRmax As Double
        Dim msLOUMax, msLOEMax, msLHLMax, msLNCMax, msLNGMax As Double

        Dim ms1stOUMin, ms1stOEMin, msOEMin, msWRMin As Double
        Dim msLOUMin, msLOEMin, msLHLMin, msLNCMin, msLNGMin As Double

        Dim sAuto1stOU, sAuto1stOE, sAutoOE, sAutoWR As Double
        Dim sAutoLOU, sAutoLOE, sAutoLHL, sAutoLNC, sAutoLNG As Double


        m1stOUMax2 = CastAide.AsDouble(dr("v161mpositiontaking_81_max"))
        m1stOEMax2 = CastAide.AsDouble(dr("v161mpositiontaking_83_max"))
        'mOUMax2 = CastAide.AsDouble(dr("v161mpositiontaking_85_max"))
        mOEMax2 = CastAide.AsDouble(dr("v161mpositiontaking_86_max"))
        MWRMax2 = CastAide.AsDouble(dr("v161mpositiontaking_88_max"))

        mLOUMax2 = CastAide.AsDouble(dr("v161mlivepositiontaking_85_max"))
        mLOEMax2 = CastAide.AsDouble(dr("v161mlivepositiontaking_86_max"))
        mLHLMax2 = CastAide.AsDouble(dr("v161mlivepositiontaking_87_max"))
        mLNCMax2 = CastAide.AsDouble(dr("v161mlivepositiontaking_89_max"))
        mLNGMax2 = CastAide.AsDouble(dr("v161mlivepositiontaking_90_max"))

        s1stOUMax = CastAide.AsDouble(dr("v161spositiontaking_81"))
        s1stOEMax = CastAide.AsDouble(dr("v161spositiontaking_83"))
        'sOUMax = CastAide.AsDouble(dr("v161spositiontaking_85"))
        sOEMax = CastAide.AsDouble(dr("v161spositiontaking_86"))
        sWRMax = CastAide.AsDouble(dr("v161spositiontaking_88"))


        sLOUMax = CastAide.AsDouble(dr("v161slivepositiontaking_85"))
        sLOEMax = CastAide.AsDouble(dr("v161slivepositiontaking_86"))
        sLHLMax = CastAide.AsDouble(dr("v161slivepositiontaking_87"))
        sLNCMax = CastAide.AsDouble(dr("v161slivepositiontaking_89"))
        sLNGMax = CastAide.AsDouble(dr("v161slivepositiontaking_90"))

        ms1stOUMax = CastAide.AsDouble(dr("v161mspositiontaking_81_max"))
        ms1stOEMax = CastAide.AsDouble(dr("v161mspositiontaking_83_max"))
        'msOUMax = CastAide.AsDouble(dr("v161mspositiontaking_85_max"))
        msOEMax = CastAide.AsDouble(dr("v161mspositiontaking_86_max"))
        msWRmax = CastAide.AsDouble(dr("v161mspositiontaking_88_max"))


        msLOUMax = CastAide.AsDouble(dr("v161mslivepositiontaking_85_max"))
        msLOEMax = CastAide.AsDouble(dr("v161mslivepositiontaking_86_max"))
        msLHLMax = CastAide.AsDouble(dr("v161mslivepositiontaking_87_max"))
        msLNCMax = CastAide.AsDouble(dr("v161mslivepositiontaking_89_max"))
        msLNGMax = CastAide.AsDouble(dr("v161mslivepositiontaking_90_max"))

        ms1stOUMin = CastAide.AsDouble(dr("v161mspositiontaking_81_min"))
        ms1stOEMin = CastAide.AsDouble(dr("v161mspositiontaking_83_min"))
        'msOUMin = CastAide.AsDouble(dr("v161mspositiontaking_85_min"))
        msOEMin = CastAide.AsDouble(dr("v161mspositiontaking_86_min"))
        msWRMin = CastAide.AsDouble(dr("v161mspositiontaking_88_min"))


        msLOUMin = CastAide.AsDouble(dr("v161mslivepositiontaking_85_min"))
        msLOEMin = CastAide.AsDouble(dr("v161mslivepositiontaking_86_min"))
        msLHLMin = CastAide.AsDouble(dr("v161mslivepositiontaking_87_min"))
        msLNCMin = CastAide.AsDouble(dr("v161mslivepositiontaking_89_min"))
        msLNGMin = CastAide.AsDouble(dr("v161mslivepositiontaking_90_min"))

        sAuto1stOU = CastAide.AsDouble(dr("auto_v161spositiontaking_81"))
        sAuto1stOE = CastAide.AsDouble(dr("auto_v161spositiontaking_83"))
        'sAutoOU = CastAide.AsDouble(dr("auto_v161spositiontaking_85"))
        sAutoOE = CastAide.AsDouble(dr("auto_v161spositiontaking_86"))
        sAutoWR = CastAide.AsDouble(dr("auto_v161spositiontaking_88"))


        sAutoLOU = CastAide.AsDouble(dr("auto_v161slivepositiontaking_85"))
        sAutoLOE = CastAide.AsDouble(dr("auto_v161slivepositiontaking_86"))
        sAutoLHL = CastAide.AsDouble(dr("auto_v161slivepositiontaking_87"))
        sAutoLNC = CastAide.AsDouble(dr("auto_v161slivepositiontaking_89"))
        sAutoLNG = CastAide.AsDouble(dr("auto_v161slivepositiontaking_90"))
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        m1stOUMax = ms1stOUMax - s1stOUMax
        m1stOEMax = ms1stOEMax - s1stOEMax
        mOEMax = msOEMax - sOEMax
        mWRMax = msWRmax - sWRMax

        mLOUMax = msLOUMax - sLOUMax
        mLOEMax = msLOEMax - sLOEMax
        mLHLMax = msLHLMax - sLHLMax
        mLNCMax = msLNCMax - sLNCMax
        mLNGMax = msLNGMax - sLNGMax


        hiddenHash.Add("maxMS1611stOUD", ms1stOUMax)
        hiddenHash.Add("maxMS1611stOED", ms1stOEMax)
        hiddenHash.Add("maxMS161OED", msOEMax)
        hiddenHash.Add("maxMS161WRD", msWRmax)


        hiddenHash.Add("maxMS161OUL", msLOUMax)
        hiddenHash.Add("maxMS161OEL", msLOEMax)
        hiddenHash.Add("maxMS161HLL", msLHLMax)
        hiddenHash.Add("maxMS161NCL", msLNCMax)
        hiddenHash.Add("maxMS161NGL", msLNGMax)


        hiddenHash.Add("minMS1611stOUD", ms1stOUMin)
        hiddenHash.Add("minMS1611stOED", ms1stOEMin)
        hiddenHash.Add("minMS161OED", msOEMin)
        hiddenHash.Add("minMS161WRD", msWRMin)

        hiddenHash.Add("minMS161OUL", msLOUMin)
        hiddenHash.Add("minMS161OEL", msLOEMin)
        hiddenHash.Add("minMS161HLL", msLHLMin)
        hiddenHash.Add("minMS161NCL", msLNCMin)
        hiddenHash.Add("minMS161NGL", msLNGMin)

        hiddenHash.Add("maxM1611stOUD", m1stOUMax2)
        hiddenHash.Add("maxM1611stOED", m1stOEMax2)
        hiddenHash.Add("maxM161OED", mOEMax2)
        hiddenHash.Add("maxM161WRD", mWRMax2)

        hiddenHash.Add("maxM161OUL", mLOUMax2)
        hiddenHash.Add("maxM161OEL", mLOEMax2)
        hiddenHash.Add("maxM161HLL", mLHLMax2)
        hiddenHash.Add("maxM161NCL", mLNCMax2)
        hiddenHash.Add("maxM161NGL", mLNGMax2)

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
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1stOUMin, m1stOUMax, m1stOUMax))
        TemplateHandler.Assign(numbergameTpl, "a-1stlastou-val", pt)
        TemplateHandler.Assign(numbergameTpl, "checked-1stlastou", String.Empty)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1611stOED")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1stOEMin, m1stOEMax, m1stOEMax))
        TemplateHandler.Assign(numbergameTpl, "a-1stlastoe-val", pt)
        TemplateHandler.Assign(numbergameTpl, "checked-1stlastoe", String.Empty)


        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161OED")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOEMax))
        TemplateHandler.Assign(numbergameTpl, "a-oe-val", pt)
        TemplateHandler.Assign(numbergameTpl, "checked-oe", String.Empty)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161WRD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mWRMin, mWRMax, mWRMax))
        TemplateHandler.Assign(numbergameTpl, "a-warrior-val", pt)
        TemplateHandler.Assign(numbergameTpl, "checked-w", String.Empty)

        'get PT deadball value for super preset PT
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS1611stOUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(s1stOUMin, s1stOUMax, s1stOUMax))
        TemplateHandler.Assign(numbergameTpl, "p-1stlastou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS1611stOED")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(s1stOEMin, s1stOEMax, s1stOEMax))
        TemplateHandler.Assign(numbergameTpl, "p-1stlastoe-val", pt)


        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS161OED")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOEMin, sOEMax, sOEMax))
        TemplateHandler.Assign(numbergameTpl, "p-oe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS161WRD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sWRMin, sWRMax, sWRMax))
        TemplateHandler.Assign(numbergameTpl, "p-warrior-val", pt)

        'get PT value for super preset PT

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161OUL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLOUMin, mLOUMax, mLOUMax))
        TemplateHandler.Assign(numbergameTpl, "a-Lou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161OEL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLOEMin, mLOEMax, mLOEMax))
        TemplateHandler.Assign(numbergameTpl, "a-Loe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161HLL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLHLMin, mLHLMax, mLHLMax))
        TemplateHandler.Assign(numbergameTpl, "a-Lhl-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161NCL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLNCMin, mLNCMax, mLNCMax))
        TemplateHandler.Assign(numbergameTpl, "a-nextCombo-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161NGL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLNGMin, mLNGMax, mLNGMax))
        TemplateHandler.Assign(numbergameTpl, "a-NumGr-val", pt)

        '' insert by david.tran
     
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS161OUL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sLOUMin, sLOUMax, sLOUMax))
        TemplateHandler.Assign(numbergameTpl, "p-Lou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS161OEL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sLOEMin, sLOEMax, sLOEMax))
        TemplateHandler.Assign(numbergameTpl, "p-Loe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS161HLL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sLHLMin, sLHLMax, sLHLMax))
        TemplateHandler.Assign(numbergameTpl, "p-Lhl-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS161NCL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sLNCMin, sLNCMax, sLNCMax))
        TemplateHandler.Assign(numbergameTpl, "p-nextCombo-val", pt)

        '' insert by david.tran
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS161NGL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sLNGMin, sLNGMax, sLNGMax))
        TemplateHandler.Assign(numbergameTpl, "p-NumGr-val", pt)

        
        ' Get value for Auto Postion taking-------------------------------------------------------------------------------

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS1611stOUD")
        TemplateHandler.Assign(numbergameTpl, "au-1stlastou-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS161OUL")
        TemplateHandler.Assign(numbergameTpl, "au-Lou-btn", btnTpl)

        'For Deadball

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1611stOUD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAuto1stOU, 0))
        TemplateHandler.Assign(numbergameTpl, "au-1stlastou-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1611stOED")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAuto1stOE, 0))
        TemplateHandler.Assign(numbergameTpl, "au-1stlastoe-val", ptAuto)


        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161OED")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOE, 0))
        TemplateHandler.Assign(numbergameTpl, "au-oe-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161WRD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoWR, 0))
        TemplateHandler.Assign(numbergameTpl, "au-warrior-val", ptAuto)


        ''For live ball
        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161OUL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoLOU, 0))
        TemplateHandler.Assign(numbergameTpl, "au-Lou-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161OEL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoLOE, 0))
        TemplateHandler.Assign(numbergameTpl, "au-Loe-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161HLL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoLHL, 0))
        TemplateHandler.Assign(numbergameTpl, "au-Lhl-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161NCL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoLNC, 0))
        TemplateHandler.Assign(numbergameTpl, "au-nextCombo-val", ptAuto)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161NGL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoLNG, 0))
        TemplateHandler.Assign(numbergameTpl, "au-NumGr-val", ptAuto)

        Return numbergameTpl

    End Function




#End Region


#Region "Assign info for  Basketball"
    'this function is used to assign data into template for all sporttype that have 4 bettype
    '(1:Handicap, 3:Over/Under, 2:Odd/Even,9: Mix Parlay, 20:Money Line)
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (2: Basketball)
    Function GetBasketball(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String) As String
        Dim pt As String
        Dim ptAuto As String = String.Empty
        Dim hiddenHash As New Hashtable

        'assign text for position taking
        GetTitleCommon(tpl)
        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "oe-text", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(tpl, "title-col", "7")
        TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")
        TemplateHandler.RemoveBlock(tpl, "none-edit")
        'assign value for position taking
        GetMinMaxAddValue(tpl, sportTypeId, dr)
        GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

        Dim mHdpMin, mOUMin, mOEMin, mMLMin As Double
        Dim sHdpMin, sOUMin, sOEMin, sMLMin As Double

        Dim mHdpMax, mOUMax, mOEMax, mMLMax As Double
        Dim mHdpMax2, mOUMax2, mOEMax2, mMLMax2 As Double
        Dim sHdpMax, sOUMax, sOEMax, sMLMax As Double

        Dim msHdpMax, msOUMax, msOEMax, msMLMax As Double
        Dim msHdpMin, msOUMin, msOEMin, msMLMin As Double
        Dim sAutoHdp, sAutoOU, sAutoOE, sAutoML As Double

        mHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_max"))
        mOUMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_max"))
        mOEMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2_max"))

        mMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_max"))

        sHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1"))
        sOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3"))
        sOEMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_2"))

        sMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20"))

        msHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_1_max"))
        msOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_3_max"))
        msOEMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_2_max"))

        msMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_20_max"))

        msHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_1_min"))
        msOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_3_min"))
        msOEMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_2_min"))

        msMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_20_min"))

        sAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1"))
        sAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_3"))
        sAutoOE = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_2"))

        sAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20"))

        mHdpMax = msHdpMax - sHdpMax
        mOUMax = msOUMax - sOUMax
        mOEMax = msOEMax - sOEMax

        mMLMax = msMLMax - sMLMax

        hiddenHash.Add("maxMS" & sportTypeId & "Hdp", msHdpMax)
        hiddenHash.Add("maxMS" & sportTypeId & "OU", msOUMax)
        hiddenHash.Add("maxMS" & sportTypeId & "OE", msOEMax)
        hiddenHash.Add("maxMS" & sportTypeId & "ML", msMLMax)

        hiddenHash.Add("minMS" & sportTypeId & "Hdp", msHdpMin)
        hiddenHash.Add("minMS" & sportTypeId & "OU", msOUMin)
        hiddenHash.Add("minMS" & sportTypeId & "OE", msOEMin)
        hiddenHash.Add("minMS" & sportTypeId & "ML", msMLMin)

        hiddenHash.Add("maxM" & sportTypeId & "Hdp", mHdpMax2)
        hiddenHash.Add("maxM" & sportTypeId & "OU", mOUMax2)
        hiddenHash.Add("maxM" & sportTypeId & "OE", mOEMax2)
        hiddenHash.Add("maxM" & sportTypeId & "ML", mMLMax2)

        strHidBasket = PositionTaking.AssignMulipleHidden(hiddenHash)
        'assign data for Position taking dropdowns
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdpMax))
        TemplateHandler.Assign(tpl, "a-hdp-val", pt)


        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOUMax))
        TemplateHandler.Assign(tpl, "a-ou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OE")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOEMax))
        TemplateHandler.Assign(tpl, "a-oe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mMLMax))
        TemplateHandler.Assign(tpl, "a-ml-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sHdpMin, sHdpMax, sHdpMax))
        TemplateHandler.Assign(tpl, "p-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OU")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOUMin, sOUMax, sOUMax))
        TemplateHandler.Assign(tpl, "p-ou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OE")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOEMin, sOEMax, sOEMax))
        TemplateHandler.Assign(tpl, "p-oe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sMLMin, sMLMax, sMLMax))
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
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoHdp, 0))
        TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-Hdp", String.Empty)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOU, 0))
        TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-OU", String.Empty)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OE")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOE, 0))
        TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-OE", String.Empty)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoML, 0))
        TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-ML", String.Empty)


        GetBasketball = tpl
    End Function
#End Region

#Region "Assign info for Football, Tennis, Golf, OtherSports"
    'this function is used to assign data into template for all sporttype that have 4 bettype
    '(1:Handicap, 3:Over/Under, 2:Odd/Even, 20:Money Line)
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (2: Basketball, 3:Football, 5:Tennis, 10:Golf, 99:OtherSports )
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
        TemplateHandler.RemoveBlock(tpl, "none-edit")

        'assign value for position taking
        GetMinMaxAddValue(tpl, sportTypeId, dr)
        GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

        Dim mHdpMin, mOUMin, mOEMin, mMLMin As Double
        Dim sHdpMin, sOUMin, sOEMin, sMLMin As Double

        Dim mHdpMax, mOUMax, mOEMax, mMLMax As Double
        Dim mHdpMax2, mOUMax2, mOEMax2, mMLMax2 As Double
        Dim sHdpMax, sOUMax, sOEMax, sMLMax As Double

        Dim msHdpMax, msOUMax, msOEMax, msMLMax As Double
        Dim msHdpMin, msOUMin, msOEMin, msMLMin As Double
        Dim sAutoHdp, sAutoOU, sAutoOE, sAutoML As Double

        mHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_max"))
        mOUMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_max"))
        mOEMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2_max"))
        mMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_max"))

        sHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1"))
        sOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3"))
        sOEMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_2"))
        sMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20"))

        msHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_1_max"))
        msOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_3_max"))
        msOEMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_2_max"))
        msMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_20_max"))

        msHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_1_min"))
        msOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_3_min"))
        msOEMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_2_min"))
        msMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_20_min"))

        sAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1"))
        sAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_3"))
        sAutoOE = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_2"))
        sAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20"))

        mHdpMax = msHdpMax - sHdpMax
        mOUMax = msOUMax - sOUMax
        mOEMax = msOEMax - sOEMax
        mMLMax = msMLMax - sMLMax

        'hiddenHash.Add("autoS" & sportTypeId & "Hdp", sAutoHdp)
        'hiddenHash.Add("autoS" & sportTypeId & "OU", sAutoOU)
        'hiddenHash.Add("autoS" & sportTypeId & "OE", sAutoOE)
        'hiddenHash.Add("autoS" & sportTypeId & "ML", sAutoML)

        hiddenHash.Add("maxMS" & sportTypeId & "Hdp", msHdpMax)
        hiddenHash.Add("maxMS" & sportTypeId & "OU", msOUMax)
        hiddenHash.Add("maxMS" & sportTypeId & "OE", msOEMax)
        hiddenHash.Add("maxMS" & sportTypeId & "ML", msMLMax)

        hiddenHash.Add("minMS" & sportTypeId & "Hdp", msHdpMin)
        hiddenHash.Add("minMS" & sportTypeId & "OU", msOUMin)
        hiddenHash.Add("minMS" & sportTypeId & "OE", msOEMin)
        hiddenHash.Add("minMS" & sportTypeId & "ML", msMLMin)

        hiddenHash.Add("maxM" & sportTypeId & "Hdp", mHdpMax2)
        hiddenHash.Add("maxM" & sportTypeId & "OU", mOUMax2)
        hiddenHash.Add("maxM" & sportTypeId & "OE", mOEMax2)
        hiddenHash.Add("maxM" & sportTypeId & "ML", mMLMax2)

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
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdpMax))
        TemplateHandler.Assign(tpl, "a-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOUMax))
        TemplateHandler.Assign(tpl, "a-ou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OE")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOEMax))
        TemplateHandler.Assign(tpl, "a-oe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mMLMax))
        TemplateHandler.Assign(tpl, "a-ml-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sHdpMin, sHdpMax, sHdpMax))
        TemplateHandler.Assign(tpl, "p-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OU")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOUMin, sOUMax, sOUMax))
        TemplateHandler.Assign(tpl, "p-ou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OE")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOEMin, sOEMax, sOEMax))
        TemplateHandler.Assign(tpl, "p-oe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sMLMin, sMLMax, sMLMax))
        TemplateHandler.Assign(tpl, "p-ml-val", pt)
        '----------------------------------------------------------------------------------------------------------------


        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoHdp, 0))
        TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-Hdp", String.Empty)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOU, 0))
        TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-OU", String.Empty)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OE")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOE, 0))
        TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-OE", String.Empty)


        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoML, 0))
        TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-ML", String.Empty)

        Dim btnTpl As String = groupBtn
        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

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
        TemplateHandler.RemoveBlock(tpl, "none-edit")

        'assign value for position taking
        GetMinMaxAddValue(tpl, sportTypeId, dr)
        GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

        Dim mHdpMax, mOUMax, mMLMax As Double
        Dim mHdpMax2, mOUMax2, mMLMax2 As Double
        Dim mHdpMin, mOUMin, mMLMin As Double

        Dim sHdpMax, sOUMax, sMLMax As Double
        Dim sHdpMin, sOUMin, sMLMin As Double
        Dim sAutoHdp, sAutoOU, sAutoML As Double

        Dim msHdpMax, msOUMax, msMLMax As Double
        Dim msHdpMin, msOUMin, msMLMin As Double

        mHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_max"))
        mOUMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_max"))
        mMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_max"))

        sHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1"))
        sOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3"))
        sMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20"))

        msHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_1_max"))
        msOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_3_max"))
        msMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_20_max"))

        msHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_1_min"))
        msOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_3_min"))
        msMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_20_min"))

        sAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1"))
        sAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_3"))
        sAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20"))

        mHdpMax = msHdpMax - sHdpMax
        mOUMax = msOUMax - sOUMax
        mMLMax = msMLMax - sMLMax

        'hiddenHash.Add("autoS" & sportTypeId & "Hdp", sAutoHdp)
        'hiddenHash.Add("autoS" & sportTypeId & "OU", sAutoOU)
        'hiddenHash.Add("autoS" & sportTypeId & "ML", sAutoML)

        hiddenHash.Add("maxMS" & sportTypeId & "Hdp", msHdpMax)
        hiddenHash.Add("maxMS" & sportTypeId & "OU", msOUMax)
        hiddenHash.Add("maxMS" & sportTypeId & "ML", msMLMax)

        hiddenHash.Add("minMS" & sportTypeId & "Hdp", msHdpMin)
        hiddenHash.Add("minMS" & sportTypeId & "OU", msOUMin)
        hiddenHash.Add("minMS" & sportTypeId & "ML", msMLMin)

        hiddenHash.Add("maxM" & sportTypeId & "Hdp", mHdpMax2)
        hiddenHash.Add("maxM" & sportTypeId & "OU", mOUMax2)
        hiddenHash.Add("maxM" & sportTypeId & "ML", mMLMax2)

        strHidBaseball = PositionTaking.AssignMulipleHidden(hiddenHash)

        'assign data for Position taking dropdowns
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdpMax))
        TemplateHandler.Assign(tpl, "a-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOUMax))
        TemplateHandler.Assign(tpl, "a-ou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mMLMax))
        TemplateHandler.Assign(tpl, "a-ml-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sHdpMin, sHdpMax, sHdpMax))
        TemplateHandler.Assign(tpl, "p-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OU")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOUMin, sOUMax, sOUMax))
        TemplateHandler.Assign(tpl, "p-ou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sMLMin, sMLMax, sMLMax))
        TemplateHandler.Assign(tpl, "p-ml-val", pt)
        '----------------------------------------------------------------------------------------------------------------

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoHdp, 0))
        TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-Hdp", String.Empty)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOU, 0))
        TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-OU", String.Empty)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoML, 0))
        TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-ML", String.Empty)


        Dim btnTpl As String = groupBtn

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

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
        TemplateHandler.RemoveBlock(tpl, "none-edit")

        'assign value for minbet, maxbet, maxpermatch (all are 0 for adding case)            
        GetMinMaxAddValue(tpl, 11, dr)
        GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

        'get data and assign for template Position taking
        Dim mHdpMax, mMLMax As Double
        Dim mHdpMax2, mMLMax2 As Double
        Dim mHdpMin, mMLMin As Double

        Dim sHdpMax, sMLMax As Double
        Dim sHdpMin, sMLMin As Double

        Dim msHdpMax, msMLMax As Double
        Dim msHdpMin, msMLMin As Double
        Dim sAutoHdp, sAutoML As Double

        mHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_max"))
        mMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_max"))

        sHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1"))
        sMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20"))

        msHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_1_max"))
        msMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_20_max"))

        msHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_1_min"))
        msMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_20_min"))

        sAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1"))
        sAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20"))

        mHdpMax = msHdpMax - sHdpMax
        mMLMax = msMLMax - sMLMax

        'hiddenHash.Add("autoS" & sportTypeId & "Hdp", sAutoHdp)
        'hiddenHash.Add("autoS" & sportTypeId & "ML", sAutoML)

        hiddenHash.Add("maxMS" & sportTypeId & "Hdp", msHdpMax)
        hiddenHash.Add("maxMS" & sportTypeId & "ML", msMLMax)

        hiddenHash.Add("minMS" & sportTypeId & "Hdp", msHdpMin)
        hiddenHash.Add("minMS" & sportTypeId & "ML", msMLMin)

        hiddenHash.Add("maxM" & sportTypeId & "Hdp", mHdpMax2)
        hiddenHash.Add("maxM" & sportTypeId & "ML", mMLMax2)

        strHidMoto = PositionTaking.AssignMulipleHidden(hiddenHash)

        'assign data for Position taking dropdowns
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdpMax))
        TemplateHandler.Assign(tpl, "a-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mMLMax))
        TemplateHandler.Assign(tpl, "a-ml-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sHdpMin, sHdpMax, sHdpMax))
        TemplateHandler.Assign(tpl, "p-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sMLMin, sMLMax, sMLMax))
        TemplateHandler.Assign(tpl, "p-ml-val", pt)
        '----------------------------------------------------------------------------------------------------------------

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoHdp, 0))
        TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-Hdp", String.Empty)

        ptAuto = selDisabledTpl
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoML, 0))
        TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-ML", String.Empty)


        Dim btnTpl As String = groupBtn

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)

        GetDataMotoSports = tpl
    End Function
#End Region

#Region "Get Title Common"
    Sub GetTitleCommon(ByRef tpl As String)
        ' TemplateHandler.RemoveBlock(tpl, "auto-preset")
        TemplateHandler.Assign(tpl, "bet-text", Resources.Lang.lblBetSetting)
        TemplateHandler.Assign(tpl, "minbet-text", Resources.Lang.lblMinBet)
        TemplateHandler.Assign(tpl, "maxbet-text", Resources.Lang.lblMaxBet)
        TemplateHandler.Assign(tpl, "maxpermatch-text", Resources.Lang.lblMaxPerMatch)
        TemplateHandler.Assign(tpl, "pt-text", Resources.Lang.positiontaking)
        TemplateHandler.Assign(tpl, "auto-super-text", Resources.Lang.lblAutoSuperPresetPosition)
        TemplateHandler.Assign(tpl, "a-presetpt-text", Resources.Lang.lblMasterPresetPosition)
        TemplateHandler.Assign(tpl, "p-presetpt-text", Resources.Lang.lblSuperPresetPosition)
        TemplateHandler.Assign(tpl, "hdp-text", Resources.Lang.lblHandicapFC)
        TemplateHandler.Assign(tpl, "ml-text", Resources.Lang.moneyline)
        TemplateHandler.Assign(tpl, "copy-hdp-text", Resources.Lang.lblCopyFromHandicap)
        TemplateHandler.Assign(tpl, "au-presetpt-text", Resources.Lang.lblAutoSuperPresetPosition)



        TemplateHandler.Assign(tpl, "copy-hdp-col", "2")
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
    Shared Function GetCredit(ByVal dr As DataRow, ByRef tbl As String, Optional ByVal isEdit As Boolean = False)
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
    Shared Sub GetTranferTime(ByRef tpl As String, Optional ByVal isEdit As Boolean = False, Optional ByVal isDisplay As Boolean = False)
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
    End Sub
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
        TemplateHandler.Assign(strHtml, "{warrior-text}", Resources.Lang.warrior)
        TemplateHandler.Assign(strHtml, "{oe-text}", Resources.Lang.lblOddEven)

        Return strHtml
    End Function

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

#Region "GetMinMaxAddValue"
    Shared Sub GetMinMaxAddValue(ByRef tpl As String, ByVal sportTypeId As Integer, ByVal dr As DataRow)
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


End Class

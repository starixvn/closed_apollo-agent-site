Imports System.Resources
Imports DNAide.Utils
Imports Fanex.Data.DNAide
Imports DNAide.Core
Imports DNAide.Web
Imports System.Data
Imports System.Text
Imports Sunplus.Agent.Model.Entities.Permission

Public Class MasterEditMember
    Inherits IbcPage2

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

    Protected style As String
    Dim bodyTpl, for4ddlTpl, for3ddlTpl, basketballTpl As String
    Dim groupBtn As String
    Dim selectTpl As String
    Dim selDisabledTpl As String
    Public strUser As String
    Dim strContent As String
    Protected m As Integer
    Dim strHidSoccer, strHidBasket, strHidFootball, strHidTennis As String
    Dim strHidBaseball, strHidGolf, strHidMoto, strHidOther, strHidNumberGame, strHidMixedSport As String
    Public script As String = ""
    Dim CopyAll, CopyBasket As Boolean
    Dim isAutoHdp_master_Soccer As Boolean
    Dim isAutoHdp_master_Basketball As Boolean
    Dim isAutoHdp_Master As Boolean

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


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
        selectTpl = TemplateHandler.LoadTemplate("MemberInfo/DdlMemPT.html")
        selDisabledTpl = TemplateHandler.LoadTemplate("MemberInfo/DdlDisabledMem.html")
        for4ddlTpl = TemplateHandler.LoadTemplate("MemberInfo/Edit4Bettype.html")
        basketballTpl = TemplateHandler.LoadTemplate("MemberInfo/Edit5Bettype.html")
        TemplateHandler.RemoveBlock(bodyTpl, "none-edit-Min")

        If (String.IsNullOrEmpty(Request.QueryString("nobackground"))) Then
            lnkNobackground.Visible = False
        Else
            'If is from Account Menu, Then Set delay reload page
            Dim hash As Hashtable = AppSession.RequestQueryString
            If (hash Is Nothing) Then
                hash = New Hashtable
            End If
            If (Not hash.ContainsKey("FromAccount")) Then
                hash.Add("FromAccount", "../_MemberInfo/CustomerList/Master/MemberList.aspx")
            Else
                hash("FromAccount") = "../_MemberInfo/CustomerList/Master/MemberList.aspx"
            End If
            AppSession.RequestQueryString = hash
        End If

        If Not Request.QueryString("m") Then
            If Request.QueryString("m") = "0" Then
                m = 0
                saveCustid.Value = Request.QueryString("custid")
                strContent = LoadDataSingle()
                strContent += VariableOfScript()
                htmlSuperEditMember.Text = strContent
                isMult.Value = "0"
            Else
                m = 1
                strContent = LoadDataMulti()
                strContent += VariableOfScript()
                htmlSuperEditMember.Text = strContent
                isMult.Value = "1"
            End If
            saveAgentid.Value = Request.QueryString("agentid")
        End If

        If (Not String.IsNullOrEmpty(Request.QueryString("succ"))) Then
            htmlSuperEditMember.Text += String.Format(" <script>showErrMsg('{0}', 1); parent.top.menu.reloadMenuUsrInfo();</script> ", Resources.Lang.updatesuccessfull)
        End If

    End Sub

#Region "Load Information for Single Edit Member"
    Function LoadDataSingle() As String
        Dim strEditContent As New StringBuilder

        Dim rs As DbResult
        Dim hash As New Hashtable
        hash.Add("custid", saveCustid.Value)
        hash.Add("agentid", Request.QueryString("agentid"))
        rs = AccountInfo.Acc_CustomerSelEditCustomer_MAg(hash) 'DataServices.RunSP("Acc_CustomerSelEditCustomer_MAg", hash, cmdPool, 0, ConnectionType.ARCHIVE)
        If Not rs Is Nothing Then
            If rs.ResultSet.Tables(0).Rows.Count > 0 Then
                Dim row As DataRow = rs.ResultSet.Tables(0).Rows(0)
                strEditContent.Append(GetDataInfo(row, m))

                ' Casino
                TemplateHandler.Assign(bodyTpl, "hidSynCS", row("synccasino"))

                Common.GetSportTitle(bodyTpl, True)
                TemplateHandler.Assign(bodyTpl, "add-text", Resources.Lang.submit)
                TemplateHandler.Assign(bodyTpl, "add-id", "btnSubmitFooter")
                TemplateHandler.Assign(bodyTpl, "soccerPT", GetDataSoccer(row, m))
                TemplateHandler.Assign(bodyTpl, "basketPT", GetDataBasketball(row, 2, basketballTpl, m))
                TemplateHandler.Assign(bodyTpl, "footballPT", GetData4DDl(row, 3, for4ddlTpl, m))
                TemplateHandler.Assign(bodyTpl, "tennisPT", GetData4DDl(row, 5, for4ddlTpl, m))
                TemplateHandler.Assign(bodyTpl, "baseballPT", GetDataBaseball(row, 8, m))
                TemplateHandler.Assign(bodyTpl, "golfPT", GetData4DDl(row, 10, for4ddlTpl, m))
                TemplateHandler.Assign(bodyTpl, "motoPT", GetDataMotoSports(row, 11, m))

                TemplateHandler.Assign(bodyTpl, "otherPT", GetData4DDl(row, 99, for4ddlTpl, m))
                If UserPermission.IsShowNumberGame Then
                    TemplateHandler.Assign(bodyTpl, "numbergamePT", GetDataNumberGame(row, m))
                Else
                    TemplateHandler.Assign(bodyTpl, "numbergamePT", "")
                End If

                If UserPermission.IsShowMixedSport Then
                    TemplateHandler.Assign(bodyTpl, "mixSportPT", GetDataMixedSports(row, m))
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
                bodyTpl &= strHidMixedSport

                If isAutoHdp_master_Soccer Then
                    TemplateHandler.Assign(bodyTpl, "disable_chkall", "disabled")
                Else
                    TemplateHandler.Assign(bodyTpl, "disable_chkall", "")
                End If

                If isAutoHdp_master_Basketball Then
                    TemplateHandler.Assign(bodyTpl, "disable_chkbasket", "disabled")
                Else
                    TemplateHandler.Assign(bodyTpl, "disable_chkbasket", "")
                End If

                If CastAide.AsBoolean(row("disableautompt"), False) Then
                    TemplateHandler.Assign(bodyTpl, "clssHidden", "hidden")
                Else
                    TemplateHandler.Assign(bodyTpl, "clssHidden", "")
                End If

                strEditContent.Append(bodyTpl)
            End If
        End If

        LoadDataSingle = strEditContent.ToString()
    End Function
#End Region

#Region "Load Information for Multiple Edit Member"
    Function LoadDataMulti() As String
        Dim strEditContent As New StringBuilder

        Dim rs As DbResult
        Dim hash As New Hashtable
        hash.Add("custid", Sunplus.Agent.Model.Entities.UserSession.Current.CustId)
        hash.Add("recommend", Request.QueryString("agentid"))
        hash.Add("strcustid", Request("ArrCustid"))
        rs = AccountInfo.Acc_CustomerSelMAgentInfo_Multiple(hash)
        If Not rs Is Nothing Then
            If rs.ResultSet.Tables(0).Rows.Count > 0 Then
                Dim row As DataRow = rs.ResultSet.Tables(0).Rows(0)
                Common.GetSportTitle(bodyTpl, True)
                ' TemplateHandler.RemoveBlock(bodyTpl, "none-Mult")
                'TemplateHandler.RemoveBlock(bodyTpl, "none-Mult")

                strEditContent.Append(GetDataInfo(row, m))
                TemplateHandler.Assign(bodyTpl, "add-text", Resources.Lang.submit)
                TemplateHandler.Assign(bodyTpl, "add-id", "btnSubmit")
                TemplateHandler.Assign(bodyTpl, "soccerPT", GetDataSoccer(row, m))
                TemplateHandler.Assign(bodyTpl, "basketPT", GetDataBasketball(row, 2, basketballTpl, m))
                TemplateHandler.Assign(bodyTpl, "footballPT", GetData4DDl(row, 3, for4ddlTpl, m))
                TemplateHandler.Assign(bodyTpl, "tennisPT", GetData4DDl(row, 5, for4ddlTpl, m))
                TemplateHandler.Assign(bodyTpl, "baseballPT", GetDataBaseball(row, 8, m))
                TemplateHandler.Assign(bodyTpl, "golfPT", GetData4DDl(row, 10, for4ddlTpl, m))
                TemplateHandler.Assign(bodyTpl, "motoPT", GetDataMotoSports(row, 11, m))
                TemplateHandler.Assign(bodyTpl, "otherPT", GetData4DDl(row, 99, for4ddlTpl, m))

                If UserPermission.IsShowNumberGame Then
                    TemplateHandler.Assign(bodyTpl, "numbergamePT", GetDataNumberGame(row, m))
                Else
                    TemplateHandler.Assign(bodyTpl, "numbergamePT", "")
                End If

                If UserPermission.IsShowMixedSport Then
                    TemplateHandler.Assign(bodyTpl, "mixSportPT", GetDataMixedSports(row, m))
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
                bodyTpl &= strHidMixedSport

                'TemplateHandler.Assign(bodyTpl, "disable_chkall", "disabled")
                'TemplateHandler.Assign(bodyTpl, "disable_chkbasket", "disabled")
                If isAutoHdp_master_Soccer Then
                    TemplateHandler.Assign(bodyTpl, "disable_chkall", "disabled")
                Else
                    TemplateHandler.Assign(bodyTpl, "disable_chkall", "")
                End If

                If isAutoHdp_master_Basketball Then
                    TemplateHandler.Assign(bodyTpl, "disable_chkbasket", "disabled")
                Else
                    TemplateHandler.Assign(bodyTpl, "disable_chkbasket", "")
                End If

                If CastAide.AsBoolean(row("disableautompt"), False) Then
                    TemplateHandler.Assign(bodyTpl, "clssHidden", "hidden")
                Else
                    TemplateHandler.Assign(bodyTpl, "clssHidden", "")
                End If

                strEditContent.Append(bodyTpl)


            End If
        End If

        LoadDataMulti = strEditContent.ToString()
    End Function
#End Region

#Region "Assign info about UNam, Max Credit, Commission ..."
    Function GetDataInfo(ByVal dr As DataRow, ByVal isMult As Boolean) As String
        Dim strAccInfo As String = String.Empty

        strAccInfo = TemplateHandler.LoadTemplate("MemberInfo/AccInfoNoneEdit.html")
        'Dim errorTpl As String = TemplateHandler.LoadTemplate("ErrorMsg.html")
        'TemplateHandler.Assign(strAccInfo, "error-message", errorTpl)

        'strAccInfo = strAccInfo.Replace("header_main", "header_popup") 'nqminh - change to popup stylesheet!
        strAccInfo = Common.ReplaceWithPopupStyle(strAccInfo, 0, 0)

        If Not isMult Then
            GetTitleMember(strAccInfo)
            TemplateHandler.RemoveBlock(strAccInfo, "none-Single")
            TemplateHandler.RemoveBlock(strAccInfo, "none-Master")
            'TemplateHandler.RemoveBlock(strAccInfo, "none-Master")

            Dim nickname As String = CastAide.AsString(dr("username2"), String.Empty)
            If nickname.Length > 0 Then
                nickname = " [" + nickname + "]"
            End If
            TemplateHandler.Assign(strAccInfo, "title", Resources.Lang.edit + " " + CastAide.AsString(dr("username")) + nickname)
            TemplateHandler.Assign(strAccInfo, "uname-val", CastAide.AsString(dr("username")))
            TemplateHandler.Assign(strAccInfo, "fname-val", CastAide.AsString(dr("firstname")))
            TemplateHandler.Assign(strAccInfo, "lname-val", CastAide.AsString(dr("lastname")))
            TemplateHandler.Assign(strAccInfo, "email-val", CastAide.AsString(dr("fax")))
            TemplateHandler.Assign(strAccInfo, "group-val", CastAide.AsString(dr("ugroup")))
            TemplateHandler.Assign(strAccInfo, "credit-val", DataFormat.FormatNumber1(dr("credit")))
            TemplateHandler.Assign(strAccInfo, "phone-val", CastAide.AsString(dr("phone")))
            TemplateHandler.Assign(strAccInfo, "mobile-val", CastAide.AsString(dr("mobilephone")))
            TemplateHandler.Assign(strAccInfo, "suspend-text", Resources.Lang.suspended)
            TemplateHandler.Assign(strAccInfo, "add-text", Resources.Lang.submit)
            TemplateHandler.Assign(strAccInfo, "add-id", "btnSubmit")
            Dim StaClosed As String = ""
            If CBool(dr("closed")) Then
                StaClosed = "checked='checked' "

            End If

            If (CBool(dr("uplineClosed"))) Then
                StaClosed = StaClosed + "disabled='disabled'"
            End If
            TemplateHandler.Assign(strAccInfo, "check-val", StaClosed)
            TemplateHandler.Assign(strAccInfo, "IsClosed", CastAide.AsInt32(dr("closed"), 0))
            TemplateHandler.Assign(strAccInfo, "UplineClosed", CastAide.AsInt32(dr("uplineClosed"), 0))

            Dim status As String = ""

            If (CastAide.AsBoolean(dr("suspended"), False)) Then
                status = "checked='checked' "
            End If

            If (CBool(dr("uplineClosed")) Or CastAide.AsBoolean(dr("uplinesuspended"), False)) Then
                status = status + "disabled='disabled'"
            End If

            TemplateHandler.Assign(strAccInfo, "suspend-check", status)
            TemplateHandler.Assign(strAccInfo, "IsSuspended", CastAide.AsInt32(dr("suspended"), 0))
            TemplateHandler.Assign(strAccInfo, "UplineSuspended", CastAide.AsInt32(dr("uplinesuspended"), 0))

            TemplateHandler.Assign(strAccInfo, "a-comm-val", DataFormat.FormatNumber4(dr("discount")))
            TemplateHandler.Assign(strAccInfo, "a-commo-val", DataFormat.FormatNumber4(dr("discountcs")))
            TemplateHandler.Assign(strAccInfo, "a-comm1x2-val", DataFormat.FormatNumber4(dr("discount1x2")))


            TemplateHandler.Assign(strAccInfo, "comm-val", DataFormat.FormatNumber4(dr("playerdiscount")))
            TemplateHandler.Assign(strAccInfo, "commo-val", DataFormat.FormatNumber4(dr("playerdiscountcs")))
            TemplateHandler.Assign(strAccInfo, "comm1x2-val", DataFormat.FormatNumber4(dr("playerdiscount1x2")))


            If (Not UserPermission.IsShowNumberGame Or Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission) Then
                TemplateHandler.Assign(strAccInfo, "a-commng-val", String.Empty)
                TemplateHandler.Assign(strAccInfo, "commng-val", String.Empty)
            Else
                TemplateHandler.Assign(strAccInfo, "a-commng-val", DataFormat.FormatNumber4(dr("discountnumber")))
                TemplateHandler.Assign(strAccInfo, "commng-val", DataFormat.FormatNumber4(dr("playerdiscountnumber")))
            End If

            TemplateHandler.Assign(strAccInfo, "maxperball-value", DataFormat.FormatNumber1(dr("maxperballv161")))

            TemplateHandler.Assign(strAccInfo, "minbet-val", DataFormat.FormatNumber1(dr("minbet")))
            TemplateHandler.Assign(strAccInfo, "maxbet-val", DataFormat.FormatNumber1(dr("maxbet")))
            TemplateHandler.Assign(strAccInfo, "maxpermatch-val", DataFormat.FormatNumber1(dr("maxpermatch")))
            GetMinMaxEditValue(strAccInfo, "2", dr)
            GetMinMaxEditValue(strAccInfo, "3", dr)
            GetMinMaxEditValue(strAccInfo, "5", dr)
            GetMinMaxEditValue(strAccInfo, "8", dr)
            GetMinMaxEditValue(strAccInfo, "10", dr)
            GetMinMaxEditValue(strAccInfo, "11", dr)

            If (Not UserPermission.IsShowNumberGame) Then

                TemplateHandler.Assign(strAccInfo, "minbet161-val", DataFormat.FormatNumber1(dr("minbetv99MP")))
                TemplateHandler.Assign(strAccInfo, "maxbet161-val", DataFormat.FormatNumber1(dr("maxbetv99MP")))
                TemplateHandler.Assign(strAccInfo, "maxpermatch161-val", DataFormat.FormatNumber1(dr("maxpermatchv99MP")))

                TemplateHandler.Assign(strAccInfo, "minbet99MP-val", String.Empty)
                TemplateHandler.Assign(strAccInfo, "maxbet99MP-val", String.Empty)
                TemplateHandler.Assign(strAccInfo, "maxpermatch99MP-val", String.Empty)
                TemplateHandler.Assign(strAccInfo, "hideClss", "hidden")

            Else
                TemplateHandler.Assign(strAccInfo, "minbet99MP-val", DataFormat.FormatNumber1(dr("minbetv99MP")))
                TemplateHandler.Assign(strAccInfo, "maxbet99MP-val", DataFormat.FormatNumber1(dr("maxbetv99MP")))
                TemplateHandler.Assign(strAccInfo, "maxpermatch99MP-val", DataFormat.FormatNumber1(dr("maxpermatchv99MP")))

                TemplateHandler.Assign(strAccInfo, "minbet161-val", DataFormat.FormatNumber1(dr("minbetv161")))
                TemplateHandler.Assign(strAccInfo, "maxbet161-val", DataFormat.FormatNumber1(dr("maxbetv161")))
                TemplateHandler.Assign(strAccInfo, "maxpermatch161-val", DataFormat.FormatNumber1(dr("maxpermatchv161")))
                TemplateHandler.Assign(strAccInfo, "hideClss", String.Empty)
            End If

            GetMinMaxEditValue(strAccInfo, "99", dr)

        Else
            TemplateHandler.RemoveBlock(strAccInfo, "none-Mult")
            TemplateHandler.Assign(strAccInfo, "title", Resources.Lang.lblEditMultiple)
            TemplateHandler.Assign(strAccInfo, "check-val", "")
            TemplateHandler.Assign(strAccInfo, "status-text", Resources.Lang.status)
            TemplateHandler.Assign(strAccInfo, "closed-text", Resources.Lang.close)
            TemplateHandler.Assign(strAccInfo, "add-text", Resources.Lang.submit)
            TemplateHandler.Assign(strAccInfo, "add-id", "btnSubmit")
        End If

        GetDataInfo = strAccInfo
    End Function
#End Region

#Region "Assign Info for Soccer"
    'this function is used to assign data into template for Soccer
    'dr: row contains data get from db
    Function GetDataSoccer(ByVal dr As DataRow, Optional ByVal isMult As Boolean = False) As String
        Dim soccerTpl As String = String.Empty
        Dim headerSTpl As String = String.Empty
        Dim hiddenHash As New Hashtable

        soccerTpl = TemplateHandler.LoadTemplate("MemberInfo/SoccerEditMPT.html")
        headerSTpl = TemplateHandler.LoadTemplate("MemberInfo/SBettypeHeader.html")

        TemplateHandler.RemoveBlock(soccerTpl, "none-Master")
        If isMult Then
            If CastAide.AsBoolean(dr("disableautompt"), False) Then
                TemplateHandler.Assign(soccerTpl, "clssHidden", "hidden")
                TemplateHandler.Assign(soccerTpl, "dead-row", "2")
                TemplateHandler.Assign(soccerTpl, "live-row", "1")
                TemplateHandler.Assign(soccerTpl, "copy-live-col", "1")
            Else
                TemplateHandler.Assign(soccerTpl, "clssHidden", "")
                TemplateHandler.Assign(soccerTpl, "dead-row", "3")
                TemplateHandler.Assign(soccerTpl, "live-row", "3")
                TemplateHandler.Assign(soccerTpl, "copy-live-col", "3")
            End If
            TemplateHandler.RemoveBlock(soccerTpl, "none-Multi")
            TemplateHandler.RemoveBlock(soccerTpl, "none-Multi")

            Dim chkUpdateMultiple As String = LoadCheckBoxUpdateMultipe("1", Resources.Lang.positiontaking)
            TemplateHandler.Assign(soccerTpl, "ChkUpdatePT", chkUpdateMultiple)


        Else
            If CastAide.AsBoolean(dr("disableautompt"), False) Then
                TemplateHandler.Assign(soccerTpl, "clssHidden", "hidden")
                TemplateHandler.Assign(soccerTpl, "dead-row", "3")
                TemplateHandler.Assign(soccerTpl, "live-row", "2")
                TemplateHandler.Assign(soccerTpl, "copy-live-col", "1")
            Else
                TemplateHandler.Assign(soccerTpl, "clssHidden", "")
                TemplateHandler.Assign(soccerTpl, "dead-row", "5")
                TemplateHandler.Assign(soccerTpl, "live-row", "4")
                TemplateHandler.Assign(soccerTpl, "copy-live-col", "3")
            End If
            TemplateHandler.Assign(soccerTpl, "ChkUpdatePT", String.Empty)
        End If
        TemplateHandler.Assign(soccerTpl, "col-title", "12")
        TemplateHandler.Assign(soccerTpl, "bettype-header", LoadSoccerPTHeader())
        GetTitleCommon(soccerTpl)

        TemplateHandler.Assign(soccerTpl, "deadball-text", Resources.Lang.lblDeadball)
        TemplateHandler.Assign(soccerTpl, "live-text", Resources.Lang.lblLive)
        TemplateHandler.Assign(soccerTpl, "p-preset-live", Resources.Lang.master)
        TemplateHandler.Assign(soccerTpl, "a-preset-live", Resources.Lang.agent)
        TemplateHandler.Assign(soccerTpl, "cols-live", "1")

        TemplateHandler.Assign(soccerTpl, "copy-1x2-text", Resources.Lang.lblCopyFrom1x2)
        'assign value of Minbet, Maxbet , ...           

        Dim aHdp, aOU, a1stHdp, a1stOU, aOE, a1x2, aCS, aTG, aMP, aOR As Double
        Dim aLHdp, aLOU, aL1stHdp, aL1stOU As Double

        Dim mHdpMin, mOUMin, m1stHdpMin, m1stOUMin, mOEMin, m1x2Min, mCSMin, mTGMin, mMPMin, mORMin As Double
        Dim mLHdpMin, mLOUMin, mL1stHdpMin, mL1stOUMin As Double

        Dim mHdp, mOU, m1stHdp, m1stOU, mOE, m1x2, mCS, mTG, mMP, mOR As Double
        Dim mLHdp, mLOU, mL1stHdp, mL1stOU As Double

        Dim mHdpMax, mOUMax, m1stHdpMax, m1stOUMax, mOEMax, m1x2Max, mCSMax, mTGMax, mMPMax, mORMax As Double
        Dim mLHdpMax, mLOUMax, mL1stHdpMax, mL1stOUMax As Double

        Dim isAutoHdp, isAutoOU, isAuto1stHdp, isAuto1stOU, isAutoOE, isAuto1x2, isAutoCS, isAutoTG, isAutoMP, isAutoOR As Integer
        Dim isAutoLHdp, isAutoLOU, isAutoL1stHdp, isAutoL1stOU As Integer

        Dim isAutoHdp_mem, isAutoOU_mem, isAuto1stHdp_mem, isAuto1stOU_mem, isAutoOE_mem, isAuto1x2_mem, isAutoCS_mem, isAutoTG_mem, isAutoMP_mem, isAutoOR_mem As Integer
        Dim isAutoLHdp_mem, isAutoLOU_mem, isAutoL1stHdp_mem, isAutoL1stOU_mem As Integer

        Dim mAutoHdp, mAutoOU, mAuto1stHdp, mAuto1stOU, mAutoOE, mAuto1x2, mAutoCS, mAutoTG, mAutoMP, mAutoOR As Double
        Dim mAutoLHdp, mAutoLOU, mAutoL1stHdp, mAutoL1stOU As Double

        Dim mAutoHdpMax, mAutoOUMax, mAuto1stHdpMax, mAuto1stOUMax, mAutoOEMax, mAuto1x2Max, mAutoCSMax, mAutoTGMax, mAutoMPMax, mAutoORMax As Double
        Dim mAutoLHdpMax, mAutoLOUMax, mAutoL1stHdpMax, mAutoL1stOUMax As Double

        mHdp = CastAide.AsDouble(dr("mpositiontaking_1"))
        mOU = CastAide.AsDouble(dr("mpositiontaking_3"))
        m1stHdp = CastAide.AsDouble(dr("mpositiontaking_7"))
        m1stOU = CastAide.AsDouble(dr("mpositiontaking_8"))
        mOE = CastAide.AsDouble(dr("mpositiontaking_2"))
        m1x2 = CastAide.AsDouble(dr("mpositiontaking_5"))
        mCS = CastAide.AsDouble(dr("mpositiontaking_4"))
        mTG = CastAide.AsDouble(dr("mpositiontaking_6"))
        mMP = CastAide.AsDouble(dr("mpositiontaking_9"))
        mOR = CastAide.AsDouble(dr("mpositiontaking_10"))
        mLHdp = CastAide.AsDouble(dr("mlivepositiontaking_1"))
        mLOU = CastAide.AsDouble(dr("mlivepositiontaking_3"))
        mL1stHdp = CastAide.AsDouble(dr("mlivepositiontaking_7"))
        mL1stOU = CastAide.AsDouble(dr("mlivepositiontaking_8"))

        ' IsAuto master
        isAutoHdp = CastAide.AsInt16(dr("isauto_mpositiontaking_1"))
        isAutoOU = CastAide.AsInt16(dr("isauto_mpositiontaking_3"))
        isAuto1stHdp = CastAide.AsInt16(dr("isauto_mpositiontaking_7"))
        isAuto1stOU = CastAide.AsInt16(dr("isauto_mpositiontaking_8"))
        isAutoOE = CastAide.AsInt16(dr("isauto_mpositiontaking_2"))
        isAuto1x2 = CastAide.AsInt16(dr("isauto_mpositiontaking_5"))
        isAutoCS = CastAide.AsInt16(dr("isauto_mpositiontaking_4"))
        isAutoTG = CastAide.AsInt16(dr("isauto_mpositiontaking_6"))
        isAutoMP = CastAide.AsInt16(dr("isauto_mpositiontaking_9"))
        isAutoOR = CastAide.AsInt16(dr("isauto_mpositiontaking_10"))

        isAutoLHdp = CastAide.AsInt16(dr("isauto_mlivepositiontaking_1"))
        isAutoLOU = CastAide.AsInt16(dr("isauto_mlivepositiontaking_3"))
        isAutoL1stHdp = CastAide.AsInt16(dr("isauto_mlivepositiontaking_7"))
        isAutoL1stOU = CastAide.AsInt16(dr("isauto_mlivepositiontaking_8"))
        '--

        ' Auto master on member current value
        mAutoHdp = CastAide.AsDouble(dr("auto_mpositiontaking_mem_1"))
        mAutoOU = CastAide.AsDouble(dr("auto_mpositiontaking_mem_3"))
        mAuto1stHdp = CastAide.AsDouble(dr("auto_mpositiontaking_mem_7"))
        mAuto1stOU = CastAide.AsDouble(dr("auto_mpositiontaking_mem_8"))
        mAutoOE = CastAide.AsDouble(dr("auto_mpositiontaking_mem_2"))
        mAuto1x2 = CastAide.AsDouble(dr("auto_mpositiontaking_mem_5"))
        mAutoCS = CastAide.AsDouble(dr("auto_mpositiontaking_mem_4"))
        mAutoTG = CastAide.AsDouble(dr("auto_mpositiontaking_mem_6"))
        mAutoMP = CastAide.AsDouble(dr("auto_mpositiontaking_mem_9"))
        mAutoOR = CastAide.AsDouble(dr("auto_mpositiontaking_mem_10"))

        mAutoLHdp = CastAide.AsDouble(dr("auto_mlivepositiontaking_mem_1"))
        mAutoLOU = CastAide.AsDouble(dr("auto_mlivepositiontaking_mem_3"))
        mAutoL1stHdp = CastAide.AsDouble(dr("auto_mlivepositiontaking_mem_7"))
        mAutoL1stOU = CastAide.AsDouble(dr("auto_mlivepositiontaking_mem_8"))
        '----

        hiddenHash.Add("isAutoHdp", isAutoHdp)
        hiddenHash.Add("isAutoOU", isAutoOU)
        hiddenHash.Add("isAuto1stHdp", isAuto1stHdp)
        hiddenHash.Add("isAuto1stOU", isAuto1stOU)
        hiddenHash.Add("isAutoOE", isAutoOE)

        hiddenHash.Add("isAuto1x2", isAuto1x2)
        hiddenHash.Add("isAutoCS", isAutoCS)
        hiddenHash.Add("isAutoTG", isAutoTG)
        hiddenHash.Add("isAutoMP", isAutoMP)
        hiddenHash.Add("isAutoOR", isAutoOR)

        hiddenHash.Add("isAutoLHdp", isAutoLHdp)
        hiddenHash.Add("isAutoLOU", isAutoLOU)
        hiddenHash.Add("isAutoL1stHdp", isAutoL1stHdp)
        hiddenHash.Add("isAutoL1stOU", isAutoL1stOU)

        strHidSoccer = PositionTaking.AssignMulipleHidden(hiddenHash)

        If Not isMult Then 'this case for Single Edit              
            aHdp = CastAide.AsDouble(dr("apositiontaking_1"))
            aOU = CastAide.AsDouble(dr("apositiontaking_3"))
            a1stHdp = CastAide.AsDouble(dr("apositiontaking_7"))
            a1stOU = CastAide.AsDouble(dr("apositiontaking_8"))
            aOE = CastAide.AsDouble(dr("apositiontaking_2"))
            a1x2 = CastAide.AsDouble(dr("apositiontaking_5"))
            aCS = CastAide.AsDouble(dr("apositiontaking_4"))
            aTG = CastAide.AsDouble(dr("apositiontaking_6"))
            aMP = CastAide.AsDouble(dr("apositiontaking_9"))
            aOR = CastAide.AsDouble(dr("apositiontaking_10"))
            aLHdp = CastAide.AsDouble(dr("alivepositiontaking_1"))
            aLOU = CastAide.AsDouble(dr("alivepositiontaking_3"))
            aL1stHdp = CastAide.AsDouble(dr("alivepositiontaking_7"))
            aL1stOU = CastAide.AsDouble(dr("alivepositiontaking_8"))

            mHdpMin = CastAide.AsDouble(dr("minpos_1"))
            mOUMin = CastAide.AsDouble(dr("minpos_3"))
            m1stHdpMin = CastAide.AsDouble(dr("minpos_7"))
            m1stOUMin = CastAide.AsDouble(dr("minpos_8"))
            mOEMin = CastAide.AsDouble(dr("minpos_2"))
            m1x2Min = CastAide.AsDouble(dr("minpos_5"))
            mCSMin = CastAide.AsDouble(dr("minpos_4"))
            mTGMin = CastAide.AsDouble(dr("minpos_6"))
            mMPMin = CastAide.AsDouble(dr("minpos_9"))
            mORMin = CastAide.AsDouble(dr("minpos_10"))
            mLHdpMin = CastAide.AsDouble(dr("minlpos_1"))
            mLOUMin = CastAide.AsDouble(dr("minlpos_3"))
            mL1stHdpMin = CastAide.AsDouble(dr("minlpos_7"))
            mL1stOUMin = CastAide.AsDouble(dr("minlpos_8"))

            mHdpMax = CastAide.AsDouble(dr("maxpos_1"))
            mOUMax = CastAide.AsDouble(dr("maxpos_3"))
            m1stHdpMax = CastAide.AsDouble(dr("maxpos_7"))
            m1stOUMax = CastAide.AsDouble(dr("maxpos_8"))
            mOEMax = CastAide.AsDouble(dr("maxpos_2"))
            m1x2Max = CastAide.AsDouble(dr("maxpos_5"))
            mCSMax = CastAide.AsDouble(dr("maxpos_4"))
            mTGMax = CastAide.AsDouble(dr("maxpos_6"))
            mMPMax = CastAide.AsDouble(dr("maxpos_9"))
            mORMax = CastAide.AsDouble(dr("maxpos_10"))
            mLHdpMax = CastAide.AsDouble(dr("maxlpos_1"))
            mLOUMax = CastAide.AsDouble(dr("maxlpos_3"))
            mL1stHdpMax = CastAide.AsDouble(dr("maxlpos_7"))
            mL1stOUMax = CastAide.AsDouble(dr("maxlpos_8"))

            ' Auto master max value
            mAutoHdpMax = CastAide.AsDouble(dr("maxauto_mpositiontaking_mem_1"))
            mAutoOUMax = CastAide.AsDouble(dr("maxauto_mpositiontaking_mem_3"))
            mAuto1stHdpMax = CastAide.AsDouble(dr("maxauto_mpositiontaking_mem_7"))
            mAuto1stOUMax = CastAide.AsDouble(dr("maxauto_mpositiontaking_mem_8"))
            mAutoOEMax = CastAide.AsDouble(dr("maxauto_mpositiontaking_mem_2"))
            mAuto1x2Max = CastAide.AsDouble(dr("maxauto_mpositiontaking_mem_5"))
            mAutoCSMax = CastAide.AsDouble(dr("maxauto_mpositiontaking_mem_4"))
            mAutoTGMax = CastAide.AsDouble(dr("maxauto_mpositiontaking_mem_6"))
            mAutoMPMax = CastAide.AsDouble(dr("maxauto_mpositiontaking_mem_9"))
            mAutoORMax = CastAide.AsDouble(dr("maxauto_mpositiontaking_mem_10"))

            mAutoLHdpMax = CastAide.AsDouble(dr("maxauto_mlivepositiontaking_mem_1"))
            mAutoLOUMax = CastAide.AsDouble(dr("maxauto_mlivepositiontaking_mem_3"))
            mAutoL1stHdpMax = CastAide.AsDouble(dr("maxauto_mlivepositiontaking_mem_7"))
            mAutoL1stOUMax = CastAide.AsDouble(dr("maxauto_mlivepositiontaking_mem_8"))
            '---
            ' IsAuto master on member
            isAutoHdp_mem = CastAide.AsInt16(dr("isauto_mpositiontaking_mem_1"))
            isAutoOU_mem = CastAide.AsInt16(dr("isauto_mpositiontaking_mem_3"))
            isAuto1stHdp_mem = CastAide.AsInt16(dr("isauto_mpositiontaking_mem_7"))
            isAuto1stOU_mem = CastAide.AsInt16(dr("isauto_mpositiontaking_mem_8"))
            isAutoOE_mem = CastAide.AsInt16(dr("isauto_mpositiontaking_mem_2"))
            isAuto1x2_mem = CastAide.AsInt16(dr("isauto_mpositiontaking_mem_5"))
            isAutoCS_mem = CastAide.AsInt16(dr("isauto_mpositiontaking_mem_4"))
            isAutoTG_mem = CastAide.AsInt16(dr("isauto_mpositiontaking_mem_6"))
            isAutoMP_mem = CastAide.AsInt16(dr("isauto_mpositiontaking_mem_9"))
            isAutoOR_mem = CastAide.AsInt16(dr("isauto_mpositiontaking_mem_10"))

            isAutoLHdp_mem = CastAide.AsInt16(dr("isauto_mlivepositiontaking_mem_1"))
            isAutoLOU_mem = CastAide.AsInt16(dr("isauto_mlivepositiontaking_mem_3"))
            isAutoL1stHdp_mem = CastAide.AsInt16(dr("isauto_mlivepositiontaking_mem_7"))
            isAutoL1stOU_mem = CastAide.AsInt16(dr("isauto_mlivepositiontaking_mem_8"))
            '--
        Else 'this case for Multiple Edit
            mHdpMax = mHdp
            mOUMax = mOU
            m1stHdpMax = m1stHdp
            m1stOUMax = m1stOU
            mOEMax = mOE
            m1x2Max = m1x2
            mCSMax = mCS
            mTGMax = mTG
            mMPMax = mMP
            mORMax = mOR
            mLHdpMax = mLHdp
            mLOUMax = mLOU
            mL1stHdpMax = mL1stHdp
            mL1stOUMax = mL1stOU

            'Auto master max value
            mAutoHdpMax = mAutoHdp
            mAutoOUMax = mAutoOU
            mAuto1stHdpMax = mAuto1stHdp
            mAuto1stOUMax = mAuto1stOU

            mAutoOEMax = mAutoOE
            mAuto1x2Max = mAuto1x2
            mAutoCSMax = mAutoCS
            mAutoTGMax = mAutoTG
            mAutoMPMax = mAutoMP
            mAutoORMax = mAutoOR

            mAutoLHdpMax = mAutoLHdp
            mAutoLOUMax = mAutoLOU
            mAutoL1stHdpMax = mAutoL1stHdp
            mAutoL1stOUMax = mAutoL1stOU
            '---

        End If

        'assign Increase and Reduce button for template
        Dim btnTpl As String = groupBtn
        If isAutoHdp Then
            TemplateHandler.Assign(soccerTpl, "p-hdp-btn", String.Empty)
            If isAutoHdp_mem = 0 Then
                TemplateHandler.Assign(soccerTpl, "chk-hdpd", "disabled")
                isAutoHdp_master_Soccer = True
            End If
        Else
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM1HdpD")
            TemplateHandler.Assign(soccerTpl, "p-hdp-btn", btnTpl)
            isAutoHdp_master_Soccer = False
        End If

        btnTpl = groupBtn
        If isAuto1x2 Then
            TemplateHandler.Assign(soccerTpl, "p-1x2-btn", String.Empty)
            If isAuto1x2_mem = 0 Then
                TemplateHandler.Assign(soccerTpl, "chk-1x2", "disabled")
            End If
        Else
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM11x2")
            TemplateHandler.Assign(soccerTpl, "p-1x2-btn", btnTpl)
        End If

        btnTpl = groupBtn
        If isAutoLHdp Then
            TemplateHandler.Assign(soccerTpl, "p-Lhdp-btn", String.Empty)
            If isAutoLHdp_mem = 0 Then
                TemplateHandler.Assign(soccerTpl, "chk-hdpl", "disabled")
            End If
        Else
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM1HdpL")
            TemplateHandler.Assign(soccerTpl, "p-Lhdp-btn", btnTpl)
        End If

        ' Auto master on member
        Dim ptAuto As String
        Dim strChecked As String
        Dim pt As String = selectTpl
        If isAutoHdp_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM1HdpD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoHdpMax, mAutoHdp))
        TemplateHandler.Assign(soccerTpl, "au-hdp-val", ptAuto)
        TemplateHandler.Assign(soccerTpl, "checked-Hdp", strChecked)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoM1HdpD")
        TemplateHandler.Assign(soccerTpl, "au-hdp-btn", btnTpl)
        '---

        If isAutoHdp Then
            TemplateHandler.Assign(soccerTpl, "a-hdp-val", aHdp)
            TemplateHandler.Assign(soccerTpl, "p-hdp-val", mHdp)
        Else
            TemplateHandler.Assign(soccerTpl, "a-hdp-val", aHdp)
            TemplateHandler.Assign(pt, "ddl-Id", "listM1HdpD")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(soccerTpl, "p-hdp-val", pt)
            If isAutoOU And isAuto1stHdp And isAuto1stOU And isAutoOE Then
                TemplateHandler.RemoveBlock(soccerTpl, "limit-CopyHdp")
            End If

        End If

        'Auto master on member
        If isAutoOU_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM1OUD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoOUMax, mAutoOU))
        TemplateHandler.Assign(soccerTpl, "au-ou-val", ptAuto)
        TemplateHandler.Assign(soccerTpl, "checked-OU", strChecked)
        '---

        If isAutoOU Then
            TemplateHandler.Assign(soccerTpl, "a-ou-val", aOU)
            TemplateHandler.Assign(soccerTpl, "p-ou-val", mOU)

        Else
            TemplateHandler.Assign(soccerTpl, "a-ou-val", aOU)
            TemplateHandler.Assign(pt, "ddl-Id", "listM1OUD")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOU))
            TemplateHandler.Assign(soccerTpl, "p-ou-val", pt)
        End If


        'Auto master on member
        If isAuto1stHdp_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM11stHdpD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAuto1stHdpMax, mAuto1stHdp))
        TemplateHandler.Assign(soccerTpl, "au-1sthdp-val", ptAuto)
        TemplateHandler.Assign(soccerTpl, "checked-1stHdp", strChecked)

        '---

        If isAuto1stHdp Then
            TemplateHandler.Assign(soccerTpl, "a-1sthdp-val", a1stHdp)
            TemplateHandler.Assign(soccerTpl, "p-1sthdp-val", m1stHdp)

        Else
            TemplateHandler.Assign(soccerTpl, "a-1sthdp-val", a1stHdp)
            TemplateHandler.Assign(pt, "ddl-Id", "listM11stHdpD")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1stHdpMin, m1stHdpMax, m1stHdp))
            TemplateHandler.Assign(soccerTpl, "p-1sthdp-val", pt)
        End If


        'Auto master on member
        If isAuto1stOU_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM11stOUD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAuto1stOUMax, mAuto1stOU))
        TemplateHandler.Assign(soccerTpl, "au-1stou-val", ptAuto)
        TemplateHandler.Assign(soccerTpl, "checked-1stOU", strChecked)
        '---

        If isAuto1stOU Then
            TemplateHandler.Assign(soccerTpl, "a-1stou-val", a1stOU)
            TemplateHandler.Assign(soccerTpl, "p-1stou-val", m1stOU)

        Else
            TemplateHandler.Assign(soccerTpl, "a-1stou-val", a1stOU)
            TemplateHandler.Assign(pt, "ddl-Id", "listM11stOUD")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1stOUMin, m1stOUMax, m1stOU))
            TemplateHandler.Assign(soccerTpl, "p-1stou-val", pt)
        End If


        'Auto master on member
        If isAutoOE_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM1OE")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoOEMax, mAutoOE))
        TemplateHandler.Assign(soccerTpl, "au-oe-val", ptAuto)
        TemplateHandler.Assign(soccerTpl, "checked-OE", strChecked)
        '---

        If isAutoOE Then
            TemplateHandler.Assign(soccerTpl, "a-oe-val", aOE)
            TemplateHandler.Assign(soccerTpl, "p-oe-val", mOE)

        Else
            TemplateHandler.Assign(soccerTpl, "a-oe-val", aOE)
            TemplateHandler.Assign(pt, "ddl-Id", "listM1OE")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOE))
            TemplateHandler.Assign(soccerTpl, "p-oe-val", pt)
        End If


        'Auto master on member
        If isAuto1x2_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM11x2")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAuto1x2Max, mAuto1x2))
        TemplateHandler.Assign(soccerTpl, "au-1x2-val", ptAuto)
        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoM11x2")
        TemplateHandler.Assign(soccerTpl, "au-1x2-btn", btnTpl)
        TemplateHandler.Assign(soccerTpl, "checked-1x2", strChecked)
        '---

        If isAuto1x2 Then
            TemplateHandler.Assign(soccerTpl, "a-1x2-val", a1x2)
            TemplateHandler.Assign(soccerTpl, "p-1x2-val", m1x2)
            ' TemplateHandler.RemoveBlock(soccerTpl, "limit-Copy1x2")

        Else
            TemplateHandler.Assign(soccerTpl, "a-1x2-val", a1x2)
            TemplateHandler.Assign(pt, "ddl-Id", "listM11x2")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1x2Min, m1x2Max, m1x2))
            TemplateHandler.Assign(soccerTpl, "p-1x2-val", pt)
            If isAutoCS And isAutoTG And isAutoMP And isAutoOR Then
                TemplateHandler.RemoveBlock(soccerTpl, "limit-Copy1x2")
            End If
        End If


        'Auto master on member
        If isAutoCS_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM1CS")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoCSMax, mAutoCS))
        TemplateHandler.Assign(soccerTpl, "au-cs-val", ptAuto)
        TemplateHandler.Assign(soccerTpl, "checked-CS", strChecked)
        '---

        If isAutoCS Then
            TemplateHandler.Assign(soccerTpl, "a-cs-val", aCS)
            TemplateHandler.Assign(soccerTpl, "p-cs-val", mCS)

        Else
            TemplateHandler.Assign(soccerTpl, "a-cs-val", aCS)
            TemplateHandler.Assign(pt, "ddl-Id", "listM1CS")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mCSMin, mCSMax, mCS))
            TemplateHandler.Assign(soccerTpl, "p-cs-val", pt)
        End If


        'Auto master on member
        If isAutoTG_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM1TG")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoTGMax, mAutoTG))
        TemplateHandler.Assign(soccerTpl, "au-tg-val", ptAuto)
        TemplateHandler.Assign(soccerTpl, "checked-TG", strChecked)
        '---

        If isAutoTG Then
            TemplateHandler.Assign(soccerTpl, "a-tg-val", aTG)
            TemplateHandler.Assign(soccerTpl, "p-tg-val", mTG)

        Else
            TemplateHandler.Assign(soccerTpl, "a-tg-val", aTG)
            TemplateHandler.Assign(pt, "ddl-Id", "listM1TG")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mTGMin, mTGMax, mTG))
            TemplateHandler.Assign(soccerTpl, "p-tg-val", pt)
        End If


        'Auto master on member
        If isAutoMP_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM1MP")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoMPMax, mAutoMP))
        TemplateHandler.Assign(soccerTpl, "au-mp-val", ptAuto)
        TemplateHandler.Assign(soccerTpl, "checked-MP", strChecked)
        '---

        If isAutoMP Then
            TemplateHandler.Assign(soccerTpl, "a-mp-val", aMP)
            TemplateHandler.Assign(soccerTpl, "p-mp-val", mMP)

        Else
            TemplateHandler.Assign(soccerTpl, "a-mp-val", aMP)
            TemplateHandler.Assign(pt, "ddl-Id", "listM1MP")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMPMin, mMPMax, mMP))
            TemplateHandler.Assign(soccerTpl, "p-mp-val", pt)
        End If


        'Auto master on member
        If isAutoOR_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM1OR")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoORMax, mAutoOR))
        TemplateHandler.Assign(soccerTpl, "au-or-val", ptAuto)
        TemplateHandler.Assign(soccerTpl, "checked-OR", strChecked)
        '---

        If isAutoOR Then
            TemplateHandler.Assign(soccerTpl, "a-or-val", aOR)
            TemplateHandler.Assign(soccerTpl, "p-or-val", mOR)

        Else
            TemplateHandler.Assign(soccerTpl, "a-or-val", aOR)
            TemplateHandler.Assign(pt, "ddl-Id", "listM1OR")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mORMin, mORMax, mOR))
            TemplateHandler.Assign(soccerTpl, "p-or-val", pt)
        End If


        ' Auto master on member
        If isAutoLHdp_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM1HdpL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoLHdpMax, mAutoLHdp))
        TemplateHandler.Assign(soccerTpl, "au-Lhdp-val", ptAuto)
        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoM1HdpL")
        TemplateHandler.Assign(soccerTpl, "au-Lhdp-btn", btnTpl)
        TemplateHandler.Assign(soccerTpl, "checked-LHdp", strChecked)
        '---

        If isAutoLHdp Then
            TemplateHandler.Assign(soccerTpl, "a-Lhdp-val", aLHdp)
            TemplateHandler.Assign(soccerTpl, "p-Lhdp-val", mLHdp)


        Else
            TemplateHandler.Assign(soccerTpl, "a-Lhdp-val", aLHdp)
            TemplateHandler.Assign(pt, "ddl-Id", "listM1HdpL")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLHdpMin, mLHdpMax, mLHdp))
            TemplateHandler.Assign(soccerTpl, "p-Lhdp-val", pt)
        End If


        ' Auto master on member
        If isAutoLOU_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM1OUL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoLOUMax, mAutoLOU))
        TemplateHandler.Assign(soccerTpl, "au-Lou-val", ptAuto)
        TemplateHandler.Assign(soccerTpl, "checked-LOU", strChecked)
        '---

        If isAutoLOU Then
            TemplateHandler.Assign(soccerTpl, "a-Lou-val", aLOU)
            TemplateHandler.Assign(soccerTpl, "p-Lou-val", mLOU)

        Else
            TemplateHandler.Assign(soccerTpl, "a-Lou-val", aLOU)
            TemplateHandler.Assign(pt, "ddl-Id", "listM1OUL")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLOUMin, mLOUMax, mLOU))
            TemplateHandler.Assign(soccerTpl, "p-Lou-val", pt)
        End If


        ' Auto master on member
        If isAutoL1stHdp_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM11stHdpL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoL1stHdpMax, mAutoL1stHdp))
        TemplateHandler.Assign(soccerTpl, "au-L1stHdp-val", ptAuto)
        TemplateHandler.Assign(soccerTpl, "checked-L1stHdp", strChecked)
        '---

        If isAutoL1stHdp Then
            TemplateHandler.Assign(soccerTpl, "a-L1stHdp-val", aL1stHdp)
            TemplateHandler.Assign(soccerTpl, "p-L1stHdp-val", mL1stHdp)

        Else
            TemplateHandler.Assign(soccerTpl, "a-L1stHdp-val", aL1stHdp)
            TemplateHandler.Assign(pt, "ddl-Id", "listM11stHdpL")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mL1stHdpMin, mL1stHdpMax, mL1stHdp))
            TemplateHandler.Assign(soccerTpl, "p-L1stHdp-val", pt)

        End If

        ' Auto master on member
        If isAutoL1stOU_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM11stOUL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoL1stOUMax, mAutoL1stOU))
        TemplateHandler.Assign(soccerTpl, "au-L1stou-val", ptAuto)
        TemplateHandler.Assign(soccerTpl, "checked-L1stOU", strChecked)
        '---

        If isAutoL1stOU Then
            TemplateHandler.Assign(soccerTpl, "a-L1stou-val", aL1stOU)
            TemplateHandler.Assign(soccerTpl, "p-L1stou-val", mL1stOU)

        Else
            TemplateHandler.Assign(soccerTpl, "a-L1stou-val", aL1stOU)
            TemplateHandler.Assign(pt, "ddl-Id", "listM11stOUL")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mL1stOUMin, mL1stOUMax, mL1stOU))
            TemplateHandler.Assign(soccerTpl, "p-L1stou-val", pt)
        End If


        GetDataSoccer = soccerTpl
    End Function
#End Region

#Region "Assign info for  Basketball"
    'this function is used to assign data into template for all sporttype that have 4 bettype
    '(1:Handicap, 3:Over/Under, 2:Odd/Even, 9:Mix Parlay, 20:Money Line)
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (3:Football, 5:Tennis, 10:Golf, 99:OtherSports )
    Function GetDataBasketball(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String, Optional ByVal isMult As Boolean = False) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable

        'assign text for position taking
        GetTitleCommon(tpl)
        'TemplateHandler.RemoveBlock(tpl, "none-Master")
        TemplateHandler.RemoveBlock(tpl, "none-Master")
        If isMult Then
            TemplateHandler.RemoveBlock(tpl, "none-Multi")
            Dim chkUpdateMultiple As String = LoadCheckBoxUpdateMultipe(sportTypeId, Resources.Lang.positiontaking)
            TemplateHandler.Assign(tpl, "ChkUpdatePT", chkUpdateMultiple)
        Else
            TemplateHandler.Assign(tpl, "ChkUpdatePT", String.Empty)
        End If
        TemplateHandler.Assign(tpl, "sportId", sportTypeId)
        TemplateHandler.Assign(tpl, "master-text", Resources.Lang.master)
        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "oe-text", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(tpl, "copy-hdp-col", "5")
        TemplateHandler.Assign(tpl, "title-col", "7")

        'assign value for position taking           
        Dim aHdp, aOU, aOE, aML As Double

        Dim mHdpMin, mOUMin, mOEMin, mMLMin As Double
        Dim mHdp, mOU, mOE, mML As Double
        Dim mHdpMax, mOUMax, mOEMax, mMLMax As Double

        Dim isAutoHdp, isAutoOU, isAutoOE, isAutoML As Double

        Dim mAutoHdp, mAutoOU, mAutoOE, mAutoML As Double
        Dim mAutoHdpMax, mAutoOUMax, mAutoOEMax, mAutoMLMax As Double
        Dim isAutoHdp_mem, isAutoOU_mem, isAutoOE_mem, isAutoML_mem As Double

        mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
        mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
        mOE = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))
        mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

        mAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_1"))
        mAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_3"))
        mAutoOE = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_2"))
        mAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_20"))

        isAutoHdp = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_1"))
        isAutoOU = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_3"))
        isAutoOE = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_2"))
        isAutoML = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_20"))

        hiddenHash.Add("isAuto_V" & sportTypeId & "Hdp", isAutoHdp)
        hiddenHash.Add("isAuto_V" & sportTypeId & "OU", isAutoOU)
        hiddenHash.Add("isAuto_V" & sportTypeId & "OE", isAutoOE)
        hiddenHash.Add("isAuto_V" & sportTypeId & "ML", isAutoML)
        strHidBasket = PositionTaking.AssignMulipleHidden(hiddenHash)


        If Not isMult Then
            aHdp = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
            aOU = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3"))
            aOE = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2"))
            aML = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

            mHdpMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_1"))
            mOUMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_3"))
            mOEMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_2"))
            mMLMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_20"))

            mHdpMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_1"))
            mOUMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_3"))
            mOEMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_2"))
            mMLMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_20"))

            mAutoHdpMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_1"))
            mAutoOUMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_3"))
            mAutoOEMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_2"))
            mAutoMLMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_20"))

            isAutoHdp_mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_1"))
            isAutoOU_mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_3"))
            isAutoOE_mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_2"))
            isAutoML_mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_20"))
        Else
            mHdpMax = mHdp
            mOUMax = mOU
            mOEMax = mOE
            mMLMax = mML

            mAutoHdpMax = mAutoHdp
            mAutoOUMax = mAutoOU
            mAutoOEMax = mAutoOE
            mAutoMLMax = mAutoML
        End If

        Dim btnTpl As String = groupBtn
        If isAutoHdp Then
            TemplateHandler.Assign(tpl, "p-hdp-btn", String.Empty)
            If isAutoHdp_mem = 0 Then
                TemplateHandler.Assign(tpl, "chk-hdp", "disabled")
                isAutoHdp_master_Basketball = True
            End If
        Else
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)
            isAutoHdp_master_Basketball = False
        End If

        'Auto master on member
        Dim ptAuto As String
        Dim strChecked As String
        If isAutoHdp_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoHdpMax, mAutoHdp))
        TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-Hdp", strChecked)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)
        '---

        If isAutoHdp Then
            TemplateHandler.Assign(tpl, "a-hdp-val", aHdp)
            TemplateHandler.Assign(tpl, "p-hdp-val", mHdp)

            'CopyBasket = True
        Else
            TemplateHandler.Assign(tpl, "a-hdp-val", aHdp)
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", pt)
            If isAutoOU And isAutoOE And isAutoML Then
                TemplateHandler.RemoveBlock(tpl, "limit-CopyHdp")
            End If
            'CopyBasket = False
        End If

        'Auto master on member
        If isAutoOU_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "OU")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoOUMax, mAutoOU))
        TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-OU", strChecked)
        '----

        If isAutoOU Then
            TemplateHandler.Assign(tpl, "a-ou-val", aOU)
            TemplateHandler.Assign(tpl, "p-ou-val", mOU)

        Else
            TemplateHandler.Assign(tpl, "a-ou-val", aOU)
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOU))
            TemplateHandler.Assign(tpl, "p-ou-val", pt)
        End If


        'Auto master on member
        If isAutoOE_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "OE")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoOEMax, mAutoOE))
        TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-OE", strChecked)
        '----

        If isAutoOE Then
            TemplateHandler.Assign(tpl, "a-oe-val", aOE)
            TemplateHandler.Assign(tpl, "p-oe-val", mOE)

        Else
            TemplateHandler.Assign(tpl, "a-oe-val", aOE)
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OE")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOE))
            TemplateHandler.Assign(tpl, "p-oe-val", pt)
        End If


        'Auto master on member
        If isAutoML_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "ML")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoMLMax, mAutoML))
        TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-ML", strChecked)
        '----

        If isAutoML Then
            TemplateHandler.Assign(tpl, "a-ml-val", aML)
            TemplateHandler.Assign(tpl, "p-ml-val", mML)

        Else
            TemplateHandler.Assign(tpl, "a-ml-val", aML)
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mML))
            TemplateHandler.Assign(tpl, "p-ml-val", pt)
        End If


        GetDataBasketball = tpl
    End Function
#End Region

#Region "Assign info for Football, Tennis, Golf, OtherSports"
    'this function is used to assign data into template for all sporttype that have 4 bettype
    '(1:Handicap, 3:Over/Under, 2:Odd/Even, 20:Money Line)
    'dr: row contains data get from db
    'sportTypeId: id of sporttype ( 3:Football, 5:Tennis, 10:Golf, 99:OtherSports )
    Function GetData4DDl(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String, Optional ByVal isMult As Boolean = False) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable

        'assign text for position taking
        GetTitleCommon(tpl)
        'TemplateHandler.RemoveBlock(tpl, "none-Master")
        TemplateHandler.RemoveBlock(tpl, "none-Master")
        If isMult Then
            TemplateHandler.RemoveBlock(tpl, "none-Multi")
            Dim chkUpdateMultiple As String = LoadCheckBoxUpdateMultipe(sportTypeId, Resources.Lang.positiontaking)
            TemplateHandler.Assign(tpl, "ChkUpdatePT", chkUpdateMultiple)
        Else
            TemplateHandler.Assign(tpl, "ChkUpdatePT", String.Empty)
        End If
        TemplateHandler.Assign(tpl, "sportId", sportTypeId)
        TemplateHandler.Assign(tpl, "master-text", Resources.Lang.master)
        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "oe-text", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(tpl, "copy-hdp-col", "5")
        TemplateHandler.Assign(tpl, "title-col", "6")

        'assign value for position taking           
        Dim aHdp, aOU, aOE, aML As Double

        Dim mHdpMin, mOUMin, mOEMin, mMLMin As Double
        Dim mHdp, mOU, mOE, mML As Double
        Dim mHdpMax, mOUMax, mOEMax, mMLMax As Double

        Dim isAutoHdp, isAutoOU, isAutoOE, isAutoML As Integer

        Dim mAutoHdp, mAutoOU, mAutoOE, mAutoML As Double
        Dim mAutoHdpMax, mAutoOUMax, mAutoOEMax, mAutoMLMax As Double
        Dim isAutoHdp_mem, isAutoOU_mem, isAutoOE_mem, isAutoML_mem As Double

        mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
        mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
        mOE = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))
        mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

        isAutoHdp = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_1"))
        isAutoOU = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_3"))
        isAutoOE = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_2"))
        isAutoML = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_20"))

        mAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_1"))
        mAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_3"))
        mAutoOE = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_2"))
        mAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_20"))

        hiddenHash.Add("isAuto_V" & sportTypeId & "Hdp", isAutoHdp)
        hiddenHash.Add("isAuto_V" & sportTypeId & "OU", isAutoOU)
        hiddenHash.Add("isAuto_V" & sportTypeId & "OE", isAutoOE)
        hiddenHash.Add("isAuto_V" & sportTypeId & "ML", isAutoML)
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


        If Not isMult Then
            aHdp = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
            aOU = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3"))
            aOE = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2"))
            aML = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

            mHdpMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_1"))
            mOUMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_3"))
            mOEMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_2"))
            mMLMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_20"))

            mHdpMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_1"))
            mOUMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_3"))
            mOEMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_2"))
            mMLMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_20"))

            mAutoHdpMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_1"))
            mAutoOUMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_3"))
            mAutoOEMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_2"))
            mAutoMLMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_20"))

            isAutoHdp_mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_1"))
            isAutoOU_mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_3"))
            isAutoOE_mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_2"))
            isAutoML_mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_20"))
        Else
            mHdpMax = mHdp
            mOUMax = mOU
            mOEMax = mOE
            mMLMax = mML

            mAutoHdpMax = mAutoHdp
            mAutoOUMax = mAutoOU
            mAutoOEMax = mAutoOE
            mAutoMLMax = mAutoML
        End If

        Dim btnTpl As String = groupBtn
        If isAutoHdp Then
            TemplateHandler.Assign(tpl, "p-hdp-btn", String.Empty)
            If isAutoHdp_mem = 0 Then
                TemplateHandler.Assign(tpl, "chk-hdp", "disabled")
            End If
        Else
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)
        End If

        'Auto master on member
        Dim ptAuto As String
        Dim strChecked As String
        If isAutoHdp_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoHdpMax, mAutoHdp))
        TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-Hdp", strChecked)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)
        '---

        If isAutoHdp Then
            TemplateHandler.Assign(tpl, "a-hdp-val", aHdp)
            TemplateHandler.Assign(tpl, "p-hdp-val", mHdp)


        Else
            TemplateHandler.Assign(tpl, "a-hdp-val", aHdp)
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", pt)
            If isAutoOU And isAutoOE And isAutoML Then
                TemplateHandler.RemoveBlock(tpl, "limit-CopyHdp")
            End If
        End If

        'Auto master on member
        If isAutoOU_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "OU")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoOUMax, mAutoOU))
        TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-OU", strChecked)
        '--

        If isAutoOU Then
            TemplateHandler.Assign(tpl, "a-ou-val", aOU)
            TemplateHandler.Assign(tpl, "p-ou-val", mOU)

        Else
            TemplateHandler.Assign(tpl, "a-ou-val", aOU)
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOU))
            TemplateHandler.Assign(tpl, "p-ou-val", pt)
        End If


        'Auto master on member
        If isAutoOE_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "OE")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoOEMax, mAutoOE))
        TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-OE", strChecked)
        '--

        If isAutoOE Then
            TemplateHandler.Assign(tpl, "a-oe-val", aOE)
            TemplateHandler.Assign(tpl, "p-oe-val", mOE)

        Else
            TemplateHandler.Assign(tpl, "a-oe-val", aOE)
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OE")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOE))
            TemplateHandler.Assign(tpl, "p-oe-val", pt)
        End If


        'Auto master on member
        If isAutoML_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "ML")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoMLMax, mAutoML))
        TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-ML", strChecked)
        '--

        If isAutoML Then
            TemplateHandler.Assign(tpl, "a-ml-val", aML)
            TemplateHandler.Assign(tpl, "p-ml-val", mML)

        Else
            TemplateHandler.Assign(tpl, "a-ml-val", aML)
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mML))
            TemplateHandler.Assign(tpl, "p-ml-val", pt)
        End If


        GetData4DDl = tpl
    End Function
#End Region

#Region "Assign info for  Baseball"
    'this function is used to assign data into template for Baseball
    'dr: row contains data get from db
    'sportTypeId: id of sporttype
    'Baseball have only Handicap, Over/Under & Money Line bettype
    Function GetDataBaseball(ByVal dr As DataRow, ByVal sportTypeId As Integer, Optional ByVal isMult As Boolean = False) As String
        Dim pt As String
        Dim tpl As String
        Dim hiddenHash As New Hashtable

        'assign text for position taking
        tpl = TemplateHandler.LoadTemplate("MemberInfo/Edit3Bettype.html")
        TemplateHandler.RemoveBlock(tpl, "none-Master")
        TemplateHandler.RemoveBlock(tpl, "none-Master")
        GetTitleCommon(tpl)
        If isMult Then
            TemplateHandler.RemoveBlock(tpl, "none-Multi")
            Dim chkUpdateMultiple As String = LoadCheckBoxUpdateMultipe(sportTypeId, Resources.Lang.positiontaking)
            TemplateHandler.Assign(tpl, "ChkUpdatePT", chkUpdateMultiple)
        Else
            TemplateHandler.Assign(tpl, "ChkUpdatePT", String.Empty)
        End If
        TemplateHandler.Assign(tpl, "sportId", sportTypeId)
        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "copy-hdp-col", "1")
        TemplateHandler.Assign(tpl, "title-col", "5")
        TemplateHandler.Assign(tpl, "cols-a", "5")

        Dim aHdp, aOU, aML As Double
        Dim mHdpMin, mOUMin, mMLMin As Double
        Dim mHdp, mOU, mML As Double
        Dim mHdpMax, mOUMax, mMLMax As Double

        Dim isAutoHdp, isAutoOU, isAutoML As Integer

        Dim mAutoHdp, mAutoOU, mAutoML As Double
        Dim mAutoHdpMax, mAutoOUMax, mAutoMLMax As Double
        Dim isAutoHdp_mem, isAutoOU_mem, isAutoML_mem As Double


        mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
        mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
        mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

        isAutoHdp = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_1"))
        isAutoOU = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_3"))
        isAutoML = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_20"))

        mAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_1"))
        mAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_3"))
        mAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_20"))

        hiddenHash.Add("isAuto_V" & sportTypeId & "Hdp", isAutoHdp)
        hiddenHash.Add("isAuto_V" & sportTypeId & "OU", isAutoOU)
        hiddenHash.Add("isAuto_V" & sportTypeId & "ML", isAutoML)
        strHidBaseball = PositionTaking.AssignMulipleHidden(hiddenHash)

        If Not isMult Then
            aHdp = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
            aOU = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3"))
            aML = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

            mHdpMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_1"))
            mOUMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_3"))
            mMLMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_20"))

            mHdpMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_1"))
            mOUMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_3"))
            mMLMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_20"))

            mAutoHdpMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_1"))
            mAutoOUMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_3"))
            mAutoMLMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_20"))

            isAutoHdp_mem = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_1"))
            isAutoOU_mem = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_3"))
            isAutoML_mem = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_20"))

        Else
            mHdpMax = mHdp
            mOUMax = mOU
            mMLMax = mML

            mAutoHdpMax = mAutoHdp
            mAutoOUMax = mAutoOU
            mAutoMLMax = mAutoML
        End If
        'assign data for Position taking dropdowns          
        Dim btnTpl As String = groupBtn
        If isAutoHdp Then
            TemplateHandler.Assign(tpl, "p-hdp-btn", String.Empty)
            If isAutoHdp_mem = 0 Then
                TemplateHandler.Assign(tpl, "chk-hdp", "disabled")
            End If
        Else
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)
        End If

        'Auto master on member
        Dim ptAuto As String
        Dim strChecked As String
        If isAutoHdp_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoHdpMax, mAutoHdp))
        TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-Hdp", strChecked)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)
        '---

        If isAutoHdp Then
            TemplateHandler.Assign(tpl, "a-hdp-val", aHdp)
            TemplateHandler.Assign(tpl, "p-hdp-val", mHdp)


        Else
            TemplateHandler.Assign(tpl, "a-hdp-val", aHdp)
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", pt)
            If isAutoOU And isAutoML Then
                TemplateHandler.RemoveBlock(tpl, "limit-CopyHdp")
            End If
        End If

        'Auto master on member
        If isAutoOU_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "OU")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoOUMax, mAutoOU))
        TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-OU", strChecked)
        '---

        If isAutoOU Then
            TemplateHandler.Assign(tpl, "a-ou-val", aOU)
            TemplateHandler.Assign(tpl, "p-ou-val", mOU)
            TemplateHandler.Assign(tpl, "p-ou-btn", String.Empty)
        Else
            TemplateHandler.Assign(tpl, "a-ou-val", aOU)
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOU))
            TemplateHandler.Assign(tpl, "p-ou-val", pt)
            TemplateHandler.Assign(tpl, "p-ou-btn", String.Empty)
        End If


        'Auto master on member
        If isAutoML_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "ML")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoMLMax, mAutoML))
        TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-ML", strChecked)
        '---

        If isAutoML Then
            TemplateHandler.Assign(tpl, "a-ml-val", aML)
            TemplateHandler.Assign(tpl, "p-ml-val", mML)

        Else
            TemplateHandler.Assign(tpl, "a-ml-val", aML)
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mML))
            TemplateHandler.Assign(tpl, "p-ml-val", pt)
        End If


        GetDataBaseball = tpl
    End Function
#End Region

#Region "Assign info for MotoSports"
    'this function is used to assign data into template for MotoSports
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (11: motosports)
    'Motosports have only Handicap & Money Line bettype
    Function GetDataMotoSports(ByVal dr As DataRow, ByVal sportTypeId As Integer, Optional ByVal isMult As Boolean = False) As String
        Dim pt As String
        Dim tpl As String
        Dim hiddenHash As New Hashtable

        'assign text for position taking
        tpl = TemplateHandler.LoadTemplate("MemberInfo/Edit2Bettype.html")
        TemplateHandler.RemoveBlock(tpl, "none-Master")
        TemplateHandler.RemoveBlock(tpl, "none-Master")
        GetTitleCommon(tpl)
        If isMult Then
            TemplateHandler.RemoveBlock(tpl, "none-Multi")
            Dim chkUpdateMultiple As String = LoadCheckBoxUpdateMultipe(sportTypeId, Resources.Lang.positiontaking)
            TemplateHandler.Assign(tpl, "ChkUpdatePT", chkUpdateMultiple)
        Else
            TemplateHandler.Assign(tpl, "ChkUpdatePT", String.Empty)
        End If
        TemplateHandler.Assign(tpl, "copy-hdp-col", "1")
        TemplateHandler.Assign(tpl, "cols-a", "1")
        TemplateHandler.Assign(tpl, "title-col", "4")

        'get data and assign for template Position taking
        Dim aHdp, aML As Double
        Dim mHdpMin, mMLMin As Double
        Dim mHdp, mML As Double
        Dim mHdpMax, mMLMax As Double

        Dim isAutoHdp, isAutoML As Integer

        Dim mAutoHdp, mAutoML As Double
        Dim mAutoHdpMax, mAutoMLMax As Double
        Dim isAutoHdp_mem, isAutoML_mem As Double

        mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
        mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

        isAutoHdp = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_1"))
        isAutoML = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_20"))

        mAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_1"))
        mAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_20"))

        hiddenHash.Add("isAuto_V" & sportTypeId & "Hdp", isAutoHdp)
        hiddenHash.Add("isAuto_V" & sportTypeId & "ML", isAutoML)
        strHidMoto = PositionTaking.AssignMulipleHidden(hiddenHash)

        If Not isMult Then
            aHdp = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
            aML = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

            mHdpMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_1"))
            mMLMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_20"))

            mHdpMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_1"))
            mMLMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_20"))

            mAutoHdpMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_1"))
            mAutoMLMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_20"))

            isAutoHdp_mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_1"))
            isAutoML_mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_20"))
        Else
            mHdpMax = mHdp
            mMLMax = mML

            mAutoHdpMax = mAutoHdp
            mAutoMLMax = mAutoML
        End If
        'assign data for Position taking dropdowns     
        Dim btnTpl As String = groupBtn
        If isAutoHdp Then
            TemplateHandler.Assign(tpl, "p-hdp-btn", String.Empty)
            If isAutoHdp_mem = 0 Then
                TemplateHandler.Assign(tpl, "chk-hdp", "disabled")
            End If
        Else
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)
        End If


        'Auto master on member
        Dim ptAuto As String
        Dim strChecked As String
        If isAutoHdp_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoHdpMax, mAutoHdp))
        TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-Hdp", strChecked)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)
        '---

        If isAutoHdp Then
            TemplateHandler.Assign(tpl, "a-hdp-val", aHdp)
            TemplateHandler.Assign(tpl, "p-hdp-val", mHdp)


        Else
            TemplateHandler.Assign(tpl, "a-hdp-val", aHdp)
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", pt)
            If isAutoML Then
                TemplateHandler.RemoveBlock(tpl, "limit-CopyHdp")
            End If
        End If

        'Auto master on member
        If isAutoML_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "ML")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoMLMax, mAutoML))
        TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-ML", strChecked)
        '----

        If isAutoML Then
            TemplateHandler.Assign(tpl, "a-ml-val", aML)
            TemplateHandler.Assign(tpl, "p-ml-val", mML)

        Else
            TemplateHandler.Assign(tpl, "a-ml-val", aML)
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mML))
            TemplateHandler.Assign(tpl, "p-ml-val", pt)
        End If


        GetDataMotoSports = tpl
    End Function
#End Region

#Region "Assign Info for Number Game"
    'this function is used to assign data into template for Soccer
    'dr: row contains data get from db
    Function GetDataNumberGame(ByVal dr As DataRow, Optional ByVal isMult As Boolean = False) As String
        Dim numbergameTpl As String = String.Empty
        Dim headerSTpl As String = String.Empty
        Dim hiddenHash As New Hashtable

        numbergameTpl = TemplateHandler.LoadTemplate("MemberInfo/NumberGameEditMemberPT.html")

        TemplateHandler.RemoveBlock(numbergameTpl, "none-Master")
        If isMult Then
            If CastAide.AsBoolean(dr("disableautompt"), False) Then
                TemplateHandler.Assign(numbergameTpl, "clssHidden", "hidden")
                TemplateHandler.Assign(numbergameTpl, "dead-row", "1")
                TemplateHandler.Assign(numbergameTpl, "live-row", "1")
                TemplateHandler.Assign(numbergameTpl, "copy-live-col", "1")
            Else
                TemplateHandler.Assign(numbergameTpl, "clssHidden", "")
                TemplateHandler.Assign(numbergameTpl, "dead-row", "3")
                TemplateHandler.Assign(numbergameTpl, "live-row", "3")
                TemplateHandler.Assign(numbergameTpl, "copy-live-col", "2")
            End If

            Dim strCopy1stOU As String = TemplateHandler.CopyBlock(numbergameTpl, "limit-Copy1stOU")
            Dim strCopyOU As String = TemplateHandler.CopyBlock(numbergameTpl, "limit-CopyOUL")
            TemplateHandler.ReplaceBlock(numbergameTpl, "Copy1stOU", strCopy1stOU)
            TemplateHandler.ReplaceBlock(numbergameTpl, "CopyOUL", strCopyOU)

            TemplateHandler.RemoveBlock(numbergameTpl, "none-Multi")
            TemplateHandler.RemoveBlock(numbergameTpl, "none-Multi")

            Dim chkUpdateMultiple As String = LoadCheckBoxUpdateMultipe("161", Resources.Lang.positiontaking)
            TemplateHandler.Assign(numbergameTpl, "ChkUpdatePT", chkUpdateMultiple)
        Else
            If CastAide.AsBoolean(dr("disableautompt"), False) Then
                TemplateHandler.Assign(numbergameTpl, "clssHidden", "hidden")
                TemplateHandler.Assign(numbergameTpl, "dead-row", "2")
                TemplateHandler.Assign(numbergameTpl, "live-row", "2")
                TemplateHandler.Assign(numbergameTpl, "copy-live-col", "2")
            Else
                TemplateHandler.Assign(numbergameTpl, "clssHidden", "")
                TemplateHandler.Assign(numbergameTpl, "dead-row", "4")
                TemplateHandler.Assign(numbergameTpl, "live-row", "4")
                TemplateHandler.Assign(numbergameTpl, "copy-live-col", "2")
            End If
            TemplateHandler.Assign(numbergameTpl, "ChkUpdatePT", String.Empty)
        End If
        TemplateHandler.Assign(numbergameTpl, "col-title", "2")
        GetTitleCommon(numbergameTpl)

        TemplateHandler.Assign(numbergameTpl, "deadball-text", Resources.Lang.lblDeadball)
        TemplateHandler.Assign(numbergameTpl, "live-text", Resources.Lang.lblLive)
        TemplateHandler.Assign(numbergameTpl, "p-preset-live", Resources.Lang.master)
        TemplateHandler.Assign(numbergameTpl, "a-preset-live", Resources.Lang.agent)
        TemplateHandler.Assign(numbergameTpl, "cols-live", "3")

        TemplateHandler.Assign(numbergameTpl, "copy-1stou-text", Resources.Lang.lbl1stcopyfromoverunder)
        TemplateHandler.Assign(numbergameTpl, "copy-ouL-text", Resources.Lang.lblcopyfromoverunder)
        TemplateHandler.Assign(numbergameTpl, "1stlastou-text", Resources.Lang.firstlastoverunder)
        TemplateHandler.Assign(numbergameTpl, "1stlastoe-text", Resources.Lang.firstlastoddeven)
        TemplateHandler.Assign(numbergameTpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(numbergameTpl, "oe-text", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(numbergameTpl, "hl-text", Resources.Lang.highlow)

        ' insert by david.tran on 27/7/2011
        TemplateHandler.Assign(numbergameTpl, "cb-text", Resources.Lang.nextcombo)
        TemplateHandler.Assign(numbergameTpl, "w-text", Resources.Lang.warrior)
        TemplateHandler.Assign(numbergameTpl, "nb-text", Resources.Lang.numberGroup)

        If (Not UserPermission.IsShowNumberGroup) Then
            TemplateHandler.Assign(numbergameTpl, "hide", "hide")
        End If
        'assign value of Minbet, Maxbet , ...           

        Dim a1stOU, a1stOE, aOE, aWR As Double
        Dim aLOU, aLOE, aLHL, aLNC, aLNG As Double

        Dim m1stOUMin, m1stOEMin, mOEMin, mWRMin As Double
        Dim mLOUMin, mLOEMin, mLHLMin, mLNCMin, mLNGMin As Double

        Dim m1stOU, m1stOE, mOE, mWR As Double
        Dim mLOU, mLOE, mLHL, mLNC, mLNG As Double

        Dim m1stOUMax, m1stOEMax, mOEMax, mWRMax As Double
        Dim mLOUMax, mLOEMax, mLHLMax, mLNCMax, mLNGMax As Double

        Dim isAuto1stOU, isAuto1stOE, isAutoOE, isAutoWR As Integer
        Dim isAutoLOU, isAutoLOE, isAutoLHL, isAutoLNC, isAutoLNG As Integer

        Dim isAuto1stOU_mem, isAuto1stOE_mem, isAutoOE_mem, isAutoWR_mem As Integer
        Dim isAutoLOU_mem, isAutoLOE_mem, isAutoLHL_mem, isAutoLNC_mem, isAutoLNG_mem As Integer

        Dim mAuto1stOU, mAuto1stOE, mAutoOE, mAutoWR As Double
        Dim mAutoLOU, mAutoLOE, mAutoLHL, mAutoLNC, mAutoLNG As Double

        Dim mAuto1stOUMax, mAuto1stOEMax, mAutoOEMax, mAutoWRMax As Double
        Dim mAutoLOUMax, mAutoLOEMax, mAutoLHLMax, mAutoLNCMax, mAutoLNGMax As Double

        m1stOU = CastAide.AsDouble(dr("v161mpositiontaking_81"))
        m1stOE = CastAide.AsDouble(dr("v161mpositiontaking_83"))
        mOE = CastAide.AsDouble(dr("v161mpositiontaking_86"))
        mWR = CastAide.AsDouble(dr("v161mpositiontaking_88"))

        mLOU = CastAide.AsDouble(dr("v161mlivepositiontaking_85"))
        mLOE = CastAide.AsDouble(dr("v161mlivepositiontaking_86"))
        mLHL = CastAide.AsDouble(dr("v161mlivepositiontaking_87"))
        mLNC = CastAide.AsDouble(dr("v161mlivepositiontaking_89"))
        mLNG = CastAide.AsDouble(dr("v161mlivepositiontaking_90"))

        ' IsAuto master
        isAuto1stOU = CastAide.AsInt16(dr("isauto_v161mpositiontaking_81"))
        isAuto1stOE = CastAide.AsInt16(dr("isauto_v161mpositiontaking_83"))
        isAutoOE = CastAide.AsInt16(dr("isauto_v161mpositiontaking_86"))
        isAutoWR = CastAide.AsInt16(dr("isauto_v161mpositiontaking_88"))

        isAutoLOU = CastAide.AsInt16(dr("isauto_v161mlivepositiontaking_85"))
        isAutoLOE = CastAide.AsInt16(dr("isauto_v161mlivepositiontaking_86"))
        isAutoLHL = CastAide.AsInt16(dr("isauto_v161mlivepositiontaking_87"))
        isAutoLNC = CastAide.AsInt16(dr("isauto_v161mlivepositiontaking_89"))
        isAutoLNG = CastAide.AsInt16(dr("isauto_v161mlivepositiontaking_90"))

        '--

        ' Auto master on member current value
        mAuto1stOU = CastAide.AsDouble(dr("auto_v161mpositiontaking_mem_81"))
        mAuto1stOE = CastAide.AsDouble(dr("auto_v161mpositiontaking_mem_83"))
        mAutoOE = CastAide.AsDouble(dr("auto_v161mpositiontaking_mem_86"))
        mAutoWR = CastAide.AsDouble(dr("auto_v161mpositiontaking_mem_88"))

        mAutoLOU = CastAide.AsDouble(dr("auto_v161mlivepositiontaking_mem_85"))
        mAutoLOE = CastAide.AsDouble(dr("auto_v161mlivepositiontaking_mem_86"))
        mAutoLHL = CastAide.AsDouble(dr("auto_v161mlivepositiontaking_mem_87"))
        mAutoLNC = CastAide.AsDouble(dr("auto_v161mlivepositiontaking_mem_89"))
        mAutoLNG = CastAide.AsDouble(dr("auto_v161mlivepositiontaking_mem_90"))

        '----

        hiddenHash.Add("isAuto_V1611stOUD", isAuto1stOU)
        hiddenHash.Add("isAuto_V1611stOED", isAuto1stOE)
        hiddenHash.Add("isAuto_V161OED", isAutoOE)
        hiddenHash.Add("isAuto_V161WRD", isAutoWR)

        hiddenHash.Add("isAuto_V161OUL", isAutoLOU)
        hiddenHash.Add("isAuto_V161OEL", isAutoLOE)
        hiddenHash.Add("isAuto_V161HLL", isAutoLHL)
        hiddenHash.Add("isAuto_V161NCL", isAutoLNC)
        hiddenHash.Add("isAuto_V161NGL", isAutoLNG)

        strHidNumberGame = PositionTaking.AssignMulipleHidden(hiddenHash)

        If Not isMult Then 'this case for Single Edit              
            a1stOU = CastAide.AsDouble(dr("v161apositiontaking_81"))
            a1stOE = CastAide.AsDouble(dr("v161apositiontaking_83"))
            aOE = CastAide.AsDouble(dr("v161apositiontaking_86"))
            aWR = CastAide.AsDouble(dr("v161apositiontaking_88"))

            aLOU = CastAide.AsDouble(dr("v161alivepositiontaking_85"))
            aLOE = CastAide.AsDouble(dr("v161alivepositiontaking_86"))
            aLHL = CastAide.AsDouble(dr("v161alivepositiontaking_87"))
            aLNC = CastAide.AsDouble(dr("v161alivepositiontaking_89"))
            aLNG = CastAide.AsDouble(dr("v161alivepositiontaking_90"))

            m1stOUMin = CastAide.AsDouble(dr("minv161pos_81"))
            m1stOEMin = CastAide.AsDouble(dr("minv161pos_83"))
            mOEMin = CastAide.AsDouble(dr("minv161pos_86"))
            mWRMin = CastAide.AsDouble(dr("minv161pos_88"))

            mLOUMin = CastAide.AsDouble(dr("minv161lpos_85"))
            mLOEMin = CastAide.AsDouble(dr("minv161lpos_86"))
            mLHLMin = CastAide.AsDouble(dr("minv161lpos_87"))
            mLNCMin = CastAide.AsDouble(dr("minv161lpos_89"))
            mLNGMin = CastAide.AsDouble(dr("minv161lpos_90"))

            m1stOUMax = CastAide.AsDouble(dr("maxv161pos_81"))
            m1stOEMax = CastAide.AsDouble(dr("maxv161pos_83"))
            mOEMax = CastAide.AsDouble(dr("maxv161pos_86"))
            mWRMax = CastAide.AsDouble(dr("maxv161pos_88"))

            mLOUMax = CastAide.AsDouble(dr("maxv161lpos_85"))
            mLOEMax = CastAide.AsDouble(dr("maxv161lpos_86"))
            mLHLMax = CastAide.AsDouble(dr("maxv161lpos_87"))
            mLNCMax = CastAide.AsDouble(dr("maxv161lpos_89"))
            mLNGMax = CastAide.AsDouble(dr("maxv161lpos_90"))

            ' Auto master max value
            mAuto1stOUMax = CastAide.AsDouble(dr("maxauto_v161mpositiontaking_mem_81"))
            mAuto1stOEMax = CastAide.AsDouble(dr("maxauto_v161mpositiontaking_mem_83"))
            mAutoOEMax = CastAide.AsDouble(dr("maxauto_v161mpositiontaking_mem_86"))
            mAutoWRMax = CastAide.AsDouble(dr("maxauto_v161mpositiontaking_mem_88"))

            mAutoLOUMax = CastAide.AsDouble(dr("maxauto_v161mlivepositiontaking_mem_85"))
            mAutoLOEMax = CastAide.AsDouble(dr("maxauto_v161mlivepositiontaking_mem_86"))
            mAutoLHLMax = CastAide.AsDouble(dr("maxauto_v161mlivepositiontaking_mem_87"))
            mAutoLNCMax = CastAide.AsDouble(dr("maxauto_v161mlivepositiontaking_mem_89"))
            mAutoLNGMax = CastAide.AsDouble(dr("maxauto_v161mlivepositiontaking_mem_90"))
            '---

            ' IsAuto master on member
            isAuto1stOU_mem = CastAide.AsInt16(dr("isauto_v161mpositiontaking_mem_81"))
            isAuto1stOE_mem = CastAide.AsInt16(dr("isauto_v161mpositiontaking_mem_83"))
            isAutoOE_mem = CastAide.AsInt16(dr("isauto_v161mpositiontaking_mem_86"))
            isAutoWR_mem = CastAide.AsInt16(dr("isauto_v161mpositiontaking_mem_88"))

            isAutoLOU_mem = CastAide.AsInt16(dr("isauto_v161mlivepositiontaking_mem_85"))
            isAutoLOE_mem = CastAide.AsInt16(dr("isauto_v161mlivepositiontaking_mem_86"))
            isAutoLHL_mem = CastAide.AsInt16(dr("isauto_v161mlivepositiontaking_mem_87"))
            isAutoLNC_mem = CastAide.AsInt16(dr("isauto_v161mlivepositiontaking_mem_89"))
            isAutoLNG_mem = CastAide.AsInt16(dr("isauto_v161mlivepositiontaking_mem_90"))

            '--
        Else 'this case for Multiple Edit
            m1stOUMax = m1stOU
            m1stOEMax = m1stOE
            mOEMax = mOE
            mWRMax = mWR

            mLOUMax = mLOU
            mLOEMax = mLOE
            mLHLMax = mLHL
            mLNCMax = mLNC
            mLNGMax = mLNG

            'Auto master max value
            mAuto1stOUMax = mAuto1stOU
            mAuto1stOEMax = mAuto1stOE
            mAutoOEMax = mAutoOE
            mAutoWRMax = mAutoWR

            mAutoLOUMax = mAutoLOU
            mAutoLOEMax = mAutoLOE
            mAutoLHLMax = mAutoLHL
            mAutoLNCMax = mAutoLNC
            mAutoLNGMax = mAutoLNG
            '---

        End If

        '    'assign Increase and Reduce button for template
        Dim btnTpl As String = groupBtn
        If isAuto1stOU Then
            TemplateHandler.Assign(numbergameTpl, "p-1stou-btn", String.Empty)
            If isAuto1stOU_mem = 0 Then
                TemplateHandler.Assign(numbergameTpl, "chk-1stoud", "disabled")

            End If
        Else
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM1611stOUD")
            TemplateHandler.Assign(numbergameTpl, "p-1stou-btn", btnTpl)

        End If

        btnTpl = groupBtn
        If isAutoLOU Then
            TemplateHandler.Assign(numbergameTpl, "p-Lou-btn", String.Empty)
            If isAutoLOU_mem = 0 Then
                TemplateHandler.Assign(numbergameTpl, "chk-oul", "disabled")
            End If
        Else
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM161OUL")
            TemplateHandler.Assign(numbergameTpl, "p-Lou-btn", btnTpl)
        End If

        ' Auto master on member
        Dim ptAuto As String
        Dim strChecked As String
        Dim pt As String = selectTpl
        If isAuto1stOU_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM1611stOUD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAuto1stOUMax, mAuto1stOU))
        TemplateHandler.Assign(numbergameTpl, "au-1stou-val", ptAuto)
        TemplateHandler.Assign(numbergameTpl, "checked-1stOUD", strChecked)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoM1611stOUD")
        TemplateHandler.Assign(numbergameTpl, "au-1stou-btn", btnTpl)
        '---

        If isAuto1stOU Then
            TemplateHandler.Assign(numbergameTpl, "a-1stou-val", a1stOU)
            TemplateHandler.Assign(numbergameTpl, "p-1stou-val", m1stOU)
        Else
            TemplateHandler.Assign(numbergameTpl, "a-1stou-val", a1stOU)
            TemplateHandler.Assign(pt, "ddl-Id", "listM1611stOUD")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1stOUMin, m1stOUMax, m1stOU))
            TemplateHandler.Assign(numbergameTpl, "p-1stou-val", pt)
            If isAuto1stOU And isAuto1stOE And isAutoOE Then
                TemplateHandler.RemoveBlock(numbergameTpl, "limit-Copy1stOU")
            End If

        End If

        'Auto master on member
        If isAuto1stOE_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM1611stOED")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAuto1stOEMax, mAuto1stOE))
        TemplateHandler.Assign(numbergameTpl, "au-1stoe-val", ptAuto)
        TemplateHandler.Assign(numbergameTpl, "checked-1stOED", strChecked)
        '---

        If isAuto1stOE Then
            TemplateHandler.Assign(numbergameTpl, "a-1stoe-val", a1stOE)
            TemplateHandler.Assign(numbergameTpl, "p-1stoe-val", m1stOE)

        Else
            TemplateHandler.Assign(numbergameTpl, "a-1stoe-val", a1stOE)
            TemplateHandler.Assign(pt, "ddl-Id", "listM1611stOED")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1stOEMin, m1stOEMax, m1stOE))
            TemplateHandler.Assign(numbergameTpl, "p-1stoe-val", pt)
        End If

        'Auto master on member
        If isAutoOE_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM161OED")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoOEMax, mAutoOE))
        TemplateHandler.Assign(numbergameTpl, "au-oe-val", ptAuto)
        TemplateHandler.Assign(numbergameTpl, "checked-OED", strChecked)
        '---

        If isAutoOE Then
            TemplateHandler.Assign(numbergameTpl, "a-oe-val", aOE)
            TemplateHandler.Assign(numbergameTpl, "p-oe-val", mOE)

        Else
            TemplateHandler.Assign(numbergameTpl, "a-oe-val", aOE)
            TemplateHandler.Assign(pt, "ddl-Id", "listM161OED")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOE))
            TemplateHandler.Assign(numbergameTpl, "p-oe-val", pt)
        End If

        'Auto master on member
        If isAutoWR_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM161WRD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoWRMax, mAutoWR))
        TemplateHandler.Assign(numbergameTpl, "au-warrior-val", ptAuto)
        TemplateHandler.Assign(numbergameTpl, "checked-WRD", strChecked)
        '---

        If isAutoWR Then
            TemplateHandler.Assign(numbergameTpl, "a-warrior-val", aWR)
            TemplateHandler.Assign(numbergameTpl, "p-warrior-val", mWR)

        Else
            TemplateHandler.Assign(numbergameTpl, "a-warrior-val", aWR)
            TemplateHandler.Assign(pt, "ddl-Id", "listM161WRD")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mWRMin, mWRMax, mWR))
            TemplateHandler.Assign(numbergameTpl, "p-warrior-val", pt)
        End If


        ' Auto master on member
        If isAutoLOU_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM161OUL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoLOUMax, mAutoLOU))
        TemplateHandler.Assign(numbergameTpl, "au-Lou-val", ptAuto)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoM161OUL")
        TemplateHandler.Assign(numbergameTpl, "au-Lou-btn", btnTpl)
        TemplateHandler.Assign(numbergameTpl, "checked-LOU", strChecked)
        '---

        If isAutoLOU Then
            TemplateHandler.Assign(numbergameTpl, "a-Lou-val", aLOU)
            TemplateHandler.Assign(numbergameTpl, "p-Lou-val", mLOU)
        Else
            TemplateHandler.Assign(numbergameTpl, "a-Lou-val", aLOU)
            TemplateHandler.Assign(pt, "ddl-Id", "listM161OUL")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLOUMin, mLOUMax, mLOU))
            TemplateHandler.Assign(numbergameTpl, "p-Lou-val", pt)

            '----- 
            If isAutoLOU And isAutoLOE And isAutoLHL Then
                TemplateHandler.RemoveBlock(numbergameTpl, "limit-CopyOUL")
            End If
        End If


        ' Auto master on member
        If isAutoLOE_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM161OEL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoLOEMax, mAutoLOE))
        TemplateHandler.Assign(numbergameTpl, "au-Loe-val", ptAuto)
        TemplateHandler.Assign(numbergameTpl, "checked-LOE", strChecked)
        '---

        If isAutoLOE Then
            TemplateHandler.Assign(numbergameTpl, "a-Loe-val", aLOE)
            TemplateHandler.Assign(numbergameTpl, "p-Loe-val", mLOE)

        Else
            TemplateHandler.Assign(numbergameTpl, "a-Loe-val", aLOE)
            TemplateHandler.Assign(pt, "ddl-Id", "listM161OEL")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLOEMin, mLOEMax, mLOE))
            TemplateHandler.Assign(numbergameTpl, "p-Loe-val", pt)
        End If


        ' Auto master on member
        If isAutoLHL_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM161HLL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoLHLMax, mAutoLHL))
        TemplateHandler.Assign(numbergameTpl, "au-Lhl-val", ptAuto)
        TemplateHandler.Assign(numbergameTpl, "checked-LHL", strChecked)
        '---

        If isAutoLHL Then
            TemplateHandler.Assign(numbergameTpl, "a-Lhl-val", aLHL)
            TemplateHandler.Assign(numbergameTpl, "p-Lhl-val", mLHL)

        Else
            TemplateHandler.Assign(numbergameTpl, "a-Lhl-val", aLHL)
            TemplateHandler.Assign(pt, "ddl-Id", "listM161HLL")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLHLMin, mLHLMax, mLHL))
            TemplateHandler.Assign(numbergameTpl, "p-Lhl-val", pt)

        End If

        ' Auto master on member
        If isAutoLNC_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM161NCL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoLNCMax, mAutoLNC))
        TemplateHandler.Assign(numbergameTpl, "au-Lcb-val", ptAuto)
        TemplateHandler.Assign(numbergameTpl, "checked-NCL", strChecked)
        '---

        If isAutoLNC Then
            TemplateHandler.Assign(numbergameTpl, "a-Lcb-val", aLNC)
            TemplateHandler.Assign(numbergameTpl, "p-Lcb-val", mLNC)

        Else
            TemplateHandler.Assign(numbergameTpl, "a-Lcb-val", aLNC)
            TemplateHandler.Assign(pt, "ddl-Id", "listM161NCL")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLNCMin, mLNCMax, mLNC))
            TemplateHandler.Assign(numbergameTpl, "p-Lcb-val", pt)

        End If

        ' Auto master on member
        If isAutoLNG_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM161NGL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoLNGMax, mAutoLNG))
        TemplateHandler.Assign(numbergameTpl, "au-NumGr-val", ptAuto)
        TemplateHandler.Assign(numbergameTpl, "checked-NGL", strChecked)
        '---

        If isAutoLNG Then
            TemplateHandler.Assign(numbergameTpl, "a-NumGr-val", aLNG)
            TemplateHandler.Assign(numbergameTpl, "p-NumGr-val", mLNG)

        Else
            TemplateHandler.Assign(numbergameTpl, "a-NumGr-val", aLNG)
            TemplateHandler.Assign(pt, "ddl-Id", "listM161NGL")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLNGMin, mLNGMax, mLNG))
            TemplateHandler.Assign(numbergameTpl, "p-NumGr-val", pt)

        End If

        Return numbergameTpl
    End Function
#End Region

#Region "Assign info for Mixed Sports"

    Function GetDataMixedSports(ByVal dr As DataRow, Optional ByVal isMult As Boolean = False) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable
        Dim tpl As String = TemplateHandler.LoadTemplate("MemberInfo/MixSportsEditMinPT.html")

        'assign text for position taking
        GetTitleCommon(tpl)
        'TemplateHandler.RemoveBlock(tpl, "none-Master")
        TemplateHandler.RemoveBlock(tpl, "none-Master")
        If isMult Then
            TemplateHandler.RemoveBlock(tpl, "none-Multi")
            Dim chkUpdateMultiple As String = LoadCheckBoxUpdateMultipe("99_M", Resources.Lang.positiontaking)
            TemplateHandler.Assign(tpl, "ChkUpdatePT", chkUpdateMultiple)
        Else
            TemplateHandler.Assign(tpl, "ChkUpdatePT", String.Empty)
        End If

        TemplateHandler.Assign(tpl, "master-text", Resources.Lang.master)
        TemplateHandler.Assign(tpl, "mp-text", Resources.Lang.lblMixParlay)
        TemplateHandler.Assign(tpl, "title-col", "6")

        'assign value for position taking           
        Dim aMP As Double

        Dim mMPMin As Double
        Dim mMP As Double
        Dim mMPMax As Double

        Dim isAutoMP As Integer

        Dim mAutoMP As Double
        Dim mAutoMPMax As Double
        Dim isAutoMP_mem As Double

        mMP = CastAide.AsDouble(dr("v99mpositiontaking_9"))
        isAutoMP = CastAide.AsInt16(dr("isauto_v99mpositiontaking_9"))
        mAutoMP = CastAide.AsDouble(dr("auto_v99mpositiontaking_mem_9"))

        hiddenHash.Add("isAuto_V99_M", isAutoMP)
        strHidMixedSport = PositionTaking.AssignMulipleHidden(hiddenHash)

        If Not isMult Then
            aMP = CastAide.AsDouble(dr("v99apositiontaking_9"))
            mMPMin = CastAide.AsDouble(dr("minv99pos_9"))
            mMPMax = CastAide.AsDouble(dr("maxv99pos_9"))
            mAutoMPMax = CastAide.AsDouble(dr("maxauto_v99mpositiontaking_mem_9"))
            isAutoMP_mem = CastAide.AsInt16(dr("isauto_v99mpositiontaking_mem_9"))

        Else
            mMPMax = mMP
            mAutoMPMax = mAutoMP
        End If

        Dim btnTpl As String = groupBtn
        If isAutoMP Then
            TemplateHandler.Assign(tpl, "p-mp-btn", String.Empty)
            If isAutoMP_mem = 0 Then
                TemplateHandler.Assign(tpl, "chk-MP", "disabled")
            End If
        Else
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM99_M")
            TemplateHandler.Assign(tpl, "p-mp-btn", btnTpl)
        End If

        ''Auto master on member
        Dim ptAuto As String
        Dim strChecked As String
        If isAutoMP_mem Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM99_M")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoMPMax, mAutoMP))
        TemplateHandler.Assign(tpl, "au-mp-val", ptAuto)
        TemplateHandler.Assign(tpl, "checked-MP", strChecked)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoM99_M")
        TemplateHandler.Assign(tpl, "au-mp-btn", btnTpl)
        ''---

        If isAutoMP Then
            TemplateHandler.Assign(tpl, "a-mp-val", aMP)
            TemplateHandler.Assign(tpl, "p-mp-val", mMP)
        Else
            TemplateHandler.Assign(tpl, "a-mp-val", aMP)
            TemplateHandler.Assign(pt, "ddl-Id", "listM99_M")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMPMin, mMPMax, mMP))
            TemplateHandler.Assign(tpl, "p-mp-val", pt)
        End If


        Return tpl
    End Function
#End Region

#Region "GetTitleMember"
    Sub GetTitleMember(ByRef tpl)
        TemplateHandler.Assign(tpl, "uname-text", Resources.Lang.username)
        TemplateHandler.Assign(tpl, "fname-text", Resources.Lang.firstname)
        TemplateHandler.Assign(tpl, "lname-text", Resources.Lang.lastname)
        TemplateHandler.Assign(tpl, "email-text", Resources.Lang.lblFax)
        TemplateHandler.Assign(tpl, "group-text", Resources.Lang.group)
        TemplateHandler.Assign(tpl, "credit-text", Resources.Lang.credit)
        TemplateHandler.Assign(tpl, "phone-text", Resources.Lang.lblPhone)
        TemplateHandler.Assign(tpl, "mobile-text", Resources.Lang.lblMobilePhone)
        TemplateHandler.Assign(tpl, "status-text", Resources.Lang.status)
        TemplateHandler.Assign(tpl, "closed-text", Resources.Lang.close)

        TemplateHandler.Assign(tpl, "othersetting-text", Resources.Lang.othersetting)
        TemplateHandler.Assign(tpl, "maxperball-text", Resources.Lang.NumberGameMaxperball)

        TemplateHandler.Assign(tpl, "commission-text", Resources.Lang.commission)
        TemplateHandler.Assign(tpl, "a-comm-text", Resources.Lang.lblCommissionForAgent & " <u>" & Resources.Lang.lblAsianHDC_OU_OE & " </u>")
        TemplateHandler.Assign(tpl, "a-commo-text", Resources.Lang.lblCommissionForAgent & " <u>" & Resources.Lang.lblOtherTypes & " </u>")
        TemplateHandler.Assign(tpl, "comm-text", Resources.Lang.lblCommissionForMember & " <u>" & Resources.Lang.lblAsianHDC_OU_OE & " </u>")
        TemplateHandler.Assign(tpl, "commo-text", Resources.Lang.lblCommissionForMember & " <u>" & Resources.Lang.lblOtherTypes & " </u>")

        TemplateHandler.Assign(tpl, "a-comm1x2-text", Resources.Lang.lblCommissionForAgent & " <u>" & Resources.Lang.lbl1x2 & " </u>")
        TemplateHandler.Assign(tpl, "comm1x2-text", Resources.Lang.lblCommissionForMember & " <u>" & Resources.Lang.lbl1x2 & " </u>")
        If (Not UserPermission.IsShowNumberGame Or Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission) Then
            TemplateHandler.Assign(tpl, "a-commng-text", String.Empty)
            TemplateHandler.Assign(tpl, "commng-text", String.Empty)
        Else
            TemplateHandler.Assign(tpl, "a-commng-text", Resources.Lang.lblCommissionForAgent & " <u>" & Resources.Lang.numbergame & " </u>")
            TemplateHandler.Assign(tpl, "commng-text", Resources.Lang.lblCommissionForMember & " <u>" & Resources.Lang.numbergame & " </u>")
        End If





        TemplateHandler.Assign(tpl, "minbet-text", Resources.Lang.lblMinBet)
        TemplateHandler.Assign(tpl, "maxbet-text", Resources.Lang.lblMaxBet)
        TemplateHandler.Assign(tpl, "maxpermatch-text", Resources.Lang.lblMaxPerMatch)
        TemplateHandler.Assign(tpl, "soccer-text", Resources.Lang.soccer)
        TemplateHandler.Assign(tpl, "basket-text", Resources.Lang.Basketball)
        TemplateHandler.Assign(tpl, "football-text", Resources.Lang.Football)
        TemplateHandler.Assign(tpl, "tennis-text", Resources.Lang.Tennis)
        TemplateHandler.Assign(tpl, "baseball-text", Resources.Lang.Baseball)
        TemplateHandler.Assign(tpl, "golf-text", Resources.Lang.Golf)
        TemplateHandler.Assign(tpl, "moto-text", Resources.Lang.MotorSports)
        TemplateHandler.Assign(tpl, "othersports-text", Resources.Lang.OtherSports)
        If Not UserPermission.IsShowNumberGame Then
            TemplateHandler.Assign(tpl, "numbergame-text", Resources.Lang.mixsports)
            TemplateHandler.Assign(tpl, "mixsports-text", String.Empty)
        Else
            TemplateHandler.Assign(tpl, "numbergame-text", Resources.Lang.numbergame)
            TemplateHandler.Assign(tpl, "mixsports-text", Resources.Lang.mixsports)
        End If

    End Sub
#End Region

#Region "Get Title Common"
    Sub GetTitleCommon(ByRef tpl As String)
        TemplateHandler.Assign(tpl, "pt-text", Resources.Lang.lblPositionTaking)
        TemplateHandler.Assign(tpl, "a-presetpt-text", Resources.Lang.agent)
        TemplateHandler.Assign(tpl, "p-presetpt-text", Resources.Lang.master)
        TemplateHandler.Assign(tpl, "au-presetpt-text", Resources.Lang.lblMasterAuto)
        TemplateHandler.Assign(tpl, "au-preset-live", Resources.Lang.lblMasterAuto)
        TemplateHandler.Assign(tpl, "hdp-text", Resources.Lang.lblHandicapFC)
        TemplateHandler.Assign(tpl, "ml-text", Resources.Lang.moneyline)
        TemplateHandler.Assign(tpl, "copy-hdp-text", Resources.Lang.lblCopyFromHandicap)
        TemplateHandler.Assign(tpl, "copy-hdp-col", "1")
        TemplateHandler.Assign(tpl, "cols-a", "1")
    End Sub
#End Region

#Region "ReplaceData"
    'Public Function ReplaceData(ByVal str As String) As String
    '    Return str.Replace("""", "&quot;").Replace("'", "&#39;").Replace(vbNewLine, "")
    'End Function
#End Region

#Region "GetMinMaxEditValue"
    'this function is used to assign value for Min Bet, Max Bet, Max Per Match in Single Editing case
    Shared Sub GetMinMaxEditValue(ByRef tpl As String, ByVal sportTypeId As String, ByVal dr As DataRow)
        TemplateHandler.Assign(tpl, "minbet" & sportTypeId & "-val", DataFormat.FormatNumber1(dr("minbetv" & sportTypeId)))
        TemplateHandler.Assign(tpl, "maxbet" & sportTypeId & "-val", DataFormat.FormatNumber1(dr("maxbetv" & sportTypeId)))
        TemplateHandler.Assign(tpl, "maxpermatch" & sportTypeId & "-val", DataFormat.FormatNumber1(dr("maxpermatchv" & sportTypeId)))
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

#Region "GetListPositionTaking"
    'Shared Function GetListPositionTaking(ByVal minVal As Double, ByVal maxVal As Double, Optional ByVal currentVal As Double = 0) As String
    '    Dim html As String
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

#Region "Declare parameter for Script"
    'declare parameters is used in client script 
    'this function is call in code inline
    Function VariableOfScript() As String
        Dim script As String = ""
        script += "<script language='javascript'>" & vbNewLine
        script += " var confirmUnSuspend=""" & Resources.Lang.confirmUnSuspendMem & """;" & vbNewLine
        script += " var confirmSuspend=""" & Resources.Lang.confirmSuspendMem & """;" & vbNewLine
        script += " var confirmClosed=""" & Resources.Lang.confirmCloseMem & """;" & vbNewLine
        script += " var confirmOpen=""" & Resources.Lang.confirmCloseMem & """;" & vbNewLine
        script += " var alertMaxperMatchNegative ='" & Resources.Lang.lblmaxperballgreaterhanzero & "';" & vbNewLine
        script += "</script>"
        Return script
    End Function
#End Region

#Region "Load Checkbox Update Multiple "
    Function LoadCheckBoxUpdateMultipe(ByVal sportId As String, ByVal text As String) As String
        Dim chkUpatePT As String = TemplateHandler.LoadTemplate("MemberInfo/AllowUpdatePT.htm")
        TemplateHandler.Assign(chkUpatePT, "sportId", sportId)
        TemplateHandler.Assign(chkUpatePT, "chktext", text)
        Return chkUpatePT
    End Function
#End Region

End Class

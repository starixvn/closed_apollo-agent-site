Imports DNAide.Web
Imports DNAide.Core
Imports Fanex.Data.DNAide
Imports DNAide.Utils
Imports System.Text
Imports System.Data
Imports Sunplus.Agent.Model.Entities.Permission
Public Class EditMember
    Inherits IbcPage2
    Public style As String
    Dim bodyTpl, for4ddlTpl, for3ddlTpl, basketballTpl As String
    Dim groupBtn As String
    Dim selectTpl As String
    Public strUser As String
    Protected m As Integer
    Dim strHidSoccer, strHidBasket, strHidFootball, strHidTennis As String
    Dim strHidBaseball, strHidGolf, strHidMoto, strHidOther, strHidNumberGame, strHidMixSports As String
    Dim selDisabledTpl As String
    Dim strContent As String
    Dim isAutoHdp_Super_Soccer As Boolean
    Dim isAutoHdp_Super_Basketball As Boolean
    Public script As String

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
        for4ddlTpl = TemplateHandler.LoadTemplate("MemberInfo/Edit4Bettype.html")
        basketballTpl = TemplateHandler.LoadTemplate("MemberInfo/Edit5Bettype.html")
        selDisabledTpl = TemplateHandler.LoadTemplate("MemberInfo/DdlDisabledMem.html")

        If (String.IsNullOrEmpty(Request.QueryString("nobackground"))) Then
            lnkNobackground.Visible = False
        Else
            'If is from Account Menu, Then Set delay reload page
            Dim hash As Hashtable = AppSession.RequestQueryString
            If (hash Is Nothing) Then
                hash = New Hashtable
            End If
            If (Not hash.ContainsKey("FromAccount")) Then
                hash.Add("FromAccount", "../_MemberInfo/CustomerList/Super/MemberList.aspx")
            Else
                hash("FromAccount") = "../_MemberInfo/CustomerList/Super/MemberList.aspx"
            End If
            AppSession.RequestQueryString = hash
        End If

        If Not Request.QueryString("m") Then
            If Request.QueryString("m") = "0" Then
                m = 0
                saveCustid.Value = Request.QueryString("custid").Replace("^", "")
                strUser = Request.QueryString("USER").Replace("^", "")
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
        hash.Add("agentid", Request("agentid"))
        rs = AccountInfo.Acc_CustomerSelEditCustomer_SAg(hash)
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
                    TemplateHandler.Assign(bodyTpl, "numbergamePT", GetDataNumberGame(row, 161, m))
                Else
                    TemplateHandler.Assign(bodyTpl, "numbergamePT", "")
                End If

                If UserPermission.IsShowMixedSport Then
                    TemplateHandler.Assign(bodyTpl, "mixSportPT", GetDataMixSports(row, m))
                End If



                TemplateHandler.RemoveBlock(bodyTpl, "none-edit-Min")
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

                If isAutoHdp_Super_Soccer Then
                    TemplateHandler.Assign(bodyTpl, "disable_chkall", "disabled")
                Else
                    TemplateHandler.Assign(bodyTpl, "disable_chkall", "")
                End If

                If isAutoHdp_Super_Basketball Then
                    TemplateHandler.Assign(bodyTpl, "disable_chkbasket", "disabled")
                Else
                    TemplateHandler.Assign(bodyTpl, "disable_chkbasket", "")
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
        'Dim cmdPool As CommandPool = Application("CommandPool")
        Dim hash As New Hashtable
        hash.Add("custid", Sunplus.Agent.Model.Entities.UserSession.Current.CustId)
        hash.Add("mrecommend", Request("masterid"))
        rs = AccountInfo.Acc_CustomerSelSAgentInfo_Multiple(hash)
        If Not rs Is Nothing Then
            If rs.ResultSet.Tables(0).Rows.Count > 0 Then
                Dim row As DataRow = rs.ResultSet.Tables(0).Rows(0)
                strEditContent.Append(GetDataInfo(row, m))
                Common.GetSportTitle(bodyTpl, True)
                'TemplateHandler.RemoveBlock(bodyTpl, "none-Mult")
                'TemplateHandler.RemoveBlock(bodyTpl, "none-Mult")
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
                    TemplateHandler.Assign(bodyTpl, "numbergamePT", GetDataNumberGame(row, 161, m))
                Else
                    TemplateHandler.Assign(bodyTpl, "numbergamePT", "")
                End If

                If UserPermission.IsShowMixedSport Then
                    TemplateHandler.Assign(bodyTpl, "mixSportPT", GetDataMixSports(row, m))
                End If

                TemplateHandler.RemoveBlock(bodyTpl, "none-edit-Min")
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

                If isAutoHdp_Super_Soccer Then
                    TemplateHandler.Assign(bodyTpl, "disable_chkall", "disabled")
                Else
                    TemplateHandler.Assign(bodyTpl, "disable_chkall", "")
                End If

                If isAutoHdp_Super_Basketball Then
                    TemplateHandler.Assign(bodyTpl, "disable_chkbasket", "disabled")
                Else
                    TemplateHandler.Assign(bodyTpl, "disable_chkbasket", "")
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
        strAccInfo = Common.ReplaceWithPopupStyle(strAccInfo, 0, 0)
        TemplateHandler.Assign(strAccInfo, "add-text", Resources.Lang.submit)
        TemplateHandler.Assign(strAccInfo, "add-id", "btnSubmit")
        If Not isMult Then
            TemplateHandler.RemoveBlock(strAccInfo, "none-Single")
            TemplateHandler.RemoveBlock(strAccInfo, "none-Super")
            TemplateHandler.RemoveBlock(strAccInfo, "none-Super")
            GetTitleMember(strAccInfo)
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


            TemplateHandler.Assign(strAccInfo, "suspend-text", Resources.Lang.suspended)
            TemplateHandler.Assign(strAccInfo, "ma-comm-val", DataFormat.FormatNumber4(dr("adiscount")))
            TemplateHandler.Assign(strAccInfo, "ma-commo-val", DataFormat.FormatNumber4(dr("adiscountcs")))
            TemplateHandler.Assign(strAccInfo, "ma-comm1x2-val", DataFormat.FormatNumber4(dr("adiscount1x2")))


            TemplateHandler.Assign(strAccInfo, "a-comm-val", DataFormat.FormatNumber4(dr("discount")))
            TemplateHandler.Assign(strAccInfo, "a-commo-val", DataFormat.FormatNumber4(dr("discountcs")))
            TemplateHandler.Assign(strAccInfo, "a-comm1x2-val", DataFormat.FormatNumber4(dr("discount1x2")))


            TemplateHandler.Assign(strAccInfo, "comm-val", DataFormat.FormatNumber4(dr("playerdiscount")))
            TemplateHandler.Assign(strAccInfo, "commo-val", DataFormat.FormatNumber4(dr("playerdiscountcs")))
            TemplateHandler.Assign(strAccInfo, "comm1x2-val", DataFormat.FormatNumber4(dr("playerdiscount1x2")))

            If UserPermission.IsShowNumberGame Then
                TemplateHandler.Assign(strAccInfo, "a-commng-val", IIf(Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission, String.Empty, DataFormat.FormatNumber4(dr("discount161"))))
                TemplateHandler.Assign(strAccInfo, "ma-commng-val", IIf(Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission, String.Empty, DataFormat.FormatNumber4(dr("adiscount161"))))
                TemplateHandler.Assign(strAccInfo, "commng-val", IIf(Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission, String.Empty, DataFormat.FormatNumber4(dr("playerdiscount161"))))
                TemplateHandler.Assign(strAccInfo, "hideClss", String.Empty)
            Else
                TemplateHandler.Assign(strAccInfo, "a-commng-val", String.Empty)
                TemplateHandler.Assign(strAccInfo, "ma-commng-val", String.Empty)
                TemplateHandler.Assign(strAccInfo, "commng-val", String.Empty)
                TemplateHandler.Assign(strAccInfo, "hideClss", "hidden")
            End If

            TemplateHandler.Assign(strAccInfo, "maxperball-value", DataFormat.FormatNumber1(dr("maxperballv161")))

            TemplateHandler.Assign(strAccInfo, "minbet-val", DataFormat.FormatNumber1(dr("minbet")))
            TemplateHandler.Assign(strAccInfo, "maxbet-val", DataFormat.FormatNumber1(dr("maxbet")))
            TemplateHandler.Assign(strAccInfo, "maxpermatch-val", DataFormat.FormatNumber1(dr("maxpermatch")))
            GetMinMaxEditValue(strAccInfo, 2, dr)
            GetMinMaxEditValue(strAccInfo, 3, dr)
            GetMinMaxEditValue(strAccInfo, 5, dr)
            GetMinMaxEditValue(strAccInfo, 8, dr)
            GetMinMaxEditValue(strAccInfo, 10, dr)
            GetMinMaxEditValue(strAccInfo, 11, dr)
            GetMinMaxEditValue(strAccInfo, 99, dr)

            If UserPermission.IsShowNumberGame Then
                GetMinMaxEditValue(strAccInfo, 161, dr)
            Else
                TemplateHandler.Assign(strAccInfo, "minbet161-val", String.Empty)
                TemplateHandler.Assign(strAccInfo, "maxbet161-val", String.Empty)
                TemplateHandler.Assign(strAccInfo, "maxpermatch161-val", String.Empty)
            End If

            If UserPermission.IsShowMixedSport Then
                GetMinMaxEditValue(strAccInfo, "99MP", dr)
            Else
                TemplateHandler.Assign(strAccInfo, "minbet99MP-val", String.Empty)
                TemplateHandler.Assign(strAccInfo, "maxbet99MP-val", String.Empty)
                TemplateHandler.Assign(strAccInfo, "maxpermatch99MP-val", String.Empty)
            End If


        Else
            TemplateHandler.RemoveBlock(strAccInfo, "none-Mult")
            TemplateHandler.Assign(strAccInfo, "check-val", "")
            TemplateHandler.Assign(strAccInfo, "status-text", Resources.Lang.status)
            TemplateHandler.Assign(strAccInfo, "closed-text", Resources.Lang.close)
            TemplateHandler.Assign(strAccInfo, "title", Resources.Lang.lblEditMultiple)
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
        Dim chkUpatePT As String = String.Empty

        Dim hiddenHash As New Hashtable

        soccerTpl = TemplateHandler.LoadTemplate("MemberInfo/SoccerEditMPT.html")
        headerSTpl = TemplateHandler.LoadTemplate("MemberInfo/SBettypeHeader.html")

       

        If isMult Then
            TemplateHandler.RemoveBlock(soccerTpl, "none-Multi")
            TemplateHandler.RemoveBlock(soccerTpl, "none-Multi")
            TemplateHandler.Assign(soccerTpl, "dead-row", "4")
            TemplateHandler.Assign(soccerTpl, "live-row", "3")
            Dim chkUpdateMultiple As String = LoadCheckBoxUpdateMultipe("1", Resources.Lang.positiontaking)
            TemplateHandler.Assign(soccerTpl, "ChkUpdatePT", chkUpdateMultiple)
        Else
            TemplateHandler.Assign(soccerTpl, "dead-row", "6")
            TemplateHandler.Assign(soccerTpl, "live-row", "5")
            TemplateHandler.Assign(soccerTpl, "ChkUpdatePT", String.Empty)
        End If
        TemplateHandler.Assign(soccerTpl, "col-title", "12")
        TemplateHandler.Assign(soccerTpl, "cols-live", "2")
        TemplateHandler.Assign(soccerTpl, "bettype-header", LoadSoccerPTHeader())
        GetTitleCommon(soccerTpl)
        TemplateHandler.Assign(soccerTpl, "deadball-text", Resources.Lang.lblDeadball)
        TemplateHandler.Assign(soccerTpl, "live-text", Resources.Lang.lblLive)
        TemplateHandler.Assign(soccerTpl, "a-preset-live", Resources.Lang.agent)
        TemplateHandler.Assign(soccerTpl, "p-preset-live", Resources.Lang.super)
        TemplateHandler.Assign(soccerTpl, "au-presetpt-text", Resources.Lang.lblSuperAuto)
        TemplateHandler.Assign(soccerTpl, "au-preset-live", Resources.Lang.lblSuperAuto)
        TemplateHandler.Assign(soccerTpl, "master-text", Resources.Lang.master)
        TemplateHandler.Assign(soccerTpl, "copy-live-col", "3")
        TemplateHandler.Assign(soccerTpl, "copy-1x2-text", Resources.Lang.lblCopyFrom1x2)
        'assign value of Minbet, Maxbet , ...           

        'assign value position taking for template          
        Dim mHdp, mOU, m1stHdp, m1stOU, mOE, m1x2, mCS, mTG, mMP, mOR As Double
        Dim mLHdp, mLOU, mL1stHdp, mL1stOU As Double

        Dim aHdp, aOU, a1stHdp, a1stOU, aOE, a1x2, aCS, aTG, aMP, aOR As Double
        Dim aLHdp, aLOU, aL1stHdp, aL1stOU As Double

        Dim sHdpMin, sOUMin, s1stHdpMin, s1stOUMin, sOEMin, s1x2Min, sCSMin, sTGMin, sMPMin, sORMin As Double
        Dim sLHdpMin, sLOUMin, sL1stHdpMin, sL1stOUMin As Double

        Dim sHdp, sOU, s1stHdp, s1stOU, sOE, s1x2, sCS, sTG, sMP, sOR As Double
        Dim sLHdp, sLOU, sL1stHdp, sL1stOU As Double

        Dim sHdpMax, sOUMax, s1stHdpMax, s1stOUMax, sOEMax, s1x2Max, sCSMax, sTGMax, sMPMax, sORMax As Double
        Dim sLHdpMax, sLOUMax, sL1stHdpMax, sL1stOUMax As Double

        Dim sAutoHdp, sAutoOU, sAuto1stHdp, sAuto1stOU, sAutoOE, sAuto1x2, sAutoCS, sAutoTG, sAutoMP, sAutoOR As Double
        Dim sAutoLHdp, sAutoLOU, sAutoL1stHdp, sAutoL1stOU As Double

        Dim sAutoHdpMax, sAutoOUMax, sAuto1stHdpMax, sAuto1stOUMax, sAutoOEMax, sAuto1x2Max, sAutoCSMax, sAutoTGMax, sAutoMPMax, sAutoORMax As Double
        Dim sAutoLHdpMax, sAutoLOUMax, sAutoL1stHdpMax, sAutoL1stOUMax As Double

        Dim isAutoHdp, isAutoOU, isAuto1stHdp, isAuto1stOU, isAutoOE, isAuto1x2, isAutoCS, isAutoTG, isAutoMP, isAutoOR As Double
        Dim isAutoLHdp, isAutoLOU, isAutoL1stHdp, isAutoL1stOU As Double

        Dim isSAutoHdp, isSAutoOU, isSAuto1stHdp, isSAuto1stOU, isSAutoOE, isSAuto1x2, isSAutoCS, isSAutoTG, isSAutoMP, isSAutoOR As Double
        Dim isSAutoLHdp, isSAutoLOU, isSAutoL1stHdp, isSAutoL1stOU As Double

        sHdp = CastAide.AsDouble(dr("spositiontaking_1"))
        sOU = CastAide.AsDouble(dr("spositiontaking_3"))
        s1stHdp = CastAide.AsDouble(dr("spositiontaking_7"))
        s1stOU = CastAide.AsDouble(dr("spositiontaking_8"))
        sOE = CastAide.AsDouble(dr("spositiontaking_2"))
        s1x2 = CastAide.AsDouble(dr("spositiontaking_5"))
        sCS = CastAide.AsDouble(dr("spositiontaking_4"))
        sTG = CastAide.AsDouble(dr("spositiontaking_6"))
        sMP = CastAide.AsDouble(dr("spositiontaking_9"))
        sOR = CastAide.AsDouble(dr("spositiontaking_10"))
        sLHdp = CastAide.AsDouble(dr("slivepositiontaking_1"))
        sLOU = CastAide.AsDouble(dr("slivepositiontaking_3"))
        sL1stHdp = CastAide.AsDouble(dr("slivepositiontaking_7"))
        sL1stOU = CastAide.AsDouble(dr("slivepositiontaking_8"))

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

        isSAutoHdp = CastAide.AsDouble(dr("isauto_spositiontaking_1"))
        isSAutoOU = CastAide.AsDouble(dr("isauto_spositiontaking_3"))
        isSAuto1stHdp = CastAide.AsDouble(dr("isauto_spositiontaking_7"))
        isSAuto1stOU = CastAide.AsDouble(dr("isauto_spositiontaking_8"))
        isSAutoOE = CastAide.AsDouble(dr("isauto_spositiontaking_2"))
        isSAuto1x2 = CastAide.AsDouble(dr("isauto_spositiontaking_5"))
        isSAutoCS = CastAide.AsDouble(dr("isauto_spositiontaking_4"))
        isSAutoTG = CastAide.AsDouble(dr("isauto_spositiontaking_6"))
        isSAutoMP = CastAide.AsDouble(dr("isauto_spositiontaking_9"))
        isSAutoOR = CastAide.AsDouble(dr("isauto_spositiontaking_10"))

        isSAutoLHdp = CastAide.AsDouble(dr("isauto_slivepositiontaking_1"))
        isSAutoLOU = CastAide.AsDouble(dr("isauto_slivepositiontaking_3"))
        isSAutoL1stHdp = CastAide.AsDouble(dr("isauto_slivepositiontaking_7"))
        isSAutoL1stOU = CastAide.AsDouble(dr("isauto_slivepositiontaking_8"))

        If Not isMult Then 'this case for Single Edit      
            sHdpMin = CastAide.AsDouble(dr("minpos_1"))
            sOUMin = CastAide.AsDouble(dr("minpos_3"))
            s1stHdpMin = CastAide.AsDouble(dr("minpos_7"))
            s1stOUMin = CastAide.AsDouble(dr("minpos_8"))
            sOEMin = CastAide.AsDouble(dr("minpos_2"))
            s1x2Min = CastAide.AsDouble(dr("minpos_5"))
            sCSMin = CastAide.AsDouble(dr("minpos_4"))
            sTGMin = CastAide.AsDouble(dr("minpos_6"))
            sMPMin = CastAide.AsDouble(dr("minpos_9"))
            sORMin = CastAide.AsDouble(dr("minpos_10"))
            sLHdpMin = CastAide.AsDouble(dr("minlpos_1"))
            sLOUMin = CastAide.AsDouble(dr("minlpos_3"))
            sL1stHdpMin = CastAide.AsDouble(dr("minlpos_7"))
            sL1stOUMin = CastAide.AsDouble(dr("minlpos_8"))

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

            sHdpMin = CastAide.AsDouble(dr("minpos_1"))
            sOUMin = CastAide.AsDouble(dr("minpos_3"))
            s1stHdpMin = CastAide.AsDouble(dr("minpos_7"))
            s1stOUMin = CastAide.AsDouble(dr("minpos_8"))
            sOEMin = CastAide.AsDouble(dr("minpos_2"))
            s1x2Min = CastAide.AsDouble(dr("minpos_5"))
            sCSMin = CastAide.AsDouble(dr("minpos_4"))
            sTGMin = CastAide.AsDouble(dr("minpos_6"))
            sMPMin = CastAide.AsDouble(dr("minpos_9"))
            sORMin = CastAide.AsDouble(dr("minpos_10"))
            sLHdpMin = CastAide.AsDouble(dr("minlpos_1"))
            sLOUMin = CastAide.AsDouble(dr("minlpos_3"))
            sL1stHdpMin = CastAide.AsDouble(dr("minlpos_7"))
            sL1stOUMin = CastAide.AsDouble(dr("minlpos_8"))

            sHdpMax = CastAide.AsDouble(dr("maxpos_1"))
            sOUMax = CastAide.AsDouble(dr("maxpos_3"))
            s1stHdpMax = CastAide.AsDouble(dr("maxpos_7"))
            s1stOUMax = CastAide.AsDouble(dr("maxpos_8"))
            sOEMax = CastAide.AsDouble(dr("maxpos_2"))
            s1x2Max = CastAide.AsDouble(dr("maxpos_5"))
            sCSMax = CastAide.AsDouble(dr("maxpos_4"))
            sTGMax = CastAide.AsDouble(dr("maxpos_6"))
            sMPMax = CastAide.AsDouble(dr("maxpos_9"))
            sORMax = CastAide.AsDouble(dr("maxpos_10"))
            sLHdpMax = CastAide.AsDouble(dr("maxlpos_1"))
            sLOUMax = CastAide.AsDouble(dr("maxlpos_3"))
            sL1stHdpMax = CastAide.AsDouble(dr("maxlpos_7"))
            sL1stOUMax = CastAide.AsDouble(dr("maxlpos_8"))

            sAutoHdpMax = CastAide.AsDouble(dr("maxautopos_1"))
            sAutoOUMax = CastAide.AsDouble(dr("maxautopos_3"))
            sAuto1stHdpMax = CastAide.AsDouble(dr("maxautopos_7"))
            sAuto1stOUMax = CastAide.AsDouble(dr("maxautopos_8"))
            sAutoOEMax = CastAide.AsDouble(dr("maxautopos_2"))
            sAuto1x2Max = CastAide.AsDouble(dr("maxautopos_5"))
            sAutoCSMax = CastAide.AsDouble(dr("maxautopos_4"))
            sAutoTGMax = CastAide.AsDouble(dr("maxautopos_6"))
            sAutoMPMax = CastAide.AsDouble(dr("maxautopos_9"))
            sAutoORMax = CastAide.AsDouble(dr("maxautopos_10"))
            sAutoLHdpMax = CastAide.AsDouble(dr("maxautolpos_1"))
            sAutoLOUMax = CastAide.AsDouble(dr("maxautolpos_3"))
            sAutoL1stHdpMax = CastAide.AsDouble(dr("maxautolpos_7"))
            sAutoL1stOUMax = CastAide.AsDouble(dr("maxautolpos_8"))

            isAutoHdp = CastAide.AsInt16(dr("isauto_spositiontaking_mem_1"))
            isAutoOU = CastAide.AsInt16(dr("isauto_spositiontaking_mem_3"))
            isAuto1stHdp = CastAide.AsInt16(dr("isauto_spositiontaking_mem_7"))
            isAuto1stOU = CastAide.AsInt16(dr("isauto_spositiontaking_mem_8"))
            isAutoOE = CastAide.AsInt16(dr("isauto_spositiontaking_mem_2"))
            isAuto1x2 = CastAide.AsInt16(dr("isauto_spositiontaking_mem_5"))
            isAutoCS = CastAide.AsInt16(dr("isauto_spositiontaking_mem_4"))
            isAutoTG = CastAide.AsInt16(dr("isauto_spositiontaking_mem_6"))
            isAutoMP = CastAide.AsInt16(dr("isauto_spositiontaking_mem_9"))
            isAutoOR = CastAide.AsInt16(dr("isauto_spositiontaking_mem_10"))
            isAutoLHdp = CastAide.AsInt16(dr("isauto_slivepositiontaking_mem_1"))
            isAutoLOU = CastAide.AsInt16(dr("isauto_slivepositiontaking_mem_3"))
            isAutoL1stHdp = CastAide.AsInt16(dr("isauto_slivepositiontaking_mem_7"))
            isAutoL1stOU = CastAide.AsInt16(dr("isauto_slivepositiontaking_mem_8"))
        Else 'this case for Multiple Edit
            sHdpMax = sHdp
            sOUMax = sOU
            s1stHdpMax = s1stHdp
            s1stOUMax = s1stOU
            sOEMax = sOE
            s1x2Max = s1x2
            sCSMax = sCS
            sTGMax = sTG
            sMPMax = sMP
            sORMax = sOR
            sLHdpMax = sLHdp
            sLOUMax = sLOU
            sL1stHdpMax = sL1stHdp
            sL1stOUMax = sL1stOU

            sAutoHdpMax = sAutoHdp
            sAutoOUMax = sAutoOU
            sAuto1stHdpMax = sAuto1stHdp
            sAuto1stOUMax = sAuto1stOU
            sAutoOEMax = sAutoOE
            sAuto1x2Max = sAuto1x2
            sAutoCSMax = sAutoCS
            sAutoTGMax = sAutoTG
            sAutoMPMax = sAutoMP
            sAutoORMax = sAutoOR
            sAutoLHdpMax = sAutoLHdp
            sAutoLOUMax = sAutoLOU
            sAutoL1stHdpMax = sAutoL1stHdp
            sAutoL1stOUMax = sAutoL1stOU
        End If
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''          
        '''''''''''end script to get max PT value of master that master can be preseted                       

        hiddenHash.Add("isSAuto1HdpD", isSAutoHdp)
        hiddenHash.Add("isSAuto1OUD", isSAutoOU)
        hiddenHash.Add("isSAuto11stHdpD", isSAuto1stHdp)
        hiddenHash.Add("isSAuto11stOUD", isSAuto1stOU)
        hiddenHash.Add("isSAuto1OE", isSAutoOE)

        hiddenHash.Add("isSAuto11x2", isSAuto1x2)
        hiddenHash.Add("isSAuto1CS", isSAutoCS)
        hiddenHash.Add("isSAuto1TG", isSAutoTG)
        hiddenHash.Add("isSAuto1MP", isSAutoMP)
        hiddenHash.Add("isSAuto1OR", isSAutoOR)

        hiddenHash.Add("isSAuto1HdpL", isSAutoLHdp)
        hiddenHash.Add("isSAuto1OUL", isSAutoLOU)
        hiddenHash.Add("isSAuto11stHdpL", isSAutoL1stHdp)
        hiddenHash.Add("isSAuto11stOUL", isSAutoL1stOU)

        strHidSoccer = PositionTaking.AssignMulipleHidden(hiddenHash)

        'assign Increase and Reduce button for template
        Dim btnTpl As String = groupBtn
        If isSAutoHdp Then
            TemplateHandler.Assign(soccerTpl, "p-hdp-btn", String.Empty)
            If isAutoHdp = 0 Then
                TemplateHandler.Assign(soccerTpl, "chk-hdpd", "disabled")
                isAutoHdp_Super_Soccer = True
            End If
        Else
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS1HdpD")
            TemplateHandler.Assign(soccerTpl, "p-hdp-btn", btnTpl)
            isAutoHdp_Super_Soccer = False
        End If

        If isSAuto1x2 Then
            TemplateHandler.Assign(soccerTpl, "p-1x2-btn", String.Empty)
            If isAuto1x2 = 0 Then
                TemplateHandler.Assign(soccerTpl, "chk-1x2", "disabled")
            End If
        Else
            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS11x2")
            TemplateHandler.Assign(soccerTpl, "p-1x2-btn", btnTpl)
        End If

        If isSAutoLHdp Then
            TemplateHandler.Assign(soccerTpl, "p-Lhdp-btn", String.Empty)
            If isAuto1stHdp = 0 Then
                TemplateHandler.Assign(soccerTpl, "chk-hdpl", "disabled")
            End If
        Else
            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS1HdpL")
            TemplateHandler.Assign(soccerTpl, "p-Lhdp-btn", btnTpl)
        End If

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS1HdpD")
        TemplateHandler.Assign(soccerTpl, "au-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS11x2")
        TemplateHandler.Assign(soccerTpl, "au-1x2-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS1HdpL")
        TemplateHandler.Assign(soccerTpl, "au-Lhdp-btn", btnTpl)

        'this script is used to check or uncheck All Auto Checkbox
        'disable or enable Super Preset Position or Auto Super Preset Position
        Dim ptAuto As String
        Dim strChecked As String
        Dim pt As String = selectTpl
        If isAutoHdp Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-Hdp", strChecked)
        TemplateHandler.Assign(soccerTpl, "a-hdp-val", aHdp)
        TemplateHandler.Assign(soccerTpl, "m-hdp-val", mHdp)

        If isSAutoHdp Then
            TemplateHandler.Assign(soccerTpl, "p-hdp-val", sHdp)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS1HdpD")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sHdpMin, sHdpMax, sHdp))
            TemplateHandler.Assign(soccerTpl, "p-hdp-val", pt)
        End If
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1HdpD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoHdpMax, sAutoHdp))
        TemplateHandler.Assign(soccerTpl, "au-hdp-val", ptAuto)

        If isAutoOU Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-OU", strChecked)
        TemplateHandler.Assign(soccerTpl, "a-ou-val", aOU)
        TemplateHandler.Assign(soccerTpl, "m-ou-val", mOU)

        If isSAutoOU Then
            TemplateHandler.Assign(soccerTpl, "p-ou-val", sOU)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS1OUD")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOUMin, sOUMax, sOU))
            TemplateHandler.Assign(soccerTpl, "p-ou-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OUD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOUMax, sAutoOU))
        TemplateHandler.Assign(soccerTpl, "au-ou-val", ptAuto)

        If isAuto1stHdp Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-1stHdp", strChecked)
        TemplateHandler.Assign(soccerTpl, "a-1sthdp-val", a1stHdp)
        TemplateHandler.Assign(soccerTpl, "m-1sthdp-val", m1stHdp)

        If isSAuto1stHdp Then
            TemplateHandler.Assign(soccerTpl, "p-1sthdp-val", s1stHdp)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS11stHdpD")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(s1stHdpMin, s1stHdpMax, s1stHdp))
            TemplateHandler.Assign(soccerTpl, "p-1sthdp-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stHdpD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAuto1stHdpMax, sAuto1stHdp))
        TemplateHandler.Assign(soccerTpl, "au-1sthdp-val", ptAuto)

        If isAuto1stOU Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-1stOU", strChecked)
        TemplateHandler.Assign(soccerTpl, "a-1stou-val", a1stOU)
        TemplateHandler.Assign(soccerTpl, "m-1stou-val", m1stOU)

        If isSAuto1stOU Then
            TemplateHandler.Assign(soccerTpl, "p-1stou-val", s1stOU)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS11stOUD")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(s1stOUMin, s1stOUMax, s1stOU))
            TemplateHandler.Assign(soccerTpl, "p-1stou-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stOUD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAuto1stOUMax, sAuto1stOU))
        TemplateHandler.Assign(soccerTpl, "au-1stou-val", ptAuto)

        If isAutoOE Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-OE", strChecked)
        TemplateHandler.Assign(soccerTpl, "a-oe-val", aOE)
        TemplateHandler.Assign(soccerTpl, "m-oe-val", mOE)

        If isSAutoOE Then
            TemplateHandler.Assign(soccerTpl, "p-oe-val", sOE)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS1OE")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOEMin, sOEMax, sOE))
            TemplateHandler.Assign(soccerTpl, "p-oe-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OE")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOEMax, sAutoOE))
        TemplateHandler.Assign(soccerTpl, "au-oe-val", ptAuto)

        If isAuto1x2 Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-1x2", strChecked)
        TemplateHandler.Assign(soccerTpl, "a-1x2-val", a1x2)
        TemplateHandler.Assign(soccerTpl, "m-1x2-val", m1x2)

        If isSAuto1x2 Then
            TemplateHandler.Assign(soccerTpl, "p-1x2-val", s1x2)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS11x2")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(s1x2Min, s1x2Max, s1x2))
            TemplateHandler.Assign(soccerTpl, "p-1x2-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11x2")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAuto1x2Max, sAuto1x2))
        TemplateHandler.Assign(soccerTpl, "au-1x2-val", ptAuto)
        If isAutoCS Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-CS", strChecked)
        TemplateHandler.Assign(soccerTpl, "a-cs-val", aCS)
        TemplateHandler.Assign(soccerTpl, "m-cs-val", mCS)

        If isSAutoCS Then
            TemplateHandler.Assign(soccerTpl, "p-cs-val", sCS)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS1CS")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sCSMin, sCSMax, sCS))
            TemplateHandler.Assign(soccerTpl, "p-cs-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1CS")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoCSMax, sAutoCS))
        TemplateHandler.Assign(soccerTpl, "au-cs-val", ptAuto)

        If isAutoTG Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-TG", strChecked)
        TemplateHandler.Assign(soccerTpl, "a-tg-val", aTG)
        TemplateHandler.Assign(soccerTpl, "m-tg-val", mTG)

        If isSAutoTG Then
            TemplateHandler.Assign(soccerTpl, "p-tg-val", sTG)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS1TG")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sTGMin, sTGMax, sTG))
            TemplateHandler.Assign(soccerTpl, "p-tg-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1TG")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoTGMax, sAutoTG))
        TemplateHandler.Assign(soccerTpl, "au-tg-val", ptAuto)

        If isAutoMP Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-MP", strChecked)
        TemplateHandler.Assign(soccerTpl, "a-mp-val", aMP)
        TemplateHandler.Assign(soccerTpl, "m-mp-val", mMP)

        If isSAutoMP Then
            TemplateHandler.Assign(soccerTpl, "p-mp-val", sMP)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS1MP")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sMPMin, sMPMax, sMP))
            TemplateHandler.Assign(soccerTpl, "p-mp-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1MP")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoMPMax, sAutoMP))
        TemplateHandler.Assign(soccerTpl, "au-mp-val", ptAuto)

        If isAutoOR Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-OR", strChecked)
        TemplateHandler.Assign(soccerTpl, "a-or-val", aOR)
        TemplateHandler.Assign(soccerTpl, "m-or-val", mOR)

        If isSAutoOR Then
            TemplateHandler.Assign(soccerTpl, "p-or-val", sOR)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS1OR")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sORMin, sORMax, sOR))
            TemplateHandler.Assign(soccerTpl, "p-or-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OR")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoORMax, sAutoOR))
        TemplateHandler.Assign(soccerTpl, "au-or-val", ptAuto)

        If isAutoLHdp Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-LHdp", strChecked)
        TemplateHandler.Assign(soccerTpl, "a-Lhdp-val", aLHdp)
        TemplateHandler.Assign(soccerTpl, "m-Lhdp-val", mLHdp)

        If isSAutoLHdp Then
            TemplateHandler.Assign(soccerTpl, "p-Lhdp-val", sLHdp)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS1HdpL")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sLHdpMin, sLHdpMax, sLHdp))
            TemplateHandler.Assign(soccerTpl, "p-Lhdp-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1HdpL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoLHdpMax, sAutoLHdp))
        TemplateHandler.Assign(soccerTpl, "au-Lhdp-val", ptAuto)

        If isAutoLOU Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-LOU", strChecked)
        TemplateHandler.Assign(soccerTpl, "a-Lou-val", aLOU)
        TemplateHandler.Assign(soccerTpl, "m-Lou-val", mLOU)

        If isSAutoLOU Then
            TemplateHandler.Assign(soccerTpl, "p-Lou-val", sLOU)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS1OUL")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sLOUMin, sLOUMax, sLOU))
            TemplateHandler.Assign(soccerTpl, "p-Lou-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OUL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoLOUMax, sAutoLOU))
        TemplateHandler.Assign(soccerTpl, "au-Lou-val", ptAuto)

        If isAutoL1stHdp Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-L1stHdp", strChecked)
        TemplateHandler.Assign(soccerTpl, "a-L1stHdp-val", aL1stHdp)
        TemplateHandler.Assign(soccerTpl, "m-L1stHdp-val", mL1stHdp)

        If isSAutoL1stHdp Then
            TemplateHandler.Assign(soccerTpl, "p-L1stHdp-val", sL1stHdp)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS11stHdpL")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sL1stHdpMin, sL1stHdpMax, sL1stHdp))
            TemplateHandler.Assign(soccerTpl, "p-L1stHdp-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stHdpL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoL1stHdpMax, sAutoL1stHdp))
        TemplateHandler.Assign(soccerTpl, "au-L1stHdp-val", ptAuto)

        If isAutoL1stOU Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-L1stOU", strChecked)
        TemplateHandler.Assign(soccerTpl, "a-L1stou-val", aL1stOU)
        TemplateHandler.Assign(soccerTpl, "m-L1stou-val", mL1stOU)

        If isSAutoL1stOU Then
            TemplateHandler.Assign(soccerTpl, "p-L1stou-val", sL1stOU)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS11stOUL")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sL1stOUMin, sL1stOUMax, sL1stOU))
            TemplateHandler.Assign(soccerTpl, "p-L1stou-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stOUL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoL1stOUMax, sAutoL1stOU))
        TemplateHandler.Assign(soccerTpl, "au-L1stou-val", ptAuto)

        GetDataSoccer = soccerTpl
    End Function
#End Region

#Region "Assign info for  Basketball"
    'this function is used to assign data into template for all sporttype that have 4 bettype
    '(1:Handicap, 3:Over/Under, 2:Odd/Even, 9: Mix Parlay, 20:Money Line)
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (2: Basketball )
    Function GetDataBasketball(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String, Optional ByVal isMult As Boolean = False) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable

        'assign text for position taking
        GetTitleCommon(tpl)
        If isMult Then
            TemplateHandler.RemoveBlock(tpl, "none-Multi")

            Dim chkUpdateMultiple As String = LoadCheckBoxUpdateMultipe(sportTypeId.ToString(), Resources.Lang.positiontaking)
            TemplateHandler.Assign(tpl, "ChkUpdatePT", chkUpdateMultiple)
        Else
            TemplateHandler.Assign(tpl, "ChkUpdatePT", String.Empty)
        End If
        TemplateHandler.Assign(tpl, "sportId", sportTypeId)
        TemplateHandler.Assign(tpl, "cols-a", "2")
        TemplateHandler.Assign(tpl, "master-text", Resources.Lang.master)
        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "oe-text", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(tpl, "mp-text", Resources.Lang.lblMixParlay)
        TemplateHandler.Assign(tpl, "copy-hdp-col", "3")
        TemplateHandler.Assign(tpl, "title-col", "7")
        TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")



        'assign value for position taking           
        Dim mHdp, mOU, mOE, mML As Double
        Dim aHdp, aOU, aOE, aML As Double

        Dim sHdpMin, sOUMin, sOEMin, sMLMin As Double
        Dim sHdp, sOU, sOE, sML As Double
        Dim sHdpMax, sOUMax, sOEMax, sMLMax As Double

        Dim sAutoHdp, sAutoOU, sAutoOE, sAutoML As Double
        Dim sAutoHdpMax, sAutoOUMax, sAutoOEMax, sAutoMLMax As Double
        Dim isAutoHdp, isAutoOU, isAutoOE, isAutoML As Double

        Dim isSAutoHdp, isSAutoOU, isSAutoOE, isSAutoML As Double

        sHdp = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1"))
        sOU = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3"))
        sOE = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_2"))
        sML = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20"))

        sAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1"))
        sAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_3"))
        sAutoOE = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_2"))
        sAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20"))

        isSAutoHdp = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_1"))
        isSAutoOU = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_3"))
        isSAutoOE = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_2"))
        isSAutoML = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_20"))

        If Not isMult Then
            aHdp = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
            aOU = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3"))
            aOE = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2"))
            aML = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

            mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
            mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
            mOE = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))
            mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

            sHdpMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_1"))
            sOUMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_3"))
            sOEMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_2"))
            sMLMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_20"))

            sHdpMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_1"))
            sOUMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_3"))
            sOEMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_2"))
            sMLMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_20"))

            sAutoHdpMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_1"))
            sAutoOUMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_3"))
            sAutoOEMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_2"))
            sAutoMLMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_20"))

            isAutoHdp = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_mem_1"))
            isAutoOU = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_mem_3"))
            isAutoOE = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_mem_2"))
            isAutoML = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_mem_20"))
        Else
            sHdpMax = sHdp
            sOUMax = sOU
            sOEMax = sOE
            sMLMax = sML

            sAutoHdpMax = sAutoHdp
            sAutoOUMax = sAutoOU
            sAutoOEMax = sAutoOE
            sAutoMLMax = sAutoML
        End If

        hiddenHash.Add("isSAuto" & sportTypeId & "Hdp", isSAutoHdp)
        hiddenHash.Add("isSAuto" & sportTypeId & "OU", isSAutoOU)
        hiddenHash.Add("isSAuto" & sportTypeId & "OE", isSAutoOE)
        hiddenHash.Add("isSAuto" & sportTypeId & "ML", isSAutoML)

        strHidBasket = PositionTaking.AssignMulipleHidden(hiddenHash)

        Dim btnTpl As String = groupBtn
        If isSAutoHdp Then
            TemplateHandler.Assign(tpl, "p-hdp-btn", String.Empty)
            If isAutoHdp = 0 Then
                TemplateHandler.Assign(tpl, "chk-hdp", "disabled")
                isAutoHdp_Super_Basketball = True
            End If
        Else
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)
            isAutoHdp_Super_Basketball = False
        End If


        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

        Dim ptAuto As String
        Dim strChecked As String
        If isAutoHdp Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-Hdp", strChecked)
        TemplateHandler.Assign(tpl, "a-hdp-val", aHdp)
        TemplateHandler.Assign(tpl, "m-hdp-val", mHdp)

        If isSAutoHdp Then
            TemplateHandler.Assign(tpl, "p-hdp-val", sHdp)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sHdpMin, sHdpMax, sHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoHdpMax, sAutoHdp))
        TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)

        If isAutoOU Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-OU", strChecked)
        TemplateHandler.Assign(tpl, "a-ou-val", aOU)
        TemplateHandler.Assign(tpl, "m-ou-val", mOU)

        If isSAutoOU Then
            TemplateHandler.Assign(tpl, "p-ou-val", sOU)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOUMin, sOUMax, sOU))
            TemplateHandler.Assign(tpl, "p-ou-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOUMax, sAutoOU))
        TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)

        If isAutoOE Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-OE", strChecked)
        TemplateHandler.Assign(tpl, "a-oe-val", aOE)
        TemplateHandler.Assign(tpl, "m-oe-val", mOE)

        If isSAutoOE Then
            TemplateHandler.Assign(tpl, "p-oe-val", sOE)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OE")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOEMin, sOEMax, sOE))
            TemplateHandler.Assign(tpl, "p-oe-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OE")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOEMax, sAutoOE))
        TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)

        If isAutoML Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-ML", strChecked)
        TemplateHandler.Assign(tpl, "a-ml-val", aML)
        TemplateHandler.Assign(tpl, "m-ml-val", mML)

        If isSAutoML Then
            TemplateHandler.Assign(tpl, "p-ml-val", sML)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sMLMin, sMLMax, sML))
            TemplateHandler.Assign(tpl, "p-ml-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoMLMax, sAutoML))
        TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)


        GetDataBasketball = tpl
    End Function
#End Region

#Region "Assign info for  Football, Tennis, Golf, OtherSports"
    'this function is used to assign data into template for all sporttype that have 4 bettype
    '(1:Handicap, 3:Over/Under, 2:Odd/Even, 20:Money Line)
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (3:Football, 5:Tennis, 10:Golf, 99:OtherSports )
    Function GetData4DDl(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String, Optional ByVal isMult As Boolean = False) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable

        'assign text for position taking
        GetTitleCommon(tpl)
        If isMult Then
            TemplateHandler.RemoveBlock(tpl, "none-Multi")
            Dim chkUpdateMultiple As String = LoadCheckBoxUpdateMultipe(sportTypeId.ToString(), Resources.Lang.positiontaking)
            TemplateHandler.Assign(tpl, "ChkUpdatePT", chkUpdateMultiple)
        Else
            TemplateHandler.Assign(tpl, "ChkUpdatePT", String.Empty)
        End If
        TemplateHandler.Assign(tpl, "sportId", sportTypeId)
        TemplateHandler.Assign(tpl, "cols-a", "2")
        TemplateHandler.Assign(tpl, "master-text", Resources.Lang.master)
        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "oe-text", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(tpl, "copy-hdp-col", "5")
        TemplateHandler.Assign(tpl, "title-col", "6")
        TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")

        'assign value for position taking           
        Dim mHdp, mOU, mOE, mML As Double
        Dim aHdp, aOU, aOE, aML As Double

        Dim sHdpMin, sOUMin, sOEMin, sMLMin As Double
        Dim sHdp, sOU, sOE, sML As Double
        Dim sHdpMax, sOUMax, sOEMax, sMLMax As Double

        Dim sAutoHdp, sAutoOU, sAutoOE, sAutoML As Double
        Dim sAutoHdpMax, sAutoOUMax, sAutoOEMax, sAutoMLMax As Double
        Dim isAutoHdp, isAutoOU, isAutoOE, isAutoML As Double

        Dim isSAutoHdp, isSAutoOU, isSAutoOE, isSAutoML As Double

        sHdp = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1"))
        sOU = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3"))
        sOE = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_2"))
        sML = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20"))

        sAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1"))
        sAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_3"))
        sAutoOE = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_2"))
        sAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20"))

        isSAutoHdp = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_1"))
        isSAutoOU = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_3"))
        isSAutoOE = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_2"))
        isSAutoML = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_20"))

        If Not isMult Then
            aHdp = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
            aOU = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3"))
            aOE = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2"))
            aML = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

            mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
            mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
            mOE = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))
            mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

            sHdpMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_1"))
            sOUMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_3"))
            sOEMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_2"))
            sMLMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_20"))

            sHdpMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_1"))
            sOUMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_3"))
            sOEMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_2"))
            sMLMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_20"))

            sAutoHdpMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_1"))
            sAutoOUMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_3"))
            sAutoOEMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_2"))
            sAutoMLMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_20"))

            isAutoHdp = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_mem_1"))
            isAutoOU = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_mem_3"))
            isAutoOE = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_mem_2"))
            isAutoML = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_mem_20"))
        Else
            sHdpMax = sHdp
            sOUMax = sOU
            sOEMax = sOE
            sMLMax = sML

            sAutoHdpMax = sAutoHdp
            sAutoOUMax = sAutoOU
            sAutoOEMax = sAutoOE
            sAutoMLMax = sAutoML
        End If

        hiddenHash.Add("isSAuto" & sportTypeId & "Hdp", isSAutoHdp)
        hiddenHash.Add("isSAuto" & sportTypeId & "OU", isSAutoOU)
        hiddenHash.Add("isSAuto" & sportTypeId & "OE", isSAutoOE)
        hiddenHash.Add("isSAuto" & sportTypeId & "ML", isSAutoML)

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

        Dim btnTpl As String = groupBtn
        If isSAutoHdp Then
            TemplateHandler.Assign(tpl, "p-hdp-btn", String.Empty)
            If isAutoHdp = 0 Then
                TemplateHandler.Assign(tpl, "chk-hdp", "disabled")
            End If
        Else
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)
        End If

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

        Dim ptAuto As String
        Dim strChecked As String
        If isAutoHdp Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-Hdp", strChecked)
        TemplateHandler.Assign(tpl, "a-hdp-val", aHdp)
        TemplateHandler.Assign(tpl, "m-hdp-val", mHdp)

        If isSAutoHdp Then
            TemplateHandler.Assign(tpl, "p-hdp-val", sHdp)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sHdpMin, sHdpMax, sHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoHdpMax, sAutoHdp))
        TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)

        If isAutoOU Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-OU", strChecked)
        TemplateHandler.Assign(tpl, "a-ou-val", aOU)
        TemplateHandler.Assign(tpl, "m-ou-val", mOU)

        If isSAutoOU Then
            TemplateHandler.Assign(tpl, "p-ou-val", sOU)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOUMin, sOUMax, sOU))
            TemplateHandler.Assign(tpl, "p-ou-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOUMax, sAutoOU))
        TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)

        If isAutoOE Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-OE", strChecked)
        TemplateHandler.Assign(tpl, "a-oe-val", aOE)
        TemplateHandler.Assign(tpl, "m-oe-val", mOE)

        If isSAutoOE Then
            TemplateHandler.Assign(tpl, "p-oe-val", sOE)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OE")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOEMin, sOEMax, sOE))
            TemplateHandler.Assign(tpl, "p-oe-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OE")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOEMax, sAutoOE))
        TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)

        If isAutoML Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-ML", strChecked)
        TemplateHandler.Assign(tpl, "a-ml-val", aML)
        TemplateHandler.Assign(tpl, "m-ml-val", mML)

        If isSAutoML Then
            TemplateHandler.Assign(tpl, "p-ml-val", sML)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sMLMin, sMLMax, sML))
            TemplateHandler.Assign(tpl, "p-ml-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoMLMax, sAutoML))
        TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)


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
        GetTitleCommon(tpl)
        If isMult Then
            TemplateHandler.RemoveBlock(tpl, "none-Multi")
            Dim chkUpdateMultiple As String = LoadCheckBoxUpdateMultipe(sportTypeId.ToString(), Resources.Lang.positiontaking)
            TemplateHandler.Assign(tpl, "ChkUpdatePT", chkUpdateMultiple)
        Else
            TemplateHandler.Assign(tpl, "ChkUpdatePT", String.Empty)
        End If
        TemplateHandler.Assign(tpl, "cols-a", "2")
        TemplateHandler.Assign(tpl, "sportId", sportTypeId)
        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "copy-hdp-col", "4")
        TemplateHandler.Assign(tpl, "title-col", "5")
        TemplateHandler.Assign(tpl, "master-text", Resources.Lang.master)
        TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "chk1-Id", "chkAuto8Hdp")
        TemplateHandler.Assign(tpl, "chk2-Id", "chkAuto8OU")
        TemplateHandler.Assign(tpl, "chk3-Id", "chkAuto8ML")
        TemplateHandler.Assign(tpl, "p-ou-btn", String.Empty)
        Dim aHdp, aOU, aML As Double
        Dim mHdp, mOU, mML As Double

        Dim sHdpMin, sOUMin, sMLMin As Double
        Dim sHdp, sOU, sML As Double
        Dim sHdpMax, sOUMax, sMLMax As Double

        Dim sAutoHdp, sAutoOU, sAutoML As Double
        Dim sAutoHdpMax, sAutoOUMax, sAutoMLMax As Double
        Dim isAutoHdp, isAutoOU, isAutoML As Double
        Dim isSAutoHdp, isSAutoOU, isSAutoML As Double

        sHdp = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1"))
        sOU = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3"))
        sML = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20"))

        sAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1"))
        sAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_3"))
        sAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20"))

        isSAutoHdp = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_1"))
        isSAutoOU = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_3"))
        isSAutoML = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_20"))

        If Not isMult Then
            aHdp = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
            aOU = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3"))
            aML = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

            mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
            mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
            mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

            sHdpMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_1"))
            sOUMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_3"))
            sMLMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_20"))

            sHdpMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_1"))
            sOUMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_3"))
            sMLMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_20"))

            sAutoHdpMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_1"))
            sAutoOUMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_3"))
            sAutoMLMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_20"))

            isAutoHdp = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_mem_1"))
            isAutoOU = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_mem_3"))
            isAutoML = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_mem_20"))
        Else
            sHdpMax = sHdp
            sOUMax = sOU
            sMLMax = sML

            sAutoHdpMax = sAutoHdp
            sAutoOUMax = sAutoOU
            sAutoMLMax = sAutoML
        End If

        hiddenHash.Add("isSAuto" & sportTypeId & "Hdp", isSAutoHdp)
        hiddenHash.Add("isSAuto" & sportTypeId & "OU", isSAutoOU)
        hiddenHash.Add("isSAuto" & sportTypeId & "ML", isSAutoML)

        strHidBaseball = PositionTaking.AssignMulipleHidden(hiddenHash)

        'assign data for Position taking dropdowns          
        Dim btnTpl As String = groupBtn
        If isSAutoHdp Then
            TemplateHandler.Assign(tpl, "p-hdp-btn", String.Empty)
            If isAutoHdp = 0 Then
                TemplateHandler.Assign(tpl, "chk-hdp", "disabled")
            End If
        Else
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)
        End If

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

        Dim ptAuto As String
        Dim strChecked As String
        If isAutoHdp Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-Hdp", strChecked)
        TemplateHandler.Assign(tpl, "a-hdp-val", aHdp)
        TemplateHandler.Assign(tpl, "m-hdp-val", mHdp)

        If isSAutoHdp Then
            TemplateHandler.Assign(tpl, "p-hdp-val", sHdp)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sHdpMin, sHdpMax, sHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoHdpMax, sAutoHdp))
        TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)

        If isAutoOU Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-OU", strChecked)
        TemplateHandler.Assign(tpl, "a-ou-val", aOU)
        TemplateHandler.Assign(tpl, "m-ou-val", mOU)

        If isSAutoOU Then
            TemplateHandler.Assign(tpl, "p-ou-val", sOU)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOUMin, sOUMax, sOU))
            TemplateHandler.Assign(tpl, "p-ou-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOUMax, sAutoOU))
        TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)

        If isAutoML Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-ML", strChecked)
        TemplateHandler.Assign(tpl, "a-ml-val", aML)
        TemplateHandler.Assign(tpl, "m-ml-val", mML)

        If isSAutoML Then
            TemplateHandler.Assign(tpl, "p-ml-val", sML)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sMLMin, sMLMax, sML))
            TemplateHandler.Assign(tpl, "p-ml-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoMLMax, sAutoML))
        TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)


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
        TemplateHandler.RemoveBlock(tpl, "none-edit")
        GetTitleCommon(tpl)
        If isMult Then
            TemplateHandler.RemoveBlock(tpl, "none-Multi")
            Dim chkUpdateMultiple As String = LoadCheckBoxUpdateMultipe(sportTypeId.ToString(), Resources.Lang.positiontaking)
            TemplateHandler.Assign(tpl, "ChkUpdatePT", chkUpdateMultiple)
        Else
            TemplateHandler.Assign(tpl, "ChkUpdatePT", String.Empty)
        End If
        TemplateHandler.Assign(tpl, "sportId", sportTypeId)
        TemplateHandler.Assign(tpl, "cols-a", "2")
        TemplateHandler.Assign(tpl, "copy-hdp-col", "4")
        TemplateHandler.Assign(tpl, "title-col", "4")
        TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")

        'get data and assign for template Position taking
        Dim aHdp, aML As Double
        Dim mHdp, mML As Double

        Dim sHdpMin, sMLMin As Double
        Dim sHdp, sML As Double
        Dim sHdpMax, sMLMax As Double

        Dim sAutoHdp, sAutoML As Double
        Dim sAutoHdpMax, sAutoMLMax As Double
        Dim isAutoHdp, isAutoML As Double

        Dim isSAutoHdp, isSAutoML As Double

        sHdp = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1"))
        sML = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20"))

        sAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1"))
        sAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20"))

        isSAutoHdp = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_1"))
        isSAutoML = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_20"))

        If Not isMult Then
            aHdp = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
            aML = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

            mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
            mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

            sHdpMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_1"))
            sMLMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_20"))

            sHdpMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_1"))
            sMLMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_20"))

            sAutoHdpMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_1"))
            sAutoMLMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_20"))

            isAutoHdp = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_mem_1"))
            isAutoML = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_mem_20"))
        Else
            sHdpMax = sHdp
            sMLMax = sML

            sAutoHdpMax = sAutoHdp
            sAutoMLMax = sAutoML
        End If

        hiddenHash.Add("isSAuto" & sportTypeId & "Hdp", isSAutoHdp)
        hiddenHash.Add("isSAuto" & sportTypeId & "ML", isSAutoML)

        strHidMoto = PositionTaking.AssignMulipleHidden(hiddenHash)

        'assign data for Position taking dropdowns     
        Dim btnTpl As String = groupBtn
        If isSAutoHdp Then
            TemplateHandler.Assign(tpl, "p-hdp-btn", String.Empty)
            If isAutoHdp = 0 Then
                TemplateHandler.Assign(tpl, "chk-hdp", "disabled")
            End If

        Else
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)
        End If


        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

        Dim ptAuto As String
        Dim strChecked As String
        If isAutoHdp Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-Hdp", strChecked)
        TemplateHandler.Assign(tpl, "a-hdp-val", aHdp)
        TemplateHandler.Assign(tpl, "m-hdp-val", mHdp)

        If isSAutoHdp Then
            TemplateHandler.Assign(tpl, "p-hdp-val", sHdp)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sHdpMin, sHdpMax, sHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoHdpMax, sAutoHdp))
        TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)

        If isAutoML Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-ML", strChecked)
        TemplateHandler.Assign(tpl, "a-ml-val", aML)
        TemplateHandler.Assign(tpl, "m-ml-val", mML)

        If isSAutoML Then
            TemplateHandler.Assign(tpl, "p-ml-val", sML)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sMLMin, sMLMax, sML))
            TemplateHandler.Assign(tpl, "p-ml-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoMLMax, sAutoML))
        TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)

        GetDataMotoSports = tpl
    End Function
#End Region

#Region "Assign info for  Number Game"
    Function GetDataNumberGame(ByVal dr As DataRow, ByVal sportTypeId As Integer, Optional ByVal isMult As Boolean = False) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable
        Dim tpl As String = TemplateHandler.LoadTemplate("MemberInfo/NumberGameEditMemberPT.html")


        If isMult Then
            Dim strCopy1stOU As String = TemplateHandler.CopyBlock(tpl, "limit-Copy1stOU")
            Dim strCopyOU As String = TemplateHandler.CopyBlock(tpl, "limit-CopyOUL")
            TemplateHandler.ReplaceBlock(tpl, "Copy1stOU", strCopy1stOU)
            TemplateHandler.ReplaceBlock(tpl, "CopyOUL", strCopyOU)

            TemplateHandler.RemoveBlock(tpl, "none-Multi")
            TemplateHandler.RemoveBlock(tpl, "none-Multi")
            TemplateHandler.Assign(tpl, "dead-row", "3")
            TemplateHandler.Assign(tpl, "live-row", "3")

            Dim chkUpdateMultiple As String = LoadCheckBoxUpdateMultipe(sportTypeId.ToString(), Resources.Lang.positiontaking)
            TemplateHandler.Assign(tpl, "ChkUpdatePT", chkUpdateMultiple)
        Else
            TemplateHandler.Assign(tpl, "dead-row", "5")
            TemplateHandler.Assign(tpl, "live-row", "5")
            TemplateHandler.Assign(tpl, "ChkUpdatePT", String.Empty)
        End If

        TemplateHandler.Assign(tpl, "col-title", "2")
        GetTitleCommon(tpl)
        TemplateHandler.Assign(tpl, "deadball-text", Resources.Lang.lblDeadball)
        TemplateHandler.Assign(tpl, "live-text", Resources.Lang.lblLive)
        TemplateHandler.Assign(tpl, "a-preset-live", Resources.Lang.agent)
        TemplateHandler.Assign(tpl, "p-preset-live", Resources.Lang.super)
        TemplateHandler.Assign(tpl, "au-presetpt-text", Resources.Lang.lblSuperAuto)
        TemplateHandler.Assign(tpl, "au-preset-live", Resources.Lang.lblSuperAuto)
        TemplateHandler.Assign(tpl, "master-text", Resources.Lang.master)
        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "oe-text", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(tpl, "hl-text", Resources.Lang.highlow)

        TemplateHandler.Assign(tpl, "1stlastou-text", Resources.Lang.firstlastoverunder)
        TemplateHandler.Assign(tpl, "1stlastoe-text", Resources.Lang.firstlastoddeven)

        TemplateHandler.Assign(tpl, "copy-1stou-text", Resources.Lang.lbl1stcopyfromoverunder)
        TemplateHandler.Assign(tpl, "copy-ouL-text", Resources.Lang.lblcopyfromoverunder)

        ' insert by david.tran on 27/7/2011
        TemplateHandler.Assign(tpl, "cb-text", Resources.Lang.nextcombo)
        TemplateHandler.Assign(tpl, "w-text", Resources.Lang.warrior)
        TemplateHandler.Assign(tpl, "nb-text", Resources.Lang.numberGroup)

        TemplateHandler.Assign(tpl, "copy-live-col", "3")

        If (Not UserPermission.IsShowNumberGroup) Then
            TemplateHandler.Assign(tpl, "hide", "hide")
        End If

        'assign value for position taking           
        Dim m1stOU, m1stOE, mOE, mWR As Double
        Dim a1stOU, a1stOE, aOE, aWR As Double

        Dim s1stOUMin, s1stOEMin, sOEMin, sWRMin As Double
        Dim s1stOU, s1stOE, sOE, sWR As Double
        Dim s1stOUMax, s1stOEMax, sOEMax, sWRMax As Double

        Dim sAuto1stOU, sAuto1stOE, sAutoOE, sAutoWR As Double
        Dim sAuto1stOUMax, sAuto1stOEMax, sAutoOEMax, sAutoWRMax As Double

        Dim isAuto1stOU, isAuto1stOE, isAutoOE, isAutoWR As Double
        Dim isSAuto1stOU, isSAuto1stOE, isSAutoOE, isSAutoWR As Double

        Dim mOUL, mOEL, mHLL, mNCL, mNGL As Double
        Dim aOUL, aOEL, aHLL, aNCL, aNGL As Double

        Dim sOUMinL, sOEMinL, sHLMinL, sNCMinL, sNGMinL As Double
        Dim sOUL, sOEL, sHLL, sNCL, sNGL As Double
        Dim sOUMaxL, sOEMaxL, sHLMaxL, sNGMaxL, sNCMaxL As Double

        Dim sAutoOUL, sAutoOEL, sAutoHLL, sAutoNCL, sAutoNGL As Double
        Dim sAutoOUMaxL, sAutoOEMaxL, sAutoHLMaxL, sAutoNCMaxL, sAutoNGMaxL As Double

        Dim isAutoOUL, isAutoOEL, isAutoHLL, isAutoNCL, isAutoNGL As Double
        Dim isSAutoOUL, isSAutoOEL, isSAutoHLL, isSAutoNCL, isSAutoNGL As Double

        s1stOU = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_81"))
        s1stOE = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_83"))
        'sOU = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_85"))
        sOE = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_86"))
        sWR = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_88"))

        sOUL = CastAide.AsDouble(dr("v" & sportTypeId & "slivepositiontaking_85"))
        sOEL = CastAide.AsDouble(dr("v" & sportTypeId & "slivepositiontaking_86"))
        sHLL = CastAide.AsDouble(dr("v" & sportTypeId & "slivepositiontaking_87"))
        sNCL = CastAide.AsDouble(dr("v" & sportTypeId & "slivepositiontaking_89"))
        sNGL = CastAide.AsDouble(dr("v" & sportTypeId & "slivepositiontaking_90"))

        sAuto1stOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_81"))
        sAuto1stOE = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_83"))
        'sAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_85"))
        sAutoOE = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_86"))
        sAutoWR = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_88"))

        sAutoOUL = CastAide.AsDouble(dr("auto_v" & sportTypeId & "slivepositiontaking_85"))
        sAutoOEL = CastAide.AsDouble(dr("auto_v" & sportTypeId & "slivepositiontaking_86"))
        sAutoHLL = CastAide.AsDouble(dr("auto_v" & sportTypeId & "slivepositiontaking_87"))
        sAutoNCL = CastAide.AsDouble(dr("auto_v" & sportTypeId & "slivepositiontaking_89"))
        sAutoNGL = CastAide.AsDouble(dr("auto_v" & sportTypeId & "slivepositiontaking_90"))

        isSAuto1stOU = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_81"))
        isSAuto1stOE = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_83"))
        'isSAutoOU = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_85"))
        isSAutoOE = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_86"))
        isSAutoWR = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_88"))

        isSAutoOUL = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "slivepositiontaking_85"))
        isSAutoOEL = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "slivepositiontaking_86"))
        isSAutoHLL = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "slivepositiontaking_87"))
        isSAutoNCL = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "slivepositiontaking_89"))
        isSAutoNGL = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "slivepositiontaking_90"))

        If Not isMult Then
            a1stOU = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_81"))
            a1stOE = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_83"))
            'aOU = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_85"))
            aOE = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_86"))
            aWR = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_88"))

            aOUL = CastAide.AsDouble(dr("v" & sportTypeId & "alivepositiontaking_85"))
            aOEL = CastAide.AsDouble(dr("v" & sportTypeId & "alivepositiontaking_86"))
            aHLL = CastAide.AsDouble(dr("v" & sportTypeId & "alivepositiontaking_87"))
            aNCL = CastAide.AsDouble(dr("v" & sportTypeId & "alivepositiontaking_89"))
            aNGL = CastAide.AsDouble(dr("v" & sportTypeId & "alivepositiontaking_90"))

            m1stOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_81"))
            m1stOE = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_83"))
            'mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_85"))
            mOE = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_86"))
            mWR = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_88"))

            mOUL = CastAide.AsDouble(dr("v" & sportTypeId & "mlivepositiontaking_85"))
            mOEL = CastAide.AsDouble(dr("v" & sportTypeId & "mlivepositiontaking_86"))
            mHLL = CastAide.AsDouble(dr("v" & sportTypeId & "mlivepositiontaking_87"))
            mNCL = CastAide.AsDouble(dr("v" & sportTypeId & "mlivepositiontaking_89"))
            mNGL = CastAide.AsDouble(dr("v" & sportTypeId & "mlivepositiontaking_90"))

            s1stOUMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_81"))
            s1stOEMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_83"))
            'sOUMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_85"))
            sOEMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_86"))
            sWRMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_88"))

            sOUMinL = CastAide.AsDouble(dr("minv" & sportTypeId & "lpos_85"))
            sOEMinL = CastAide.AsDouble(dr("minv" & sportTypeId & "lpos_86"))
            sHLMinL = CastAide.AsDouble(dr("minv" & sportTypeId & "lpos_87"))
            sNCMinL = CastAide.AsDouble(dr("minv" & sportTypeId & "lpos_89"))
            sNGMinL = CastAide.AsDouble(dr("minv" & sportTypeId & "lpos_90"))

            s1stOUMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_81"))
            s1stOEMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_83"))
            'sOUMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_85"))
            sOEMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_86"))
            sWRMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_88"))

            sOUMaxL = CastAide.AsDouble(dr("maxv" & sportTypeId & "lpos_85"))
            sOEMaxL = CastAide.AsDouble(dr("maxv" & sportTypeId & "lpos_86"))
            sHLMaxL = CastAide.AsDouble(dr("maxv" & sportTypeId & "lpos_87"))
            sNCMaxL = CastAide.AsDouble(dr("maxv" & sportTypeId & "lpos_89"))
            sNGMaxL = CastAide.AsDouble(dr("maxv" & sportTypeId & "lpos_90"))

            sAuto1stOUMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_81"))
            sAuto1stOEMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_83"))
            'sAutoOUMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_85"))
            sAutoOEMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_86"))
            sAutoWRMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_88"))

            sAutoOUMaxL = CastAide.AsDouble(dr("maxautov" & sportTypeId & "lpos_85"))
            sAutoOEMaxL = CastAide.AsDouble(dr("maxautov" & sportTypeId & "lpos_86"))
            sAutoHLMaxL = CastAide.AsDouble(dr("maxautov" & sportTypeId & "lpos_87"))
            sAutoNCMaxL = CastAide.AsDouble(dr("maxautov" & sportTypeId & "lpos_89"))
            sAutoNGMaxL = CastAide.AsDouble(dr("maxautov" & sportTypeId & "lpos_90"))

            isAuto1stOU = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_mem_81"))
            isAuto1stOE = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_mem_83"))
            'isAutoOU = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_mem_85"))
            isAutoOE = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_mem_86"))
            isAutoWR = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_mem_88"))

            isAutoOUL = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "slivepositiontaking_mem_85"))
            isAutoOEL = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "slivepositiontaking_mem_86"))
            isAutoHLL = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "slivepositiontaking_mem_87"))
            isAutoNCL = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "slivepositiontaking_mem_89"))
            isAutoNGL = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "slivepositiontaking_mem_90"))
        Else
            s1stOUMax = s1stOU
            s1stOEMax = s1stOE
            'sOUMax = sOU
            sOEMax = sOE
            sWRMax = sWR

            sOUMaxL = sOUL
            sOEMaxL = sOEL
            sHLMaxL = sHLL
            sNCMaxL = sNCL
            sNGMaxL = sNGL

            sAuto1stOUMax = sAuto1stOU
            sAuto1stOEMax = sAuto1stOE
            'sAutoOUMax = sAutoOU
            sAutoOEMax = sAutoOE
            sAutoWRMax = sAutoWR

            sAutoOUMaxL = sAutoOUL
            sAutoOEMaxL = sAutoOEL
            sAutoHLMaxL = sAutoHLL
            sAutoNCMaxL = sAutoNCL
            sAutoNGMaxL = sAutoNGL

        End If

        hiddenHash.Add("isSAuto" & sportTypeId & "1stOU", isSAuto1stOU)
        hiddenHash.Add("isSAuto" & sportTypeId & "1stOE", isSAuto1stOE)
        'hiddenHash.Add("isSAuto" & sportTypeId & "OU", isSAutoOU)
        hiddenHash.Add("isSAuto" & sportTypeId & "OE", isSAutoOE)
        hiddenHash.Add("isSAuto" & sportTypeId & "WR", isSAutoWR)

        hiddenHash.Add("isSAuto" & sportTypeId & "OUL", isSAutoOUL)
        hiddenHash.Add("isSAuto" & sportTypeId & "OEL", isSAutoOEL)
        hiddenHash.Add("isSAuto" & sportTypeId & "HLL", isSAutoHLL)
        hiddenHash.Add("isSAuto" & sportTypeId & "NCL", isSAutoNCL)
        hiddenHash.Add("isSAuto" & sportTypeId & "NGL", isSAutoNGL)

        strHidNumberGame = PositionTaking.AssignMulipleHidden(hiddenHash)

        Dim btnTpl As String = groupBtn

        If isSAuto1stOU Then
            TemplateHandler.Assign(tpl, "p-1stou-btn", String.Empty)
            If isAuto1stOU = 0 Then
                TemplateHandler.Assign(tpl, "chk-1stoud", "disabled")
            End If

        Else
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "1stOUD")
            TemplateHandler.Assign(tpl, "p-1stou-btn", btnTpl)
        End If


        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "1stOUD")
        TemplateHandler.Assign(tpl, "au-1stou-btn", btnTpl)

        'For Live
        If isSAutoOUL Then
            TemplateHandler.Assign(tpl, "p-Lou-btn", String.Empty)
            If isAutoOUL = 0 Then
                TemplateHandler.Assign(tpl, "chk-oul", "disabled")

            End If
        Else
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "OUL")
            TemplateHandler.Assign(tpl, "p-Lou-btn", btnTpl)

        End If


        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "OUL")
        TemplateHandler.Assign(tpl, "au-Lou-btn", btnTpl)
        '---

        Dim ptAuto As String
        Dim strChecked As String

      

        ' for 1st/Last O/U
        If isAuto1stOU Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-1stOUD", strChecked)
        TemplateHandler.Assign(tpl, "a-1stou-val", a1stOU)
        TemplateHandler.Assign(tpl, "m-1stou-val", m1stOU)

        If isSAuto1stOU Then
            TemplateHandler.Assign(tpl, "p-1stou-val", s1stOU)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "1stOUD")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(s1stOUMin, s1stOUMax, s1stOU))
            TemplateHandler.Assign(tpl, "p-1stou-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "1stOUD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAuto1stOUMax, sAuto1stOU))
        TemplateHandler.Assign(tpl, "au-1stou-val", ptAuto)

        ' for 1st/Lat O/E
        If isAuto1stOE Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-1stOED", strChecked)
        TemplateHandler.Assign(tpl, "a-1stoe-val", a1stOE)
        TemplateHandler.Assign(tpl, "m-1stoe-val", m1stOE)

        If isSAuto1stOE Then
            TemplateHandler.Assign(tpl, "p-1stoe-val", sOE)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "1stOED")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(s1stOEMin, s1stOEMax, s1stOE))
            TemplateHandler.Assign(tpl, "p-1stoe-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "1stOED")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAuto1stOEMax, sAuto1stOE))
        TemplateHandler.Assign(tpl, "au-1stoe-val", ptAuto)

     
        'for Odd/Even
        If isAutoOE Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-OED", strChecked)
        TemplateHandler.Assign(tpl, "a-oe-val", aOE)
        TemplateHandler.Assign(tpl, "m-oe-val", mOE)

        If isSAutoOE Then
            TemplateHandler.Assign(tpl, "p-oe-val", sOE)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OED")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOEMin, sOEMax, sOE))
            TemplateHandler.Assign(tpl, "p-oe-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OED")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOEMax, sAutoOE))
        TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)

        ' for Warrior
        If isAutoWR Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-WRD", strChecked)
        TemplateHandler.Assign(tpl, "a-warrior-val", aWR)
        TemplateHandler.Assign(tpl, "m-warrior-val", mWR)

        If isSAutoWR Then
            TemplateHandler.Assign(tpl, "p-warrior-val", sWR)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "WRD")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sWRMin, sWRMax, sWR))
            TemplateHandler.Assign(tpl, "p-warrior-val", pt)
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "WRD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sWRMin, sWRMax, sWR))
        TemplateHandler.Assign(tpl, "p-warrior-val", 0)


        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "WRD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoWRMax, sAutoWR))
        TemplateHandler.Assign(tpl, "au-warrior-val", ptAuto)

        
        ' for Over/Under with Live

        If isAutoOUL Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-LOU", strChecked)
        TemplateHandler.Assign(tpl, "a-Lou-val", aOUL)
        TemplateHandler.Assign(tpl, "m-Lou-val", mOUL)

        If isSAutoOUL Then
            TemplateHandler.Assign(tpl, "p-Lou-val", sOUL)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OUL")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOUMinL, sOUMaxL, sOUL))
            TemplateHandler.Assign(tpl, "p-Lou-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OUL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOUMaxL, sAutoOUL))
        TemplateHandler.Assign(tpl, "au-Lou-val", ptAuto)

        'for Odd/Even with Live
        If isAutoOEL Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-LOE", strChecked)
        TemplateHandler.Assign(tpl, "a-Loe-val", aOEL)
        TemplateHandler.Assign(tpl, "m-Loe-val", mOEL)

        If isSAutoOEL Then
            TemplateHandler.Assign(tpl, "p-Loe-val", sOUL)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OEL")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOEMinL, sOEMaxL, sOEL))
            TemplateHandler.Assign(tpl, "p-Loe-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OEL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOEMaxL, sAutoOEL))
        TemplateHandler.Assign(tpl, "au-Loe-val", ptAuto)

        'For High/Low with Live
        If isAutoHLL Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-LHL", strChecked)
        TemplateHandler.Assign(tpl, "a-Lhl-val", aHLL)
        TemplateHandler.Assign(tpl, "m-Lhl-val", mHLL)

        If isSAutoHLL Then
            TemplateHandler.Assign(tpl, "p-Lhl-val", sHLL)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "HLL")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sHLMinL, sHLMaxL, sHLL))
            TemplateHandler.Assign(tpl, "p-Lhl-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "HLL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoHLMaxL, sAutoHLL))
        TemplateHandler.Assign(tpl, "au-Lhl-val", ptAuto)

        ' for Number Group
        If isAutoNGL Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-NGL", strChecked)
        TemplateHandler.Assign(tpl, "a-NumGr-val", aNGL)
        TemplateHandler.Assign(tpl, "m-NumGr-val", mNGL)

        If isSAutoNGL Then
            TemplateHandler.Assign(tpl, "p-NumGr-val", sNGL)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "NGL")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sNGMinL, sNGMaxL, sNGL))
            TemplateHandler.Assign(tpl, "p-NumGr-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "NGL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoNGMaxL, sAutoNGL))
        TemplateHandler.Assign(tpl, "au-NumGr-val", ptAuto)

        ' for Next Combo in Live
        If isAutoNCL Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-NCL", strChecked)
        TemplateHandler.Assign(tpl, "a-Lcb-val", aNCL)
        TemplateHandler.Assign(tpl, "m-Lcb-val", mNCL)

        If isSAutoNCL Then
            TemplateHandler.Assign(tpl, "p-Lcb-val", sNCL)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "NCL")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sNCMinL, sNCMaxL, sNCL))
            TemplateHandler.Assign(tpl, "p-Lcb-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "NCL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoNCMaxL, sAutoNCL))
        TemplateHandler.Assign(tpl, "au-Lcb-val", ptAuto)


        Return tpl
    End Function
#End Region

#Region "Assign info for Mix Sports"

    Function GetDataMixSports(ByVal dr As DataRow, Optional ByVal isMult As Boolean = False) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable
        Dim tpl As String = TemplateHandler.LoadTemplate("MemberInfo/MixSportsEditMinPT.html")

        GetTitleCommon(tpl)
        If isMult Then
            TemplateHandler.RemoveBlock(tpl, "none-Multi")
            Dim chkUpdateMultiple As String = LoadCheckBoxUpdateMultipe("99_M", Resources.Lang.positiontaking)
            TemplateHandler.Assign(tpl, "ChkUpdatePT", chkUpdateMultiple)
        Else
            TemplateHandler.Assign(tpl, "ChkUpdatePT", String.Empty)
        End If
        TemplateHandler.Assign(tpl, "cols-a", "2")
        TemplateHandler.Assign(tpl, "master-text", Resources.Lang.master)
        TemplateHandler.Assign(tpl, "mp-text", Resources.Lang.lblMixParlay)

        'assign value for position taking           
        Dim mMP As Double
        Dim aMP As Double

        Dim sMPMin As Double
        Dim sMP As Double
        Dim sMPMax As Double

        Dim sAutoMP As Double
        Dim sAutoMPMax As Double
        Dim isAutoMP As Double

        Dim isSAutoMP As Double

        sMP = CastAide.AsDouble(dr("v99spositiontaking_9"))
        sAutoMP = CastAide.AsDouble(dr("auto_v99spositiontaking_9"))
        isSAutoMP = CastAide.AsDouble(dr("isauto_v99spositiontaking_9"))

        If Not isMult Then
            aMP = CastAide.AsDouble(dr("v99apositiontaking_9"))
            mMP = CastAide.AsDouble(dr("v99mpositiontaking_9"))
            sMPMin = CastAide.AsDouble(dr("minv99pos_9"))
            sMPMax = CastAide.AsDouble(dr("maxv99pos_9"))
            sAutoMPMax = CastAide.AsDouble(dr("maxautov99pos_9"))
            isAutoMP = CastAide.AsInt16(dr("isauto_v99spositiontaking_mem_9"))
        Else
            sMPMax = sMP
            sAutoMPMax = sAutoMP
        End If

        hiddenHash.Add("isSAuto99_M", isSAutoMP)
        strHidMixSports = PositionTaking.AssignMulipleHidden(hiddenHash)

        Dim btnTpl As String = groupBtn
        If isSAutoMP Then
            TemplateHandler.Assign(tpl, "p-mp-btn", String.Empty)
            If isAutoMP = 0 Then
                TemplateHandler.Assign(tpl, "chk-mp", "disabled")
            End If
        Else
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS99_M")
            TemplateHandler.Assign(tpl, "p-mp-btn", btnTpl)
        End If

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS99_M")
        TemplateHandler.Assign(tpl, "au-mp-btn", btnTpl)

        Dim ptAuto As String
        Dim strChecked As String
        If isAutoMP Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-MP", strChecked)
        TemplateHandler.Assign(tpl, "a-mp-val", aMP)
        TemplateHandler.Assign(tpl, "m-mp-val", mMP)

        If isSAutoMP Then
            TemplateHandler.Assign(tpl, "p-mp-val", sMP)
        Else
            TemplateHandler.Assign(pt, "ddl-Id", "listS99_M")
            TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sMPMin, sMPMax, sMP))
            TemplateHandler.Assign(tpl, "p-mp-val", pt)
        End If

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS99_M")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoMPMax, sAutoMP))
        TemplateHandler.Assign(tpl, "au-mp-val", ptAuto)


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

        TemplateHandler.Assign(tpl, "commission-text", Resources.Lang.commission)
        TemplateHandler.Assign(tpl, "othersetting-text", Resources.Lang.othersetting)
        TemplateHandler.Assign(tpl, "maxperball-text", Resources.Lang.NumberGameMaxperball)
        '
        TemplateHandler.Assign(tpl, "ma-comm-text", Resources.Lang.lblCommissionForMaster & " <u>" & Resources.Lang.lblAsianHDC_OU_OE & " </u>")
        TemplateHandler.Assign(tpl, "ma-commo-text", Resources.Lang.lblCommissionForMaster & " <u>" & Resources.Lang.lblOtherTypes & " </u>")
        TemplateHandler.Assign(tpl, "a-comm-text", Resources.Lang.lblCommissionForAgent & " <u>" & Resources.Lang.lblAsianHDC_OU_OE & " </u>")
        TemplateHandler.Assign(tpl, "a-commo-text", Resources.Lang.lblCommissionForAgent & " <u>" & Resources.Lang.lblOtherTypes & " </u>")
        TemplateHandler.Assign(tpl, "comm-text", Resources.Lang.lblCommissionForMember & " <u>" & Resources.Lang.lblAsianHDC_OU_OE & " </u>")
        TemplateHandler.Assign(tpl, "commo-text", Resources.Lang.lblCommissionForMember & " <u>" & Resources.Lang.lblOtherTypes & " </u>")

        TemplateHandler.Assign(tpl, "ma-comm1x2-text", Resources.Lang.lblCommissionForMaster & " <u>" & Resources.Lang.lbl1x2 & " </u>")
        TemplateHandler.Assign(tpl, "a-comm1x2-text", Resources.Lang.lblCommissionForAgent & " <u>" & Resources.Lang.lbl1x2 & " </u>")
        TemplateHandler.Assign(tpl, "comm1x2-text", Resources.Lang.lblCommissionForMember & " <u>" & Resources.Lang.lbl1x2 & " </u>")

        TemplateHandler.Assign(tpl, "ma-commng-text", IIf(Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission, String.Empty, Resources.Lang.lblCommissionForMaster & " <u>" & Resources.Lang.numbergame & " </u>"))
        TemplateHandler.Assign(tpl, "a-commng-text", IIf(Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission, String.Empty, Resources.Lang.lblCommissionForAgent & " <u>" & Resources.Lang.numbergame & " </u>"))
        TemplateHandler.Assign(tpl, "commng-text", IIf(Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission, String.Empty, Resources.Lang.lblCommissionForMember & " <u>" & Resources.Lang.numbergame & " </u>"))

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

        TemplateHandler.Assign(tpl, "numbergame-text", IIf(UserPermission.IsShowNumberGame, Resources.Lang.numbergame, String.Empty))
        TemplateHandler.Assign(tpl, "mixsports-text", Resources.Lang.mixsports)


        TemplateHandler.Assign(tpl, "add-text", Resources.Lang.submit)
        TemplateHandler.Assign(tpl, "add-id", "btnSubmit")
    End Sub
#End Region

#Region "Get Title Common"
    Sub GetTitleCommon(ByRef tpl As String)
        TemplateHandler.Assign(tpl, "pt-text", Resources.Lang.lblPositionTaking)
        TemplateHandler.Assign(tpl, "au-presetpt-text", Resources.Lang.lblSuperAuto)
        TemplateHandler.Assign(tpl, "a-presetpt-text", Resources.Lang.agent)
        TemplateHandler.Assign(tpl, "p-presetpt-text", Resources.Lang.lblSuper)
        TemplateHandler.Assign(tpl, "hdp-text", Resources.Lang.lblHandicapFC)
        TemplateHandler.Assign(tpl, "ml-text", Resources.Lang.moneyline)
        TemplateHandler.Assign(tpl, "copy-hdp-text", Resources.Lang.lblCopyFromHandicap)
        TemplateHandler.Assign(tpl, "copy-hdp-col", "3")
        TemplateHandler.Assign(tpl, "master-text", Resources.Lang.master)
    End Sub
#End Region

    '-----------------------------------------------------
#Region "ReplaceData"
    Public Function ReplaceData(ByVal str As String) As String
        Return str.Replace("""", "&quot;").Replace("'", "&#39;").Replace(vbNewLine, "")
    End Function
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

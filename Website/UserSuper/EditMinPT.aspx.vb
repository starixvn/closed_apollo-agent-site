'Imports IBCDB
Imports DNAide.Core
Imports DNAide.Web
Imports Fanex.Data.DNAide
Imports DNAide.Utils
Imports System.Data
Imports System.Text
Imports Sunplus.Agent.Model.Entities.Permission
Public Class EditMinPT
    Inherits IbcPage2
    Public style As String
    Dim strContent As String
    Dim bodyTpl, for4ddlTpl, for3ddlTpl, basketballTpl As String
    Dim groupBtn As String
    Dim selectTpl As String
    Public strUser As String
    Dim strPre As String
    Dim strHidSoccer, strHidBasket, strHidFootball, strHidTennis As String
    Dim strHidBaseball, strHidGolf, strHidMoto, strHidOther As String
    Protected disabledBtn As String = "", script As String = "", Level As String = "Super"
    Dim selDisabledTpl As String

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents isMult As System.Web.UI.HtmlControls.HtmlInputHidden

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

        If (AppSettings.IsSuperSite) Then
            Level = "Super"
        ElseIf (AppSettings.IsMasterSite) Then
            Level = "Master"
        ElseIf (AppSettings.IsAgentSite) Then
            Level = "Agent"
        End If

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
                hash.Add("FromAccount", "../MemberInfo/Downlinelist.aspx")
            Else
                hash("FromAccount") = "../MemberInfo/Downlinelist.aspx"
            End If
            AppSession.RequestQueryString = hash
        End If

        saveCustid.Value = Request.QueryString("custid")
        If (Not Request("username") Is Nothing) Then
            strUser = Request.QueryString("username")
            currentUser.Value = Request.QueryString("username").Trim()
        End If


        strContent = LoadDataSingle()

        TemplateHandler.Assign(strContent, "ChkUpdatePT", String.Empty)

        htmlEditMinPT.Text = strContent

        'show message successfully
        If (Not String.IsNullOrEmpty(Request.QueryString("succ"))) Then
            htmlEditMinPT.Text += String.Format(" <script>showErrMsg('{0}', 1);</script>", Resources.Lang.updatesuccessfull)
        End If



    End Sub

#Region "Load Information for Single Edit Member"
    Function LoadDataSingle() As String
        Dim strEditContent As New StringBuilder

        Dim rs As DbResult


        Dim hash As New Hashtable
        hash.Add("custid", saveCustid.Value)
        If Sunplus.Agent.Model.Entities.UserSession.Current.RoleId = 4 Then
            rs = AccountInfo.Acc_CustomerSelComEditOverride_MAg(hash)
            strPre = "m"
        ElseIf Sunplus.Agent.Model.Entities.UserSession.Current.RoleId = 3 Then
            hash.Add("mrecommend", Sunplus.Agent.Model.Entities.UserSession.Current.CustId)
            rs = AccountInfo.Acc_CustomerSelComEditOverride_Ag(hash)
            strPre = "a"
        End If

        If Not rs Is Nothing Then
            If rs.ResultSet.Tables(0).Rows.Count > 0 Then
                Dim row As DataRow = rs.ResultSet.Tables(0).Rows(0)
                strEditContent.Append(GetDataInfo(row))
                Common.GetSportTitle(bodyTpl, True)
                TemplateHandler.Assign(bodyTpl, "add-text", Resources.Lang.submit)
                TemplateHandler.Assign(bodyTpl, "add-id", "btnSubmitFooter")
                TemplateHandler.Assign(bodyTpl, "soccerPT", GetDataSoccer(row, strPre))
                TemplateHandler.Assign(bodyTpl, "basketPT", GetDataBasketball(row, 2, basketballTpl, strPre))
                TemplateHandler.Assign(bodyTpl, "footballPT", GetData4DDl(row, 3, for4ddlTpl, strPre))
                TemplateHandler.Assign(bodyTpl, "tennisPT", GetData4DDl(row, 5, for4ddlTpl, strPre))
                TemplateHandler.Assign(bodyTpl, "baseballPT", GetDataBaseball(row, 8, strPre))
                TemplateHandler.Assign(bodyTpl, "golfPT", GetData4DDl(row, 10, for4ddlTpl, strPre))
                TemplateHandler.Assign(bodyTpl, "motoPT", GetDataMotoSports(row, 11, strPre))

                If UserPermission.IsShowNumberGame Then
                    TemplateHandler.Assign(bodyTpl, "numbergamePT", GetDataNumberGame(row, strPre))
                Else
                    TemplateHandler.Assign(bodyTpl, "numbergamePT", "")
                End If

                TemplateHandler.Assign(bodyTpl, "otherPT", GetData4DDl(row, 99, for4ddlTpl, strPre))

                If UserPermission.IsShowMixedSport Then
                    TemplateHandler.Assign(bodyTpl, "mixSportPT", GetDataMixSport(row, strPre))
                End If

                TemplateHandler.RemoveBlock(bodyTpl, "none-edit-Min")
                strEditContent.Append(bodyTpl)
            End If
        End If

        LoadDataSingle = strEditContent.ToString()
    End Function
#End Region

#Region "Assign info about UNam, Max Credit, Commission ..."
    Function GetDataInfo(ByVal dr As DataRow) As String
        Dim strAccInfo As String = String.Empty

        strAccInfo = TemplateHandler.LoadTemplate("MemberInfo/AccInfoNoneEdit.html")
        'Dim errorTpl As String = TemplateHandler.LoadTemplate("ErrorMsg.html")
        'TemplateHandler.Assign(strAccInfo, "error-message", errorTpl)

        strAccInfo = Common.ReplaceWithPopupStyle(strAccInfo, 5, 0)

        TemplateHandler.RemoveBlock(strAccInfo, "none-edit-min")
        GetTitleMember(strAccInfo)
        'TemplateHandler.Assign(strAccInfo, "title", Resources.Lang.edit + " " + CastAide.AsString(dr("username")))
        TemplateHandler.Assign(strAccInfo, "title", Resources.Lang.edit + " " + Resources.Lang.minposition + " " + CastAide.AsString(dr("username")))
        TemplateHandler.Assign(strAccInfo, "uname-val", HttpUtility.HtmlEncode(CastAide.AsString(dr("username"))))
        TemplateHandler.Assign(strAccInfo, "fname-val", HttpUtility.HtmlEncode(CastAide.AsString(dr("firstname"))))
        TemplateHandler.Assign(strAccInfo, "lname-val", HttpUtility.HtmlEncode(CastAide.AsString(dr("lastname"))))
        TemplateHandler.Assign(strAccInfo, "email-val", HttpUtility.HtmlEncode(CastAide.AsString(dr("fax"))))
        TemplateHandler.Assign(strAccInfo, "phone-val", HttpUtility.HtmlEncode(CastAide.AsString(dr("phone"))))
        TemplateHandler.Assign(strAccInfo, "mobile-val", HttpUtility.HtmlEncode(CastAide.AsString(dr("mobilephone"))))

        GetDataInfo = strAccInfo
    End Function
#End Region

#Region "Assign info for Mix Sport"
    Function GetDataMixSport(ByVal dr As DataRow, ByVal pre As String) As String
        Dim MixSports As String = TemplateHandler.LoadTemplate("MemberInfo/EditMinPTMixSports.html")

        TemplateHandler.Assign(MixSports, "mp-text", Resources.Lang.lblMixParlay)
        Dim pt As String
        Dim hiddenHash As New Hashtable


        Dim mMPMin As Double
        Dim mMP As Double
        Dim mMPMax As Double

        mMPMax = CastAide.AsDouble(dr("v99" & pre & "positiontaking_9"))
        mMP = CastAide.AsDouble(dr("v99" & pre & "positiontaking_9_min"))


        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM99_M")
        TemplateHandler.Assign(MixSports, "mp-btn", btnTpl)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM99_M")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMPMin, mMPMax, mMP))
        TemplateHandler.Assign(MixSports, "mp-val", pt)




        Return MixSports
    End Function
#End Region

#Region "Assign Info for Soccer"
    'this function is used to assign data into template for Soccer
    'dr: row contains data get from db
    Function GetDataSoccer(ByVal dr As DataRow, Optional ByVal pre As String = "") As String
        Dim soccerTpl As String = String.Empty
        Dim headerSTpl As String = String.Empty
        Dim hiddenHash As New Hashtable

        soccerTpl = TemplateHandler.LoadTemplate("MemberInfo/SoccerEditMPT.html")
        headerSTpl = TemplateHandler.LoadTemplate("MemberInfo/SBettypeHeader.html")
        TemplateHandler.RemoveBlock(soccerTpl, "none-edit-min")
        TemplateHandler.RemoveBlock(soccerTpl, "none-edit-min")
        TemplateHandler.RemoveBlock(soccerTpl, "none-edit-min")
        TemplateHandler.RemoveBlock(soccerTpl, "none-edit-min")
        TemplateHandler.Assign(soccerTpl, "dead-row", "2")
        TemplateHandler.Assign(soccerTpl, "live-row", "1")
        TemplateHandler.Assign(soccerTpl, "col-title", "11")
        TemplateHandler.Assign(soccerTpl, "cols-live", "2")
        TemplateHandler.Assign(soccerTpl, "bettype-header", LoadSoccerPTHeader(True))
        GetTitleCommon(soccerTpl)
        TemplateHandler.Assign(soccerTpl, "deadball-text", Resources.Lang.lblDeadball)
        TemplateHandler.Assign(soccerTpl, "live-text", Resources.Lang.lblLive)
        TemplateHandler.Assign(soccerTpl, "copy-live-col", "1")
        TemplateHandler.Assign(soccerTpl, "copy-1x2-text", Resources.Lang.lblCopyFrom1x2)
        'assign value of Minbet, Maxbet , ...           

        'assign value position taking for template                      
        Dim mHdpMin, mOUMin, m1stHdpMin, m1stOUMin, mOEMin, m1x2Min, mCSMin, mTGMin, mMPMin, mORMin As Double
        Dim mLHdpMin, mLOUMin, mL1stHdpMin, mL1stOUMin As Double

        Dim mHdp, mOU, m1stHdp, m1stOU, mOE, m1x2, mCS, mTG, mMP, mOR As Double
        Dim mLHdp, mLOU, mL1stHdp, mL1stOU As Double

        Dim mHdpMax, mOUMax, m1stHdpMax, m1stOUMax, mOEMax, m1x2Max, mCSMax, mTGMax, mMPMax, mORMax As Double
        Dim mLHdpMax, mLOUMax, mL1stHdpMax, mL1stOUMax As Double

        mHdpMax = CastAide.AsDouble(dr(pre & "positiontaking_1"))
        mOUMax = CastAide.AsDouble(dr(pre & "positiontaking_3"))
        m1stHdpMax = CastAide.AsDouble(dr(pre & "positiontaking_7"))
        m1stOUMax = CastAide.AsDouble(dr(pre & "positiontaking_8"))
        mOEMax = CastAide.AsDouble(dr(pre & "positiontaking_2"))
        m1x2Max = CastAide.AsDouble(dr(pre & "positiontaking_5"))
        mCSMax = CastAide.AsDouble(dr(pre & "positiontaking_4"))
        mTGMax = CastAide.AsDouble(dr(pre & "positiontaking_6"))
        mMPMax = CastAide.AsDouble(dr(pre & "positiontaking_9"))
        mORMax = CastAide.AsDouble(dr(pre & "positiontaking_10"))
        mLHdpMax = CastAide.AsDouble(dr(pre & "livepositiontaking_1"))
        mLOUMax = CastAide.AsDouble(dr(pre & "livepositiontaking_3"))
        mL1stHdpMax = CastAide.AsDouble(dr(pre & "livepositiontaking_7"))
        mL1stOUMax = CastAide.AsDouble(dr(pre & "livepositiontaking_8"))

        mHdp = CastAide.AsDouble(dr(pre & "positiontaking_1_min"))
        mOU = CastAide.AsDouble(dr(pre & "positiontaking_3_min"))
        m1stHdp = CastAide.AsDouble(dr(pre & "positiontaking_7_min"))
        m1stOU = CastAide.AsDouble(dr(pre & "positiontaking_8_min"))
        mOE = CastAide.AsDouble(dr(pre & "positiontaking_2_min"))
        m1x2 = CastAide.AsDouble(dr(pre & "positiontaking_5_min"))
        mCS = CastAide.AsDouble(dr(pre & "positiontaking_4_min"))
        mTG = CastAide.AsDouble(dr(pre & "positiontaking_6_min"))
        mMP = CastAide.AsDouble(dr(pre & "positiontaking_9_min"))
        mOR = CastAide.AsDouble(dr(pre & "positiontaking_10_min"))
        mLHdp = CastAide.AsDouble(dr(pre & "livepositiontaking_1_min"))
        mLOU = CastAide.AsDouble(dr(pre & "livepositiontaking_3_min"))
        mL1stHdp = CastAide.AsDouble(dr(pre & "livepositiontaking_7_min"))
        mL1stOU = CastAide.AsDouble(dr(pre & "livepositiontaking_8_min"))

        'assign Increase and Reduce button for template
        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM1HdpD")
        TemplateHandler.Assign(soccerTpl, "p-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM11x2")
        TemplateHandler.Assign(soccerTpl, "p-1x2-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM1HdpL")
        TemplateHandler.Assign(soccerTpl, "p-Lhdp-btn", btnTpl)

        Dim pt As String = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1HdpD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
        TemplateHandler.Assign(soccerTpl, "p-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1OUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOU))
        TemplateHandler.Assign(soccerTpl, "p-ou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM11stHdpD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1stHdpMin, m1stHdpMax, m1stHdp))
        TemplateHandler.Assign(soccerTpl, "p-1sthdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM11stOUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1stOUMin, m1stOUMax, m1stOU))
        TemplateHandler.Assign(soccerTpl, "p-1stou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1OE")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOE))
        TemplateHandler.Assign(soccerTpl, "p-oe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM11x2")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1x2Min, m1x2Max, m1x2))
        TemplateHandler.Assign(soccerTpl, "p-1x2-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1CS")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mCSMin, mCSMax, mCS))
        TemplateHandler.Assign(soccerTpl, "p-cs-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1TG")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mTGMin, mTGMax, mTG))
        TemplateHandler.Assign(soccerTpl, "p-tg-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1MP")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMPMin, mMPMax, mMP))
        TemplateHandler.Assign(soccerTpl, "p-mp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1OR")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mORMin, mORMax, mOR))
        TemplateHandler.Assign(soccerTpl, "p-or-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1HdpL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLHdpMin, mLHdpMax, mLHdp))
        TemplateHandler.Assign(soccerTpl, "p-Lhdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1OUL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLOUMin, mLOUMax, mLOU))
        TemplateHandler.Assign(soccerTpl, "p-Lou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM11stHdpL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mL1stHdpMin, mL1stHdpMax, mL1stHdp))
        TemplateHandler.Assign(soccerTpl, "p-L1stHdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM11stOUL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mL1stOUMin, mL1stOUMax, mL1stOU))
        TemplateHandler.Assign(soccerTpl, "p-L1stou-val", pt)

        GetDataSoccer = soccerTpl
    End Function
#End Region

#Region "Assign info for Number Game"
    Function GetDataNumberGame(ByVal dr As DataRow, ByVal strPre As String) As String
        Dim numbergameTpl As String = String.Empty

        numbergameTpl = TemplateHandler.LoadTemplate("MemberInfo/NumberGameEditMinPT.html")
        TemplateHandler.Assign(numbergameTpl, "dead-row", "2")
        TemplateHandler.Assign(numbergameTpl, "live-row", "1")
        TemplateHandler.Assign(numbergameTpl, "col-title", "11")
        TemplateHandler.Assign(numbergameTpl, "cols-live", "2")
        TemplateHandler.Assign(numbergameTpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(numbergameTpl, "oe-text", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(numbergameTpl, "hl-text", Resources.Lang.highlow)
        TemplateHandler.Assign(numbergameTpl, "ng-text", Resources.Lang.numberGroup)
        TemplateHandler.Assign(numbergameTpl, "cb-text", Resources.Lang.nextcombo)
        TemplateHandler.Assign(numbergameTpl, "copy-ou-text", Resources.Lang.lblcopyfromoverunder)
        TemplateHandler.Assign(numbergameTpl, "copy-1stlastou-text", Resources.Lang.lbl1stcopyfromoverunder)
        TemplateHandler.Assign(numbergameTpl, "bettype-header", LoadNumberGameHeader(True))
        GetTitleCommon(numbergameTpl)
        TemplateHandler.Assign(numbergameTpl, "deadball-text", Resources.Lang.lblDeadball)
        TemplateHandler.Assign(numbergameTpl, "live-text", Resources.Lang.lblLive)
        TemplateHandler.Assign(numbergameTpl, "copy-live-col", "1")
        TemplateHandler.Assign(numbergameTpl, "copy-1x2-text", Resources.Lang.lblCopyFrom1x2)
        'assign value of Minbet, Maxbet , ...   
        If (Not UserPermission.IsShowNumberGroup) Then
            TemplateHandler.Assign(numbergameTpl, "hide", "hide")
        End If

        Dim m1stOUMin, m1stOEMin, mOEMin, mWRMin As Double
        Dim m1stOU, m1stOE, mOE, mWR As Double
        Dim m1stOUMax, m1stOEMax, mOEMax, mWRMax As Double

        Dim mOUMinL, mOEMinL, mHLMinL, mNCMinL, mNGMinL As Double
        Dim mOUL, mOEL, mHLL, mNCL, mNGL As Double
        Dim mOUMaxL, mOEMaxL, mHLMaxL, mNCMaxL, mNGMaxL As Double

        m1stOUMax = CastAide.AsDouble(dr("v161" + strPre + "positiontaking_81"))
        m1stOEMax = CastAide.AsDouble(dr("v161" + strPre + "positiontaking_83"))
        'mOUMax = CastAide.AsDouble(dr("v161" + strPre + "positiontaking_85"))
        mOEMax = CastAide.AsDouble(dr("v161" + strPre + "positiontaking_86"))
        mWRMax = CastAide.AsDouble(dr("v161" + strPre + "positiontaking_88"))

        mOUMaxL = CastAide.AsDouble(dr("v161" + strPre + "livepositiontaking_85"))
        mOEMaxL = CastAide.AsDouble(dr("v161" + strPre + "livepositiontaking_86"))
        mHLMaxL = CastAide.AsDouble(dr("v161" + strPre + "livepositiontaking_87"))
        mNCMaxL = CastAide.AsDouble(dr("v161" + strPre + "livepositiontaking_89"))
        mNGMaxL = CastAide.AsDouble(dr("v161" + strPre + "livepositiontaking_90"))

        m1stOU = CastAide.AsDouble(dr("v161" + strPre + "positiontaking_81_min"))
        m1stOE = CastAide.AsDouble(dr("v161" + strPre + "positiontaking_83_min"))
        'mOU = CastAide.AsDouble(dr("v161" + strPre + "positiontaking_85_min"))
        mOE = CastAide.AsDouble(dr("v161" + strPre + "positiontaking_86_min"))
        mWR = CastAide.AsDouble(dr("v161" + strPre + "positiontaking_88_min"))

        mOUL = CastAide.AsDouble(dr("v161" + strPre + "livepositiontaking_85_min"))
        mOEL = CastAide.AsDouble(dr("v161" + strPre + "livepositiontaking_86_min"))
        mHLL = CastAide.AsDouble(dr("v161" + strPre + "livepositiontaking_87_min"))
        mNCL = CastAide.AsDouble(dr("v161" + strPre + "livepositiontaking_89_min"))
        mNGL = CastAide.AsDouble(dr("v161" + strPre + "livepositiontaking_90_min"))

        Dim btnTpl As String = String.Empty
        Dim pt As String = String.Empty

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM1611stOUD")
        TemplateHandler.Assign(numbergameTpl, "p-1stlastou-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM161OUL")
        TemplateHandler.Assign(numbergameTpl, "p-Lou-btn", btnTpl)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1611stOUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1stOUMin, m1stOUMax, m1stOU))
        TemplateHandler.Assign(numbergameTpl, "p-1stlastou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1611stOED")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1stOEMin, m1stOEMax, m1stOE))
        TemplateHandler.Assign(numbergameTpl, "p-1stlastoe-val", pt)

        
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161OED")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOE))
        TemplateHandler.Assign(numbergameTpl, "p-oe-val", pt)

        'insert bettype "warrior"  by david.tran on 27/7/2011
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161WRD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mWRMin, mWRMax, mWR))
        TemplateHandler.Assign(numbergameTpl, "p-w-val", pt)


        '' insert bettype "Next Combo" and "Number Group"
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161NCL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mNCMinL, mNCMaxL, mNCL))
        TemplateHandler.Assign(numbergameTpl, "p-Lcb-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161NGL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mNGMinL, mNGMaxL, mNGL))
        TemplateHandler.Assign(numbergameTpl, "p-ng-val", pt)
        ''----
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161OUL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOUMinL, mOUMaxL, mOUL))
        TemplateHandler.Assign(numbergameTpl, "p-Lou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161OEL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOEMinL, mOEMaxL, mOEL))
        TemplateHandler.Assign(numbergameTpl, "p-Loe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161HLL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHLMinL, mHLMaxL, mHLL))
        TemplateHandler.Assign(numbergameTpl, "p-Lhl-val", pt)


        Return numbergameTpl
    End Function

#End Region

#Region "Assign info for  Basketball"
    'this function is used to assign data into template for all sporttype that have 4 bettype
    '(1:Handicap, 3:Over/Under, 2:Odd/Even,9:Mix Parlay, 20:Money Line)
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (2: Basketball)
    Function GetDataBasketball(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String, Optional ByVal pre As String = "") As String
        Dim pt As String
        Dim hiddenHash As New Hashtable

        'assign text for position taking
        GetTitleCommon(tpl)
        TemplateHandler.RemoveBlock(tpl, "none-edit-min")
        TemplateHandler.RemoveBlock(tpl, "none-edit-min")
        TemplateHandler.RemoveBlock(tpl, "none-edit-min")
        TemplateHandler.Assign(tpl, "sportId", sportTypeId)
        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "mp-text", Resources.Lang.lblMixParlay)
        TemplateHandler.Assign(tpl, "oe-text", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(tpl, "copy-hdp-col", "5")
        TemplateHandler.Assign(tpl, "title-col", "6")

        Dim mHdpMin, mOUMin, mOEMin, mMLMin As Double
        Dim mHdp, mOU, mOE, mML As Double
        Dim mHdpMax, mOUMax, mOEMax, mMLMax As Double

        mHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_1"))
        mOUMax = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_3"))
        mOEMax = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_2"))
        mMLMax = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_20"))

        mHdp = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_1_min"))
        mOU = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_3_min"))
        mOE = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_2_min"))
        mML = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_20_min"))

        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
        TemplateHandler.Assign(tpl, "p-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOU))
        TemplateHandler.Assign(tpl, "p-ou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OE")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOE))
        TemplateHandler.Assign(tpl, "p-oe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mML))
        TemplateHandler.Assign(tpl, "p-ml-val", pt)

        GetDataBasketball = tpl
    End Function
#End Region

#Region "Assign info for  Football, Tennis, Golf, OtherSports"
    'this function is used to assign data into template for all sporttype that have 4 bettype
    '(1:Handicap, 3:Over/Under, 2:Odd/Even, 20:Money Line)
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (3:Football, 5:Tennis, 10:Golf, 99:OtherSports )
    Function GetData4DDl(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String, Optional ByVal pre As String = "") As String
        Dim pt As String
        Dim hiddenHash As New Hashtable

        'assign text for position taking
        GetTitleCommon(tpl)
        TemplateHandler.RemoveBlock(tpl, "none-edit-min")
        TemplateHandler.RemoveBlock(tpl, "none-edit-min")
        TemplateHandler.RemoveBlock(tpl, "none-edit-min")
        TemplateHandler.Assign(tpl, "sportId", sportTypeId)
        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "oe-text", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(tpl, "copy-hdp-col", "5")
        TemplateHandler.Assign(tpl, "title-col", "5")

        Dim mHdpMin, mOUMin, mOEMin, mMLMin As Double
        Dim mHdp, mOU, mOE, mML As Double
        Dim mHdpMax, mOUMax, mOEMax, mMLMax As Double

        mHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_1"))
        mOUMax = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_3"))
        mOEMax = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_2"))
        mMLMax = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_20"))

        mHdp = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_1_min"))
        mOU = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_3_min"))
        mOE = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_2_min"))
        mML = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_20_min"))

        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
        TemplateHandler.Assign(tpl, "p-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOU))
        TemplateHandler.Assign(tpl, "p-ou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OE")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOE))
        TemplateHandler.Assign(tpl, "p-oe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mML))
        TemplateHandler.Assign(tpl, "p-ml-val", pt)

        GetData4DDl = tpl
    End Function
#End Region

#Region "Assign info for  Baseball"
    'this function is used to assign data into template for Baseball
    'dr: row contains data get from db
    'sportTypeId: id of sporttype
    'Baseball have only Handicap, Over/Under & Money Line bettype
    Function GetDataBaseball(ByVal dr As DataRow, ByVal sportTypeId As Integer, Optional ByVal pre As String = "") As String
        Dim pt As String
        Dim tpl As String
        Dim hiddenHash As New Hashtable

        'assign text for position taking
        tpl = TemplateHandler.LoadTemplate("MemberInfo/Edit3Bettype.html")
        TemplateHandler.RemoveBlock(tpl, "none-edit-min")
        TemplateHandler.RemoveBlock(tpl, "none-edit-min")
        TemplateHandler.RemoveBlock(tpl, "none-edit-min")
        GetTitleCommon(tpl)
        TemplateHandler.Assign(tpl, "sportId", sportTypeId)
        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "copy-hdp-col", "1")
        TemplateHandler.Assign(tpl, "title-col", "4")
        TemplateHandler.Assign(tpl, "cols-a", "5")
        TemplateHandler.Assign(tpl, "p-ou-btn", String.Empty)
        TemplateHandler.Assign(tpl, "150px", "150px")
        Dim mHdpMin, mOUMin, mMLMin As Double
        Dim mHdp, mOU, mML As Double
        Dim mHdpMax, mOUMax, mMLMax As Double

        mHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_1"))
        mOUMax = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_3"))
        mMLMax = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_20"))

        mHdp = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_1_min"))
        mOU = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_3_min"))
        mML = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_20_min"))

        'assign data for Position taking dropdowns          
        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
        TemplateHandler.Assign(tpl, "p-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOU))
        TemplateHandler.Assign(tpl, "p-ou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mML))
        TemplateHandler.Assign(tpl, "p-ml-val", pt)

        GetDataBaseball = tpl
    End Function
#End Region

#Region "Assign info for MotoSports"
    'this function is used to assign data into template for MotoSports
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (11: motosports)
    'Motosports have only Handicap & Money Line bettype
    Function GetDataMotoSports(ByVal dr As DataRow, ByVal sportTypeId As Integer, Optional ByVal pre As String = "") As String
        Dim pt As String
        Dim tpl As String
        Dim hiddenHash As New Hashtable

        'assign text for position taking
        tpl = TemplateHandler.LoadTemplate("MemberInfo/Edit2Bettype.html")
        TemplateHandler.RemoveBlock(tpl, "none-edit-min")
        TemplateHandler.RemoveBlock(tpl, "none-edit-min")
        TemplateHandler.RemoveBlock(tpl, "none-edit-min")
        GetTitleCommon(tpl)
        TemplateHandler.Assign(tpl, "copy-hdp-col", "1")
        TemplateHandler.Assign(tpl, "cols-a", "1")
        TemplateHandler.Assign(tpl, "title-col", "4")
        'get data and assign for template Position taking
        Dim mHdpMin, mMLMin As Double
        Dim mHdp, mML As Double
        Dim mHdpMax, mMLMax As Double
        mHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_1"))
        mMLMax = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_20"))

        mHdp = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_1_min"))
        mML = CastAide.AsDouble(dr("v" & sportTypeId & pre & "positiontaking_20_min"))

        'assign data for Position taking dropdowns     
        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
        TemplateHandler.Assign(tpl, "p-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mML))
        TemplateHandler.Assign(tpl, "p-ml-val", pt)

        GetDataMotoSports = tpl
    End Function
#End Region

#Region "Get Title Common"
    Sub GetTitleMember(ByRef tpl)
        TemplateHandler.Assign(tpl, "uname-text", Resources.Lang.username)
        TemplateHandler.Assign(tpl, "fname-text", Resources.Lang.firstname)
        TemplateHandler.Assign(tpl, "lname-text", Resources.Lang.lastname)
        TemplateHandler.Assign(tpl, "email-text", Resources.Lang.lblFax)

        TemplateHandler.Assign(tpl, "phone-text", Resources.Lang.lblPhone)
        TemplateHandler.Assign(tpl, "mobile-text", Resources.Lang.lblMobilePhone)
        TemplateHandler.Assign(tpl, "add-text", Resources.Lang.submit)
        TemplateHandler.Assign(tpl, "add-id", "btnSubmit")
    End Sub


    Sub GetTitleCommon(ByRef tpl As String)
        TemplateHandler.Assign(tpl, "pt-text", Resources.Lang.lblPositionTaking)
        TemplateHandler.Assign(tpl, "hdp-text", Resources.Lang.lblHandicapFC)
        TemplateHandler.Assign(tpl, "ml-text", Resources.Lang.moneyline)
        TemplateHandler.Assign(tpl, "copy-hdp-text", Resources.Lang.lblCopyFromHandicap)
        TemplateHandler.Assign(tpl, "copy-hdp-col", "1")
    End Sub
#End Region

    '--------------------------------------------------
#Region "ReplaceData"
    Public Shared Function ReplaceData(ByVal str As String) As String
        Return str.Replace("""", "&quot;").Replace("'", "&#39;").Replace(vbNewLine, "")
    End Function
#End Region

#Region "GetSportTitle"
    Shared Sub GetSportTitle(ByRef tpl As String, Optional ByVal isEditMin As Boolean = False)
        TemplateHandler.Assign(tpl, "soccer-text", Resources.Lang.soccer)
        TemplateHandler.Assign(tpl, "basket-text", Resources.Lang.Basketball)
        TemplateHandler.Assign(tpl, "football-text", Resources.Lang.Football)
        TemplateHandler.Assign(tpl, "tennis-text", Resources.Lang.Tennis)
        TemplateHandler.Assign(tpl, "baseball-text", Resources.Lang.Baseball)
        TemplateHandler.Assign(tpl, "golf-text", Resources.Lang.Golf)
        TemplateHandler.Assign(tpl, "moto-text", Resources.Lang.MotorSports)
        TemplateHandler.Assign(tpl, "othersports-text", Resources.Lang.OtherSports)
        If isEditMin Then
            If UserPermission.IsShowNumberGame Then
                TemplateHandler.Assign(tpl, "copy-all-text", Resources.Lang.lblCopyAllPTfromHdpMin)
                TemplateHandler.Assign(tpl, "copy-basket-text", Resources.Lang.lblCopyBasketHdpMin)
            Else
                TemplateHandler.Assign(tpl, "copy-all-text", Resources.Lang.lblCopyAllPTfromHdpMinNoNumberGame)
                TemplateHandler.Assign(tpl, "copy-basket-text", Resources.Lang.lblCopyBasketHdpMinNoNumberGame)
            End If

        Else
            If UserPermission.IsShowNumberGame Then
                TemplateHandler.Assign(tpl, "copy-all-text", Resources.Lang.lblCopyAllPTfromHdp)
                TemplateHandler.Assign(tpl, "copy-basket-text", Resources.Lang.lblCopyBasketHdp)
            Else
                TemplateHandler.Assign(tpl, "copy-all-text", Resources.Lang.lblCopyAllPTfromHdpNoNumberGame)
                TemplateHandler.Assign(tpl, "copy-basket-text", Resources.Lang.lblCopyBasketHdpNoNumberGame)
            End If

        End If

        TemplateHandler.Assign(tpl, "financial-text", Resources.Lang.Financial)
    End Sub
#End Region

#Region "Assign Header for Number Game"
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


End Class

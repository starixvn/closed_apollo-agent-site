Imports System.Resources
Imports DNAide.Web
Imports System.Text
Imports DNAide.Core
Imports System.Data
Imports Fanex.Data.DNAide
Imports DNAide.Utils
Imports Sunplus.Agent.Model.Entities.Permission



Public Class EditMaster
    Inherits TplPage
    Public style As String
    Public strUser As String
    Dim strcustid As String

    Dim bodyTpl, tennisTpl, basketballTpl, footballTpl, otherSportsTpl, golfTpl As String
    Dim strHidSoccer, strHidBasket, strHidFootball, strHidTennis As String
    Dim strHidBaseball, strHidGolf, strHidMoto, strHidOther, strHidMixSports, strHidNumberGame As String
    Dim groupBtn As String
    Dim selectTpl As String
    Dim selDisabledTpl As String
    Public strErrMsg As String
    Dim saveCustid As String = String.Empty



    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        Dim mainContent As String
        Me.CssGroup = "popup"
        mainContent = TemplateHandler.LoadTemplate("MemberInfo/Super_EditMaster.html")
        bodyTpl = TemplateHandler.LoadTemplate("MemberInfo/MenuPT.html")

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
        selDisabledTpl = TemplateHandler.LoadTemplate("MemberInfo/DdlDisablePT.html")
        tennisTpl = TemplateHandler.LoadTemplate("MemberInfo/TennisPT.html")
        basketballTpl = TemplateHandler.LoadTemplate("MemberInfo/BasketballPT.html")
        footballTpl = TemplateHandler.LoadTemplate("MemberInfo/FootballPT.html")
        otherSportsTpl = TemplateHandler.LoadTemplate("MemberInfo/OtherSportsPT.html")
        golfTpl = TemplateHandler.LoadTemplate("MemberInfo/GolfPT.html")

        If (String.IsNullOrEmpty(Request.QueryString("nobackground"))) Then
            TemplateHandler.RemoveBlock(mainContent, "nobackground")
        Else
            'If is from Account Menu, Then Set delay reload page
            Dim hash As Hashtable = AppSession.RequestQueryString
            If (hash Is Nothing) Then
                hash = New Hashtable
            End If
            If (Not hash.ContainsKey("FromAccount")) Then
                hash.Add("FromAccount", "../_MemberInfo/CustomerList/Super/MasterList.aspx")
            Else
                hash("FromAccount") = "../_MemberInfo/CustomerList/Super/MasterList.aspx"
            End If
            AppSession.RequestQueryString = hash
        End If

        If Request.QueryString("m") = "0" Then
            saveCustid = Request.QueryString("custid").Replace("^", "")
            TemplateHandler.Assign(mainContent, "saveCustid", saveCustid)
            TemplateHandler.Assign(mainContent, "saveUsername", Request.QueryString("USER").Replace("^", ""))
            TemplateHandler.Assign(mainContent, "isMult", "0")
            TemplateHandler.Assign(mainContent, "htmlEditMasterContent", GetDataSingle())
        Else
            TemplateHandler.Assign(mainContent, "isMult", "1")
            TemplateHandler.Assign(mainContent, "htmlEditMasterContent", GetDataMultiple())
        End If

        If (Sunplus.Agent.Model.Entities.UserSession.Current.IsInternal) Then
            TemplateHandler.Assign(mainContent, "clickable", "return false")
        Else
            TemplateHandler.Assign(mainContent, "clickable", String.Empty)
        End If

        Me.TplSetContent(mainContent)

    End Sub

#Region "Declare parameter for Script"
    'declare parameters is used in client script 
    'this function is call in code inline
    Function VariableOfScript()
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

        script += " var confirmUnSuspend=""" & Resources.Lang.confirmUnSuspended & """;" & vbNewLine
        script += " var confirmSuspend=""" & Resources.Lang.confirmSuspendDownline & """;" & vbNewLine

        script += " var confirmClosed=""" & Resources.Lang.confirmCloseDownline & """;" & vbNewLine
        script += " var confirmOpen=""" & Resources.Lang.confirmCloseMem & """;" & vbNewLine

        Dim lblCreditInvalid As String
        lblCreditInvalid = Resources.Lang.lblCreditInvalid.Replace("{0}", Resources.Lang.lblMasterMaxCredit)
        script += " var alertCreditInvalid='" & lblCreditInvalid & "';" & vbNewLine
        Dim strAlertMinPT As String = Resources.Lang.alertLessthanMinPT
        strAlertMinPT = strAlertMinPT.Replace("{1}", Resources.Lang.lblMasterPresetPosition)
        strAlertMinPT = strAlertMinPT.Replace("{2}", Resources.Lang.lblSuperPresetPosition)
        script += " var alertLessthanMinPT='" & strAlertMinPT & "';" & vbNewLine
        script += "</script>"
        Return script
    End Function
#End Region

#Region "Get Information in Single Edit case"
    Function GetDataSingle()
        Dim strEditContent As New StringBuilder

        Dim rs As DbResult
        Dim hash As New Hashtable
        hash.Add("custid", saveCustid)
        hash.Add("srecommend", Sunplus.Agent.Model.Entities.UserSession.Current.CustId)
        rs = AccountInfo.Acc_CustomerSelSEditCustomer(hash)
        If Not rs Is Nothing Then
            If rs.ResultSet.Tables(0).Rows.Count > 0 Then
                Dim row As DataRow = rs.ResultSet.Tables(0).Rows(0)
                strEditContent.Append(GetDataInfo(row))

                ' Casino
                TemplateHandler.Assign(bodyTpl, "hidSynCS", row("synccasino"))

                'GetSportTitle(bodyTpl)
                Common.GetSportTitle(bodyTpl, False)
                TemplateHandler.Assign(bodyTpl, "add-text", Resources.Lang.submit)
                TemplateHandler.Assign(bodyTpl, "add-id", "btnSubmitFooter")

                TemplateHandler.Assign(bodyTpl, "soccerPT", GetDataSoccer(row))
                TemplateHandler.Assign(bodyTpl, "basketPT", GetDataBasketball(row, 2, basketballTpl))
                TemplateHandler.Assign(bodyTpl, "footballPT", GetData4DDl(row, 3, footballTpl))
                TemplateHandler.Assign(bodyTpl, "tennisPT", GetData4DDl(row, 5, tennisTpl))
                TemplateHandler.Assign(bodyTpl, "baseballPT", GetDataBaseball(row, 8))
                TemplateHandler.Assign(bodyTpl, "golfPT", GetData4DDl(row, 10, golfTpl))
                TemplateHandler.Assign(bodyTpl, "motoPT", GetDataMotoSports(row, 11))
                TemplateHandler.Assign(bodyTpl, "otherPT", GetData4DDl(row, 99, otherSportsTpl))

                If UserPermission.IsShowMixedSport Then
                    TemplateHandler.Assign(bodyTpl, "mixSportPT", GetDataMixSport(row, False))
                End If


                If UserPermission.IsShowNumberGame Then
                    TemplateHandler.Assign(bodyTpl, "numbergamePT", GetDataNumberGame(row, False))
                Else
                    TemplateHandler.Assign(bodyTpl, "numbergamePT", "")
                End If



                '--------------
                TemplateHandler.RemoveBlock(bodyTpl, "none-edit-Min")
                bodyTpl &= strHidSoccer
                bodyTpl &= strHidBasket
                bodyTpl &= strHidFootball
                bodyTpl &= strHidTennis
                bodyTpl &= strHidBaseball
                bodyTpl &= strHidGolf
                bodyTpl &= strHidMoto
                bodyTpl &= strHidOther
                bodyTpl &= strHidMixSports
                bodyTpl &= strHidNumberGame

                strEditContent.Append(bodyTpl)
            End If
        End If

        GetDataSingle = strEditContent.ToString()
    End Function
#End Region

#Region "Get Information in Multiple Edit case"
    Function GetDataMultiple() As String
        Dim strEditContent As New StringBuilder

        Dim rs As DbResult
        Dim hash As New Hashtable
        hash.Add("custid", Sunplus.Agent.Model.Entities.UserSession.Current.CustId)
        rs = AccountInfo.Acc_CustomerSelSAgentInfo(hash)
        If Not rs Is Nothing Then
            If rs.ResultSet.Tables(0).Rows.Count > 0 Then
                Dim row As DataRow = rs.ResultSet.Tables(0).Rows(0)
                strEditContent.Append(GetDataInfo(row, True))

                'TemplateHandler.RemoveBlock(bodyTpl, "none-Mult")
                'TemplateHandler.RemoveBlock(bodyTpl, "none-Mult")
                TemplateHandler.Assign(bodyTpl, "add-text", Resources.Lang.submit)
                TemplateHandler.Assign(bodyTpl, "add-id", "btnSubmit")
                Common.GetSportTitle(bodyTpl, False)

                TemplateHandler.Assign(bodyTpl, "soccerPT", GetDataSoccer(row, True))
                TemplateHandler.Assign(bodyTpl, "basketPT", GetDataBasketball(row, 2, basketballTpl, True))
                TemplateHandler.Assign(bodyTpl, "footballPT", GetData4DDl(row, 3, footballTpl, True))
                TemplateHandler.Assign(bodyTpl, "tennisPT", GetData4DDl(row, 5, tennisTpl, True))
                TemplateHandler.Assign(bodyTpl, "baseballPT", GetDataBaseball(row, 8, True))
                TemplateHandler.Assign(bodyTpl, "golfPT", GetData4DDl(row, 10, golfTpl, True))
                TemplateHandler.Assign(bodyTpl, "motoPT", GetDataMotoSports(row, 11, True))
                TemplateHandler.Assign(bodyTpl, "otherPT", GetData4DDl(row, 99, otherSportsTpl, True))
                If UserPermission.IsShowMixedSport Then
                    TemplateHandler.Assign(bodyTpl, "mixSportPT", GetDataMixSport(row, True))
                End If

                If UserPermission.IsShowNumberGame Then
                    TemplateHandler.Assign(bodyTpl, "numbergamePT", GetDataNumberGame(row, True))
                Else
                    TemplateHandler.Assign(bodyTpl, "numbergamePT", "")
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
                bodyTpl &= strHidMixSports
                bodyTpl &= strHidNumberGame

                strEditContent.Append(bodyTpl)
            End If
        End If

        Return strEditContent.ToString()
    End Function
#End Region

#Region "Assign info about UNam, Max Credit, Commission ..."
    Function GetDataInfo(ByVal dr As DataRow, Optional ByVal isMult As Boolean = False) As String
        Dim strAccInfo As String = String.Empty


        strAccInfo = TemplateHandler.LoadTemplate("MemberInfo/AccInfo.html")

        If (Sunplus.Agent.Model.Entities.UserSession.Current.IsInternal) Then
            TemplateHandler.Assign(strAccInfo, "clickable", "return false")
        Else
            TemplateHandler.Assign(strAccInfo, "clickable", String.Empty)
        End If

        'nqminh - change to popup stylesheet!            
        strAccInfo = Common.ReplaceWithPopupStyle(strAccInfo)

        TemplateHandler.RemoveBlock(strAccInfo, "none-member")
        TemplateHandler.RemoveBlock(strAccInfo, "none-member")
        TemplateHandler.RemoveBlock(strAccInfo, "none-member")
        TemplateHandler.RemoveBlock(strAccInfo, "none-group")
        TemplateHandler.RemoveBlock(strAccInfo, "none-group")
        TemplateHandler.RemoveBlock(strAccInfo, "none-edit-member")
        TemplateHandler.RemoveBlock(strAccInfo, "none-HRdisable")
        '
        TemplateHandler.Assign(strAccInfo, "tranfer-time", "")
        TemplateHandler.Assign(strAccInfo, "suspend-text", Resources.Lang.suspended)
        TemplateHandler.Assign(strAccInfo, "add-text", Resources.Lang.submit)
        TemplateHandler.Assign(strAccInfo, "add-id", "btnSubmit")
        TemplateHandler.Assign(strAccInfo, "discountnumbergame-text", Resources.Lang.numbergamecomm)
        If isMult Then
            TemplateHandler.RemoveBlock(strAccInfo, "none-mult")
            TemplateHandler.Assign(strAccInfo, "cols-untext", "5")
            TemplateHandler.Assign(strAccInfo, "groupa-val", PositionTaking.GetListGroup(CastAide.AsDouble(dr("groupa")), CastAide.AsDouble(dr("groupa"))))
            TemplateHandler.Assign(strAccInfo, "groupb-val", PositionTaking.GetListGroup(CastAide.AsDouble(dr("groupb")), CastAide.AsDouble(dr("groupb"))))
            TemplateHandler.Assign(strAccInfo, "groupc-val", PositionTaking.GetListGroup(CastAide.AsDouble(dr("groupc")), CastAide.AsDouble(dr("groupc"))))
            TemplateHandler.Assign(strAccInfo, "groupd-val", PositionTaking.GetListGroup(CastAide.AsDouble(dr("groupd")), CastAide.AsDouble(dr("groupd"))))
            TemplateHandler.Assign(strAccInfo, "discount1x2-val", PositionTaking.GetListGroup(CastAide.AsDouble(dr("discount1x2")), CastAide.AsDouble(dr("discount1x2"))))
            TemplateHandler.Assign(strAccInfo, "discountcs-val", PositionTaking.GetListGroup(CastAide.AsDouble(dr("discountcs")), CastAide.AsDouble(dr("discountcs"))))
            If UserPermission.IsShowNumberGame Then
                TemplateHandler.Assign(strAccInfo, "disnumbergame-val", PositionTaking.GetListGroup(CastAide.AsDouble(dr("discountnumber")), CastAide.AsDouble(dr("discountnumber"))))
            End If


            TemplateHandler.Assign(strAccInfo, "sel-a", "disabled")
            TemplateHandler.Assign(strAccInfo, "sel-b", "disabled")
            TemplateHandler.Assign(strAccInfo, "sel-c", "disabled")
            TemplateHandler.Assign(strAccInfo, "sel-d", "disabled")
            TemplateHandler.Assign(strAccInfo, "sel-dis", "disabled")
            TemplateHandler.Assign(strAccInfo, "sel-dis1x2", "disabled")
            TemplateHandler.Assign(strAccInfo, "sel-disnumbergame", "disabled")

            TemplateHandler.Assign(strAccInfo, "{credit-col}", "2")
            TemplateHandler.Assign(strAccInfo, "check-val", "disabled")
            TemplateHandler.Assign(strAccInfo, "title", Resources.Lang.lblEditMultiple)

            TemplateHandler.Assign(strAccInfo, "IsSuspended", 0)
            TemplateHandler.Assign(strAccInfo, "UplineSuspended", 0)
        Else
            TemplateHandler.RemoveBlock(strAccInfo, "none-edit")
            TemplateHandler.RemoveBlock(strAccInfo, "none-edit")
            TemplateHandler.RemoveBlock(strAccInfo, "none-single")
            TemplateHandler.RemoveBlock(strAccInfo, "none-single")
            TemplateHandler.RemoveBlock(strAccInfo, "none-single")
            TemplateHandler.RemoveBlock(strAccInfo, "none-super-master")

            Dim mytemplate As String = TemplateHandler.LoadTemplate("Memberinfo/DisableStatus.html")
            TemplateHandler.RemoveBlock(mytemplate, "Captchar")
            TemplateHandler.RemoveBlock(strAccInfo, "for-master-site")
            TemplateHandler.RemoveBlock(strAccInfo, "for-super-site")
            TemplateHandler.RemoveBlock(strAccInfo, "for-agent-site")
            '----

            Dim strSuspend2 As String = TemplateHandler.CopyBlock(mytemplate, "Suspend")
            TemplateHandler.ReplaceBlock(strAccInfo, "suspend1", strSuspend2)
            TemplateHandler.RemoveBlock(strAccInfo, "suspend")

            TemplateHandler.Assign(strAccInfo, "cols-untext", "4")
            TemplateHandler.Assign(strAccInfo, "title", Resources.Lang.edit + " " + dr("username"))
            TemplateHandler.Assign(strAccInfo, "max-credit-text", Resources.Lang.lblMasterMaxCredit)
            TemplateHandler.Assign(strAccInfo, "uname-parent", Sunplus.Agent.Model.Entities.UserSession.Current.UserName)

            TemplateHandler.Assign(strAccInfo, "fname-val", PositionTaking.ReplaceData(CastAide.AsString(dr("firstname"))))
            TemplateHandler.Assign(strAccInfo, "lname-val", PositionTaking.ReplaceData(CastAide.AsString(dr("lastname"))))
            TemplateHandler.Assign(strAccInfo, "phone-val", PositionTaking.ReplaceData(CastAide.AsString(dr("phone"))))
            TemplateHandler.Assign(strAccInfo, "mobile-val", PositionTaking.ReplaceData(CastAide.AsString(dr("mobilephone"))))
            TemplateHandler.Assign(strAccInfo, "fax-val", PositionTaking.ReplaceData(CastAide.AsString(dr("fax"))))


            'display value,limit of credit
            GetCredit(dr, strAccInfo, True)
            If CBool(dr("closed")) Then
                TemplateHandler.Assign(strAccInfo, "check-val", "checked")
            Else
                TemplateHandler.Assign(strAccInfo, "check-val", "")
            End If
            TemplateHandler.Assign(strAccInfo, "IsClosed", CastAide.AsInt32(dr("closed"), 0))


            Dim status As String = ""
            If (CastAide.AsBoolean(dr("suspended"), False)) Then
                status = "checked='checked' "
            End If

            If (CastAide.AsBoolean(dr("Ssuspended"), False)) Then
                status = status + "disabled='disabled'"
            End If
            TemplateHandler.Assign(strAccInfo, "suspend-text", Resources.Lang.suspended)
            TemplateHandler.Assign(strAccInfo, "suspend-check", status)
            TemplateHandler.Assign(strAccInfo, "IsSuspended", CastAide.AsInt32(dr("suspended"), 0))

            TemplateHandler.Assign(strAccInfo, "UplineSuspended", CastAide.AsInt32(dr("Ssuspended"), 0))

            TemplateHandler.Assign(strAccInfo, "groupa-val", PositionTaking.GetListGroup(CastAide.AsDouble(dr("groupa_max")), CastAide.AsDouble(dr("groupa"))))
            TemplateHandler.Assign(strAccInfo, "groupb-val", PositionTaking.GetListGroup(CastAide.AsDouble(dr("groupb_max")), CastAide.AsDouble(dr("groupb"))))
            TemplateHandler.Assign(strAccInfo, "groupc-val", PositionTaking.GetListGroup(CastAide.AsDouble(dr("groupc_max")), CastAide.AsDouble(dr("groupc"))))
            TemplateHandler.Assign(strAccInfo, "groupd-val", PositionTaking.GetListGroup(CastAide.AsDouble(dr("groupd_max")), CastAide.AsDouble(dr("groupd"))))
            TemplateHandler.Assign(strAccInfo, "discount1x2-val", PositionTaking.GetListGroup(CastAide.AsDouble(dr("discount1x2_max")), CastAide.AsDouble(dr("discount1x2"))))
            TemplateHandler.Assign(strAccInfo, "discountcs-val", PositionTaking.GetListGroup(CastAide.AsDouble(dr("discountcs_max")), CastAide.AsDouble(dr("discountcs"))))
            If UserPermission.IsShowNumberGame Then
                TemplateHandler.Assign(strAccInfo, "disnumbergame-val", PositionTaking.GetListGroup(CastAide.AsDouble(dr("discountnumber_max")), CastAide.AsDouble(dr("discountnumber"))))
            End If



            TemplateHandler.Assign(strAccInfo, "sel-a", "")
            TemplateHandler.Assign(strAccInfo, "sel-b", "")
            TemplateHandler.Assign(strAccInfo, "sel-c", "")
            TemplateHandler.Assign(strAccInfo, "sel-d", "")
            TemplateHandler.Assign(strAccInfo, "sel-dis", "")
            TemplateHandler.Assign(strAccInfo, "sel-disFC", "")
            TemplateHandler.Assign(strAccInfo, "{credit-col}", "2")
            'sel-disfc
        End If
        GetTitleInfo(strAccInfo, isMult)



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

        soccerTpl = TemplateHandler.LoadTemplate("MemberInfo/SoccerPT.html")
        headerSTpl = TemplateHandler.LoadTemplate("MemberInfo/SBettypeHeader.html")
        ' assign all text according language
        TemplateHandler.RemoveBlock(soccerTpl, "none-edit")
        TemplateHandler.Assign(soccerTpl, "col-title", "12")
        TemplateHandler.Assign(soccerTpl, "bettype-header", LoadSoccerPTHeader())
        LoadChkUpdate(soccerTpl, 1, isMult)
        GetTitleCommon(soccerTpl)

        TemplateHandler.Assign(soccerTpl, "deadball-text", Resources.Lang.lblDeadball)
        TemplateHandler.Assign(soccerTpl, "au-preset-live", Resources.Lang.lblAutoSuperPresetPosition)
        TemplateHandler.Assign(soccerTpl, "live-text", Resources.Lang.lblLive)
        TemplateHandler.Assign(soccerTpl, "a-preset-live", Resources.Lang.lblMasterPresetPosition)
        TemplateHandler.Assign(soccerTpl, "p-preset-live", Resources.Lang.lblSuperPresetPosition)
        TemplateHandler.Assign(soccerTpl, "auto-super-text", Resources.Lang.lblAutoSuperPresetPosition)
        TemplateHandler.Assign(soccerTpl, "dead-row", "5")
        TemplateHandler.Assign(soccerTpl, "live-row", "4")
        TemplateHandler.Assign(soccerTpl, "copy-live-col", "4")
        TemplateHandler.Assign(soccerTpl, "copy-1x2-text", Resources.Lang.lblCopyFrom1x2)

        TemplateHandler.Assign(soccerTpl, "chkAuto-1", "chkAuto1HdpD")
        TemplateHandler.Assign(soccerTpl, "chkAuto-2", "chkAuto11x2")
        TemplateHandler.Assign(soccerTpl, "chkAuto-3", "chkAuto1HdpL")
        'assign value of Minbet, Maxbet , ...
        If Not isMult Then
            TemplateHandler.Assign(soccerTpl, "minbet1-val", DataFormat.FormatNumber1(dr("minbet")))
            TemplateHandler.Assign(soccerTpl, "maxbet1-val", DataFormat.FormatNumber1(dr("maxbet")))
            TemplateHandler.Assign(soccerTpl, "maxpermatch1-val", DataFormat.FormatNumber1(dr("maxpermatch")))
            TemplateHandler.Assign(soccerTpl, "chk-hdpd", "")
            TemplateHandler.Assign(soccerTpl, "chk-1x2", "")
            TemplateHandler.Assign(soccerTpl, "chk-hdpl", "")
        Else
            GetMinMaxAddValue(soccerTpl, 1, "0"" disabled='true'")
            TemplateHandler.Assign(soccerTpl, "chk-hdpd", "disabled")
            TemplateHandler.Assign(soccerTpl, "chk-1x2", "disabled")
            TemplateHandler.Assign(soccerTpl, "chk-hdpl", "disabled")
        End If
        GetLimitMinMaxBet(dr, soccerTpl, "")

        'assign value position taking for template
        Dim mHdpMin, mOUMin, m1stHdpMin, m1stOUMin, mOEMin, m1x2Min, mCSMin, mTGMin, mMPMin, mORMin As Double
        Dim mLHdpMin, mLOUMin, mL1stHdpMin, mL1stOUMin As Double

        Dim sHdpMin, sOUMin, s1stHdpMin, s1stOUMin, sOEMin, s1x2Min, sCSMin, sTGMin, sMPMin, sORMin As Double
        Dim sLHdpMin, sLOUMin, sL1stHdpMin, sL1stOUMin As Double

        Dim mHdp, mOU, m1stHdp, m1stOU, mOE, m1x2, mCS, mTG, mMP, mOR As Double
        Dim mLHdp, mLOU, mL1stHdp, mL1stOU As Double

        Dim mHdpMax, mOUMax, m1stHdpMax, m1stOUMax, mOEMax, m1x2Max, mCSMax, mTGMax, mMPMax, mORMax As Double
        Dim mLHdpMax, mLOUMax, mL1stHdpMax, mL1stOUMax As Double

        Dim mHdpMax2, mOUMax2, m1stHdpMax2, m1stOUMax2, mOEMax2, m1x2Max2, mCSMax2, mTGMax2, mMPMax2, mORMax2 As Double
        Dim mLHdpMax2, mLOUMax2, mL1stHdpMax2, mL1stOUMax2 As Double

        Dim sHdp, sOU, s1stHdp, s1stOU, sOE, s1x2, sCS, sTG, sMP, sOR As Double
        Dim sLHdp, sLOU, sL1stHdp, sL1stOU As Double

        Dim sHdpMax, sOUMax, s1stHdpMax, s1stOUMax, sOEMax, s1x2Max, sCSMax, sTGMax, sMPMax, sORMax As Double
        Dim sLHdpMax, sLOUMax, sL1stHdpMax, sL1stOUMax As Double

        Dim msHdpMax, msOUMax, ms1stHdpMax, ms1stOUMax, msOEMax, ms1x2Max, msCSMax, msTGMax, msMPMax, msORMax As Double
        Dim msLHdpMax, msLOUMax, msL1stHdpMax, msL1stOUMax As Double

        Dim msHdpMin, msOUMin, ms1stHdpMin, ms1stOUMin, msOEMin, ms1x2Min, msCSMin, msTGMin, msMPMin, msORMin As Double
        Dim msLHdpMin, msLOUMin, msL1stHdpMin, msL1stOUMin As Double

        Dim sAutoHdp, sAutoOU, sAuto1stHdp, sAuto1stOU, sAutoOE, sAuto1x2, sAutoCS, sAutoTG, sAutoMP, sAutoOR As Double
        Dim sAutoLHdp, sAutoLOU, sAutoL1stHdp, sAutoL1stOU As Double

        Dim sAutoHdpMax, sAutoOUMax, sAuto1stHdpMax, sAuto1stOUMax, sAutoOEMax, sAuto1x2Max, sAutoCSMax, sAutoTGMax, sAutoMPMax, sAutoORMax As Double
        Dim sAutoLHdpMax, sAutoLOUMax, sAutoL1stHdpMax, sAutoL1stOUMax As Double

        Dim isAutoHdp, isAutoOU, isAuto1stHdp, isAuto1stOU, isAutoOE, isAuto1x2, isAutoCS, isAutoTG, isAutoMP, isAutoOR As Double
        Dim isAutoLHdp, isAutoLOU, isAutoL1stHdp, isAutoL1stOU As Double

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

        ''this is used to save into hidden fields to check in client script '''''''
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

        If Not isMult Then 'this case for Single Edit
            mHdpMin = CastAide.AsDouble(dr("mpositiontaking_1_min"))
            mOUMin = CastAide.AsDouble(dr("mpositiontaking_3_min"))
            m1stHdpMin = CastAide.AsDouble(dr("mpositiontaking_7_min"))
            m1stOUMin = CastAide.AsDouble(dr("mpositiontaking_8_min"))
            mOEMin = CastAide.AsDouble(dr("mpositiontaking_2_min"))
            m1x2Min = CastAide.AsDouble(dr("mpositiontaking_5_min"))
            mCSMin = CastAide.AsDouble(dr("mpositiontaking_4_min"))
            mTGMin = CastAide.AsDouble(dr("mpositiontaking_6_min"))
            mMPMin = CastAide.AsDouble(dr("mpositiontaking_9_min"))
            mORMin = CastAide.AsDouble(dr("mpositiontaking_10_min"))
            mLHdpMin = CastAide.AsDouble(dr("mlivepositiontaking_1_min"))
            mLOUMin = CastAide.AsDouble(dr("mlivepositiontaking_3_min"))
            mL1stHdpMin = CastAide.AsDouble(dr("mlivepositiontaking_7_min"))
            mL1stOUMin = CastAide.AsDouble(dr("mlivepositiontaking_8_min"))

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

            sHdpMax = CastAide.AsDouble(dr("spositiontaking_1_max"))
            sOUMax = CastAide.AsDouble(dr("spositiontaking_3_max"))
            s1stHdpMax = CastAide.AsDouble(dr("spositiontaking_7_max"))
            s1stOUMax = CastAide.AsDouble(dr("spositiontaking_8_max"))
            sOEMax = CastAide.AsDouble(dr("spositiontaking_2_max"))
            s1x2Max = CastAide.AsDouble(dr("spositiontaking_5_max"))
            sCSMax = CastAide.AsDouble(dr("spositiontaking_4_max"))
            sTGMax = CastAide.AsDouble(dr("spositiontaking_6_max"))
            sMPMax = CastAide.AsDouble(dr("spositiontaking_9_max"))
            sORMax = CastAide.AsDouble(dr("spositiontaking_10_max"))
            sLHdpMax = CastAide.AsDouble(dr("slivepositiontaking_1_max"))
            sLOUMax = CastAide.AsDouble(dr("slivepositiontaking_3_max"))
            sL1stHdpMax = CastAide.AsDouble(dr("slivepositiontaking_7_max"))
            sL1stOUMax = CastAide.AsDouble(dr("slivepositiontaking_8_max"))

            sAutoHdpMax = CastAide.AsDouble(dr("auto_spositiontaking_1_max"))
            sAutoOUMax = CastAide.AsDouble(dr("auto_spositiontaking_3_max"))
            sAuto1stHdpMax = CastAide.AsDouble(dr("auto_spositiontaking_7_max"))
            sAuto1stOUMax = CastAide.AsDouble(dr("auto_spositiontaking_8_max"))
            sAutoOEMax = CastAide.AsDouble(dr("auto_spositiontaking_2_max"))
            sAuto1x2Max = CastAide.AsDouble(dr("auto_spositiontaking_5_max"))
            sAutoCSMax = CastAide.AsDouble(dr("auto_spositiontaking_4_max"))
            sAutoTGMax = CastAide.AsDouble(dr("auto_spositiontaking_6_max"))
            sAutoMPMax = CastAide.AsDouble(dr("auto_spositiontaking_9_max"))
            sAutoORMax = CastAide.AsDouble(dr("auto_spositiontaking_10_max"))
            sAutoLHdpMax = CastAide.AsDouble(dr("auto_slivepositiontaking_1_max"))
            sAutoLOUMax = CastAide.AsDouble(dr("auto_slivepositiontaking_3_max"))
            sAutoL1stHdpMax = CastAide.AsDouble(dr("auto_slivepositiontaking_7_max"))
            sAutoL1stOUMax = CastAide.AsDouble(dr("auto_slivepositiontaking_8_max"))

            isAutoHdp = CastAide.AsInt16(dr("isauto_spositiontaking_1"))
            isAutoOU = CastAide.AsInt16(dr("isauto_spositiontaking_3"))
            isAuto1stHdp = CastAide.AsInt16(dr("isauto_spositiontaking_7"))
            isAuto1stOU = CastAide.AsInt16(dr("isauto_spositiontaking_8"))
            isAutoOE = CastAide.AsInt16(dr("isauto_spositiontaking_2"))
            isAuto1x2 = CastAide.AsInt16(dr("isauto_spositiontaking_5"))
            isAutoCS = CastAide.AsInt16(dr("isauto_spositiontaking_4"))
            isAutoTG = CastAide.AsInt16(dr("isauto_spositiontaking_6"))
            isAutoMP = CastAide.AsInt16(dr("isauto_spositiontaking_9"))
            isAutoOR = CastAide.AsInt16(dr("isauto_spositiontaking_10"))
            isAutoLHdp = CastAide.AsInt16(dr("isauto_slivepositiontaking_1"))
            isAutoLOU = CastAide.AsInt16(dr("isauto_slivepositiontaking_3"))
            isAutoL1stHdp = CastAide.AsInt16(dr("isauto_slivepositiontaking_7"))
            isAutoL1stOU = CastAide.AsInt16(dr("isauto_slivepositiontaking_8"))

            sHdpMax = sHdpMax - mHdpMin
            sOUMax = sOUMax - mOUMin
            s1stHdpMax = s1stHdpMax - m1stHdpMin
            s1stOUMax = s1stOUMax - m1stOUMin
            sOEMax = sOEMax - mOEMin
            s1x2Max = s1x2Max - m1x2Min
            sCSMax = sCSMax - mCSMin
            sTGMax = sTGMax - mTGMin
            sMPMax = sMPMax - mMPMin
            sORMax = sORMax - mORMin
            sLHdpMax = sLHdpMax - mLHdpMin
            sLOUMax = sLOUMax - mLOUMin
            sL1stHdpMax = sL1stHdpMax - mL1stHdpMin
            sL1stOUMax = sL1stOUMax - mL1stOUMin
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
        'this script to get max PT value of master that master can be preseted
        'max PT value = max ms - current PT value of super
        mHdpMax = IIf(isAutoHdp, msHdpMax, msHdpMax - sHdp)
        mOUMax = IIf(isAutoOU, msOUMax, msOUMax - sOU)
        m1stHdpMax = IIf(isAuto1stHdp, ms1stHdpMax, ms1stHdpMax - s1stHdp)
        m1stOUMax = IIf(isAuto1stOU, ms1stOUMax, ms1stOUMax - s1stOU)
        mOEMax = IIf(isAutoOE, msOEMax, msOEMax - sOE)
        m1x2Max = IIf(isAuto1x2, ms1x2Max, ms1x2Max - s1x2)
        mCSMax = IIf(isAutoCS, msCSMax, msCSMax - sCS)
        mTGMax = IIf(isAutoTG, msTGMax, msTGMax - sTG)
        mMPMax = IIf(isAutoMP, msMPMax, msMPMax - sMP)
        mORMax = IIf(isAutoOR, msORMax, msORMax - sOR)
        mLHdpMax = IIf(isAutoLHdp, msLHdpMax, msLHdpMax - sLHdp)
        mLOUMax = IIf(isAutoLOU, msLOUMax, msLOUMax - sLOU)
        mL1stHdpMax = IIf(isAutoL1stHdp, msL1stHdpMax, msL1stHdpMax - sL1stHdp)
        mL1stOUMax = IIf(isAutoL1stOU, msL1stOUMax, msL1stOUMax - sL1stOU)

        ' if max ms - current PT > max PT of master then max PT value = max PT of master
        mHdpMax = IIf(mHdpMax > mHdpMax2, mHdpMax2, mHdpMax)
        mOUMax = IIf(mOUMax > mOUMax2, mOUMax2, mOUMax)
        m1stHdpMax = IIf(m1stHdpMax > m1stHdpMax2, m1stHdpMax2, m1stHdpMax)
        m1stOUMax = IIf(m1stOUMax > m1stOUMax2, m1stOUMax2, m1stOUMax)
        mOEMax = IIf(mOEMax > mOEMax2, mOEMax2, mOEMax)
        m1x2Max = IIf(m1x2Max > m1x2Max2, m1x2Max2, m1x2Max)
        mCSMax = IIf(mCSMax > mCSMax2, mCSMax2, mCSMax)
        mTGMax = IIf(mTGMax > mTGMax2, mTGMax2, mTGMax)
        mMPMax = IIf(mMPMax > mMPMax2, mMPMax2, mMPMax)
        mORMax = IIf(mORMax > mORMax2, mORMax2, mORMax)
        mLHdpMax = IIf(mLHdpMax > mLHdpMax2, mLHdpMax2, mLHdpMax)
        mLOUMax = IIf(mLOUMax > mLOUMax2, mLOUMax2, mLOUMax)
        mL1stHdpMax = IIf(mL1stHdpMax > mL1stHdpMax2, mL1stHdpMax2, mL1stHdpMax)
        mL1stOUMax = IIf(mL1stOUMax > mL1stOUMax2, mL1stOUMax2, mL1stOUMax)
        '''''''''''end script to get max PT value of master that master can be preseted            

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

        hiddenHash.Add("isAuto1HdpD", isAutoHdp)
        hiddenHash.Add("isAuto1OUD", isAutoOU)
        hiddenHash.Add("isAuto11stHdpD", isAuto1stHdp)
        hiddenHash.Add("isAuto11stOUD", isAuto1stOU)
        hiddenHash.Add("isAuto1OE", isAutoOE)

        hiddenHash.Add("isAuto11x2", isAuto1x2)
        hiddenHash.Add("isAuto1CS", isAutoCS)
        hiddenHash.Add("isAuto1TG", isAutoTG)
        hiddenHash.Add("isAuto1MP", isAutoMP)
        hiddenHash.Add("isAuto1OR", isAutoOR)

        hiddenHash.Add("isAuto1HdpL", isAutoLHdp)
        hiddenHash.Add("isAuto1OUL", isAutoLOU)
        hiddenHash.Add("isAuto11stHdpL", isAutoL1stHdp)
        hiddenHash.Add("isAuto11stOUL", isAutoL1stOU)

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
        Dim ptMaster As String
        Dim pt As String = selectTpl
        If isMult Then
            ptAuto = selDisabledTpl
            pt = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoHdp Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-Hdp", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1HdpD")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
        TemplateHandler.Assign(soccerTpl, "a-hdp-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS1HdpD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sHdpMin, sHdpMax, sHdp))
        TemplateHandler.Assign(soccerTpl, "p-hdp-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1HdpD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoHdpMax, sAutoHdp))
        TemplateHandler.Assign(soccerTpl, "au-hdp-val", ptAuto)

        If isMult Then
            ptAuto = selDisabledTpl
            pt = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoOU Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-OU", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1OUD")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOU))
        TemplateHandler.Assign(soccerTpl, "a-ou-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS1OUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOUMin, sOUMax, sOU))
        TemplateHandler.Assign(soccerTpl, "p-ou-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OUD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOUMax, sAutoOU))
        TemplateHandler.Assign(soccerTpl, "au-ou-val", ptAuto)

        If isMult Then
            ptAuto = selDisabledTpl
            pt = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAuto1stHdp Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            strChecked = "checked"
            ptMaster = selectTpl
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-1stHdp", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM11stHdpD")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(m1stHdpMin, m1stHdpMax, m1stHdp))
        TemplateHandler.Assign(soccerTpl, "a-1sthdp-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS11stHdpD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(s1stHdpMin, s1stHdpMax, s1stHdp))
        TemplateHandler.Assign(soccerTpl, "p-1sthdp-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stHdpD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAuto1stHdpMax, sAuto1stHdp))
        TemplateHandler.Assign(soccerTpl, "au-1sthdp-val", ptAuto)

        If isMult Then
            ptAuto = selDisabledTpl
            pt = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAuto1stOU Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-1stOU", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM11stOUD")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(m1stOUMin, m1stOUMax, m1stOU))
        TemplateHandler.Assign(soccerTpl, "a-1stou-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS11stOUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(s1stOUMin, s1stOUMax, s1stOU))
        TemplateHandler.Assign(soccerTpl, "p-1stou-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stOUD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAuto1stOUMax, sAuto1stOU))
        TemplateHandler.Assign(soccerTpl, "au-1stou-val", ptAuto)

        If isMult Then
            ptAuto = selDisabledTpl
            pt = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoOE Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-OE", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1OE")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOE))
        TemplateHandler.Assign(soccerTpl, "a-oe-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS1OE")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOEMin, sOEMax, sOE))
        TemplateHandler.Assign(soccerTpl, "p-oe-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OE")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOEMax, sAutoOE))
        TemplateHandler.Assign(soccerTpl, "au-oe-val", ptAuto)

        If isMult Then
            ptAuto = selDisabledTpl
            pt = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAuto1x2 Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-1x2", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM11x2")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(m1x2Min, m1x2Max, m1x2))
        TemplateHandler.Assign(soccerTpl, "a-1x2-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS11x2")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(s1x2Min, s1x2Max, s1x2))
        TemplateHandler.Assign(soccerTpl, "p-1x2-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11x2")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAuto1x2Max, sAuto1x2))
        TemplateHandler.Assign(soccerTpl, "au-1x2-val", ptAuto)

        If isMult Then
            ptAuto = selDisabledTpl
            pt = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoCS Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-CS", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1CS")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mCSMin, mCSMax, mCS))
        TemplateHandler.Assign(soccerTpl, "a-cs-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS1CS")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sCSMin, sCSMax, sCS))
        TemplateHandler.Assign(soccerTpl, "p-cs-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1CS")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoCSMax, sAutoCS))
        TemplateHandler.Assign(soccerTpl, "au-cs-val", ptAuto)

        If isMult Then
            ptAuto = selDisabledTpl
            pt = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoTG Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-TG", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1TG")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mTGMin, mTGMax, mTG))
        TemplateHandler.Assign(soccerTpl, "a-tg-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS1TG")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sTGMin, sTGMax, sTG))
        TemplateHandler.Assign(soccerTpl, "p-tg-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1TG")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoTGMax, sAutoTG))
        TemplateHandler.Assign(soccerTpl, "au-tg-val", ptAuto)

        If isMult Then
            ptAuto = selDisabledTpl
            pt = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoMP Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-MP", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1MP")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mMPMin, mMPMax, mMP))
        TemplateHandler.Assign(soccerTpl, "a-mp-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS1MP")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sMPMin, sMPMax, sMP))
        TemplateHandler.Assign(soccerTpl, "p-mp-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1MP")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoMPMax, sAutoMP))
        TemplateHandler.Assign(soccerTpl, "au-mp-val", ptAuto)

        If isMult Then
            ptAuto = selDisabledTpl
            pt = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoOR Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-OR", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1OR")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mORMin, mORMax, mOR))
        TemplateHandler.Assign(soccerTpl, "a-or-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS1OR")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sORMin, sORMax, sOR))
        TemplateHandler.Assign(soccerTpl, "p-or-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OR")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoORMax, sAutoOR))
        TemplateHandler.Assign(soccerTpl, "au-or-val", ptAuto)

        If isMult Then
            ptAuto = selDisabledTpl
            pt = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoLHdp Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-LHdp", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1HdpL")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mLHdpMin, mLHdpMax, mLHdp))
        TemplateHandler.Assign(soccerTpl, "a-Lhdp-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS1HdpL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sLHdpMin, sLHdpMax, sLHdp))
        TemplateHandler.Assign(soccerTpl, "p-Lhdp-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1HdpL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoLHdpMax, sAutoLHdp))
        TemplateHandler.Assign(soccerTpl, "au-Lhdp-val", ptAuto)

        If isMult Then
            ptAuto = selDisabledTpl
            pt = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoLOU Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-LOU", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1OUL")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mLOUMin, mLOUMax, mLOU))
        TemplateHandler.Assign(soccerTpl, "a-Lou-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS1OUL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sLOUMin, sLOUMax, sLOU))
        TemplateHandler.Assign(soccerTpl, "p-Lou-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OUL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoLOUMax, sAutoLOU))
        TemplateHandler.Assign(soccerTpl, "au-Lou-val", ptAuto)

        If isMult Then
            ptAuto = selDisabledTpl
            pt = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoL1stHdp Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-L1stHdp", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM11stHdpL")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mL1stHdpMin, mL1stHdpMax, mL1stHdp))
        TemplateHandler.Assign(soccerTpl, "a-L1stHdp-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS11stHdpL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sL1stHdpMin, sL1stHdpMax, sL1stHdp))
        TemplateHandler.Assign(soccerTpl, "p-L1stHdp-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stHdpL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoL1stHdpMax, sAutoL1stHdp))
        TemplateHandler.Assign(soccerTpl, "au-L1stHdp-val", ptAuto)

        If isMult Then
            ptAuto = selDisabledTpl
            pt = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoL1stOU Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(soccerTpl, "checked-L1stOU", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM11stOUL")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mL1stOUMin, mL1stOUMax, mL1stOU))
        TemplateHandler.Assign(soccerTpl, "a-L1stou-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS11stOUL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sL1stOUMin, sL1stOUMax, sL1stOU))
        TemplateHandler.Assign(soccerTpl, "p-L1stou-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stOUL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoL1stOUMax, sAutoL1stOU))
        TemplateHandler.Assign(soccerTpl, "au-L1stou-val", ptAuto)



        GetDataSoccer = soccerTpl
    End Function
#End Region

#Region "Assign info for  Basketball"
    'this function is used to assign data into template for all sporttype that have 4 bettype
    '(1:Handicap, 3:Over/Under, 2:Odd/Even,9: Mix Parlay, 20:Money Line)
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (2: Basketball)
    Function GetDataBasketball(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String, Optional ByVal isMult As Boolean = False) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable

        'assign text for position taking
        TemplateHandler.RemoveBlock(tpl, "none-edit")
        LoadChkUpdate(tpl, sportTypeId, isMult)
        GetTitleCommon(tpl)
        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "oe-text", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(tpl, "mp-text", Resources.Lang.lblMixParlay)
        TemplateHandler.Assign(tpl, "title-col", "7")
        TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")

        'assign value for position taking
        If Not isMult Then
            GetMinMaxEditValue(tpl, sportTypeId, dr)
            TemplateHandler.Assign(tpl, "chk-hdp", "")
        Else
            GetMinMaxAddValue(tpl, sportTypeId, "0"" disabled='true'")
            TemplateHandler.Assign(tpl, "chk-hdp", "disabled")
        End If
        GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

        Dim mHdpMin, mOUMin, mOEMin, mMLMin As Double
        Dim mHdp, mOU, mOE, mML As Double
        Dim mHdpMax, mOUMax, mOEMax, mMLMax As Double
        Dim mHdpMax2, mOUMax2, mOEMax2, mMLMax2 As Double

        Dim sHdpMin, sOUMin, sOEMin, sMLMin As Double
        Dim sHdp, sOU, sOE, sML As Double
        Dim sHdpMax, sOUMax, sOEMax, sMLMax As Double

        Dim msHdpMax, msOUMax, msOEMax, msMLMax As Double
        Dim msHdpMin, msOUMin, msOEMin, msMLMin As Double

        Dim sAutoHdp, sAutoOU, sAutoOE, sAutoML As Double
        Dim sAutoHdpMax, sAutoOUMax, sAutoOEMax, sAutoMLMax As Double
        Dim isAutoHdp, isAutoOU, isAutoOE, isAutoML As Double

        mHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_max"))
        mOUMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_max"))
        mOEMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2_max"))
        mMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_max"))

        sHdp = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1"))
        sOU = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3"))
        sOE = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_2"))
        sML = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20"))

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

        If Not isMult Then
            mHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_min"))
            mOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_min"))
            mOEMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2_min"))
            mMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_min"))

            mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
            mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
            mOE = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))
            mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

            sHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1_max"))
            sOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3_max"))
            sOEMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_2_max"))
            sMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20_max"))

            sAutoHdpMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1_max"))
            sAutoOUMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_3_max"))
            sAutoOEMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_2_max"))
            sAutoMLMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20_max"))

            isAutoHdp = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_1"))
            isAutoOU = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_3"))
            isAutoOE = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_2"))
            isAutoML = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_20"))

            sHdpMax = sHdpMax - mHdpMin
            sOUMax = sOUMax - mOUMin
            sOEMax = sOEMax - mOEMin
            sMLMax = sMLMax - mMLMin
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

        'this script to get max PT value of master that can preset to
        mHdpMax = IIf(isAutoHdp, msHdpMax, msHdpMax - sHdp)
        mOUMax = IIf(isAutoOU, msOUMax, msOUMax - sOU)
        mOEMax = IIf(isAutoOE, msOEMax, msOEMax - sOE)
        mMLMax = IIf(isAutoML, msMLMax, msMLMax - sML)

        mHdpMax = IIf(mHdpMax > mHdpMax2, mHdpMax2, mHdpMax)
        mOUMax = IIf(mOUMax > mOUMax2, mOUMax2, mOUMax)
        mOEMax = IIf(mOEMax > mOEMax2, mOEMax2, mOEMax)
        mMLMax = IIf(mMLMax > mMLMax2, mMLMax2, mMLMax)
        ''''''''end script''''''''''''''            

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

        hiddenHash.Add("isAuto" & sportTypeId & "Hdp", isAutoHdp)
        hiddenHash.Add("isAuto" & sportTypeId & "OU", isAutoOU)
        hiddenHash.Add("isAuto" & sportTypeId & "OE", isAutoOE)
        hiddenHash.Add("isAuto" & sportTypeId & "ML", isAutoML)
        strHidBasket = PositionTaking.AssignMulipleHidden(hiddenHash)

        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

        Dim ptAuto As String
        Dim strChecked As String
        Dim ptMaster As String
        If isMult Then
            pt = selDisabledTpl
            ptAuto = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoHdp Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-Hdp", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
        TemplateHandler.Assign(tpl, "a-hdp-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sHdpMin, sHdpMax, sHdp))
        TemplateHandler.Assign(tpl, "p-hdp-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoHdpMax, sAutoHdp))
        TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)

        If isMult Then
            pt = selDisabledTpl
            ptAuto = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoOU Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-OU", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "OU")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOU))
        TemplateHandler.Assign(tpl, "a-ou-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OU")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOUMin, sOUMax, sOU))
        TemplateHandler.Assign(tpl, "p-ou-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOUMax, sAutoOU))
        TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)

        If isMult Then
            pt = selDisabledTpl
            ptAuto = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoOE Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-OE", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "OE")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOE))
        TemplateHandler.Assign(tpl, "a-oe-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OE")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOEMin, sOEMax, sOE))
        TemplateHandler.Assign(tpl, "p-oe-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OE")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOEMax, sAutoOE))
        TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)

        If isMult Then
            pt = selDisabledTpl
            ptAuto = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoML Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-ML", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "ML")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mML))
        TemplateHandler.Assign(tpl, "a-ml-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sMLMin, sMLMax, sML))
        TemplateHandler.Assign(tpl, "p-ml-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoMLMax, sAutoML))
        TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)


        GetDataBasketball = tpl
    End Function
#End Region

#Region "Assign info for Football, Tennis, Golf, OtherSports"
    'this function is used to assign data into template for all sporttype that have 4 bettype
    '(1:Handicap, 3:Over/Under, 2:Odd/Even, 20:Money Line)
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (3:Football, 5:Tennis, 10:Golf, 99:OtherSports )
    Function GetData4DDl(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String, Optional ByVal isMult As Boolean = False) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable

        'assign text for position taking
        TemplateHandler.RemoveBlock(tpl, "none-edit")
        LoadChkUpdate(tpl, sportTypeId, isMult)
        GetTitleCommon(tpl)
        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "oe-text", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(tpl, "title-col", "6")
        TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")

        'assign value for position taking
        If Not isMult Then
            GetMinMaxEditValue(tpl, sportTypeId, dr)
            TemplateHandler.Assign(tpl, "chk-hdp", "")
        Else
            GetMinMaxAddValue(tpl, sportTypeId, "0"" disabled='true'")
            TemplateHandler.Assign(tpl, "chk-hdp", "disabled")
        End If
        GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

        Dim mHdpMin, mOUMin, mOEMin, mMLMin As Double
        Dim mHdp, mOU, mOE, mML As Double
        Dim mHdpMax, mOUMax, mOEMax, mMLMax As Double
        Dim mHdpMax2, mOUMax2, mOEMax2, mMLMax2 As Double

        Dim sHdpMin, sOUMin, sOEMin, sMLMin As Double
        Dim sHdp, sOU, sOE, sML As Double
        Dim sHdpMax, sOUMax, sOEMax, sMLMax As Double

        Dim msHdpMax, msOUMax, msOEMax, msMLMax As Double
        Dim msHdpMin, msOUMin, msOEMin, msMLMin As Double

        Dim sAutoHdp, sAutoOU, sAutoOE, sAutoML As Double
        Dim sAutoHdpMax, sAutoOUMax, sAutoOEMax, sAutoMLMax As Double
        Dim isAutoHdp, isAutoOU, isAutoOE, isAutoML As Double

        mHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_max"))
        mOUMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_max"))
        mOEMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2_max"))
        mMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_max"))

        sHdp = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1"))
        sOU = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3"))
        sOE = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_2"))
        sML = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20"))

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

        If Not isMult Then
            mHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_min"))
            mOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_min"))
            mOEMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2_min"))
            mMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_min"))

            mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
            mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
            mOE = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))
            mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

            sHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1_max"))
            sOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3_max"))
            sOEMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_2_max"))
            sMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20_max"))

            sAutoHdpMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1_max"))
            sAutoOUMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_3_max"))
            sAutoOEMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_2_max"))
            sAutoMLMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20_max"))

            isAutoHdp = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_1"))
            isAutoOU = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_3"))
            isAutoOE = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_2"))
            isAutoML = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_20"))

            sHdpMax = sHdpMax - mHdpMin
            sOUMax = sOUMax - mOUMin
            sOEMax = sOEMax - mOEMin
            sMLMax = sMLMax - mMLMin
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

        'this script to get max PT value of master that can preset to
        mHdpMax = IIf(isAutoHdp, msHdpMax, msHdpMax - sHdp)
        mOUMax = IIf(isAutoOU, msOUMax, msOUMax - sOU)
        mOEMax = IIf(isAutoOE, msOEMax, msOEMax - sOE)
        mMLMax = IIf(isAutoML, msMLMax, msMLMax - sML)

        mHdpMax = IIf(mHdpMax > mHdpMax2, mHdpMax2, mHdpMax)
        mOUMax = IIf(mOUMax > mOUMax2, mOUMax2, mOUMax)
        mOEMax = IIf(mOEMax > mOEMax2, mOEMax2, mOEMax)
        mMLMax = IIf(mMLMax > mMLMax2, mMLMax2, mMLMax)
        ''''''''end script''''''''''''''            

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

        hiddenHash.Add("isAuto" & sportTypeId & "Hdp", isAutoHdp)
        hiddenHash.Add("isAuto" & sportTypeId & "OU", isAutoOU)
        hiddenHash.Add("isAuto" & sportTypeId & "OE", isAutoOE)
        hiddenHash.Add("isAuto" & sportTypeId & "ML", isAutoML)

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
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

        Dim ptAuto As String
        Dim strChecked As String
        Dim ptMaster As String
        If isMult Then
            pt = selDisabledTpl
            ptAuto = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoHdp Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-Hdp", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
        TemplateHandler.Assign(tpl, "a-hdp-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sHdpMin, sHdpMax, sHdp))
        TemplateHandler.Assign(tpl, "p-hdp-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoHdpMax, sAutoHdp))
        TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)

        If isMult Then
            pt = selDisabledTpl
            ptAuto = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoOU Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-OU", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "OU")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOU))
        TemplateHandler.Assign(tpl, "a-ou-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OU")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOUMin, sOUMax, sOU))
        TemplateHandler.Assign(tpl, "p-ou-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOUMax, sAutoOU))
        TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)

        If isMult Then
            pt = selDisabledTpl
            ptAuto = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoOE Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-OE", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "OE")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOE))
        TemplateHandler.Assign(tpl, "a-oe-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OE")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOEMin, sOEMax, sOE))
        TemplateHandler.Assign(tpl, "p-oe-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OE")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOEMax, sAutoOE))
        TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)

        If isMult Then
            pt = selDisabledTpl
            ptAuto = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoML Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-ML", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "ML")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mML))
        TemplateHandler.Assign(tpl, "a-ml-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sMLMin, sMLMax, sML))
        TemplateHandler.Assign(tpl, "p-ml-val", pt)
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
        tpl = TemplateHandler.LoadTemplate("MemberInfo/BaseballPT.html")
        TemplateHandler.RemoveBlock(tpl, "none-edit")
        LoadChkUpdate(tpl, sportTypeId, isMult)
        GetTitleCommon(tpl)
        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "title-col", "5")
        TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")


        'assign value for position taking
        If Not isMult Then
            GetMinMaxEditValue(tpl, sportTypeId, dr)
            TemplateHandler.Assign(tpl, "chk-hdp", "")
        Else
            GetMinMaxAddValue(tpl, sportTypeId, "0"" disabled='true'")
            TemplateHandler.Assign(tpl, "chk-hdp", "disabled")
        End If
        GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

        Dim mHdpMin, mOUMin, mMLMin As Double
        Dim mHdp, mOU, mML As Double
        Dim mHdpMax, mOUMax, mMLMax As Double
        Dim mHdpMax2, mOUMax2, mMLMax2 As Double

        Dim sHdpMin, sOUMin, sMLMin As Double
        Dim sHdp, sOU, sML As Double
        Dim sHdpMax, sOUMax, sMLMax As Double

        Dim msHdpMax, msOUMax, msMLMax As Double
        Dim msHdpMin, msOUMin, msMLMin As Double

        Dim sAutoHdp, sAutoOU, sAutoML As Double
        Dim sAutoHdpMax, sAutoOUMax, sAutoMLMax As Double
        Dim isAutoHdp, isAutoOU, isAutoML As Double

        mHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_max"))
        mOUMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_max"))
        mMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_max"))

        sHdp = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1"))
        sOU = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3"))
        sML = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20"))

        msHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_1_max"))
        msOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_3_max"))
        msMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_20_max"))

        msHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_1_min"))
        msOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_3_min"))
        msMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_20_min"))

        sAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1"))
        sAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_3"))
        sAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20"))

        If Not isMult Then
            mHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_min"))
            mOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_min"))
            mMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_min"))

            mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
            mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
            mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

            sHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1_max"))
            sOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3_max"))
            sMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20_max"))

            sAutoHdpMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1_max"))
            sAutoOUMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_3_max"))
            sAutoMLMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20_max"))

            isAutoHdp = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_1"))
            isAutoOU = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_3"))
            isAutoML = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_20"))

            sHdpMax = sHdpMax - mHdpMin
            sOUMax = sOUMax - mOUMin
            sMLMax = sMLMax - mMLMin
        Else
            sHdpMax = sHdp
            sOUMax = sOU
            sMLMax = sML

            sAutoHdpMax = sAutoHdp
            sAutoOUMax = sAutoOU
            sAutoMLMax = sAutoML
        End If
        'this script to get max PT value of master that can be preseted to
        mHdpMax = IIf(isAutoHdp, msHdpMax, msHdpMax - sHdp)
        mOUMax = IIf(isAutoOU, msOUMax, msOUMax - sOU)
        mMLMax = IIf(isAutoML, msMLMax, msMLMax - sML)

        mHdpMax = IIf(mHdpMax > mHdpMax2, mHdpMax2, mHdpMax)
        mOUMax = IIf(mOUMax > mOUMax2, mOUMax2, mOUMax)
        mMLMax = IIf(mMLMax > mMLMax2, mMLMax2, mMLMax)
        '''''end script '''''''''''

        hiddenHash.Add("maxMS" & sportTypeId & "Hdp", msHdpMax)
        hiddenHash.Add("maxMS" & sportTypeId & "OU", msOUMax)
        hiddenHash.Add("maxMS" & sportTypeId & "ML", msMLMax)

        hiddenHash.Add("minMS" & sportTypeId & "Hdp", msHdpMin)
        hiddenHash.Add("minMS" & sportTypeId & "OU", msOUMin)
        hiddenHash.Add("minMS" & sportTypeId & "ML", msMLMin)

        hiddenHash.Add("maxM" & sportTypeId & "Hdp", mHdpMax2)
        hiddenHash.Add("maxM" & sportTypeId & "OU", mOUMax2)
        hiddenHash.Add("maxM" & sportTypeId & "ML", mMLMax2)

        hiddenHash.Add("isAuto" & sportTypeId & "Hdp", isAutoHdp)
        hiddenHash.Add("isAuto" & sportTypeId & "OU", isAutoOU)
        hiddenHash.Add("isAuto" & sportTypeId & "ML", isAutoML)
        strHidBaseball = PositionTaking.AssignMulipleHidden(hiddenHash)

        'assign data for Position taking dropdowns          
        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

        Dim ptAuto As String
        Dim strChecked As String
        Dim ptMaster As String
        If isMult Then
            pt = selDisabledTpl
            ptAuto = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoHdp Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-Hdp", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
        TemplateHandler.Assign(tpl, "a-hdp-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sHdpMin, sHdpMax, sHdp))
        TemplateHandler.Assign(tpl, "p-hdp-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoHdpMax, sAutoHdp))
        TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)

        If isMult Then
            pt = selDisabledTpl
            ptAuto = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoOU Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-OU", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "OU")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOU))
        TemplateHandler.Assign(tpl, "a-ou-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OU")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOUMin, sOUMax, sOU))
        TemplateHandler.Assign(tpl, "p-ou-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOUMax, sAutoOU))
        TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)

        If isMult Then
            pt = selDisabledTpl
            ptAuto = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoML Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-ML", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "ML")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mML))
        TemplateHandler.Assign(tpl, "a-ml-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sMLMin, sMLMax, sML))
        TemplateHandler.Assign(tpl, "p-ml-val", pt)
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
        tpl = TemplateHandler.LoadTemplate("MemberInfo/MotorSportsPT.html")
        TemplateHandler.RemoveBlock(tpl, "none-edit")
        LoadChkUpdate(tpl, sportTypeId, isMult)
        GetTitleCommon(tpl)
        TemplateHandler.Assign(tpl, "title-col", "4")
        TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")
        'assign value for minbet, maxbet, maxpermatch (all are 0 for adding case)
        If Not isMult Then
            GetMinMaxEditValue(tpl, sportTypeId, dr)
            TemplateHandler.Assign(tpl, "chk-hdp", "")
        Else
            GetMinMaxAddValue(tpl, sportTypeId, "0"" disabled='true'")
            TemplateHandler.Assign(tpl, "chk-hdp", "disabled")
        End If
        GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

        'get data and assign for template Position taking
        Dim mHdpMin, mMLMin As Double
        Dim mHdp, mML As Double
        Dim mHdpMax, mMLMax As Double
        Dim mHdpMax2, mMLMax2 As Double

        Dim sHdpMin, sMLMin As Double
        Dim sHdp, sML As Double
        Dim sHdpMax, sMLMax As Double

        Dim msHdpMax, msMLMax As Double
        Dim msHdpMin, msMLMin As Double

        Dim sAutoHdp, sAutoML As Double
        Dim sAutoHdpMax, sAutoMLMax As Double
        Dim isAutoHdp, isAutoML As Double

        mHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_max"))
        mMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_max"))

        sHdp = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1"))
        sML = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20"))

        msHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_1_max"))
        msMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_20_max"))

        msHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_1_min"))
        msMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_20_min"))

        sAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1"))
        sAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20"))

        If Not isMult Then
            mHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_min"))
            mMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_min"))

            mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
            mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

            sHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1_max"))
            sMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20_max"))

            sAutoHdpMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1_max"))
            sAutoMLMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20_max"))

            isAutoHdp = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_1"))
            isAutoML = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_20"))
            sHdpMax = sHdpMax - mHdpMin
            sMLMax = sMLMax - mMLMin
        Else
            sHdpMax = sHdp
            sMLMax = sML

            sAutoHdpMax = sAutoHdp
            sAutoMLMax = sAutoML
        End If

        mHdpMax = IIf(isAutoHdp, msHdpMax, msHdpMax - sHdp)
        mMLMax = IIf(isAutoML, msMLMax, msMLMax - sML)

        mHdpMax = IIf(mHdpMax > mHdpMax2, mHdpMax2, mHdpMax)
        mMLMax = IIf(mMLMax > mMLMax2, mMLMax2, mMLMax)

        hiddenHash.Add("maxMS" & sportTypeId & "Hdp", msHdpMax)
        hiddenHash.Add("maxMS" & sportTypeId & "ML", msMLMax)

        hiddenHash.Add("minMS" & sportTypeId & "Hdp", msHdpMin)
        hiddenHash.Add("minMS" & sportTypeId & "ML", msMLMin)

        hiddenHash.Add("maxM" & sportTypeId & "Hdp", mHdpMax2)
        hiddenHash.Add("maxM" & sportTypeId & "ML", mMLMax2)

        hiddenHash.Add("isAuto" & sportTypeId & "Hdp", isAutoHdp)
        hiddenHash.Add("isAuto" & sportTypeId & "ML", isAutoML)
        strHidMoto = PositionTaking.AssignMulipleHidden(hiddenHash)

        'assign data for Position taking dropdowns     
        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

        Dim ptAuto As String
        Dim strChecked As String
        Dim ptMaster As String
        If isMult Then
            pt = selDisabledTpl
            ptAuto = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoHdp Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-Hdp", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
        TemplateHandler.Assign(tpl, "a-hdp-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sHdpMin, sHdpMax, sHdp))
        TemplateHandler.Assign(tpl, "p-hdp-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoHdpMax, sAutoHdp))
        TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)

        If isMult Then
            pt = selDisabledTpl
            ptAuto = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoML Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            pt = selectTpl
            ptAuto = selDisabledTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(tpl, "checked-ML", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "ML")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mML))
        TemplateHandler.Assign(tpl, "a-ml-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sMLMin, sMLMax, sML))
        TemplateHandler.Assign(tpl, "p-ml-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoMLMax, sAutoML))
        TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)


        GetDataMotoSports = tpl
    End Function
#End Region

#Region "Get Title Common"
    Sub GetTitleCommon(ByRef tpl As String)
        TemplateHandler.Assign(tpl, "bet-text", Resources.Lang.lblBetSetting)
        TemplateHandler.Assign(tpl, "minbet-text", Resources.Lang.lblMinBet)
        TemplateHandler.Assign(tpl, "maxbet-text", Resources.Lang.lblMaxBet)
        TemplateHandler.Assign(tpl, "maxpermatch-text", Resources.Lang.lblMaxPerMatch)
        TemplateHandler.Assign(tpl, "pt-text", Resources.Lang.positiontaking)
        TemplateHandler.Assign(tpl, "auto-super-text", Resources.Lang.lblAutoSuperPresetPosition)
        TemplateHandler.Assign(tpl, "a-presetpt-text", Resources.Lang.lblMasterPresetPosition)
        TemplateHandler.Assign(tpl, "p-presetpt-text", Resources.Lang.lblSuperPresetPosition)
        TemplateHandler.Assign(tpl, "au-presetpt-text", Resources.Lang.lblAutoSuperPresetPosition)
        TemplateHandler.Assign(tpl, "hdp-text", Resources.Lang.lblHandicapFC)
        TemplateHandler.Assign(tpl, "ml-text", Resources.Lang.moneyline)
        TemplateHandler.Assign(tpl, "copy-hdp-text", Resources.Lang.lblCopyFromHandicap)
        TemplateHandler.Assign(tpl, "copy-hdp-col", "4")
    End Sub
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

    End Sub
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
        Dim strfccomm As String = String.Empty
        Dim strMaxPerBall As String = String.Empty
        If isMult Then
            str = "<input type=""checkbox"" id=""chkUpdComm"" onclick=""AllowUpdate(this,false);"" name=""chkUpdComm""/>"

            strSS = "<input type=""checkbox"" id=""chkUpdStatus"" onclick=""AllowUpdate(this,true);"" name=""chkUpdStatus""/>"

            strMaxPerBall = "<input type=""checkbox"" id=""chkUpdMaxPerBall"" onclick=""AllowUpdate(this,true);"" name=""chkUpdMaxPerBall""/>"

        End If

        TemplateHandler.Assign(strHtml, "{status-text}", strSS & Resources.Lang.status)
        TemplateHandler.Assign(strHtml, "{commission-text}", str & Resources.Lang.commission)
        TemplateHandler.Assign(strHtml, "{groupa-text}", Resources.Lang.lblGroupa)
        TemplateHandler.Assign(strHtml, "{groupb-text}", Resources.Lang.lblGroupb)
        TemplateHandler.Assign(strHtml, "{groupc-text}", Resources.Lang.lblGroupc)
        TemplateHandler.Assign(strHtml, "{groupd-text}", Resources.Lang.lblGroupd)
        TemplateHandler.Assign(strHtml, "{discountcs-text}", Resources.Lang.lblOtherComm)
        TemplateHandler.Assign(strHtml, "{discount1x2-text}", Resources.Lang.lbl1x2Comm)

        TemplateHandler.Assign(strHtml, "{othersetting}", strMaxPerBall + Resources.Lang.othersetting)
        TemplateHandler.Assign(strHtml, "{lblMaxPerBall}", Resources.Lang.maxperball)
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

#Region "Assign Info for Number Game"
    Function GetDataNumberGame(ByVal dr As DataRow, ByVal isMult As Boolean) As String
        Dim numbergameTpl As String = String.Empty
        Dim headerSTpl As String = String.Empty
        Dim hiddenHash As New Hashtable
        Dim ptAuto As String = String.Empty
        Dim pt As String = String.Empty


        numbergameTpl = TemplateHandler.LoadTemplate("MemberInfo/NumberGamePT.html")
        GetTitleInfo(numbergameTpl, isMult)

        If (Not UserPermission.IsShowNumberGame) Then
            TemplateHandler.Assign(numbergameTpl, "hideClss", "hidden")
        Else
            If (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission) Then
                TemplateHandler.Assign(numbergameTpl, "hideClss", "hidden")
            Else
                TemplateHandler.Assign(numbergameTpl, "hideClss", String.Empty)
            End If
        End If

        If UserPermission.IsShowNumberGame Then
            If Not isMult Then
                GetMaxPerBallEditValue(numbergameTpl, 161, dr)
            Else
                GetMaxPerBallAddValue(numbergameTpl, "0"" disabled='true'")
            End If
            GetLimitMaxPerBall(dr, numbergameTpl, 161)
        Else
            TemplateHandler.RemoveBlock(numbergameTpl, "none-maxperball")
        End If


        TemplateHandler.RemoveBlock(numbergameTpl, "none-edit")
        LoadChkUpdate(numbergameTpl, 161, isMult)
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
        TemplateHandler.Assign(numbergameTpl, "copy-oulive-text", Resources.Lang.lblcopyfromoverunder)
        TemplateHandler.Assign(numbergameTpl, "au-preset-live", Resources.Lang.lblAutoSuperPresetPosition)
        TemplateHandler.Assign(numbergameTpl, "copy-1stlastou-text", Resources.Lang.lbl1stcopyfromoverunder)
        TemplateHandler.Assign(numbergameTpl, "next-combo-text", Resources.Lang.nextcombo)
        TemplateHandler.Assign(numbergameTpl, "numberGroup-text", Resources.Lang.numberGroup)


        If (Not UserPermission.IsShowNumberGroup) Then
            TemplateHandler.Assign(numbergameTpl, "hide", "hide")
        End If



        'assign value for position taking
        If Not isMult Then
            GetMinMaxEditValue(numbergameTpl, 161, dr)
            TemplateHandler.Assign(numbergameTpl, "chk-1611stOUD", "")
            TemplateHandler.Assign(numbergameTpl, "chk-161OUL", "")
        Else
            GetMinMaxAddValue(numbergameTpl, 161, "0"" disabled='true'")
            TemplateHandler.Assign(numbergameTpl, "chk-1611stOUD", "disabled")
            TemplateHandler.Assign(numbergameTpl, "chk-161OUL", "disabled")
        End If
        GetLimitMinMaxBet(dr, numbergameTpl, 161)

        Dim m1stOUMin, m1stOEMin, mOEMin, mWRMin As Double
        Dim m1stOU, m1stOE, mOE, mWR As Double
        Dim m1stOUMax, m1stOEMax, mOEMax, mWRMax As Double
        Dim m1stOUMax2, m1stOEMax2, mOEMax2, mWRMax2 As Double

        Dim mOUMinL, mOEMinL, mHLMinL, mNCMinL, mNGMinL As Double
        Dim mOUL, mOEL, mHLL, mNCL, mNGL As Double
        Dim mOUMaxL, mOEMaxL, mHLMaxL, mNCMaxL, mNGMaxL As Double
        Dim mOUMax2L, mOEMax2L, mHLMax2L, mNCMax2L, mNGMax2L As Double

        Dim s1stOUMin, s1stOEMin, sOEMin, sWRMin As Double
        Dim s1stOU, s1stOE, sOE, sWR As Double
        Dim s1stOUMax, s1stOEMax, sOEMax, sWRMax As Double

        Dim sOUMinL, sOEMinL, sHLMinL, sNCMinL, sNGMinL As Double
        Dim sOUL, sOEL, sHLL, sNCL, sNGL As Double
        Dim sOUMaxL, sOEMaxL, sHLMaxL, sNCMaxL, sNGMaxL As Double

        Dim ms1stOUMax, ms1stOEMax, msOEMax, msWRMax As Double
        Dim ms1stOUMin, ms1stOEMin, msOEMin, msWRMin As Double

        Dim msOUMaxL, msOEMaxL, msHLMaxL, msNCMaxL, msNGMaxL As Double
        Dim msOUMinL, msOEMinL, msHLMinL, msNCMinL, msNGMinL As Double

        Dim sAuto1stOU, sAuto1stOE, sAutoOE, sAutoWR As Double
        Dim sAuto1stOUMax, sAuto1stOEMax, sAutoOEMax, sAutoWRMax As Double
        Dim isAuto1stOU, isAuto1stOE, isAutoOE, isAutoWR As Double

        Dim sAutoOUL, sAutoOEL, sAutoHLL, sAutoNCL, sAutoNGL As Double
        Dim sAutoOUMaxL, sAutoOEMaxL, sAutoHLMaxL, sAutoNCMaxL, sAutoNGMaxL As Double
        Dim isAutoOUL, isAutoOEL, isAutoHLL, isAutoNCL, isAutoNGL As Double

        m1stOUMax2 = CastAide.AsDouble(dr("v161mpositiontaking_81_max"))
        m1stOEMax2 = CastAide.AsDouble(dr("v161mpositiontaking_83_max"))
        'mOUMax2 = CastAide.AsDouble(dr("v161mpositiontaking_85_max"))
        mOEMax2 = CastAide.AsDouble(dr("v161mpositiontaking_86_max"))
        mWRMax2 = CastAide.AsDouble(dr("v161mpositiontaking_88_max"))

        mOUMax2L = CastAide.AsDouble(dr("v161mlivepositiontaking_85_max"))
        mOEMax2L = CastAide.AsDouble(dr("v161mlivepositiontaking_86_max"))
        mHLMax2L = CastAide.AsDouble(dr("v161mlivepositiontaking_87_max"))
        mNCMax2L = CastAide.AsDouble(dr("v161mlivepositiontaking_89_max"))
        mNGMax2L = CastAide.AsDouble(dr("v161mlivepositiontaking_90_max"))

        s1stOU = CastAide.AsDouble(dr("v161spositiontaking_81"))
        s1stOE = CastAide.AsDouble(dr("v161spositiontaking_83"))
        'sOU = CastAide.AsDouble(dr("v161spositiontaking_85"))
        sOE = CastAide.AsDouble(dr("v161spositiontaking_86"))
        sWR = CastAide.AsDouble(dr("v161spositiontaking_88"))

        sOUL = CastAide.AsDouble(dr("v161slivepositiontaking_85"))
        sOEL = CastAide.AsDouble(dr("v161slivepositiontaking_86"))
        sHLL = CastAide.AsDouble(dr("v161slivepositiontaking_87"))
        sNCL = CastAide.AsDouble(dr("v161slivepositiontaking_89"))
        sNGL = CastAide.AsDouble(dr("v161slivepositiontaking_90"))

        ms1stOUMax = CastAide.AsDouble(dr("v161mspositiontaking_81_max"))
        ms1stOEMax = CastAide.AsDouble(dr("v161mspositiontaking_83_max"))
        'msOUMax = CastAide.AsDouble(dr("v161mspositiontaking_85_max"))
        msOEMax = CastAide.AsDouble(dr("v161mspositiontaking_86_max"))
        msWRMax = CastAide.AsDouble(dr("v161mspositiontaking_88_max"))


        msOUMaxL = CastAide.AsDouble(dr("v161mslivepositiontaking_85_max"))
        msOEMaxL = CastAide.AsDouble(dr("v161mslivepositiontaking_86_max"))
        msHLMaxL = CastAide.AsDouble(dr("v161mslivepositiontaking_87_max"))
        msNCMaxL = CastAide.AsDouble(dr("v161mslivepositiontaking_89_max"))
        msNGMaxL = CastAide.AsDouble(dr("v161mslivepositiontaking_90_max"))


        ms1stOUMin = CastAide.AsDouble(dr("v161mspositiontaking_81_min"))
        ms1stOEMin = CastAide.AsDouble(dr("v161mspositiontaking_83_min"))
        'msOUMin = CastAide.AsDouble(dr("v161mspositiontaking_85_min"))
        msOEMin = CastAide.AsDouble(dr("v161mspositiontaking_86_min"))
        msWRMin = CastAide.AsDouble(dr("v161mspositiontaking_88_min"))

        msOUMinL = CastAide.AsDouble(dr("v161mslivepositiontaking_85_min"))
        msOEMinL = CastAide.AsDouble(dr("v161mslivepositiontaking_86_min"))
        msHLMinL = CastAide.AsDouble(dr("v161mslivepositiontaking_87_min"))
        msNCMinL = CastAide.AsDouble(dr("v161mslivepositiontaking_89_min"))
        msNGMinL = CastAide.AsDouble(dr("v161mslivepositiontaking_90_min"))


        sAuto1stOU = CastAide.AsDouble(dr("auto_v161spositiontaking_81"))
        sAuto1stOE = CastAide.AsDouble(dr("auto_v161spositiontaking_83"))
        'sAutoOU = CastAide.AsDouble(dr("auto_v161spositiontaking_85"))
        sAutoOE = CastAide.AsDouble(dr("auto_v161spositiontaking_86"))
        sAutoWR = CastAide.AsDouble(dr("auto_v161spositiontaking_88"))

        sAutoOUL = CastAide.AsDouble(dr("auto_v161slivepositiontaking_85"))
        sAutoOEL = CastAide.AsDouble(dr("auto_v161slivepositiontaking_86"))
        sAutoHLL = CastAide.AsDouble(dr("auto_v161slivepositiontaking_87"))
        sAutoNCL = CastAide.AsDouble(dr("auto_v161slivepositiontaking_89"))
        sAutoNGL = CastAide.AsDouble(dr("auto_v161slivepositiontaking_90"))

        If Not isMult Then
            m1stOUMin = CastAide.AsDouble(dr("v161mpositiontaking_81_min"))
            m1stOEMin = CastAide.AsDouble(dr("v161mpositiontaking_83_min"))
            mOEMin = CastAide.AsDouble(dr("v161mpositiontaking_86_min"))
            mWRMin = CastAide.AsDouble(dr("v161mpositiontaking_88_min"))

            mOUMinL = CastAide.AsDouble(dr("v161mlivepositiontaking_85_min"))
            mOEMinL = CastAide.AsDouble(dr("v161mlivepositiontaking_86_min"))
            mHLMinL = CastAide.AsDouble(dr("v161mlivepositiontaking_87_min"))
            mNCMinL = CastAide.AsDouble(dr("v161mlivepositiontaking_89_min"))
            mNGMinL = CastAide.AsDouble(dr("v161mlivepositiontaking_90_min"))

            m1stOU = CastAide.AsDouble(dr("v161mpositiontaking_81"))
            m1stOE = CastAide.AsDouble(dr("v161mpositiontaking_83"))
            mOE = CastAide.AsDouble(dr("v161mpositiontaking_86"))
            mWR = CastAide.AsDouble(dr("v161mpositiontaking_88"))

            mOUL = CastAide.AsDouble(dr("v161mlivepositiontaking_85"))
            mOEL = CastAide.AsDouble(dr("v161mlivepositiontaking_86"))
            mHLL = CastAide.AsDouble(dr("v161mlivepositiontaking_87"))
            mNCL = CastAide.AsDouble(dr("v161mlivepositiontaking_89"))
            mNGL = CastAide.AsDouble(dr("v161mlivepositiontaking_90"))

            ' Dim s1stOUMax, s1stOEMax, sOUMax, sOEMax As Double
            s1stOUMax = CastAide.AsDouble(dr("v161spositiontaking_81_max"))
            s1stOEMax = CastAide.AsDouble(dr("v161spositiontaking_83_max"))
            sOEMax = CastAide.AsDouble(dr("v161spositiontaking_86_max"))
            sWRMax = CastAide.AsDouble(dr("v161spositiontaking_88_max"))

            sOUMaxL = CastAide.AsDouble(dr("v161slivepositiontaking_85_max"))
            sOEMaxL = CastAide.AsDouble(dr("v161slivepositiontaking_86_max"))
            sHLMaxL = CastAide.AsDouble(dr("v161slivepositiontaking_87_max"))
            sNCMaxL = CastAide.AsDouble(dr("v161slivepositiontaking_89_max"))
            sNGMaxL = CastAide.AsDouble(dr("v161slivepositiontaking_90_max"))

            ' Dim sAuto1stOUMax, sAuto1stOEMax, sAutoOUMax, sAutoOEMax As Double
            sAuto1stOUMax = CastAide.AsDouble(dr("auto_v161spositiontaking_81_max"))
            sAuto1stOEMax = CastAide.AsDouble(dr("auto_v161spositiontaking_83_max"))
            sAutoOEMax = CastAide.AsDouble(dr("auto_v161spositiontaking_86_max"))
            sAutoWRMax = CastAide.AsDouble(dr("auto_v161spositiontaking_88_max"))

            sAutoOUMaxL = CastAide.AsDouble(dr("auto_v161slivepositiontaking_85_max"))
            sAutoOEMaxL = CastAide.AsDouble(dr("auto_v161slivepositiontaking_86_max"))
            sAutoHLMaxL = CastAide.AsDouble(dr("auto_v161slivepositiontaking_87_max"))
            sAutoNCMaxL = CastAide.AsDouble(dr("auto_v161slivepositiontaking_89_max"))
            sAutoNGMaxL = CastAide.AsDouble(dr("auto_v161slivepositiontaking_90_max"))

            'Dim isAuto1stOU, isAuto1stOE, isAutoOU, isAutoOE As Double
            isAuto1stOU = CastAide.AsInt16(dr("isauto_v161spositiontaking_81"))
            isAuto1stOE = CastAide.AsInt16(dr("isauto_v161spositiontaking_83"))
            isAutoOE = CastAide.AsInt16(dr("isauto_v161spositiontaking_86"))
            isAutoWR = CastAide.AsInt16(dr("isauto_v161spositiontaking_88"))

            isAutoOUL = CastAide.AsInt16(dr("isauto_v161slivepositiontaking_85"))
            isAutoOEL = CastAide.AsInt16(dr("isauto_v161slivepositiontaking_86"))
            isAutoHLL = CastAide.AsInt16(dr("isauto_v161slivepositiontaking_87"))
            isAutoNCL = CastAide.AsInt16(dr("isauto_v161slivepositiontaking_89"))
            isAutoNGL = CastAide.AsInt16(dr("isauto_v161slivepositiontaking_90"))

            s1stOUMax = s1stOUMax - m1stOUMin
            s1stOEMax = s1stOEMax - m1stOEMin
            sOEMax = sOEMax - mOEMin
            sWRMax = sWRMax - mWRMin

            sOUMaxL = sOUMaxL - mOUMinL
            sOEMaxL = sOEMaxL - mOEMinL
            sHLMaxL = sHLMaxL - mHLMinL
            sNCMaxL = sNCMaxL - mNCMinL
            sNGMaxL = sNGMaxL - mNGMinL

        Else
            s1stOUMax = s1stOU
            s1stOEMax = s1stOE
            sOEMax = sOE
            sWRMax = sWR

            sOUMaxL = sOUL
            sOEMaxL = sOEL
            sHLMaxL = sHLL
            sNCMaxL = sNCL
            sNGMaxL = sNGL

            sAuto1stOUMax = sAuto1stOU
            sAuto1stOEMax = sAuto1stOE
            sAutoOEMax = sAutoOE
            sAutoWRMax = sAutoWR

            sAutoOUMaxL = sAutoOUL
            sAutoOEMaxL = sAutoOEL
            sAutoHLMaxL = sAutoHLL
            sAutoNCMaxL = sAutoNCL
            sAutoNGMaxL = sAutoNGL

        End If

        'this script to get max PT value of master that can preset to
        '  Dim m1stOUMax, m1stOEMax, mOUMax, mOEMax As Double
        m1stOUMax = IIf(isAuto1stOU, ms1stOUMax, ms1stOUMax - s1stOU)
        m1stOEMax = IIf(isAuto1stOE, ms1stOEMax, ms1stOEMax - s1stOE)
        mOEMax = IIf(isAutoOE, msOEMax, msOEMax - sOE)
        mWRMax = IIf(isAutoWR, msWRMax, msWRMax - sWR)

        mOUMaxL = IIf(isAutoOUL, msOUMaxL, msOUMaxL - sOUL)
        mOEMaxL = IIf(isAutoOEL, msOEMaxL, msOEMaxL - sOEL)
        mHLMaxL = IIf(isAutoHLL, msHLMaxL, msHLMaxL - sHLL)
        mNCMaxL = IIf(isAutoNCL, msNCMaxL, msNCMaxL - sNCL)
        mNGMaxL = IIf(isAutoNGL, msNGMaxL, msNGMaxL - sNGL)


        m1stOUMax = IIf(m1stOUMax > m1stOUMax2, m1stOUMax2, m1stOUMax)
        m1stOEMax = IIf(m1stOEMax > m1stOEMax2, m1stOEMax2, m1stOEMax)
        mOEMax = IIf(mOEMax > mOEMax2, mOEMax2, mOEMax)
        mWRMax = IIf(mWRMax > mWRMax2, mWRMax2, mWRMax)

        mOUMaxL = IIf(mOUMaxL > mOUMax2L, mOUMax2L, mOUMaxL)
        mOEMaxL = IIf(mOEMaxL > mOEMax2L, mOEMax2L, mOEMaxL)
        mHLMaxL = IIf(mHLMaxL > mHLMax2L, mHLMax2L, mHLMaxL)
        mNCMaxL = IIf(mNCMaxL > mNCMax2L, mNCMax2L, mNCMaxL)
        mNGMaxL = IIf(mNGMaxL > mNGMax2L, mNGMax2L, mNGMaxL)

        ''''''''end script''''''''''''''            

        hiddenHash.Add("maxMS1611stOUD", ms1stOUMax)
        hiddenHash.Add("maxMS1611stOED", ms1stOEMax)
        hiddenHash.Add("maxMS161OED", msOEMax)
        hiddenHash.Add("maxMS161WRD", msWRMax)

        hiddenHash.Add("maxMS161OUL", msOUMaxL)
        hiddenHash.Add("maxMS161OEL", msOEMaxL)
        hiddenHash.Add("maxMS161HLL", msHLMaxL)
        hiddenHash.Add("maxMS161NCL", msNCMaxL)
        hiddenHash.Add("maxMS161NGL", msNGMaxL)

        hiddenHash.Add("minMS1611stOUD", ms1stOUMin)
        hiddenHash.Add("minMS1611stOED", ms1stOEMin)
        hiddenHash.Add("minMS161OED", msOEMin)
        hiddenHash.Add("minMS161WRD", msWRMin)

        hiddenHash.Add("maxM1611stOUD", m1stOUMax2)
        hiddenHash.Add("maxM1611stOED", m1stOEMax2)
        hiddenHash.Add("maxM161OED", mOEMax2)
        hiddenHash.Add("maxM161WRD", mWRMax2)

        hiddenHash.Add("maxM161OUL", mOUMax2L)
        hiddenHash.Add("maxM161OEL", mOEMax2L)
        hiddenHash.Add("maxM161HLL", mHLMax2L)
        hiddenHash.Add("maxM161NCL", mNCMax2L)
        hiddenHash.Add("maxM161NGL", mNGMax2L)


        hiddenHash.Add("isAuto1611stOUD", isAuto1stOU)
        hiddenHash.Add("isAuto1611stOED", isAuto1stOE)
        hiddenHash.Add("isAuto161OED", isAutoOE)
        hiddenHash.Add("isAuto161WRD", isAutoWR)

        hiddenHash.Add("isAuto161OUL", isAutoOUL)
        hiddenHash.Add("isAuto161OEL", isAutoOEL)
        hiddenHash.Add("isAuto161HLL", isAutoHLL)
        hiddenHash.Add("isAuto161NCL", isAutoNCL)
        hiddenHash.Add("isAuto161NGL", isAutoNGL)

        strHidNumberGame = PositionTaking.AssignMulipleHidden(hiddenHash)

        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM1611stOUD")
        TemplateHandler.Assign(numbergameTpl, "a-1stlastou-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listS1611stOUD")
        TemplateHandler.Assign(numbergameTpl, "p-1stlastou-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS1611stOUD")
        TemplateHandler.Assign(numbergameTpl, "au-1stlastou-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM161OUL")
        TemplateHandler.Assign(numbergameTpl, "a-Lou-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listS161OUL")
        TemplateHandler.Assign(numbergameTpl, "p-Lou-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS161OUL")
        TemplateHandler.Assign(numbergameTpl, "au-Lou-btn", btnTpl)

        Dim strChecked As String
        Dim ptMaster As String

        If isMult Then
            pt = selDisabledTpl
            ptAuto = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAuto1stOU Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(numbergameTpl, "checked-1stOUD", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1611stOUD")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(m1stOUMin, m1stOUMax, m1stOU))
        TemplateHandler.Assign(numbergameTpl, "a-1stlastou-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS1611stOUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(s1stOUMin, s1stOUMax, s1stOU))
        TemplateHandler.Assign(numbergameTpl, "p-1stlastou-val", pt)

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1611stOUD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAuto1stOUMax, sAuto1stOU))
        TemplateHandler.Assign(numbergameTpl, "au-1stlastou-val", ptAuto)

        If isMult Then
            pt = selDisabledTpl
            ptAuto = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAuto1stOE Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(numbergameTpl, "checked-1stOED", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1611stOED")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(m1stOEMin, m1stOEMax, m1stOE))
        TemplateHandler.Assign(numbergameTpl, "a-1stlastoe-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS1611stOED")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(s1stOEMin, s1stOEMax, s1stOE))
        TemplateHandler.Assign(numbergameTpl, "p-1stlastoe-val", pt)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1611stOED")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAuto1stOEMax, sAuto1stOE))
        TemplateHandler.Assign(numbergameTpl, "au-1stlastoe-val", ptAuto)


        If isMult Then
            pt = selDisabledTpl
            ptAuto = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoOE Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(numbergameTpl, "checked-OED", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM161OED")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOE))
        TemplateHandler.Assign(numbergameTpl, "a-oe-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS161OED")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOEMin, sOEMax, sOE))
        TemplateHandler.Assign(numbergameTpl, "p-oe-val", pt)

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161OED")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOEMax, sAutoOE))
        TemplateHandler.Assign(numbergameTpl, "au-oe-val", ptAuto)

        'for Warrior
        If isMult Then
            pt = selDisabledTpl
            ptAuto = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoWR Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(numbergameTpl, "checked-WRD", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM161WRD")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mWRMin, mWRMax, mWR))
        TemplateHandler.Assign(numbergameTpl, "a-warrior-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS161WRD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sWRMin, sWRMax, sWR))
        TemplateHandler.Assign(numbergameTpl, "p-warrior-val", pt)

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161WRD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoWRMax, sAutoWR))
        TemplateHandler.Assign(numbergameTpl, "au-warrior-val", ptAuto)

        ' For Live
        If isMult Then
            pt = selDisabledTpl
            ptAuto = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoOUL Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(numbergameTpl, "checked-OUL", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM161OUL")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mOUMinL, mOUMaxL, mOUL))
        TemplateHandler.Assign(numbergameTpl, "a-Lou-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS161OUL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOUMinL, sOUMaxL, sOUL))
        TemplateHandler.Assign(numbergameTpl, "p-Lou-val", pt)

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161OUL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOUMaxL, sAutoOUL))
        TemplateHandler.Assign(numbergameTpl, "au-Lou-val", ptAuto)


        If isMult Then
            pt = selDisabledTpl
            ptAuto = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoOEL Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(numbergameTpl, "checked-OEL", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM161OEL")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mOEMinL, mOEMaxL, mOEL))
        TemplateHandler.Assign(numbergameTpl, "a-Loe-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS161OEL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sOEMinL, sOEMaxL, sOEL))
        TemplateHandler.Assign(numbergameTpl, "p-Loe-val", pt)

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161OEL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoOEMaxL, sAutoOEL))
        TemplateHandler.Assign(numbergameTpl, "au-Loe-val", ptAuto)


        If isMult Then
            pt = selDisabledTpl
            ptAuto = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoHLL Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(numbergameTpl, "checked-HLL", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM161HLL")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mHLMinL, mHLMaxL, mHLL))
        TemplateHandler.Assign(numbergameTpl, "a-Lhl-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS161HLL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sHLMinL, sHLMaxL, sHLL))
        TemplateHandler.Assign(numbergameTpl, "p-Lhl-val", pt)

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161HLL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoHLMaxL, sAutoHLL))
        TemplateHandler.Assign(numbergameTpl, "au-Lhl-val", ptAuto)


        'for Next Combo
        If isMult Then
            pt = selDisabledTpl
            ptAuto = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoNCL Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(numbergameTpl, "checked-NCL", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM161NCL")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mNCMinL, mNCMaxL, mNCL))
        TemplateHandler.Assign(numbergameTpl, "a-nextCombo-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS161NCL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sNCMinL, sNCMaxL, sNCL))
        TemplateHandler.Assign(numbergameTpl, "p-nextCombo-val", pt)

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161NCL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoNCMaxL, sAutoNCL))
        TemplateHandler.Assign(numbergameTpl, "au-nextCombo-val", ptAuto)


        'for Number Group
        If isMult Then
            pt = selDisabledTpl
            ptAuto = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoNGL Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(numbergameTpl, "checked-NGL", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM161NGL")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mNGMinL, mNGMaxL, mNGL))
        TemplateHandler.Assign(numbergameTpl, "a-NumGr-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS161NGL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sNGMinL, sNGMaxL, sNGL))
        TemplateHandler.Assign(numbergameTpl, "p-NumGr-val", pt)

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161NGL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoNGMaxL, sAutoNGL))
        TemplateHandler.Assign(numbergameTpl, "au-NumGr-val", ptAuto)


        Return numbergameTpl

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
        TemplateHandler.Assign(strHtml, "{warrior-text}", Resources.Lang.warrior)
        TemplateHandler.Assign(strHtml, "{oe-text}", Resources.Lang.lblOddEven)

        Return strHtml
    End Function

#End Region

#Region "Assign Info for Mix Sports"
    Function GetDataMixSport(ByVal dr As DataRow, ByVal isMult As Boolean) As String
        Dim mixsportsTpl As String = String.Empty
        Dim hiddenHash As New Hashtable

        Dim minbetMin As Double
        Dim maxbetMax As Double
        Dim maxpermatchMin As Double
        Dim maxpermatchMax As Double


        mixsportsTpl = TemplateHandler.LoadTemplate("MemberInfo/MixSportsPT.html")
        LoadChkUpdate(mixsportsTpl, "99_M", isMult)
        GetTitleCommon(mixsportsTpl)
        TemplateHandler.Assign(mixsportsTpl, "mp-text", Resources.Lang.lblMixParlay)
        TemplateHandler.Assign(mixsportsTpl, "title-col", "6")
        TemplateHandler.RemoveBlock(mixsportsTpl, "none-edit")


        'assign value for position taking
        If Not isMult Then
            TemplateHandler.Assign(mixsportsTpl, "minbet99_M-val", DataFormat.FormatNumber1(dr("minbetv99MP")))
            TemplateHandler.Assign(mixsportsTpl, "maxbet99_M-val", DataFormat.FormatNumber1(dr("maxbetv99MP")))
            TemplateHandler.Assign(mixsportsTpl, "maxpermatch99_M-val", DataFormat.FormatNumber1(dr("maxpermatchv99MP")))
        Else
            Dim strReplace As String = "0"" disabled='true'"
            TemplateHandler.Assign(mixsportsTpl, "minbet99_M-val", strReplace)
            TemplateHandler.Assign(mixsportsTpl, "maxbet99_M-val", strReplace)
            TemplateHandler.Assign(mixsportsTpl, "maxpermatch99_M-val", strReplace)


        End If

        minbetMin = DataFormat.FormatNumber1(dr("MinBetv99MP_Max"))
        maxbetMax = DataFormat.FormatNumber1(dr("MaxBetv99MP_Min"))
        maxpermatchMin = DataFormat.FormatNumber1(dr("MaxPerMatchv99MP_Min"))
        maxpermatchMax = DataFormat.FormatNumber1(dr("MaxPerMatchv99MP_Max"))

        TemplateHandler.Assign(mixsportsTpl, "minbet-min-99_M", DataFormat.FormatNumber1(minbetMin))
        TemplateHandler.Assign(mixsportsTpl, "maxbet-max-99_M", DataFormat.FormatNumber1(maxbetMax))
        TemplateHandler.Assign(mixsportsTpl, "maxpermatch-min-99_M", DataFormat.FormatNumber1(maxpermatchMin))
        TemplateHandler.Assign(mixsportsTpl, "maxpermatch-max-99_M", DataFormat.FormatNumber1(maxpermatchMax))
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


        Dim mMPMin As Double
        Dim mMP As Double
        Dim mMPMax As Double
        Dim mMPMax2 As Double

        Dim sMPMin As Double
        Dim sMP As Double
        Dim sMPMax As Double

        Dim msMPMax As Double
        Dim msMPMin As Double

        Dim sAutoMP As Double
        Dim sAutoMPMax As Double
        Dim isAutoMP As Double

        mMPMax2 = CastAide.AsDouble(dr("v99mpositiontaking_9_max"))
        sMP = CastAide.AsDouble(dr("v99spositiontaking_9"))
        msMPMax = CastAide.AsDouble(dr("v99mspositiontaking_9_max"))
        msMPMin = CastAide.AsDouble(dr("v99mspositiontaking_9_min"))
        sAutoMP = CastAide.AsDouble(dr("auto_v99spositiontaking_9"))

        If Not isMult Then
            mMPMin = CastAide.AsDouble(dr("v99mpositiontaking_9_min"))
            mMP = CastAide.AsDouble(dr("v99mpositiontaking_9"))
            sMPMax = CastAide.AsDouble(dr("v99spositiontaking_9_max"))
            sAutoMPMax = CastAide.AsDouble(dr("auto_v99spositiontaking_9_max"))
            isAutoMP = CastAide.AsInt16(dr("isauto_v99spositiontaking_9"))
            sMPMax = sMPMax - mMPMin
        Else
            sMPMax = sMP
            sAutoMPMax = sAutoMP
        End If

        'this script to get max PT value of master that can preset to
        mMPMax = IIf(isAutoMP, msMPMax, msMPMax - sMP)
        mMPMax = IIf(mMPMax > mMPMax2, mMPMax2, mMPMax)

        ''''''''end script''''''''''''''            

        hiddenHash.Add("maxMS99_M", msMPMax)
        hiddenHash.Add("minMS99_M", msMPMin)
        hiddenHash.Add("maxM99_M", mMPMax2)
        hiddenHash.Add("isAuto99_M", isAutoMP)


        strHidMixSports = PositionTaking.AssignMulipleHidden(hiddenHash)

        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM99_M")
        TemplateHandler.Assign(mixsportsTpl, "a-mp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listS99_M")
        TemplateHandler.Assign(mixsportsTpl, "p-mp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS99_M")
        TemplateHandler.Assign(mixsportsTpl, "au-mp-btn", btnTpl)

        Dim ptAuto As String
        Dim strChecked As String
        Dim ptMaster As String
        Dim pt As String
        If isMult Then
            pt = selDisabledTpl
            ptAuto = selDisabledTpl
            ptMaster = selDisabledTpl
            strChecked = "disabled"
        ElseIf isAutoMP Then
            pt = selDisabledTpl
            ptAuto = selectTpl
            ptMaster = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabledTpl
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(mixsportsTpl, "checked-mp", strChecked)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listM99_M")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mMPMin, mMPMax, mMP))
        TemplateHandler.Assign(mixsportsTpl, "a-mp-val", ptMaster)

        TemplateHandler.Assign(pt, "ddl-Id", "listS99_M")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(sMPMin, sMPMax, sMP))
        TemplateHandler.Assign(mixsportsTpl, "p-mp-val", pt)

        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS99_M")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, sAutoMPMax, sAutoMP))
        TemplateHandler.Assign(mixsportsTpl, "au-mp-val", ptAuto)


        Return mixsportsTpl
    End Function
#End Region

#Region "LoadChkUpdate"
    'this function is used to Load checkbox allow Update Info or not
    'in Multiple Edit case, checkbox is checked
    Shared Sub LoadChkUpdate(ByRef tpl As String, ByVal sportId As String, ByVal isMult As Boolean)

        Dim strChk As String
        Dim strChkPt As String
        If isMult Then
            Dim chkTpl As String = String.Empty
            chkTpl = TemplateHandler.LoadTemplate("MemberInfo/CheckboxTpl.html")
            strChk = chkTpl
            strChkPt = chkTpl
            TemplateHandler.Assign(strChk, "chk-Id", "chkUpdInfo" & sportId)
            TemplateHandler.Assign(strChk, "is-bet", "true")

            TemplateHandler.Assign(strChkPt, "chk-Id", "chkUpdPT" & sportId)
            TemplateHandler.Assign(strChkPt, "is-bet", "false")
        End If
        TemplateHandler.Assign(tpl, "bet-text", strChk & Resources.Lang.lblBetSetting)
        TemplateHandler.Assign(tpl, "pt-text", strChkPt & Resources.Lang.positiontaking)

    End Sub
#End Region

#Region "GetMaxPerBallAddValue"
    Shared Sub GetMaxPerBallAddValue(ByRef tpl As String, ByVal strReplace As String)
        TemplateHandler.Assign(tpl, "MaxPerBall-val", strReplace)
    End Sub
#End Region

#Region "GetMaxPerBallEditValue"
    'this function is used to assign value for Min Bet, Max Bet, Max Per Match in Single Editing case
    Sub GetMaxPerBallEditValue(ByRef tpl As String, ByVal sportTypeId As String, ByVal dr As DataRow)
        TemplateHandler.Assign(tpl, "MaxPerBall-val", DataFormat.FormatNumber1(dr("maxperballv" & sportTypeId)))
    End Sub
#End Region

#Region "GetLimitMaxPerBall"
    'this function is used to display or not limit values of Min Bet, Max Bet, Max Per Match 
    'dr: row contains data get from db
    'tpl: template for each sporttype
    'sportTypeId: id of sporttype (same to Sport table in db)
    Shared Sub GetLimitMaxPerBall(ByVal dr As DataRow, ByRef tpl As String, ByVal sportTypeId As String)

        Dim maxperballMax As Double

        maxperballMax = DataFormat.FormatNumber1(dr("MaxPerBallv" & sportTypeId & "_Max"))

        TemplateHandler.Assign(tpl, "MaxPerBall-max", DataFormat.FormatNumber1(maxperballMax))

        If maxperballMax = 0 Then
            TemplateHandler.Assign(tpl, "maxperball_clss", "hidden")
        Else
            TemplateHandler.Assign(tpl, "maxperball_clss", "minmaxbet")
        End If


    End Sub
#End Region

#Region "GetMinMaxAddValue"
    'this function is used to assign value for Min Bet, Max Bet, Max Per Match in Adding, Multiple Editing case
    Shared Sub GetMinMaxAddValue(ByRef tpl As String, ByVal sportTypeId As Integer, ByVal strReplace As String)
        TemplateHandler.Assign(tpl, "minbet" & sportTypeId & "-val", strReplace)
        TemplateHandler.Assign(tpl, "maxbet" & sportTypeId & "-val", strReplace)
        TemplateHandler.Assign(tpl, "maxpermatch" & sportTypeId & "-val", strReplace)
        If sportTypeId = "161" Then
            TemplateHandler.Assign(tpl, "MaxPerBall-val", strReplace)
        End If

    End Sub
#End Region

#Region "GetMinMaxEditValue"
    'this function is used to assign value for Min Bet, Max Bet, Max Per Match in Single Editing case
    Sub GetMinMaxEditValue(ByRef tpl As String, ByVal sportTypeId As String, ByVal dr As DataRow)
        Dim hiddenMinMaxBet As New Hashtable
        TemplateHandler.Assign(tpl, "minbet" & sportTypeId & "-val", DataFormat.FormatNumber1(dr("minbetv" & sportTypeId)))
        TemplateHandler.Assign(tpl, "maxbet" & sportTypeId & "-val", DataFormat.FormatNumber1(dr("maxbetv" & sportTypeId)))
        TemplateHandler.Assign(tpl, "maxpermatch" & sportTypeId & "-val", DataFormat.FormatNumber1(dr("maxpermatchv" & sportTypeId)))
    End Sub
#End Region

#Region "GetLimitMinMaxBet"
    'this function is used to display or not limit values of Min Bet, Max Bet, Max Per Match 
    'dr: row contains data get from db
    'tpl: template for each sporttype
    'sportTypeId: id of sporttype (same to Sport table in db)
    Shared Function GetLimitMinMaxBet(ByVal dr As DataRow, ByRef tpl As String, ByVal sportTypeId As String)

        Dim minbetMin As Double
        Dim maxbetMax As Double
        Dim maxpermatchMin As Double
        Dim maxpermatchMax As Double
        Dim maxperballMax As Double

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
        TemplateHandler.Assign(tpl, "minbet-min-" & sportTypeId & "", DataFormat.FormatNumber1(minbetMin))
        TemplateHandler.Assign(tpl, "maxbet-max-" & sportTypeId & "", DataFormat.FormatNumber1(maxbetMax))
        TemplateHandler.Assign(tpl, "maxpermatch-min-" & sportTypeId & "", DataFormat.FormatNumber1(maxpermatchMin))
        TemplateHandler.Assign(tpl, "maxpermatch-max-" & sportTypeId & "", DataFormat.FormatNumber1(maxpermatchMax))

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

    End Function
#End Region


End Class

'Programmer : Hanh

Imports System.Resources
Imports DNAide.Web
Imports System.Text
Imports DNAide.Core
Imports Fanex.Data.DNAide
Imports DNAide.Utils
Imports System.Data
Imports System.Collections
Imports Sunplus.Agent.Model.Entities.Permission
Public Class EditAgent
    Inherits TplPage

    Public style As String

    Dim bodyTpl, tennisTpl, basketballTpl, footballTpl, otherSportsTpl, golfTpl As String
    Dim strHidSoccer, strHidBasket, strHidFootball, strHidTennis As String
    Dim strHidBaseball, strHidGolf, strHidMoto, strHidOther, strHidNumberGame, strHidMixSports As String
    Dim groupBtn As String
    Protected disabledBtn As String
    Dim selectTpl, selDisabled As String

    Public script As String = String.Empty
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        Me.CssGroup = "__"

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
        selDisabled = TemplateHandler.LoadTemplate("MemberInfo/DdlDisablePT.html")

        If (String.IsNullOrEmpty(Request.QueryString("nobackground"))) Then
            'TemplateHandler.RemoveBlock(mainContent, "nobackground")
            lnkNobackground.Visible = False
        Else
            'If is from Account Menu, Then Set delay reload page
            Dim hash As Hashtable = AppSession.RequestQueryString
            If (hash Is Nothing) Then
                hash = New Hashtable
            End If
            If (Not hash.ContainsKey("FromAccount")) Then
                hash.Add("FromAccount", "../_MemberInfo/CustomerList/Master/AgentList.aspx")
            Else
                hash("FromAccount") = "../_MemberInfo/CustomerList/Master/AgentList.aspx"
            End If
            AppSession.RequestQueryString = hash
        End If

        If Request.QueryString("m") = "0" Then
            saveCustid.Value = Request.QueryString("custid").Replace("^", "")
            saveUsername.Value = Request.QueryString("USER").Replace("^", "")
            htmlEditAgentContent.Text = LoadDataSingle()
        Else
            isMult.Value = "1"
            htmlEditAgentContent.Text = LoadDataMult()
        End If

    End Sub

#Region "Declare parameter for Script"
    'declare parameters is used in client script 
    'this function is call in code inline
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
        script += " var alertUserName='" & Resources.Lang.alertUserName & "';" & vbNewLine
        script += " var lblPasswordRequire='" & Resources.Lang.lblPasswordRequire & "';" & vbNewLine
        script += " var alertPasswordEmpty='" & Resources.Lang.alertPasswordEmpty & "';" & vbNewLine
        script += " var alertIncorrectCredit='" & Resources.Lang.alertAgentCredit & "';" & vbNewLine
        script += " var alertIncorrectMemberMaxCredit='" & Resources.Lang.alertMemberCredit & "';" & vbNewLine
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
        lblCreditInvalid = Resources.Lang.lblCreditInvalid.Replace("{0}", Resources.Lang.lblAgentMaxCredit)

        Dim lblMemberCreditInvalid As String
        lblMemberCreditInvalid = Resources.Lang.lblCreditInvalid.Replace("{0}", Resources.Lang.lblMemberMaxCredit)

        script += " var alertCreditInvalid='" & lblCreditInvalid & "';" & vbNewLine
        script += " var alertMemberCreditInvalid='" & lblMemberCreditInvalid & "';" & vbNewLine
        Dim strAlertMinPT As String = Resources.Lang.alertLessthanMinPT
        strAlertMinPT = strAlertMinPT.Replace("{1}", Resources.Lang.lblMasterPresetPosition)
        strAlertMinPT = strAlertMinPT.Replace("{2}", Resources.Lang.lblSuperPresetPosition)
        script += " var alertLessthanMinPT='" & strAlertMinPT & "';" & vbNewLine
        script += "</script>"
        Response.Write(script)
    End Sub
#End Region

#Region "Load data for single edit"
    Function LoadDataSingle() As String
        Dim html As New StringBuilder

        Dim hash As New Hashtable()
        hash.Add("custid", saveCustid.Value)

        Dim custid As String = String.Empty
        If (Not Request("masterid") Is Nothing And AppSettings.IsSuperSite) Then
            custid = Request("masterid")
        Else
            custid = Sunplus.Agent.Model.Entities.UserSession.Current.CustId
        End If

        hash.Add("mrecommend", custid)
        Dim rs As DbResult
        rs = AccountInfo.Acc_CustomerSelMEditCustomer(hash)
        If Not rs Is Nothing Then
            If rs.ResultSet.Tables(0).Rows.Count > 0 Then
                Dim row As DataRow = rs.ResultSet.Tables(0).Rows(0)
                'If CastAide.AsBoolean(row("AllowUpdatePositiontaking")) Then
                '    isAllowUpd.Value = "1"
                'Else
                '    isAllowUpd.Value = "0"
                '    disabledBtn = "<script type=""text/javascript"">$(""btnSubmit"").style.color='#cccccc';</script>"
                'End If
                html.Append(GetDataInfo(row))
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

                If UserPermission.IsShowNumberGame Then
                    TemplateHandler.Assign(bodyTpl, "numbergamePT", GetDataNumberGame(row, False))
                Else
                    TemplateHandler.Assign(bodyTpl, "numbergamePT", "")
                End If

                If UserPermission.IsShowMixedSport Then
                    TemplateHandler.Assign(bodyTpl, "mixSportPT", GetDataMixedSports(row, False))
                End If

                '
                ' Casino
                TemplateHandler.Assign(bodyTpl, "hidSynCS", row("synccasino"))

                If False Or CastAide.AsBoolean(row("disableautompt"), False) Then
                    TemplateHandler.Assign(bodyTpl, "hdauto", "hidden")
                Else
                    TemplateHandler.Assign(bodyTpl, "hdauto", String.Empty)
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

                html.Append(bodyTpl)
            End If
        End If


        LoadDataSingle = html.ToString()
    End Function
#End Region

#Region "Load Data for Multiple Edit"
    Function LoadDataMult() As String
        Dim html As New StringBuilder

        Dim rs As DbResult
        Dim hash As New Hashtable
        hash.Add("custid", Sunplus.Agent.Model.Entities.UserSession.Current.CustId)
        rs = AccountInfo.Acc_CustomerSelMAgentInfo(hash)
        If Not rs Is Nothing Then
            If rs.ResultSet.Tables(0).Rows.Count > 0 Then
                Dim row As DataRow = rs.ResultSet.Tables(0).Rows(0)
                'If CastAide.AsBoolean(row("AllowUpdatePositiontaking")) Then
                '    isAllowUpd.Value = "1"
                'Else
                '    isAllowUpd.Value = "0"
                '    disabledBtn = "<script type=""text/javascript"">$(""btnSubmit"").style.color='#cccccc';</script>"
                'End If

                html.Append(GetDataInfo(row, True))
                Common.GetSportTitle(bodyTpl, False)
                'TemplateHandler.RemoveBlock(bodyTpl, "none-Mult")
                'TemplateHandler.RemoveBlock(bodyTpl, "none-Mult")
                'TemplateHandler.Assign(bodyTpl, "disable_chkall", "disabled")
                'TemplateHandler.Assign(bodyTpl, "disable_chkbasket", "disabled")

                TemplateHandler.Assign(bodyTpl, "add-text", Resources.Lang.submit)
                TemplateHandler.Assign(bodyTpl, "add-id", "btnSubmit")
                TemplateHandler.Assign(bodyTpl, "soccerPT", GetDataSoccer(row, True))
                TemplateHandler.Assign(bodyTpl, "basketPT", GetDataBasketball(row, 2, basketballTpl, True))
                TemplateHandler.Assign(bodyTpl, "footballPT", GetData4DDl(row, 3, footballTpl, True))
                TemplateHandler.Assign(bodyTpl, "tennisPT", GetData4DDl(row, 5, tennisTpl, True))
                TemplateHandler.Assign(bodyTpl, "baseballPT", GetDataBaseball(row, 8, True))
                TemplateHandler.Assign(bodyTpl, "golfPT", GetData4DDl(row, 10, golfTpl, True))
                TemplateHandler.Assign(bodyTpl, "motoPT", GetDataMotoSports(row, 11, True))
                TemplateHandler.Assign(bodyTpl, "otherPT", GetData4DDl(row, 99, otherSportsTpl, True))
                If UserPermission.IsShowNumberGame Then
                    TemplateHandler.Assign(bodyTpl, "numbergamePT", GetDataNumberGame(row, True))
                Else
                    TemplateHandler.Assign(bodyTpl, "numbergamePT", "")
                End If

                If UserPermission.IsShowMixedSport Then
                    TemplateHandler.Assign(bodyTpl, "mixSportPT", GetDataMixedSports(row, True))
                End If


                Dim htbDisableAutoMpt As New Hashtable

                If False Or CastAide.AsBoolean(row("disableautompt"), False) Then
                    TemplateHandler.Assign(bodyTpl, "hdauto", "hidden")
                    htbDisableAutoMpt.Add("isDisableAutoMpt", "1")
                Else
                    TemplateHandler.Assign(bodyTpl, "hdauto", String.Empty)
                    htbDisableAutoMpt.Add("isDisableAutoMpt", "0")
                End If
                TemplateHandler.RemoveBlock(bodyTpl, "none-edit-Min")
                bodyTpl &= PositionTaking.AssignMulipleHidden(htbDisableAutoMpt)

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


        LoadDataMult = html.ToString()
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

        strAccInfo = Common.ReplaceWithPopupStyle(strAccInfo)

        TemplateHandler.RemoveBlock(strAccInfo, "none-member")
        TemplateHandler.RemoveBlock(strAccInfo, "none-member")
        TemplateHandler.RemoveBlock(strAccInfo, "none-member")
        TemplateHandler.RemoveBlock(strAccInfo, "none-edit-member")
        TemplateHandler.Assign(strAccInfo, "tranfer-time", "")
        TemplateHandler.RemoveBlock(strAccInfo, "none-HRdisable")
        TemplateHandler.Assign(strAccInfo, "add-text", Resources.Lang.submit)
        TemplateHandler.Assign(strAccInfo, "add-id", "btnSubmit")
        If isMult Then
            TemplateHandler.RemoveBlock(strAccInfo, "none-mult")
            TemplateHandler.Assign(strAccInfo, "cols-untext", "5")
            TemplateHandler.Assign(strAccInfo, "title", Resources.Lang.lblEditMultiple)
            TemplateHandler.Assign(strAccInfo, "groupa-val", GetListGroup(CastAide.AsDouble(dr("groupa")), CastAide.AsDouble(dr("groupa"))))
            TemplateHandler.Assign(strAccInfo, "groupb-val", GetListGroup(CastAide.AsDouble(dr("groupb")), CastAide.AsDouble(dr("groupb"))))
            TemplateHandler.Assign(strAccInfo, "groupc-val", GetListGroup(CastAide.AsDouble(dr("groupc")), CastAide.AsDouble(dr("groupc"))))
            TemplateHandler.Assign(strAccInfo, "groupd-val", GetListGroup(CastAide.AsDouble(dr("groupd")), CastAide.AsDouble(dr("groupd"))))
            TemplateHandler.Assign(strAccInfo, "discount1x2-val", GetListOtherComm(CastAide.AsDouble(dr("discount1x2")), CastAide.AsDouble(dr("discount1x2"))))
            TemplateHandler.Assign(strAccInfo, "discountcs-val", GetListOtherComm(CastAide.AsDouble(dr("discountcs")), CastAide.AsDouble(dr("discountcs"))))

            If UserPermission.IsShowNumberGame Then
                TemplateHandler.Assign(strAccInfo, "disnumbergame-val", GetListOtherComm(CastAide.AsDouble(dr("discountnumber")), CastAide.AsDouble(dr("discountnumber"))))
            Else
                TemplateHandler.RemoveBlock(strAccInfo, "none-text-numbergame")
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

            TemplateHandler.Assign(strAccInfo, "IsSuspended", 0)
            TemplateHandler.Assign(strAccInfo, "UplineSuspended", 0)
        Else
            TemplateHandler.RemoveBlock(strAccInfo, "none-edit")
            TemplateHandler.RemoveBlock(strAccInfo, "none-group")
            TemplateHandler.RemoveBlock(strAccInfo, "none-group")
            TemplateHandler.RemoveBlock(strAccInfo, "none-single")
            TemplateHandler.RemoveBlock(strAccInfo, "none-single")
            TemplateHandler.RemoveBlock(strAccInfo, "none-single")
            TemplateHandler.RemoveBlock(strAccInfo, "none-super-master")

            'Terry Nguyen: Modify disable Suspend
            Dim htbDis As Hashtable = New Hashtable()

            htbDis.Add("Suspend", True)
            Dim mytemplate As String = TemplateHandler.LoadTemplate("Memberinfo/DisableStatus.html")
            Dim mycolspan As String = "<td colspan='2'><td>"
            TemplateHandler.RemoveBlock(mytemplate, "Captchar")
            TemplateHandler.ReplaceBlock(strAccInfo, "Status-Disable", StatusSetting.StatusDisable(mycolspan, htbDis, mytemplate))

            'TemplateHandler.Assign(strAccInfo, "hddisableFI-value", CastAide.AsInt32(dr("DisableFinancial"), 0))
            'TemplateHandler.Assign(strAccInfo, "hddisableFIDownline-value", CastAide.AsInt32(dr("MeDisableFinancial"), 0))
            '--
            Dim strClose As String = TemplateHandler.CopyBlock(strAccInfo, "Close")
            TemplateHandler.RemoveBlock(strAccInfo, "none-add")
            TemplateHandler.ReplaceBlock(strAccInfo, "Add-Close", strClose)

            TemplateHandler.Assign(strAccInfo, "mMinCredit", "minmaxbet")
            TemplateHandler.Assign(strAccInfo, "mMaxCredit", "minmaxbet")
            TemplateHandler.Assign(strAccInfo, "member-max-credit", Resources.Lang.lblMemberMaxCredit)
            TemplateHandler.Assign(strAccInfo, "member-credit-min", DataFormat.FormatNumber1(dr("MaxCredit_Min")))
            TemplateHandler.Assign(strAccInfo, "member-credit-max", DataFormat.FormatNumber1(dr("credit")))
            TemplateHandler.Assign(strAccInfo, "MemberMaxCredit-val", DataFormat.FormatNumber1(dr("Mem_MaxCredit")))

            TemplateHandler.Assign(strAccInfo, "cols-untext", "4")
            TemplateHandler.Assign(strAccInfo, "title", Resources.Lang.edit + " " + saveUsername.Value)
            TemplateHandler.Assign(strAccInfo, "max-credit-text", Resources.Lang.lblAgentMaxCredit)
            TemplateHandler.Assign(strAccInfo, "uname-parent", Sunplus.Agent.Model.Entities.UserSession.Current.UserName)

            TemplateHandler.Assign(strAccInfo, "fname-val", PositionTaking.ReplaceData(CastAide.AsString(dr("firstname"))))
            TemplateHandler.Assign(strAccInfo, "lname-val", PositionTaking.ReplaceData(CastAide.AsString(dr("lastname"))))
            TemplateHandler.Assign(strAccInfo, "phone-val", PositionTaking.ReplaceData(CastAide.AsString(dr("phone"))))
            TemplateHandler.Assign(strAccInfo, "mobile-val", PositionTaking.ReplaceData(CastAide.AsString(dr("mobilephone"))))
            TemplateHandler.Assign(strAccInfo, "fax-val", PositionTaking.ReplaceData(CastAide.AsString(dr("fax"))))
            'display value, limits of credit
            GetCredit(dr, strAccInfo, True)

            TemplateHandler.Assign(strAccInfo, "suspend-text", Resources.Lang.suspended)
            TemplateHandler.Assign(strAccInfo, "IsSuspended", CastAide.AsInt32(dr("suspended"), 0))

            Dim isClosed As Boolean = CastAide.AsBoolean(dr("closed"))
            If isClosed Then
                TemplateHandler.Assign(strAccInfo, "check-val", "checked")
            Else
                TemplateHandler.Assign(strAccInfo, "check-val", "")
            End If
            TemplateHandler.Assign(strAccInfo, "IsClosed", CastAide.AsInt32(dr("closed"), 0))

            Dim status As String = ""

            If (CastAide.AsBoolean(dr("suspended"), False)) Then
                status = "checked='checked' "
            End If

            If (CastAide.AsBoolean(dr("Msuspended"), False)) Then
                status = status + "disabled='disabled'"
            End If

            TemplateHandler.Assign(strAccInfo, "suspend-check", status)
            TemplateHandler.Assign(strAccInfo, "IsSuspended", CastAide.AsInt32(dr("suspended"), 0))
            TemplateHandler.Assign(strAccInfo, "UplineSuspended", CastAide.AsInt32(dr("Msuspended"), 0))

            TemplateHandler.Assign(strAccInfo, "groupa-val", GetListGroup(CastAide.AsDouble(dr("groupa_max")), CastAide.AsDouble(dr("groupa"))))
            TemplateHandler.Assign(strAccInfo, "groupb-val", GetListGroup(CastAide.AsDouble(dr("groupb_max")), CastAide.AsDouble(dr("groupb"))))
            TemplateHandler.Assign(strAccInfo, "groupc-val", GetListGroup(CastAide.AsDouble(dr("groupc_max")), CastAide.AsDouble(dr("groupc"))))
            TemplateHandler.Assign(strAccInfo, "groupd-val", GetListGroup(CastAide.AsDouble(dr("groupd_max")), CastAide.AsDouble(dr("groupd"))))
            TemplateHandler.Assign(strAccInfo, "discount1x2-val", GetListOtherComm(CastAide.AsDouble(dr("discount1x2_max")), CastAide.AsDouble(dr("discount1x2"))))
            TemplateHandler.Assign(strAccInfo, "discountcs-val", GetListOtherComm(CastAide.AsDouble(dr("discountcs_max")), CastAide.AsDouble(dr("discountcs"))))

            If UserPermission.IsShowNumberGame Then
                TemplateHandler.Assign(strAccInfo, "disnumbergame-val", GetListOtherComm(CastAide.AsDouble(dr("discountnumber_max")), CastAide.AsDouble(dr("discountnumber"))))
                TemplateHandler.Assign(strAccInfo, "hideClss", String.Empty)
            Else
                TemplateHandler.RemoveBlock(strAccInfo, "none-text-numbergame")
                TemplateHandler.Assign(strAccInfo, "hideClss", "hidden")
            End If


            TemplateHandler.Assign(strAccInfo, "sel-a", "")
            TemplateHandler.Assign(strAccInfo, "sel-b", "")
            TemplateHandler.Assign(strAccInfo, "sel-c", "")
            TemplateHandler.Assign(strAccInfo, "sel-d", "")
            TemplateHandler.Assign(strAccInfo, "sel-dis", "")
            TemplateHandler.Assign(strAccInfo, "sel-disFC", "")
            TemplateHandler.Assign(strAccInfo, "sel-disnumbergame", "")
            'sel-disnumbergame
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
        LoadChkUpdate(soccerTpl, 1, isMult)
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
            'TemplateHandler.RemoveBlock(soccerTpl, "auto-preset")
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
        'assign value of Minbet, Maxbet , ...
        If isMult Then
            GetMinMaxAddValue(soccerTpl, 1, "0"" disabled='true'")
            TemplateHandler.Assign(soccerTpl, "chk-hdpd", "disabled")
            TemplateHandler.Assign(soccerTpl, "chk-1x2", "disabled")
            TemplateHandler.Assign(soccerTpl, "chk-hdpl", "disabled")
        Else
            TemplateHandler.Assign(soccerTpl, "minbet1-val", DataFormat.FormatNumber1(dr("minbet")))
            TemplateHandler.Assign(soccerTpl, "maxbet1-val", DataFormat.FormatNumber1(dr("maxbet")))
            TemplateHandler.Assign(soccerTpl, "maxpermatch1-val", DataFormat.FormatNumber1(dr("maxpermatch")))
            TemplateHandler.Assign(soccerTpl, "chk-hdpd", "")
            TemplateHandler.Assign(soccerTpl, "chk-1x2", "")
            TemplateHandler.Assign(soccerTpl, "chk-hdpl", "")
        End If
        GetLimitMinMaxBet(dr, soccerTpl, "")

        'assign value position taking for template
        Dim mHdpMin, mOUMin, m1stHdpMin, m1stOUMin, mOEMin, m1x2Min, mCSMin, mTGMin, mMPMin, mORMin As Double
        Dim mLHdpMin, mLOUMin, mL1stHdpMin, mL1stOUMin As Double

        Dim maHdpMin, maOUMin, ma1stHdpMin, ma1stOUMin, maOEMin, ma1x2Min, maCSMin, maTGMin, maMPMin, maORMin As Double
        Dim maLHdpMin, maLOUMin, maL1stHdpMin, maL1stOUMin As Double

        Dim maHdpMax, maOUMax, ma1stHdpMax, ma1stOUMax, maOEMax, ma1x2Max, maCSMax, maTGMax, maMPMax, maORMax As Double
        Dim maLHdpMax, maLOUMax, maL1stHdpMax, maL1stOUMax As Double

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

        '------------------------Auto PT ------------------------- Tho Add -------------------------------------
        Dim isAutoHdp, isAutoOU, isAuto1stHdp, isAuto1stOU, isAutoOE, isAuto1x2, isAutoCS, isAutoTG, isAutoMP, isAutoOR As Double
        Dim isAutoLHdp, isAutoLOU, isAutoL1stHdp, isAutoL1stOU As Double

        Dim mAutoHdp, mAutoOU, mAuto1stHdp, mAuto1stOU, mAutoOE, mAuto1x2, mAutoCS, mAutoTG, mAutoMP, mAutoOR As Double
        Dim mAutoLHdp, mAutoLOU, mAutoL1stHdp, mAutoL1stOU As Double

        Dim mAutoHdpMax, mAutoOUMax, mAuto1stHdpMax, mAuto1stOUMax, mAutoOEMax, mAuto1x2Max, mAutoCSMax, mAutoTGMax, mAutoMPMax, mAutoORMax As Double
        Dim mAutoLHdpMax, mAutoLOUMax, mAutoL1stHdpMax, mAutoL1stOUMax As Double
        '-------------------------------------------------------------------------------------------------------

        aHdpMax2 = CastAide.AsDouble(dr("apositiontaking_1_max")) 'min(a4,l2)
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


        If isMult Then ' this case for Multiple Edit
            maHdpMax = CastAide.AsDouble(dr("mpositiontaking_1"))
            maOUMax = CastAide.AsDouble(dr("mpositiontaking_3"))
            ma1stHdpMax = CastAide.AsDouble(dr("mpositiontaking_7"))
            ma1stOUMax = CastAide.AsDouble(dr("mpositiontaking_8"))
            maOEMax = CastAide.AsDouble(dr("mpositiontaking_2"))
            ma1x2Max = CastAide.AsDouble(dr("mpositiontaking_5"))
            maCSMax = CastAide.AsDouble(dr("mpositiontaking_4"))
            maTGMax = CastAide.AsDouble(dr("mpositiontaking_6"))
            maMPMax = CastAide.AsDouble(dr("mpositiontaking_9"))
            maORMax = CastAide.AsDouble(dr("mpositiontaking_10"))
            maLHdpMax = CastAide.AsDouble(dr("mlivepositiontaking_1"))
            maLOUMax = CastAide.AsDouble(dr("mlivepositiontaking_3"))
            maL1stHdpMax = CastAide.AsDouble(dr("mlivepositiontaking_7"))
            maL1stOUMax = CastAide.AsDouble(dr("mlivepositiontaking_8"))

            mHdp = maHdpMax
            mOU = maOUMax
            m1stHdp = ma1stHdpMax
            m1stOU = ma1stOUMax
            mOE = maOEMax
            m1x2 = ma1x2Max
            mCS = maCSMax
            mTG = maTGMax
            mMP = maMPMax
            mOR = maORMax
            mLHdp = maLHdpMax
            mLOU = maLOUMax
            mL1stHdp = maL1stHdpMax
            mL1stOU = maL1stOUMax

            mHdpMax = maHdpMax
            mOUMax = maOUMax
            m1stHdpMax = ma1stHdpMax
            m1stOUMax = ma1stOUMax
            mOEMax = maOEMax
            m1x2Max = ma1x2Max
            mCSMax = maCSMax
            mTGMax = maTGMax
            mMPMax = maMPMax
            mORMax = maORMax
            mLHdpMax = maLHdpMax
            mLOUMax = maLOUMax
            mL1stHdpMax = maL1stHdpMax
            mL1stOUMax = maL1stOUMax

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

            'Terry Nguyen modify: Auto position taking
            mAutoHdpMax = maHdpMax 'max u2
            mAutoOUMax = maOUMax
            mAuto1stHdpMax = m1stHdpMax
            mAuto1stOUMax = m1stOUMax
            mAutoOEMax = mOEMax
            mAuto1x2Max = m1x2Max
            mAutoCSMax = mCSMax
            mAutoTGMax = mTGMax
            mAutoMPMax = mMPMax
            mAutoORMax = mORMax
            mAutoLHdpMax = mLHdpMax
            mAutoLOUMax = mLOUMax
            mAutoL1stHdpMax = mL1stHdpMax
            mAutoL1stOUMax = mL1stOUMax
            '/---
        Else ' this case for Single Edit
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

            aHdpMin = CastAide.AsDouble(dr("apositiontaking_1_min"))
            aOUMin = CastAide.AsDouble(dr("apositiontaking_3_min"))
            a1stHdpMin = CastAide.AsDouble(dr("apositiontaking_7_min"))
            a1stOUMin = CastAide.AsDouble(dr("apositiontaking_8_min"))
            aOEMin = CastAide.AsDouble(dr("apositiontaking_2_min"))
            a1x2Min = CastAide.AsDouble(dr("apositiontaking_5_min"))
            aCSMin = CastAide.AsDouble(dr("apositiontaking_4_min"))
            aTGMin = CastAide.AsDouble(dr("apositiontaking_6_min"))
            aMPMin = CastAide.AsDouble(dr("apositiontaking_9_min"))
            aORMin = CastAide.AsDouble(dr("apositiontaking_10_min"))
            aLHdpMin = CastAide.AsDouble(dr("alivepositiontaking_1_min"))
            aLOUMin = CastAide.AsDouble(dr("alivepositiontaking_3_min"))
            aL1stHdpMin = CastAide.AsDouble(dr("alivepositiontaking_7_min"))
            aL1stOUMin = CastAide.AsDouble(dr("alivepositiontaking_8_min"))

            maHdpMax = CastAide.AsDouble(dr("max_mapositiontaking_1")) ' min(m3,l3)
            maOUMax = CastAide.AsDouble(dr("max_mapositiontaking_3"))
            ma1stHdpMax = CastAide.AsDouble(dr("max_mapositiontaking_7"))
            ma1stOUMax = CastAide.AsDouble(dr("max_mapositiontaking_8"))
            maOEMax = CastAide.AsDouble(dr("max_mapositiontaking_2"))
            ma1x2Max = CastAide.AsDouble(dr("max_mapositiontaking_5"))
            maCSMax = CastAide.AsDouble(dr("max_mapositiontaking_4"))
            maTGMax = CastAide.AsDouble(dr("max_mapositiontaking_6"))
            maMPMax = CastAide.AsDouble(dr("max_mapositiontaking_9"))
            maORMax = CastAide.AsDouble(dr("max_mapositiontaking_10"))
            maLHdpMax = CastAide.AsDouble(dr("max_malivepositiontaking_1"))
            maLOUMax = CastAide.AsDouble(dr("max_malivepositiontaking_3"))
            maL1stHdpMax = CastAide.AsDouble(dr("max_malivepositiontaking_7"))
            maL1stOUMax = CastAide.AsDouble(dr("max_malivepositiontaking_8"))

            maHdpMin = CastAide.AsDouble(dr("min_mapositiontaking_1"))
            maOUMin = CastAide.AsDouble(dr("min_mapositiontaking_3"))
            ma1stHdpMin = CastAide.AsDouble(dr("min_mapositiontaking_7"))
            ma1stOUMin = CastAide.AsDouble(dr("min_mapositiontaking_8"))
            maOEMin = CastAide.AsDouble(dr("min_mapositiontaking_2"))
            ma1x2Min = CastAide.AsDouble(dr("min_mapositiontaking_5"))
            maCSMin = CastAide.AsDouble(dr("min_mapositiontaking_4"))
            maTGMin = CastAide.AsDouble(dr("min_mapositiontaking_6"))
            maMPMin = CastAide.AsDouble(dr("min_mapositiontaking_9"))
            maORMin = CastAide.AsDouble(dr("min_mapositiontaking_10"))
            maLHdpMin = CastAide.AsDouble(dr("min_malivepositiontaking_1"))
            maLOUMin = CastAide.AsDouble(dr("min_malivepositiontaking_3"))
            maL1stHdpMin = CastAide.AsDouble(dr("min_malivepositiontaking_7"))
            maL1stOUMin = CastAide.AsDouble(dr("min_malivepositiontaking_8"))

            'Terry Nguyen modify: Auto position taking
            mAutoHdp = CastAide.AsDouble(dr("auto_mpositiontaking_1")) 'u2
            mAutoOU = CastAide.AsDouble(dr("auto_mpositiontaking_3"))
            mAuto1stHdp = CastAide.AsDouble(dr("auto_mpositiontaking_7"))
            mAuto1stOU = CastAide.AsDouble(dr("auto_mpositiontaking_8"))
            mAutoOE = CastAide.AsDouble(dr("auto_mpositiontaking_2"))
            mAuto1x2 = CastAide.AsDouble(dr("auto_mpositiontaking_5"))
            mAutoCS = CastAide.AsDouble(dr("auto_mpositiontaking_4"))
            mAutoTG = CastAide.AsDouble(dr("auto_mpositiontaking_6"))
            mAutoMP = CastAide.AsDouble(dr("auto_mpositiontaking_9"))
            mAutoOR = CastAide.AsDouble(dr("auto_mpositiontaking_10"))
            mAutoLHdp = CastAide.AsDouble(dr("auto_mlivepositiontaking_1"))
            mAutoLOU = CastAide.AsDouble(dr("auto_mlivepositiontaking_3"))
            mAutoL1stHdp = CastAide.AsDouble(dr("auto_mlivepositiontaking_7"))
            mAutoL1stOU = CastAide.AsDouble(dr("auto_mlivepositiontaking_8"))
            '/---

            '------------------------Auto PT ------------------------- Tho Add ---------------------------------------------
            isAutoHdp = CastAide.AsInt16(dr("isauto_mpositiontaking_1")) ' i2
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

            mAutoHdpMax = CastAide.AsDouble(dr("auto_mpositiontaking_1_max")) 'max u2
            mAutoOUMax = CastAide.AsDouble(dr("auto_mpositiontaking_3_max"))
            mAuto1stHdpMax = CastAide.AsDouble(dr("auto_mpositiontaking_7_max"))
            mAuto1stOUMax = CastAide.AsDouble(dr("auto_mpositiontaking_8_max"))
            mAutoOEMax = CastAide.AsDouble(dr("auto_mpositiontaking_2_max"))
            mAuto1x2Max = CastAide.AsDouble(dr("auto_mpositiontaking_5_max"))
            mAutoCSMax = CastAide.AsDouble(dr("auto_mpositiontaking_4_max"))
            mAutoTGMax = CastAide.AsDouble(dr("auto_mpositiontaking_6_max"))
            mAutoMPMax = CastAide.AsDouble(dr("auto_mpositiontaking_9_max"))
            mAutoORMax = CastAide.AsDouble(dr("auto_mpositiontaking_10_max"))
            mAutoLHdpMax = CastAide.AsDouble(dr("auto_mlivepositiontaking_1_max"))
            mAutoLOUMax = CastAide.AsDouble(dr("auto_mlivepositiontaking_3_max"))
            mAutoL1stHdpMax = CastAide.AsDouble(dr("auto_mlivepositiontaking_7_max"))
            mAutoL1stOUMax = CastAide.AsDouble(dr("auto_mlivepositiontaking_8_max"))

            '-------------------------------------------------------------------------------------------------------
            mHdpMax = maHdpMax - aHdpMin
            mOUMax = maOUMax - aOUMin
            m1stHdpMax = ma1stHdpMax - a1stHdpMin
            m1stOUMax = ma1stOUMax - a1stOUMin
            mOEMax = maOEMax - aOEMin
            m1x2Max = ma1x2Max - a1x2Min
            mCSMax = maCSMax - aCSMin
            mTGMax = maTGMax - aTGMin
            mMPMax = maMPMax - aMPMin
            mORMax = maORMax - aORMin
            mLHdpMax = maLHdpMax - aLHdpMin
            mLOUMax = maLOUMax - aLOUMin
            mL1stHdpMax = maL1stHdpMax - aL1stHdpMin
            mL1stOUMax = maL1stOUMax - aL1stOUMin
        End If
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        aHdpMax = IIf(maHdpMax - mHdp > aHdpMax2, aHdpMax2, maHdpMax - mHdp)
        aOUMax = IIf(maOUMax - mOU > aOUMax2, aOUMax2, maOUMax - mOU)
        a1stHdpMax = IIf(ma1stHdpMax - m1stHdp > a1stHdpMax2, a1stHdpMax2, ma1stHdpMax - m1stHdp)
        a1stOUMax = IIf(ma1stOUMax - m1stOU > a1stOUMax2, a1stOUMax2, ma1stOUMax - m1stOU)
        aOEMax = IIf(maOEMax - mOE > aOEMax2, aOEMax2, maOEMax - mOE)
        a1x2Max = IIf(ma1x2Max - m1x2 > a1x2Max2, a1x2Max2, ma1x2Max - m1x2)
        aCSMax = IIf(maCSMax - mCS > aCSMax2, aCSMax2, maCSMax - mCS)
        aTGMax = IIf(maTGMax - mTG > aTGMax2, aTGMax2, maTGMax - mTG)
        aMPMax = IIf(maMPMax - mMP > aMPMax2, aMPMax2, maMPMax - mMP)
        aORMax = IIf(maORMax - mOR > aORMax2, aORMax2, maORMax - mOR)
        aLHdpMax = IIf(maLHdpMax - mLHdp > aLHdpMax2, aLHdpMax2, maLHdpMax - mLHdp)
        aLOUMax = IIf(maLOUMax - mLOU > aLOUMax2, aLOUMax2, maLOUMax - mLOU)
        aL1stHdpMax = IIf(maL1stHdpMax - mL1stHdp > aL1stHdpMax2, aL1stHdpMax2, maL1stHdpMax - mL1stHdp)
        aL1stOUMax = IIf(maL1stOUMax - mL1stOU > aL1stOUMax2, aL1stOUMax2, maL1stOUMax - mL1stOU)

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

        hiddenHash.Add("maxA1HdpD", IIf(aHdpMax2 > maHdpMax, maHdpMax, aHdpMax2))
        hiddenHash.Add("maxA1OUD", IIf(aOUMax2 > maOUMax, maOUMax, aOUMax2))
        hiddenHash.Add("maxA11stHdpD", IIf(a1stHdpMax2 > ma1stHdpMax, ma1stHdpMax, a1stHdpMax2))
        hiddenHash.Add("maxA11stOUD", IIf(a1stOUMax2 > ma1stOUMax, ma1stOUMax, a1stOUMax2))
        hiddenHash.Add("maxA1OE", IIf(aOEMax2 > maOEMax, maOEMax, aOEMax2))

        hiddenHash.Add("maxA11x2", IIf(a1x2Max2 > ma1x2Max, ma1x2Max, a1x2Max2))
        hiddenHash.Add("maxA1CS", IIf(aCSMax2 > maCSMax, maCSMax, aCSMax2))
        hiddenHash.Add("maxA1TG", IIf(aTGMax2 > maTGMax, maTGMax, aTGMax2))
        hiddenHash.Add("maxA1MP", IIf(aMPMax2 > maMPMax, maMPMax, aMPMax2))
        hiddenHash.Add("maxA1OR", IIf(aORMax2 > maORMax, maORMax, aORMax2))

        hiddenHash.Add("maxA1HdpL", IIf(aLHdpMax2 > maLHdpMax, maLHdpMax, aLHdpMax2))
        hiddenHash.Add("maxA1OUL", IIf(aLOUMax2 > maLOUMax, maLOUMax, aLOUMax2))
        hiddenHash.Add("maxA11stHdpL", IIf(aL1stHdpMax2 > maL1stHdpMax, maL1stHdpMax, aL1stHdpMax2))
        hiddenHash.Add("maxA11stOUL", IIf(aL1stOUMax2 > maL1stOUMax, maL1stOUMax, aL1stOUMax2))

        hiddenHash.Add("maxM1HdpD", maHdpMax)
        hiddenHash.Add("maxM1OUD", maOUMax)
        hiddenHash.Add("maxM11stHdpD", ma1stHdpMax)
        hiddenHash.Add("maxM11stOUD", ma1stOUMax)
        hiddenHash.Add("maxM1OE", maOEMax)

        hiddenHash.Add("maxM11x2", ma1x2Max)
        hiddenHash.Add("maxM1CS", maCSMax)
        hiddenHash.Add("maxM1TG", maTGMax)
        hiddenHash.Add("maxM1MP", maMPMax)
        hiddenHash.Add("maxM1OR", maORMax)

        hiddenHash.Add("maxM1HdpL", maLHdpMax)
        hiddenHash.Add("maxM1OUL", maLOUMax)
        hiddenHash.Add("maxM11stHdpL", maL1stHdpMax)
        hiddenHash.Add("maxM11stOUL", maL1stOUMax)

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
        'Terry Nguyen modify: Auto position taking
        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS1HdpD")
        TemplateHandler.Assign(soccerTpl, "au-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS11x2")
        TemplateHandler.Assign(soccerTpl, "au-1x2-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS1HdpL")
        TemplateHandler.Assign(soccerTpl, "au-Lhdp-btn", btnTpl)
        '/--
        'get PT deadball value for master preset PT
        Dim pt As String
        Dim ptMaster As String
        Dim strChecked As String
        Dim ptAuto As String
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoHdp Then
            ptAuto = selectTpl
            pt = selectTpl
            ptMaster = selDisabled
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM1HdpD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aHdpMin, aHdpMax, aHdp))
        TemplateHandler.Assign(soccerTpl, "a-hdp-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS1HdpD")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
        TemplateHandler.Assign(soccerTpl, "p-hdp-val", ptMaster)

        'Terry Nguyen modify: Auto position taking

        TemplateHandler.Assign(soccerTpl, "checked-Hdp", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1HdpD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoHdpMax, mAutoHdp))
        TemplateHandler.Assign(soccerTpl, "au-hdp-val", ptAuto)
        '/--
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoOU Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM1OUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aOUMin, aOUMax, aOU))
        TemplateHandler.Assign(soccerTpl, "a-ou-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS1OUD")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOU))
        TemplateHandler.Assign(soccerTpl, "p-ou-val", ptMaster)

        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(soccerTpl, "checked-OU", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OUD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoOUMax, mAutoOU))
        TemplateHandler.Assign(soccerTpl, "au-ou-val", ptAuto)
        '/--
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAuto1stHdp Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM11stHdpD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(a1stHdpMin, a1stHdpMax, a1stHdp))
        TemplateHandler.Assign(soccerTpl, "a-1sthdp-val", pt)
        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS11stHdpD")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(m1stHdpMin, m1stHdpMax, m1stHdp))
        TemplateHandler.Assign(soccerTpl, "p-1sthdp-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(soccerTpl, "checked-1stHdp", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stHdpD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAuto1stHdpMax, mAuto1stHdp))
        TemplateHandler.Assign(soccerTpl, "au-1sthdp-val", ptAuto)
        '/--
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAuto1stOU Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM11stOUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(a1stOUMin, a1stOUMax, a1stOU))
        TemplateHandler.Assign(soccerTpl, "a-1stou-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS11stOUD")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(m1stOUMin, m1stOUMax, m1stOU))
        TemplateHandler.Assign(soccerTpl, "p-1stou-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(soccerTpl, "checked-1stOU", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stOUD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAuto1stOUMax, mAuto1stOU))
        TemplateHandler.Assign(soccerTpl, "au-1stou-val", ptAuto)
        '/--
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoOE Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM1OE")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aOEMin, aOEMax, aOE))
        TemplateHandler.Assign(soccerTpl, "a-oe-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS1OE")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOE))
        TemplateHandler.Assign(soccerTpl, "p-oe-val", ptMaster)

        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(soccerTpl, "checked-OE", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OE")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoOEMax, mAutoOE))
        TemplateHandler.Assign(soccerTpl, "au-oe-val", ptAuto)
        '/--
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAuto1x2 Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM11x2")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(a1x2Min, a1x2Max, a1x2))
        TemplateHandler.Assign(soccerTpl, "a-1x2-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS11x2")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(m1x2Min, m1x2Max, m1x2))
        TemplateHandler.Assign(soccerTpl, "p-1x2-val", ptMaster)

        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(soccerTpl, "checked-1x2", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11x2")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAuto1x2Max, mAuto1x2))
        TemplateHandler.Assign(soccerTpl, "au-1x2-val", ptAuto)
        '/---
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoCS Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM1CS")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aCSMin, aCSMax, aCS))
        TemplateHandler.Assign(soccerTpl, "a-cs-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS1CS")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mCSMin, mCSMax, mCS))
        TemplateHandler.Assign(soccerTpl, "p-cs-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(soccerTpl, "checked-CS", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1CS")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoCSMax, mAutoCS))
        TemplateHandler.Assign(soccerTpl, "au-cs-val", ptAuto)
        '/---
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoTG Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM1TG")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aTGMin, aTGMax, aTG))
        TemplateHandler.Assign(soccerTpl, "a-tg-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS1TG")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mTGMin, mTGMax, mTG))
        TemplateHandler.Assign(soccerTpl, "p-tg-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(soccerTpl, "checked-TG", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1TG")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoTGMax, mAutoTG))
        TemplateHandler.Assign(soccerTpl, "au-tg-val", ptAuto)
        '/---
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoMP Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM1MP")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aMPMin, aMPMax, aMP))
        TemplateHandler.Assign(soccerTpl, "a-mp-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS1MP")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mMPMin, mMPMax, mMP))
        TemplateHandler.Assign(soccerTpl, "p-mp-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(soccerTpl, "checked-MP", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1MP")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoMPMax, mAutoMP))
        TemplateHandler.Assign(soccerTpl, "au-mp-val", ptAuto)
        '/---
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoOR Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM1OR")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aORMin, aORMax, aOR))
        TemplateHandler.Assign(soccerTpl, "a-or-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS1OR")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mORMin, mORMax, mOR))
        TemplateHandler.Assign(soccerTpl, "p-or-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(soccerTpl, "checked-OR", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OR")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoORMax, mAutoOR))
        TemplateHandler.Assign(soccerTpl, "au-or-val", ptAuto)
        '/---
        'get PT value for super preset PT
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoLHdp Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM1HdpL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aLHdpMin, aLHdpMax, aLHdp))
        TemplateHandler.Assign(soccerTpl, "a-Lhdp-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS1HdpL")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mLHdpMin, mLHdpMax, mLHdp))
        TemplateHandler.Assign(soccerTpl, "p-Lhdp-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(soccerTpl, "checked-LHdp", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1HdpL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoLHdpMax, mAutoLHdp))
        TemplateHandler.Assign(soccerTpl, "au-Lhdp-val", ptAuto)
        '/---
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoLOU Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM1OUL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aLOUMin, aLOUMax, aLOU))
        TemplateHandler.Assign(soccerTpl, "a-Lou-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS1OUL")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mLOUMin, mLOUMax, mLOU))
        TemplateHandler.Assign(soccerTpl, "p-Lou-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(soccerTpl, "checked-LOU", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OUL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoLOUMax, mAutoLOU))
        TemplateHandler.Assign(soccerTpl, "au-Lou-val", ptAuto)
        '/---
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoL1stHdp Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM11stHdpL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aL1stHdpMin, aL1stHdpMax, aL1stHdp))
        TemplateHandler.Assign(soccerTpl, "a-L1stHdp-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS11stHdpL")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mL1stHdpMin, mL1stHdpMax, mL1stHdp))
        TemplateHandler.Assign(soccerTpl, "p-L1stHdp-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(soccerTpl, "checked-L1stHdp", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stHdpL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoL1stHdpMax, mAutoL1stHdp))
        TemplateHandler.Assign(soccerTpl, "au-L1stHdp-val", ptAuto)
        '/---
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoL1stOU Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM11stOUL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aL1stOUMin, aL1stOUMax, aL1stOU))
        TemplateHandler.Assign(soccerTpl, "a-L1stou-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS11stOUL")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mL1stOUMin, mL1stOUMax, mL1stOU))
        TemplateHandler.Assign(soccerTpl, "p-L1stou-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(soccerTpl, "checked-L1stOU", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stOUL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoL1stOUMax, mAutoL1stOU))
        TemplateHandler.Assign(soccerTpl, "au-L1stou-val", ptAuto)
        '/---


        GetDataSoccer = soccerTpl
    End Function
#End Region

#Region "Assign info for  Basketball, Football, Tennis, Golf, OtherSports"
    'this function is used to assign data into template for all sporttype that have 4 bettype
    '(1:Handicap, 3:Over/Under, 2:Odd/Even, 20:Money Line)
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (2: Basketball, 3:Football, 5:Tennis, 10:Golf, 99:OtherSports )
    Function GetDataBasketball(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String, Optional ByVal isMult As Boolean = False) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable

        'assign text for position taking
        LoadChkUpdate(tpl, sportTypeId, isMult)
        GetTitleCommon(tpl)
        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "oe-text", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(tpl, "mp-text", Resources.Lang.lblMixParlay)
        TemplateHandler.Assign(tpl, "title-col", "7")
        TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")
        TemplateHandler.RemoveBlock(tpl, "none-edit")
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
        Dim mHdpMax, mOUMax, mOEMax, mMLMax As Double
        Dim mHdp, mOU, mOE, mML As Double

        Dim aHdpMin, aOUMin, aOEMin, aMLMin As Double
        Dim aHdpMax, aOUMax, aOEMax, aMLMax As Double
        Dim aHdp, aOU, aOE, aML As Double
        Dim aHdpMax2, aOUMax2, aOEMax2, aMLMax2 As Double
        Dim maHdpMin, maOUMin, maOEMin, maMLMin As Double
        Dim maHdpMax, maOUMax, maOEMax, maMLMax As Double
        'Terry Nguyen modify: Auto position taking
        Dim mAutoHdp, mAutoOU, mAutoOE, mAutoML As Double
        Dim mAutoHdpMax, mAutoOUMax, mAutoOEMax, mAutoMLMax As Double
        Dim isAutoHdp, isAutoOU, isAutoOE, isAutoML As Double
        '/---
        aHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_max"))
        aOUMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3_max"))
        aOEMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2_max"))
        aMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_max"))



        If isMult Then
            maHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
            maOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
            maOEMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))
            maMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))
            mHdp = maHdpMax
            mOU = maOUMax
            mOE = maOEMax

            mML = maMLMax

            mHdpMax = maHdpMax
            mOUMax = maOUMax
            mOEMax = maOEMax

            mMLMax = maMLMax

            maHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_min"))
            maOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_min"))
            maOEMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2_min"))

            maMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_min"))

            aHdp = aHdpMax
            aOU = aOUMax
            aOE = aOEMax
            aML = aMLMax

            'Terry Nguyen modify: Auto position taking
            mAutoHdpMax = maHdpMax 'max u2
            mAutoOUMax = maOUMax
            mAutoOEMax = mOEMax

            mAutoMLMax = mMLMax
            '/---
        Else
            maHdpMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_1"))
            maOUMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_3"))
            maOEMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_2"))

            maMLMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_20"))

            mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
            mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
            mOE = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))

            mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

            aHdp = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
            aOU = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3"))
            aOE = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2"))

            aML = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

            aHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_min"))
            aOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3_min"))
            aOEMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2_min"))

            aMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_min"))

            maHdpMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_1"))
            maOUMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_3"))
            maOEMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_2"))

            maMLMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_20"))

            mHdpMax = maHdpMax - aHdpMin
            mOUMax = maOUMax - aOUMin
            mOEMax = maOEMax - aOEMin

            mMLMax = maMLMax - aMLMin
            'Terry Nguyen modify: Auto position taking
            mAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_1")) ' u2
            mAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_3"))
            mAutoOE = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_2"))

            mAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_20"))
            '/---

            'Terry Nguyen modify: Auto position taking
            isAutoHdp = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_1"))
            isAutoOU = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_3"))
            isAutoOE = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_2"))

            isAutoML = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_20"))

            mAutoHdpMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_1_max")) ' max u2
            mAutoOUMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_3_max"))
            mAutoOEMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_2_max"))

            mAutoMLMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_20_max"))

            '/---
        End If
        aHdpMax = IIf(maHdpMax - mHdp > aHdpMax2, aHdpMax2, maHdpMax - mHdp)
        aOUMax = IIf(maOUMax - mOU > aOUMax2, aOUMax2, maOUMax - mOU)
        aOEMax = IIf(maOEMax - mOE > aOEMax2, aOEMax2, maOEMax - mOE)

        aMLMax = IIf(maMLMax - mML > aMLMax2, aMLMax2, maMLMax - mML)

        hiddenHash.Add("minM" & sportTypeId & "Hdp", maHdpMin)
        hiddenHash.Add("minM" & sportTypeId & "OU", maOUMin)
        hiddenHash.Add("minM" & sportTypeId & "OE", maOEMin)

        hiddenHash.Add("minM" & sportTypeId & "ML", maMLMin)

        hiddenHash.Add("maxA" & sportTypeId & "Hdp", IIf(aHdpMax2 > maHdpMax, maHdpMax, aHdpMax2))
        hiddenHash.Add("maxA" & sportTypeId & "OU", IIf(aOUMax2 > maOUMax, maOUMax, aOUMax2))
        hiddenHash.Add("maxA" & sportTypeId & "OE", IIf(aOEMax2 > maOEMax, maOEMax, aOEMax2))

        hiddenHash.Add("maxA" & sportTypeId & "ML", IIf(aMLMax2 > maMLMax, maMLMax, aMLMax2))

        hiddenHash.Add("maxM" & sportTypeId & "Hdp", maHdpMax)
        hiddenHash.Add("maxM" & sportTypeId & "OU", maOUMax)
        hiddenHash.Add("maxM" & sportTypeId & "OE", maOEMax)

        hiddenHash.Add("maxM" & sportTypeId & "ML", maMLMax)

        hiddenHash.Add("isAuto" & sportTypeId & "Hdp", isAutoHdp)
        hiddenHash.Add("isAuto" & sportTypeId & "OU", isAutoOU)
        hiddenHash.Add("isAuto" & sportTypeId & "OE", isAutoOE)

        hiddenHash.Add("isAuto" & sportTypeId & "ML", isAutoML)

        strHidBasket = PositionTaking.AssignMulipleHidden(hiddenHash)

        'assign data for Position taking dropdowns
        pt = selectTpl
        Dim ptMaster As String
        Dim ptAuto As String
        Dim strChecked As String
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoHdp Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aHdpMin, aHdpMax, aHdp))
        TemplateHandler.Assign(tpl, "a-hdp-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
        TemplateHandler.Assign(tpl, "p-hdp-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(tpl, "checked-Hdp", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoHdpMax, mAutoHdp))
        TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
        '/---
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoOU Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aOUMin, aOUMax, aOU))
        TemplateHandler.Assign(tpl, "a-ou-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "OU")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOU))
        TemplateHandler.Assign(tpl, "p-ou-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(tpl, "checked-OU", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoOUMax, mAutoOU))
        TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)
        '/---
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoOE Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OE")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aOEMin, aOEMax, aOE))
        TemplateHandler.Assign(tpl, "a-oe-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "OE")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOE))
        TemplateHandler.Assign(tpl, "p-oe-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(tpl, "checked-OE", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OE")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoOEMax, mAutoOE))
        TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)
        '/---
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoML Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aMLMin, aMLMax, aML))
        TemplateHandler.Assign(tpl, "a-ml-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "ML")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mML))
        TemplateHandler.Assign(tpl, "p-ml-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(tpl, "checked-ML", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoMLMax, mAutoML))
        TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)

        '/---
        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)
        'Terry Nguyen modify: Auto position taking
        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)
        '/---

        GetDataBasketball = tpl
    End Function
#End Region

#Region "Assign info for Football, Tennis, Golf, OtherSports"
    'this function is used to assign data into template for all sporttype that have 4 bettype
    '(1:Handicap, 3:Over/Under, 2:Odd/Even, 9:Mix Parlay, 20:Money Line)
    'dr: row contains data get from db
    'sportTypeId: id of sporttype ( 3:Football, 5:Tennis, 10:Golf, 99:OtherSports )
    Function GetData4DDl(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String, Optional ByVal isMult As Boolean = False) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable

        'assign text for position taking
        LoadChkUpdate(tpl, sportTypeId, isMult)
        GetTitleCommon(tpl)
        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "oe-text", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(tpl, "title-col", "6")
        TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")
        TemplateHandler.RemoveBlock(tpl, "none-edit")
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
        Dim mHdpMax, mOUMax, mOEMax, mMLMax As Double
        Dim mHdp, mOU, mOE, mML As Double

        Dim aHdpMin, aOUMin, aOEMin, aMLMin As Double
        Dim aHdpMax, aOUMax, aOEMax, aMLMax As Double
        Dim aHdp, aOU, aOE, aML As Double
        Dim aHdpMax2, aOUMax2, aOEMax2, aMLMax2 As Double
        Dim maHdpMin, maOUMin, maOEMin, maMLMin As Double
        Dim maHdpMax, maOUMax, maOEMax, maMLMax As Double
        'Terry Nguyen modify: Auto position taking
        Dim mAutoHdp, mAutoOU, mAutoOE, mAutoML As Double
        Dim mAutoHdpMax, mAutoOUMax, mAutoOEMax, mAutoMLMax As Double
        Dim isAutoHdp, isAutoOU, isAutoOE, isAutoML As Double
        '/---
        aHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_max"))
        aOUMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3_max"))
        aOEMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2_max"))
        aMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_max"))


        If isMult Then
            maHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
            maOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
            maOEMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))
            maMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))
            mHdp = maHdpMax
            mOU = maOUMax
            mOE = maOEMax
            mML = maMLMax

            mHdpMax = maHdpMax
            mOUMax = maOUMax
            mOEMax = maOEMax
            mMLMax = maMLMax

            maHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_min"))
            maOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_min"))
            maOEMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2_min"))
            maMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_min"))

            aHdp = aHdpMax
            aOU = aOUMax
            aOE = aOEMax
            aML = aMLMax
            'Terry Nguyen modify: Auto position taking
            mAutoHdpMax = maHdpMax 'max u2
            mAutoOUMax = maOUMax
            mAutoOEMax = mOEMax
            mAutoMLMax = maMLMax
            '/---
        Else
            maHdpMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_1"))
            maOUMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_3"))
            maOEMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_2"))
            maMLMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_20"))

            mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
            mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
            mOE = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))
            mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

            aHdp = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
            aOU = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3"))
            aOE = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2"))
            aML = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

            aHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_min"))
            aOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3_min"))
            aOEMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2_min"))
            aMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_min"))

            maHdpMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_1"))
            maOUMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_3"))
            maOEMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_2"))
            maMLMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_20"))

            mHdpMax = maHdpMax - aHdpMin
            mOUMax = maOUMax - aOUMin
            mOEMax = maOEMax - aOEMin
            mMLMax = maMLMax - aMLMin

            'Terry Nguyen modify: Auto position taking
            mAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_1"))
            mAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_3"))
            mAutoOE = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_2"))
            mAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_20"))
            '/---

            'Terry Nguyen modify: Auto position taking
            isAutoHdp = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_1"))
            isAutoOU = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_3"))
            isAutoOE = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_2"))
            isAutoML = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_20"))

            mAutoHdpMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_1_max"))
            mAutoOUMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_3_max"))
            mAutoOEMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_2_max"))
            mAutoMLMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_20_max"))

            '/---
        End If
        aHdpMax = IIf(maHdpMax - mHdp > aHdpMax2, aHdpMax2, maHdpMax - mHdp)
        aOUMax = IIf(maOUMax - mOU > aOUMax2, aOUMax2, maOUMax - mOU)
        aOEMax = IIf(maOEMax - mOE > aOEMax2, aOEMax2, maOEMax - mOE)
        aMLMax = IIf(maMLMax - mML > aMLMax2, aMLMax2, maMLMax - mML)

        hiddenHash.Add("minM" & sportTypeId & "Hdp", maHdpMin)
        hiddenHash.Add("minM" & sportTypeId & "OU", maOUMin)
        hiddenHash.Add("minM" & sportTypeId & "OE", maOEMin)
        hiddenHash.Add("minM" & sportTypeId & "ML", maMLMin)

        hiddenHash.Add("maxA" & sportTypeId & "Hdp", IIf(aHdpMax2 > maHdpMax, maHdpMax, aHdpMax2))
        hiddenHash.Add("maxA" & sportTypeId & "OU", IIf(aOUMax2 > maOUMax, maOUMax, aOUMax2))
        hiddenHash.Add("maxA" & sportTypeId & "OE", IIf(aOEMax2 > maOEMax, maOEMax, aOEMax2))
        hiddenHash.Add("maxA" & sportTypeId & "ML", IIf(aMLMax2 > maMLMax, maMLMax, aMLMax2))

        hiddenHash.Add("maxM" & sportTypeId & "Hdp", maHdpMax)
        hiddenHash.Add("maxM" & sportTypeId & "OU", maOUMax)
        hiddenHash.Add("maxM" & sportTypeId & "OE", maOEMax)
        hiddenHash.Add("maxM" & sportTypeId & "ML", maMLMax)

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

        'assign data for Position taking dropdowns
        pt = selectTpl
        Dim ptMaster As String
        Dim ptAuto As String
        Dim strChecked As String
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoHdp Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aHdpMin, aHdpMax, aHdp))
        TemplateHandler.Assign(tpl, "a-hdp-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
        TemplateHandler.Assign(tpl, "p-hdp-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(tpl, "checked-Hdp", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoHdpMax, mAutoHdp))
        TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
        '/---
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoOU Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aOUMin, aOUMax, aOU))
        TemplateHandler.Assign(tpl, "a-ou-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "OU")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOU))
        TemplateHandler.Assign(tpl, "p-ou-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(tpl, "checked-OU", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoOUMax, mAutoOU))
        TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)
        '/---
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoOE Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OE")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aOEMin, aOEMax, aOE))
        TemplateHandler.Assign(tpl, "a-oe-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "OE")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOE))
        TemplateHandler.Assign(tpl, "p-oe-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(tpl, "checked-OE", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OE")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoOEMax, mAutoOE))
        TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)
        '/---
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoML Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aMLMin, aMLMax, aML))
        TemplateHandler.Assign(tpl, "a-ml-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "ML")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mML))
        TemplateHandler.Assign(tpl, "p-ml-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(tpl, "checked-ML", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoMLMax, mAutoML))
        TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)

        '/---
        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)
        'Terry Nguyen modify: Auto position taking
        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

        '/---

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
        LoadChkUpdate(tpl, sportTypeId, isMult)
        GetTitleCommon(tpl)
        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "title-col", "5")
        TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")
        TemplateHandler.RemoveBlock(tpl, "none-edit")
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
        Dim mHdpMax, mOUMax, mMLMax As Double
        Dim mHdp, mOU, mML As Double

        Dim aHdpMax, aOUMax, aMLMax As Double
        Dim aHdpMin, aOUMin, aMLMin As Double
        Dim aHdp, aOU, aML As Double
        Dim aHdpMax2, aOUMax2, aMLMax2 As Double
        Dim maHdpMin, maOUMin, maMLMin As Double
        Dim maHdpMax, maOUMax, maMLMax As Double
        'Terry Nguyen modify: Auto position taking
        Dim mAutoHdp, mAutoOU, mAutoML As Double
        Dim mAutoHdpMax, mAutoOUMax, mAutoMLMax As Double
        Dim isAutoHdp, isAutoOU, isAutoML As Double
        '/---
        aHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_max"))
        aOUMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3_max"))
        aMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_max"))

        If isMult Then
            maHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
            maOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
            maMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))
            mHdp = maHdpMax
            mOU = maOUMax
            mML = maMLMax
            mHdpMax = maHdpMax
            mOUMax = maOUMax
            mMLMax = maMLMax

            maHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_min"))
            maOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_min"))
            maMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_min"))

            aHdp = aHdpMax
            aOU = aOUMax
            aML = aMLMax

            'Terry Nguyen modify: Auto position taking
            mAutoHdpMax = maHdpMax 'max u2
            mAutoOUMax = maOUMax
            mAutoMLMax = maMLMax
            '/---
        Else
            mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
            mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
            mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

            aHdp = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
            aOU = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3"))
            aML = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

            aHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_min"))
            aOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3_min"))
            aMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_min"))

            maHdpMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_1"))
            maOUMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_3"))
            maMLMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_20"))

            maHdpMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_1"))
            maOUMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_3"))
            maMLMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_20"))

            mHdpMax = maHdpMax - aHdpMin
            mOUMax = maOUMax - aOUMin
            mMLMax = maMLMax - aMLMin

            'Terry Nguyen modify: Auto position taking
            mAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_1"))
            mAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_3"))
            mAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_20"))
            '/---

            'Terry Nguyen modify: Auto position taking
            isAutoHdp = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_1"))
            isAutoOU = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_3"))
            isAutoML = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_20"))

            mAutoHdpMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_1_max"))
            mAutoOUMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_3_max"))
            mAutoMLMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_20_max"))


            '/---
        End If
        aHdpMax = IIf(maHdpMax - mHdp > aHdpMax2, aHdpMax2, maHdpMax - mHdp)
        aOUMax = IIf(maOUMax - mOU > aOUMax2, aOUMax2, maOUMax - mOU)
        aMLMax = IIf(maMLMax - mML > aMLMax2, aMLMax2, maMLMax - mML)

        hiddenHash.Add("minM" & sportTypeId & "Hdp", maHdpMin)
        hiddenHash.Add("minM" & sportTypeId & "OU", maOUMin)
        hiddenHash.Add("minM" & sportTypeId & "ML", maMLMin)

        hiddenHash.Add("maxA" & sportTypeId & "Hdp", IIf(aHdpMax2 > maHdpMax, maHdpMax, aHdpMax2))
        hiddenHash.Add("maxA" & sportTypeId & "OU", IIf(aOUMax2 > maOUMax, maOUMax, aOUMax2))
        hiddenHash.Add("maxA" & sportTypeId & "ML", IIf(aMLMax2 > maMLMax, maMLMax, aMLMax2))

        hiddenHash.Add("maxM" & sportTypeId & "Hdp", maHdpMax)
        hiddenHash.Add("maxM" & sportTypeId & "OU", maOUMax)
        hiddenHash.Add("maxM" & sportTypeId & "ML", maMLMax)

        hiddenHash.Add("isAuto" & sportTypeId & "Hdp", isAutoHdp)
        hiddenHash.Add("isAuto" & sportTypeId & "OU", isAutoOU)
        hiddenHash.Add("isAuto" & sportTypeId & "ML", isAutoML)
        strHidBaseball = PositionTaking.AssignMulipleHidden(hiddenHash)

        'assign data for Position taking dropdowns
        Dim ptMaster As String
        Dim ptAuto As String
        Dim strChecked As String
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoHdp Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aHdpMin, aHdpMax, aHdp))
        TemplateHandler.Assign(tpl, "a-hdp-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
        TemplateHandler.Assign(tpl, "p-hdp-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(tpl, "checked-Hdp", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoHdpMax, mAutoHdp))
        TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)

        '/---
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoOU Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aOUMin, aOUMax, aOU))
        TemplateHandler.Assign(tpl, "a-ou-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "OU")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOU))
        TemplateHandler.Assign(tpl, "p-ou-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(tpl, "checked-OU", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoOUMax, mAutoOU))
        TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)
        '/---
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoML Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aMLMin, aMLMax, aML))
        TemplateHandler.Assign(tpl, "a-ml-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "ML")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mML))
        TemplateHandler.Assign(tpl, "p-ml-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(tpl, "checked-ML", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoMLMax, mAutoML))
        TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)
        '/---
        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)
        'Terry Nguyen modify: Auto position taking
        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)
        '/---

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
        LoadChkUpdate(tpl, sportTypeId, isMult)
        GetTitleCommon(tpl)
        TemplateHandler.Assign(tpl, "title-col", "4")
        TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")
        TemplateHandler.RemoveBlock(tpl, "none-edit")
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
        Dim mHdpMax, mMLMax As Double
        Dim mHdp, mML As Double

        Dim aHdpMax, aMLMax As Double
        Dim aHdpMin, aMLMin As Double
        Dim aHdp, aML As Double
        Dim aHdpMax2, aMLMax2 As Double
        Dim maHdpMin, maMLMin As Double
        Dim maHdpMax, maMLMax As Double
        'Terry Nguyen modify: Auto position taking
        Dim mAutoHdp, mAutoML As Double
        Dim mAutoHdpMax, mAutoMLMax As Double
        Dim isAutoHdp, isAutoML As Double
        '/---
        aHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_max"))
        aMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_max"))

        If isMult Then
            maHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
            maMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

            mHdp = maHdpMax
            mML = maMLMax
            mHdpMax = maHdpMax
            mMLMax = maMLMax

            maHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_min"))
            maMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_min"))

            aHdp = aHdpMax
            aML = aMLMax
            'Terry Nguyen modify: Auto position taking
            mAutoHdpMax = maHdpMax 'max u2
            mAutoMLMax = maMLMax
            '/---
        Else
            mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
            mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

            aHdp = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
            aML = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

            aHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_min"))
            aMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_min"))

            maHdpMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_1"))
            maMLMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_20"))

            maHdpMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_1"))
            maMLMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_20"))
            mHdpMax = maHdpMax - aHdpMin
            mMLMax = maMLMax - aMLMin

            'Terry Nguyen modify: Auto position taking
            mAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_1"))
            mAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_20"))
            '/---

            'Terry Nguyen modify: Auto position taking
            isAutoHdp = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_1"))
            isAutoML = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_20"))

            mAutoHdpMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_1_max"))
            mAutoMLMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_20_max"))

            '/---
        End If
        aHdpMax = IIf(maHdpMax - mHdp > aHdpMax2, aHdpMax2, maHdpMax - mHdp)
        aMLMax = IIf(maMLMax - mML > aMLMax2, aMLMax2, maMLMax - mML)

        hiddenHash.Add("minM" & sportTypeId & "Hdp", maHdpMin)
        hiddenHash.Add("minM" & sportTypeId & "ML", maMLMin)

        hiddenHash.Add("maxA" & sportTypeId & "Hdp", IIf(aHdpMax2 > maHdpMax, maHdpMax, aHdpMax2))
        hiddenHash.Add("maxA" & sportTypeId & "ML", IIf(aMLMax2 > maMLMax, maMLMax, aMLMax2))

        hiddenHash.Add("maxM" & sportTypeId & "Hdp", maHdpMax)
        hiddenHash.Add("maxM" & sportTypeId & "ML", maMLMax)

        hiddenHash.Add("isAuto" & sportTypeId & "Hdp", isAutoHdp)
        hiddenHash.Add("isAuto" & sportTypeId & "ML", isAutoML)

        strHidMoto = PositionTaking.AssignMulipleHidden(hiddenHash)

        'assign data for Position taking dropdowns
        Dim ptMaster As String
        Dim ptAuto As String
        Dim strChecked As String
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoHdp Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aHdpMin, aHdpMax, aHdp))
        TemplateHandler.Assign(tpl, "a-hdp-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
        TemplateHandler.Assign(tpl, "p-hdp-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(tpl, "checked-Hdp", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoHdpMax, mAutoHdp))
        TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
        '/---
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoML Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aMLMin, aMLMax, aML))
        TemplateHandler.Assign(tpl, "a-ml-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "ML")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mML))
        TemplateHandler.Assign(tpl, "p-ml-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(tpl, "checked-ML", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoMLMax, mAutoML))
        TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)
        '/---
        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)
        'Terry Nguyen modify: Auto position taking
        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

        '/---

        GetDataMotoSports = tpl
    End Function
#End Region

#Region "Assign Info for Number Game"

    Function GetDataNumberGame(ByVal dr As DataRow, Optional ByVal isMult As Boolean = False) As String
        Dim numbergameTpl As String = String.Empty
        Dim headerSTpl As String = String.Empty
        Dim hiddenHash As New Hashtable

        numbergameTpl = TemplateHandler.LoadTemplate("MemberInfo/NumberGamePT.html")

        GetTitleNumberGame(numbergameTpl, isMult)
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

        If UserPermission.IsShowNumberGame Then
            TemplateHandler.Assign(numbergameTpl, "hideClss", String.Empty)
        Else
            TemplateHandler.Assign(numbergameTpl, "hideClss", "hidden")
        End If


        ' assign all text according language
        TemplateHandler.RemoveBlock(numbergameTpl, "none-edit")
        LoadChkUpdate(numbergameTpl, 161, isMult)
        GetTitleCommon(numbergameTpl)
        TemplateHandler.Assign(numbergameTpl, "col-title", "12")
        TemplateHandler.Assign(numbergameTpl, "bettype-header", LoadNumberGameHeader())
        TemplateHandler.Assign(numbergameTpl, "deadball-text", Resources.Lang.lblDeadball)
        TemplateHandler.Assign(numbergameTpl, "live-text", Resources.Lang.lblLive)
        TemplateHandler.Assign(numbergameTpl, "a-preset-live", Resources.Lang.agentposition)
        TemplateHandler.Assign(numbergameTpl, "p-preset-live", Resources.Lang.lblMasterPresetPosition)
        TemplateHandler.Assign(numbergameTpl, "{overunder-text}", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(numbergameTpl, "{oddeven-text}", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(numbergameTpl, "{highlow-text}", Resources.Lang.highlow)
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
        TemplateHandler.Assign(numbergameTpl, "copy-oulive-text", Resources.Lang.lblcopyfromoverunder)
        TemplateHandler.Assign(numbergameTpl, "copy-1stlastou-text", Resources.Lang.lbl1stcopyfromoverunder)
        'TemplateHandler.Assign(numbergameTpl, "chkAuto-1", "chkAuto1HdpD")
        'TemplateHandler.Assign(numbergameTpl, "chkAuto-2", "chkAuto11x2")
        'TemplateHandler.Assign(numbergameTpl, "chkAuto-3", "chkAuto1HdpL")

        'assign value of Minbet, Maxbet , ...
        If isMult Then
            GetMinMaxAddValue(numbergameTpl, 161, "0"" disabled='true'")

            TemplateHandler.Assign(numbergameTpl, "chk-1611stOUD", "disabled")
            TemplateHandler.Assign(numbergameTpl, "chk-161OUL", "disabled")
        Else
            TemplateHandler.Assign(numbergameTpl, "minbet161-val", DataFormat.FormatNumber1(dr("minbetv161")))
            TemplateHandler.Assign(numbergameTpl, "maxbet161-val", DataFormat.FormatNumber1(dr("maxbetv161")))
            TemplateHandler.Assign(numbergameTpl, "maxpermatch161-val", DataFormat.FormatNumber1(dr("maxpermatchv161")))

            TemplateHandler.Assign(numbergameTpl, "chk-1611stOUD", "")
            TemplateHandler.Assign(numbergameTpl, "chk-161OUL", "")
        End If
        GetLimitMinMaxBet(dr, numbergameTpl, 161)

        If (Not UserPermission.IsShowNumberGroup) Then
            TemplateHandler.Assign(numbergameTpl, "hide", "hide")
        End If

        ''assign value position taking for template
        Dim m1stOUMin, m1stOEMin, mOEMin, mWRMin As Double
        Dim mLOUMin, mLOEMin, mLHLMin, mLNCMin, mLNGMin As Double

        Dim ma1stOUMin, ma1stOEMin, maOEMin, maWRMin As Double
        Dim maLOUMin, maLOEMin, maLHLMin, maLNCMin, maLNGMin As Double

        Dim ma1stOUMax, ma1stOEMax, maOEMax, maWRMax As Double
        Dim maLOUMax, maLOEMax, maLHLMax, maLNCMax, maLNGMax As Double

        Dim m1stOuMax, m1stOEMax, mOEMax, mWRMax As Double
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

        ''------------------------Auto PT ------------------------- Tho Add -------------------------------------
        Dim isAuto1stOU, isAuto1stOE, isAutoOE, isAutoWR As Double
        Dim isAutoLOU, isAutoLOE, isAutoLHL, isAutoLNC, isAutoLNG As Double

        Dim mAuto1stOU, mAuto1stOE, mAutoOE, mAutoWR As Double
        Dim mAutoLOU, mAutoLOE, mAutoLHL, mAutoLNC, mAutoLNG As Double

        Dim mAuto1stOUMax, mAuto1stOEMax, mAutoOEMax, mAutoWRMax As Double
        Dim mAutoLOUMax, mAutoLOEMax, mAutoLHLMax, mAutoLNCMax, mAutoLNGMax As Double
        ''-------------------------------------------------------------------------------------------------------

        a1stOUMax2 = CastAide.AsDouble(dr("v161apositiontaking_81_max")) 'min(a4,l2)
        a1stOEMax2 = CastAide.AsDouble(dr("v161apositiontaking_83_max"))
        aOEMax2 = CastAide.AsDouble(dr("v161apositiontaking_86_max"))
        aWRMax2 = CastAide.AsDouble(dr("v161apositiontaking_88_max"))

        aLOUMax2 = CastAide.AsDouble(dr("v161alivepositiontaking_85_max"))
        aLOEMax2 = CastAide.AsDouble(dr("v161alivepositiontaking_86_max"))
        aLHLMax2 = CastAide.AsDouble(dr("v161alivepositiontaking_87_max"))
        aLNCMax2 = CastAide.AsDouble(dr("v161alivepositiontaking_89_max"))
        aLNGMax2 = CastAide.AsDouble(dr("v161alivepositiontaking_90_max"))

        If isMult Then ' this case for Multiple Edit
            ma1stOUMax = CastAide.AsDouble(dr("v161mpositiontaking_81"))
            ma1stOEMax = CastAide.AsDouble(dr("v161mpositiontaking_83"))
            maOEMax = CastAide.AsDouble(dr("v161mpositiontaking_86"))
            maWRMax = CastAide.AsDouble(dr("v161mpositiontaking_88"))

            maLOUMax = CastAide.AsDouble(dr("v161mlivepositiontaking_85"))
            maLOEMax = CastAide.AsDouble(dr("v161mlivepositiontaking_86"))
            maLHLMax = CastAide.AsDouble(dr("v161mlivepositiontaking_87"))
            maLNCMax = CastAide.AsDouble(dr("v161mlivepositiontaking_89"))
            maLNGMax = CastAide.AsDouble(dr("v161mlivepositiontaking_90"))

            m1stOU = ma1stOUMax
            m1stOE = ma1stOEMax
            mOE = maOEMax
            mWR = maWRMax

            mLOU = maLOUMax
            mLOE = maLOEMax
            mLHL = maLHLMax
            mLNC = maLNCMax
            mLNG = maLNGMax

            m1stOuMax = ma1stOUMax
            m1stOEMax = ma1stOEMax
            mOEMax = maOEMax
            mWRMax = maWRMax

            mLOUMax = maLOUMax
            mLOEMax = maLOEMax
            mLHLMax = maLHLMax
            mLNCMax = maLNCMax
            mLNGMax = maLNGMax

            ma1stOUMin = CastAide.AsDouble(dr("v161mpositiontaking_81_min"))
            ma1stOEMin = CastAide.AsDouble(dr("v161mpositiontaking_83_min"))
            maOEMin = CastAide.AsDouble(dr("v161mpositiontaking_86_min"))
            maWRMin = CastAide.AsDouble(dr("v161mpositiontaking_88_min"))

            maLOUMin = CastAide.AsDouble(dr("v161mlivepositiontaking_85_min"))
            maLOEMin = CastAide.AsDouble(dr("v161mlivepositiontaking_86_min"))
            maLHLMin = CastAide.AsDouble(dr("v161mlivepositiontaking_87_min"))
            maLNCMin = CastAide.AsDouble(dr("v161mlivepositiontaking_89_min"))
            maLNGMin = CastAide.AsDouble(dr("v161mlivepositiontaking_90_min"))

            a1stOU = a1stOUMax
            a1stOE = a1stOEMax
            aOE = aOEMax
            aWR = aWRMax

            aLOU = aLOUMax
            aLOE = aLOEMax
            aLHL = aLHLMax
            aLNC = aLNCMax
            aLNG = aLNGMax

            'Terry Nguyen modify: Auto position taking
            mAuto1stOUMax = ma1stOUMax 'max u2
            mAuto1stOEMax = ma1stOEMax
            mAutoOEMax = mOEMax
            mAutoWRMax = mWRMax

            mAutoLOUMax = mLOUMax
            mAutoLOEMax = mLOEMax
            mAutoLHLMax = mLHLMax
            mAutoLNCMax = mLNCMax
            mAutoLNGMax = mLNGMax
            '    '/---
        Else ' this case for Single Edit
            m1stOU = CastAide.AsDouble(dr("v161mpositiontaking_81"))
            m1stOE = CastAide.AsDouble(dr("v161mpositiontaking_83"))
            mOE = CastAide.AsDouble(dr("v161mpositiontaking_86"))
            mWR = CastAide.AsDouble(dr("v161mpositiontaking_88"))

            mLOU = CastAide.AsDouble(dr("v161mlivepositiontaking_85"))
            mLOE = CastAide.AsDouble(dr("v161mlivepositiontaking_86"))
            mLHL = CastAide.AsDouble(dr("v161mlivepositiontaking_87"))
            mLNC = CastAide.AsDouble(dr("v161mlivepositiontaking_89"))
            mLNG = CastAide.AsDouble(dr("v161mlivepositiontaking_90"))

            a1stOU = CastAide.AsDouble(dr("v161apositiontaking_81"))
            a1stOE = CastAide.AsDouble(dr("v161apositiontaking_83"))
            aOE = CastAide.AsDouble(dr("v161apositiontaking_86"))
            aWR = CastAide.AsDouble(dr("v161apositiontaking_88"))

            aLOU = CastAide.AsDouble(dr("v161alivepositiontaking_85"))
            aLOE = CastAide.AsDouble(dr("v161alivepositiontaking_86"))
            aLHL = CastAide.AsDouble(dr("v161alivepositiontaking_87"))
            aLNC = CastAide.AsDouble(dr("v161alivepositiontaking_89"))
            aLNG = CastAide.AsDouble(dr("v161alivepositiontaking_90"))

            a1stOUMin = CastAide.AsDouble(dr("v161apositiontaking_81_min"))
            a1stOEMin = CastAide.AsDouble(dr("v161apositiontaking_83_min"))
            aOEMin = CastAide.AsDouble(dr("v161apositiontaking_86_min"))
            aWRMin = CastAide.AsDouble(dr("v161apositiontaking_88_min"))

            aLOUMin = CastAide.AsDouble(dr("v161alivepositiontaking_85_min"))
            aLOEMin = CastAide.AsDouble(dr("v161alivepositiontaking_86_min"))
            aLHLMin = CastAide.AsDouble(dr("v161alivepositiontaking_87_min"))
            aLNCMin = CastAide.AsDouble(dr("v161alivepositiontaking_89_min"))
            aLNGMin = CastAide.AsDouble(dr("v161alivepositiontaking_90_min"))

            ma1stOUMax = CastAide.AsDouble(dr("max_v161mapositiontaking_81")) ' min(m3,l3)
            ma1stOEMax = CastAide.AsDouble(dr("max_v161mapositiontaking_83"))
            maOEMax = CastAide.AsDouble(dr("max_v161mapositiontaking_86"))
            maWRMax = CastAide.AsDouble(dr("max_v161mapositiontaking_88"))

            maLOUMax = CastAide.AsDouble(dr("max_v161malivepositiontaking_85"))
            maLOEMax = CastAide.AsDouble(dr("max_v161malivepositiontaking_86"))
            maLHLMax = CastAide.AsDouble(dr("max_v161malivepositiontaking_87"))
            maLNCMax = CastAide.AsDouble(dr("max_v161malivepositiontaking_89"))
            maLNGMax = CastAide.AsDouble(dr("max_v161malivepositiontaking_90"))

            ma1stOUMin = CastAide.AsDouble(dr("min_v161mapositiontaking_81"))
            ma1stOEMin = CastAide.AsDouble(dr("min_v161mapositiontaking_83"))
            maOEMin = CastAide.AsDouble(dr("min_v161mapositiontaking_86"))
            maWRMin = CastAide.AsDouble(dr("min_v161mapositiontaking_88"))

            maLOUMin = CastAide.AsDouble(dr("min_v161malivepositiontaking_85"))
            maLOEMin = CastAide.AsDouble(dr("min_v161malivepositiontaking_86"))
            maLHLMin = CastAide.AsDouble(dr("min_v161malivepositiontaking_87"))
            maLNCMin = CastAide.AsDouble(dr("min_v161malivepositiontaking_89"))
            maLNGMin = CastAide.AsDouble(dr("min_v161malivepositiontaking_90"))

            'Terry Nguyen modify: Auto position taking
            mAuto1stOU = CastAide.AsDouble(dr("auto_v161mpositiontaking_81")) 'u2
            mAuto1stOE = CastAide.AsDouble(dr("auto_v161mpositiontaking_83"))
            mAutoOE = CastAide.AsDouble(dr("auto_v161mpositiontaking_86"))
            mAutoWR = CastAide.AsDouble(dr("auto_v161mpositiontaking_88"))

            mAutoLOU = CastAide.AsDouble(dr("auto_v161mlivepositiontaking_85"))
            mAutoLOE = CastAide.AsDouble(dr("auto_v161mlivepositiontaking_86"))
            mAutoLHL = CastAide.AsDouble(dr("auto_v161mlivepositiontaking_87"))
            mAutoLNC = CastAide.AsDouble(dr("auto_v161mlivepositiontaking_89"))
            mAutoLNG = CastAide.AsDouble(dr("auto_v161mlivepositiontaking_90"))
            '/---

            '------------------------Auto PT ------------------------- Tho Add ---------------------------------------------
            isAuto1stOU = CastAide.AsInt16(dr("isauto_v161mpositiontaking_81")) ' i2
            isAuto1stOE = CastAide.AsInt16(dr("isauto_v161mpositiontaking_83"))
            isAutoOE = CastAide.AsInt16(dr("isauto_v161mpositiontaking_86"))
            isAutoWR = CastAide.AsInt16(dr("isauto_v161mpositiontaking_88"))

            isAutoLOU = CastAide.AsInt16(dr("isauto_v161mlivepositiontaking_85"))
            isAutoLOE = CastAide.AsInt16(dr("isauto_v161mlivepositiontaking_86"))
            isAutoLHL = CastAide.AsInt16(dr("isauto_v161mlivepositiontaking_87"))
            isAutoLNC = CastAide.AsInt16(dr("isauto_v161mlivepositiontaking_89"))
            isAutoLNG = CastAide.AsInt16(dr("isauto_v161mlivepositiontaking_90"))

            mAuto1stOUMax = CastAide.AsDouble(dr("auto_v161mpositiontaking_81_max")) 'max u2
            mAuto1stOEMax = CastAide.AsDouble(dr("auto_v161mpositiontaking_83_max"))
            mAutoOEMax = CastAide.AsDouble(dr("auto_v161mpositiontaking_86_max"))
            mAutoWRMax = CastAide.AsDouble(dr("auto_v161mpositiontaking_88_max"))

            mAutoLOUMax = CastAide.AsDouble(dr("auto_v161mlivepositiontaking_85_max"))
            mAutoLOEMax = CastAide.AsDouble(dr("auto_v161mlivepositiontaking_86_max"))
            mAutoLHLMax = CastAide.AsDouble(dr("auto_v161mlivepositiontaking_87_max"))
            mAutoLNCMax = CastAide.AsDouble(dr("auto_v161mlivepositiontaking_89_max"))
            mAutoLNGMax = CastAide.AsDouble(dr("auto_v161mlivepositiontaking_90_max"))
            '    '-------------------------------------------------------------------------------------------------------
            m1stOuMax = ma1stOUMax - a1stOUMin
            m1stOEMax = ma1stOEMax - a1stOEMin
            mOEMax = maOEMax - aOEMin
            mWRMax = maWRMax - aWRMin

            mLOUMax = maLOUMax - aLOUMin
            mLOEMax = maLOEMax - aLOEMin
            mLHLMax = maLHLMax - aLHLMin
            mLNCMax = maLNCMax - aLNCMin
            mLNGMax = maLNGMax - aLNGMin
        End If
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        a1stOUMax = IIf(ma1stOUMax - m1stOU > a1stOUMax2, a1stOUMax2, ma1stOUMax - m1stOU)
        a1stOEMax = IIf(ma1stOEMax - m1stOE > a1stOEMax2, a1stOEMax2, ma1stOEMax - m1stOE)
        aOEMax = IIf(maOEMax - mOE > aOEMax2, aOEMax2, maOEMax - mOE)
        aWRMax = IIf(maWRMax - mWR > aWRMax2, aWRMax2, maWRMax - mWR)

        aLOUMax = IIf(maLOUMax - mLOU > aLOUMax2, aLOUMax2, maLOUMax - mLOU)
        aLOEMax = IIf(maLOEMax - mLOE > aLOEMax2, aLOEMax2, maLOEMax - mLOE)
        aLHLMax = IIf(maLHLMax - mLHL > aLHLMax2, aLHLMax2, maLHLMax - mLHL)
        aLNCMax = IIf(maLNCMax - mLNC > aLNCMax2, aLNCMax2, maLNCMax - mLNC)
        aLNGMax = IIf(maLNGMax - mLNG > aLNGMax2, aLNGMax2, maLNGMax - mLNG)

        hiddenHash.Add("minM1611stOUD", m1stOUMin)
        hiddenHash.Add("minM1611stOED", m1stOEMin)
        hiddenHash.Add("minM161OED", mOEMin)
        hiddenHash.Add("minM161WRD", mWRMin)

        hiddenHash.Add("minM161OUL", mLOUMin)
        hiddenHash.Add("minM161OEL", mLOEMin)
        hiddenHash.Add("minM161HLL", mLHLMin)
        hiddenHash.Add("minM161NCL", mLNCMin)
        hiddenHash.Add("minM161NGL", mLNGMin)

        hiddenHash.Add("maxA1611stOUD", IIf(a1stOUMax2 > ma1stOUMax, ma1stOUMax, a1stOUMax2))
        hiddenHash.Add("maxA1611stOED", IIf(a1stOEMax2 > ma1stOEMax, ma1stOEMax, a1stOEMax2))
        hiddenHash.Add("maxA161OED", IIf(aOEMax2 > maOEMax, maOEMax, aOEMax2))
        hiddenHash.Add("maxA161WRD", IIf(aWRMax2 > maWRMax, maWRMax, aWRMax2))

        hiddenHash.Add("maxA161OUL", IIf(aLOUMax2 > maLOUMax, maLOUMax, aLOUMax2))
        hiddenHash.Add("maxA161OEL", IIf(aLOEMax2 > maLOEMax, maLOEMax, aLOEMax2))
        hiddenHash.Add("maxA161HLL", IIf(aLHLMax2 > maLHLMax, maLHLMax, aLHLMax2))
        hiddenHash.Add("maxA161NCL", IIf(aLNCMax2 > maLNCMax, maLNCMax, aLNCMax2))
        hiddenHash.Add("maxA161NGL", IIf(aLNGMax2 > maLNGMax, maLNGMax, aLNGMax2))
        '--------
        hiddenHash.Add("maxM1611stOUD", ma1stOUMax)
        hiddenHash.Add("maxM1611stOED", ma1stOEMax)
        hiddenHash.Add("maxM161OED", maOEMax)
        hiddenHash.Add("maxM161WRD", maWRMax)

        hiddenHash.Add("maxM161OUL", maLOUMax)
        hiddenHash.Add("maxM161OEL", maLOEMax)
        hiddenHash.Add("maxM161HLL", maLHLMax)
        hiddenHash.Add("maxM161NCL", maLNCMax)
        hiddenHash.Add("maxM161NGL", maLNGMax)

        hiddenHash.Add("isAuto1611stOUD", isAuto1stOU)
        hiddenHash.Add("isAuto1611stOED", isAuto1stOE)
        hiddenHash.Add("isAuto161OED", isAutoOE)
        hiddenHash.Add("isAuto161WRD", isAutoWR)

        hiddenHash.Add("isAuto161OUL", isAutoLOU)
        hiddenHash.Add("isAuto161OEL", isAutoLOE)
        hiddenHash.Add("isAuto161HLL", isAutoLHL)
        hiddenHash.Add("isAuto161NCL", isAutoLNC)
        hiddenHash.Add("isAuto161NGL", isAutoLNG)

        strHidNumberGame = PositionTaking.AssignMulipleHidden(hiddenHash)

        ''assign Increase and Reduce button for template
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

        ''Terry Nguyen modify: Auto position taking
        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS1611stOUD")
        TemplateHandler.Assign(numbergameTpl, "au-1stlastou-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS161OUL")
        TemplateHandler.Assign(numbergameTpl, "au-Lou-btn", btnTpl)

        ''/--
        ''get PT deadball value for master preset PT
        Dim pt As String
        Dim ptMaster As String
        Dim strChecked As String
        Dim ptAuto As String
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAuto1stOU Then
            ptAuto = selectTpl
            pt = selectTpl
            ptMaster = selDisabled
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM1611stOUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(a1stOUMin, a1stOUMax, a1stOU))
        TemplateHandler.Assign(numbergameTpl, "a-1stlastou-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS1611stOUD")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(m1stOUMin, m1stOuMax, m1stOU))
        TemplateHandler.Assign(numbergameTpl, "p-1stlastou-val", ptMaster)

        ''Terry Nguyen modify: Auto position taking

        TemplateHandler.Assign(numbergameTpl, "checked-1stOUD", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1611stOUD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAuto1stOUMax, mAuto1stOU))
        TemplateHandler.Assign(numbergameTpl, "au-1stlastou-val", ptAuto)
        ''/--
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAuto1stOE Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM1611stOED")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(a1stOEMin, a1stOEMax, a1stOE))
        TemplateHandler.Assign(numbergameTpl, "a-1stlastoe-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS1611stOED")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(m1stOEMin, m1stOEMax, m1stOE))
        TemplateHandler.Assign(numbergameTpl, "p-1stlastoe-val", ptMaster)

        ''Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(numbergameTpl, "checked-1stOED", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1611stOED")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAuto1stOEMax, mAuto1stOE))
        TemplateHandler.Assign(numbergameTpl, "au-1stlastoe-val", ptAuto)
        ''/--

        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoOE Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM161OED")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aOEMin, aOEMax, aOE))
        TemplateHandler.Assign(numbergameTpl, "a-oe-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS161OED")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOE))
        TemplateHandler.Assign(numbergameTpl, "p-oe-val", ptMaster)

        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(numbergameTpl, "checked-OED", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161OED")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoOEMax, mAutoOE))
        TemplateHandler.Assign(numbergameTpl, "au-oe-val", ptAuto)
        ''/--
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoWR Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM161WRD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aWRMin, aWRMax, aWR))
        TemplateHandler.Assign(numbergameTpl, "a-warrior-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS161WRD")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mWRMin, mWRMax, mWR))
        TemplateHandler.Assign(numbergameTpl, "p-warrior-val", ptMaster)

        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(numbergameTpl, "checked-WRD", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161WRD")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoWRMax, mAutoWR))
        TemplateHandler.Assign(numbergameTpl, "au-warrior-val", ptAuto)
        ''------


        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoLOU Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM161OUL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aLOUMin, aLOUMax, aLOU))
        TemplateHandler.Assign(numbergameTpl, "a-Lou-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS161OUL")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mLOUMin, mLOUMax, mLOU))
        TemplateHandler.Assign(numbergameTpl, "p-Lou-val", ptMaster)

        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(numbergameTpl, "checked-OUL", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161OUL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoLOUMax, mAutoLOU))
        TemplateHandler.Assign(numbergameTpl, "au-Lou-val", ptAuto)
        ''/---
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoLOE Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM161OEL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aLOEMin, aLOEMax, aLOE))
        TemplateHandler.Assign(numbergameTpl, "a-Loe-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS161OEL")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mLOEMin, mLOEMax, mLOE))
        TemplateHandler.Assign(numbergameTpl, "p-Loe-val", ptMaster)

        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(numbergameTpl, "checked-OEL", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161OEL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoLOEMax, mAutoLOE))
        TemplateHandler.Assign(numbergameTpl, "au-Loe-val", ptAuto)

        ''/---
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoLHL Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM161HLL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aLHLMin, aLHLMax, aLHL))
        TemplateHandler.Assign(numbergameTpl, "a-Lhl-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS161HLL")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mLHLMin, mLHLMax, mLHL))
        TemplateHandler.Assign(numbergameTpl, "p-Lhl-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(numbergameTpl, "checked-HLL", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161HLL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoLHLMax, mAutoLHL))
        TemplateHandler.Assign(numbergameTpl, "au-Lhl-val", ptAuto)

        '---
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoLNC Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM161NCL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aLNCMin, aLNCMax, aLNC))
        TemplateHandler.Assign(numbergameTpl, "a-nextCombo-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS161NCL")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mLNCMin, mLNCMax, mLNC))
        TemplateHandler.Assign(numbergameTpl, "p-nextCombo-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(numbergameTpl, "checked-NCL", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161NCL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoLNCMax, mAutoLNC))
        TemplateHandler.Assign(numbergameTpl, "au-nextCombo-val", ptAuto)
        ''----

        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoLNG Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM161NGL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aLNGMin, aLNGMax, aLNG))
        TemplateHandler.Assign(numbergameTpl, "a-NumGr-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS161NGL")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mLNGMin, mLNGMax, mLNG))
        TemplateHandler.Assign(numbergameTpl, "p-NumGr-val", ptMaster)
        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(numbergameTpl, "checked-NGL", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS161NGL")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoLNGMax, mAutoLNG))
        TemplateHandler.Assign(numbergameTpl, "au-NumGr-val", ptAuto)

        Return numbergameTpl
    End Function
#End Region

#Region "Assign info for MixedSports"
    Function GetDataMixedSports(ByVal dr As DataRow, Optional ByVal isMult As Boolean = False) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable
        Dim tpl As String = TemplateHandler.LoadTemplate("MemberInfo/MixSportsPT.html")

        'assign text for position taking
        TemplateHandler.Assign(tpl, "title-col", "2")
        LoadChkUpdate(tpl, "99_M", isMult)
        GetTitleCommon(tpl)
        TemplateHandler.Assign(tpl, "mp-text", Resources.Lang.lblOverUnder)
        TemplateHandler.RemoveBlock(tpl, "none-edit")
        'assign value for position taking
        If Not isMult Then
            TemplateHandler.Assign(tpl, "minbet99_M-val", DataFormat.FormatNumber1(dr("minbetv99MP")))
            TemplateHandler.Assign(tpl, "maxbet99_M-val", DataFormat.FormatNumber1(dr("maxbetv99MP")))
            TemplateHandler.Assign(tpl, "maxpermatch99_M-val", DataFormat.FormatNumber1(dr("maxpermatchv99MP")))

            TemplateHandler.Assign(tpl, "chk-mp", "")
        Else
            TemplateHandler.Assign(tpl, "minbet99_M-val", "0"" disabled='true'")
            TemplateHandler.Assign(tpl, "maxbet99_M-val", "0"" disabled='true'")
            TemplateHandler.Assign(tpl, "maxpermatch99_M-val", "0"" disabled='true'")

            TemplateHandler.Assign(tpl, "chk-mp", "disabled")
        End If
        Dim minbetMin As Double
        Dim maxbetMax As Double
        Dim maxpermatchMin As Double
        Dim maxpermatchMax As Double

        minbetMin = DataFormat.FormatNumber1(dr("MinBetv99MP_Max"))
        maxbetMax = DataFormat.FormatNumber1(dr("MaxBetv99MP_Min"))
        maxpermatchMin = DataFormat.FormatNumber1(dr("MaxPerMatchv99MP_Min"))
        maxpermatchMax = DataFormat.FormatNumber1(dr("MaxPerMatchv99MP_Max"))

        TemplateHandler.Assign(tpl, "minbet-min-99_M", DataFormat.FormatNumber1(minbetMin))
        TemplateHandler.Assign(tpl, "maxbet-max-99_M", DataFormat.FormatNumber1(maxbetMax))
        TemplateHandler.Assign(tpl, "maxpermatch-min-99_M", DataFormat.FormatNumber1(maxpermatchMin))
        TemplateHandler.Assign(tpl, "maxpermatch-max-99_M", DataFormat.FormatNumber1(maxpermatchMax))
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
        Dim maMPMax As Double

        'Terry Nguyen modify: Auto position taking
        Dim mAutoMP As Double
        Dim mAutoMPMax As Double
        Dim isAutoMP As Double
        ''/---
        aMPMax2 = CastAide.AsDouble(dr("v99apositiontaking_9_max"))

        If isMult Then
            maMPMax = CastAide.AsDouble(dr("v99mpositiontaking_9"))
            mMP = maMPMax
            mMPMax = maMPMax
            maMPMin = CastAide.AsDouble(dr("v99mpositiontaking_9_min"))
            aMP = aMPMax

            'Terry Nguyen modify: Auto position taking
            mAutoMPMax = maMPMax 'max u2               
            '/---
        Else
            maMPMax = CastAide.AsDouble(dr("max_v99mapositiontaking_9"))
            mMP = CastAide.AsDouble(dr("v99mpositiontaking_9"))
            aMP = CastAide.AsDouble(dr("v99apositiontaking_9"))
            aMPMin = CastAide.AsDouble(dr("v99apositiontaking_9_min"))
            maMPMin = CastAide.AsDouble(dr("min_v99mapositiontaking_9"))
            mMPMax = maMPMax - aMPMin

            'Terry Nguyen modify: Auto position taking
            mAutoMP = CastAide.AsDouble(dr("auto_v99mpositiontaking_9"))
            '/---

            'Terry Nguyen modify: Auto position taking
            isAutoMP = CastAide.AsInt16(dr("isauto_v99mpositiontaking_9"))
            mAutoMPMax = CastAide.AsDouble(dr("auto_v99mpositiontaking_9_max"))
            '/---
        End If

        aMPMax = IIf(maMPMax - mMP > aMPMax2, aMPMax2, maMPMax - mMP)

        hiddenHash.Add("minM99_M", maMPMin)
        hiddenHash.Add("maxA99_M", IIf(aMPMax2 > maMPMax, maMPMax, aMPMax2))
        hiddenHash.Add("maxM99_M", maMPMax)
        hiddenHash.Add("isAuto99_M", isAutoMP)

        strHidMixSports = PositionTaking.AssignMulipleHidden(hiddenHash)

        ''assign data for Position taking dropdowns
        pt = selectTpl
        Dim ptMaster As String
        Dim ptAuto As String
        Dim strChecked As String
        If isMult Then
            ptAuto = selDisabled
            pt = selDisabled
            ptMaster = selDisabled
            strChecked = "disabled"
        ElseIf isAutoMP Then
            ptAuto = selectTpl
            ptMaster = selDisabled
            pt = selectTpl
            strChecked = "checked"
        Else
            ptAuto = selDisabled
            pt = selectTpl
            ptMaster = selectTpl
            strChecked = ""
        End If
        TemplateHandler.Assign(pt, "ddl-Id", "listM99_M")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(aMPMin, aMPMax, aMP))
        TemplateHandler.Assign(tpl, "a-mp-val", pt)

        TemplateHandler.Assign(ptMaster, "ddl-Id", "listS99_M")
        TemplateHandler.Assign(ptMaster, "ddl-val", PositionTaking.GetListPositionTaking(mMPMin, mMPMax, mMP))
        TemplateHandler.Assign(tpl, "p-mp-val", ptMaster)

        'Terry Nguyen modify: Auto position taking
        TemplateHandler.Assign(tpl, "checked-mp", strChecked)
        TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS99_M")
        TemplateHandler.Assign(ptAuto, "ddl-val", PositionTaking.GetListPositionTaking(0, mAutoMPMax, mAutoMP))
        TemplateHandler.Assign(tpl, "au-mp-val", ptAuto)

        ''/---

        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM99_M")
        TemplateHandler.Assign(tpl, "a-mp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listS99_M")
        TemplateHandler.Assign(tpl, "p-mp-btn", btnTpl)

        'Terry Nguyen modify: Auto position taking
        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS99_M")
        TemplateHandler.Assign(tpl, "au-mp-btn", btnTpl)

        '/---

        Return tpl
    End Function
#End Region

#Region "Get Title Common"
    Sub GetTitleCommon(ByRef tpl As String)

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
#End Region

    '--------------------------------------------------------------------------

#Region "ReplaceData"
    'Public Function ReplaceData(ByVal str As String) As String
    '    Return str.Replace("""", "&quot;").Replace("'", "&#39;").Replace(vbNewLine, "")
    'End Function
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

#Region "GetTitleInfo"
    Shared Sub GetTitleNumberGame(ByRef strHtml As String, Optional ByVal isMult As Boolean = False)

        Dim strMaxPerBall As String = String.Empty
        If isMult Then
            strMaxPerBall = "<input type=""checkbox"" id=""chkUpdMaxPerBall"" onclick=""AllowUpdate(this,true);"" name=""chkUpdMaxPerBall""/>"
        End If
        TemplateHandler.Assign(strHtml, "{othersetting}", strMaxPerBall + Resources.Lang.othersetting)
        TemplateHandler.Assign(strHtml, "{lblMaxPerBall}", Resources.Lang.maxperball)
    End Sub

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
        TemplateHandler.Assign(strHtml, "discount1x2-text", Resources.Lang.lbl1x2Comm)
        TemplateHandler.Assign(strHtml, "discountnumbergame-text", Resources.Lang.numbergamecomm)
        TemplateHandler.Assign(strHtml, "{othersetting}", strMaxPerBall + Resources.Lang.othersetting)
        TemplateHandler.Assign(strHtml, "{lblMaxPerBall}", Resources.Lang.maxperball)
    End Sub
#End Region

#Region "LoadChkUpdate"
    'this function is used to Load checkbox allow Update Info or not
    'in Multiple Edit case, checkbox is checked
    Sub LoadChkUpdate(ByRef tpl As String, ByVal sportId As String, ByVal isMult As Boolean)

        Dim strChk As String = String.Empty
        Dim strChkPt As String = String.Empty
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

#Region "LoadSoccerPTHeader"
    Function LoadSoccerPTHeader(Optional ByVal isMember As Boolean = False) As String
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
        TemplateHandler.Assign(strHtml, "{warrior-text}", Resources.Lang.warrior)
        TemplateHandler.Assign(strHtml, "{oe-text}", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(strHtml, "{numberGroup-text}", Resources.Lang.numberGroup)



        Return strHtml
    End Function

#End Region

#Region "GetMinMaxAddValue"
    'this function is used to assign value for Min Bet, Max Bet, Max Per Match in Adding, Multiple Editing case
    Shared Sub GetMinMaxAddValue(ByRef tpl As String, ByVal sportTypeId As Integer, ByVal strReplace As String)
        TemplateHandler.Assign(tpl, "minbet" & sportTypeId & "-val", strReplace)
        TemplateHandler.Assign(tpl, "maxbet" & sportTypeId & "-val", strReplace)
        TemplateHandler.Assign(tpl, "maxpermatch" & sportTypeId & "-val", strReplace)
    End Sub
#End Region

#Region "GetLimitMinMaxBet"
    'this function is used to display or not limit values of Min Bet, Max Bet, Max Per Match 
    'dr: row contains data get from db
    'tpl: template for each sporttype
    'sportTypeId: id of sporttype (same to Sport table in db)
    Sub GetLimitMinMaxBet(ByVal dr As DataRow, ByRef tpl As String, ByVal sportTypeId As String)

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

    End Sub
#End Region

#Region "GetListPositionTaking"
    'Function GetListPositionTaking(ByVal minVal As Double, ByVal maxVal As Double, Optional ByVal currentVal As Double = 0) As String
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

#Region "GetMinMaxEditValue"
    'this function is used to assign value for Min Bet, Max Bet, Max Per Match in Single Editing case
    Sub GetMinMaxEditValue(ByRef tpl As String, ByVal sportTypeId As Integer, ByVal dr As DataRow)
        Dim hiddenMinMaxBet As New Hashtable
        TemplateHandler.Assign(tpl, "minbet" & sportTypeId & "-val", DataFormat.FormatNumber1(dr("minbetv" & sportTypeId)))
        TemplateHandler.Assign(tpl, "maxbet" & sportTypeId & "-val", DataFormat.FormatNumber1(dr("maxbetv" & sportTypeId)))
        TemplateHandler.Assign(tpl, "maxpermatch" & sportTypeId & "-val", DataFormat.FormatNumber1(dr("maxpermatchv" & sportTypeId)))
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
End Class

Imports System
Imports System.Text
Imports System.Data
Imports System.Resources
Imports System.Collections

Imports DNAide.Web
Imports DNAide.Core
Imports DNAide.Utils
Imports Fanex.Data.DNAide

Imports Sunplus.Agent.Model.Security
Imports Sunplus.Agent.Model.Entities.Permission

Public Class EditMember2
    Inherits TplPage
    Public username As String = String.Empty
    Protected maxPerMatchDisp As String = String.Empty
    Private array_custid As String
    Public CID As String = ""


    Dim bodyTpl, strAccInfo, tennisTpl, basketballTpl, footballTpl, otherSportsTpl, golfTpl, groupBtn, selectTpl, strHidSoccer As String
    Dim strHidBaseball, strHidGolf, strHidMoto, strHidOther, strHidBasket, strHidFootball, strHidTennis, strHidVariable As String
    Public script, content As New StringBuilder
    Protected groupa As String = String.Empty
    Protected groupb As String = String.Empty
    Protected groupc As String = String.Empty
    Protected groupd As String = String.Empty
    Public AllowUdpPT As String = "0", Mult = "0"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        'Dim spName As String = "acc_CustomerSelEditCustomer"
        Me.CssGroup = "popup"
        Dim mainContent As String
        mainContent = TemplateHandler.LoadTemplate("MemberInfo/Agent_EditMember.html")

        If (String.IsNullOrEmpty(Request.QueryString("nobackground"))) Then
            TemplateHandler.RemoveBlock(mainContent, "nobackground")
        Else
            'If is from Account Menu, Then Set delay reload page
            Dim hash As Hashtable = AppSession.RequestQueryString
            If (hash Is Nothing) Then
                hash = New Hashtable
            End If
            If (Not hash.ContainsKey("FromAccount")) Then
                hash.Add("FromAccount", "../_MemberInfo/CustomerList/Agent/MemberList.aspx")
            Else
                hash("FromAccount") = "../_MemberInfo/CustomerList/Agent/MemberList.aspx"
            End If
            AppSession.RequestQueryString = hash
        End If


        If Not IsPostBack Then
            If (Request.QueryString("m") = "0") Then ' edit single
                content.Append("<script type=""text/javascript""> var isMult=0;</script>")
                CID = Request.QueryString("custid").Replace("^", "")
                Mult = "0"
            Else ' edit mult
                CID = Request("agentid") 'Sunplus.Agent.Model.Entities.UserSession.Current.CustId
                content.Append("<script type=""text/javascript""> var isMult=1;</script>")
                Mult = "1"
            End If
        End If

        Dim rs As DbResult

        Dim ht As New Hashtable
        ht.Add("custid", CID)
        If Mult = "0" Then 'Single
            rs = AccountInfo.Acc_CustomerSelEditCustomer(ht)
        Else 'Mult
            rs = AccountInfo.Acc_CustomerSelAgentInfo(ht)
        End If

        If rs.ResultSet.Tables(0).Rows.Count > 0 Then

            Dim dr As DataRow = rs.ResultSet.Tables(0).Rows(0)

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



            strAccInfo = TemplateHandler.LoadTemplate("MemberInfo/AccInfo.html")

            If (Sunplus.Agent.Model.Entities.UserSession.Current.IsInternal) Then
                TemplateHandler.Assign(strAccInfo, "clickable", "return false")
            Else
                TemplateHandler.Assign(strAccInfo, "clickable", String.Empty)
            End If

            ' Casino
            If Mult = "0" Then 'Single
                TemplateHandler.Assign(bodyTpl, "hidSynCS", dr("synccasino"))
            End If
            TemplateHandler.RemoveBlock(strAccInfo, "casino")

            'nqminh - change to popup stylesheet!
            strAccInfo = Common.ReplaceWithPopupStyle(strAccInfo)

            groupBtn = TemplateHandler.LoadTemplate("MemberInfo/BtnGroup.html")
            selectTpl = TemplateHandler.LoadTemplate("MemberInfo/DdlMemPT.html")
            tennisTpl = TemplateHandler.LoadTemplate("MemberInfo/TennisPT.html")
            basketballTpl = TemplateHandler.LoadTemplate("MemberInfo/BasketballPT.html")
            footballTpl = TemplateHandler.LoadTemplate("MemberInfo/FootballPT.html")
            otherSportsTpl = TemplateHandler.LoadTemplate("MemberInfo/OtherSportsPT.html")
            golfTpl = TemplateHandler.LoadTemplate("MemberInfo/GolfPT.html")
            TemplateHandler.RemoveBlock(selectTpl, "none-member")
            Common.GetSportTitle(bodyTpl, False)
            'If Mult = "1" Then
            '    TemplateHandler.RemoveBlock(bodyTpl, "none-Mult")
            '    TemplateHandler.RemoveBlock(bodyTpl, "none-Mult")
            'End If

            TemplateHandler.Assign(bodyTpl, "add-text", Resources.Lang.submit)
            TemplateHandler.Assign(bodyTpl, "add-id", "btnSubmit")

            TemplateHandler.Assign(strAccInfo, "add-text", Resources.Lang.submit)
            TemplateHandler.Assign(strAccInfo, "add-id", "btnSubmitFooter")

            If UserPermission.IsShowNumberGame Then
                TemplateHandler.Assign(bodyTpl, "copy-all-text", Resources.Lang.lblCopyAllPTfromHdp)
                TemplateHandler.Assign(bodyTpl, "copy-basket-text", Resources.Lang.lblCopyBasketHdp)
            Else
                TemplateHandler.Assign(bodyTpl, "copy-all-text", Resources.Lang.lblCopyAllPTfromHdpNoNumberGame)
                TemplateHandler.Assign(bodyTpl, "copy-basket-text", Resources.Lang.lblCopyBasketHdpNoNumberGame)
            End If


            TemplateHandler.Assign(bodyTpl, "soccerPT", GetDataSoccer(dr))
            TemplateHandler.Assign(bodyTpl, "basketPT", GetData4DDl(dr, 2, basketballTpl))
            TemplateHandler.Assign(bodyTpl, "footballPT", GetData4DDl(dr, 3, footballTpl))
            TemplateHandler.Assign(bodyTpl, "tennisPT", GetData4DDl(dr, 5, tennisTpl))
            TemplateHandler.Assign(bodyTpl, "baseballPT", GetDataBaseball(dr, 8))
            TemplateHandler.Assign(bodyTpl, "golfPT", GetData4DDl(dr, 10, golfTpl))
            TemplateHandler.Assign(bodyTpl, "motoPT", GetDataMotoSports(dr, 11))

            TemplateHandler.Assign(bodyTpl, "otherPT", GetData4DDl(dr, 99, otherSportsTpl))
            If UserPermission.IsShowNumberGame Then
                TemplateHandler.Assign(bodyTpl, "numbergamePT", GetDataNumberGame(dr))
            Else
                TemplateHandler.Assign(bodyTpl, "numbergamePT", "")
            End If

            If UserPermission.IsShowMixedSport Then
                TemplateHandler.Assign(bodyTpl, "mixSportPT", GetDataMixedSports(dr))
            End If

            '

            TemplateHandler.RemoveBlock(bodyTpl, "none-edit-Min")

            'If (Convert.ToBoolean(dr("AllowUpdatePositiontaking"))) Then
            '    AllowUdpPT = "1"
            'End If
            AllowUdpPT = "1"
            content.Append(AssignDataInfo(dr))
            content.Append(bodyTpl)
            content.Append(strHidVariable)

            content.Append(VariableScript())


        End If
        content.Append("<script type=""text/javascript""> var AllowUdpPT = " + AllowUdpPT + ";</script>")
        TemplateHandler.Assign(mainContent, "CID", CID.ToString())
        TemplateHandler.Assign(mainContent, "Mult", Mult)
        TemplateHandler.Assign(mainContent, "AllowUdpPT", AllowUdpPT)

        TemplateHandler.Assign(mainContent, "content", content.ToString)

        If (Sunplus.Agent.Model.Entities.UserSession.Current.IsInternal) Then
            TemplateHandler.Assign(mainContent, "clickable", "return false")
        Else
            TemplateHandler.Assign(mainContent, "clickable", String.Empty)
        End If

        'If Mult = "1" Then
        '    TemplateHandler.Assign(mainContent, "disable_chkall", "disabled")
        '    TemplateHandler.Assign(mainContent, "disable_chkbasket", "disabled")
        'End If


        Me.TplSetContent(mainContent)

    End Sub

    '------------------------------------------------
#Region "Declare javascript parameter"
    Function VariableScript()
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
        script += " var alertMaxperMatchNegative ='" & Resources.Lang.lblmaxperballgreaterhanzero & "';" & vbNewLine
        script += " var alertMaxperball='" & Resources.Lang.alertIncorrectMaxPerBall & "';" & vbNewLine
        script += " var alertMaxperballCMax='" & Resources.Lang.lblalertMaxPerBallCompareMax & "';" & vbNewLine
        script += " var alertMaxperballC='" & Resources.Lang.lblalertMaxPerBallCompare & "';" & vbNewLine
        script += " var alertMaxPerMatchLessMaxPerBall='" & Resources.Lang.alertMaxPerMatchLessMaxPerBall & "';" & vbNewLine
        script += " var alertMaxPerBallLessMaxBet='" & Resources.Lang.alertMaxPerBallLessMaxBet & "';" & vbNewLine

        script += " var confirmUnSuspend=""" & Resources.Lang.confirmUnSuspendMem & """;" & vbNewLine
        script += " var confirmSuspend=""" & Resources.Lang.confirmSuspendMem & """;" & vbNewLine
        script += " var confirmClosed=""" & Resources.Lang.confirmCloseMem & """;" & vbNewLine
        script += " var confirmOpen=""" & Resources.Lang.confirmCloseMem & """;" & vbNewLine

        Dim lblCreditInvalid As String
        lblCreditInvalid = Resources.Lang.lblCreditInvalid.Replace("{0}", Resources.Lang.lblMemberMaxCredit)

        If (Mult = "0") Then
            script += " var alertUserName='" & Resources.Lang.alertUserName & "';" & vbNewLine
            script += " var lblPasswordRequire='" & Resources.Lang.lblPasswordRequire & "';" & vbNewLine
            script += " var alertPasswordEmpty='" & Resources.Lang.alertPasswordEmpty & "';" & vbNewLine
            script += " var alertIncorrectCredit='" & Resources.Lang.alertMemberCredit & "';" & vbNewLine
            script += " var alertCreditInvalid='" & lblCreditInvalid & "';" & vbNewLine
        End If

        Dim strAlertMinPT As String = Resources.Lang.alertLessthanMinPT
        strAlertMinPT = strAlertMinPT.Replace("{1}", Resources.Lang.lblMasterPresetPosition)
        strAlertMinPT = strAlertMinPT.Replace("{2}", Resources.Lang.lblSuperPresetPosition)
        script += " var alertLessthanMinPT='" & strAlertMinPT & "';" & vbNewLine
        script += "</script>"
        Return script

    End Function
#End Region

#Region "Load AccInfoMem template"
    Function AssignDataInfo(ByVal dr As DataRow) As String


        If Mult = "1" Then ' edit multi
            TemplateHandler.Assign(strAccInfo, "title", Resources.Lang.lblEditMultiple)
        Else
            Dim nickname As String = CastAide.AsString(dr("username2"), String.Empty)
            If nickname.Length > 0 Then
                nickname = " (" + nickname + ")"
            End If
            TemplateHandler.Assign(strAccInfo, "title", Resources.Lang.edit & " " & Request("USER").Replace("^", "") + nickname)
        End If
        TemplateHandler.Assign(strAccInfo, "max-credit-text", Resources.Lang.lblMemberMaxCredit)
        Dim discount As Double = 0
        Dim discountcs As Double = CastAide.AsDouble(dr("discountcs"))
        Dim discount1x2 As Double = CastAide.AsDouble(dr("discount1x2"))


        'Dim discountfc As Double = 0
        TemplateHandler.RemoveBlock(strAccInfo, "none-super")
        TemplateHandler.RemoveBlock(strAccInfo, "none-text-user")
        TemplateHandler.RemoveBlock(strAccInfo, "none-edit")
        TemplateHandler.Assign(strAccInfo, "tranfer-time", "")
        TemplateHandler.Assign(strAccInfo, "suspend-text", Resources.Lang.suspended)
        TemplateHandler.RemoveBlock(strAccInfo, "none-HRdisable")

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

        If Mult = "1" Then ' mult
            TemplateHandler.RemoveBlock(strAccInfo, "none-mult")
            TemplateHandler.RemoveBlock(strAccInfo, "none-show-info-user-when-edit-mult")
            TemplateHandler.RemoveBlock(strAccInfo, "none-edit-member")
            TemplateHandler.Assign(strAccInfo, "cols-untext", "5")
            Dim str As String = "<input type=""checkbox"" id=""chkUpdStatus"" onclick=""AllowUpdate(this,false);"" name=""chkUpdStatus""/>" + Resources.Lang.status
            TemplateHandler.Assign(strAccInfo, "status-text", str)
            TemplateHandler.Assign(strAccInfo, "colspan", "colspan='2'")
            TemplateHandler.Assign(strAccInfo, "IsSuspended", 0)
            TemplateHandler.Assign(strAccInfo, "UplineSuspended", 0)





        Else 'single
            Dim htbDis As Hashtable = New Hashtable()
            htbDis.Add("Suspend", True)
            Dim mytemplate As String = TemplateHandler.LoadTemplate("Memberinfo/DisableStatus.html")
            Dim mycolspan As String = "<td colspan='2'><td>"
            TemplateHandler.RemoveBlock(mytemplate, "Captchar")
            TemplateHandler.ReplaceBlock(strAccInfo, "Status-Disable", StatusSetting.StatusDisable(mycolspan, htbDis, mytemplate))
            TemplateHandler.Assign(strAccInfo, "suspend-text", Resources.Lang.suspended)

            TemplateHandler.RemoveBlock(strAccInfo, "none-single")
            TemplateHandler.RemoveBlock(strAccInfo, "none-single")
            TemplateHandler.RemoveBlock(strAccInfo, "none-single")
            TemplateHandler.RemoveBlock(strAccInfo, "none-cell-member")
            TemplateHandler.Assign(strAccInfo, "colspan", "")
            TemplateHandler.Assign(strAccInfo, "cols-untext", "4")
            TemplateHandler.Assign(strAccInfo, "fname-val", ReplaceData(CStr(dr("firstname"))))
            TemplateHandler.Assign(strAccInfo, "lname-val", ReplaceData(CStr(dr("lastname"))))
            TemplateHandler.Assign(strAccInfo, "phone-val", ReplaceData(CStr(dr("phone"))))
            TemplateHandler.Assign(strAccInfo, "mobile-val", ReplaceData(CStr(dr("mobilephone"))))
            ' display value , limits of credit
            TemplateHandler.Assign(strAccInfo, "credit-val", DataFormat.FormatNumber1(dr("credit")))
            GetCredit(dr, strAccInfo)
            discount = CastAide.AsDouble(dr("discount"))
            TemplateHandler.Assign(strAccInfo, "fax-val", dr("fax"))
            TemplateHandler.Assign(strAccInfo, "UGroup", dr("ugroup"))

            Dim isClosed As String = ""
            If dr("closed") Then
                isClosed = "checked"
            End If
            TemplateHandler.Assign(strAccInfo, "check-val", isClosed)

            Dim status As String = ""

            If (CastAide.AsBoolean(dr("suspended"), False)) Then
                status = "checked='checked' "
            End If

            If (CastAide.AsBoolean(dr("Asuspended"), False)) Then
                status = status + "disabled='disabled'"
            End If

            TemplateHandler.Assign(strAccInfo, "suspend-check", status)
            TemplateHandler.Assign(strAccInfo, "IsSuspended", CastAide.AsInt32(dr("suspended"), 0))
            TemplateHandler.Assign(strAccInfo, "IsClosed", CastAide.AsInt32(dr("closed"), 0))
            TemplateHandler.Assign(strAccInfo, "UplineSuspended", CastAide.AsInt32(dr("Asuspended"), 0))

        End If

        TemplateHandler.Assign(strAccInfo, "discount-val", CStr(discount))
        TemplateHandler.Assign(strAccInfo, "discountcs-val", CStr(discountcs))
        TemplateHandler.Assign(strAccInfo, "discount1x2-val", CStr(discount1x2))
        Dim discountNumberGame As Double = CastAide.AsDouble(dr("discountnumber"))
        If (Not UserPermission.IsShowNumberGame Or Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission) Then
            TemplateHandler.Assign(strAccInfo, "lblCommissionForAgent_numbergame", String.Empty)
            TemplateHandler.Assign(strAccInfo, "lblCommissionForMember_numbergame", String.Empty)
            TemplateHandler.Assign(strAccInfo, "numbergame", String.Empty)
            TemplateHandler.RemoveBlock(strAccInfo, "none-text-numbergame")
            TemplateHandler.Assign(strAccInfo, "CommissionNumberGame", String.Empty)
            TemplateHandler.Assign(strAccInfo, "hideClss", "hidden")
        Else
            TemplateHandler.Assign(strAccInfo, "lblCommissionForAgent_numbergame", Resources.Lang.lblCommissionForAgent)
            TemplateHandler.Assign(strAccInfo, "lblCommissionForMember_numbergame", Resources.Lang.lblCommissionForMember)

            TemplateHandler.Assign(strAccInfo, "numbergame", Resources.Lang.numbergame)
            TemplateHandler.Assign(strAccInfo, "discountnumber-val", CStr(discountNumberGame))
            TemplateHandler.Assign(strAccInfo, "CommissionNumberGame", AssignData4Commission(dr, "ddrCommissionumbergame"))
            TemplateHandler.Assign(strAccInfo, "hideClss", String.Empty)
        End If


        TemplateHandler.Assign(strAccInfo, "group-text", Resources.Lang.group)
        TemplateHandler.Assign(strAccInfo, "lblCommissionForAgent", Resources.Lang.lblCommissionForAgent)
        TemplateHandler.Assign(strAccInfo, "lblAsianHDC_OU_OE", Resources.Lang.lblAsianHDC_OU_OE)
        TemplateHandler.Assign(strAccInfo, "lbl1x2_CS_TG", Resources.Lang.lblOtherTypes)

        TemplateHandler.Assign(strAccInfo, "lblCommissionForMember", Resources.Lang.lblCommissionForMember)
        TemplateHandler.Assign(strAccInfo, "CommissionH_OU_OE", AssignData4Commission(dr, "ddrCommissionH_OU_OE"))
        TemplateHandler.Assign(strAccInfo, "CommissionCS_TG", AssignData4Commission(dr, "ddrCommissionCS_TG"))
        TemplateHandler.Assign(strAccInfo, "Commission1x2", AssignData4Commission(dr, "ddrCommission1x2"))





        GetTitleInfo(strAccInfo, IIf(Mult = "1", True, False))
        Dim hiddenHash As Hashtable = New Hashtable
        hiddenHash.Add("hidden_discount", discount)
        hiddenHash.Add("hidden_discountcs", discountcs)
        hiddenHash.Add("hidden_discount1x2", discount1x2)
        hiddenHash.Add("hidden_discountnumbergame", discountNumberGame)
        strHidVariable = PositionTaking.AssignMulipleHidden(hiddenHash)
        Return strAccInfo

    End Function
#End Region

#Region "AssignData4Commission(ByVal dr As DataRow, ByVal ddl As String)"
    Function AssignData4Commission(ByVal dr As DataRow, ByVal ddl As String) As String
        Dim d As Integer = 4
        Dim csm As Double = 0
        Dim str As String = "<SELECT class='selectcomm' id='" + ddl + "' name='" + ddl + "'>"
        If (ddl = "ddrCommissionH_OU_OE") Then
            If Mult = "1" Then
                'csm = 0.01
                csm = CDbl(FormatNumber(dr("groupa"), d))
            Else
                str += "<option value='" + Format(dr("playerdiscount"), "0.####") + "' selected='true'>" + Format(dr("playerdiscount"), "0.####") + "</option>"
                csm = CDbl(dr("discount"))
            End If
        ElseIf ddl = "ddrCommission1x2" Then
            d = 5
            If (Mult = "1") Then
                'csm = 0.05
                csm = CDbl(FormatNumber(dr("discount1x2"), d))
            Else
                str += "<option value='" + Format(dr("playerdiscount1x2"), "0.#####") + "' selected='true'>" + Format(dr("playerdiscount1x2"), "0.#####") + "</option>"
                csm = CDbl(dr("discount1x2"))
            End If
        ElseIf ddl = "ddrCommissionumbergame" Then
            d = 5
            If (Mult = "1") Then
                'csm = 0.05
                csm = CDbl(FormatNumber(dr("discountnumber"), d))
            Else
                str += "<option value='" + Format(dr("playerdiscountnumber"), "0.#####") + "' selected='true'>" + Format(dr("playerdiscountnumber"), "0.#####") + "</option>"
                csm = CDbl(dr("discountnumber"))
            End If
        Else
            d = 5
            If (Mult = "1") Then
                'csm = 0.05
                csm = CDbl(FormatNumber(dr("discountcs"), d))
            Else
                str += "<option value='" + Format(dr("playerdiscountcs"), "0.#####") + "' selected='true'>" + Format(dr("playerdiscountcs"), "0.#####") + "</option>"
                csm = CDbl(dr("discountcs"))
            End If
        End If
        Dim index As Integer = 0
        While CDbl(FormatNumber(csm, d)) > 0
            If (d = 4) Then
                str += "<option value='" + Format(csm, "0.####") + "'" + IIf(Mult = "1" And index = 0, "selected='true'", "") + ">" + Format(csm, "0.####") + "</option>"
                csm = csm - 0.0005
            Else
                'csm = csm - 0.005
                str += "<option value='" + Format(csm, "0.#####") + "'" + IIf(Mult = "1" And index = 0, "selected='true'", "") + ">" + Format(csm, "0.#####") + "</option>"
                csm = csm - 0.0005
            End If
            index = index + 1
        End While
        str += "<option value='0'>0</option>"
        str += "</select>"
        Return str
    End Function

#End Region

#Region "AssignData4Group(ByVal dr As DataRow)"
    Function AssignData4Group() As String
        Dim str As String = "<select id = 'ddlUGruop' name ='ddlUGruop'onchange='javascript:updateCommission(this.form);' >"
        Dim ht2 As New Hashtable
        ht2.Add("custid", Sunplus.Agent.Model.Entities.UserSession.Current.CustId)
        Dim rs As DbResult = AccountInfo.Acc_CustomerSelCustomerInfo(ht2)

        str += "<option value='a'>a</option>"
        str += "<option value='b'>b</option>"
        str += "<option value='c'>c</option>"
        str += "<option value='d'>d</option>"
        If rs.ResultSet.Tables(0).Rows.Count > 0 Then
            If rs.ResultSet.Tables(0).Rows(0).Item("site").ToString() = "IBSBET" Then
                str += "<option value='indoa'>indoa</option>"
                str += "<option value='indob'>indob</option>"
                str += "<option value='indoc'>indoc</option>"
                str += "<option value='indod'>indod</option>"
            Else
                str += "<option value='hka'>hka</option>"
                str += "<option value='hkb'>hkb</option>"
                str += "<option value='hkc'>hkc</option>"
                str += "<option value='hkd'>hkd</option>"
            End If
        End If
        str += "</select>"
        Return str
    End Function
#End Region

#Region "Assign Info for Soccer"
    'this function is used to assign data into template for Soccer
    'dr: row contains data get from db
    Function GetDataSoccer(ByVal dr As DataRow) As String
        Dim soccerTpl As String = String.Empty
        Dim headerSTpl As String = String.Empty
        Dim hiddenHash As New Hashtable

        soccerTpl = TemplateHandler.LoadTemplate("MemberInfo/SoccerPT.html")
        headerSTpl = TemplateHandler.LoadTemplate("MemberInfo/SBettypeHeader.html")
        ' assign all text according language
        TemplateHandler.RemoveBlock(soccerTpl, "none-member")
        TemplateHandler.RemoveBlock(soccerTpl, "none-member")
        TemplateHandler.RemoveBlock(soccerTpl, "none-member")
        TemplateHandler.RemoveBlock(soccerTpl, "none-member")
        TemplateHandler.RemoveBlock(soccerTpl, "none-member")
        TemplateHandler.RemoveBlock(soccerTpl, "none-member")
        TemplateHandler.RemoveBlock(soccerTpl, "none-member")
        TemplateHandler.RemoveBlock(soccerTpl, "none-member")
        TemplateHandler.RemoveBlock(soccerTpl, "auto-preset")
        TemplateHandler.RemoveBlock(soccerTpl, "auto-preset")
        TemplateHandler.RemoveBlock(soccerTpl, "none-edit")

        TemplateHandler.Assign(soccerTpl, "bettype-header", LoadSoccerPTHeader(True))

        Dim str As String = "<input type=""checkbox"" id=""chkUpdSoccerPT"" onclick=""AllowUpdate(this,false);"" name=""chkUpdSoccerPT""/>" + Resources.Lang.lblPositionTaking
        If Mult = "0" Then
            str = Resources.Lang.lblPositionTaking
        End If
        TemplateHandler.Assign(soccerTpl, "pt-text", str)

        str = "<input type=""checkbox"" id=""chkUpdSoccerBet"" onclick=""AllowUpdate(this,false);"" name=""chkUpdSoccerBet""/>" + Resources.Lang.lblBetSetting
        If Mult = "0" Then
            str = Resources.Lang.lblBetSetting
        End If
        TemplateHandler.Assign(soccerTpl, "bet-text", str)

        GetTitleCommon(soccerTpl)

        TemplateHandler.Assign(soccerTpl, "deadball-text", Resources.Lang.lblDeadball)
        TemplateHandler.Assign(soccerTpl, "live-text", Resources.Lang.lblLive)
        TemplateHandler.Assign(soccerTpl, "a-preset-live", Resources.Lang.lblMasterPresetPosition)
        TemplateHandler.Assign(soccerTpl, "p-preset-live", Resources.Lang.lblSuperPresetPosition)
        TemplateHandler.Assign(soccerTpl, "auto-super-text", Resources.Lang.lblAutoSuperPresetPosition)
        TemplateHandler.Assign(soccerTpl, "dead-row", "2")
        TemplateHandler.Assign(soccerTpl, "live-row", "1")
        TemplateHandler.Assign(soccerTpl, "copy-live-col", "1")
        TemplateHandler.Assign(soccerTpl, "copy-1x2-text", Resources.Lang.lblCopyFrom1x2)
        TemplateHandler.Assign(soccerTpl, "col-title", 11)
        TemplateHandler.Assign(soccerTpl, "col-auto", 9)

        'assign value of Minbet, Maxbet , ...

        GetLimitMinMaxBet(dr, soccerTpl, "")

        'assign value position taking for template
        Dim mHdp, mOU, m1stHdp, m1stOU, mOE, m1x2, mCS, mTG, mMP, mOR As Double
        Dim mLHdp, mLOU, mL1stHdp, mL1stOU As Double
        Dim mHdpMin, mOUMin, m1stHdpMin, m1stOUMin, mOEMin, m1x2Min, mCSMin, mTGMin, mMPMin, mORMin As Double
        Dim mLHdpMin, mLOUMin, mL1stHdpMin, mL1stOUMin As Double

        Dim mHdpMax, mOUMax, m1stHdpMax, m1stOUMax, mOEMax, m1x2Max, mCSMax, mTGMax, mMPMax, mORMax As Double
        Dim mLHdpMax, mLOUMax, mL1stHdpMax, mL1stOUMax As Double

        mHdp = CastAide.AsDouble(dr("apositiontaking_1"))
        mOU = CastAide.AsDouble(dr("apositiontaking_3"))
        m1stHdp = CastAide.AsDouble(dr("apositiontaking_7"))
        m1stOU = CastAide.AsDouble(dr("apositiontaking_8"))
        mOE = CastAide.AsDouble(dr("apositiontaking_2"))
        m1x2 = CastAide.AsDouble(dr("apositiontaking_5"))
        mCS = CastAide.AsDouble(dr("apositiontaking_4"))
        mTG = CastAide.AsDouble(dr("apositiontaking_6"))
        mMP = CastAide.AsDouble(dr("apositiontaking_9"))
        mOR = CastAide.AsDouble(dr("apositiontaking_10"))
        mLHdp = CastAide.AsDouble(dr("alivepositiontaking_1"))
        mLOU = CastAide.AsDouble(dr("alivepositiontaking_3"))
        mL1stHdp = CastAide.AsDouble(dr("alivepositiontaking_7"))
        mL1stOU = CastAide.AsDouble(dr("alivepositiontaking_8"))

        If Mult = "0" Then ' single
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

            TemplateHandler.Assign(soccerTpl, "minbet1-val", DataFormat.FormatNumber1(dr("minbet")))
            TemplateHandler.Assign(soccerTpl, "maxbet1-val", DataFormat.FormatNumber1(dr("maxbet")))
            TemplateHandler.Assign(soccerTpl, "maxpermatch1-val", DataFormat.FormatNumber1(dr("maxpermatch")))

        Else 'mult
            mHdpMin = CastAide.AsDouble(dr("apositiontaking_1_min"))
            mOUMin = CastAide.AsDouble(dr("apositiontaking_3_min"))
            m1stHdpMin = CastAide.AsDouble(dr("apositiontaking_7_min"))
            m1stOUMin = CastAide.AsDouble(dr("apositiontaking_8_min"))
            mOEMin = CastAide.AsDouble(dr("apositiontaking_2_min"))
            m1x2Min = CastAide.AsDouble(dr("apositiontaking_5_min"))
            mCSMin = CastAide.AsDouble(dr("apositiontaking_4_min"))
            mTGMin = CastAide.AsDouble(dr("apositiontaking_6_min"))
            mMPMin = CastAide.AsDouble(dr("apositiontaking_9_min"))
            mORMin = CastAide.AsDouble(dr("apositiontaking_10_min"))
            mLHdpMin = CastAide.AsDouble(dr("alivepositiontaking_1_min"))
            mLOUMin = CastAide.AsDouble(dr("alivepositiontaking_3_min"))
            mL1stHdpMin = CastAide.AsDouble(dr("alivepositiontaking_7_min"))
            mL1stOUMin = CastAide.AsDouble(dr("alivepositiontaking_8_min"))

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

            TemplateHandler.Assign(soccerTpl, "minbet1-val", 0)
            TemplateHandler.Assign(soccerTpl, "maxbet1-val", 0)
            TemplateHandler.Assign(soccerTpl, "maxpermatch1-val", 0)
        End If

        ''assign Increase and Reduce button for template
        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM1HdpD")
        TemplateHandler.Assign(soccerTpl, "a-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM11x2")
        TemplateHandler.Assign(soccerTpl, "a-1x2-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM1HdpL")
        TemplateHandler.Assign(soccerTpl, "a-Lhdp-btn", btnTpl)

        ''get PT deadball value for master preset PT
        Dim pt As String = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1HdpD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
        TemplateHandler.Assign(soccerTpl, "a-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1OUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOU))
        TemplateHandler.Assign(soccerTpl, "a-ou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM11stHdpD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1stHdpMin, m1stHdpMax, m1stHdp))
        TemplateHandler.Assign(soccerTpl, "a-1sthdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM11stOUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1stOUMin, m1stOUMax, m1stOU))
        TemplateHandler.Assign(soccerTpl, "a-1stou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1OE")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOE))
        TemplateHandler.Assign(soccerTpl, "a-oe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM11x2")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1x2Min, m1x2Max, m1x2))
        TemplateHandler.Assign(soccerTpl, "a-1x2-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1CS")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mCSMin, mCSMax, mCS))
        TemplateHandler.Assign(soccerTpl, "a-cs-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1TG")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mTGMin, mTGMax, mTG))
        TemplateHandler.Assign(soccerTpl, "a-tg-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1MP")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMPMin, mMPMax, mMP))
        TemplateHandler.Assign(soccerTpl, "a-mp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1OR")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mORMin, mORMax, mOR))
        TemplateHandler.Assign(soccerTpl, "a-or-val", pt)

        ''get PT value for super preset PT

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1HdpL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLHdpMin, mLHdpMax, mLHdp))
        TemplateHandler.Assign(soccerTpl, "a-Lhdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1OUL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLOUMin, mLOUMax, mLOU))
        TemplateHandler.Assign(soccerTpl, "a-Lou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM11stHdpL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mL1stHdpMin, mL1stHdpMax, mL1stHdp))
        TemplateHandler.Assign(soccerTpl, "a-L1stHdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM11stOUL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mL1stOUMin, mL1stOUMax, mL1stOU))
        TemplateHandler.Assign(soccerTpl, "a-L1stou-val", pt)



        Return soccerTpl
    End Function
#End Region

#Region "Assign info for  Basketball, Football, Tennis, Golf, OtherSports"
    'this function is used to assign data into template for all sporttype that have 4 bettype
    '(1:Handicap, 3:Over/Under, 2:Odd/Even, 20:Money Line)
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (2: Basketball, 3:Football, 5:Tennis, 10:Golf, 99:OtherSports )
    Function GetData4DDl(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable

        'assign text for position taking
        TemplateHandler.RemoveBlock(tpl, "auto-preset")
        TemplateHandler.RemoveBlock(tpl, "none-edit")
        TemplateHandler.RemoveBlock(tpl, "none-member")
        TemplateHandler.RemoveBlock(tpl, "none-member")
        TemplateHandler.RemoveBlock(tpl, "none-member")

        Dim str As String = "<input type=""checkbox"" id=""chkUpd" + CStr(sportTypeId) + "Bet" + """ onclick=""AllowUpdate(this,false);"" name=""chkUpd" + CStr(sportTypeId) + "Bet" + """/>" + Resources.Lang.lblBetSetting
        If Mult = "0" Then
            str = Resources.Lang.lblBetSetting
        End If
        TemplateHandler.Assign(tpl, "bet-text", str)

        str = "<input type=""checkbox"" id=""chkUpd" + CStr(sportTypeId) + "PT" + """ onclick=""AllowUpdate(this,false);"" name=""chkUpd" + CStr(sportTypeId) + "PT" + """/>" + Resources.Lang.lblPositionTaking
        If Mult = "0" Then
            str = Resources.Lang.lblPositionTaking
        End If
        TemplateHandler.Assign(tpl, "pt-text", str)

        GetTitleCommon(tpl)

        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "oe-text", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(tpl, "mp-text", Resources.Lang.lblMixParlay)
        TemplateHandler.Assign(tpl, "title-col", "5")

        TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId)

        GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

        Dim mHdpMin, mOUMin, mOEMin, mMLMin As Double
        Dim mHdpMax, mOUMax, mOEMax, mMLMax As Double
        Dim mHdp, mOU, mOE, mML As Double
        Dim mMinbet, mMaxbet, mMaxPerMatch_Min, mMaxPerMatch_Max As Double

        If Mult = "0" Then 'single
            mHdpMax = CastAide.AsDouble(dr("maxv" + CStr(sportTypeId) + "pos_1"))
            mOUMax = CastAide.AsDouble(dr("maxv" + CStr(sportTypeId) + "pos_3"))
            mOEMax = CastAide.AsDouble(dr("maxv" + CStr(sportTypeId) + "pos_2"))
            mMLMax = CastAide.AsDouble(dr("maxv" + CStr(sportTypeId) + "pos_20"))

            mHdpMin = CastAide.AsDouble(dr("minv" + CStr(sportTypeId) + "pos_1"))
            mOUMin = CastAide.AsDouble(dr("minv" + CStr(sportTypeId) + "pos_3"))
            mOEMin = CastAide.AsDouble(dr("minv" + CStr(sportTypeId) + "pos_2"))
            mMLMin = CastAide.AsDouble(dr("minv" + CStr(sportTypeId) + "pos_20"))

            mHdp = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_1"))
            mOU = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_3"))
            mOE = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_2"))
            mML = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_20"))

            'assign value for position taking
            TemplateHandler.Assign(tpl, "minbet" & sportTypeId & "-val", DataFormat.FormatNumber1(dr("MinBetv" + CStr(sportTypeId))))
            TemplateHandler.Assign(tpl, "maxbet" & sportTypeId & "-val", DataFormat.FormatNumber1(dr("MaxBetv" + CStr(sportTypeId))))
            TemplateHandler.Assign(tpl, "maxpermatch" & sportTypeId & "-val", DataFormat.FormatNumber1(dr("maxpermatchv" + CStr(sportTypeId))))

        Else 'multi
            mHdp = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_1"))
            mOU = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_3"))
            mOE = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_2"))
            mML = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_20"))

            mHdpMax = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_1"))
            mOUMax = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_3"))
            mOEMax = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_2"))
            mMLMax = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_20"))

            mHdpMin = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_1_min"))
            mOUMin = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_3_min"))
            mOEMin = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_2_min"))
            mMLMin = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_20_min"))

            'assign value for position taking
            TemplateHandler.Assign(tpl, "minbet" & sportTypeId & "-val", 0)
            TemplateHandler.Assign(tpl, "maxbet" & sportTypeId & "-val", 0)
            TemplateHandler.Assign(tpl, "maxpermatch" & sportTypeId & "-val", 0)
        End If

        mMinbet = CastAide.AsDouble(dr("minbet" + "v" & sportTypeId & "_Max"))
        mMaxbet = CastAide.AsDouble(dr("maxbet" + "v" & sportTypeId & "_Min"))
        mMaxPerMatch_Min = CastAide.AsDouble(dr("MaxPerMatch" + "v" & sportTypeId & "_Min"))
        mMaxPerMatch_Max = CastAide.AsDouble(dr("MaxPerMatch" + "v" & sportTypeId & "_Max"))

        'assign data for bet setting
        TemplateHandler.Assign(tpl, "minbet-min-" & sportTypeId, mMinbet)
        TemplateHandler.Assign(tpl, "maxbet-max-" & sportTypeId, mMaxbet)
        TemplateHandler.Assign(tpl, "maxpermatch-min-" & sportTypeId, mMaxPerMatch_Min)
        TemplateHandler.Assign(tpl, "maxpermatch-max-" & sportTypeId, mMaxPerMatch_Max)

        'assign data for Position taking dropdowns
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
        TemplateHandler.Assign(tpl, "a-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOU))
        TemplateHandler.Assign(tpl, "a-ou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OE")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOE))
        TemplateHandler.Assign(tpl, "a-oe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mML))
        TemplateHandler.Assign(tpl, "a-ml-val", pt)

        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)


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
        Dim hiddenHash As New Hashtable

        'assign text for position taking
        tpl = TemplateHandler.LoadTemplate("MemberInfo/BaseballPT.html")

        TemplateHandler.RemoveBlock(tpl, "auto-preset")
        TemplateHandler.RemoveBlock(tpl, "none-edit")
        TemplateHandler.RemoveBlock(tpl, "none-member")
        TemplateHandler.RemoveBlock(tpl, "none-member")
        TemplateHandler.RemoveBlock(tpl, "none-member")
        TemplateHandler.RemoveBlock(tpl, "none-member")

        Dim str As String = "<input type=""checkbox"" id=""chkUpd" + CStr(sportTypeId) + "Bet" + """ onclick=""AllowUpdate(this,false);"" name=""chkUpd" + CStr(sportTypeId) + "Bet" + """/>" + Resources.Lang.lblBetSetting
        If Mult = "0" Then
            str = Resources.Lang.lblBetSetting
        End If
        TemplateHandler.Assign(tpl, "bet-text", str)

        str = "<input type=""checkbox"" id=""chkUpd" + CStr(sportTypeId) + "PT" + """ onclick=""AllowUpdate(this,false);"" name=""chkUpd" + CStr(sportTypeId) + "PT" + """/>" + Resources.Lang.lblPositionTaking
        If Mult = "0" Then
            str = Resources.Lang.lblPositionTaking
        End If
        TemplateHandler.Assign(tpl, "pt-text", str)

        GetTitleCommon(tpl)

        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "oe-text", Resources.Lang.lblOddEven)

        TemplateHandler.Assign(tpl, "copy-hdp-text", Resources.Lang.lblCopyFromHandicap)

        TemplateHandler.Assign(tpl, "title-col", "4")
        TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId)

        GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

        Dim mHdpMax, mOUMax, mMLMax As Double
        Dim mHdp, mOU, mML As Double
        Dim mHdpMin, mOUMin, mMLMin As Double

        Dim mMinbet, mMaxbet, mMaxPerMatch_Min, mMaxPerMatch_Max As Double

        mMinbet = CastAide.AsDouble(dr("minbet" + "v" & sportTypeId & "_Max"))
        mMaxbet = CastAide.AsDouble(dr("maxbet" + "v" & sportTypeId & "_Min"))
        mMaxPerMatch_Min = CastAide.AsDouble(dr("MaxPerMatch" + "v" & sportTypeId & "_Min"))
        mMaxPerMatch_Max = CastAide.AsDouble(dr("MaxPerMatch" + "v" & sportTypeId & "_Max"))

        'assign data for bet setting
        TemplateHandler.Assign(tpl, "minbet-min-" & sportTypeId, mMinbet)
        TemplateHandler.Assign(tpl, "maxbet-max-" & sportTypeId, mMaxbet)
        TemplateHandler.Assign(tpl, "maxpermatch-min-" & sportTypeId, mMaxPerMatch_Min)
        TemplateHandler.Assign(tpl, "maxpermatch-max-" & sportTypeId, mMaxPerMatch_Max)

        If Mult = "0" Then ' single
            mHdpMax = CastAide.AsDouble(dr("maxv" + CStr(sportTypeId) + "pos_1"))
            mOUMax = CastAide.AsDouble(dr("maxv" + CStr(sportTypeId) + "pos_3"))
            mMLMax = CastAide.AsDouble(dr("maxv" + CStr(sportTypeId) + "pos_20"))

            mHdp = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_1"))
            mOU = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_3"))
            mML = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_20"))

            mHdpMin = CastAide.AsDouble(dr("minv" + CStr(sportTypeId) + "pos_1"))
            mOUMin = CastAide.AsDouble(dr("minv" + CStr(sportTypeId) + "pos_3"))
            mMLMin = CastAide.AsDouble(dr("minv" + CStr(sportTypeId) + "pos_20"))

            'assign value for position taking
            TemplateHandler.Assign(tpl, "minbet" & sportTypeId & "-val", DataFormat.FormatNumber1(dr("MinBetv" + CStr(sportTypeId))))
            TemplateHandler.Assign(tpl, "maxbet" & sportTypeId & "-val", DataFormat.FormatNumber1(dr("MaxBetv" + CStr(sportTypeId))))
            TemplateHandler.Assign(tpl, "maxpermatch" & sportTypeId & "-val", DataFormat.FormatNumber1(dr("maxpermatchv" + CStr(sportTypeId))))
        Else ' multi
            mHdpMax = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_1"))
            mOUMax = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_3"))
            mMLMax = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_20"))

            mHdp = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_1"))
            mOU = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_3"))
            mML = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_20"))

            mHdpMin = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_1_min"))
            mOUMin = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_3_min"))
            mMLMin = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_20_min"))

            'assign value for position taking
            TemplateHandler.Assign(tpl, "minbet" & sportTypeId & "-val", 0)
            TemplateHandler.Assign(tpl, "maxbet" & sportTypeId & "-val", 0)
            TemplateHandler.Assign(tpl, "maxpermatch" & sportTypeId & "-val", 0)
        End If

        strHidBaseball = PositionTaking.AssignMulipleHidden(hiddenHash)

        'assign data for Position taking dropdowns
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
        TemplateHandler.Assign(tpl, "a-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOU))
        TemplateHandler.Assign(tpl, "a-ou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mML))
        TemplateHandler.Assign(tpl, "a-ml-val", pt)

        Dim btnTpl As String = groupBtn
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
        Dim hiddenHash As New Hashtable

        'assign text for position taking
        tpl = TemplateHandler.LoadTemplate("MemberInfo/MotorSportsPT.html")
        TemplateHandler.RemoveBlock(tpl, "auto-preset")
        TemplateHandler.RemoveBlock(tpl, "none-edit")
        TemplateHandler.RemoveBlock(tpl, "none-member")
        TemplateHandler.RemoveBlock(tpl, "none-member")
        TemplateHandler.RemoveBlock(tpl, "none-member")
        TemplateHandler.RemoveBlock(tpl, "none-member")

        Dim str As String = "<input type=""checkbox"" id=""chkUpd" + CStr(sportTypeId) + "Bet" + """ onclick=""AllowUpdate(this,false);"" name=""chkUpd" + CStr(sportTypeId) + "Bet" + """/>" + Resources.Lang.lblBetSetting
        If Mult = "0" Then
            str = Resources.Lang.lblBetSetting
        End If
        TemplateHandler.Assign(tpl, "bet-text", str)

        str = "<input type=""checkbox"" id=""chkUpd" + CStr(sportTypeId) + "PT" + """ onclick=""AllowUpdate(this,false);"" name=""chkUpd" + CStr(sportTypeId) + "PT" + """/>" + Resources.Lang.lblPositionTaking
        If Mult = "0" Then
            str = Resources.Lang.lblPositionTaking
        End If
        TemplateHandler.Assign(tpl, "pt-text", str)

        GetTitleCommon(tpl)

        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "oe-text", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(tpl, "copy-hdp-col", "2")
        TemplateHandler.Assign(tpl, "title-col", "3")
        TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId)

        GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())


        'get data and assign for template Position taking
        Dim mHdpMax, mMLMax As Double
        Dim mHdp, mML As Double
        Dim mHdpMin, mMLMin As Double

        Dim mMinbet, mMaxbet, mMaxPerMatch_Min, mMaxPerMatch_Max As Double

        mMinbet = CastAide.AsDouble(dr("minbet" + "v" & sportTypeId & "_Max"))
        mMaxbet = CastAide.AsDouble(dr("maxbet" + "v" & sportTypeId & "_Min"))
        mMaxPerMatch_Min = CastAide.AsDouble(dr("MaxPerMatch" + "v" & sportTypeId & "_Min"))
        mMaxPerMatch_Max = CastAide.AsDouble(dr("MaxPerMatch" + "v" & sportTypeId & "_Max"))

        'assign data for bet setting
        TemplateHandler.Assign(tpl, "minbet-min-" & sportTypeId, mMinbet)
        TemplateHandler.Assign(tpl, "maxbet-max-" & sportTypeId, mMaxbet)
        TemplateHandler.Assign(tpl, "maxpermatch-min-" & sportTypeId, mMaxPerMatch_Min)
        TemplateHandler.Assign(tpl, "maxpermatch-max-" & sportTypeId, mMaxPerMatch_Max)

        If Mult = "0" Then 'single
            mHdpMin = CastAide.AsDouble(dr("minv" + CStr(sportTypeId) + "pos_1"))
            mMLMin = CastAide.AsDouble(dr("minv" + CStr(sportTypeId) + "pos_20"))

            mHdpMax = CastAide.AsDouble(dr("maxv" + CStr(sportTypeId) + "pos_1"))
            mMLMax = CastAide.AsDouble(dr("maxv" + CStr(sportTypeId) + "pos_20"))

            mHdp = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_1"))
            mML = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_20"))

            'assign value for minbet, maxbet, maxpermatch (all are 0 for adding case)
            TemplateHandler.Assign(tpl, "minbet" & sportTypeId & "-val", DataFormat.FormatNumber1(dr("MinBetv" + CStr(sportTypeId))))
            TemplateHandler.Assign(tpl, "maxbet" & sportTypeId & "-val", DataFormat.FormatNumber1(dr("MaxBetv" + CStr(sportTypeId))))
            TemplateHandler.Assign(tpl, "maxpermatch" & sportTypeId & "-val", DataFormat.FormatNumber1(dr("maxpermatchv" + CStr(sportTypeId))))
        Else 'mult
            mHdpMin = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_1_min"))
            mMLMin = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_20_min"))

            mHdpMax = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_1"))
            mMLMax = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_20"))

            mHdp = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_1"))
            mML = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_20"))

            'assign value for minbet, maxbet, maxpermatch (all are 0 for adding case)
            TemplateHandler.Assign(tpl, "minbet" & sportTypeId & "-val", 0)
            TemplateHandler.Assign(tpl, "maxbet" & sportTypeId & "-val", 0)
            TemplateHandler.Assign(tpl, "maxpermatch" & sportTypeId & "-val", 0)
        End If

        strHidMoto = PositionTaking.AssignMulipleHidden(hiddenHash)

        'assign data for Position taking dropdowns
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
        TemplateHandler.Assign(tpl, "a-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mML))
        TemplateHandler.Assign(tpl, "a-ml-val", pt)

        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)

        GetDataMotoSports = tpl
    End Function
#End Region

#Region "Assign info for NumberGame"

    Function GetDataNumberGame(ByVal dr As DataRow) As String
        Dim pt As String
        Dim tpl As String = TemplateHandler.LoadTemplate("MemberInfo/NumberGamePTAddMember.html")

        If UserPermission.IsShowNumberGame Then
            If Mult = "0" Then
                GetMaxPerBallEditValue(tpl, 161, dr)
            Else
                GetMaxPerBallAddValue(tpl, "0"" disabled='true'")
            End If
            GetLimitMaxPerBall(dr, tpl, 161)
        Else
            TemplateHandler.RemoveBlock(tpl, "none-maxperball")
        End If

        Dim hiddenHash As New Hashtable

        'assign text for position taking
        TemplateHandler.RemoveBlock(tpl, "auto-preset")
        TemplateHandler.RemoveBlock(tpl, "none-edit")
        TemplateHandler.RemoveBlock(tpl, "none-member")
        TemplateHandler.RemoveBlock(tpl, "none-member")
        TemplateHandler.RemoveBlock(tpl, "none-member")

        Dim str As String = "<input type=""checkbox"" id=""chkUpd161Bet" + """ onclick=""AllowUpdate(this,false);"" name=""chkUpd161Bet" + """/>" + Resources.Lang.lblBetSetting
        If Mult = "0" Then
            str = Resources.Lang.lblBetSetting
        End If
        TemplateHandler.Assign(tpl, "bet-text", str)

        str = "<input type=""checkbox"" id=""chkUpd161PT" + """ onclick=""AllowUpdate(this,false);"" name=""chkUpd161PT" + """/>" + Resources.Lang.lblPositionTaking
        If Mult = "0" Then
            str = Resources.Lang.lblPositionTaking
        End If
        TemplateHandler.Assign(tpl, "pt-text", str)
        GetTitleMaxperball(tpl, Mult)
        GetTitleCommon(tpl)
        TemplateHandler.Assign(tpl, "live-text", Resources.Lang.lblLive)
        TemplateHandler.Assign(tpl, "copy-1stlastou-text", Resources.Lang.lbl1stcopyfromoverunder)
        TemplateHandler.Assign(tpl, "copy-oulive-text", Resources.Lang.lblcopyfromoverunder)
        TemplateHandler.Assign(tpl, "deadball-text", Resources.Lang.lblDeadball)
        TemplateHandler.Assign(tpl, "overunder-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "oddeven-text", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(tpl, "highlow-text", Resources.Lang.highlow)
        TemplateHandler.Assign(tpl, "col-title", "8")
        TemplateHandler.Assign(tpl, "bettype-header", LoadNumberGameHeader(True))
        GetLimitMinMaxBet(dr, tpl, 161)

        ' insert by david.tran
        TemplateHandler.Assign(tpl, "nextcombo-text", Resources.Lang.combo)
        TemplateHandler.Assign(tpl, "warrior-text", Resources.Lang.warrior)
        TemplateHandler.Assign(tpl, "ng-text", Resources.Lang.numberGroup)

        If (Not UserPermission.IsShowNumberGroup) Then
            TemplateHandler.Assign(tpl, "hide", "hide")
        End If

        Dim m1stOUMin, m1stOEMin, mOEMin, mWRMin As Double
        Dim mOULMin, mOELMin, mHLLMin, mNCLMin, mNGLMin As Double

        Dim m1stOUMax, m1stOEMax, mOEMax, mWRMax As Double
        Dim mOULMax, mOELMax, mHLLMax, mNCLMax, mNGLMax As Double

        Dim m1stOU, m1stOE, mOE, mWR As Double
        Dim mOUL, mOEL, mHLL, mNCL, mNGL As Double

        If Mult = "0" Then 'single
            m1stOUMax = CastAide.AsDouble(dr("maxv161pos_81"))
            m1stOEMax = CastAide.AsDouble(dr("maxv161pos_83"))
            mOEMax = CastAide.AsDouble(dr("maxv161pos_86"))
            mWRMax = CastAide.AsDouble(dr("maxv161pos_88"))

            mOULMax = CastAide.AsDouble(dr("maxv161lpos_85"))
            mOELMax = CastAide.AsDouble(dr("maxv161lpos_86"))
            mHLLMax = CastAide.AsDouble(dr("maxv161lpos_87"))
            mNCLMax = CastAide.AsDouble(dr("maxv161lpos_89"))
            mNGLMax = CastAide.AsDouble(dr("maxv161lpos_90"))

            m1stOUMin = CastAide.AsDouble(dr("minv161pos_81"))
            m1stOEMin = CastAide.AsDouble(dr("minv161pos_83"))
            mOEMin = CastAide.AsDouble(dr("minv161pos_86"))
            mWRMin = CastAide.AsDouble(dr("minv161pos_88"))

            mOULMin = CastAide.AsDouble(dr("minv161lpos_85"))
            mOELMin = CastAide.AsDouble(dr("minv161lpos_86"))
            mHLLMin = CastAide.AsDouble(dr("minv161lpos_87"))
            mNCLMin = CastAide.AsDouble(dr("minv161lpos_89"))
            mNGLMin = CastAide.AsDouble(dr("minv161lpos_90"))

            m1stOU = CastAide.AsDouble(dr("v161apositiontaking_81"))
            m1stOE = CastAide.AsDouble(dr("v161apositiontaking_83"))
            mOE = CastAide.AsDouble(dr("v161apositiontaking_86"))
            mWR = CastAide.AsDouble(dr("v161apositiontaking_88"))

            mOUL = CastAide.AsDouble(dr("v161alivepositiontaking_85"))
            mOEL = CastAide.AsDouble(dr("v161alivepositiontaking_86"))
            mHLL = CastAide.AsDouble(dr("v161alivepositiontaking_87"))
            mNCL = CastAide.AsDouble(dr("v161alivepositiontaking_89"))
            mNGL = CastAide.AsDouble(dr("v161alivepositiontaking_90"))

            'assign value for position taking
            TemplateHandler.Assign(tpl, "minbet161-val", DataFormat.FormatNumber1(dr("MinBetv161")))
            TemplateHandler.Assign(tpl, "maxbet161-val", DataFormat.FormatNumber1(dr("MaxBetv161")))
            TemplateHandler.Assign(tpl, "maxpermatch161-val", DataFormat.FormatNumber1(dr("maxpermatchv161")))

        Else 'multi
            m1stOU = CastAide.AsDouble(dr("v161apositiontaking_81"))
            m1stOE = CastAide.AsDouble(dr("v161apositiontaking_83"))
            mOE = CastAide.AsDouble(dr("v161apositiontaking_86"))
            mWR = CastAide.AsDouble(dr("v161apositiontaking_88"))

            mOUL = CastAide.AsDouble(dr("v161alivepositiontaking_85"))
            mOEL = CastAide.AsDouble(dr("v161alivepositiontaking_86"))
            mHLL = CastAide.AsDouble(dr("v161alivepositiontaking_87"))
            mNCL = CastAide.AsDouble(dr("v161alivepositiontaking_89"))
            mNGL = CastAide.AsDouble(dr("v161alivepositiontaking_90"))

            m1stOUMax = CastAide.AsDouble(dr("v161apositiontaking_81"))
            m1stOEMax = CastAide.AsDouble(dr("v161apositiontaking_83"))
            mOEMax = CastAide.AsDouble(dr("v161apositiontaking_86"))
            mWRMax = CastAide.AsDouble(dr("v161apositiontaking_88"))

            mOULMax = CastAide.AsDouble(dr("v161alivepositiontaking_85"))
            mOELMax = CastAide.AsDouble(dr("v161alivepositiontaking_86"))
            mHLLMax = CastAide.AsDouble(dr("v161alivepositiontaking_87"))
            mNCLMax = CastAide.AsDouble(dr("v161alivepositiontaking_89"))
            mNGLMax = CastAide.AsDouble(dr("v161alivepositiontaking_90"))

            m1stOUMin = CastAide.AsDouble(dr("v161apositiontaking_81_min"))
            m1stOEMin = CastAide.AsDouble(dr("v161apositiontaking_83_min"))
            mOEMin = CastAide.AsDouble(dr("v161apositiontaking_86_min"))
            mWRMin = CastAide.AsDouble(dr("v161apositiontaking_88_min"))

            mOULMin = CastAide.AsDouble(dr("v161alivepositiontaking_85_min"))
            mOELMin = CastAide.AsDouble(dr("v161alivepositiontaking_86_min"))
            mHLLMin = CastAide.AsDouble(dr("v161alivepositiontaking_87_min"))
            mNCLMin = CastAide.AsDouble(dr("v161alivepositiontaking_89_min"))
            mNGLMin = CastAide.AsDouble(dr("v161alivepositiontaking_90_min"))

            'assign value for position taking
            TemplateHandler.Assign(tpl, "minbet161-val", 0)
            TemplateHandler.Assign(tpl, "maxbet161-val", 0)
            TemplateHandler.Assign(tpl, "maxpermatch161-val", 0)
        End If

        'assign data for Position taking dropdowns
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1611stOUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1stOUMin, m1stOUMax, m1stOU))
        TemplateHandler.Assign(tpl, "a-1stlastou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1611stOED")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1stOEMin, m1stOEMax, m1stOE))
        TemplateHandler.Assign(tpl, "a-1stlastoe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161OED")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOE))
        TemplateHandler.Assign(tpl, "a-oe-val", pt)

        ' insert by david.tran
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161WRD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mWRMin, mWRMax, mWR))
        TemplateHandler.Assign(tpl, "a-w-val", pt)

        ''

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161OUL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOULMin, mOULMax, mOUL))
        TemplateHandler.Assign(tpl, "a-Lou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161OEL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOELMin, mOELMax, mOEL))
        TemplateHandler.Assign(tpl, "a-Loe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161HLL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHLLMin, mHLLMax, mHLL))
        TemplateHandler.Assign(tpl, "a-Lhl-val", pt)

        ' insert by david.tran
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161NCL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mNCLMin, mNCLMax, mNCL))
        TemplateHandler.Assign(tpl, "a-Lcb-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161NGL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mNGLMin, mNGLMax, mNGL))
        TemplateHandler.Assign(tpl, "a-ng-val", pt)


        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM1611stOUD")
        TemplateHandler.Assign(tpl, "a-1stlastou-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM161OUL")
        TemplateHandler.Assign(tpl, "a-Lou-btn", btnTpl)

        
        Return tpl
    End Function
#End Region

#Region "Assign info for  MixedSports"

    Function GetDataMixedSports(ByVal dr As DataRow) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable
        Dim tpl As String = TemplateHandler.LoadTemplate("MemberInfo/MixSportsPT.html")

        'assign text for position taking
        TemplateHandler.RemoveBlock(tpl, "auto-preset")
        TemplateHandler.RemoveBlock(tpl, "none-edit")
        TemplateHandler.RemoveBlock(tpl, "none-member")
        TemplateHandler.RemoveBlock(tpl, "none-member")
        TemplateHandler.RemoveBlock(tpl, "none-member")

        Dim str As String = "<input type=""checkbox"" id=""chkUpd99_MBet" + """ onclick=""AllowUpdate(this,false);"" name=""chkUpd99_MBet" + """/>" + Resources.Lang.lblBetSetting
        If Mult = "0" Then
            str = Resources.Lang.lblBetSetting
        End If
        TemplateHandler.Assign(tpl, "bet-text", str)

        str = "<input type=""checkbox"" id=""chkUpd99_MPT" + """ onclick=""AllowUpdate(this,false);"" name=""chkUpd99_MPT" + """/>" + Resources.Lang.lblPositionTaking
        If Mult = "0" Then
            str = Resources.Lang.lblPositionTaking
        End If
        TemplateHandler.Assign(tpl, "pt-text", str)
        GetTitleCommon(tpl)
        TemplateHandler.Assign(tpl, "mp-text", Resources.Lang.lblMixParlay)
        TemplateHandler.Assign(tpl, "title-col", "1")

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
        Dim mMinbet, mMaxbet, mMaxPerMatch_Min, mMaxPerMatch_Max As Double

        If Mult = "0" Then 'single
            mMPMax = CastAide.AsDouble(dr("maxv99pos_9"))
            mMPMin = CastAide.AsDouble(dr("minv99pos_9"))
            mMP = CastAide.AsDouble(dr("v99apositiontaking_9"))

            'assign value for position taking
            TemplateHandler.Assign(tpl, "minbet99_M-val", DataFormat.FormatNumber1(dr("MinBetv99MP")))
            TemplateHandler.Assign(tpl, "maxbet99_M-val", DataFormat.FormatNumber1(dr("MaxBetv99MP")))
            TemplateHandler.Assign(tpl, "maxpermatch99_M-val", DataFormat.FormatNumber1(dr("maxpermatchv99MP")))

        Else 'multi
            mMP = CastAide.AsDouble(dr("v99apositiontaking_9"))
            mMPMax = CastAide.AsDouble(dr("v99apositiontaking_9"))
            mMPMin = CastAide.AsDouble(dr("v99apositiontaking_9_min"))

            'assign value for position taking
            TemplateHandler.Assign(tpl, "minbet99_M-val", 0)
            TemplateHandler.Assign(tpl, "maxbet99_M-val", 0)
            TemplateHandler.Assign(tpl, "maxpermatch99_M-val", 0)
        End If

        ''assign data for Position taking dropdowns
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM99_M")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMPMin, mMPMax, mMP))
        TemplateHandler.Assign(tpl, "a-mp-val", pt)

        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM99_M")
        TemplateHandler.Assign(tpl, "a-mp-btn", btnTpl)


        Return tpl
    End Function
#End Region

#Region "Get Title Common"
    Sub GetTitleCommon(ByRef tpl As String, Optional ByVal sporttype As String = "0")
        TemplateHandler.RemoveBlock(tpl, "auto-preset")
        TemplateHandler.Assign(tpl, "bet-text", Resources.Lang.lblBetSetting)
        TemplateHandler.Assign(tpl, "minbet-text", Resources.Lang.lblMinBet)
        TemplateHandler.Assign(tpl, "maxbet-text", Resources.Lang.lblMaxBet)
        TemplateHandler.Assign(tpl, "maxpermatch-text", Resources.Lang.lblMaxPerMatch)
        TemplateHandler.Assign(tpl, "pt-text", Resources.Lang.lblPositionTaking)
        TemplateHandler.Assign(tpl, "auto-super-text", Resources.Lang.lblAutoSuperPresetPosition)
        TemplateHandler.Assign(tpl, "a-presetpt-text", Resources.Lang.lblMasterPresetPosition)
        TemplateHandler.Assign(tpl, "p-presetpt-text", Resources.Lang.lblSuperPresetPosition)
        TemplateHandler.Assign(tpl, "hdp-text", Resources.Lang.lblHandicapFC)
        TemplateHandler.Assign(tpl, "ml-text", Resources.Lang.moneyline)
        TemplateHandler.Assign(tpl, "copy-hdp-text", Resources.Lang.lblCopyFromHandicap)
        TemplateHandler.Assign(tpl, "copy-oe-text", Resources.Lang.lblCopyFromOE)
        TemplateHandler.Assign(tpl, "copy-hdp-col", "1")
    End Sub
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
        Return strHtml
    End Function

#End Region

    '----------------------------------------------------------------------
#Region "ReplaceData"
    Public Function ReplaceData(ByVal str As String) As String
        str = Server.HtmlDecode(str)
        Return str.Replace("""", "&quot;").Replace("'", "&#39;").Replace(vbNewLine, "")
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

#Region "GetTitleInfo"
    Shared Sub GetTitleMaxperball(ByRef strHtml As String, Optional ByVal isMult As Boolean = False)

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
        TemplateHandler.Assign(strHtml, "{othersetting}", strMaxPerBall + Resources.Lang.othersetting)
        TemplateHandler.Assign(strHtml, "{lblMaxPerBall}", Resources.Lang.maxperball)
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

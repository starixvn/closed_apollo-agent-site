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

Public Class AddNewMember2
    Inherits TplPage

    Protected groupa As String = String.Empty
    Protected groupb As String = String.Empty
    Protected groupc As String = String.Empty
    Protected groupd As String = String.Empty

    Public lminbet, lmaxbet, lminpermatch, lmaxpermatch, lminbetb, lmaxbetb, lminpermatchb, lmaxpermatchb As String

    Dim bodyTpl, strAccInfo, tennisTpl, basketballTpl, footballTpl, otherSportsTpl, golfTpl, groupBtn, selectTpl, strHidSoccer As String
    Dim strHidBaseball, strHidGolf, strHidMoto, strHidOther, strHidBasket, strHidFootball, strHidTennis, strHidVariable As String
    Public script, content As New StringBuilder
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        If Not IsPostBack Then

            Me.CssGroup = "main"
            Dim rs As DbResult
            Dim mainContent As String
            mainContent = TemplateHandler.LoadTemplate("MemberInfo/AddNewMember.html")
            Dim ht As New Hashtable
            ht.Add("custid", Sunplus.Agent.Model.Entities.UserSession.Current.CustId)
            rs = AccountInfo.Acc_CustomerSelAgentInfo(ht)
            If rs.ResultSet.Tables(0).Rows.Count > 0 Then
                Dim dr As DataRow = rs.ResultSet.Tables(0).Rows(0)

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


                strAccInfo = TemplateHandler.LoadTemplate("MemberInfo/AccInfo.html")

                If (Sunplus.Agent.Model.Entities.UserSession.Current.IsInternal) Then
                    TemplateHandler.Assign(strAccInfo, "clickable", "return false")
                Else
                    TemplateHandler.Assign(strAccInfo, "clickable", String.Empty)
                End If

                'If Request("simple") <> "0" Then
                '    TemplateHandler.ReplaceBlock(strAccInfo, "single-mode", "<input class=""btn"" style=""font-size: 8pt;"" type=""button"" value=""Simple Mode"" onclick=""location.href='AddNewMemberSingle.aspx';""/>")
                'End If
                groupBtn = TemplateHandler.LoadTemplate("MemberInfo/BtnGroup.html")
                selectTpl = TemplateHandler.LoadTemplate("MemberInfo/DdlMemPT.html")

                tennisTpl = TemplateHandler.LoadTemplate("MemberInfo/TennisPT.html")
                basketballTpl = TemplateHandler.LoadTemplate("MemberInfo/BasketballPT.html")
                footballTpl = TemplateHandler.LoadTemplate("MemberInfo/FootballPT.html")
                otherSportsTpl = TemplateHandler.LoadTemplate("MemberInfo/OtherSportsPT.html")
                golfTpl = TemplateHandler.LoadTemplate("MemberInfo/GolfPT.html")

                TemplateHandler.RemoveBlock(selectTpl, "none-member")

                Common.GetSportTitle(bodyTpl, False)

                TemplateHandler.Assign(bodyTpl, "add-text", Resources.Lang.btnAdd)
                TemplateHandler.Assign(bodyTpl, "add-id", "btnAdd")
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

                'GetDataMixedSports


                TemplateHandler.Assign(strAccInfo, "add", Resources.Lang.btnAdd)
                TemplateHandler.Assign(strAccInfo, "editmode", Resources.Lang.fullmode)
                TemplateHandler.Assign(strAccInfo, "processing", Resources.Lang.processing)

                TemplateHandler.Assign(strAccInfo, "add-text", Resources.Lang.btnAdd)
                TemplateHandler.Assign(strAccInfo, "add-id", "btnAdd")

                content.Append(AssignDataInfo(dr))
                content.Append(bodyTpl)

                content.Append(strHidVariable)

                content.Append(VariableScript(dr))
                TemplateHandler.Assign(mainContent, "AddNerMemberContent", content.ToString)
                Me.TplSetContent(mainContent)

            End If
        End If

    End Sub


#Region "Declare javascript parameter"
    Function VariableScript(ByVal dr As DataRow) As String
        Dim script As String = ""

        groupa = Format(dr("groupa"), "#,##0.####")
        groupb = Format(dr("groupb"), "#,##0.####")
        groupc = Format(dr("groupc"), "#,##0.####")
        groupd = Format(dr("groupd"), "#,##0.####")

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
        script += " var alertIncorrectCredit='" & Resources.Lang.alertMemberCredit & "';" & vbNewLine
        script += " var alertMaxperMatchNegative ='" & Resources.Lang.lblmaxperballgreaterhanzero & "';" & vbNewLine
        script += " var alertMaxperball='" & Resources.Lang.alertIncorrectMaxPerBall & "';" & vbNewLine
        script += " var alertMaxperballCMax='" & Resources.Lang.lblalertMaxPerBallCompareMax & "';" & vbNewLine
        script += " var alertMaxperballC='" & Resources.Lang.lblalertMaxPerBallCompare & "';" & vbNewLine
        script += " var alertMaxPerMatchLessMaxPerBall='" & Resources.Lang.alertMaxPerMatchLessMaxPerBall & "';" & vbNewLine
        script += " var alertMaxPerBallLessMaxBet='" & Resources.Lang.alertMaxPerBallLessMaxBet & "';" & vbNewLine
        script += " var errRequireValidationCode='" & Resources.Lang.errRequireValidationCode & "';" & vbNewLine

        Dim lblCreditInvalid As String

        lblCreditInvalid = Resources.Lang.lblCreditInvalid.Replace("{0}", Resources.Lang.lblMemberMaxCredit)
        script += " var alertCreditInvalid='" & lblCreditInvalid & "';" & vbNewLine
        script += "  var valGroupa='" + groupa + "';"
        script += "  var valGroupb='" + groupb + "';"
        script += "  var valGroupc='" + groupc + "';"
        script += "  var valGroupd='" + groupd + "';"
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

        TemplateHandler.RemoveBlock(strAccInfo, "none-super")
        TemplateHandler.RemoveBlock(strAccInfo, "none-single")
        TemplateHandler.RemoveBlock(strAccInfo, "none-single")
        TemplateHandler.RemoveBlock(strAccInfo, "none-single")
        TemplateHandler.RemoveBlock(strAccInfo, "none-addmember")
        '
        TemplateHandler.Assign(strAccInfo, "tranfer-time", "")
        TemplateHandler.Assign(strAccInfo, "cols-untext", "4")
        'TemplateHandler.Assign(strAccInfo, "title", Resources.Lang.lblAddNewMember)
        TemplateHandler.Assign(strAccInfo, "max-credit-text", Resources.Lang.givencredit)

        TemplateHandler.Assign(strAccInfo, "fname-val", "")
        TemplateHandler.Assign(strAccInfo, "lname-val", "")
        TemplateHandler.Assign(strAccInfo, "phone-val", "")
        TemplateHandler.Assign(strAccInfo, "mobile-val", "")

        If (AppSettings.IsMasterSite) Then
            TemplateHandler.RemoveBlock(strAccInfo, "for-agent-site")
            TemplateHandler.RemoveBlock(strAccInfo, "for-super-site")
            TemplateHandler.Assign(strAccInfo, "member-max-credit", Resources.Lang.lblMemberMaxCredit)
        ElseIf (AppSettings.IsSuperSite) Then
            TemplateHandler.RemoveBlock(strAccInfo, "for-agent-site")
            TemplateHandler.RemoveBlock(strAccInfo, "for-master-site")
        Else

            'Terry Nguyen modify: Disable Casino and Suspend
            Dim htbDis As Hashtable = New Hashtable()
            htbDis.Add("Suspend", True)

            Dim mytemplate As String = TemplateHandler.LoadTemplate("Memberinfo/DisableStatus.html")
            Dim mycolspan As String = "<td colspan='2'><td>"

            Dim rule1 As HttpFrequenceRule = New HttpFrequenceRule("UpdateNewMember.aspx", 20, TimeSpan.FromDays(2))

            If Not rule1.IsViolated Then
                TemplateHandler.RemoveBlock(mytemplate, "Captchar")
            Else
                mycolspan = String.Empty
            End If

            TemplateHandler.ReplaceBlock(strAccInfo, "Status-Disable", StatusSetting.StatusDisable(mycolspan, htbDis, mytemplate))

            TemplateHandler.Assign(strAccInfo, "suspend-text", Resources.Lang.suspended)

            TemplateHandler.RemoveBlock(strAccInfo, "downline")
            TemplateHandler.RemoveBlock(strAccInfo, "for-super-site")
            TemplateHandler.RemoveBlock(strAccInfo, "for-master-site")
        End If

        TemplateHandler.Assign(strAccInfo, "hdAllowedCurr-value", CastAide.AsInt16(dr("AllowedCurr")))

        TemplateHandler.Assign(strAccInfo, "suspend-text", Resources.Lang.suspended)
        If CastAide.AsBoolean(dr("suspended"), False) Then
            TemplateHandler.Assign(strAccInfo, "suspend-check", "checked='checked' disabled='disabled'")
        Else
            TemplateHandler.Assign(strAccInfo, "suspend-check", "")
        End If

        TemplateHandler.Assign(strAccInfo, "IsSuspended", CastAide.AsInt32(dr("suspended"), 0))
        TemplateHandler.Assign(strAccInfo, "UplineSuspended", CastAide.AsInt32(dr("suspended"), 0))

        GetCredit(dr, strAccInfo)
        TemplateHandler.Assign(strAccInfo, "check-val", "")
        TemplateHandler.Assign(strAccInfo, "fax-val", "")

        Dim discount As Double = CastAide.AsDouble(dr("groupa"))
        Dim discountcs As Double = CastAide.AsDouble(dr("discountcs"))
        Dim discount1x2 As Double = CastAide.AsDouble(dr("discount1x2"))


        TemplateHandler.Assign(strAccInfo, "discount-val", CStr(discount))
        TemplateHandler.Assign(strAccInfo, "discountcs-val", CStr(discountcs))
        TemplateHandler.Assign(strAccInfo, "discount1x2-val", CStr(discount1x2))


        If (Not UserPermission.IsShowNumberGame Or Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission) Then
            TemplateHandler.Assign(strAccInfo, "lblCommissionForAgent_numbergame", String.Empty)
            TemplateHandler.Assign(strAccInfo, "lblCommissionForMember_numbergame", String.Empty)
            TemplateHandler.RemoveBlock(strAccInfo, "none-text-numbergame")
            TemplateHandler.Assign(strAccInfo, "numbergame", String.Empty)
            TemplateHandler.Assign(strAccInfo, "CommissionNumberGame", String.Empty)
            TemplateHandler.Assign(strAccInfo, "hideClss", "hidden")
        Else
            TemplateHandler.Assign(strAccInfo, "lblCommissionForAgent_numbergame", Resources.Lang.lblCommissionForAgent)
            TemplateHandler.Assign(strAccInfo, "lblCommissionForMember_numbergame", Resources.Lang.lblCommissionForMember)
            Dim discountNumberGame As Double = CastAide.AsDouble(dr("discountnumber"))
            TemplateHandler.Assign(strAccInfo, "discountnumber-val", CStr(discountNumberGame))
            TemplateHandler.Assign(strAccInfo, "numbergame", Resources.Lang.numbergame)
            TemplateHandler.Assign(strAccInfo, "CommissionNumberGame", AssignData4Commission(dr, "ddrCommissionNumber"))
            TemplateHandler.Assign(strAccInfo, "hideClss", String.Empty)
        End If



        Dim username_parent As String = Sunplus.Agent.Model.Entities.UserSession.Current.UserName
        Dim sel As String = selectTpl
        TemplateHandler.Assign(sel, "ddl-Id", "number1")
        TemplateHandler.Assign(sel, "ddl-val", GetListNumber(dr("NewUserName"), True)(0))
        TemplateHandler.Assign(strAccInfo, "number1-val", sel)
        sel = selectTpl
        TemplateHandler.Assign(sel, "ddl-Id", "number2")
        TemplateHandler.Assign(sel, "ddl-val", GetListNumber(dr("NewUserName"), True)(1))
        TemplateHandler.Assign(strAccInfo, "number2-val", sel)
        sel = selectTpl
        TemplateHandler.Assign(sel, "ddl-Id", "number3")
        TemplateHandler.Assign(sel, "ddl-val", GetListNumber(dr("NewUserName"), True)(2))
        TemplateHandler.Assign(strAccInfo, "number3-val", sel)
        TemplateHandler.Assign(strAccInfo, "uname-parent", username_parent)

        TemplateHandler.Assign(strAccInfo, "group-text", Resources.Lang.group)
        TemplateHandler.Assign(strAccInfo, "lblCommissionForAgent", Resources.Lang.lblCommissionForAgent)
        TemplateHandler.Assign(strAccInfo, "lblAsianHDC_OU_OE", Resources.Lang.lblAsianHDC_OU_OE)
        TemplateHandler.Assign(strAccInfo, "lbl1x2_CS_TG", Resources.Lang.lblOtherTypes)


        TemplateHandler.Assign(strAccInfo, "lblCommissionForMember", Resources.Lang.lblCommissionForMember)
        TemplateHandler.Assign(strAccInfo, "CommissionH_OU_OE", AssignData4Commission(dr, "ddrCommissionH_OU_OE"))
        TemplateHandler.Assign(strAccInfo, "Commission1x2", AssignData4Commission(dr, "ddrCommission1x2"))
        TemplateHandler.Assign(strAccInfo, "CommissionCS_TG", AssignData4Commission(dr, "ddrCommissionCS_TG"))

        TemplateHandler.Assign(strAccInfo, "UGroup", AssignData4Group())

        GetTitleInfo(strAccInfo)






        Dim hiddenHash As Hashtable = New Hashtable

        hiddenHash.Add("hidden_discount", discount)
        hiddenHash.Add("hidden_discountcs", discountcs)
        hiddenHash.Add("hidden_discount1x2", discount1x2)
        hiddenHash.Add("hidden_uparent", username_parent)
        strHidVariable = PositionTaking.AssignMulipleHidden(hiddenHash)
        Return strAccInfo

    End Function
#End Region

#Region "AssignData4Commission(ByVal dr As DataRow, ByVal ddl As String)"
    Function AssignData4Commission(ByVal dr As DataRow, ByVal ddl As String) As String
        Dim d As Integer = 4
        Dim csm As Decimal = 0
        Dim vstep As Integer = 5
        If (ddl = "ddrCommissionH_OU_OE") Then
            csm = FormatNumber(dr("groupa"), d) * 10000
        ElseIf (ddl = "ddrCommission31") Then
            d = 5
            vstep = 25
            csm = FormatNumber(dr("discountcsUK"), 5) * 10000
        ElseIf (ddl = "ddrCommission32") Then
            d = 5
            vstep = 25
            csm = FormatNumber(dr("discountcsOther"), 5) * 10000
        ElseIf (ddl = "ddrCommission1x2") Then
            d = 5
            vstep = 5
            csm = FormatNumber(dr("discount1x2"), 5) * 10000
        ElseIf (ddl = "ddrCommissionNumber") Then

            d = 5
            vstep = 5
            csm = FormatNumber(dr("discountnumber"), 5) * 10000
        Else
            d = 5
            vstep = 5
            csm = FormatNumber(dr("discountcs"), 5) * 10000
        End If
        Dim str As String = "<SELECT class='selectcomm' id='" & ddl & "' name='" & ddl & "'>"
        Dim index As Integer = 0
        While csm > 0
            'str += "<option value='" + FormatNumber(csm / IIf(d = 4, 10000, 100000), d) + "'" + IIf(index = 0, "selected='true'", "") + " >" + FormatNumber(csm / IIf(d = 4, 10000, 100000), d) + "</option>"
            str += "<option value='" & csm / IIf(d = 4, 10000, 10000) & "'" + IIf(index = 0, "selected='true'", "") + " >" & csm / IIf(d = 4, 10000, 10000) & "</option>"
            csm = csm - vstep
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

        If rs.ResultSet.Tables(0).Rows.Count > 0 Then
            If rs.ResultSet.Tables(0).Rows(0).Item("currency").ToString().ToLower() = "won" Then
                str += "<option value='dea'>dea</option>"
                str += "<option value='deb'>deb</option>"
                str += "<option value='dec'>dec</option>"
                str += "<option value='ded'>ded</option>"
            Else
                str += "<option value='a'>a</option>"
                str += "<option value='b'>b</option>"
                str += "<option value='c'>c</option>"
                str += "<option value='d'>d</option>"

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
        End If
        str += "</select>"
        Return str
    End Function
#End Region

#Region "AssignDataUserName"
    Function AssignDataUserName(ByVal ddl As String, ByVal val As String) As String
        Dim str As String = ""
        str += "<select class='Sel40' id = '" + ddl + "' name ='" + ddl + "'>"
        For i As Integer = 0 To 9
            If (i = CInt(val)) Then
                str += "<option selected='true' value='" + CStr(i) + "'>" + CStr(i) + "</option>"
            Else
                str += "<option value='" + CStr(i) + "'>" + CStr(i) + "</option>"
            End If
        Next
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
        TemplateHandler.RemoveBlock(soccerTpl, "auto-preset")
        TemplateHandler.RemoveBlock(soccerTpl, "none-edit")

        TemplateHandler.Assign(soccerTpl, "bettype-header", LoadSoccerPTHeader(True))

        GetTitleCommon(soccerTpl)
        TemplateHandler.Assign(soccerTpl, "live-text", Resources.Lang.lblLive)
        TemplateHandler.Assign(soccerTpl, "copy-1x2-text", Resources.Lang.lblCopyFrom1x2)
        TemplateHandler.Assign(soccerTpl, "deadball-text", Resources.Lang.lblDeadball)
        TemplateHandler.Assign(soccerTpl, "dead-row", "2")
        TemplateHandler.Assign(soccerTpl, "live-row", "1")
        TemplateHandler.Assign(soccerTpl, "copy-live-col", "1")
        TemplateHandler.Assign(soccerTpl, "col-title", 11)
        TemplateHandler.Assign(soccerTpl, "col-auto", 9)

        'assign value of Minbet, Maxbet , ...
        'TemplateHandler.Assign(soccerTpl, "minbet1-val", "0")
        'TemplateHandler.Assign(soccerTpl, "maxbet1-val", "0")
        'TemplateHandler.Assign(soccerTpl, "maxpermatch1-val", "0")
        GetLimitMinMaxBet(dr, soccerTpl, "")

        'assign value position taking for template
        Dim mHdpMin, mOUMin, m1stHdpMin, m1stOUMin, mOEMin, m1x2Min, mCSMin, mTGMin, mMPMin, mORMin As Double
        Dim mLHdpMin, mLOUMin, mL1stHdpMin, mL1stOUMin As Double

        Dim mHdpMax, mOUMax, m1stHdpMax, m1stOUMax, mOEMax, m1x2Max, mCSMax, mTGMax, mMPMax, mORMax As Double
        Dim mLHdpMax, mLOUMax, mL1stHdpMax, mL1stOUMax As Double

        mHdpMax = CastAide.AsDouble(dr("apositiontaking_1"))
        mOUMax = CastAide.AsDouble(dr("apositiontaking_3"))
        m1stHdpMax = CastAide.AsDouble(dr("apositiontaking_7"))
        m1stOUMax = CastAide.AsDouble(dr("apositiontaking_8"))
        mOEMax = CastAide.AsDouble(dr("apositiontaking_2"))
        m1x2Max = CastAide.AsDouble(dr("apositiontaking_5"))
        mCSMax = CastAide.AsDouble(dr("apositiontaking_4"))
        mTGMax = CastAide.AsDouble(dr("apositiontaking_6"))
        mMPMax = CastAide.AsDouble(dr("apositiontaking_9"))
        mORMax = CastAide.AsDouble(dr("apositiontaking_10"))
        mLHdpMax = CastAide.AsDouble(dr("alivepositiontaking_1"))
        mLOUMax = CastAide.AsDouble(dr("alivepositiontaking_3"))
        mL1stHdpMax = CastAide.AsDouble(dr("alivepositiontaking_7"))
        mL1stOUMax = CastAide.AsDouble(dr("alivepositiontaking_8"))

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
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdpMax))
        TemplateHandler.Assign(soccerTpl, "a-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1OUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOUMin, mOUMax, mOUMax))
        TemplateHandler.Assign(soccerTpl, "a-ou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM11stHdpD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1stHdpMin, m1stHdpMax, m1stHdpMax))
        TemplateHandler.Assign(soccerTpl, "a-1sthdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM11stOUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1stOUMin, m1stOUMax, m1stOUMax))
        TemplateHandler.Assign(soccerTpl, "a-1stou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1OE")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOEMax))
        TemplateHandler.Assign(soccerTpl, "a-oe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM11x2")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1x2Min, m1x2Max, m1x2Max))
        TemplateHandler.Assign(soccerTpl, "a-1x2-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1CS")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mCSMin, mCSMax, mCSMax))
        TemplateHandler.Assign(soccerTpl, "a-cs-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1TG")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mTGMin, mTGMax, mTGMax))
        TemplateHandler.Assign(soccerTpl, "a-tg-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1MP")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMPMin, mMPMax, mMPMax))
        TemplateHandler.Assign(soccerTpl, "a-mp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1OR")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mORMin, mORMax, mORMax))
        TemplateHandler.Assign(soccerTpl, "a-or-val", pt)

        ''get PT value for super preset PT

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


        Return soccerTpl
    End Function
#End Region

#Region "Assign info for  Basketball, Football, Tennis, Golf, OtherSports, MixParlay"
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

        GetTitleCommon(tpl)
        TemplateHandler.Assign(tpl, "hdp-text", Resources.Lang.lblHandicapFC)
        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "oe-text", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(tpl, "mp-text", Resources.Lang.lblMixParlay)
        TemplateHandler.Assign(tpl, "ml-text", Resources.Lang.moneyline)

        TemplateHandler.Assign(tpl, "copy-hdp-col", "2")
        If (sportTypeId = "2") Then
            TemplateHandler.Assign(tpl, "title-col", "6")
        Else
            TemplateHandler.Assign(tpl, "title-col", "5")
        End If
        TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId)

        'GetMinMaxAddValue(tpl, sportTypeId, "0")
        GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

        Dim mHdpMin, mOUMin, mOEMin, mMLMin As Double
        Dim mHdpMax, mOUMax, mOEMax, mMLMax As Double
        Dim mMinbet, mMaxbet, mMaxPerMatch_Min, mMaxPerMatch_Max As Double

        mHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
        mOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3"))
        mOEMax = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2"))

        mMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

        mHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_min"))
        mOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3_min"))
        mOEMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2_min"))

        mMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_min"))

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

        GetTitleCommon(tpl)

        TemplateHandler.Assign(tpl, "hdp-text", Resources.Lang.lblHandicapFC)
        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "oe-text", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(tpl, "ml-text", Resources.Lang.moneyline)

        TemplateHandler.Assign(tpl, "copy-hdp-col", "2")
        TemplateHandler.Assign(tpl, "title-col", "4")
        TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId)

        ' GetMinMaxAddValue(tpl, sportTypeId, "0")
        GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

        Dim mHdpMax, mOUMax, mMLMax As Double
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

        mHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
        mOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3"))
        mMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

        mHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_min"))
        mOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3_min"))
        mMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_min"))


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

        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)


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

        GetTitleCommon(tpl)

        TemplateHandler.Assign(tpl, "hdp-text", Resources.Lang.lblHandicapFC)
        TemplateHandler.Assign(tpl, "ou-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(tpl, "oe-text", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(tpl, "ml-text", Resources.Lang.moneyline)

        TemplateHandler.Assign(tpl, "title-col", "3")
        TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId)

        '  GetMinMaxAddValue(tpl, sportTypeId, "0")
        GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

        'get data and assign for template Position taking
        Dim mHdpMax, mMLMax As Double
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

        mHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
        mMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

        mHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_min"))
        mMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_min"))

        'assign data for Position taking dropdowns
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mHdpMin, mHdpMax, mHdpMax))
        TemplateHandler.Assign(tpl, "a-hdp-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMLMin, mMLMax, mMLMax))
        TemplateHandler.Assign(tpl, "a-ml-val", pt)

        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
        TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

        GetDataMotoSports = tpl
    End Function
#End Region

#Region "Assign Info for NumberGame"

    Function GetDataNumberGame(ByVal dr As DataRow) As String
        Dim numbergameTpl As String = String.Empty
        Dim hiddenHash As New Hashtable

        numbergameTpl = TemplateHandler.LoadTemplate("MemberInfo/NumberGamePTAddMember.html")

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
        TemplateHandler.RemoveBlock(numbergameTpl, "none-member")
        TemplateHandler.RemoveBlock(numbergameTpl, "auto-preset")
        TemplateHandler.RemoveBlock(numbergameTpl, "none-edit")

        TemplateHandler.Assign(numbergameTpl, "bettype-header", LoadNumberGameHeader(True))

        GetTitleCommon(numbergameTpl)
        TemplateHandler.Assign(numbergameTpl, "live-text", Resources.Lang.lblLive)
        TemplateHandler.Assign(numbergameTpl, "copy-1stlastou-text", Resources.Lang.lbl1stcopyfromoverunder)
        TemplateHandler.Assign(numbergameTpl, "copy-oulive-text", Resources.Lang.lblcopyfromoverunder)
        TemplateHandler.Assign(numbergameTpl, "deadball-text", Resources.Lang.lblDeadball)
        TemplateHandler.Assign(numbergameTpl, "col-title", 8)
        TemplateHandler.Assign(numbergameTpl, "overunder-text", Resources.Lang.lblOverUnder)
        TemplateHandler.Assign(numbergameTpl, "oddeven-text", Resources.Lang.lblOddEven)
        TemplateHandler.Assign(numbergameTpl, "highlow-text", Resources.Lang.highlow)
        TemplateHandler.Assign(numbergameTpl, "nextcombo-text", Resources.Lang.combo)
        TemplateHandler.Assign(numbergameTpl, "warrior-text", Resources.Lang.warrior)
        TemplateHandler.Assign(numbergameTpl, "ng-text", Resources.Lang.numberGroup)
        '
        If (Not UserPermission.IsShowNumberGroup) Then
            TemplateHandler.Assign(numbergameTpl, "hide", "hide")
        End If

        GetLimitMinMaxBet(dr, numbergameTpl, 161)

        ''assign value position taking for template
        Dim m1stOUMin, m1stOEMin, mOEMin, mWRMin As Double
        Dim mLOUMin, mLOEMin, mLHLMin, mLNCMin, mLNGMin As Double

        Dim m1stOUMax, m1stOEMax, mOEMax, mWRMax As Double
        Dim mLOUMax, mLOEMax, mLHLMax, mLNCMax, mLNGMax As Double

        m1stOUMax = CastAide.AsDouble(dr("v161apositiontaking_81"))
        m1stOEMax = CastAide.AsDouble(dr("v161apositiontaking_83"))
        mOEMax = CastAide.AsDouble(dr("v161apositiontaking_86"))
        mWRMax = CastAide.AsDouble(dr("v161apositiontaking_88"))

        mLOUMax = CastAide.AsDouble(dr("v161alivepositiontaking_85"))
        mLOEMax = CastAide.AsDouble(dr("v161alivepositiontaking_86"))
        mLHLMax = CastAide.AsDouble(dr("v161alivepositiontaking_87"))
        mLNCMax = CastAide.AsDouble(dr("v161alivepositiontaking_89"))
        mLNGMax = CastAide.AsDouble(dr("v161alivepositiontaking_90"))

        m1stOUMin = CastAide.AsDouble(dr("v161apositiontaking_81_min"))
        m1stOEMin = CastAide.AsDouble(dr("v161apositiontaking_83_min"))
        mOEMin = CastAide.AsDouble(dr("v161apositiontaking_86_min"))
        mWRMin = CastAide.AsDouble(dr("v161apositiontaking_88_min"))

        mLOUMin = CastAide.AsDouble(dr("v161alivepositiontaking_85_min"))
        mLOEMin = CastAide.AsDouble(dr("v161alivepositiontaking_86_min"))
        mLHLMin = CastAide.AsDouble(dr("v161alivepositiontaking_87_min"))
        mLNCMin = CastAide.AsDouble(dr("v161alivepositiontaking_89_min"))
        mLNGMin = CastAide.AsDouble(dr("v161alivepositiontaking_90_min"))


        ''assign Increase and Reduce button for template
        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM1611stOUD")
        TemplateHandler.Assign(numbergameTpl, "a-1stlastou-btn", btnTpl)

        btnTpl = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM161OUL")
        TemplateHandler.Assign(numbergameTpl, "a-Lou-btn", btnTpl)

        ''get PT deadball value for master preset PT
        Dim pt As String = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1611stOUD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1stOUMin, m1stOUMax, m1stOUMax))
        TemplateHandler.Assign(numbergameTpl, "a-1stlastou-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM1611stOED")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(m1stOEMin, m1stOEMax, m1stOEMax))
        TemplateHandler.Assign(numbergameTpl, "a-1stlastoe-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161OED")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mOEMin, mOEMax, mOEMax))
        TemplateHandler.Assign(numbergameTpl, "a-oe-val", pt)

        ' insert by david.tran
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161WRD")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mWRMin, mWRMax, mWRMax))
        TemplateHandler.Assign(numbergameTpl, "a-w-val", pt)

        ''get PT value for super preset PT

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


        ' insert by david.tran
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161NCL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLNCMin, mLNCMax, mLNCMax))
        TemplateHandler.Assign(numbergameTpl, "a-Lcb-val", pt)

        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM161NGL")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mLNGMin, mLNGMax, mLNGMax))
        TemplateHandler.Assign(numbergameTpl, "a-ng-val", pt)

        Return numbergameTpl
    End Function
#End Region

#Region "Assign info for  Mixed Sports"

    Function GetDataMixedSports(ByVal dr As DataRow) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable
        Dim tpl As String = TemplateHandler.LoadTemplate("MemberInfo/MixSportsPT.html")

        'assign text for position taking
        TemplateHandler.RemoveBlock(tpl, "auto-preset")
        TemplateHandler.RemoveBlock(tpl, "none-edit")
        TemplateHandler.RemoveBlock(tpl, "none-member")

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

        TemplateHandler.Assign(tpl, "minbet-min-99_M", NumberFormat.DoubleFormat(minbetMin, 0))
        TemplateHandler.Assign(tpl, "maxbet-max-99_M", NumberFormat.DoubleFormat(maxbetMax, 0))
        TemplateHandler.Assign(tpl, "maxpermatch-min-99_M", NumberFormat.DoubleFormat(maxpermatchMin, 0))
        TemplateHandler.Assign(tpl, "maxpermatch-max-99_M", NumberFormat.DoubleFormat(maxpermatchMax, 0))

        TemplateHandler.Assign(tpl, "minbet99_M-val", NumberFormat.DoubleFormat(minbetMin, 0))
        TemplateHandler.Assign(tpl, "maxbet99_M-val", NumberFormat.DoubleFormat(maxbetMax, 0))
        TemplateHandler.Assign(tpl, "maxpermatch99_M-val", NumberFormat.DoubleFormat(maxpermatchMax, 0))

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


        mMPMax = CastAide.AsDouble(dr("v99apositiontaking_9"))
        mMPMin = CastAide.AsDouble(dr("v99apositiontaking_9_min"))

        'assign data for Position taking dropdowns
        pt = selectTpl
        TemplateHandler.Assign(pt, "ddl-Id", "listM99_M")
        TemplateHandler.Assign(pt, "ddl-val", PositionTaking.GetListPositionTaking(mMPMin, mMPMax, mMPMax))
        TemplateHandler.Assign(tpl, "a-mp-val", pt)

        Dim btnTpl As String = groupBtn
        TemplateHandler.Assign(btnTpl, "ddl-Id", "listM99_M")
        TemplateHandler.Assign(tpl, "a-mp-btn", btnTpl)


        Return tpl
    End Function
#End Region

#Region "Get Title Common"
    Function GetTitleCommon(ByRef tpl As String)
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
    End Function
#End Region

    '-----------------------------------------------------------------------

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
    End Sub
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
                    If index >= arr.Length - 1 Then
                        strNextNo1 = arr(0)
                    Else
                        strNextNo1 = arr(index + 1)
                    End If
                Else
                    index = Array.IndexOf(arr, no2)
                    If index >= arr.Length - 1 Then
                        strNextNo2 = arr(0)
                    Else
                        strNextNo2 = arr(index + 1)
                    End If
                End If
            Else
                index = Array.IndexOf(arr, no3)
                strNextNo3 = arr(index + 1)
            End If
        Else
            If no2 = arr(arr.Length - 1) Then
                strNextNo2 = arr(0)
                index = Array.IndexOf(arr, no1)
                strNextNo1 = arr(index + 1)
            Else
                index = Array.IndexOf(arr, no2)
                strNextNo2 = arr(index + 1)
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
        TemplateHandler.Assign(tpl, "minbet-min-" & sportTypeId & "", FormatHandler.FormatNumber2(minbetMin, Threading.Thread.CurrentThread.CurrentCulture.ToString()))
        TemplateHandler.Assign(tpl, "maxbet-max-" & sportTypeId & "", FormatHandler.FormatNumber2(maxbetMax, Threading.Thread.CurrentThread.CurrentCulture.ToString()))
        TemplateHandler.Assign(tpl, "maxpermatch-min-" & sportTypeId & "", FormatHandler.FormatNumber2(maxpermatchMin, Threading.Thread.CurrentThread.CurrentCulture.ToString()))
        TemplateHandler.Assign(tpl, "maxpermatch-max-" & sportTypeId & "", FormatHandler.FormatNumber2(maxpermatchMax, Threading.Thread.CurrentThread.CurrentCulture.ToString()))

        TemplateHandler.Assign(tpl, "minbet" & sportTypeId & "-val", FormatHandler.FormatNumber2(minbetMin, Threading.Thread.CurrentThread.CurrentCulture.ToString()))
        TemplateHandler.Assign(tpl, "maxbet" & sportTypeId & "-val", FormatHandler.FormatNumber2(maxbetMax, Threading.Thread.CurrentThread.CurrentCulture.ToString()))
        TemplateHandler.Assign(tpl, "maxpermatch" & sportTypeId & "-val", FormatHandler.FormatNumber2(maxpermatchMax, Threading.Thread.CurrentThread.CurrentCulture.ToString()))

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
    Shared Sub GetMinMaxAddValue(ByRef tpl As String, ByVal sportTypeId As Integer, ByVal strReplace As String)
        TemplateHandler.Assign(tpl, "minbet" & sportTypeId & "-val", strReplace)
        TemplateHandler.Assign(tpl, "maxbet" & sportTypeId & "-val", strReplace)
        TemplateHandler.Assign(tpl, "maxpermatch" & sportTypeId & "-val", strReplace)
    End Sub
#End Region




End Class

Imports Fanex.Data.DNAide
Imports DNAide.Core
Imports DNAide.Web
Imports DNAide.Utils
Imports System.Data
Imports DataHandler
Imports System.Web.Security
Imports System.Collections
Imports Sunplus.Agent.Model.Entities.Permission
Public Class UpdateMasterEditMember
    Inherits IbcPage2
    Dim param As New Hashtable
    Dim isCopyAll As Boolean
    Dim isCopyBasket As Boolean
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

        Dim errMsg As String = ""
        If (Not Sunplus.Agent.Model.Entities.UserSession.Current.SubPageAccess.UpdateAccount) Then
            errMsg = Resources.Lang.lblNotAuthorized
            Dim customErrMsg As String = ""
            CommonGenerateHTML.GenerateErrorMsg(ReplaceData(errMsg), customErrMsg)
            Response.Write("<script type=""text/javascript"">parent.showErrMsg('" & customErrMsg & "');</script>")
            Return
        ElseIf Sunplus.Agent.Model.Entities.UserSession.Current.IsInternal Then
            errMsg = Resources.Lang.lblNotAuthorized
            Dim customErrMsg As String = ""
            CommonGenerateHTML.GenerateErrorMsg(ReplaceData(errMsg), customErrMsg)
            Response.Write("<script type=""text/javascript"">parent.UpdateErr('" & customErrMsg & "');</script>")
            Return
        Else
            UpdateData()
        End If


    End Sub
#Region "Update Data"
    Sub UpdateData()

        Dim _errcode As String = ""
        Dim _errmsg As String = ""
        Dim _username As String = ""

        Dim rs As DbResult
        Dim arCID() As String = Request("saveCustid").Split("^")
        If Not Request("chkCopyAll") Is Nothing Then
            isCopyAll = True
        End If
        If Not Request("chkCopyBasket") Is Nothing Then
            isCopyBasket = True
        End If
        Dim isMult As Boolean = False
        isMult = IIf(Request("isMult") Is Nothing, 0, CBool(Request("isMult")))
        For i As Integer = 0 To arCID.Length - 1 Step 1
            If (arCID(i) <> "") Then
                param.Clear()
                param.Add("custid", arCID(i)) 'int  

                If Not Request("chkUpdStatus") Is Nothing Or Not isMult Then
                    param.Add("updateclosed", 1)
                    If Not Request("chkClosed") Is Nothing Or (Request("IsClosed") = 1 And Request("UplineClosed") = 1) Then
                        param.Add("closed", 1) 'bit  
                    Else
                        param.Add("closed", 0) 'bit  
                    End If
                Else
                    param.Add("updateclosed", 0)
                    param.Add("closed", 0) 'bit  
                End If


                If Not isMult Then
                    param.Add("updatesuspended", 1)
                    If Not Request("chkSuspended") Is Nothing Or (Request("IsSuspended") = 1 And Request("UplineSuspended") = 1) Then
                        param.Add("suspended", 1) 'bit 
                    Else
                        param.Add("suspended", 0) 'bit 
                    End If
                Else
                    param.Add("updatesuspended", 0)
                    param.Add("suspended", 0) 'bit  
                End If
                param.Add("discasino", Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledCasino) 'int

                param.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId)
                param.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey)
                param.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId)
                param.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP())
                param.Add("returnerrcode", 1) 'bit  
                param.Add("errcode", "") 'int  
                param.Add("errmsg", "") 'nvarchar  
                param.Add("username", "")
                param.Add("updatemultiple", isMult)

                AddParaSoccer(isMult)
                AddParaFootball(2, isMult)
                AddParaFootball(3, isMult)
                AddParaFootball(5, isMult)
                AddParaBaseball(isMult)
                AddParaFootball(10, isMult)
                AddParaMotosports(isMult)
                AddParaFootball(99, isMult)
                AddParaNumberGame(161, isMult)
                AddParaMixedSport(isMult)

                rs = AccountInfo.Acc_CustomerUpdMaster_Member(param)
                If (isMult) Then
                    If rs.Outputs("errcode") <> "" Then
                        _errcode = _errcode & rs.Outputs("errcode").ToString().Substring(0, rs.Outputs("errcode").ToString().Length - 1) & "$"
                        _errmsg = _errmsg & rs.Outputs("errmsg").ToString().Substring(0, rs.Outputs("errmsg").ToString().Length - 1) & "$"
                        _username = _username & rs.Outputs("username") & "^"
                    End If

                Else
                    _errcode = rs.Outputs("errcode")
                    _errmsg = rs.Outputs("errmsg")
                End If
            End If
        Next
        Dim strErrmsg As String = ""
        Dim CallAPI As Boolean = False
        Dim res As Integer = CommonGenerateHTML.GenerateErrorMsg(_errcode, PositionTaking.ReplaceData(_errmsg), strErrmsg, _username, CallAPI)
        If CallAPI Then
            If (UserPermission.IsShowCasino) Then

                Dim strModifyBy As String = Sunplus.Agent.Model.Entities.UserSession.Current.UserName
                If Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId <> 0 Then
                    strModifyBy = Sunplus.Agent.Model.Entities.UserSession.Current.SubUserName
                End If

                ' Sync data to Casino
                Dim csCustomer As CasinoLibrary.SBAgent.Customer = New CasinoLibrary.SBAgent.Customer()
                Dim status As Boolean = False



                If (Not isMult And Request("hidSynCS") = "True") Then
                    ' Store data for casino section
                    status = csCustomer.UpdateCustClosed(CInt(param("custid")), CBool(param("closed")), Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), Sunplus.Agent.Model.Entities.UserSession.Current.CustId, strModifyBy, Sunplus.Agent.Model.BL.Casino.CasinoBO.AGENT_SYSTEM)
                    status = csCustomer.UpdateCustSuspend(CInt(param("custid")), CBool(param("suspended")), Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), Sunplus.Agent.Model.Entities.UserSession.Current.CustId, strModifyBy, Sunplus.Agent.Model.BL.Casino.CasinoBO.AGENT_SYSTEM)
                Else

                    If Not Request("chkUpdStatus") Is Nothing Then
                        Dim isClosed As Boolean = False
                        If Not Request("chkClosed") Is Nothing Then
                            isClosed = True 'bit 
                        Else
                            isClosed = False 'bit 
                        End If

                        For i As Integer = 0 To arCID.Length - 2 Step 1
                            If (New Sunplus.Agent.Model.BL.Casino.CasinoBO(Session.SessionID).IsSynchronized(CInt(arCID(i)))) Then
                                status = csCustomer.UpdateCustClosed(CInt(arCID(i)), isClosed, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), Sunplus.Agent.Model.Entities.UserSession.Current.CustId, strModifyBy, 2)
                            End If
                        Next
                    End If

                End If

            End If
        End If

        If res <> 0 Then

            Response.Write("<script type=""text/javascript"">parent.showErrMsg('" & strErrmsg & "');</script>")
        Else
            Dim hash As Hashtable = AppSession.RequestQueryString
            Dim param As String = String.Empty
           
            

            Dim FromAccount As String
            FromAccount = IIf(hash.ContainsKey("FromAccount"), hash("FromAccount"), "")
            If (String.IsNullOrEmpty(FromAccount)) Then
                param += IIf(hash.ContainsKey("ParentForm"), hash("ParentForm"), "../PositionTakingList/Sportbook/SportbookPositionTakingList.aspx")
                param += "?type=agent"
                param += "&custid=" + Request("saveAgentid")
                Response.Write("<script type='text/javascript'>top.main.DelayReloadPage('" + param + "');</script>")
            Else 'from AccountTab
                param += "../_MemberInfo/CustomerList/Master/MemberList.aspx"
                param += "?custid=" + Request("saveAgentid")
                Response.Write("<script type=""text/javascript"">parent.top.menu.reloadMenuUsrInfo(); top.main.DelayReloadPage('" + param + "');</script>")
            End If
        End If


    End Sub
#End Region

#Region "Insert parameters for Soccer Position Taking"
    'this function is used to add parameters
    'in Single Edit case get all parameters for Bet Setting and Positiontaking
    'in Multiple Edit case: 
    '       if Bet Setting checkbox is checked , value is updated db that is requested from Form 
    '       if Bet Setting checkbox is unchecked , value is updated db that is 0
    '       if Position Taking checkbox is checked , value is updated db that is requested from Form 
    '       if Position Taking checkbox is unchecked , value is updated db that is 0
    Sub AddParaSoccer(ByVal isMult As Boolean)
        '***** Start Inserting for Soccer *****

        Dim mHdpD, mOUD, m1stHdpD, m1stOUD, mOE, m1x2, mCS, mTG, mMp, mOR As Double
        Dim mHdpL, mOUL, m1stHdpL, m1stOUL As Double

        Dim autoHdpD, autoOUD, auto1stHdpD, auto1stOUD, autoOE, auto1x2, autoCS, autoTG, autoMp, autoOR As Double
        Dim isAutoHdpD, isAutoOUD, isAuto1stHdpD, isAuto1stOUD, isAutoOE, isAuto1x2, isAutoCS, isAutoTG, isAutoMp, isAutoOR As Boolean
        Dim autoHdpL, autoOUL, auto1stHdpL, auto1stOUL As Double
        Dim isAutoHdpL, isAutoOUL, isAuto1stHdpL, isAuto1stOUL As Boolean

        'Assign value for isAuto
        isAutoHdpD = IIf(Request("chkAuto1HdpD") Is Nothing, 0, 1)
        isAutoOUD = IIf(Request("chkAuto1OUD") Is Nothing, 0, 1)
        isAuto1stHdpD = IIf(Request("chkAuto11stHdpD") Is Nothing, 0, 1)
        isAuto1stOUD = IIf(Request("chkAuto11stOUD") Is Nothing, 0, 1)
        isAutoOE = IIf(Request("chkAuto1OE") Is Nothing, 0, 1)

        isAuto1x2 = IIf(Request("chkAuto11x2") Is Nothing, 0, 1)
        isAutoCS = IIf(Request("chkAuto1CS") Is Nothing, 0, 1)
        isAutoTG = IIf(Request("chkAuto1TG") Is Nothing, 0, 1)
        isAutoMp = IIf(Request("chkAuto1MP") Is Nothing, 0, 1)
        isAutoOR = IIf(Request("chkAuto1OR") Is Nothing, 0, 1)

        isAutoHdpL = IIf(Request("chkAuto1HdpL") Is Nothing, 0, 1)
        isAutoOUL = IIf(Request("chkAuto1OUL") Is Nothing, 0, 1)
        isAuto1stHdpL = IIf(Request("chkAuto11stHdpL") Is Nothing, 0, 1)
        isAuto1stOUL = IIf(Request("chkAuto11stOUL") Is Nothing, 0, 1)
        '----
       
        'Updating Positon Taking in Single Edit or Allow update in Multiple Edit
        Dim isCopyHdpD As Boolean = False
        If Not Request("chkCopy1HdpDeadball") Is Nothing Then
            isCopyHdpD = True

            isAutoOUD = isAutoHdpD
            isAuto1stHdpD = isAutoHdpD
            isAuto1stOUD = isAutoHdpD
            isAutoOE = isAutoHdpD
        End If
        Dim isCopy1x2 As Boolean = False
        If Not Request("chkCopy11x2") Is Nothing Then
            isCopy1x2 = True

            isAutoCS = isAuto1x2
            isAutoTG = isAuto1x2
            isAutoMp = isAuto1x2
            isAutoOR = isAuto1x2
        End If
        Dim isCopyHdpL As Boolean = False
        If Not Request("chkCopy1HdpLive") Is Nothing Then
            isCopyHdpL = True

            isAutoOUL = isAutoHdpL
            isAuto1stHdpL = isAutoHdpL
            isAuto1stOUL = isAutoHdpL
        End If
        If Not isCopyAll Then 'this case for Copy All PT checkbox is unchecked              
            mHdpD = IIf(isCopyHdpD, IIf(Request("isAutoHdp"), 0, Request("listM1HdpD")), Request("listM1HdpD"))
            mOUD = IIf(isCopyHdpD, IIf(Request("isAutoOU"), 0, Request("listM1HdpD")), Request("listM1OUD"))
            m1stHdpD = IIf(isCopyHdpD, IIf(Request("isAuto1stHdp"), 0, Request("listM1HdpD")), Request("listM11stHdpD"))
            m1stOUD = IIf(isCopyHdpD, IIf(Request("isAuto1stOU"), 0, Request("listM1HdpD")), Request("listM11stOUD"))
            mOE = IIf(isCopyHdpD, IIf(Request("isAutoOE"), 0, Request("listM1HdpD")), Request("listM1OE"))

            m1x2 = IIf(isCopy1x2, IIf(Request("isAuto1x2"), 0, Request("listM11x2")), Request("listM11x2"))
            mCS = IIf(isCopy1x2, IIf(Request("isAutoCS"), 0, Request("listM11x2")), Request("listM1CS"))
            mTG = IIf(isCopy1x2, IIf(Request("isAutoTG"), 0, Request("listM11x2")), Request("listM1TG"))
            mMp = IIf(isCopy1x2, IIf(Request("isAutoMP"), 0, Request("listM11x2")), Request("listM1MP"))
            mOR = IIf(isCopy1x2, IIf(Request("isAutoOR"), 0, Request("listM11x2")), Request("listM1OR"))

            mHdpL = IIf(isCopyHdpL, IIf(Request("isAutoLHdp"), 0, Request("listM1HdpL")), Request("listM1HdpL"))
            mOUL = IIf(isCopyHdpL, IIf(Request("isAutoLOU"), 0, Request("listM1HdpL")), Request("listM1OUL"))
            m1stHdpL = IIf(isCopyHdpL, IIf(Request("isAutoL1stHdp"), 0, Request("listM1HdpL")), Request("listM11stHdpL"))
            m1stOUL = IIf(isCopyHdpL, IIf(Request("isAutoL1stOU"), 0, Request("listM1HdpL")), Request("listM11stOUL"))

            autoHdpD = IIf(isAutoHdpD, IIf(isCopyHdpD, Request("autoM1HdpD"), Request("autoM1HdpD")), 0)
            autoOUD = IIf(isAutoOUD, IIf(isCopyHdpD, Request("autoM1HdpD"), Request("autoM1OUD")), 0)
            auto1stHdpD = IIf(isAuto1stHdpD, IIf(isCopyHdpD, Request("autoM1HdpD"), Request("autoM11stHdpD")), 0)
            auto1stOUD = IIf(isAuto1stOUD, IIf(isCopyHdpD, Request("autoM1HdpD"), Request("autoM11stOUD")), 0)
            autoOE = IIf(isAutoOE, IIf(isCopyHdpD, Request("autoM1HdpD"), Request("autoM1OE")), 0)

            auto1x2 = IIf(isAuto1x2, IIf(isCopy1x2, Request("autoM11x2"), Request("autoM11x2")), 0)
            autoCS = IIf(isAutoCS, IIf(isCopy1x2, Request("autoM11x2"), Request("autoM1CS")), 0)
            autoTG = IIf(isAutoTG, IIf(isCopy1x2, Request("autoM11x2"), Request("autoM1TG")), 0)
            autoMp = IIf(isAutoMp, IIf(isCopy1x2, Request("autoM11x2"), Request("autoM1MP")), 0)
            autoOR = IIf(isAutoOR, IIf(isCopy1x2, Request("autoM11x2"), Request("autoM1OR")), 0)

            autoHdpL = IIf(isAutoHdpL, IIf(isCopyHdpL, Request("autoM1HdpL"), Request("autoM1HdpL")), 0)
            autoOUL = IIf(isAutoOUL, IIf(isCopyHdpL, Request("autoM1HdpL"), Request("autoM1OUL")), 0)
            auto1stHdpL = IIf(isAuto1stHdpL, IIf(isCopyHdpL, Request("autoM1HdpL"), Request("autoM11stHdpL")), 0)
            auto1stOUL = IIf(isAuto1stOUL, IIf(isCopyHdpL, Request("autoM1HdpL"), Request("autoM11stOUL")), 0)
        Else
            Dim isAutoSoccer As Boolean = False
            If Not Request("chkAuto1HdpD") Is Nothing Then
                isAutoSoccer = True
            End If

            mHdpD = IIf(Request("isAutoHdp"), 0, Request("listM1HdpD"))
            mOUD = IIf(Request("isAutoOU"), 0, Request("listM1HdpD"))
            m1stHdpD = IIf(Request("isAuto1stHdp"), 0, Request("listM1HdpD"))
            m1stOUD = IIf(Request("isAuto1stOU"), 0, Request("listM1HdpD"))
            mOE = IIf(Request("isAutoOE"), 0, Request("listM1HdpD"))

            m1x2 = IIf(Request("isAuto1x2"), 0, Request("listM1HdpD"))
            mCS = IIf(Request("isAutoCS"), 0, Request("listM1HdpD"))
            mTG = IIf(Request("isAutoTG"), 0, Request("listM1HdpD"))
            mMp = IIf(Request("isAutoMP"), 0, Request("listM1HdpD"))
            mOR = IIf(Request("isAutoOR"), 0, Request("listM1HdpD"))

            mHdpL = IIf(Request("isAutoLHdp"), 0, Request("listM1HdpD"))
            mOUL = IIf(Request("isAutoLOU"), 0, Request("listM1HdpD"))
            m1stHdpL = IIf(Request("isAutoL1stHdp"), 0, Request("listM1HdpD"))
            m1stOUL = IIf(Request("isAutoL1stOU"), 0, Request("listM1HdpD"))

            autoHdpD = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
            autoOUD = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
            auto1stHdpD = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
            auto1stOUD = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
            autoOE = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)

            auto1x2 = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
            autoCS = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
            autoTG = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
            autoMp = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
            autoOR = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)

            autoHdpL = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
            autoOUL = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
            auto1stHdpL = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
            auto1stOUL = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)

            isAutoHdpD = CInt(isAutoSoccer)
            isAutoOUD = CInt(isAutoSoccer)
            isAuto1stHdpD = CInt(isAutoSoccer)
            isAuto1stOUD = CInt(isAutoSoccer)
            isAutoOE = CInt(isAutoSoccer)

            isAuto1x2 = CInt(isAutoSoccer)
            isAutoCS = CInt(isAutoSoccer)
            isAutoTG = CInt(isAutoSoccer)
            isAutoMp = CInt(isAutoSoccer)
            isAutoOR = CInt(isAutoSoccer)

            isAutoHdpL = CInt(isAutoSoccer)
            isAutoOUL = CInt(isAutoSoccer)
            isAuto1stHdpL = CInt(isAutoSoccer)
            isAuto1stOUL = CInt(isAutoSoccer)
        End If

        If isMult Then
            param.Add("updatepositiontaking", IIf(Request("chkUpd_1") Is Nothing, 0, 1))
        End If

        param.Add("mpositiontaking_1", mHdpD) 'smallmoney  
        param.Add("mpositiontaking_3", mOUD) 'smallmoney  
        param.Add("mpositiontaking_7", m1stHdpD) 'smallmoney  
        param.Add("mpositiontaking_8", m1stOUD) 'smallmoney  
        param.Add("mpositiontaking_2", mOE) 'smallmoney  

        param.Add("mpositiontaking_5", m1x2) 'smallmoney  
        param.Add("mpositiontaking_4", mCS) 'smallmoney 
        param.Add("mpositiontaking_6", mTG) 'smallmoney             
        param.Add("mpositiontaking_9", mMp) 'smallmoney  
        param.Add("mpositiontaking_10", mOR) 'smallmoney  

        param.Add("mlivepositiontaking_1", mHdpL) 'smallmoney  
        param.Add("mlivepositiontaking_3", mOUL) 'smallmoney  
        param.Add("mlivepositiontaking_7", m1stHdpL) 'smallmoney  
        param.Add("mlivepositiontaking_8", m1stOUL) 'smallmoney  

        param.Add("isauto_mpositiontaking_1", Convert.ToInt16(Request("isAutoHdp"))) 'bit  
        param.Add("isauto_mpositiontaking_3", Convert.ToInt16(Request("isAutoOU"))) 'bit 
        param.Add("isauto_mpositiontaking_7", Convert.ToInt16(Request("isAuto1stHdp"))) 'bit  
        param.Add("isauto_mpositiontaking_8", Convert.ToInt16(Request("isAuto1stOUD"))) 'bit  
        param.Add("isauto_mpositiontaking_2", Convert.ToInt16(Request("isAutoOE"))) 'bit  

        param.Add("isauto_mpositiontaking_5", Convert.ToInt16(Request("isAuto1x2"))) 'bit 
        param.Add("isauto_mpositiontaking_4", Convert.ToInt16(Request("isAutoCS"))) 'bit  
        param.Add("isauto_mpositiontaking_6", Convert.ToInt16(Request("isAutoTG"))) 'bit        
        param.Add("isauto_mpositiontaking_9", Convert.ToInt16(Request("isAutoMP"))) 'bit  
        param.Add("isauto_mpositiontaking_10", Convert.ToInt16(Request("isAutoOR"))) 'bit  

        param.Add("isauto_mlivepositiontaking_1", Convert.ToInt16(Request("isAutoLHdp"))) 'bit  
        param.Add("isauto_mlivepositiontaking_3", Convert.ToInt16(Request("isAutoLOU"))) 'bit  
        param.Add("isauto_mlivepositiontaking_7", Convert.ToInt16(Request("isAutoL1stHdp"))) 'bit  
        param.Add("isauto_mlivepositiontaking_8", Convert.ToInt16(Request("isAutoL1stOU"))) 'bit 

        ' Auto master on member
        param.Add("auto_mpositiontaking_mem_1", autoHdpD) 'smallmoney  
        param.Add("auto_mpositiontaking_mem_3", autoOUD) 'smallmoney  
        param.Add("auto_mpositiontaking_mem_7", auto1stHdpD) 'smallmoney  
        param.Add("auto_mpositiontaking_mem_8", auto1stOUD) 'smallmoney  
        param.Add("auto_mpositiontaking_mem_2", autoOE) 'smallmoney  

        param.Add("auto_mpositiontaking_mem_5", auto1x2) 'smallmoney  
        param.Add("auto_mpositiontaking_mem_4", autoCS) 'smallmoney  
        param.Add("auto_mpositiontaking_mem_6", autoTG) 'smallmoney              
        param.Add("auto_mpositiontaking_mem_9", autoMp) 'smallmoney  
        param.Add("auto_mpositiontaking_mem_10", autoOR) 'smallmoney 

        param.Add("auto_mlivepositiontaking_mem_1", autoHdpL) 'smallmoney  
        param.Add("auto_mlivepositiontaking_mem_3", autoOUL) 'smallmoney  
        param.Add("auto_mlivepositiontaking_mem_7", auto1stHdpL) 'smallmoney  
        param.Add("auto_mlivepositiontaking_mem_8", auto1stOUL) 'smallmoney            

        param.Add("isauto_mpositiontaking_mem_1", isAutoHdpD) 'bit  
        param.Add("isauto_mpositiontaking_mem_3", isAutoOUD) 'bit 
        param.Add("isauto_mpositiontaking_mem_7", isAuto1stHdpD) 'bit  
        param.Add("isauto_mpositiontaking_mem_8", isAuto1stOUD) 'bit  
        param.Add("isauto_mpositiontaking_mem_2", isAutoOE) 'bit  

        param.Add("isauto_mpositiontaking_mem_5", isAuto1x2) 'bit 
        param.Add("isauto_mpositiontaking_mem_4", isAutoCS) 'bit  
        param.Add("isauto_mpositiontaking_mem_6", isAutoTG) 'bit        
        param.Add("isauto_mpositiontaking_mem_9", isAutoMp) 'bit  
        param.Add("isauto_mpositiontaking_mem_10", isAutoOR) 'bit  

        param.Add("isauto_mlivepositiontaking_mem_1", isAutoHdpL) 'bit  
        param.Add("isauto_mlivepositiontaking_mem_3", isAutoOUL) 'bit  
        param.Add("isauto_mlivepositiontaking_mem_7", isAuto1stHdpL) 'bit  
        param.Add("isauto_mlivepositiontaking_mem_8", isAuto1stOUL) 'bit 

    End Sub
#End Region

#Region "Insert parameters for Basketball, Football, Tennis, Golf, Other Sports Position Taking"
    Sub AddParaFootball(ByVal sportId As String, ByVal isMult As Boolean)

        '***** Start Inserting for Football *****
        Dim mHdp, mOU, mOE, mML As Double
        Dim autoHdp, autoOU, autoOE, autoML As Double
        Dim isAutoHdp, isAutoOU, isAutoOE, isAutoML As Double

        'Assign IsAuto value
        isAutoHdp = IIf(Request("chkAuto" & sportId & "Hdp") Is Nothing, 0, 1)
        isAutoOU = IIf(Request("chkAuto" & sportId & "OU") Is Nothing, 0, 1)
        isAutoOE = IIf(Request("chkAuto" & sportId & "OE") Is Nothing, 0, 1)
        isAutoML = IIf(Request("chkAuto" & sportId & "ML") Is Nothing, 0, 1)
        '----

        Dim isCopyHdp As Boolean = False
        If Not Request("chkCopy" & sportId & "Hdp") Is Nothing Then
            isCopyHdp = True

            isAutoOU = isAutoHdp
            isAutoOE = isAutoHdp
            isAutoML = isAutoHdp

        End If
        If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
            If isCopyBasket Then

                Dim isAuto2Hdp As Integer = 0
                If Not Request("chkAuto2Hdp") Is Nothing Then
                    isAuto2Hdp = 1
                End If
                autoHdp = IIf(isAuto2Hdp, Request("autoM2Hdp"), 0)
                autoOU = IIf(isAuto2Hdp, Request("autoM2Hdp"), 0)
                autoOE = IIf(isAuto2Hdp, Request("autoM2Hdp"), 0)
                autoML = IIf(isAuto2Hdp, Request("autoM2Hdp"), 0)

                isAutoHdp = CInt(isAuto2Hdp)
                isAutoOU = CInt(isAuto2Hdp)
                isAutoOE = CInt(isAuto2Hdp)
                isAutoML = CInt(isAuto2Hdp)

                mHdp = IIf(Request("isAuto_V2Hdp"), 0, Request("listM2Hdp"))
                mOU = IIf(Request("isAuto_V2OU"), 0, Request("listM2Hdp"))
                mOE = IIf(Request("isAuto_V2OE"), 0, Request("listM2Hdp"))
                mML = IIf(Request("isAuto_V2ML"), 0, Request("listM2Hdp"))

            Else
                autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoM" & sportId & "Hdp"), Request("autoM" & sportId & "Hdp")), 0)
                autoOU = IIf(isAutoOU, IIf(isCopyHdp, Request("autoM" & sportId & "Hdp"), Request("autoM" & sportId & "OU")), 0)
                autoOE = IIf(isAutoOE, IIf(isCopyHdp, Request("autoM" & sportId & "Hdp"), Request("autoM" & sportId & "OE")), 0)
                autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoM" & sportId & "Hdp"), Request("autoM" & sportId & "ML")), 0)

                mHdp = IIf(isCopyHdp, IIf(Request("isAuto_V" & sportId & "Hdp"), 0, Request("listM" & sportId & "Hdp")), Request("listM" & sportId & "Hdp"))
                mOU = IIf(isCopyHdp, IIf(Request("isAuto_V" & sportId & "OU"), 0, Request("listM" & sportId & "Hdp")), Request("listM" & sportId & "OU"))
                mOE = IIf(isCopyHdp, IIf(Request("isAuto_V" & sportId & "OE"), 0, Request("listM" & sportId & "Hdp")), Request("listM" & sportId & "OE"))
                mML = IIf(isCopyHdp, IIf(Request("isAuto_V" & sportId & "ML"), 0, Request("listM" & sportId & "Hdp")), Request("listM" & sportId & "ML"))

            End If
        Else
            Dim isAutoSoccer As Boolean = False
            If Not Request("chkAuto1HdpD") Is Nothing Then
                isAutoSoccer = True
            End If
            autoHdp = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
            autoOU = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
            autoOE = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
            autoML = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)


            isAutoHdp = CInt(isAutoSoccer)
            isAutoOU = CInt(isAutoSoccer)
            isAutoOE = CInt(isAutoSoccer)
            isAutoML = CInt(isAutoSoccer)


            mHdp = IIf(Request("isAuto_V" & sportId & "Hdp"), 0, Request("listM1HdpD"))
            mOU = IIf(Request("isAuto_V" & sportId & "OU"), 0, Request("listM1HdpD"))
            mOE = IIf(Request("isAuto_V" & sportId & "OE"), 0, Request("listM1HdpD"))
            mML = IIf(Request("isAuto_V" & sportId & "ML"), 0, Request("listM1HdpD"))

        End If

        'If isMult Then
        '    param.Add("updatepositiontakingv" & sportId, IIf(Request("chkUpd_" & sportId) Is Nothing, 0, 1))
        'End If
        If isMult Then
            If Not isCopyAll Then
                If isCopyBasket Then
                    param.Add("updatepositiontakingv" & sportId, IIf(Request("chkUpd_2") Is Nothing, 0, 1))
                Else
                    param.Add("updatepositiontakingv" & sportId, IIf(Request("chkUpd_" & sportId) Is Nothing, 0, 1))
                End If
            Else
                param.Add("updatepositiontakingv" & sportId, IIf(Request("chkUpd_1") Is Nothing, 0, 1))
            End If
        End If

        param.Add("v" & sportId & "mpositiontaking_1", mHdp) 'smallmoney  
        param.Add("v" & sportId & "mpositiontaking_3", mOU) 'smallmoney  
        param.Add("v" & sportId & "mpositiontaking_2", mOE) 'smallmoney  
        param.Add("v" & sportId & "mpositiontaking_20", mML) 'smallmoney  

        param.Add("isauto_v" & sportId & "mpositiontaking_1", Convert.ToInt16(Request("isAuto_V" & sportId & "Hdp"))) 'bit 
        param.Add("isauto_v" & sportId & "mpositiontaking_3", Convert.ToInt16(Request("isAuto_V" & sportId & "OU"))) 'bit  
        param.Add("isauto_v" & sportId & "mpositiontaking_2", Convert.ToInt16(Request("isAuto_V" & sportId & "OE"))) 'bit 
        param.Add("isauto_v" & sportId & "mpositiontaking_20", Convert.ToInt16(Request("isAuto_V" & sportId & "ML"))) 'bit

        param.Add("auto_v" & sportId & "mpositiontaking_mem_1", autoHdp) 'smallmoney  
        param.Add("auto_v" & sportId & "mpositiontaking_mem_2", autoOE) 'smallmoney  
        param.Add("auto_v" & sportId & "mpositiontaking_mem_3", autoOU) 'smallmoney  
        param.Add("auto_v" & sportId & "mpositiontaking_mem_20", autoML) 'smallmoney 

        param.Add("isauto_v" & sportId & "mpositiontaking_mem_1", isAutoHdp) 'bit  
        param.Add("isauto_v" & sportId & "mpositiontaking_mem_2", isAutoOE) 'bit  
        param.Add("isauto_v" & sportId & "mpositiontaking_mem_3", isAutoOU) 'bit  
        param.Add("isauto_v" & sportId & "mpositiontaking_mem_20", isAutoML) 'bit  

        If sportId = "2" Then
            param.Add("v2mpositiontaking_7", mHdp) 'smallmoney 
            param.Add("v2mpositiontaking_8", mOU) 'smallmoney                  


            param.Add("isauto_v2mpositiontaking_7", Convert.ToInt16(Request("isAuto_V" & sportId & "Hdp"))) 'bit  
            param.Add("isauto_v2mpositiontaking_8", Convert.ToInt16(Request("isAuto_V" & sportId & "OU"))) 'bit
            param.Add("isauto_v2mpositiontaking_9", Convert.ToInt16(Request("isAuto_V" & sportId & "MP"))) 'bit 

            param.Add("auto_v2mpositiontaking_mem_7", autoHdp) 'smallmoney  
            param.Add("auto_v2mpositiontaking_mem_8", autoOU) 'smallmoney 


            param.Add("isauto_v2mpositiontaking_mem_7", isAutoHdp) 'bit  
            param.Add("isauto_v2mpositiontaking_mem_8", isAutoOU) 'bit  

        End If
        '***** End Inserting for Football *****

    End Sub

#End Region

#Region "Insert parameters for Baseball Position Taking"
    Sub AddParaBaseball(ByVal isMult As Boolean)

        '***** Start Inserting for Baseball *****
        Dim mHdp, mOU, mML As Double
        Dim autoHdp, autoOU, autoML As Double
        Dim isAutoHdp, isAutoOU, isAutoML As Double

        isAutoHdp = IIf(Request("chkAuto8Hdp") Is Nothing, 0, 1)
        isAutoOU = IIf(Request("chkAuto8OU") Is Nothing, 0, 1)
        isAutoML = IIf(Request("chkAuto8ML") Is Nothing, 0, 1)

        Dim isCopyHdp As Boolean = False
        If Not Request("chkCopy8Hdp") Is Nothing Then
            isCopyHdp = True
            isAutoOU = isAutoHdp
            isAutoML = isAutoHdp
        End If
        If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
            If isCopyBasket Then
                Dim isAuto2Hdp As Integer = 0
                If Not Request("chkAuto2Hdp") Is Nothing Then
                    isAuto2Hdp = 1
                End If
                autoHdp = IIf(isAuto2Hdp, Request("autoM2Hdp"), 0)
                autoOU = IIf(isAuto2Hdp, Request("autoM2Hdp"), 0)
                autoML = IIf(isAuto2Hdp, Request("autoM2Hdp"), 0)

                isAutoHdp = CInt(isAuto2Hdp)
                isAutoOU = CInt(isAuto2Hdp)
                isAutoML = CInt(isAuto2Hdp)

                mHdp = IIf(Request("isAuto_V8Hdp"), 0, Request("listM2Hdp"))
                mOU = IIf(Request("isAuto_V8OU"), 0, Request("listM2Hdp"))
                mML = IIf(Request("isAuto_V8ML"), 0, Request("listM2Hdp"))
            Else
                autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoM8Hdp"), Request("autoM8Hdp")), 0)
                autoOU = IIf(isAutoOU, IIf(isCopyHdp, Request("autoM8Hdp"), Request("autoM8OU")), 0)
                autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoM8Hdp"), Request("autoM8ML")), 0)

                mHdp = IIf(isCopyHdp, IIf(Request("isAuto_V8Hdp"), 0, Request("listM8Hdp")), Request("listM8Hdp"))
                mOU = IIf(isCopyHdp, IIf(Request("isAuto_V8OU"), 0, Request("listM8Hdp")), Request("listM8OU"))
                mML = IIf(isCopyHdp, IIf(Request("isAuto_V8ML"), 0, Request("listM8Hdp")), Request("listM8ML"))
            End If
        Else
            Dim isAutoSoccer As Boolean = False
            If Not Request("chkAuto1HdpD") Is Nothing Then
                isAutoSoccer = True
            End If
            autoHdp = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
            autoOU = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
            autoML = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)

            isAutoHdp = CInt(isAutoSoccer)
            isAutoOU = CInt(isAutoSoccer)
            isAutoML = CInt(isAutoSoccer)

            mHdp = IIf(Request("isAuto_V8Hdp"), 0, Request("listM1HdpD"))
            mOU = IIf(Request("isAuto_V8OU"), 0, Request("listM1HdpD"))
            mML = IIf(Request("isAuto_V8ML"), 0, Request("listM1HdpD"))
        End If

        'If isMult Then
        '    param.Add("updatepositiontakingv8", IIf(Request("chkUpd_8") Is Nothing, 0, 1))
        'End If
        Dim sportId As String = "8"
        If isMult Then
            If Not isCopyAll Then
                If isCopyBasket Then
                    param.Add("updatepositiontakingv" & sportId, IIf(Request("chkUpd_2") Is Nothing, 0, 1))
                Else
                    param.Add("updatepositiontakingv" & sportId, IIf(Request("chkUpd_" & sportId) Is Nothing, 0, 1))
                End If
            Else
                param.Add("updatepositiontakingv" & sportId, IIf(Request("chkUpd_1") Is Nothing, 0, 1))
            End If
        End If

        param.Add("v8mpositiontaking_1", mHdp) 'smallmoney  
        param.Add("v8mpositiontaking_3", mOU) 'smallmoney 
        param.Add("v8mpositiontaking_20", mML) 'smallmoney

        param.Add("isauto_v8mpositiontaking_1", Convert.ToInt16(Request("isAuto_V8Hdp"))) 'bit  
        param.Add("isauto_v8mpositiontaking_3", Convert.ToInt16(Request("isAuto_V8OU"))) 'bit  
        param.Add("isauto_v8mpositiontaking_20", Convert.ToInt16(Request("isAuto_V8ML"))) 'bit  

        param.Add("auto_v8mpositiontaking_mem_1", autoHdp) 'smallmoney 
        param.Add("auto_v8mpositiontaking_mem_3", autoOU) 'smallmoney  
        param.Add("auto_v8mpositiontaking_mem_20", autoML) 'smallmoney        

        param.Add("isauto_v8mpositiontaking_mem_1", isAutoHdp) 'bit  
        param.Add("isauto_v8mpositiontaking_mem_3", isAutoOU) 'bit  
        param.Add("isauto_v8mpositiontaking_mem_20", isAutoML) 'bit  

        '***** End Inserting for Baseball *****

    End Sub
#End Region

#Region "Insert parameters for Motosports Position Taking"
    Sub AddParaMotosports(ByVal isMult As Boolean)

        '***** Start Inserting for MotoSports *****
        Dim mHdp, mML As Double
        Dim autoHdp, autoML As Double
        Dim isAutoHdp, isAutoML As Double

        Dim isCopyHdp As Boolean = False

        isAutoHdp = IIf(Request("chkAuto11Hdp") Is Nothing, 0, 1)
        isAutoML = IIf(Request("chkAuto11ML") Is Nothing, 0, 1)

        If Not Request("chkCopy11Hdp") Is Nothing Then
            isCopyHdp = True
            isAutoML = isAutoHdp
        End If
        If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
            If isCopyBasket Then
                Dim isAuto2Hdp As Integer = 0
                If Not Request("chkAuto2Hdp") Is Nothing Then
                    isAuto2Hdp = 1
                End If
                autoHdp = IIf(isAuto2Hdp, Request("autoM2Hdp"), 0)
                autoML = IIf(isAuto2Hdp, Request("autoM2Hdp"), 0)

                isAutoHdp = CInt(isAuto2Hdp)
                isAutoML = CInt(isAuto2Hdp)

                mHdp = IIf(Request("isAuto_V11Hdp"), 0, Request("listM2Hdp"))
                mML = IIf(Request("isAuto_V11ML"), 0, Request("listM2Hdp"))
            Else
                autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoM11Hdp"), Request("autoM11Hdp")), 0)
                autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoM11Hdp"), Request("autoM11ML")), 0)

                mHdp = IIf(isCopyHdp, IIf(Request("isAuto_V11Hdp"), 0, Request("listM11Hdp")), Request("listM11Hdp"))
                mML = IIf(isCopyHdp, IIf(Request("isAuto_V11ML"), 0, Request("listM11Hdp")), Request("listM11ML"))
            End If
        Else
            Dim isAutoSoccer As Boolean = False
            If Not Request("chkAuto1HdpD") Is Nothing Then
                isAutoSoccer = True
            End If
            autoHdp = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
            autoML = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)

            isAutoHdp = CInt(isAutoSoccer)
            isAutoML = CInt(isAutoSoccer)

            mHdp = IIf(Request("isAuto_V11Hdp"), 0, Request("listM1HdpD"))
            mML = IIf(Request("isAuto_V11ML"), 0, Request("listM1HdpD"))
        End If

        Dim sportId As String = "11"
        If isMult Then
            If Not isCopyAll Then
                If isCopyBasket Then
                    param.Add("updatepositiontakingv" & sportId, IIf(Request("chkUpd_2") Is Nothing, 0, 1))
                Else
                    param.Add("updatepositiontakingv" & sportId, IIf(Request("chkUpd_" & sportId) Is Nothing, 0, 1))
                End If
            Else
                param.Add("updatepositiontakingv" & sportId, IIf(Request("chkUpd_1") Is Nothing, 0, 1))
            End If
        End If
        'If isMult Then
        '    param.Add("updatepositiontakingv11", IIf(Request("chkUpd_11") Is Nothing, 0, 1))
        'End If

        param.Add("v11mpositiontaking_1", mHdp) 'smallmoney         
        param.Add("v11mpositiontaking_20", mML) 'smallmoney 

        param.Add("isauto_v11mpositiontaking_1", Convert.ToInt16(Request("isAuto_V11Hdp"))) 'bit  
        param.Add("isauto_v11mpositiontaking_20", Convert.ToInt16(Request("isAuto_V11ML"))) 'bit  

        param.Add("auto_v11mpositiontaking_mem_1", autoHdp) 'smallmoney          
        param.Add("auto_v11mpositiontaking_mem_20", autoML) 'smallmoney      

        param.Add("isauto_v11mpositiontaking_mem_1", isAutoHdp) 'bit  
        param.Add("isauto_v11mpositiontaking_mem_20", isAutoML) 'bit  

        '***** End Inserting for MotoSports *****

    End Sub
#End Region

#Region "Insert parameters for NumberGame Position Taking"
    Sub AddParaNumberGame(ByVal sportId As String, ByVal isMult As Boolean)


        Dim m1stOU, m1stOE, mOE, mWR As Double
        Dim mOUL, mOEL, mHLL, mNCL, mNGL As Double
        Dim auto1stOU, auto1stOE, autoOE, autoWR As Double
        Dim autoOUL, autoOEL, autoHLL, autoNCL, autoNGL As Double
        Dim isAuto1stOU, isAuto1stOE, isAutoOE, isAutoWR As Double
        Dim isAutoOUL, isAutoOEL, isAutoHLL, isAutoNCL, isAutoNGL As Double

        'Assign IsAuto value
        isAuto1stOU = IIf(Request("chkAuto" & sportId & "1stOUD") Is Nothing, 0, 1)
        isAuto1stOE = IIf(Request("chkAuto" & sportId & "1stOED") Is Nothing, 0, 1)
        isAutoOE = IIf(Request("chkAuto" & sportId & "OED") Is Nothing, 0, 1)
        isAutoWR = IIf(Request("chkAuto" & sportId & "WRD") Is Nothing, 0, 1)

        isAutoOUL = IIf(Request("chkAuto" & sportId & "OUL") Is Nothing, 0, 1)
        isAutoOEL = IIf(Request("chkAuto" & sportId & "OEL") Is Nothing, 0, 1)
        isAutoHLL = IIf(Request("chkAuto" & sportId & "HLL") Is Nothing, 0, 1)
        isAutoNCL = IIf(Request("chkAuto" & sportId & "NCL") Is Nothing, 0, 1)
        isAutoNGL = IIf(Request("chkAuto" & sportId & "NGL") Is Nothing, 0, 1)

        '----

        Dim isCopy1stOU As Boolean = False
        If Not Request("chkCopy" & sportId & "1stOUD") Is Nothing Then
            isCopy1stOU = True

            isAuto1stOE = isAuto1stOU
            isAutoOE = isAuto1stOU
            isAutoWR = isAuto1stOU
        End If

        Dim isCopyOU As Boolean = False
        If Not Request("chkCopy" & sportId & "OUL") Is Nothing Then
            isCopyOU = True
            isAutoOEL = isAutoOUL
            isAutoHLL = isAutoOUL
            isAutoNCL = isAutoOUL
            isAutoNGL = isAutoOUL
        End If

        Dim isDisabledNumberGame As Boolean = False
        If (Not UserPermission.IsShowNumberGame) Then
            isDisabledNumberGame = True
        End If

        If Not isDisabledNumberGame Then 'this case for Copy All PT checkbox is checked
            auto1stOU = IIf(isAuto1stOU, IIf(isCopy1stOU, Request("autoM" & sportId & "1stOUD"), Request("autoM" & sportId & "1stOUD")), 0)
            auto1stOE = IIf(isAuto1stOE, IIf(isCopy1stOU, Request("autoM" & sportId & "1stOUD"), Request("autoM" & sportId & "1stOED")), 0)
            autoOE = IIf(isAutoOE, IIf(isCopy1stOU, Request("autoM" & sportId & "1stOUD"), Request("autoM" & sportId & "OED")), 0)
            autoWR = IIf(isAutoWR, IIf(isCopy1stOU, Request("autoM" & sportId & "1stOUD"), Request("autoM" & sportId & "WRD")), 0)

            autoOUL = IIf(isAutoOUL, IIf(isCopyOU, Request("autoM" & sportId & "OUL"), Request("autoM" & sportId & "OUL")), 0)
            autoOEL = IIf(isAutoOEL, IIf(isCopyOU, Request("autoM" & sportId & "OUL"), Request("autoM" & sportId & "OEL")), 0)
            autoHLL = IIf(isAutoHLL, IIf(isCopyOU, Request("autoM" & sportId & "OUL"), Request("autoM" & sportId & "HLL")), 0)
            autoNCL = IIf(isAutoNCL, IIf(isCopyOU, Request("autoM" & sportId & "OUL"), Request("autoM" & sportId & "NCL")), 0)
            autoNGL = IIf(isAutoNGL, IIf(isCopyOU, IIf(UserPermission.IsShowNumberGroup, Request("autoM" & sportId & "OUL"), Request("autoM" & sportId & "NGL")), Request("autoM" & sportId & "NGL")), 0)

            m1stOU = IIf(isCopy1stOU, IIf(Request("isAuto_V" & sportId & "1stOUD"), 0, Request("listM" & sportId & "1stOUD")), Request("listM" & sportId & "1stOUD"))
            m1stOE = IIf(isCopy1stOU, IIf(Request("isAuto_V" & sportId & "1stOED"), 0, Request("listM" & sportId & "1stOUD")), Request("listM" & sportId & "1stOED"))
            mOE = IIf(isCopy1stOU, IIf(Request("isAuto_V" & sportId & "OED"), 0, Request("listM" & sportId & "1stOUD")), Request("listM" & sportId & "OED"))
            mWR = IIf(isCopy1stOU, IIf(Request("isAuto_V" & sportId & "WRD"), 0, Request("listM" & sportId & "1stOUD")), Request("listM" & sportId & "WRD"))

            mOUL = IIf(isCopyOU, IIf(Request("isAuto_V" & sportId & "OUL"), 0, Request("listM" & sportId & "OUL")), Request("listM" & sportId & "OUL"))
            mOEL = IIf(isCopyOU, IIf(Request("isAuto_V" & sportId & "OEL"), 0, Request("listM" & sportId & "OUL")), Request("listM" & sportId & "OEL"))
            mHLL = IIf(isCopyOU, IIf(Request("isAuto_V" & sportId & "HLL"), 0, Request("listM" & sportId & "OUL")), Request("listM" & sportId & "HLL"))
            mNCL = IIf(isCopyOU, IIf(Request("isAuto_V" & sportId & "NCL"), 0, Request("listM" & sportId & "OUL")), Request("listM" & sportId & "NCL"))
            mNGL = IIf(isCopyOU, IIf(Request("isAuto_V" & sportId & "NGL"), 0, IIf(UserPermission.IsShowNumberGroup, Request("listM" & sportId & "OUL"), Request("listM" & sportId & "NGL"))), Request("listM" & sportId & "NGL"))
        Else
            Dim isAutoSoccer As Boolean = False
            If Not Request("chkAuto1HdpD") Is Nothing Then
                isAutoSoccer = True
            End If
            auto1stOU = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
            auto1stOE = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
            autoOE = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
            autoWR = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)

            autoOUL = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
            autoOEL = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
            autoHLL = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
            autoNCL = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
            autoNGL = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)

            isAuto1stOU = CInt(isAutoSoccer)
            isAuto1stOE = CInt(isAutoSoccer)
            isAutoOE = CInt(isAutoSoccer)
            isAutoWR = CInt(isAutoSoccer)

            isAutoOUL = CInt(isAutoSoccer)
            isAutoOEL = CInt(isAutoSoccer)
            isAutoHLL = CInt(isAutoSoccer)
            isAutoNCL = CInt(isAutoSoccer)
            isAutoNGL = CInt(isAutoSoccer)

            m1stOU = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
            m1stOE = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
            mOE = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
            mWR = IIf(isAutoSoccer, 0, Request("listM1HdpD"))

            mOUL = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
            mOEL = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
            mHLL = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
            mNCL = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
            mNGL = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
        End If

        If isMult Then
            param.Add("updatepositiontakingv" & sportId, IIf(Request("chkUpd_" & sportId) Is Nothing, 0, 1))
        End If

        param.Add("v" & sportId & "mpositiontaking_81", m1stOU) 'smallmoney  
        param.Add("v" & sportId & "mpositiontaking_83", m1stOE) 'smallmoney  
        param.Add("v" & sportId & "mpositiontaking_86", mOE) 'smallmoney 
        param.Add("v" & sportId & "mpositiontaking_88", mWR) 'smallmoney 

        param.Add("v" & sportId & "mlivepositiontaking_85", mOUL) 'smallmoney  
        param.Add("v" & sportId & "mlivepositiontaking_86", mOEL) 'smallmoney  
        param.Add("v" & sportId & "mlivepositiontaking_87", mHLL) 'smallmoney 
        param.Add("v" & sportId & "mlivepositiontaking_89", mNCL) 'smallmoney  
        param.Add("v" & sportId & "mlivepositiontaking_90", mNGL) 'smallmoney  

        param.Add("isauto_v" & sportId & "mpositiontaking_81", Convert.ToInt16(Request("isAuto_V" & sportId & "1stOUD"))) 'bit 
        param.Add("isauto_v" & sportId & "mpositiontaking_83", Convert.ToInt16(Request("isAuto_V" & sportId & "1stOED"))) 'bit  
        param.Add("isauto_v" & sportId & "mpositiontaking_85", Convert.ToInt16(Request("isAuto_V" & sportId & "OUD"))) 'bit 
        param.Add("isauto_v" & sportId & "mpositiontaking_86", Convert.ToInt16(Request("isAuto_V" & sportId & "OED"))) 'bit
        param.Add("isauto_v" & sportId & "mpositiontaking_88", Convert.ToInt16(Request("isAuto_V" & sportId & "WRD"))) 'bit

        param.Add("isauto_v" & sportId & "mlivepositiontaking_85", Convert.ToInt16(Request("isAuto_V" & sportId & "OUL"))) 'bit 
        param.Add("isauto_v" & sportId & "mlivepositiontaking_86", Convert.ToInt16(Request("isAuto_V" & sportId & "OEL"))) 'bit  
        param.Add("isauto_v" & sportId & "mlivepositiontaking_87", Convert.ToInt16(Request("isAuto_V" & sportId & "HLL"))) 'bit
        param.Add("isauto_v" & sportId & "mlivepositiontaking_89", Convert.ToInt16(Request("isAuto_V" & sportId & "NCL"))) 'bit
        param.Add("isauto_v" & sportId & "mlivepositiontaking_90", Convert.ToInt16(Request("isAuto_V" & sportId & "NGL"))) 'bit

        param.Add("auto_v" & sportId & "mpositiontaking_mem_81", auto1stOU) 'smallmoney  
        param.Add("auto_v" & sportId & "mpositiontaking_mem_83", auto1stOE) 'smallmoney  
        param.Add("auto_v" & sportId & "mpositiontaking_mem_86", autoOE) 'smallmoney 
        param.Add("auto_v" & sportId & "mpositiontaking_mem_88", autoWR) 'smallmoney 

        param.Add("auto_v" & sportId & "mlivepositiontaking_mem_85", autoOUL) 'smallmoney  
        param.Add("auto_v" & sportId & "mlivepositiontaking_mem_86", autoOEL) 'smallmoney  
        param.Add("auto_v" & sportId & "mlivepositiontaking_mem_87", autoHLL) 'smallmoney  
        param.Add("auto_v" & sportId & "mlivepositiontaking_mem_89", autoNCL) 'smallmoney  
        param.Add("auto_v" & sportId & "mlivepositiontaking_mem_90", autoNGL) 'smallmoney  

        param.Add("isauto_v" & sportId & "mpositiontaking_mem_81", isAuto1stOU) 'bit  
        param.Add("isauto_v" & sportId & "mpositiontaking_mem_83", isAuto1stOE) 'bit  
        param.Add("isauto_v" & sportId & "mpositiontaking_mem_86", isAutoOE) 'bit  
        param.Add("isauto_v" & sportId & "mpositiontaking_mem_88", isAutoWR) 'bit  

        param.Add("isauto_v" & sportId & "mlivepositiontaking_mem_85", isAutoOUL) 'bit  
        param.Add("isauto_v" & sportId & "mlivepositiontaking_mem_86", isAutoOEL) 'bit  
        param.Add("isauto_v" & sportId & "mlivepositiontaking_mem_87", isAutoHLL) 'bit  
        param.Add("isauto_v" & sportId & "mlivepositiontaking_mem_89", isAutoNCL) 'bit  
        param.Add("isauto_v" & sportId & "mlivepositiontaking_mem_90", isAutoNGL) 'bit  

    End Sub

#End Region

#Region "Insert parameters for MixedSports Position Taking"
    Sub AddParaMixedSport(ByVal isMult As Boolean)


        Dim mMP As Double
        Dim autoMP As Double
        Dim isAutoMP As Double

        'Assign IsAuto value
        isAutoMP = IIf(Request("chkAuto99_M") Is Nothing, 0, 1)
        '----


        If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
            If isCopyBasket Then

                Dim isAuto2Hdp As Integer = 0
                If Not Request("chkAuto2Hdp") Is Nothing Then
                    isAuto2Hdp = 1
                End If
                autoMP = IIf(isAuto2Hdp, Request("autoM2Hdp"), 0)
                isAutoMP = CInt(isAuto2Hdp)
                mMP = IIf(Request("isAuto_V2Hdp"), 0, Request("listM2Hdp"))
            Else
                autoMP = IIf(isAutoMP, Request("autoM99_M"), 0)
                mMP = IIf(Request("isAuto_V99_M"), 0, Request("listM99_M"))
            End If
        Else
            Dim isAutoSoccer As Boolean = False
            If Not Request("chkAuto1HdpD") Is Nothing Then
                isAutoSoccer = True
            End If
            autoMP = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
            isAutoMP = CInt(isAutoSoccer)
            mMP = IIf(Request("isAuto_V99_M"), 0, Request("listM1HdpD"))

        End If

        'If isMult Then
        '    param.Add("updatepositiontakingv99mp", IIf(Request("chkUpd_99_M") Is Nothing, 0, 1))
        'End If
        Dim sportId As String = "99mp"
        If isMult Then
            If Not isCopyAll Then
                If isCopyBasket Then
                    param.Add("updatepositiontakingv" & sportId, IIf(Request("chkUpd_2") Is Nothing, 0, 1))
                Else
                    param.Add("updatepositiontakingv" & sportId, IIf(Request("chkUpd_99_M") Is Nothing, 0, 1))
                End If
            Else
                param.Add("updatepositiontakingv" & sportId, IIf(Request("chkUpd_1") Is Nothing, 0, 1))
            End If
        End If



        param.Add("v99mpositiontaking_9", mMP) 'smallmoney            
        param.Add("isauto_v99mpositiontaking_9", Convert.ToInt16(Request("isAuto_V99_M"))) 'bit           
        param.Add("auto_v99mpositiontaking_mem_9", autoMP) 'smallmoney            
        param.Add("isauto_v99mpositiontaking_mem_9", isAutoMP) 'bit  

    End Sub

#End Region

    '---------------------------------------------
#Region "ReplaceData"
    Public Function ReplaceData(ByVal str As String) As String
        Return str.Replace("""", "&quot;").Replace("'", "&#39;").Replace(vbNewLine, "")
    End Function
#End Region
End Class


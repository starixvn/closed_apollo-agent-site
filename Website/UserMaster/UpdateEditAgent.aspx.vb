Imports System.Text
Imports DNAide.Utils
Imports Fanex.Data.DNAide
Imports DNAide.Core
Imports DNAide.Web
Imports System.Data
Imports System.Collections
Imports System.Resources
Imports DAL
Imports DataHandler
Imports System.Web.Security
Imports Sunplus.Agent.Model.Entities.Permission

Public Class UpdateEditAgent
    Inherits IbcPage2
    Dim param As New Hashtable
    Dim isCopyAll As Boolean = False
    Dim isCopyBasket As Boolean = False
    Dim ptStr As String = String.Empty


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
            Response.Write("<script type=""text/javascript"">parent.UpdateErr('" & customErrMsg & "');</script>")
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
#Region "Update data"
    Sub UpdateData()
        Dim arrCID() As String = Request("saveCustid").Split("^")
        Dim isMult As Boolean = False
        Dim _errcode As String = ""
        Dim _errmsg As String = ""
        Dim _username As String = ""

        Dim unameArr() As String = Request("saveUsername").Split(",")

        isMult = IIf(Request("isMult") Is Nothing, 0, CBool(Request("isMult")))

        Dim rs As DbResult
        For i As Integer = 0 To arrCID.Length - 1
            If arrCID(i) <> "" Then
                param.Clear()
                param.Add("custid", arrCID(i)) 'int 
                param.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId) 'int  
                param.Add("mrecommend", Sunplus.Agent.Model.Entities.UserSession.Current.CustId) 'int 
                param.Add("discasino", Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledCasino)
                If isMult Then
                    param.Add("firstname", "") 'nvarchar  
                    param.Add("lastname", "") 'nvarchar  
                    param.Add("phone", "") 'nvarchar  
                    param.Add("mobilephone", "") 'nvarchar  
                    param.Add("fax", "") 'nvarchar 
                    param.Add("credit", 0) 'money  
                    param.Add("updatemultiple", 1) 'bit  
                Else
                    param.Add("firstname", ReplaceData(Request("txtFName"))) 'nvarchar  
                    param.Add("lastname", ReplaceData(Request("txtLName"))) 'nvarchar  
                    param.Add("phone", ReplaceData(Request("txtPhone"))) 'nvarchar  
                    param.Add("mobilephone", ReplaceData(Request("txtMobile"))) 'nvarchar  
                    param.Add("fax", ReplaceData(Request("txtFax"))) 'nvarchar 
                    param.Add("credit", Request("txtCredit")) 'money  
                    param.Add("maxcredit", Request("txtMaxCredit")) 'money  
                    param.Add("updatemultiple", 0) 'bit  
                End If
                If Not Request("chkUpdStatus") Is Nothing Or Not isMult Then
                    param.Add("updateclosed", 1) 'bit
                    If Not Request("chkClosed") Is Nothing Then
                        param.Add("closed", 1) 'bit 
                    Else
                        param.Add("closed", 0) 'bit 
                    End If
                Else
                    param.Add("updateclosed", 0) 'bit    
                    param.Add("closed", 0) 'bit  
                End If

                If isMult Then
                    param.Add("updatesuspended", 0) 'bit    
                    param.Add("suspended", 0) 'bit  
                Else
                    param.Add("updatesuspended", 1) 'bit @UpdateSuspended
                    If Not Request("chkSuspended") Is Nothing Or (Request("IsSuspended") = 1 And Request("UplineSuspended") = 1) Then
                        param.Add("suspended", 1) 'bit 
                    Else
                        param.Add("suspended", 0) 'bit 
                    End If
                End If

                param.Add("Language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey)


                If Not Request("chkUpdComm") Is Nothing Or Not isMult Then
                    param.Add("updatecommission", 1) 'bit  
                    param.Add("groupa", Request("groupa")) 'decimal  
                    param.Add("groupb", Request("groupb")) 'decimal  
                    param.Add("groupc", Request("groupc")) 'decimal  
                    param.Add("groupd", Request("groupd")) 'decimal  
                    param.Add("discount1x2", Request("discount1x2")) 'decimal                        
                    param.Add("discountcs", Request("discountcs")) 'decimal   
                    param.Add("discountnumber", IIf(UserPermission.IsShowNumberGame, Request("disnumbergame"), 0)) 'decimal   
                Else
                    param.Add("updatecommission", 0) 'bit  
                    param.Add("groupa", 0) 'decimal  
                    param.Add("groupb", 0) 'decimal  
                    param.Add("groupc", 0) 'decimal  
                    param.Add("groupd", 0) 'decimal  
                    param.Add("discount1x2", 0) 'decimal
                    param.Add("discountcs", 0) 'decimal
                    param.Add("discountnumber", 0) 'decimal   

                End If
                If Not Request("chkCopyAll") Is Nothing Then
                    isCopyAll = True
                End If
                If Not Request("chkCopyBasket") Is Nothing Then
                    isCopyBasket = True
                End If

                Dim isUpdAll As Integer = CInt(Request("isUpdateMember"))
                param.Add("updateallmembers", isUpdAll) 'bit  
                param.Add("newmpositiontaking", Request("memberPT")) 'smallmoney  

                param.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId) 'int     
                param.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP())
                param.Add("errcode", 0) 'int  
                param.Add("errmsg", "") 'nvarchar  
                param.Add("username", "")
                'nqminh - 29Nov08 - changed
                param.Add("sitename", AppSettings.SiteLevel) 'varchar  

                AddParaSoccer(isMult)
                AddParaFootball(2, isMult)
                AddParaFootball(3, isMult)
                AddParaFootball(5, isMult)
                AddParaBaseball(isMult)
                AddParaFootball(10, isMult)
                AddParaMotosports(isMult)
                AddParaFootball(99, isMult)
                AddParaNumberGame(161, isMult)
                AddParaMixedSports("99_M", isMult) ' 2/2/2012 changed by David

                rs = AccountInfo.Acc_CustomerUpdEditCustomer_MAg(param)
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
        Dim res As Integer = CommonGenerateHTML.GenerateErrorMsg(_errcode, _errmsg, strErrmsg, _username, CallAPI)
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
                    status = csCustomer.UpdateCustInfo(CInt(param("custid")), param("firstname").ToString(), param("lastname").ToString(), param("phone").ToString(), param("mobilephone").ToString(), param("fax").ToString(), CBool(param("closed")), CBool(param("suspended")), Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), Sunplus.Agent.Model.Entities.UserSession.Current.CustId, strModifyBy, 2)

                Else
                    If Not Request("chkUpdStatus") Is Nothing Then
                        Dim isClosed As Boolean = False
                        If Not Request("chkClosed") Is Nothing Then
                            isClosed = True 'bit 
                        Else
                            isClosed = False 'bit 
                        End If
                        For i As Integer = 0 To arrCID.Length - 2 Step 1
                            If (New Sunplus.Agent.Model.BL.Casino.CasinoBO(Session.SessionID).IsSynchronized(CInt(arrCID(i)))) Then
                                status = csCustomer.UpdateCustClosed(CInt(arrCID(i)), isClosed, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), Sunplus.Agent.Model.Entities.UserSession.Current.CustId, strModifyBy, 2)
                            End If
                        Next
                    End If


                End If
            End If
        End If
        If res <> 0 Then
            Response.Write("<script type=""text/javascript"">parent.UpdateErr('" & strErrmsg.ToString() & "');</script>")
        Else
            Dim FromAccount As String
            Dim hash As Hashtable = AppSession.RequestQueryString
            FromAccount = IIf(hash.ContainsKey("FromAccount"), hash("FromAccount"), "")
            If (String.IsNullOrEmpty(FromAccount)) Then
                Response.Write("<script type=""text/javascript"">parent.parent.DelayReloadPage('');</script>")
            Else 'from AccountTab
                Response.Write("<script type=""text/javascript"">parent.top.menu.reloadMenuUsrInfo(); parent.DelayReloadPage('../_MemberInfo/CustomerList/Master/AgentList.aspx');</script>")
            End If
        End If


    End Sub
#End Region

#Region "Insert parameters for Soccer Position Taking"
    Sub AddParaSoccer(ByVal isMult As Boolean)
        'Add Bet Setting parameters        
        If Not Request("chkUpdInfo1") Is Nothing Or Not isMult Then
            param.Add("updateminmaxbet", 1) 'bit  
            param.Add("minbet", Request("txtMinbet1")) 'money  
            param.Add("maxbet", Request("txtMaxbet1")) 'money  
            param.Add("maxpermatch", Request("txtMaxPerMatch1")) 'money  
        Else
            param.Add("updateminmaxbet", 0) 'bit  
            param.Add("minbet", 0) 'money  
            param.Add("maxbet", 0) 'money  
            param.Add("maxpermatch", 0) 'money  
        End If
        '**************************
        '***** Add Position Taking paramaters*****
        Dim mHdpD, mOUD, m1stHdpD, m1stOUD, mOE, m1x2, mCS, mTG, mMp, mOR As Double
        Dim aHdpD, aOUD, a1stHdpD, a1stOUD, aOE, a1x2, aCS, aTG, aMp, aOR As Double

        Dim mHdpL, mOUL, m1stHdpL, m1stOUL As Double
        Dim aHdpL, aOUL, a1stHdpL, a1stOUL As Double

        '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
        Dim autoHdpD, autoOUD, auto1stHdpD, auto1stOUD, autoOE, auto1x2, autoCS, autoTG, autoMp, autoOR As Double
        Dim isAutoHdpD, isAutoOUD, isAuto1stHdpD, isAuto1stOUD, isAutoOE, isAuto1x2, isAutoCS, isAutoTG, isAutoMp, isAutoOR As Boolean

        Dim autoHdpL, autoOUL, auto1stHdpL, auto1stOUL As Double
        Dim isAutoHdpL, isAutoOUL, isAuto1stHdpL, isAuto1stOUL As Boolean
        '/--------------------------------------------------------------------------------------------------------------

        If Not Request("chkUpdPT1") Is Nothing Or Not isMult Then
            param.Add("updatepositiontaking", 1) 'bit 

            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
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
            '/--------------------------------------------------------------------------------------------------------------

            Dim isCopyHdpD As Boolean = False
            If Not Request("chkCopy1HdpDeadball") Is Nothing Then
                isCopyHdpD = True
                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                isAutoOUD = isAutoHdpD
                isAuto1stHdpD = isAutoHdpD
                isAuto1stOUD = isAutoHdpD
                isAutoOE = isAutoHdpD
                '/--------------------------------------------------------------------------------------------------------------

            End If
            Dim isCopy1x2 As Boolean = False
            If Not Request("chkCopy11x2") Is Nothing Then
                isCopy1x2 = True
                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                isAutoCS = isAuto1x2
                isAutoTG = isAuto1x2
                isAutoMp = isAuto1x2
                isAutoOR = isAuto1x2
                '/--------------------------------------------------------------------------------------------------------------

            End If
            Dim isCopyHdpL As Boolean = False
            If Not Request("chkCopy1HdpLive") Is Nothing Then
                isCopyHdpL = True
                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                isAutoOUL = isAutoHdpL
                isAuto1stHdpL = isAutoHdpL
                isAuto1stOUL = isAutoHdpL
                '/--------------------------------------------------------------------------------------------------------------

            End If
            '---------------------------------------Tho Update-----------------------------
            aHdpD = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM1HdpD")))
            aOUD = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM1OUD")))
            a1stHdpD = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM11stHdpD")))
            a1stOUD = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM11stOUD")))
            aOE = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM1OE")))

            a1x2 = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopy1x2, Request("listM11x2"), Request("listM11x2")))
            aCS = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopy1x2, Request("listM11x2"), Request("listM1CS")))
            aTG = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopy1x2, Request("listM11x2"), Request("listM1TG")))
            aMp = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopy1x2, Request("listM11x2"), Request("listM1MP")))
            aOR = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopy1x2, Request("listM11x2"), Request("listM1OR")))

            aHdpL = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM1HdpL")))
            aOUL = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM1OUL")))
            a1stHdpL = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM11stHdpL")))
            a1stOUL = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM11stOUL")))
            '/-------------------------------------------------------------------------
            If Not isCopyAll Then 'this case for Copy All PT checkbox is checked  

                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                autoHdpD = IIf(isAutoHdpD, IIf(isCopyHdpD, Request("autoS1HdpD"), Request("autoS1HdpD")), 0)
                autoOUD = IIf(isAutoOUD, IIf(isCopyHdpD, Request("autoS1HdpD"), Request("autoS1OUD")), 0)
                auto1stHdpD = IIf(isAuto1stHdpD, IIf(isCopyHdpD, Request("autoS1HdpD"), Request("autoS11stHdpD")), 0)
                auto1stOUD = IIf(isAuto1stOUD, IIf(isCopyHdpD, Request("autoS1HdpD"), Request("autoS11stOUD")), 0)
                autoOE = IIf(isAutoOE, IIf(isCopyHdpD, Request("autoS1HdpD"), Request("autoS1OE")), 0)

                auto1x2 = IIf(isAuto1x2, IIf(isCopy1x2, Request("autoS11x2"), Request("autoS11x2")), 0)
                autoCS = IIf(isAutoCS, IIf(isCopy1x2, Request("autoS11x2"), Request("autoS1CS")), 0)
                autoTG = IIf(isAutoTG, IIf(isCopy1x2, Request("autoS11x2"), Request("autoS1TG")), 0)
                autoMp = IIf(isAutoMp, IIf(isCopy1x2, Request("autoS11x2"), Request("autoS1MP")), 0)
                autoOR = IIf(isAutoOR, IIf(isCopy1x2, Request("autoS11x2"), Request("autoS1OR")), 0)

                autoHdpL = IIf(isAutoHdpL, IIf(isCopyHdpL, Request("autoS1HdpL"), Request("autoS1HdpL")), 0)
                autoOUL = IIf(isAutoOUL, IIf(isCopyHdpL, Request("autoS1HdpL"), Request("autoS1OUL")), 0)
                auto1stHdpL = IIf(isAuto1stHdpL, IIf(isCopyHdpL, Request("autoS1HdpL"), Request("autoS11stHdpL")), 0)
                auto1stOUL = IIf(isAuto1stOUL, IIf(isCopyHdpL, Request("autoS1HdpL"), Request("autoS11stOUL")), 0)
                '/--------------------------------------------------------------------------------------------------------------

                mHdpD = IIf(isAutoHdpD, 0, IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS1HdpD")))
                mOUD = IIf(isAutoOUD, 0, IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS1OUD")))
                m1stHdpD = IIf(isAuto1stHdpD, 0, IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS11stHdpD")))
                m1stOUD = IIf(isAuto1stOUD, 0, IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS11stOUD")))
                mOE = IIf(isAutoOE, 0, IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS1OE")))

                m1x2 = IIf(isAuto1x2, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS11x2")))
                mCS = IIf(isAutoCS, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS1CS")))
                mTG = IIf(isAutoTG, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS1TG")))
                mMp = IIf(isAutoMp, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS1MP")))
                mOR = IIf(isAutoOR, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS1OR")))

                mHdpL = IIf(isAutoHdpL, 0, IIf(isCopyHdpL, Request("listS1HdpL"), Request("listS1HdpL")))
                mOUL = IIf(isAutoOUL, 0, IIf(isCopyHdpL, Request("listS1HdpL"), Request("listS1OUL")))
                m1stHdpL = IIf(isAuto1stHdpL, 0, IIf(isCopyHdpL, Request("listS1HdpL"), Request("listS11stHdpL")))
                m1stOUL = IIf(isAuto1stOUL, 0, IIf(isCopyHdpL, Request("listS1HdpL"), Request("listS11stOUL")))
            Else

                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                Dim isAutoSoccer As Boolean = False
                If Not Request("chkAuto1HdpD") Is Nothing Then
                    isAutoSoccer = True
                End If

                autoHdpD = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoOUD = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                auto1stHdpD = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                auto1stOUD = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoOE = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)

                auto1x2 = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoCS = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoTG = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoMp = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoOR = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)

                autoHdpL = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoOUL = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                auto1stHdpL = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                auto1stOUL = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                '/--------------------------------------------------------------------------------------------------------------

                mHdpD = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mOUD = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                m1stHdpD = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                m1stOUD = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mOE = IIf(isAutoSoccer, 0, Request("listS1HdpD"))

                m1x2 = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mCS = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mTG = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mMp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mOR = IIf(isAutoSoccer, 0, Request("listS1HdpD"))

                mHdpL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mOUL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                m1stHdpL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                m1stOUL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))

                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
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
                '/--------------------------------------------------------------------------------------------------------------



            End If
        Else
            param.Add("updatepositiontaking", 0) 'bit 
        End If
        param.Add("apositiontaking_1", aHdpD) 'smallmoney  
        param.Add("apositiontaking_3", aOUD) 'smallmoney 
        param.Add("apositiontaking_7", a1stHdpD) 'smallmoney  
        param.Add("apositiontaking_8", a1stOUD) 'smallmoney  
        param.Add("apositiontaking_2", aOE) 'smallmoney  

        param.Add("apositiontaking_5", a1x2) 'smallmoney  
        param.Add("apositiontaking_4", aCS) 'smallmoney  
        param.Add("apositiontaking_6", aTG) 'smallmoney          
        param.Add("apositiontaking_9", aMp) 'smallmoney  
        param.Add("apositiontaking_10", aOR) 'smallmoney  

        param.Add("alivepositiontaking_1", aHdpL) 'smallmoney  
        param.Add("alivepositiontaking_3", aOUL) 'smallmoney  
        param.Add("alivepositiontaking_7", a1stHdpL) 'smallmoney  
        param.Add("alivepositiontaking_8", a1stOUL) 'smallmoney  

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

        '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
        If Not False Then
            param.Add("auto_mpositiontaking_1", autoHdpD) 'smallmoney    
            param.Add("auto_mpositiontaking_3", autoOUD) 'smallmoney    
            param.Add("auto_mpositiontaking_7", auto1stHdpD) 'smallmoney 
            param.Add("auto_mpositiontaking_8", auto1stOUD) 'smallmoney  
            param.Add("auto_mpositiontaking_2", autoOE) 'smallmoney                  

            param.Add("auto_mpositiontaking_5", auto1x2) 'smallmoney 
            param.Add("auto_mpositiontaking_4", autoCS) 'smallmoney
            param.Add("auto_mpositiontaking_6", autoTG) 'smallmoney             
            param.Add("auto_mpositiontaking_9", autoMp) 'smallmoney  
            param.Add("auto_mpositiontaking_10", autoOR) 'smallmoney 

            param.Add("auto_mlivepositiontaking_1", autoHdpL) 'smallmoney     
            param.Add("auto_mlivepositiontaking_3", autoOUL) 'smallmoney     
            param.Add("auto_mlivepositiontaking_7", auto1stHdpL) 'smallmoney   
            param.Add("auto_mlivepositiontaking_8", auto1stOUL) 'smallmoney  

            param.Add("isauto_mpositiontaking_1", isAutoHdpD) 'bit  
            param.Add("isauto_mpositiontaking_3", isAutoOUD) 'bit 
            param.Add("isauto_mpositiontaking_7", isAuto1stHdpD) 'bit  
            param.Add("isauto_mpositiontaking_8", isAuto1stOUD) 'bit  
            param.Add("isauto_mpositiontaking_2", isAutoOE) 'bit  

            param.Add("isauto_mpositiontaking_5", isAuto1x2) 'bit 
            param.Add("isauto_mpositiontaking_4", isAutoCS) 'bit  
            param.Add("isauto_mpositiontaking_6", isAutoTG) 'bit        
            param.Add("isauto_mpositiontaking_9", isAutoMp) 'bit  
            param.Add("isauto_mpositiontaking_10", isAutoOR) 'bit  

            param.Add("isauto_mlivepositiontaking_1", isAutoHdpL) 'bit  
            param.Add("isauto_mlivepositiontaking_3", isAutoOUL) 'bit  
            param.Add("isauto_mlivepositiontaking_7", isAuto1stHdpL) 'bit  
            param.Add("isauto_mlivepositiontaking_8", isAuto1stOUL) 'bit  
        End If
        '/--------------------------------------------------------------------------------------------------------------

        '***** End Inserting for Soccer *****
    End Sub
#End Region

#Region "Insert parameters for Basketball, Football, Tennis, Golf, Other Sports Position Taking"
    Sub AddParaFootball(ByVal sportId As String, ByVal isMult As Boolean)
        Dim minb, maxb, maxpermatch As Double

        ' 2/2/2012 changed by David
        If Not Request("chkUpdInfo1") Is Nothing Or Not Request("chkUpdInfo2") Is Nothing Or Not Request("chkUpdInfo" & sportId) Is Nothing Or Not isMult Then
            If isCopyAll = False Then
                If isCopyBasket Then
                    param.Add("updateminmaxbetv" & sportId, IIf(Request("chkUpdInfo2") Is Nothing, 0, 1)) 'bit 
                Else
                    param.Add("updateminmaxbetv" & sportId, IIf(Request("chkUpdInfo" & sportId) Is Nothing, 0, 1)) 'bit 
                End If
            Else
                param.Add("updateminmaxbetv" & sportId, IIf(Request("chkUpdInfo1") Is Nothing, 0, 1)) 'bit 
            End If
            minb = IIf(isCopyAll, Request("txtMinbet1"), IIf(isCopyBasket, Request("txtMinbet2"), Request("txtMinbet" & sportId)))
            maxb = IIf(isCopyAll, Request("txtMaxbet1"), IIf(isCopyBasket, Request("txtMaxbet2"), Request("txtMaxbet" & sportId)))
            maxpermatch = IIf(isCopyAll, Request("txtMaxPerMatch1"), IIf(isCopyBasket, Request("txtMaxPerMatch2"), Request("txtMaxPerMatch" & sportId)))
        Else
            param.Add("updateminmaxbetv" & sportId, 0) 'bit                  
        End If
        '
        
        param.Add("minbetv" & sportId, minb) 'money  
        param.Add("maxbetv" & sportId, maxb) 'money  
        param.Add("maxpermatchv" & sportId, maxpermatch) 'money  
        '***** Start Inserting for Football *****
        Dim mHdp, mOU, mOE, mML As Double
        Dim aHdp, aOU, aOE, aML As Double
        '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
        Dim autoHdp, autoOU, autoOE, autoML As Double
        Dim isAutoHdp, isAutoOU, isAutoOE, isAutoML As Double
        '/--------------------------------------------------------------------------------------------------------------

        If Not Request("chkUpdPT1") Is Nothing Or Not Request("chkUpdPT2") Is Nothing Or Not Request("chkUpdPT" & sportId) Is Nothing Or Not isMult Then
            If isCopyAll = False Then
                If isCopyBasket Then
                    param.Add("updatepositiontakingv" & sportId, IIf(Request("chkUpdPT2") Is Nothing, 0, 1)) 'bit 
                Else
                    param.Add("updatepositiontakingv" & sportId, IIf(Request("chkUpdPT" & sportId) Is Nothing, 0, 1)) 'bit 
                End If
            Else
                param.Add("updatepositiontakingv" & sportId, IIf(Request("chkUpdPT1") Is Nothing, 0, 1)) 'bit 
            End If
           
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            isAutoHdp = IIf(Request("chkAuto" & sportId & "Hdp") Is Nothing, 0, 1)
            isAutoOU = IIf(Request("chkAuto" & sportId & "OU") Is Nothing, 0, 1)
            isAutoOE = IIf(Request("chkAuto" & sportId & "OE") Is Nothing, 0, 1)
            isAutoML = IIf(Request("chkAuto" & sportId & "ML") Is Nothing, 0, 1)
            '/--------------------------------------------------------------------------------------------------------------            

            Dim isCopyHdp As Boolean = False
            If Not Request("chkCopy" & sportId & "Hdp") Is Nothing Then
                isCopyHdp = True
                isAutoOU = isAutoHdp
                isAutoOE = isAutoHdp
                isAutoML = isAutoHdp
            End If

            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            If isCopyAll Or isCopyBasket Then
                aHdp = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
                aOU = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
                aOE = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
                aML = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
            Else
                aHdp = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "Hdp"))
                aOU = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "OU"))
                aOE = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "OE"))
                aML = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "ML"))
            End If
            '/--------------------------------------------------------------------------------------------------------------

            If Not isCopyAll Then 'this case for Copy All PT from Soccer checkbox is unchecked
                If isCopyBasket Then 'this case for Copy All PT from Basketball checkbox is checked    

                    '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                    Dim isAuto2Hdp As Integer = 0
                    If Not Request("chkAuto2Hdp") Is Nothing Then
                        isAuto2Hdp = 1
                    End If

                    autoHdp = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                    autoOU = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                    autoOE = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                    autoML = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                    '/--------------------------------------------------------------------------------------------------------------

                    mHdp = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))
                    mOU = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))
                    mOE = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))
                    mML = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))

                    '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                    isAutoHdp = CInt(isAuto2Hdp)
                    isAutoOU = CInt(isAuto2Hdp)
                    isAutoOE = CInt(isAuto2Hdp)
                    isAutoML = CInt(isAuto2Hdp)
                    '/--------------------------------------------------------------------------------------------------------------

                Else
                    '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                    autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "Hdp")), 0)
                    autoOU = IIf(isAutoOU, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "OU")), 0)
                    autoOE = IIf(isAutoOE, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "OE")), 0)
                    autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "ML")), 0)
                    '/--------------------------------------------------------------------------------------------------------------

                    mHdp = IIf(isAutoHdp, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "Hdp")))
                    mOU = IIf(isAutoOU, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "OU")))
                    mOE = IIf(isAutoOE, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "OE")))
                    mML = IIf(isAutoML, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "ML")))

                End If
            Else
                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                Dim isAutoSoccer As Boolean = False
                If Not Request("chkAuto1HdpD") Is Nothing Then
                    isAutoSoccer = True
                End If
                autoHdp = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoOU = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoOE = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoML = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                '/--------------------------------------------------------------------------------------------------------------

                mHdp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mOU = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mOE = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mML = IIf(isAutoSoccer, 0, Request("listS1HdpD"))

                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                isAutoHdp = CInt(isAutoSoccer)
                isAutoOU = CInt(isAutoSoccer)
                isAutoOE = CInt(isAutoSoccer)
                isAutoML = CInt(isAutoSoccer)
                '/--------------------------------------------------------------------------------------------------------------


            End If
        Else
            param.Add("updatepositiontakingv" & sportId, 0) 'bit 
        End If
        param.Add("v" & sportId & "apositiontaking_1", aHdp) 'smallmoney  
        param.Add("v" & sportId & "apositiontaking_3", aOU) 'smallmoney  
        param.Add("v" & sportId & "apositiontaking_2", aOE) 'smallmoney  
        param.Add("v" & sportId & "apositiontaking_20", aML) 'smallmoney  

        param.Add("v" & sportId & "mpositiontaking_1", mHdp) 'smallmoney  
        param.Add("v" & sportId & "mpositiontaking_3", mOU) 'smallmoney  
        param.Add("v" & sportId & "mpositiontaking_2", mOE) 'smallmoney  
        param.Add("v" & sportId & "mpositiontaking_20", mML) 'smallmoney   
        '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
        If Not False Then
            param.Add("auto_v" & sportId & "mpositiontaking_1", autoHdp) 'smallmoney 
            param.Add("auto_v" & sportId & "mpositiontaking_3", autoOU) 'smallmoney  
            param.Add("auto_v" & sportId & "mpositiontaking_2", autoOE) 'smallmoney  
            param.Add("auto_v" & sportId & "mpositiontaking_20", autoML) 'smallmoney 

            param.Add("isauto_v" & sportId & "mpositiontaking_1", isAutoHdp) 'bit 
            param.Add("isauto_v" & sportId & "mpositiontaking_3", isAutoOU) 'bit  
            param.Add("isauto_v" & sportId & "mpositiontaking_2", isAutoOE) 'bit 
            param.Add("isauto_v" & sportId & "mpositiontaking_20", isAutoML) 'bit
        End If
        '/--------------------------------------------------------------------------------------------------------------
        If sportId = "2" Then
            param.Add("v2mpositiontaking_7", mHdp) 'smallmoney 
            param.Add("v2mpositiontaking_8", mOU) 'smallmoney  

            param.Add("v2apositiontaking_7", aHdp) 'smallmoney 
            param.Add("v2apositiontaking_8", aOU) 'smallmoney  

            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            If Not False Then
                param.Add("auto_v2mpositiontaking_7", autoHdp) 'smallmoney  
                param.Add("auto_v2mpositiontaking_8", autoOU) 'smallmoney

                param.Add("isauto_v2mpositiontaking_7", isAutoHdp) 'bit  
                param.Add("isauto_v2mpositiontaking_8", isAutoOU) 'bit

            End If
            '/--------------------------------------------------------------------------------------------------------------

        End If
        '***** End Inserting for Football *****
    End Sub
#End Region

#Region "Insert parameters for Baseball Position Taking"
    Sub AddParaBaseball(ByVal isMult As Boolean)
        Dim minb, maxb, maxpermatch As Double

        ' 2/2/2012 changed by David
        Dim sportId As String = "8"
        If Not Request("chkUpdInfo1") Is Nothing Or Not Request("chkUpdInfo2") Is Nothing Or Not Request("chkUpdInfo" & sportId) Is Nothing Or Not isMult Then
            If isCopyAll = False Then
                If isCopyBasket Then
                    param.Add("updateminmaxbetv" & sportId, IIf(Request("chkUpdInfo2") Is Nothing, 0, 1)) 'bit 
                Else
                    param.Add("updateminmaxbetv" & sportId, IIf(Request("chkUpdInfo" & sportId) Is Nothing, 0, 1)) 'bit 
                End If
            Else
                param.Add("updateminmaxbetv" & sportId, IIf(Request("chkUpdInfo1") Is Nothing, 0, 1)) 'bit 
            End If
            minb = IIf(isCopyAll, Request("txtMinbet1"), IIf(isCopyBasket, Request("txtMinbet2"), Request("txtMinbet" & sportId)))
            maxb = IIf(isCopyAll, Request("txtMaxbet1"), IIf(isCopyBasket, Request("txtMaxbet2"), Request("txtMaxbet" & sportId)))
            maxpermatch = IIf(isCopyAll, Request("txtMaxPerMatch1"), IIf(isCopyBasket, Request("txtMaxPerMatch2"), Request("txtMaxPerMatch" & sportId)))
        Else
            param.Add("updateminmaxbetv" & sportId, 0) 'bit                  
        End If
        '
       
        param.Add("minbetv8", minb) 'money  
        param.Add("maxbetv8", maxb) 'money  
        param.Add("maxpermatchv8", maxpermatch) 'money  
        '***** Start Inserting for Baseball *****
        Dim mHdp, mOU, mML As Double
        Dim aHdp, aOU, aML As Double
        '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
        Dim autoHdp, autoOU, autoML As Double
        Dim isAutoHdp, isAutoOU, isAutoML As Double
        '/--------------------------------------------------------------------------------------------------------------

        If Not Request("chkUpdPT1") Is Nothing Or Not Request("chkUpdPT2") Is Nothing Or Not Request("chkUpdPT8") Is Nothing Or Not isMult Then
            If isCopyAll = False Then
                If isCopyBasket Then
                    param.Add("updatepositiontakingv" & sportId, IIf(Request("chkUpdPT2") Is Nothing, 0, 1)) 'bit 
                Else
                    param.Add("updatepositiontakingv" & sportId, IIf(Request("chkUpdPT" & sportId) Is Nothing, 0, 1)) 'bit 
                End If
            Else
                param.Add("updatepositiontakingv" & sportId, IIf(Request("chkUpdPT1") Is Nothing, 0, 1)) 'bit 
            End If

            'If Not Request("chkUpdPT1") Is Nothing Or Not Request("chkUpdPT2") Is Nothing Or Not Request("chkUpdPT8") Is Nothing Or Not isMult Then
            '    param.Add("updatepositiontakingv8", 1) 'bit  
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            isAutoHdp = IIf(Request("chkAuto8Hdp") Is Nothing, 0, 1)
            isAutoOU = IIf(Request("chkAuto8OU") Is Nothing, 0, 1)
            isAutoML = IIf(Request("chkAuto8ML") Is Nothing, 0, 1)
            '/--------------------------------------------------------------------------------------------------------------

            Dim isCopyHdp As Boolean = False
            If Not Request("chkCopy8Hdp") Is Nothing Then
                isCopyHdp = True
                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                isAutoOU = isAutoHdp
                isAutoML = isAutoHdp
                '/--------------------------------------------------------------------------------------------------------------
            End If
            If isCopyAll Or isCopyBasket Then
                aHdp = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
                aOU = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
                aML = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
            Else
                aHdp = IIf(isCopyHdp, Request("listM8Hdp"), Request("listM8Hdp"))
                aOU = IIf(isCopyHdp, Request("listM8Hdp"), Request("listM8OU"))
                aML = IIf(isCopyHdp, Request("listM8Hdp"), Request("listM8ML"))
            End If
            If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
                If isCopyBasket Then
                    '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                    Dim isAuto2Hdp As Integer = 0
                    If Not Request("chkAuto2Hdp") Is Nothing Then
                        isAuto2Hdp = 1
                    End If
                    autoHdp = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                    autoOU = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                    autoML = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                    '/--------------------------------------------------------------------------------------------------------------

                    mHdp = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))
                    mOU = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))
                    mML = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))

                    '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                    isAutoHdp = CInt(isAuto2Hdp)
                    isAutoOU = CInt(isAuto2Hdp)
                    isAutoML = CInt(isAuto2Hdp)
                    '/--------------------------------------------------------------------------------------------------------------

                Else
                    '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                    autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoS8Hdp"), Request("autoS8Hdp")), 0)
                    autoOU = IIf(isAutoOU, IIf(isCopyHdp, Request("autoS8Hdp"), Request("autoS8OU")), 0)
                    autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoS8Hdp"), Request("autoS8ML")), 0)
                    '/--------------------------------------------------------------------------------------------------------------

                    mHdp = IIf(isAutoHdp, 0, IIf(isCopyHdp, Request("listS8Hdp"), Request("listS8Hdp")))
                    mOU = IIf(isAutoOU, 0, IIf(isCopyHdp, Request("listS8Hdp"), Request("listS8OU")))
                    mML = IIf(isAutoML, 0, IIf(isCopyHdp, Request("listS8Hdp"), Request("listS8ML")))
                End If
            Else
                'aHdp = Request("listM1HdpD")
                'aOU = Request("listM1HdpD")
                'aML = Request("listM1HdpD")
                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                Dim isAutoSoccer As Boolean = False
                If Not Request("chkAuto1HdpD") Is Nothing Then
                    isAutoSoccer = True
                End If
                autoHdp = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoOU = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoML = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                '/--------------------------------------------------------------------------------------------------------------

                mHdp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mOU = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mML = IIf(isAutoSoccer, 0, Request("listS1HdpD"))

                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                isAutoHdp = CInt(isAutoSoccer)
                isAutoOU = CInt(isAutoSoccer)
                isAutoML = CInt(isAutoSoccer)
                '/--------------------------------------------------------------------------------------------------------------
            End If
        Else
            param.Add("updatepositiontakingv8", 0) 'bit  

        End If
        param.Add("v8apositiontaking_1", aHdp) 'smallmoney  
        param.Add("v8apositiontaking_3", aOU) 'smallmoney 
        param.Add("v8apositiontaking_20", aML) 'smallmoney  

        param.Add("v8mpositiontaking_1", mHdp) 'smallmoney  
        param.Add("v8mpositiontaking_3", mOU) 'smallmoney 
        param.Add("v8mpositiontaking_20", mML) 'smallmoney

        '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
        If Not False Then
            param.Add("auto_v8mpositiontaking_1", autoHdp) 'smallmoney 
            param.Add("auto_v8mpositiontaking_3", autoOU) 'smallmoney  
            param.Add("auto_v8mpositiontaking_20", autoML) 'smallmoney        

            param.Add("isauto_v8mpositiontaking_1", isAutoHdp) 'bit  
            param.Add("isauto_v8mpositiontaking_3", isAutoOU) 'bit  
            param.Add("isauto_v8mpositiontaking_20", isAutoML) 'bit  
        End If
        '/--------------------------------------------------------------------------------------------------------------

        '***** End Inserting for Baseball *****
    End Sub
#End Region

#Region "Insert parameters for Motosports Position Taking"
    Sub AddParaMotosports(ByVal isMult As Boolean)
        Dim minb, maxb, maxpermatch As Double
        ' 2/2/2012 changed by David
        Dim sportId As String = "11"
        If Not Request("chkUpdInfo1") Is Nothing Or Not Request("chkUpdInfo2") Is Nothing Or Not Request("chkUpdInfo" & sportId) Is Nothing Or Not isMult Then
            If isCopyAll = False Then
                If isCopyBasket Then
                    param.Add("updateminmaxbetv" & sportId, IIf(Request("chkUpdInfo2") Is Nothing, 0, 1)) 'bit 
                Else
                    param.Add("updateminmaxbetv" & sportId, IIf(Request("chkUpdInfo" & sportId) Is Nothing, 0, 1)) 'bit 
                End If
            Else
                param.Add("updateminmaxbetv" & sportId, IIf(Request("chkUpdInfo1") Is Nothing, 0, 1)) 'bit 
            End If
            minb = IIf(isCopyAll, Request("txtMinbet1"), IIf(isCopyBasket, Request("txtMinbet2"), Request("txtMinbet" & sportId)))
            maxb = IIf(isCopyAll, Request("txtMaxbet1"), IIf(isCopyBasket, Request("txtMaxbet2"), Request("txtMaxbet" & sportId)))
            maxpermatch = IIf(isCopyAll, Request("txtMaxPerMatch1"), IIf(isCopyBasket, Request("txtMaxPerMatch2"), Request("txtMaxPerMatch" & sportId)))
        Else
            param.Add("updateminmaxbetv" & sportId, 0) 'bit                  
        End If
        '
   
        param.Add("minbetv11", minb) 'money  
        param.Add("maxbetv11", maxb) 'money  
        param.Add("maxpermatchv11", maxpermatch) 'money   
        '***** Start Inserting for MotoSports *****
        Dim mHdp, mML As Double
        Dim aHdp, aML As Double

        '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
        Dim autoHdp, autoML As Double
        Dim isAutoHdp, isAutoML As Double
        '/--------------------------------------------------------------------------------------------------------------

        If Not Request("chkUpdPT1") Is Nothing Or Not Request("chkUpdPT2") Is Nothing Or Not Request("chkUpdPT11") Is Nothing Or Not isMult Then
            If isCopyAll = False Then
                If isCopyBasket Then
                    param.Add("updatepositiontakingv" & sportId, IIf(Request("chkUpdPT2") Is Nothing, 0, 1)) 'bit 
                Else
                    param.Add("updatepositiontakingv" & sportId, IIf(Request("chkUpdPT" & sportId) Is Nothing, 0, 1)) 'bit 
                End If
            Else
                param.Add("updatepositiontakingv" & sportId, IIf(Request("chkUpdPT1") Is Nothing, 0, 1)) 'bit 
            End If

            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            isAutoHdp = IIf(Request("chkAuto11Hdp") Is Nothing, 0, 1)
            isAutoML = IIf(Request("chkAuto11ML") Is Nothing, 0, 1)
            '/--------------------------------------------------------------------------------------------------------------

            Dim isCopyHdp As Boolean = False
            If Not Request("chkCopy11Hdp") Is Nothing Then
                isCopyHdp = True

                isAutoML = isAutoHdp
            End If

            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            If isCopyAll Or isCopyBasket Then
                aHdp = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
                aML = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
            Else
                aHdp = IIf(isCopyHdp, Request("listM11Hdp"), Request("listM11Hdp"))
                aML = IIf(isCopyHdp, Request("listM11Hdp"), Request("listM11ML"))
            End If
            '/--------------------------------------------------------------------------------------------------------------

            If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
                If isCopyBasket Then

                    '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                    Dim isAuto2Hdp As Integer = 0
                    If Not Request("chkAuto2Hdp") Is Nothing Then
                        isAuto2Hdp = 1
                    End If
                    autoHdp = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                    autoML = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                    '/--------------------------------------------------------------------------------------------------------------

                    mHdp = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))
                    mML = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))

                    '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                    isAutoHdp = CInt(isAuto2Hdp)
                    isAutoML = CInt(isAuto2Hdp)
                    '/--------------------------------------------------------------------------------------------------------------

                Else

                    '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                    autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoS11Hdp"), Request("autoS11Hdp")), 0)
                    autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoS11Hdp"), Request("autoS11ML")), 0)
                    '/--------------------------------------------------------------------------------------------------------------

                    mHdp = IIf(isAutoHdp, 0, IIf(isCopyHdp, Request("listS11Hdp"), Request("listS11Hdp")))
                    mML = IIf(isAutoML, 0, IIf(isCopyHdp, Request("listS11Hdp"), Request("listS11ML")))
                End If
            Else

                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                Dim isAutoSoccer As Boolean = False
                If Not Request("chkAuto1HdpD") Is Nothing Then
                    isAutoSoccer = True
                End If
                autoHdp = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoML = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                '/--------------------------------------------------------------------------------------------------------------

                mHdp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mML = IIf(isAutoSoccer, 0, Request("listS1HdpD"))

                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                isAutoHdp = CInt(isAutoSoccer)
                isAutoML = CInt(isAutoSoccer)
                '/--------------------------------------------------------------------------------------------------------------

            End If
        Else
            param.Add("updatepositiontakingv11", 0) 'bit  
        End If
        param.Add("v11apositiontaking_1", aHdp) 'smallmoney         
        param.Add("v11apositiontaking_20", aML) 'smallmoney  

        param.Add("v11mpositiontaking_1", mHdp) 'smallmoney         
        param.Add("v11mpositiontaking_20", mML) 'smallmoney 

        '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
        If Not False Then
            param.Add("auto_v11mpositiontaking_1", autoHdp) 'smallmoney          
            param.Add("auto_v11mpositiontaking_20", autoML) 'smallmoney      

            param.Add("isauto_v11mpositiontaking_1", isAutoHdp) 'bit  
            param.Add("isauto_v11mpositiontaking_20", isAutoML) 'bit  
        End If
        '/--------------------------------------------------------------------------------------------------------------

        '***** End Inserting for MotoSports *****
    End Sub
#End Region

#Region "Insert parameters for Number Game Position Taking"
    Sub AddParaNumberGame(ByVal sportId As String, ByVal isMult As Boolean)

        Dim isDisableNumberGame As Boolean = False
        If (Not UserPermission.IsShowNumberGame) Then
            isDisableNumberGame = True
        End If

        Dim minb, maxb, maxpermatch, maxperball As Double
        If Not Request("chkUpdInfo" & sportId) Is Nothing Or Not isMult Then
            param.Add("updateminmaxbetv" & sportId, 1) 'bit  
            If (isDisableNumberGame) Then
                minb = Request("txtMinbet1")
                maxb = Request("txtMaxbet1")
                maxpermatch = Request("txtMaxPerMatch1")
                maxperball = maxpermatch
            Else
                minb = Request("txtMinbet" & sportId)
                maxb = Request("txtMaxbet" & sportId)
                maxpermatch = Request("txtMaxPerMatch" & sportId)
                maxperball = Request("txtMaxPerBall")
            End If

        Else
            If isDisableNumberGame And Not Request("chkUpdInfo1") Is Nothing Then
                param.Add("updateminmaxbetv" & sportId, 1) 'bit 
                minb = Request("txtMinbet1")
                maxb = Request("txtMaxbet1")
                maxpermatch = Request("txtMaxPerMatch1")
                maxperball = maxpermatch
            Else
                param.Add("updateminmaxbetv" & sportId, 0) 'bit 
            End If

        End If
        param.Add("minbetv" & sportId, minb) 'money  
        param.Add("maxbetv" & sportId, maxb) 'money  
        param.Add("maxpermatchv" & sportId, maxpermatch) 'money  
        param.Add("maxperballv" & sportId, maxperball) 'money  

        Dim m1stOU, m1stOE, mOE, mWR As Double
        Dim mOUL, mOEL, mHLL, mNCL, mNGL As Double

        Dim a1stOU, a1stOE, aOE, aWR As Double
        Dim aOUL, aOEL, aHLL, aNCL, aNGL As Double
        '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
        Dim auto1stOU, auto1stOE, autoOE, autoWR As Double
        Dim autoOUL, autoOEL, autoHLL, autoNCL, autoNGL As Double

        Dim isAuto1stOU, isAuto1stOE, isAutoOE, isAutoWR As Double
        Dim isAutoOUL, isAutoOEL, isAutoHLL, isAutoNCL, isAutoNGL As Double
        '/--------------------------------------------------------------------------------------------------------------

        If Not Request("chkUpdPT" & sportId) Is Nothing Or Not isMult Then
            param.Add("updatepositiontakingv" & sportId, 1) 'bit  

            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            isAuto1stOU = IIf(Request("chkAuto" & sportId & "1stOUD") Is Nothing, 0, 1)
            isAuto1stOE = IIf(Request("chkAuto" & sportId & "1stOED") Is Nothing, 0, 1)
            isAutoOE = IIf(Request("chkAuto" & sportId & "OED") Is Nothing, 0, 1)
            isAutoWR = IIf(Request("chkAuto" & sportId & "WRD") Is Nothing, 0, 1)

            isAutoOUL = IIf(Request("chkAuto" & sportId & "OUL") Is Nothing, 0, 1)
            isAutoOEL = IIf(Request("chkAuto" & sportId & "OEL") Is Nothing, 0, 1)
            isAutoHLL = IIf(Request("chkAuto" & sportId & "HLL") Is Nothing, 0, 1)
            isAutoNCL = IIf(Request("chkAuto" & sportId & "NCL") Is Nothing, 0, 1)
            isAutoNGL = IIf(Request("chkAuto" & sportId & "NGL") Is Nothing, 0, 1)
            '/--------------------------------------------------------------------------------------------------------------            

            Dim isCopy1stOUD As Boolean = False
            If Not Request("chkCopy" & sportId & "1stOUD") Is Nothing Then
                isCopy1stOUD = True
                isAuto1stOE = isAuto1stOU
                isAutoOE = isAuto1stOU
                isAutoWR = isAuto1stOU
            End If

            Dim isCopyOUL As Boolean = False
            If Not Request("chkCopy" & sportId & "OUL") Is Nothing Then
                isCopyOUL = True
                isAutoOEL = isAutoOUL
                isAutoHLL = isAutoOUL
                isAutoNCL = isAutoOUL
                isAutoNGL = isAutoOUL
            End If

            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            If isDisableNumberGame Then
                a1stOU = Request("listM1HdpD")
                a1stOE = Request("listM1HdpD")
                aOE = Request("listM1HdpD")
                aWR = Request("listM1HdpD")

                aOUL = Request("listM1HdpD")
                aOEL = Request("listM1HdpD")
                aHLL = Request("listM1HdpD")
                aNCL = Request("listM1HdpD")
                aNGL = Request("listM1HdpD")
            Else
                a1stOU = IIf(isCopy1stOUD, Request("listM" & sportId & "1stOUD"), Request("listM" & sportId & "1stOUD"))
                a1stOE = IIf(isCopy1stOUD, Request("listM" & sportId & "1stOUD"), Request("listM" & sportId & "1stOED"))
                aOE = IIf(isCopy1stOUD, Request("listM" & sportId & "1stOUD"), Request("listM" & sportId & "OED"))
                aWR = IIf(isCopy1stOUD, Request("listM" & sportId & "1stOUD"), Request("listM" & sportId & "WRD"))

                aOUL = IIf(isCopyOUL, Request("listM" & sportId & "OUL"), Request("listM" & sportId & "OUL"))
                aOEL = IIf(isCopyOUL, Request("listM" & sportId & "OUL"), Request("listM" & sportId & "OEL"))
                aHLL = IIf(isCopyOUL, Request("listM" & sportId & "OUL"), Request("listM" & sportId & "HLL"))
                aNCL = IIf(isCopyOUL, Request("listM" & sportId & "OUL"), Request("listM" & sportId & "NCL"))
                aNGL = IIf(isCopyOUL, IIf(UserPermission.IsShowNumberGroup, Request("listM" & sportId & "OUL"), Request("listM" & sportId & "NGL")), Request("listM" & sportId & "NGL"))
            End If

            '/--------------------------------------------------------------------------------------------------------------

            If Not isDisableNumberGame Then 'this case for Copy All PT from Soccer checkbox is unchecked
                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                auto1stOU = IIf(isAuto1stOU, IIf(isCopy1stOUD, Request("autoS" & sportId & "1stOUD"), Request("autoS" & sportId & "1stOUD")), 0)
                auto1stOE = IIf(isAuto1stOE, IIf(isCopy1stOUD, Request("autoS" & sportId & "1stOUD"), Request("autoS" & sportId & "1stOED")), 0)
                autoOE = IIf(isAutoOE, IIf(isCopy1stOUD, Request("autoS" & sportId & "1stOUD"), Request("autoS" & sportId & "OED")), 0)
                autoWR = IIf(isAutoWR, IIf(isCopy1stOUD, Request("autoS" & sportId & "1stOUD"), Request("autoS" & sportId & "WRD")), 0)

                autoOUL = IIf(isAutoOUL, IIf(isCopyOUL, Request("autoS" & sportId & "OUL"), Request("autoS" & sportId & "OUL")), 0)
                autoOEL = IIf(isAutoOEL, IIf(isCopyOUL, Request("autoS" & sportId & "OUL"), Request("autoS" & sportId & "OEL")), 0)
                autoHLL = IIf(isAutoHLL, IIf(isCopyOUL, Request("autoS" & sportId & "OUL"), Request("autoS" & sportId & "HLL")), 0)
                autoNCL = IIf(isAutoNCL, IIf(isCopyOUL, Request("autoS" & sportId & "OUL"), Request("autoS" & sportId & "NCL")), 0)
                autoNGL = IIf(isAutoNGL, IIf(isCopyOUL, IIf(UserPermission.IsShowNumberGroup, Request("autoS" & sportId & "OUL"), Request("autoS" & sportId & "NGL")), Request("autoS" & sportId & "NGL")), 0)
                '/--------------------------------------------------------------------------------------------------------------

                m1stOU = IIf(isAuto1stOU, 0, IIf(isCopy1stOUD, Request("listS" & sportId & "1stOUD"), Request("listS" & sportId & "1stOUD")))
                m1stOE = IIf(isAuto1stOE, 0, IIf(isCopy1stOUD, Request("listS" & sportId & "1stOUD"), Request("listS" & sportId & "1stOED")))
                mOE = IIf(isAutoOE, 0, IIf(isCopy1stOUD, Request("listS" & sportId & "1stOUD"), Request("listS" & sportId & "OED")))
                mWR = IIf(isAutoWR, 0, IIf(isCopy1stOUD, Request("listS" & sportId & "1stOUD"), Request("listS" & sportId & "WRD")))

                mOUL = IIf(isAutoOUL, 0, IIf(isCopyOUL, Request("listS" & sportId & "OUL"), Request("listS" & sportId & "OUL")))
                mOEL = IIf(isAutoOEL, 0, IIf(isCopyOUL, Request("listS" & sportId & "OUL"), Request("listS" & sportId & "OEL")))
                mHLL = IIf(isAutoHLL, 0, IIf(isCopyOUL, Request("listS" & sportId & "OUL"), Request("listS" & sportId & "HLL")))
                mNCL = IIf(isAutoNCL, 0, IIf(isCopyOUL, Request("listS" & sportId & "OUL"), Request("listS" & sportId & "NCL")))
                mNGL = IIf(isAutoNGL, 0, IIf(isCopyOUL, IIf(UserPermission.IsShowNumberGroup, Request("listS" & sportId & "OUL"), Request("listS" & sportId & "NGL")), Request("listS" & sportId & "NGL")))
            Else
                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                Dim isAutoSoccer As Boolean = False
                If Not Request("chkAuto1HdpD") Is Nothing Then
                    isAutoSoccer = True
                End If
                auto1stOU = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                auto1stOE = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoOE = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoWR = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)

                autoOUL = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoOEL = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoHLL = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoNCL = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoNGL = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                '/--------------------------------------------------------------------------------------------------------------

                m1stOU = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                m1stOE = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mOE = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mWR = IIf(isAutoSoccer, 0, Request("listS1HdpD"))

                mOUL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mOEL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mHLL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mNCL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mNGL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))

                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                isAuto1stOU = CInt(isAutoSoccer)
                isAuto1stOE = CInt(isAutoSoccer)
                isAutoOE = CInt(isAutoSoccer)
                isAutoWR = CInt(isAutoSoccer)

                isAutoOUL = CInt(isAutoSoccer)
                isAutoOEL = CInt(isAutoSoccer)
                isAutoHLL = CInt(isAutoSoccer)
                isAutoNCL = CInt(isAutoSoccer)
                isAutoNGL = CInt(isAutoSoccer)
                '/--------------------------------------------------------------------------------------------------------------


            End If
        Else
            param.Add("updatepositiontakingv" & sportId, 0) 'bit  
        End If
        param.Add("v" & sportId & "apositiontaking_81", a1stOU) 'smallmoney  
        param.Add("v" & sportId & "apositiontaking_83", a1stOE) 'smallmoney  
        param.Add("v" & sportId & "apositiontaking_86", aOE) 'smallmoney  
        param.Add("v" & sportId & "apositiontaking_88", aWR) 'smallmoney  

        param.Add("v" & sportId & "alivepositiontaking_85", aOUL) 'smallmoney  
        param.Add("v" & sportId & "alivepositiontaking_86", aOEL) 'smallmoney  
        param.Add("v" & sportId & "alivepositiontaking_87", aHLL) 'smallmoney 
        param.Add("v" & sportId & "alivepositiontaking_89", aNCL) 'smallmoney 
        param.Add("v" & sportId & "alivepositiontaking_90", aNGL) 'smallmoney 

        param.Add("v" & sportId & "mpositiontaking_81", m1stOU) 'smallmoney  
        param.Add("v" & sportId & "mpositiontaking_83", m1stOE) 'smallmoney  
        param.Add("v" & sportId & "mpositiontaking_86", mOE) 'smallmoney   
        param.Add("v" & sportId & "mpositiontaking_88", mWR) 'smallmoney   

        param.Add("v" & sportId & "mlivepositiontaking_85", mOUL) 'smallmoney  
        param.Add("v" & sportId & "mlivepositiontaking_86", mOEL) 'smallmoney  
        param.Add("v" & sportId & "mlivepositiontaking_87", mHLL) 'smallmoney 
        param.Add("v" & sportId & "mlivepositiontaking_89", mNCL) 'smallmoney 
        param.Add("v" & sportId & "mlivepositiontaking_90", mNGL) 'smallmoney 
        '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
        If Not False Then
            param.Add("auto_v" & sportId & "mpositiontaking_81", auto1stOU) 'smallmoney 
            param.Add("auto_v" & sportId & "mpositiontaking_83", auto1stOE) 'smallmoney  
            param.Add("auto_v" & sportId & "mpositiontaking_86", autoOE) 'smallmoney 
            param.Add("auto_v" & sportId & "mpositiontaking_88", autoWR) 'smallmoney 

            param.Add("auto_v" & sportId & "mlivepositiontaking_85", autoOUL) 'smallmoney 
            param.Add("auto_v" & sportId & "mlivepositiontaking_86", autoOEL) 'smallmoney  
            param.Add("auto_v" & sportId & "mlivepositiontaking_87", autoHLL) 'smallmoney  
            param.Add("auto_v" & sportId & "mlivepositiontaking_89", autoNCL) 'smallmoney  
            param.Add("auto_v" & sportId & "mlivepositiontaking_90", autoNGL) 'smallmoney  

            param.Add("isauto_v" & sportId & "mpositiontaking_81", isAuto1stOU) 'bit 
            param.Add("isauto_v" & sportId & "mpositiontaking_83", isAuto1stOE) 'bit  
            param.Add("isauto_v" & sportId & "mpositiontaking_86", isAutoOE) 'bit
            param.Add("isauto_v" & sportId & "mpositiontaking_88", isAutoWR) 'bit

            param.Add("isauto_v" & sportId & "mlivepositiontaking_85", isAutoOUL) 'bit 
            param.Add("isauto_v" & sportId & "mlivepositiontaking_86", isAutoOEL) 'bit  
            param.Add("isauto_v" & sportId & "mlivepositiontaking_87", isAutoHLL) 'bit 
            param.Add("isauto_v" & sportId & "mlivepositiontaking_89", isAutoNCL) 'bit 
            param.Add("isauto_v" & sportId & "mlivepositiontaking_90", isAutoNGL) 'bit 

        End If
        '/--------------------------------------------------------------------------------------------------------------

    End Sub
#End Region

#Region "Insert parameters for MixedSports Position Taking"
    Sub AddParaMixedSports(ByVal sportId As String, ByVal isMult As Boolean)

        Dim minb, maxb, maxpermatch As Double
        ' 2/2/2012 changed by David
        If Not Request("chkUpdInfo1") Is Nothing Or Not Request("chkUpdInfo2") Is Nothing Or Not Request("chkUpdInfo" & sportId) Is Nothing Or Not isMult Then
            If isCopyAll = False Then
                If isCopyBasket Then
                    param.Add("updateminmaxbetv99mp", IIf(Request("chkUpdInfo2") Is Nothing, 0, 1)) 'bit 
                Else
                    param.Add("updateminmaxbetv99mp", IIf(Request("chkUpdInfo" & sportId) Is Nothing, 0, 1)) 'bit 
                End If
            Else
                param.Add("updateminmaxbetv99mp", IIf(Request("chkUpdInfo1") Is Nothing, 0, 1)) 'bit 
            End If
            minb = IIf(isCopyAll, Request("txtMinbet1"), IIf(isCopyBasket, Request("txtMinbet2"), Request("txtMinbet" & sportId)))
            maxb = IIf(isCopyAll, Request("txtMaxbet1"), IIf(isCopyBasket, Request("txtMaxbet2"), Request("txtMaxbet" & sportId)))
            maxpermatch = IIf(isCopyAll, Request("txtMaxPerMatch1"), IIf(isCopyBasket, Request("txtMaxPerMatch2"), Request("txtMaxPerMatch" & sportId)))
        Else
            param.Add("updateminmaxbetv99mp", 0) 'bit                  
        End If
        '

        param.Add("minbetv99mp", minb) 'money  
        param.Add("maxbetv99mp", maxb) 'money  
        param.Add("maxpermatchv99mp", maxpermatch) 'money  

        Dim mmp As Double
        Dim amp As Double
        '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
        Dim automp As Double
        Dim isAutomp As Double
        '/--------------------------------------------------------------------------------------------------------------

        If Not Request("chkUpdPT1") Is Nothing Or Not Request("chkUpdPT2") Is Nothing Or Not Request("chkUpdPT" & sportId) Is Nothing Or Not isMult Then
            If isCopyAll = False Then
                If isCopyBasket Then
                    param.Add("updatepositiontakingv99mp", IIf(Request("chkUpdPT2") Is Nothing, 0, 1)) 'bit 
                Else
                    param.Add("updatepositiontakingv99mp", IIf(Request("chkUpdPT" & sportId) Is Nothing, 0, 1)) 'bit 
                End If
            Else
                param.Add("updatepositiontakingv99mp", IIf(Request("chkUpdPT1") Is Nothing, 0, 1)) 'bit 
            End If

            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            isAutomp = IIf(Request("chkAuto99_M") Is Nothing, 0, 1)
            '/--------------------------------------------------------------------------------------------------------------            

            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            If isCopyAll Or isCopyBasket Then
                amp = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
            Else
                amp = Request("listM99_M")
            End If
            '/--------------------------------------------------------------------------------------------------------------

            If Not isCopyAll Then 'this case for Copy All PT from Soccer checkbox is unchecked
                If isCopyBasket Then 'this case for Copy All PT from Basketball checkbox is checked    

                    '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                    Dim isAuto2Hdp As Integer = 0
                    If Not Request("chkAuto2Hdp") Is Nothing Then
                        isAuto2Hdp = 1
                    End If

                    automp = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                    '/--------------------------------------------------------------------------------------------------------------

                    mmp = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))

                    '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                    isAutomp = CInt(isAuto2Hdp)
                    '/--------------------------------------------------------------------------------------------------------------

                Else
                    '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                    automp = IIf(isAutomp, Request("autoS99_M"), 0)
                    '/--------------------------------------------------------------------------------------------------------------

                    mmp = IIf(isAutomp, 0, Request("listS99_M"))

                End If
            Else
                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                Dim isAutoSoccer As Boolean = False
                If Not Request("chkAuto1HdpD") Is Nothing Then
                    isAutoSoccer = True
                End If
                automp = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                '/--------------------------------------------------------------------------------------------------------------

                mmp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                isAutomp = CInt(isAutoSoccer)
                '/--------------------------------------------------------------------------------------------------------------


            End If
        Else
            param.Add("updatepositiontakingv99mp", 0) 'bit 
        End If
        param.Add("v99apositiontaking_9", amp) 'smallmoney        
        param.Add("v99mpositiontaking_9", mmp) 'smallmoney          
        '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
        If Not False Then
            param.Add("auto_v99mpositiontaking_9", automp) 'smallmoney             
            param.Add("isauto_v99mpositiontaking_9", isAutomp) 'bit             
        End If
        '/--------------------------------------------------------------------------------------------------------------

    End Sub
#End Region

#Region "ReplaceData"
    Function ReplaceData(ByVal str As String) As String
        str = Server.HtmlEncode(str)
        Return str.Replace("""", "&quot;").Replace("'", "&#39;").Replace(vbNewLine, "")
    End Function
#End Region
End Class

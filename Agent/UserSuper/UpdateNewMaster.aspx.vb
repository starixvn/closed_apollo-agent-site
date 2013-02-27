Imports System.Collections
Imports DNAide.Data
Imports DNAide.Core
Imports DNAide.Web
Imports DNAide.Utils
Imports System.Data
Imports System.Web.Security
Imports HandHistoryLibrary.DataHandler
Imports Sunplus.Agent.Model.Entities.Permission
Imports Fanex.Data.DNAide

Public Class UpdateNewMaster
    Inherits System.Web.UI.Page
    Dim param As New Hashtable
    Dim isCopyAll As Boolean = False
    Dim isCopyBasket As Boolean = False
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal ByVae As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If check(Me) = True Then
            Exit Sub
        End If

        Dim errMsg As String = ""
        If (Not Sunplus.Agent.Model.Entities.UserSession.Current.SubPageAccess.CreateAccount) Then
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
            InsertData()
        End If


    End Sub
#Region "Update data"
    Sub InsertData()

        param.Add("custid", 0) 'int  
        param.Add("firstname", ReplaceData(Request("txtFName"))) 'nvarchar  
        param.Add("lastname", ReplaceData(Request("txtLName"))) 'nvarchar  
        param.Add("creator", Sunplus.Agent.Model.Entities.UserSession.Current.UserId) 'int  
        Dim username As String = Sunplus.Agent.Model.Entities.UserSession.Current.UserName & Request("number1").Trim & Request("number2").Trim
        param.Add("username", username)  'varchar  

        param.Add("userpwd", Encryption.CfsEnCode(Request("txtPwd").Trim)) 'varchar  
        param.Add("phone", ReplaceData(Request("txtPhone"))) 'nvarchar  
        param.Add("mobilephone", ReplaceData(Request("txtMobile"))) 'nvarchar  
        param.Add("fax", ReplaceData(Request("txtFax"))) 'nvarchar  
        param.Add("email", "") 'nvarchar  
        param.Add("address", "") 'nvarchar  
        param.Add("city", "") 'nvarchar  
        param.Add("state", "") 'nvarchar  
        param.Add("country", "") 'nvarchar  
        param.Add("postal", "") 'nvarchar  
        Dim strUpdTranfer As String
        If Request("isTransferOption") = "1" Then
            If Request("tranfer") = "daily" Then
                strUpdTranfer = "00000000" '
            Else
                If Not Request("chkSun") Is Nothing Then
                    strUpdTranfer &= "1"
                Else
                    strUpdTranfer &= "0"
                End If
                If Not Request("chkMon") Is Nothing Then
                    strUpdTranfer &= "1"
                Else
                    strUpdTranfer &= "0"
                End If
                If Not Request("chkTue") Is Nothing Then
                    strUpdTranfer &= "1"
                Else
                    strUpdTranfer &= "0"
                End If
                If Not Request("chkWed") Is Nothing Then
                    strUpdTranfer &= "1"
                Else
                    strUpdTranfer &= "0"
                End If
                If Not Request("chkThurs") Is Nothing Then
                    strUpdTranfer &= "1"
                Else
                    strUpdTranfer &= "0"
                End If
                If Not Request("chkFri") Is Nothing Then
                    strUpdTranfer &= "1"
                Else
                    strUpdTranfer &= "0"
                End If
                If Not Request("chkSat") Is Nothing Then
                    strUpdTranfer &= "1"
                Else
                    strUpdTranfer &= "0"
                End If
                strUpdTranfer &= "1"
            End If
            param.Add("transferoption", strUpdTranfer) 'varchar         
        End If


        If Not Request("chkCopyAll") Is Nothing Then
            isCopyAll = True
        End If
        If Not Request("chkCopyBasket") Is Nothing Then
            isCopyBasket = True
        End If
        param.Add("credit", Request("txtCredit")) 'money  
        param.Add("srecommend", Sunplus.Agent.Model.Entities.UserSession.Current.CustId) 'int 

        param.Add("groupa", Request("groupa")) 'decimal  
        param.Add("groupb", Request("groupb")) 'decimal  
        param.Add("groupc", Request("groupc")) 'decimal  
        param.Add("groupd", Request("groupd")) 'decimal  
        param.Add("discountcs", Request("discountcs")) 'decimal          
        param.Add("discount1x2", Request("discount1x2")) 'decimal
        param.Add("discountnumber", IIf(UserPermission.IsShowNumberGame, Request("disnumbergame"), 0)) 'decimal

        If Not Request("chkSuspended") Is Nothing Then
            param.Add("suspended", 1) 'bit 
        Else
            param.Add("suspended", CastAide.AsInt32(Request("IsSuspended"), 0)) 'bit 
        End If

        param.Add("disableautompt", IIf(Request("chkallowAutoMPT") Is Nothing, 1, 0))

        param.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey)
        param.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId) 'int     
        param.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP())
        param.Add("returnerrcode", 1) 'bit  
        param.Add("errcode", 0) 'int  
        param.Add("errmsg", "") 'nvarchar  
        param.Add("site", Common.GetLevel(Sunplus.Agent.Model.Entities.UserSession.Current.RoleId)) 'varchar  

        AddParaSoccer()
        AddParaBasketball(2)
        AddParaFootball(3)
        AddParaFootball(5)
        AddParaBaseball()
        AddParaFootball(10)
        AddParaMotosports()
        AddParaFootball(99)
        AddParaNumberGame()
        AddParaMixedSports()
        Dim rs As DbResult = AccountInfo.Acc_CustomerInsMember_SAg(param)

        If CastAide.AsInt32(rs.Outputs("errcode").ToString()) <> 0 Then
            Response.Write("<script type=""text/javascript"">parent.UpdateErr('" & rs.Outputs("errmsg").ToString() & "');</script>")
        Else
            Dim MyStatus As String = ""
            Dim drStatus As DbResult = AccountInfo.Age_CustomerSelStatus(Sunplus.Agent.Model.Entities.UserSession.Current.CustId)
            Dim row As DataRow = drStatus.ResultSet.Tables(0).Rows(0)
            Dim isStatusCS As Boolean = False
            If (UserPermission.IsShowCasino) Then
                Session(Globals.SESSION_CUSTOMER) = DAL.IBCClient.IBCLibProvider.Instance.GetCustInfoIBC(rs.Outputs("custid").ToString(), param)
                isStatusCS = True
            End If

            Dim isDisableFI As String = "1"
            Dim isDisableHR As String = IIf(Not UserPermission.IsShowRacing, "1", "0")
            Dim isDisableCas As String = IIf(CastAide.AsBoolean(row("DisableCasino")) = True Or isStatusCS = False, "1", "0")
            Dim isDisableBG As String = IIf(UserPermission.IsShowBingo, "0", "1")
            Dim isDisableLiveCS As String = IIf(UserPermission.IsShowLiveCasino, "0", "1")
            Response.Write("<script type=""text/javascript"">parent.RedirectHRFICS('" & username & "','" & rs.Outputs("custid").ToString() & "','" & isDisableFI & "','" & isDisableCas & "','" & isDisableHR & "','" & isDisableBG & "','" & isDisableLiveCS & "');</script>")
        End If

    End Sub
#End Region

#Region "Insert parameters for Soccer Position Taking"
    Sub AddParaSoccer()
        'Add Bet Setting parameters
        param.Add("minbet", Request("txtMinbet1")) 'money  
        param.Add("maxbet", Request("txtMaxbet1")) 'money  
        param.Add("maxpermatch", Request("txtMaxPerMatch1")) 'money 
        '**************************
        '***** Add Position Taking paramaters*****
        Dim mHdpD, mOUD, m1stHdpD, m1stOUD, mOE, m1x2, mCS, mTG, mMp, mOR As Double
        Dim sHdpD, sOUD, s1stHdpD, s1stOUD, sOE, s1x2, sCS, sTG, sMp, sOR As Double
        Dim autoHdpD, autoOUD, auto1stHdpD, auto1stOUD, autoOE, auto1x2, autoCS, autoTG, autoMp, autoOR As Double

        Dim mHdpL, mOUL, m1stHdpL, m1stOUL As Double
        Dim sHdpL, sOUL, s1stHdpL, s1stOUL As Double
        Dim autoHdpL, autoOUL, auto1stHdpL, auto1stOUL As Double

        'Dim isAuto As Boolean = False
        '----
        Dim isAutoHdpD, isAutoOUD, isAuto1stHdpD, isAuto1stOUD, isAutoOE, isAuto1x2, isAutoCS, isAutoTG, isAutoMP, isAutoOR As Boolean
        Dim isAutoHdpL, isAutoOUL, isAuto1stHdpL, isAuto1stOUL As Boolean

        isAutoHdpD = IIf(Request("chkAuto1HdpD") Is Nothing, 0, 1)
        isAutoOUD = IIf(Request("chkAuto1OUD") Is Nothing, 0, 1)
        isAuto1stHdpD = IIf(Request("chkAuto11stHdpD") Is Nothing, 0, 1)
        isAuto1stOUD = IIf(Request("chkAuto11stOUD") Is Nothing, 0, 1)
        isAutoOE = IIf(Request("chkAuto1OE") Is Nothing, 0, 1)

        isAuto1x2 = IIf(Request("chkAuto11x2") Is Nothing, 0, 1)
        isAutoCS = IIf(Request("chkAuto1CS") Is Nothing, 0, 1)
        isAutoTG = IIf(Request("chkAuto1TG") Is Nothing, 0, 1)
        isAutoMP = IIf(Request("chkAuto1MP") Is Nothing, 0, 1)
        isAutoOR = IIf(Request("chkAuto1OR") Is Nothing, 0, 1)

        isAutoHdpL = IIf(Request("chkAuto1HdpL") Is Nothing, 0, 1)
        isAutoOUL = IIf(Request("chkAuto1OUL") Is Nothing, 0, 1)
        isAuto1stHdpL = IIf(Request("chkAuto11stHdpL") Is Nothing, 0, 1)
        isAuto1stOUL = IIf(Request("chkAuto11stOUL") Is Nothing, 0, 1)

        '----
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
            isAutoMP = isAuto1x2
            isAutoOR = isAuto1x2
        End If
        Dim isCopyHdpL As Boolean = False
        If Not Request("chkCopy1HdpLive") Is Nothing Then
            isCopyHdpL = True
            isAutoOUL = isAutoHdpL
            isAuto1stHdpL = isAutoHdpL
            isAuto1stOUL = isAutoHdpL
        End If

        mHdpD = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM1HdpD")))
        mOUD = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM1OUD")))
        m1stHdpD = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM11stHdpD")))
        m1stOUD = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM11stOUD")))
        mOE = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM1OE")))

        m1x2 = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopy1x2, Request("listM11x2"), Request("listM11x2")))
        mCS = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopy1x2, Request("listM11x2"), Request("listM1CS")))
        mTG = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopy1x2, Request("listM11x2"), Request("listM1TG")))
        mMp = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopy1x2, Request("listM11x2"), Request("listM1MP")))
        mOR = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopy1x2, Request("listM11x2"), Request("listM1OR")))

        mHdpL = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM1HdpL")))
        mOUL = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM1OUL")))
        m1stHdpL = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM11stHdpL")))
        m1stOUL = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM11stOUL")))

        If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
            autoHdpD = IIf(isAutoHdpD, IIf(isCopyHdpD, Request("autoS1HdpD"), Request("autoS1HdpD")), 0)
            autoOUD = IIf(isAutoOUD, IIf(isCopyHdpD, Request("autoS1HdpD"), Request("autoS1OUD")), 0)
            auto1stHdpD = IIf(isAuto1stHdpD, IIf(isCopyHdpD, Request("autoS1HdpD"), Request("autoS11stHdpD")), 0)
            auto1stOUD = IIf(isAuto1stOUD, IIf(isCopyHdpD, Request("autoS1HdpD"), Request("autoS11stOUD")), 0)
            autoOE = IIf(isAutoOE, IIf(isCopyHdpD, Request("autoS1HdpD"), Request("autoS1OE")), 0)

            auto1x2 = IIf(isAuto1x2, IIf(isCopy1x2, Request("autoS11x2"), Request("autoS11x2")), 0)
            autoCS = IIf(isAutoCS, IIf(isCopy1x2, Request("autoS11x2"), Request("autoS1CS")), 0)
            autoTG = IIf(isAutoTG, IIf(isCopy1x2, Request("autoS11x2"), Request("autoS1TG")), 0)
            autoMp = IIf(isAutoMP, IIf(isCopy1x2, Request("autoS11x2"), Request("autoS1MP")), 0)
            autoOR = IIf(isAutoOR, IIf(isCopy1x2, Request("autoS11x2"), Request("autoS1OR")), 0)

            autoHdpL = IIf(isAutoHdpL, IIf(isCopyHdpL, Request("autoS1HdpL"), Request("autoS1HdpL")), 0)
            autoOUL = IIf(isAutoOUL, IIf(isCopyHdpL, Request("autoS1HdpL"), Request("autoS1OUL")), 0)
            auto1stHdpL = IIf(isAuto1stHdpL, IIf(isCopyHdpL, Request("autoS1HdpL"), Request("autoS11stHdpL")), 0)
            auto1stOUL = IIf(isAuto1stOUL, IIf(isCopyHdpL, Request("autoS1HdpL"), Request("autoS11stOUL")), 0)

            sHdpD = IIf(isAutoHdpD, 0, IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS1HdpD")))
            sOUD = IIf(isAutoOUD, 0, IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS1OUD")))
            s1stHdpD = IIf(isAuto1stHdpD, 0, IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS11stHdpD")))
            s1stOUD = IIf(isAuto1stOUD, 0, IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS11stOUD")))
            sOE = IIf(isAutoOE, 0, IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS1OE")))

            s1x2 = IIf(isAuto1x2, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS11x2")))
            sCS = IIf(isAutoCS, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS1CS")))
            sTG = IIf(isAutoTG, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS1TG")))
            sMp = IIf(isAutoMP, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS1MP")))
            sOR = IIf(isAutoOR, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS1OR")))

            sHdpL = IIf(isAutoHdpL, 0, IIf(isCopyHdpL, Request("listS1HdpL"), Request("listS1HdpL")))
            sOUL = IIf(isAutoOUL, 0, IIf(isCopyHdpL, Request("listS1HdpL"), Request("listS1OUL")))
            s1stHdpL = IIf(isAuto1stHdpL, 0, IIf(isCopyHdpL, Request("listS1HdpL"), Request("listS11stHdpL")))
            s1stOUL = IIf(isAuto1stOUL, 0, IIf(isCopyHdpL, Request("listS1HdpL"), Request("listS11stOUL")))
        Else
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

            sHdpD = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sOUD = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            s1stHdpD = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            s1stOUD = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sOE = IIf(isAutoSoccer, 0, Request("listS1HdpD"))

            s1x2 = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sCS = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sTG = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sMp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sOR = IIf(isAutoSoccer, 0, Request("listS1HdpD"))

            sHdpL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sOUL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            s1stHdpL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            s1stOUL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
        End If

        param.Add("spositiontaking_1", sHdpD) 'smallmoney  
        param.Add("spositiontaking_3", sOUD) 'smallmoney  
        param.Add("spositiontaking_7", s1stHdpD) 'smallmoney  
        param.Add("spositiontaking_8", s1stOUD) 'smallmoney  
        param.Add("spositiontaking_2", sOE) 'smallmoney  

        param.Add("spositiontaking_5", s1x2) 'smallmoney 
        param.Add("spositiontaking_4", sCS) 'smallmoney
        param.Add("spositiontaking_6", sTG) 'smallmoney            
        param.Add("spositiontaking_9", sMp) 'smallmoney  
        param.Add("spositiontaking_10", sOR) 'smallmoney  

        param.Add("slivepositiontaking_1", sHdpL) 'smallmoney  
        param.Add("slivepositiontaking_3", sOUL) 'smallmoney  
        param.Add("slivepositiontaking_7", s1stHdpL) 'smallmoney  
        param.Add("slivepositiontaking_8", s1stOUL) 'smallmoney  

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

        param.Add("auto_spositiontaking_1", autoHdpD) 'smallmoney    
        param.Add("auto_spositiontaking_3", autoOUD) 'smallmoney    
        param.Add("auto_spositiontaking_7", auto1stHdpD) 'smallmoney 
        param.Add("auto_spositiontaking_8", auto1stOUD) 'smallmoney  
        param.Add("auto_spositiontaking_2", autoOE) 'smallmoney                  

        param.Add("auto_spositiontaking_5", auto1x2) 'smallmoney 
        param.Add("auto_spositiontaking_4", autoCS) 'smallmoney
        param.Add("auto_spositiontaking_6", autoTG) 'smallmoney             
        param.Add("auto_spositiontaking_9", autoMp) 'smallmoney  
        param.Add("auto_spositiontaking_10", autoOR) 'smallmoney 

        param.Add("auto_slivepositiontaking_1", autoHdpL) 'smallmoney     
        param.Add("auto_slivepositiontaking_3", autoOUL) 'smallmoney     
        param.Add("auto_slivepositiontaking_7", auto1stHdpL) 'smallmoney   
        param.Add("auto_slivepositiontaking_8", auto1stOUL) 'smallmoney 

        '***** End Inserting for Soccer *****
    End Sub
#End Region

#Region "Insert parameters for Basketball Position Taking"
    Sub AddParaBasketball(ByVal sportId As String)
        Dim minb, maxb, maxpermatch As Double
        minb = IIf(isCopyAll, Request("txtMinbet1"), IIf(isCopyBasket, Request("txtMinbet2"), Request("txtMinbet" + sportId)))
        maxb = IIf(isCopyAll, Request("txtMaxbet1"), IIf(isCopyBasket, Request("txtMaxbet2"), Request("txtMaxbet" + sportId)))
        maxpermatch = IIf(isCopyAll, Request("txtMaxPerMatch1"), IIf(isCopyBasket, Request("txtMaxPerMatch2"), Request("txtMaxPerMatch" + sportId)))
        param.Add("minbetv" + sportId, minb) 'money  
        param.Add("maxbetv" + sportId, maxb) 'money  
        param.Add("maxpermatchv" + sportId, maxpermatch) 'money  
        '***** Start Inserting for Basketball *****
        Dim mHdp, mOU, mOE, mML As Double
        Dim sHdp, sOU, sOE, sML As Double
        Dim autoHdp, autoOU, autoOE, autoML As Double

        'Dim isAuto As Boolean = False
        'If Not Request("chkAuto" & sportId) Is Nothing Then
        '    isAuto = True
        'End If
        '----
        Dim isAutoHdp, isAutoOU, isAutoOE, isAutoML As Boolean


        isAutoHdp = IIf(Request("chkAuto2Hdp") Is Nothing, 0, 1)
        isAutoOU = IIf(Request("chkAuto2OU") Is Nothing, 0, 1)
        isAutoOE = IIf(Request("chkAuto2OE") Is Nothing, 0, 1)
        isAutoML = IIf(Request("chkAuto2ML") Is Nothing, 0, 1)
        '----
        Dim isCopyHdp As Boolean = False
        If Not Request("chkCopy2Hdp") Is Nothing Then
            isCopyHdp = True
            isAutoOU = isAutoHdp
            isAutoOE = isAutoHdp
            isAutoML = isAutoHdp
        End If

        If isCopyAll Or isCopyBasket Then
            mHdp = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
            mOU = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
            mOE = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))

            mML = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
        Else
            mHdp = IIf(isCopyHdp, Request("listM2Hdp"), Request("listM2Hdp"))
            mOU = IIf(isCopyHdp, Request("listM2Hdp"), Request("listM2OU"))
            mOE = IIf(isCopyHdp, Request("listM2Hdp"), Request("listM2OE"))

            mML = IIf(isCopyHdp, Request("listM2Hdp"), Request("listM2ML"))
        End If

        If Not isCopyAll Then 'this case for Copy All PT from Soccer checkbox is unchecked
            If isCopyBasket Then 'this case for Copy All PT from Basketball checkbox is checked
                Dim isAutoBasket As Boolean = False
                If Not Request("chkAuto2Hdp") Is Nothing Then
                    isAutoBasket = True

                End If
                autoHdp = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoOU = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoOE = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoML = IIf(isAutoBasket, Request("autoS2Hdp"), 0)

                sHdp = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                sOU = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                sOE = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                sML = IIf(isAutoBasket, 0, Request("listS2Hdp"))
            Else
                autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoS2Hdp"), Request("autoS2Hdp")), 0)
                autoOU = IIf(isAutoOU, IIf(isCopyHdp, Request("autoS2Hdp"), Request("autoS2OU")), 0)
                autoOE = IIf(isAutoOE, IIf(isCopyHdp, Request("autoS2Hdp"), Request("autoS2OE")), 0)
                autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoS2Hdp"), Request("autoS2ML")), 0)

                sHdp = IIf(isAutoHdp, 0, IIf(isCopyHdp, Request("listS2Hdp"), Request("listS2Hdp")))
                sOU = IIf(isAutoOU, 0, IIf(isCopyHdp, Request("listS2Hdp"), Request("listS2OU")))
                sOE = IIf(isAutoOE, 0, IIf(isCopyHdp, Request("listS2Hdp"), Request("listS2OE")))
                sML = IIf(isAutoML, 0, IIf(isCopyHdp, Request("listS2Hdp"), Request("listS2ML")))
            End If
        Else
            Dim isAutoSoccer As Boolean = False
            If Not Request("chkAuto1HdpD") Is Nothing Then
                isAutoSoccer = True
            End If

            autoHdp = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoOU = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoOE = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoML = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)

            sHdp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sOU = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sOE = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sML = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
        End If

        param.Add("v2spositiontaking_1", sHdp) 'smallmoney  
        param.Add("v2spositiontaking_3", sOU) 'smallmoney  
        param.Add("v2spositiontaking_7", sHdp) 'smallmoney 
        param.Add("v2spositiontaking_8", sOU) 'smallmoney  
        param.Add("v2spositiontaking_2", sOE) 'smallmoney  
        param.Add("v2spositiontaking_20", sML) 'smallmoney          

        param.Add("v2mpositiontaking_1", mHdp) 'smallmoney  
        param.Add("v2mpositiontaking_3", mOU) 'smallmoney  
        param.Add("v2mpositiontaking_7", mHdp) 'smallmoney 
        param.Add("v2mpositiontaking_8", mOU) 'smallmoney 
        param.Add("v2mpositiontaking_2", mOE) 'smallmoney
        param.Add("v2mpositiontaking_20", mML) 'smallmoney 

        param.Add("auto_v2spositiontaking_1", autoHdp) 'smallmoney  
        param.Add("auto_v2spositiontaking_3", autoOU) 'smallmoney  
        param.Add("auto_v2spositiontaking_7", autoHdp) 'smallmoney  
        param.Add("auto_v2spositiontaking_8", autoOU) 'smallmoney 
        param.Add("auto_v2spositiontaking_2", autoOE) 'smallmoney  
        param.Add("auto_v2spositiontaking_20", autoML) 'smallmoney 

        '***** End Inserting for Football *****
    End Sub
#End Region

#Region "Insert parameters for Football, Tennis, Golf, Other Sports Position Taking"
    Sub AddParaFootball(ByVal sportId As String)
        Dim minb, maxb, maxpermatch As Double
        minb = IIf(isCopyAll, Request("txtMinbet1"), IIf(isCopyBasket, Request("txtMinbet2"), Request("txtMinbet" + sportId)))
        maxb = IIf(isCopyAll, Request("txtMaxbet1"), IIf(isCopyBasket, Request("txtMaxbet2"), Request("txtMaxbet" + sportId)))
        maxpermatch = IIf(isCopyAll, Request("txtMaxPerMatch1"), IIf(isCopyBasket, Request("txtMaxPerMatch2"), Request("txtMaxPerMatch" + sportId)))
        param.Add("minbetv" + sportId, minb) 'money  
        param.Add("maxbetv" + sportId, maxb) 'money  
        param.Add("maxpermatchv" + sportId, maxpermatch) 'money  
        '***** Start Inserting for Football *****
        Dim mHdp, mOU, mOE, mML As Double
        Dim sHdp, sOU, sOE, sML As Double
        Dim autoHdp, autoOU, autoOE, autoML As Double

        'Dim isAuto As Boolean = False
        'If Not Request("chkAuto" & sportId) Is Nothing Then
        '    isAuto = True
        'End If
        '----
        Dim isAutoHdp, isAutoOU, isAutoOE, isAutoML As Boolean


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

        If isCopyAll Or isCopyBasket Then
            mHdp = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
            mOU = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
            mOE = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
            mML = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
        Else
            mHdp = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "Hdp"))
            mOU = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "OU"))
            mOE = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "OE"))
            mML = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "ML"))
        End If

        If Not isCopyAll Then 'this case for Copy All PT from Soccer checkbox is unchecked
            If isCopyBasket Then 'this case for Copy All PT from Basketball checkbox is checked
                Dim isAutoBasket As Boolean = False
                If Not Request("chkAuto2Hdp") Is Nothing Then
                    isAutoBasket = True
                End If
                autoHdp = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoOU = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoOE = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoML = IIf(isAutoBasket, Request("autoS2Hdp"), 0)

                sHdp = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                sOU = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                sOE = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                sML = IIf(isAutoBasket, 0, Request("listS2Hdp"))
            Else
                autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "Hdp")), 0)
                autoOU = IIf(isAutoOU, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "OU")), 0)
                autoOE = IIf(isAutoOE, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "OE")), 0)
                autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "ML")), 0)

                sHdp = IIf(isAutoHdp, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "Hdp")))
                sOU = IIf(isAutoOU, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "OU")))
                sOE = IIf(isAutoOE, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "OE")))
                sML = IIf(isAutoML, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "ML")))
            End If
        Else
            Dim isAutoSoccer As Boolean = False
            If Not Request("chkAuto1HdpD") Is Nothing Then
                isAutoSoccer = True
            End If
            autoHdp = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoOU = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoOE = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoML = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)

            sHdp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sOU = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sOE = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sML = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
        End If

        param.Add("v" & sportId & "spositiontaking_1", sHdp) 'smallmoney  
        param.Add("v" & sportId & "spositiontaking_3", sOU) 'smallmoney  
        param.Add("v" & sportId & "spositiontaking_2", sOE) 'smallmoney  
        param.Add("v" & sportId & "spositiontaking_20", sML) 'smallmoney  

        param.Add("v" & sportId & "mpositiontaking_1", mHdp) 'smallmoney  
        param.Add("v" & sportId & "mpositiontaking_3", mOU) 'smallmoney  
        param.Add("v" & sportId & "mpositiontaking_2", mOE) 'smallmoney
        param.Add("v" & sportId & "mpositiontaking_20", mML) 'smallmoney 

        param.Add("auto_v" & sportId & "spositiontaking_1", autoHdp) 'smallmoney  
        param.Add("auto_v" & sportId & "spositiontaking_2", autoOE) 'smallmoney  
        param.Add("auto_v" & sportId & "spositiontaking_3", autoOU) 'smallmoney  
        param.Add("auto_v" & sportId & "spositiontaking_20", autoML) 'smallmoney 

        '***** End Inserting for Football *****
    End Sub
#End Region

#Region "Insert parameters for Baseball Position Taking"
    Sub AddParaBaseball()
        Dim minb, maxb, maxpermatch As Double
        minb = IIf(isCopyAll, Request("txtMinbet1"), IIf(isCopyBasket, Request("txtMinbet2"), Request("txtMinbet8")))
        maxb = IIf(isCopyAll, Request("txtMaxbet1"), IIf(isCopyBasket, Request("txtMaxbet2"), Request("txtMaxbet8")))
        maxpermatch = IIf(isCopyAll, Request("txtMaxPerMatch1"), IIf(isCopyBasket, Request("txtMaxPerMatch2"), Request("txtMaxPerMatch8")))
        param.Add("minbetv8", minb)  'money  
        param.Add("maxbetv8", maxb) 'money  
        param.Add("maxpermatchv8", maxpermatch) 'money  
        '***** Start Inserting for Baseball *****
        Dim mHdp, mOU, mML As Double
        Dim sHdp, sOU, sML As Double
        Dim autoHdp, autoOU, autoML As Double

        'Dim isAuto As Boolean = False
        'If Not Request("chkAuto8") Is Nothing Then
        '    isAuto = True
        'End If
        '----
        Dim isAutoHdp, isAutoOU, isAutoML As Boolean

        isAutoHdp = IIf(Request("chkAuto8Hdp") Is Nothing, 0, 1)
        isAutoOU = IIf(Request("chkAuto8OU") Is Nothing, 0, 1)
        isAutoML = IIf(Request("chkAuto8ML") Is Nothing, 0, 1)
        '----

        Dim isCopyHdp As Boolean = False
        If Not Request("chkCopy8Hdp") Is Nothing Then
            isCopyHdp = True
            isAutoOU = isAutoHdp
            isAutoML = isAutoHdp
        End If
        'this case for Copy All from Soccer or Basketball checkbox is checked
        If isCopyAll Or isCopyBasket Then
            mHdp = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
            mOU = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
            mML = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
        Else
            mHdp = IIf(isCopyHdp, Request("listM8Hdp"), Request("listM8Hdp"))
            mOU = IIf(isCopyHdp, Request("listM8Hdp"), Request("listM8OU"))
            mML = IIf(isCopyHdp, Request("listM8Hdp"), Request("listM8ML"))
        End If

        If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
            If isCopyBasket Then
                Dim isAutoBasket As Boolean = False
                If Not Request("chkAuto2Hdp") Is Nothing Then
                    isAutoBasket = True
                End If
                autoHdp = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoOU = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoML = IIf(isAutoBasket, Request("autoS2Hdp"), 0)

                sHdp = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                sOU = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                sML = IIf(isAutoBasket, 0, Request("listS2Hdp"))
            Else
                autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoS8Hdp"), Request("autoS8Hdp")), 0)
                autoOU = IIf(isAutoOU, IIf(isCopyHdp, Request("autoS8Hdp"), Request("autoS8OU")), 0)
                autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoS8Hdp"), Request("autoS8ML")), 0)

                sHdp = IIf(isAutoHdp, 0, IIf(isCopyHdp, Request("listS8Hdp"), Request("listS8Hdp")))
                sOU = IIf(isAutoOU, 0, IIf(isCopyHdp, Request("listS8Hdp"), Request("listS8OU")))
                sML = IIf(isAutoML, 0, IIf(isCopyHdp, Request("listS8Hdp"), Request("listS8ML")))
            End If
        Else
            Dim isAutoSoccer As Boolean = False
            If Not Request("chkAuto1HdpD") Is Nothing Then
                isAutoSoccer = True
            End If
            autoHdp = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoOU = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoML = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)

            sHdp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sOU = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sML = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
        End If

        param.Add("v8spositiontaking_1", sHdp) 'smallmoney  
        param.Add("v8spositiontaking_3", sOU) 'smallmoney 
        param.Add("v8spositiontaking_20", sML) 'smallmoney  

        param.Add("v8mpositiontaking_1", mHdp) 'smallmoney  
        param.Add("v8mpositiontaking_3", mOU) 'smallmoney 
        param.Add("v8mpositiontaking_20", mML) 'smallmoney 

        param.Add("auto_v8spositiontaking_1", autoHdp) 'smallmoney 
        param.Add("auto_v8spositiontaking_3", autoOU) 'smallmoney  
        param.Add("auto_v8spositiontaking_20", autoML) 'smallmoney 

        '***** End Inserting for Baseball *****
    End Sub
#End Region

#Region "Insert parameters for Motosports Position Taking"
    Sub AddParaMotosports()
        Dim minb, maxb, maxpermatch As Double
        minb = IIf(isCopyAll, Request("txtMinbet1"), IIf(isCopyBasket, Request("txtMinbet2"), Request("txtMinbet11")))
        maxb = IIf(isCopyAll, Request("txtMaxbet1"), IIf(isCopyBasket, Request("txtMaxbet2"), Request("txtMaxbet11")))
        maxpermatch = IIf(isCopyAll, Request("txtMaxPerMatch1"), IIf(isCopyBasket, Request("txtMaxPerMatch2"), Request("txtMaxPerMatch11")))
        param.Add("minbetv11", minb)  'money  
        param.Add("maxbetv11", maxb) 'money  
        param.Add("maxpermatchv11", maxpermatch) 'money  
        '***** Start Inserting for MotoSports *****
        Dim mHdp, mML As Double
        Dim sHdp, sML As Double
        Dim autoHdp, autoML As Double

        'Dim isAuto As Boolean = False
        'If Not Request("chkAuto11") Is Nothing Then
        '    isAuto = True
        'End If
        '----
        Dim isAutoHdp, isAutoML As Boolean

        isAutoHdp = IIf(Request("chkAuto11Hdp") Is Nothing, 0, 1)
        isAutoML = IIf(Request("chkAuto11ML") Is Nothing, 0, 1)
        '----
        Dim isCopyHdp As Boolean = False
        If Not Request("chkCopy11Hdp") Is Nothing Then
            isCopyHdp = True
            isAutoML = isAutoHdp
        End If
        'this case for Copy All from Soccer or Basketball checkbox is checked
        If isCopyAll Or isCopyBasket Then
            mHdp = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
            mML = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
        Else
            mHdp = IIf(isCopyHdp, Request("listM11Hdp"), Request("listM11Hdp"))
            mML = IIf(isCopyHdp, Request("listM11Hdp"), Request("listM11ML"))
        End If

        If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
            If isCopyBasket Then
                Dim isAutoBasket As Boolean = False
                If Not Request("chkAuto2Hdp") Is Nothing Then
                    isAutoBasket = True
                End If
                autoHdp = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoML = IIf(isAutoBasket, Request("autoS2Hdp"), 0)

                sHdp = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                sML = IIf(isAutoBasket, 0, Request("listS2Hdp"))
            Else
                autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoS11Hdp"), Request("autoS11Hdp")), 0)
                autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoS11Hdp"), Request("autoS11ML")), 0)

                sHdp = IIf(isAutoHdp, 0, IIf(isCopyHdp, Request("listS11Hdp"), Request("listS11Hdp")))
                sML = IIf(isAutoML, 0, IIf(isCopyHdp, Request("listS11Hdp"), Request("listS11ML")))
            End If
        Else
            Dim isAutoSoccer As Boolean = False
            If Not Request("chkAuto1HdpD") Is Nothing Then
                isAutoSoccer = True
            End If
            autoHdp = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoML = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)

            sHdp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sML = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
        End If

        param.Add("v11spositiontaking_1", sHdp) 'smallmoney         
        param.Add("v11spositiontaking_20", sML) 'smallmoney  

        param.Add("v11mpositiontaking_1", mHdp) 'smallmoney         
        param.Add("v11mpositiontaking_20", mML) 'smallmoney 

        param.Add("auto_v11spositiontaking_1", autoHdp) 'smallmoney          
        param.Add("auto_v11spositiontaking_20", autoML) 'smallmoney         
        '***** End Inserting for MotoSports *****
    End Sub
#End Region

#Region "Insert parameters for Number Game Position Taking"
    Sub AddParaNumberGame()

        Dim isDisableNumberGame As Boolean = False
        If (Not UserPermission.IsShowNumberGame) Then
            isDisableNumberGame = True
        End If

        'Add Bet Setting parameters
        Dim minb, maxb, maxpermatch, maxperball As Double
        If isDisableNumberGame Then
            minb = Request("txtMinbet1")
            maxb = Request("txtMaxbet1")
            maxpermatch = Request("txtMaxPerMatch1")
            maxperball = maxpermatch
        Else
            minb = Request("txtMinbet161")
            maxb = Request("txtMaxbet161")
            maxpermatch = Request("txtMaxPerMatch161")
            maxperball = Request("txtMaxPerBall")
        End If

        param.Add("minbetv161", minb) 'money  
        param.Add("maxbetv161", maxb) 'money  
        param.Add("maxpermatchv161", maxpermatch) 'money 
        param.Add("maxperballv161", maxperball) 'money 
        '**************************
        '***** Add Position Taking paramaters*****
        Dim m1stOUD, m1stOED, mOED, mWRD As Double
        Dim s1stOUD, s1stOED, sOED, sWRD As Double
        Dim auto1stOUD, auto1stOED, autoOED, autoWRD As Double

        Dim mOUL, mOEL, mHLL, mNCL, mNGL As Double
        Dim sOUL, sOEL, sHLL, sNCL, sNGL As Double
        Dim autoOUL, autoOEL, autoHLL, autoNCL, autoNGL As Double

        'Dim isAuto As Boolean = False
        '----
        Dim isAuto1stOUD, isAuto1stOED, isAutoOED, isAutoWRD As Boolean
        Dim isAutoOUL, isAutoOEL, isAutoHLL, isAutoNCL, isAutoNGL As Boolean

        isAuto1stOUD = IIf(Request("chkAuto1611stOUD") Is Nothing, 0, 1)
        isAuto1stOED = IIf(Request("chkAuto1611stOED") Is Nothing, 0, 1)
        'isAutoOUD = IIf(Request("chkAuto161OUD") Is Nothing, 0, 1)
        isAutoOED = IIf(Request("chkAuto161OED") Is Nothing, 0, 1)
        isAutoWRD = IIf(Request("chkAuto161WRD") Is Nothing, 0, 1)

        isAutoOUL = IIf(Request("chkAuto161OUL") Is Nothing, 0, 1)
        isAutoOEL = IIf(Request("chkAuto161OEL") Is Nothing, 0, 1)
        isAutoHLL = IIf(Request("chkAuto161HLL") Is Nothing, 0, 1)
        isAutoNCL = IIf(Request("chkAuto161NCL") Is Nothing, 0, 1)
        isAutoNGL = IIf(Request("chkAuto161NGL") Is Nothing, 0, 1)
        '----
        Dim isCopy1stOUD As Boolean = False
        If Not Request("chkCopy1611stOUD") Is Nothing Then
            isCopy1stOUD = True
            isAuto1stOED = isAuto1stOUD
            'isAutoOUD = isAuto1stOUD
            isAutoOED = isAuto1stOUD
            isAutoWRD = isAuto1stOUD
        End If
        Dim isCopyOUL As Boolean = False
        If Not Request("chkCopy161OUL") Is Nothing Then
            isCopyOUL = True
            isAutoOEL = isAutoOUL
            isAutoHLL = isAutoOUL
            isAutoNCL = isAutoOUL
            isAutoNGL = isAutoOUL
        End If

        If (isDisableNumberGame) Then
            m1stOUD = Request("listM1HdpD")
            m1stOED = Request("listM1HdpD")
            'mOUD = Request("listM1HdpD")
            mOED = Request("listM1HdpD")
            mWRD = Request("listM1HdpD")

            mOUL = Request("listM1HdpD")
            mOEL = Request("listM1HdpD")
            mHLL = Request("listM1HdpD")
            mNCL = Request("listM1HdpD")
            mNGL = Request("listM1HdpD")
        Else
            m1stOUD = IIf(isCopy1stOUD, Request("listM1611stOUD"), Request("listM1611stOUD"))
            m1stOED = IIf(isCopy1stOUD, Request("listM1611stOUD"), Request("listM1611stOED"))
            'mOUD = IIf(isCopy1stOUD, Request("listM1611stOUD"), Request("listM161OUD"))
            mOED = IIf(isCopy1stOUD, Request("listM1611stOUD"), Request("listM161OED"))
            mWRD = IIf(isCopy1stOUD, Request("listM1611stOUD"), Request("listM161WRD"))

            mOUL = IIf(isCopyOUL, Request("listM161OUL"), Request("listM161OUL"))
            mOEL = IIf(isCopyOUL, Request("listM161OUL"), Request("listM161OEL"))
            mHLL = IIf(isCopyOUL, Request("listM161OUL"), Request("listM161HLL"))
            mNCL = IIf(isCopyOUL, Request("listM161OUL"), Request("listM161NCL"))
            mNGL = IIf(isCopyOUL, IIf(UserPermission.IsShowNumberGroup, Request("listM161OUL"), Request("listM161NGL")), Request("listM161NGL"))

        End If

        If Not isDisableNumberGame Then 'this case for Copy All PT checkbox is checked
            auto1stOUD = IIf(isAuto1stOUD, IIf(isCopy1stOUD, Request("autoS1611stOUD"), Request("autoS1611stOUD")), 0)
            auto1stOED = IIf(isAuto1stOED, IIf(isCopy1stOUD, Request("autoS1611stOUD"), Request("autoS1611stOED")), 0)
            'autoOUD = IIf(isAutoOUD, IIf(isCopy1stOUD, Request("autoS1611stOUD"), Request("autoS161OUD")), 0)
            autoOED = IIf(isAutoOED, IIf(isCopy1stOUD, Request("autoS1611stOUD"), Request("autoS161OED")), 0)
            autoWRD = IIf(isAutoWRD, IIf(isCopy1stOUD, Request("autoS1611stOUD"), Request("autoS161WRD")), 0)

            autoOUL = IIf(isAutoOUL, IIf(isCopyOUL, Request("autoS161OUL"), Request("autoS161OUL")), 0)
            autoOEL = IIf(isAutoOEL, IIf(isCopyOUL, Request("autoS161OUL"), Request("autoS161OEL")), 0)
            autoHLL = IIf(isAutoHLL, IIf(isCopyOUL, Request("autoS161OUL"), Request("autoS161HLL")), 0)
            autoNCL = IIf(isAutoNCL, IIf(isCopyOUL, Request("autoS161OUL"), Request("autoS161NCL")), 0)
            autoNGL = IIf(isAutoNGL, IIf(isCopyOUL, Request("autoS161NGL"), Request("autoS161NGL")), 0)

            s1stOUD = IIf(isAuto1stOUD, 0, IIf(isCopy1stOUD, Request("listS1611stOUD"), Request("listS1611stOUD")))
            s1stOED = IIf(isAuto1stOED, 0, IIf(isCopy1stOUD, Request("listS1611stOUD"), Request("listS1611stOED")))
            'sOUD = IIf(isAutoOUD, 0, IIf(isCopy1stOUD, Request("listS1611stOUD"), Request("listS161OUD")))
            sOED = IIf(isAutoOED, 0, IIf(isCopy1stOUD, Request("listS1611stOUD"), Request("listS161OED")))
            sWRD = IIf(isAutoWRD, 0, IIf(isCopy1stOUD, Request("listS1611stOUD"), Request("listS161WRD")))

            sOUL = IIf(isAutoOUL, 0, IIf(isCopyOUL, Request("listS161OUL"), Request("listS161OUL")))
            sOEL = IIf(isAutoOEL, 0, IIf(isCopyOUL, Request("listS161OUL"), Request("listS161OEL")))
            sHLL = IIf(isAutoHLL, 0, IIf(isCopyOUL, Request("listS161OUL"), Request("listS161HLL")))
            sNCL = IIf(isAutoNCL, 0, IIf(isCopyOUL, Request("listS161OUL"), Request("listS161NCL")))
            sNGL = IIf(isAutoNGL, 0, IIf(isCopyOUL, IIf(UserPermission.IsShowNumberGroup, Request("listS161OUL"), Request("listS161NGL")), Request("listS161NGL")))
        Else

            auto1stOUD = Request("autoS1HdpD")
            auto1stOED = Request("autoS1HdpD")
            'autoOUD = Request("autoS1HdpD")
            autoOED = Request("autoS1HdpD")
            autoWRD = Request("autoS1HdpD")

            autoOUL = Request("autoS1HdpD")
            autoOEL = Request("autoS1HdpD")
            autoHLL = Request("autoS1HdpD")
            autoNCL = Request("autoS1HdpD")
            autoNGL = Request("autoS1HdpD")

            s1stOUD = Request("listS1HdpD")
            s1stOED = Request("listS1HdpD")
            'sOUD = Request("listS1HdpD")
            sOED = Request("listS1HdpD")
            sWRD = Request("listS1HdpD")

            sOUL = Request("listS1HdpD")
            sOEL = Request("listS1HdpD")
            sHLL = Request("listS1HdpD")
            sNCL = Request("listS1HdpD")
            sNGL = Request("listS1HdpD")
        End If

        param.Add("v161spositiontaking_81", s1stOUD)
        param.Add("v161spositiontaking_83", s1stOED)
        'param.Add("v161spositiontaking_85", sOUD)
        param.Add("v161spositiontaking_86", sOED)
        param.Add("v161spositiontaking_88", sWRD)

        param.Add("v161slivepositiontaking_85", sOUL)
        param.Add("v161slivepositiontaking_86", sOEL)
        param.Add("v161slivepositiontaking_87", sHLL)
        param.Add("v161slivepositiontaking_89", sNCL)
        param.Add("v161slivepositiontaking_90", sNGL)

        param.Add("v161mpositiontaking_81", m1stOUD)
        param.Add("v161mpositiontaking_83", m1stOED)
        'param.Add("v161mpositiontaking_85", mOUD)
        param.Add("v161mpositiontaking_86", mOED)
        param.Add("v161mpositiontaking_88", mWRD)


        param.Add("v161mlivepositiontaking_85", mOUL)
        param.Add("v161mlivepositiontaking_86", mOEL)
        param.Add("v161mlivepositiontaking_87", mHLL)
        param.Add("v161mlivepositiontaking_89", mNCL)
        param.Add("v161mlivepositiontaking_90", mNGL)

        param.Add("auto_v161spositiontaking_81", auto1stOUD)
        param.Add("auto_v161spositiontaking_83", auto1stOED)
        'param.Add("auto_v161spositiontaking_85", autoOUD)
        param.Add("auto_v161spositiontaking_86", autoOED)
        param.Add("auto_v161spositiontaking_88", autoWRD)


        param.Add("auto_v161slivepositiontaking_85", autoOUL)
        param.Add("auto_v161slivepositiontaking_86", autoOEL)
        param.Add("auto_v161slivepositiontaking_87", autoHLL)
        param.Add("auto_v161slivepositiontaking_89", autoNCL)
        param.Add("auto_v161slivepositiontaking_90", autoNGL)

        '***** End Inserting for Soccer *****
    End Sub
#End Region

#Region "Insert param for Mixed Sports Position Taking"
    Sub AddParaMixedSports()
        Dim minb, maxb, maxpermatch As Double
        minb = IIf(isCopyAll, Request("txtMinbet1"), IIf(isCopyBasket, Request("txtMinbet2"), Request("txtMinbet99_M")))
        maxb = IIf(isCopyAll, Request("txtMaxbet1"), IIf(isCopyBasket, Request("txtMaxbet2"), Request("txtMaxbet99_M")))
        maxpermatch = IIf(isCopyAll, Request("txtMaxPerMatch1"), IIf(isCopyBasket, Request("txtMaxPerMatch2"), Request("txtMaxPerMatch99_M")))
        param.Add("minbetv99mp", minb) 'money  
        param.Add("maxbetv99mp", maxb) 'money  
        param.Add("maxpermatchv99mp", maxpermatch) 'money  
        '***** Start Inserting for Football *****
        Dim mMP As Double
        Dim sMP As Double
        Dim autoMP As Double

        Dim isAutoMP As Boolean


        isAutoMP = IIf(Request("chkAuto99_M") Is Nothing, 0, 1)
        '----
        If isCopyAll Or isCopyBasket Then
            mMP = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
        Else
            mMP = Request("listM99_M")
        End If

        If Not isCopyAll Then 'this case for Copy All PT from Soccer checkbox is unchecked
            If isCopyBasket Then 'this case for Copy All PT from Basketball checkbox is checked
                Dim isAutoBasket As Boolean = False
                If Not Request("chkAuto2Hdp") Is Nothing Then
                    isAutoBasket = True
                End If
                autoMP = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                sMP = IIf(isAutoBasket, 0, Request("listS2Hdp"))
            Else
                autoMP = IIf(isAutoMP, Request("autoS99_M"), 0)
                sMP = IIf(isAutoMP, 0, Request("listS99_M"))
            End If
        Else
            Dim isAutoSoccer As Boolean = False
            If Not Request("chkAuto1HdpD") Is Nothing Then
                isAutoSoccer = True
            End If
            autoMP = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            sMP = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
        End If

        param.Add("v99spositiontaking_9", sMP) 'smallmoney    
        param.Add("v99mpositiontaking_9", mMP) 'smallmoney     
        param.Add("auto_v99spositiontaking_9", autoMP) 'smallmoney     

        '***** End Inserting for Football *****
    End Sub
#End Region

#Region "Check valid CustId"
    Public Function check(ByVal page As Page, Optional ByVal type As String = Nothing) As Boolean
        Dim value As Boolean = False

        If Sunplus.Agent.Model.Entities.UserSession.Current.CustId = 0 Then
            value = True
        End If

        If value = True Then
            FormsAuthentication.SignOut()
            page.Session.Abandon()
            If type Is Nothing Then
                page.Response.Write("<script language=""JavaScript"">window.top.location.href='Index.aspx'</script>")
            ElseIf type = "popup" Then
                page.Response.Write("<script language=""JavaScript"">self.close(); window.opener.top.location.href='../Index.aspx'</script>")
            End If
        End If

        Return value
    End Function
#End Region

#Region "ReplaceData"
    Public Function ReplaceData(ByVal str As String) As String
        Return str.Replace("""", "&quot;").Replace("'", "&#39;").Replace(vbNewLine, "")
    End Function
#End Region

End Class

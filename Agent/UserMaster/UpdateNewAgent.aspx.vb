Imports System.Collections
Imports DNAide.Utils
Imports DNAide.Data
Imports DNAide.Web
Imports DNAide.Core
Imports System.Data
Imports HandHistoryLibrary.DataHandler
Imports System.Web.Security
Imports Sunplus.Agent.Model.Entities.Permission
Imports Fanex.Data.DNAide

Public Class UpdateNewAgent
    Inherits IbcPage2
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

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

        Dim rs As DbResult
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
        param.Add("answer", "") 'nvarchar  

        If Not Request("chkSuspended") Is Nothing Then
            param.Add("suspended", 1) 'bit  
        Else
            param.Add("suspended", CastAide.AsInt32(Request("IsSuspended"), 0)) 'bit  
        End If

        If Not Request("chkCopyAll") Is Nothing Then
            isCopyAll = True
        End If
        If Not Request("chkCopyBasket") Is Nothing Then
            isCopyBasket = True
        End If
        param.Add("credit", Request("txtCredit")) 'money  
        param.Add("maxcredit", Request("txtMaxCredit")) 'money  
        param.Add("mrecommend", Sunplus.Agent.Model.Entities.UserSession.Current.CustId) 'int 

        param.Add("groupa", Request("groupa")) 'decimal  
        param.Add("groupb", Request("groupb")) 'decimal  
        param.Add("groupc", Request("groupc")) 'decimal  
        param.Add("groupd", Request("groupd")) 'decimal  
        param.Add("discount1x2", Request("discount1x2")) 'decimal
        param.Add("discountcs", Request("discountcs")) 'decimal                 
        param.Add("discountnumber", IIf(UserPermission.IsShowNumberGame, Request("disnumbergame"), 0)) 'decimal                 

        param.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey)
        param.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId) 'int     
        param.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP())
        param.Add("errcode", 0) 'int  
        param.Add("errmsg", "") 'nvarchar  
        'nqminh - 29Nov08 - changed
        param.Add("sitename", AppSettings.SiteLevel) 'varchar  

        Dim strDirectMaster As String = Request("dirMaster")
        If strDirectMaster = "1" Then
            Dim strUpdTranfer As String = ""
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

        AddParaSoccer()
        AddParaFootball(2)
        AddParaFootball(3)
        AddParaFootball(5)
        AddParaBaseball()
        AddParaFootball(10)
        AddParaMotosports()
        AddParaFootball(99)
        AddParaNumberGame(161)
        AddParaMixSports()

        rs = AccountInfo.Acc_CustomerInsMember_MAg(param)

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
        'Tho modify: Auto Master
        Dim isAutoHdpD, isAutoOUD, isAuto1stHdpD, isAuto1stOUD, isAutoOE, isAuto1x2, isAutoCS, isAutoTG, isAutoMP, isAutoOR As Boolean
        Dim isAutoHdpL, isAutoOUL, isAuto1stHdpL, isAuto1stOUL As Boolean

        Dim autoHdpD, autoOUD, auto1stHdpD, auto1stOUD, autoOE, auto1x2, autoCS, autoTG, autoMp, autoOR As Double
        Dim autoHdpL, autoOUL, auto1stHdpL, auto1stOUL As Double

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


        '***** Add Position Taking paramaters*****
        Dim mHdpD, mOUD, m1stHdpD, m1stOUD, mOE, m1x2, mCS, mTG, mMp, mOR As Double
        Dim aHdpD, aOUD, a1stHdpD, a1stOUD, aOE, a1x2, aCS, aTG, aMp, aOR As Double

        Dim mHdpL, mOUL, m1stHdpL, m1stOUL As Double
        Dim aHdpL, aOUL, a1stHdpL, a1stOUL As Double

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

        If Not isCopyAll Then 'this case for Copy All PT checkbox is checked  
            'Tho modify: Auto Master
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
            '/----

            aHdpD = IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM1HdpD"))
            aOUD = IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM1OUD"))
            a1stHdpD = IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM11stHdpD"))
            a1stOUD = IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM11stOUD"))
            aOE = IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM1OE"))

            a1x2 = IIf(isCopy1x2, Request("listM11x2"), Request("listM11x2"))
            aCS = IIf(isCopy1x2, Request("listM11x2"), Request("listM1CS"))
            aTG = IIf(isCopy1x2, Request("listM11x2"), Request("listM1TG"))
            aMp = IIf(isCopy1x2, Request("listM11x2"), Request("listM1MP"))
            aOR = IIf(isCopy1x2, Request("listM11x2"), Request("listM1OR"))

            aHdpL = IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM1HdpL"))
            aOUL = IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM1OUL"))
            a1stHdpL = IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM11stHdpL"))
            a1stOUL = IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM11stOUL"))

            mHdpD = IIf(isAutoHdpD, 0, IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS1HdpD")))
            mOUD = IIf(isAutoOUD, 0, IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS1OUD")))
            m1stHdpD = IIf(isAuto1stHdpD, 0, IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS11stHdpD")))
            m1stOUD = IIf(isAuto1stOUD, 0, IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS11stOUD")))
            mOE = IIf(isAutoOE, 0, IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS1OE")))

            m1x2 = IIf(isAuto1x2, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS11x2")))
            mCS = IIf(isAutoCS, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS1CS")))
            mTG = IIf(isAutoTG, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS1TG")))
            mMp = IIf(isAutoMP, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS1MP")))
            mOR = IIf(isAutoOR, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS1OR")))

            mHdpL = IIf(isAutoHdpL, 0, IIf(isCopyHdpL, Request("listS1HdpL"), Request("listS1HdpL")))
            mOUL = IIf(isAutoOUL, 0, IIf(isCopyHdpL, Request("listS1HdpL"), Request("listS1OUL")))
            m1stHdpL = IIf(isAuto1stHdpL, 0, IIf(isCopyHdpL, Request("listS1HdpL"), Request("listS11stHdpL")))
            m1stOUL = IIf(isAuto1stOUL, 0, IIf(isCopyHdpL, Request("listS1HdpL"), Request("listS11stOUL")))
        Else
            'Tho modify: Auto Master
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
            '/-----

            aHdpD = Request("listM1HdpD")
            aOUD = Request("listM1HdpD")
            a1stHdpD = Request("listM1HdpD")
            a1stOUD = Request("listM1HdpD")
            aOE = Request("listM1HdpD")

            a1x2 = Request("listM1HdpD")
            aCS = Request("listM1HdpD")
            aTG = Request("listM1HdpD")
            aMp = Request("listM1HdpD")
            aOR = Request("listM1HdpD")

            aHdpL = Request("listM1HdpD")
            aOUL = Request("listM1HdpD")
            a1stHdpL = Request("listM1HdpD")
            a1stOUL = Request("listM1HdpD")

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

        'Tho modify: Auto Master
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
        '/----
        '***** End Inserting for Soccer *****
    End Sub
#End Region

#Region "Insert parameters for Basketball, Football, Tennis, Golf, Other Sports Position Taking"
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
        Dim aHdp, aOU, aOE, aML As Double

        'Tho modify: Auto Master
        Dim autoHdp, autoOU, autoOE, autoML As Double
        Dim isAutoHdp, isAutoOU, isAutoOE, isAutoML As Boolean
        isAutoHdp = IIf(Request("chkAuto" & sportId & "Hdp") Is Nothing, 0, 1)
        isAutoOU = IIf(Request("chkAuto" & sportId & "OU") Is Nothing, 0, 1)
        isAutoOE = IIf(Request("chkAuto" & sportId & "OE") Is Nothing, 0, 1)
        isAutoML = IIf(Request("chkAuto" & sportId & "ML") Is Nothing, 0, 1)
        '/----

        Dim isCopyHdp As Boolean = False
        If Not Request("chkCopy" & sportId & "Hdp") Is Nothing Then
            isCopyHdp = True
            isAutoOU = isAutoHdp
            isAutoOE = isAutoHdp

            isAutoML = isAutoHdp
        End If
        If Not isCopyAll Then 'this case for Copy All PT from Soccer checkbox is unchecked
            If isCopyBasket Then 'this case for Copy All PT from Basketball checkbox is checked    
                'Tho modify: Auto Master
                Dim isAutoBasket As Boolean = False
                If Not Request("chkAuto2Hdp") Is Nothing Then
                    isAutoBasket = True
                End If
                autoHdp = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoOU = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoOE = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoML = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                '/-----
                aHdp = Request("listM2Hdp")
                aOU = Request("listM2Hdp")
                aOE = Request("listM2Hdp")
                aML = Request("listM2Hdp")

                mHdp = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                mOU = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                mOE = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                mML = IIf(isAutoBasket, 0, Request("listS2Hdp"))
            Else
                'Tho modify: Auto Master
                autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "Hdp")), 0)
                autoOU = IIf(isAutoOU, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "OU")), 0)
                autoOE = IIf(isAutoOE, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "OE")), 0)
                autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "ML")), 0)
                '/----
                aHdp = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "Hdp"))
                aOU = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "OU"))
                aOE = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "OE"))
                aML = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "ML"))

                mHdp = IIf(isAutoHdp, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "Hdp")))
                mOU = IIf(isAutoOU, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "OU")))
                mOE = IIf(isAutoOE, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "OE")))
                mML = IIf(isAutoML, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "ML")))

            End If
        Else
            'Tho modify: Auto Master
            Dim isAutoSoccer As Boolean = False
            If Not Request("chkAuto1HdpD") Is Nothing Then
                isAutoSoccer = True
            End If
            autoHdp = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoOU = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoOE = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoML = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            '/----
            aHdp = Request("listM1HdpD")
            aOU = Request("listM1HdpD")
            aOE = Request("listM1HdpD")
            aML = Request("listM1HdpD")

            mHdp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mOU = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mOE = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mML = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
        End If
        param.Add("v" & sportId & "apositiontaking_1", aHdp) 'smallmoney  
        param.Add("v" & sportId & "apositiontaking_3", aOU) 'smallmoney  
        param.Add("v" & sportId & "apositiontaking_2", aOE) 'smallmoney  
        param.Add("v" & sportId & "apositiontaking_20", aML) 'smallmoney  

        param.Add("v" & sportId & "mpositiontaking_1", mHdp) 'smallmoney  
        param.Add("v" & sportId & "mpositiontaking_3", mOU) 'smallmoney  
        param.Add("v" & sportId & "mpositiontaking_2", mOE) 'smallmoney
        param.Add("v" & sportId & "mpositiontaking_20", mML) 'smallmoney      

        'Tho modify: Auto Master
        param.Add("auto_v" & sportId & "mpositiontaking_1", autoHdp) 'smallmoney  
        param.Add("auto_v" & sportId & "mpositiontaking_3", autoOU) 'smallmoney  
        param.Add("auto_v" & sportId & "mpositiontaking_2", autoOE) 'smallmoney  
        param.Add("auto_v" & sportId & "mpositiontaking_20", autoML) 'smallmoney 
        '/---
        If sportId = "2" Then
            param.Add("v2mpositiontaking_7", mHdp) 'smallmoney 
            param.Add("v2mpositiontaking_8", mOU) 'smallmoney  

            param.Add("v2apositiontaking_7", aHdp) 'smallmoney 
            param.Add("v2apositiontaking_8", aOU) 'smallmoney  


            'Tho modify: Auto Master
            param.Add("auto_v2mpositiontaking_7", autoHdp) 'smallmoney  
            param.Add("auto_v2mpositiontaking_8", autoOU) 'smallmoney 

            '/----

        End If
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
        Dim aHdp, aOU, aML As Double

        'Tho modify: Auto Master
        Dim isAutoHdp, isAutoOU, isAutoML As Boolean
        Dim autoHdp, autoOU, autoML As Double

        isAutoHdp = IIf(Request("chkAuto8Hdp") Is Nothing, 0, 1)
        isAutoOU = IIf(Request("chkAuto8OU") Is Nothing, 0, 1)
        isAutoML = IIf(Request("chkAuto8ML") Is Nothing, 0, 1)
        '---
        Dim isCopyHdp As Boolean = False
        If Not Request("chkCopy8Hdp") Is Nothing Then
            isCopyHdp = True
            isAutoOU = isAutoHdp
            isAutoML = isAutoHdp
        End If
        If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
            If isCopyBasket Then
                'Tho modify: Auto Master
                Dim isAutoBasket As Boolean = False
                If Not Request("chkAuto2Hdp") Is Nothing Then
                    isAutoBasket = True
                End If
                autoHdp = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoOU = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoML = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                '----
                aHdp = Request("listM2Hdp")
                aOU = Request("listM2Hdp")
                aML = Request("listM2Hdp")

                mHdp = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                mOU = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                mML = IIf(isAutoBasket, 0, Request("listS2Hdp"))
            Else
                'Tho modify: Auto Master
                autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoS8Hdp"), Request("autoS8Hdp")), 0)
                autoOU = IIf(isAutoOU, IIf(isCopyHdp, Request("autoS8Hdp"), Request("autoS8OU")), 0)
                autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoS8Hdp"), Request("autoS8ML")), 0)
                '----
                aHdp = IIf(isCopyHdp, Request("listM8Hdp"), Request("listM8Hdp"))
                aOU = IIf(isCopyHdp, Request("listM8Hdp"), Request("listM8OU"))
                aML = IIf(isCopyHdp, Request("listM8Hdp"), Request("listM8ML"))

                mHdp = IIf(isAutoHdp, 0, IIf(isCopyHdp, Request("listS8Hdp"), Request("listS8Hdp")))
                mOU = IIf(isAutoOU, 0, IIf(isCopyHdp, Request("listS8Hdp"), Request("listS8OU")))
                mML = IIf(isAutoML, 0, IIf(isCopyHdp, Request("listS8Hdp"), Request("listS8ML")))
            End If
        Else
            Dim isAutoSoccer As Boolean = False
            If Not Request("chkAuto1HdpD") Is Nothing Then
                isAutoSoccer = True
            End If
            autoHdp = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoOU = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoML = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)

            aHdp = Request("listM1HdpD")
            aOU = Request("listM1HdpD")
            aML = Request("listM1HdpD")

            mHdp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mOU = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mML = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
        End If
        param.Add("v8apositiontaking_1", aHdp) 'smallmoney  
        param.Add("v8apositiontaking_3", aOU) 'smallmoney 
        param.Add("v8apositiontaking_20", aML) 'smallmoney  

        param.Add("v8mpositiontaking_1", mHdp) 'smallmoney  
        param.Add("v8mpositiontaking_3", mOU) 'smallmoney 
        param.Add("v8mpositiontaking_20", mML) 'smallmoney   

        param.Add("auto_v8mpositiontaking_1", autoHdp) 'smallmoney 
        param.Add("auto_v8mpositiontaking_3", autoOU) 'smallmoney  
        param.Add("auto_v8mpositiontaking_20", autoML) 'smallmoney

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

        'Tho modify: Auto Master
        Dim isAutoHdp, isAutoML As Boolean
        Dim autoHdp, autoML As Double

        isAutoHdp = IIf(Request("chkAuto11Hdp") Is Nothing, 0, 1)
        isAutoML = IIf(Request("chkAuto11ML") Is Nothing, 0, 1)
        '/----

        Dim mHdp, mML As Double
        Dim aHdp, aML As Double

        Dim isCopyHdp As Boolean = False
        If Not Request("chkCopy11Hdp") Is Nothing Then
            isCopyHdp = True
            isAutoML = isAutoHdp
        End If
        If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
            If isCopyBasket Then
                Dim isAutoBasket As Boolean = False
                If Not Request("chkAuto2Hdp") Is Nothing Then
                    isAutoBasket = True
                End If
                autoHdp = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoML = IIf(isAutoBasket, Request("autoS2Hdp"), 0)

                aHdp = Request("listM2Hdp")
                aML = Request("listM2Hdp")

                mHdp = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                mML = IIf(isAutoBasket, 0, Request("listS2Hdp"))
            Else
                autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoS11Hdp"), Request("autoS11Hdp")), 0)
                autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoS11Hdp"), Request("autoS11ML")), 0)

                aHdp = IIf(isCopyHdp, Request("listM11Hdp"), Request("listM11Hdp"))
                aML = IIf(isCopyHdp, Request("listM11Hdp"), Request("listM11ML"))

                mHdp = IIf(isAutoHdp, 0, IIf(isCopyHdp, Request("listS11Hdp"), Request("listS11Hdp")))
                mML = IIf(isAutoML, 0, IIf(isCopyHdp, Request("listS11Hdp"), Request("listS11ML")))
            End If
        Else
            Dim isAutoSoccer As Boolean = False
            If Not Request("chkAuto1HdpD") Is Nothing Then
                isAutoSoccer = True
            End If
            autoHdp = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoML = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)

            aHdp = Request("listM1HdpD")
            aML = Request("listM1HdpD")

            mHdp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mML = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
        End If
        param.Add("v11apositiontaking_1", aHdp) 'smallmoney         
        param.Add("v11apositiontaking_20", aML) 'smallmoney  

        param.Add("v11mpositiontaking_1", mHdp) 'smallmoney         
        param.Add("v11mpositiontaking_20", mML) 'smallmoney 

        param.Add("auto_v11mpositiontaking_1", autoHdp) 'smallmoney          
        param.Add("auto_v11mpositiontaking_20", autoML) 'smallmoney     
        '***** End Inserting for MotoSports *****
    End Sub
#End Region

#Region "Insert parameters for Number Game Position Taking"
    Sub AddParaNumberGame(ByVal sportId As String)

        Dim isDisableNumberGame As Boolean = False
        If (Not UserPermission.IsShowNumberGame) Then
            isDisableNumberGame = True
        End If

        Dim minb, maxb, maxpermatch, maxperball As Double
        If isDisableNumberGame Then
            minb = Request("txtMinbet1")
            maxb = Request("txtMaxbet1")
            maxpermatch = Request("txtMaxPerMatch1")
            maxperball = maxpermatch
        Else
            minb = Request("txtMinbet" + sportId)
            maxb = Request("txtMaxbet" + sportId)
            maxpermatch = Request("txtMaxPerMatch" + sportId)
            maxperball = Request("txtMaxPerBall")
        End If

        param.Add("minbetv" + sportId, minb) 'money  
        param.Add("maxbetv" + sportId, maxb) 'money  
        param.Add("maxpermatchv" + sportId, maxpermatch) 'money  
        param.Add("maxperballv" + sportId, maxperball) 'money  

        Dim m1stOU, m1stOE, mOE, mWR As Double
        Dim a1stOU, a1stOE, aOE, aWR As Double

        Dim mOUL, mOEL, mHLL, mNCL, mNGL As Double
        Dim aOUL, aOEL, aHLL, aNCL, aNGL As Double

        Dim auto1stOU, auto1stOE, autoOE, autoWR As Double
        Dim isAuto1stOU, isAuto1stOE, isAutoOE, isAutoWR As Boolean

        Dim autoOUL, autoOEL, autoHLL, autoNCL, autoNGL As Double
        Dim isAutoOUL, isAutoOEL, isAutoHLL, isAutoNCL, isAutoNGL As Boolean


        isAuto1stOU = IIf(Request("chkAuto" & sportId & "1stOUD") Is Nothing, 0, 1)
        isAuto1stOE = IIf(Request("chkAuto" & sportId & "1stOED") Is Nothing, 0, 1)
        isAutoOE = IIf(Request("chkAuto" & sportId & "OED") Is Nothing, 0, 1)
        isAutoWR = IIf(Request("chkAuto" & sportId & "WRD") Is Nothing, 0, 1)

        isAutoOUL = IIf(Request("chkAuto" & sportId & "OUL") Is Nothing, 0, 1)
        isAutoOEL = IIf(Request("chkAuto" & sportId & "OEL") Is Nothing, 0, 1)
        isAutoHLL = IIf(Request("chkAuto" & sportId & "HLL") Is Nothing, 0, 1)
        isAutoNCL = IIf(Request("chkAuto" & sportId & "NCL") Is Nothing, 0, 1)
        isAutoNGL = IIf(Request("chkAuto" & sportId & "NGL") Is Nothing, 0, 1)
        '/----

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


        If Not isDisableNumberGame Then 'this case for Copy All PT from Soccer checkbox is unchecked
            'Tho modify: Auto Master
            auto1stOU = IIf(isAuto1stOU, IIf(isCopy1stOU, Request("autoS" & sportId & "1stOUD"), Request("autoS" & sportId & "1stOUD")), 0)
            auto1stOE = IIf(isAuto1stOE, IIf(isCopy1stOU, Request("autoS" & sportId & "1stOUD"), Request("autoS" & sportId & "1stOED")), 0)
            autoOE = IIf(isAutoOE, IIf(isCopy1stOU, Request("autoS" & sportId & "1stOUD"), Request("autoS" & sportId & "OED")), 0)
            autoWR = IIf(isAutoOE, IIf(isCopy1stOU, Request("autoS" & sportId & "1stOUD"), Request("autoS" & sportId & "WRD")), 0)

            autoOUL = IIf(isAutoOUL, IIf(isCopyOU, Request("autoS" & sportId & "OUL"), Request("autoS" & sportId & "OUL")), 0)
            autoOEL = IIf(isAutoOEL, IIf(isCopyOU, Request("autoS" & sportId & "OUL"), Request("autoS" & sportId & "OEL")), 0)
            autoHLL = IIf(isAutoHLL, IIf(isCopyOU, Request("autoS" & sportId & "OUL"), Request("autoS" & sportId & "HLL")), 0)
            autoNCL = IIf(isAutoHLL, IIf(isCopyOU, Request("autoS" & sportId & "OUL"), Request("autoS" & sportId & "NCL")), 0)
            autoNGL = IIf(isAutoHLL, IIf(isCopyOU, IIf(UserPermission.IsShowNumberGroup, Request("autoS" & sportId & "OUL"), Request("autoS" & sportId & "NGL")), Request("autoS" & sportId & "NGL")), 0)

            '/----
            a1stOU = IIf(isCopy1stOU, Request("listM" & sportId & "1stOUD"), Request("listM" & sportId & "1stOUD"))
            a1stOE = IIf(isCopy1stOU, Request("listM" & sportId & "1stOUD"), Request("listM" & sportId & "1stOED"))
            aOE = IIf(isCopy1stOU, Request("listM" & sportId & "1stOUD"), Request("listM" & sportId & "OED"))
            aWR = IIf(isCopy1stOU, Request("listM" & sportId & "1stOUD"), Request("listM" & sportId & "WRD"))

            aOUL = IIf(isCopyOU, Request("listM" & sportId & "OUL"), Request("listM" & sportId & "OUL"))
            aOEL = IIf(isCopyOU, Request("listM" & sportId & "OUL"), Request("listM" & sportId & "OEL"))
            aHLL = IIf(isCopyOU, Request("listM" & sportId & "OUL"), Request("listM" & sportId & "HLL"))
            aNCL = IIf(isCopyOU, Request("listM" & sportId & "OUL"), Request("listM" & sportId & "NCL"))
            aNGL = IIf(isCopyOU, IIf(UserPermission.IsShowNumberGroup, Request("listM" & sportId & "OUL"), Request("listM" & sportId & "NGL")), Request("listM" & sportId & "NGL"))

            m1stOU = IIf(isAuto1stOU, 0, IIf(isCopy1stOU, Request("listS" & sportId & "1stOUD"), Request("listS" & sportId & "1stOUD")))
            m1stOE = IIf(isAuto1stOE, 0, IIf(isCopy1stOU, Request("listS" & sportId & "1stOUD"), Request("listS" & sportId & "1stOED")))
            mOE = IIf(isAutoOE, 0, IIf(isCopy1stOU, Request("listS" & sportId & "1stOUD"), Request("listS" & sportId & "OED")))
            mWR = IIf(isAutoWR, 0, IIf(isCopy1stOU, Request("listS" & sportId & "1stOUD"), Request("listS" & sportId & "WRD")))

            mOUL = IIf(isAutoOUL, 0, IIf(isCopyOU, Request("listS" & sportId & "OUL"), Request("listS" & sportId & "OUL")))
            mOEL = IIf(isAutoOEL, 0, IIf(isCopyOU, Request("listS" & sportId & "OUL"), Request("listS" & sportId & "OEL")))
            mHLL = IIf(isAutoHLL, 0, IIf(isCopyOU, Request("listS" & sportId & "OUL"), Request("listS" & sportId & "HLL")))
            mNCL = IIf(isAutoNCL, 0, IIf(isCopyOU, Request("listS" & sportId & "OUL"), Request("listS" & sportId & "NCL")))
            mNGL = IIf(isAutoNGL, 0, IIf(isCopyOU, IIf(UserPermission.IsShowNumberGroup, Request("listS" & sportId & "OUL"), Request("listS" & sportId & "NGL")), Request("listS" & sportId & "NGL")))
        Else
            'Tho modify: Auto Master
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

            '/----

            a1stOU = Request("listM1HdpD")
            a1stOE = Request("listM1HdpD")
            aOE = Request("listM1HdpD")
            aWR = Request("listM1HdpD")

            aOUL = Request("listM1HdpD")
            aOEL = Request("listM1HdpD")
            aHLL = Request("listM1HdpD")
            aNCL = Request("listM1HdpD")
            aNGL = Request("listM1HdpD")

            m1stOU = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            m1stOE = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mOE = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mWR = IIf(isAutoSoccer, 0, Request("listS1HdpD"))

            mOUL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mOEL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mHLL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mNCL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mNGL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))

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

        'Tho modify: Auto Master
        param.Add("auto_v" & sportId & "mpositiontaking_81", auto1stOU) 'smallmoney  
        param.Add("auto_v" & sportId & "mpositiontaking_83", auto1stOE) 'smallmoney  
        param.Add("auto_v" & sportId & "mpositiontaking_86", autoOE) 'smallmoney 
        param.Add("auto_v" & sportId & "mpositiontaking_88", autoWR) 'smallmoney 

        param.Add("auto_v" & sportId & "mlivepositiontaking_85", autoOUL) 'smallmoney  
        param.Add("auto_v" & sportId & "mlivepositiontaking_86", autoOEL) 'smallmoney  
        param.Add("auto_v" & sportId & "mlivepositiontaking_87", autoHLL) 'smallmoney  
        param.Add("auto_v" & sportId & "mlivepositiontaking_89", autoNCL) 'smallmoney 
        param.Add("auto_v" & sportId & "mlivepositiontaking_90", autoNGL) 'smallmoney  
        '/---


    End Sub
#End Region

#Region "Insert parameters for Mixed Sports Position Taking"
    Sub AddParaMixSports()
        Dim minb, maxb, maxpermatch As Double
        minb = IIf(isCopyAll, Request("txtMinbet1"), IIf(isCopyBasket, Request("txtMinbet2"), Request("txtMinbet99_M")))
        maxb = IIf(isCopyAll, Request("txtMaxbet1"), IIf(isCopyBasket, Request("txtMaxbet2"), Request("txtMaxbet99_M")))
        maxpermatch = IIf(isCopyAll, Request("txtMaxPerMatch1"), IIf(isCopyBasket, Request("txtMaxPerMatch2"), Request("txtMaxPerMatch99_M")))
        param.Add("minbetv99mp", minb) 'money  
        param.Add("maxbetv99mp", maxb) 'money  
        param.Add("maxpermatchv99mp", maxpermatch) 'money  

        Dim mMP As Double
        Dim aMP As Double

        'Tho modify: Auto Master
        Dim autoMP As Double
        Dim isAutoMP As Boolean
        isAutoMP = IIf(Request("chkAuto99_M") Is Nothing, 0, 1)

        '/----

        If Not isCopyAll Then 'this case for Copy All PT from Soccer checkbox is unchecked
            If isCopyBasket Then 'this case for Copy All PT from Basketball checkbox is checked    
                'Tho modify: Auto Master
                Dim isAutoBasket As Boolean = False
                If Not Request("chkAuto2Hdp") Is Nothing Then
                    isAutoBasket = True
                End If
                autoMP = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                '/-----
                aMP = Request("listM2Hdp")
                mMP = IIf(isAutoBasket, 0, Request("listS2Hdp"))
            Else
                autoMP = Request("autoS99_M")
                aMP = Request("listM99_M")
                mMP = Request("listS99_M")

            End If
        Else
            'Tho modify: Auto Master
            Dim isAutoSoccer As Boolean = False
            If Not Request("chkAuto1HdpD") Is Nothing Then
                isAutoSoccer = True
            End If
            autoMP = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            '/----
            aMP = Request("listM1HdpD")
            mMP = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
        End If
        param.Add("v99apositiontaking_9", aMP) 'smallmoney          
        param.Add("v99mpositiontaking_9", mMP) 'smallmoney          
        param.Add("auto_v99mpositiontaking_9", autoMP) 'smallmoney       

    End Sub
#End Region

#Region "ReplaceData"
    Public Function ReplaceData(ByVal str As String) As String
        Return str.Replace("""", "&quot;").Replace("'", "&#39;").Replace(vbNewLine, "")
    End Function
#End Region
End Class

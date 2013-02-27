Imports DNAide.Utils
Imports DNAide.Web
Imports Fanex.Data.DNAide
Imports DNAide.Core
Imports System.Web.Security
Imports DataHandler
Imports System.Collections
Imports Sunplus.Agent.Model.Entities.Permission
Public Class UpdateEditMinMaxPT
    Inherits System.Web.UI.Page
    Dim param As New Hashtable
    Dim isCopyAll As Boolean
    Dim isCopyBasket As Boolean
    Dim userName As String = String.Empty

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

        If Not Request("currentUser") Is Nothing And (Request("currentUser") <> "") Then
            userName = Request("currentUser") + ": "
        End If

        'If (Not Sunplus.Agent.Model.Entities.UserSession.Current.SubPageAccess.UpdateAccount And Request("isAllowUpdPT") <> "1") Then
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
        ' Dim isUpdate As Boolean = True

        If Not Request("chkCopyAll") Is Nothing Then
            isCopyAll = True
        End If
        If Not Request("chkCopyBasket") Is Nothing Then
            isCopyBasket = True
        End If
        Dim rs As DbResult

        param.Add("custid", Request("saveCustid")) 'int
        param.Add("updateminpos", 1) 'bit
        param.Add("updatemaxpos", 0) 'bit
        param.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId)
        param.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId)
        param.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey)
        param.Add("writeLog", 1) 'bit  
        param.Add("returnerrcode", 1) 'bit  
        param.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP())
        param.Add("errcode", 0) 'int  
        param.Add("errmsg", "") 'nvarchar  
        AddParaSoccer()
        AddParaFootball(2)
        AddParaFootball(3)
        AddParaFootball(5)
        AddParaBaseball()
        AddParaFootball(10)
        AddParaMotosports()
        AddParaFootball(99)
        AddParaNumberGame(161)
        AddParaMixedSports()

        Dim isPopup As Boolean = False

        If (Not Request("showPopup") Is Nothing) And (Request("showPopup") = 1) Then
            isPopup = True
        End If

        rs = AccountInfo.Acc_CustOverrideUpd(param)
        If Not rs Is Nothing Then
            Dim strErrmsg As String = Resources.LangMsg.succ_107
            Dim CallAPI As Boolean = False
            Dim res As Integer = CommonGenerateHTML.GenerateErrorMsg(rs.Outputs("errcode").ToString(), rs.Outputs("errmsg").ToString(), strErrmsg, CallAPI)
            If res <> 0 Then
                'isUpdate = False
                Response.Write("<script type=""text/javascript"">parent.UpdateErr('" & strErrmsg & "');</script>")
            Else
                Dim hash As Hashtable = AppSession.RequestQueryString
                Dim FromAccount As String
                FromAccount = IIf(hash.ContainsKey("FromAccount"), hash("FromAccount"), "")
                If (String.IsNullOrEmpty(FromAccount)) Then
                    Dim strScript As String = String.Empty

                    If (isPopup) Then
                        strScript = "<script type='text/javascript'> parent.parent.DelayPage();</script>"
                    Else
                        strScript = "<script type='text/javascript'> parent.showErrMsg('" & userName & strErrmsg & "',1); parent.enabledBtn(true); window.scroll(0, 0);</script>"
                    End If

                    Response.Write(strScript)
                Else 'from AccountTab
                    Dim urlBack As String = Context.Request.UrlReferrer.OriginalString
                    If (Not Context.Request.UrlReferrer.Query.Contains("succ")) Then
                        urlBack += "&succ=1"
                    End If
                    Response.Write("<script type='text/javascript'>parent.DelayReloadPage('" + urlBack + "');</script>")
                End If
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
    Sub AddParaSoccer()
        '***** Start Inserting for Soccer *****

        Dim mHdpD, mOUD, m1stHdpD, m1stOUD, mOE, m1x2, mCS, mTG, mMp, mOR As Double
        Dim mHdpL, mOUL, m1stHdpL, m1stOUL As Double
        'Updating Positon Taking in Single Edit or Allow update in Multiple Edit
        Dim isCopyHdpD As Boolean = False
        If Not Request("chkCopy1HdpDeadball") Is Nothing Then
            isCopyHdpD = True
        End If
        Dim isCopy1x2 As Boolean = False
        If Not Request("chkCopy11x2") Is Nothing Then
            isCopy1x2 = True
        End If
        Dim isCopyHdpL As Boolean = False
        If Not Request("chkCopy1HdpLive") Is Nothing Then
            isCopyHdpL = True
        End If
        If Not isCopyAll Then 'this case for Copy All PT checkbox is checked              
            mHdpD = IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM1HdpD"))
            mOUD = IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM1OUD"))
            m1stHdpD = IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM11stHdpD"))
            m1stOUD = IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM11stOUD"))
            mOE = IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM1OE"))

            m1x2 = IIf(isCopy1x2, Request("listM11x2"), Request("listM11x2"))
            mCS = IIf(isCopy1x2, Request("listM11x2"), Request("listM1CS"))
            mTG = IIf(isCopy1x2, Request("listM11x2"), Request("listM1TG"))
            mMp = IIf(isCopy1x2, Request("listM11x2"), Request("listM1MP"))
            mOR = IIf(isCopy1x2, Request("listM11x2"), Request("listM1OR"))

            mHdpL = IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM1HdpL"))
            mOUL = IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM1OUL"))
            m1stHdpL = IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM11stHdpL"))
            m1stOUL = IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM11stOUL"))
        Else
            mHdpD = Request("listM1HdpD")
            mOUD = Request("listM1HdpD")
            m1stHdpD = Request("listM1HdpD")
            m1stOUD = Request("listM1HdpD")
            mOE = Request("listM1HdpD")

            m1x2 = Request("listM1HdpD")
            mCS = Request("listM1HdpD")
            mTG = Request("listM1HdpD")
            mMp = Request("listM1HdpD")
            mOR = Request("listM1HdpD")

            mHdpL = Request("listM1HdpD")
            mOUL = Request("listM1HdpD")
            m1stHdpL = Request("listM1HdpD")
            m1stOUL = Request("listM1HdpD")
        End If
        param.Add("minpositiontaking_1", mHdpD) 'smallmoney  
        param.Add("minpositiontaking_3", mOUD) 'smallmoney  
        param.Add("minpositiontaking_7", m1stHdpD) 'smallmoney  
        param.Add("minpositiontaking_8", m1stOUD) 'smallmoney  
        param.Add("minpositiontaking_2", mOE) 'smallmoney  

        param.Add("minpositiontaking_5", m1x2) 'smallmoney  
        param.Add("minpositiontaking_4", mCS) 'smallmoney  
        param.Add("minpositiontaking_6", mTG) 'smallmoney              
        param.Add("minpositiontaking_9", mMp) 'smallmoney  
        param.Add("minpositiontaking_10", mOR) 'smallmoney 

        param.Add("minlivepositiontaking_1", mHdpL) 'smallmoney  
        param.Add("minlivepositiontaking_3", mOUL) 'smallmoney  
        param.Add("minlivepositiontaking_7", m1stHdpL) 'smallmoney  
        param.Add("minlivepositiontaking_8", m1stOUL) 'smallmoney  

        param.Add("maxpositiontaking_1", 0) 'smallmoney  
        param.Add("maxpositiontaking_2", 0) 'smallmoney  
        param.Add("maxpositiontaking_3", 0) 'smallmoney  
        param.Add("maxpositiontaking_4", 0) 'smallmoney  
        param.Add("maxpositiontaking_5", 0) 'smallmoney  
        param.Add("maxpositiontaking_6", 0) 'smallmoney  
        param.Add("maxpositiontaking_7", 0) 'smallmoney  
        param.Add("maxpositiontaking_8", 0) 'smallmoney  
        param.Add("maxpositiontaking_9", 0) 'smallmoney  
        param.Add("maxpositiontaking_10", 0) 'smallmoney  
        param.Add("maxlivepositiontaking_1", 0) 'smallmoney  
        param.Add("maxlivepositiontaking_3", 0) 'smallmoney  
        param.Add("maxlivepositiontaking_7", 0) 'smallmoney  
        param.Add("maxlivepositiontaking_8", 0) 'smallmoney  

    End Sub
#End Region

#Region "Insert parameters for Basketball, Football, Tennis, Golf, Other Sports Position Taking"
    Sub AddParaFootball(ByVal sportId As String)

        '***** Start Inserting for Football *****
        Dim mHdp, mOU, mOE, mML As Double
        Dim isCopyHdp As Boolean = False
        If Not Request("chkCopy" & sportId & "Hdp") Is Nothing Then
            isCopyHdp = True
        End If
        If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
            If isCopyBasket Then
                mHdp = Request("listM2Hdp")
                mOU = Request("listM2Hdp")
                mOE = Request("listM2Hdp")
                mML = Request("listM2Hdp")
            Else
                mHdp = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "Hdp"))
                mOU = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "OU"))
                mOE = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "OE"))
                mML = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "ML"))
            End If
        Else
            mHdp = Request("listM1HdpD")
            mOU = Request("listM1HdpD")
            mOE = Request("listM1HdpD")
            mML = Request("listM1HdpD")
        End If
        param.Add("minv" & sportId & "positiontaking_1", mHdp) 'smallmoney  
        param.Add("minv" & sportId & "positiontaking_3", mOU) 'smallmoney  
        param.Add("minv" & sportId & "positiontaking_2", mOE) 'smallmoney  
        param.Add("minv" & sportId & "positiontaking_20", mML) 'smallmoney             

        param.Add("maxv" & sportId & "positiontaking_1", 0) 'smallmoney  
        param.Add("maxv" & sportId & "positiontaking_3", 0) 'smallmoney  
        param.Add("maxv" & sportId & "positiontaking_2", 0) 'smallmoney  
        param.Add("maxv" & sportId & "positiontaking_20", 0) 'smallmoney             

        If sportId = "2" Then
            param.Add("minv2positiontaking_7", mHdp) 'smallmoney 
            param.Add("minv2positiontaking_8", mOU) 'smallmoney   

            param.Add("maxv2positiontaking_7", 0) 'smallmoney 
            param.Add("maxv2positiontaking_8", 0) 'smallmoney     

        End If


    End Sub

#End Region

#Region "Insert parameters for Baseball Position Taking"
    Sub AddParaBaseball()

        '***** Start Inserting for Baseball *****
        Dim mHdp, mOU, mML As Double
        Dim isCopyHdp As Boolean = False
        If Not Request("chkCopy8Hdp") Is Nothing Then
            isCopyHdp = True
        End If
        If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
            If isCopyBasket Then
                mHdp = Request("listM2Hdp")
                mOU = Request("listM2Hdp")
                mML = Request("listM2Hdp")
            Else
                mHdp = IIf(isCopyHdp, Request("listM8Hdp"), Request("listM8Hdp"))
                mOU = IIf(isCopyHdp, Request("listM8Hdp"), Request("listM8OU"))
                mML = IIf(isCopyHdp, Request("listM8Hdp"), Request("listM8ML"))
            End If
        Else
            mHdp = Request("listM1HdpD")
            mOU = Request("listM1HdpD")
            mML = Request("listM1HdpD")
        End If
        param.Add("minv8positiontaking_1", mHdp) 'smallmoney  
        param.Add("minv8positiontaking_3", mOU) 'smallmoney 
        param.Add("minv8positiontaking_20", mML) 'smallmoney 

        param.Add("maxv8positiontaking_1", 0) 'smallmoney  
        param.Add("maxv8positiontaking_3", 0) 'smallmoney  
        param.Add("maxv8positiontaking_20", 0) 'smallmoney  
        '***** End Inserting for Baseball *****


    End Sub
#End Region

#Region "Insert parameters for Motosports Position Taking"
    Sub AddParaMotosports()

        '***** Start Inserting for MotoSports *****
        Dim mHdp, mML As Double
        Dim isCopyHdp As Boolean = False
        If Not Request("chkCopy11Hdp") Is Nothing Then
            isCopyHdp = True
        End If
        If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
            If isCopyBasket Then
                mHdp = Request("listM2Hdp")
                mML = Request("listM2Hdp")
            Else
                mHdp = IIf(isCopyHdp, Request("listM11Hdp"), Request("listM11Hdp"))
                mML = IIf(isCopyHdp, Request("listM11Hdp"), Request("listM11ML"))
            End If
        Else
            mHdp = Request("listM1HdpD")
            mML = Request("listM1HdpD")
        End If
        param.Add("minv11positiontaking_1", mHdp) 'smallmoney         
        param.Add("minv11positiontaking_20", mML) 'smallmoney 

        param.Add("maxv11positiontaking_1", 0) 'smallmoney  
        param.Add("maxv11positiontaking_20", 0) 'smallmoney  
        '***** End Inserting for MotoSports *****


    End Sub
#End Region


#Region "Insert parameters for Number Game Position Taking"
    Sub AddParaNumberGame(ByVal sportId As String)

        Dim isDisableNumberGame As Boolean = False
        If (Not UserPermission.IsShowNumberGame) Then
            isDisableNumberGame = True
        End If


        Dim m1stOU, m1stOE, mOE, mWR As Double
        Dim mOUL, mOEL, mHLL, mNCL, mNGL As Double

        Dim isCopy1stOU As Boolean = False
        If Not Request("chkCopy" & sportId & "1stOUD") Is Nothing Then
            isCopy1stOU = True
        End If

        Dim isCopyOU As Boolean = False
        If Not Request("chkCopy" & sportId & "OUL") Is Nothing Then
            isCopyOU = True
        End If

        If Not isDisableNumberGame Then 'this case for Copy All PT checkbox is checked
            m1stOU = IIf(isCopy1stOU, Request("listM" & sportId & "1stOUD"), Request("listM" & sportId & "1stOUD"))
            m1stOE = IIf(isCopy1stOU, Request("listM" & sportId & "1stOUD"), Request("listM" & sportId & "1stOED"))
            'mOU = IIf(isCopy1stOU, Request("listM" & sportId & "1stOUD"), Request("listM" & sportId & "OUD"))
            mOE = IIf(isCopy1stOU, Request("listM" & sportId & "1stOUD"), Request("listM" & sportId & "OED"))
            mWR = IIf(isCopy1stOU, Request("listM" & sportId & "1stOUD"), Request("listM" & sportId & "WRD"))

            mOUL = IIf(isCopyOU, Request("listM" & sportId & "OUL"), Request("listM" & sportId & "OUL"))
            mOEL = IIf(isCopyOU, Request("listM" & sportId & "OUL"), Request("listM" & sportId & "OEL"))
            mHLL = IIf(isCopyOU, Request("listM" & sportId & "OUL"), Request("listM" & sportId & "HLL"))
            mNCL = IIf(isCopyOU, Request("listM" & sportId & "OUL"), Request("listM" & sportId & "NCL"))
            mNGL = IIf(isCopyOU, IIf(UserPermission.IsShowNumberGroup, Request("listM" & sportId & "OUL"), Request("listM" & sportId & "NGL")), Request("listM" & sportId & "NGL"))
        Else
            m1stOU = Request("listM1HdpD")
            m1stOE = Request("listM1HdpD")
            'mOU = Request("listM1HdpD")
            mOE = Request("listM1HdpD")
            mWR = Request("listM1HdpD")

            mOUL = Request("listM1HdpD")
            mOEL = Request("listM1HdpD")
            mHLL = Request("listM1HdpD")
            mNCL = Request("listM1HdpD")
            mNGL = Request("listM1HdpD")
        End If
        param.Add("minv" & sportId & "positiontaking_81", m1stOU) 'smallmoney  
        param.Add("minv" & sportId & "positiontaking_83", m1stOE) 'smallmoney  
        'param.Add("minv" & sportId & "positiontaking_85", mOU) 'smallmoney  
        param.Add("minv" & sportId & "positiontaking_86", mOE) 'smallmoney    
        param.Add("minv" & sportId & "positiontaking_88", mWR) 'smallmoney    

        param.Add("minv" & sportId & "livepositiontaking_85", mOUL) 'smallmoney  
        param.Add("minv" & sportId & "livepositiontaking_86", mOEL) 'smallmoney  
        param.Add("minv" & sportId & "livepositiontaking_87", mHLL) 'smallmoney  
        param.Add("minv" & sportId & "livepositiontaking_89", mNCL) 'smallmoney  
        param.Add("minv" & sportId & "livepositiontaking_90", mNGL) 'smallmoney  

        param.Add("maxv" & sportId & "positiontaking_81", 0) 'smallmoney  
        param.Add("maxv" & sportId & "positiontaking_83", 0) 'smallmoney  
        'param.Add("maxv" & sportId & "positiontaking_85", 0) 'smallmoney  
        param.Add("maxv" & sportId & "positiontaking_86", 0) 'smallmoney      
        param.Add("maxv" & sportId & "positiontaking_88", 0) 'smallmoney    

        param.Add("maxv" & sportId & "livepositiontaking_85", 0) 'smallmoney  
        param.Add("maxv" & sportId & "livepositiontaking_86", 0) 'smallmoney  
        param.Add("maxv" & sportId & "livepositiontaking_87", 0) 'smallmoney  
        param.Add("maxv" & sportId & "livepositiontaking_89", 0) 'smallmoney  
        param.Add("maxv" & sportId & "livepositiontaking_90", 0) 'smallmoney  



    End Sub

#End Region

#Region "Insert parameters for Mixed Sports Position Taking"
    Sub AddParaMixedSports()

        Dim mmp As Double
        If Not isCopyAll Then
            If isCopyBasket Then
                mmp = Request("listM2Hdp")
            Else
                mmp = Request("listM99_M")
            End If
        Else
            mmp = Request("listM1HdpD")
        End If
        param.Add("minv99positiontaking_9", mmp) 'smallmoney                    
        param.Add("maxv99positiontaking_9", 0) 'smallmoney  



    End Sub

#End Region
    '---------------------------------------------------------------------------

#Region "ReplaceData"
    Public Shared Function ReplaceData(ByVal str As String) As String
        Return str.Replace("""", "&quot;").Replace("'", "&#39;").Replace(vbNewLine, "")
    End Function
#End Region
End Class

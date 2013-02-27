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

Public Class UpdateNewMember
    Inherits System.Web.UI.Page
    Dim param As New Hashtable
    Dim rule1 As HttpFrequenceRule

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim errMsg As String = ""
        rule1 = New HttpFrequenceRule("UpdateNewMember.aspx", 20, TimeSpan.FromDays(2))
        ' rule1 is violated and enter right capchar --> reset
        ' rule1 is violated and enter wrong capchar --> return
        Dim code As String = Request("txtCaptchaAddNewMember")

        If rule1.IsViolated Then
            If code <> Sunplus.Agent.Model.Entities.UserSession.Current.CaptchaCode Then
                errMsg = Resources.Lang.errInvalidCaptcha
                Dim customErrMsg As String = ""
                CommonGenerateHTML.GenerateErrorMsg(ReplaceData(errMsg), customErrMsg)
                Response.Write("<script type=""text/javascript"">parent.UpdateErr('" & customErrMsg & "');</script>")
                Response.Write("<script type=""text/javascript"">parent.GetValidateImage();</script>")
                Response.Write("<script type=""text/javascript"">parent.$('txtCaptchaAddNewMember').value = '';</script>")
                Response.Write("<script type=""text/javascript"">parent.$('txtCaptchaAddNewMember').focus();</script>")
                Return
            Else
                rule1.Reset()
            End If
        End If


        If (Request("mode") Is Nothing) Then 'add

            If (Not Sunplus.Agent.Model.Entities.UserSession.Current.SubPageAccess.CreateAccount) Then
                errMsg = Resources.Lang.lblNotAuthorized
                Dim customErrMsg As String = ""
                CommonGenerateHTML.GenerateErrorMsg(ReplaceData(errMsg), customErrMsg)
                Response.Write("<script type=""text/javascript"">parent.UpdateErr('" & customErrMsg & "');</script>")
                Return
            Else
                InsertData()
            End If
        Else 'edit
            If (Not Sunplus.Agent.Model.Entities.UserSession.Current.SubPageAccess.UpdateAccount) Then
                errMsg = Resources.Lang.lblNotAuthorized
                Dim customErrMsg As String = ""
                CommonGenerateHTML.GenerateErrorMsg(ReplaceData(errMsg), customErrMsg)
                Response.Write("<script type=""text/javascript"">parent.UpdateErr('" & customErrMsg & "');</script>")
                Return
            Else
                InsertData()
            End If
        End If

    End Sub
#Region "InsertData()"
    Sub InsertData()

        Dim username As String = String.Empty
        Dim rs As DbResult = Nothing
        Dim _errcode As String = ""
        Dim _errMsg As String = ""
        Dim _username As String = ""
        Dim isMult As Boolean = CastAide.AsBoolean(CastAide.AsInt32(Request("isMult"), 0), False)
        Dim Ids As String = Request("txtIDs")
        Dim str As String = "0-"
        Dim CID() As String = {"1"} ' new
        If isMult Then ' edit multi
            CID = Ids.Split("^")
        ElseIf (Not Request("custid") Is Nothing) Then ' edit single
            CID = Request("custid").Split("^")
        End If

        Dim custid As String = String.Empty
        For Each custid In CID
            param.Clear()
            param.Add("firstname", IIf(isMult, String.Empty, ReplaceData(Request("txtFName")))) 'nvarchar  
            param.Add("lastname", IIf(isMult, String.Empty, ReplaceData(Request("txtLName")))) 'nvarchar  


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

            If (Request("mode") Is Nothing) Then 'add new              

                param.Add("creator", Sunplus.Agent.Model.Entities.UserSession.Current.UserId) 'int  
                param.Add("custid", 0) 'int 
                username = Sunplus.Agent.Model.Entities.UserSession.Current.UserName + Request("number1").Trim + Request("number2").Trim + Request("number3").Trim
                param.Add("username", username)  'varchar  
                param.Add("userpwd", Encryption.CfsEnCode(Request("txtPwd").Trim)) 'varchar  
                'param.Add("discount", Request("hidden_discount")) 'decimal  
                'param.Add("discountcs", Request("hidden_discountcs")) 'decimal  

                param.Add("recommend", Sunplus.Agent.Model.Entities.UserSession.Current.CustId) 'int 
                param.Add("ugroup", Request("ddlUGruop")) 'varchar  
                param.Add("answer", "") 'varchar  
                param.Add("success", 0) 'bit 
                param.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId) 'int                                   

            Else ' edit
                Dim iscopyAll As Boolean = False
                Dim iscopyBasketBall As Boolean = False
                If Not (Request("chkCopyAll")) Is Nothing Then
                    iscopyAll = True
                Else
                    If Not (Request("chkCopyBasket")) Is Nothing Then
                        iscopyBasketBall = True
                    End If
                End If



                param.Add("discasino", Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledCasino) 'nvarchar
                param.Add("custid", custid)  'int  
                param.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId)
                param.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId) ' Sub-account
                param.Add("userName", "") ' varchar(20) = '' OUTPUT
                param.Add("updatemultiple", isMult) 'bit  

                Dim updComm1 As Integer = IIf(Request("chkUpdComm") Is Nothing, 0, 1)
                param.Add("updatecommission", IIf(isMult, updComm1, 1)) ' bit

                If Not Request("chkUpdSoccerBet") Is Nothing Or Not isMult Then
                    param.Add("updateminmaxbet", 1) ' bit
                Else
                    param.Add("updateminmaxbet", 0) ' bit
                End If

                If Not Request("chkUpdSoccerPT") Is Nothing Or Not isMult Then
                    param.Add("updatepositiontaking", 1) 'bit  
                Else
                    param.Add("updatepositiontaking", 0) 'bit  
                End If

                ' Edit by David 03/02/2012
                If Not Request("chkUpdSoccerBet") Is Nothing Or Not Request("chkUpd2Bet") Is Nothing Or Not Request("chkUpd2Bet") Is Nothing Or Not isMult Then
                    If iscopyAll = False Then
                        If iscopyBasketBall Then
                            param.Add("updateminmaxbetv2", IIf(Request("chkUpd2Bet") Is Nothing, 0, 1)) 'bit 
                        Else
                            param.Add("updateminmaxbetv2", IIf(Request("chkUpd2Bet") Is Nothing, 0, 1)) 'bit 
                        End If
                    Else
                        param.Add("updateminmaxbetv2", IIf(Request("chkUpdSoccerBet") Is Nothing, 0, 1)) 'bit 
                    End If
                Else
                    param.Add("updateminmaxbetv2", 0) 'bit                  
                End If


                If Not Request("chkUpdSoccerPT") Is Nothing Or Not Request("chkUpd2PT") Is Nothing Or Not Request("chkUpd2PT") Is Nothing Or Not isMult Then
                    If iscopyAll = False Then
                        If iscopyBasketBall Then
                            param.Add("updatepositiontakingv2", IIf(Request("chkUpd2PT") Is Nothing, 0, 1)) 'bit 
                        Else
                            param.Add("updatepositiontakingv2", IIf(Request("chkUpd2PT") Is Nothing, 0, 1)) 'bit 
                        End If
                    Else
                        param.Add("updatepositiontakingv2", IIf(Request("chkUpdSoccerPT") Is Nothing, 0, 1)) 'bit 
                    End If
                Else
                    param.Add("updatepositiontakingv2", 0) 'bit 
                End If



                If Not Request("chkUpdSoccerBet") Is Nothing Or Not Request("chkUpd2Bet") Is Nothing Or Not Request("chkUpd3Bet") Is Nothing Or Not isMult Then
                    If iscopyAll = False Then
                        If iscopyBasketBall Then
                            param.Add("updateminmaxbetv3", IIf(Request("chkUpd2Bet") Is Nothing, 0, 1)) 'bit 
                        Else
                            param.Add("updateminmaxbetv3", IIf(Request("chkUpd3Bet") Is Nothing, 0, 1)) 'bit 
                        End If
                    Else
                        param.Add("updateminmaxbetv3", IIf(Request("chkUpdSoccerBet") Is Nothing, 0, 1)) 'bit 
                    End If
                Else
                    param.Add("updateminmaxbetv3", 0) 'bit                  
                End If


                If Not Request("chkUpdSoccerPT") Is Nothing Or Not Request("chkUpd2PT") Is Nothing Or Not Request("chkUpd3PT") Is Nothing Or Not isMult Then
                    If iscopyAll = False Then
                        If iscopyBasketBall Then
                            param.Add("updatepositiontakingv3", IIf(Request("chkUpd2PT") Is Nothing, 0, 1)) 'bit 
                        Else
                            param.Add("updatepositiontakingv3", IIf(Request("chkUpd3PT") Is Nothing, 0, 1)) 'bit 
                        End If
                    Else
                        param.Add("updatepositiontakingv3", IIf(Request("chkUpdSoccerPT") Is Nothing, 0, 1)) 'bit 
                    End If
                Else
                    param.Add("updatepositiontakingv3", 0) 'bit 
                End If




                If Not Request("chkUpdSoccerBet") Is Nothing Or Not Request("chkUpd2Bet") Is Nothing Or Not Request("chkUpd5Bet") Is Nothing Or Not isMult Then
                    If iscopyAll = False Then
                        If iscopyBasketBall Then
                            param.Add("updateminmaxbetv5", IIf(Request("chkUpd2Bet") Is Nothing, 0, 1)) 'bit 
                        Else
                            param.Add("updateminmaxbetv5", IIf(Request("chkUpd5Bet") Is Nothing, 0, 1)) 'bit 
                        End If
                    Else
                        param.Add("updateminmaxbetv5", IIf(Request("chkUpdSoccerBet") Is Nothing, 0, 1)) 'bit 
                    End If
                Else
                    param.Add("updateminmaxbetv5", 0) 'bit                  
                End If


                If Not Request("chkUpdSoccerPT") Is Nothing Or Not Request("chkUpd2PT") Is Nothing Or Not Request("chkUpd5PT") Is Nothing Or Not isMult Then
                    If iscopyAll = False Then
                        If iscopyBasketBall Then
                            param.Add("updatepositiontakingv5", IIf(Request("chkUpd2PT") Is Nothing, 0, 1)) 'bit 
                        Else
                            param.Add("updatepositiontakingv5", IIf(Request("chkUpd5PT") Is Nothing, 0, 1)) 'bit 
                        End If
                    Else
                        param.Add("updatepositiontakingv5", IIf(Request("chkUpdSoccerPT") Is Nothing, 0, 1)) 'bit 
                    End If
                Else
                    param.Add("updatepositiontakingv5", 0) 'bit 
                End If




                If Not Request("chkUpdSoccerBet") Is Nothing Or Not Request("chkUpd2Bet") Is Nothing Or Not Request("chkUpd8Bet") Is Nothing Or Not isMult Then
                    If iscopyAll = False Then
                        If iscopyBasketBall Then
                            param.Add("updateminmaxbetv8", IIf(Request("chkUpd2Bet") Is Nothing, 0, 1)) 'bit 
                        Else
                            param.Add("updateminmaxbetv8", IIf(Request("chkUpd8Bet") Is Nothing, 0, 1)) 'bit 
                        End If
                    Else
                        param.Add("updateminmaxbetv8", IIf(Request("chkUpdSoccerBet") Is Nothing, 0, 1)) 'bit 
                    End If
                Else
                    param.Add("updateminmaxbetv8", 0) 'bit                  
                End If


                If Not Request("chkUpdSoccerPT") Is Nothing Or Not Request("chkUpd2PT") Is Nothing Or Not Request("chkUpd8PT") Is Nothing Or Not isMult Then
                    If iscopyAll = False Then
                        If iscopyBasketBall Then
                            param.Add("updatepositiontakingv8", IIf(Request("chkUpd2PT") Is Nothing, 0, 1)) 'bit 
                        Else
                            param.Add("updatepositiontakingv8", IIf(Request("chkUpd8PT") Is Nothing, 0, 1)) 'bit 
                        End If
                    Else
                        param.Add("updatepositiontakingv8", IIf(Request("chkUpdSoccerPT") Is Nothing, 0, 1)) 'bit 
                    End If
                Else
                    param.Add("updatepositiontakingv8", 0) 'bit 
                End If





                If Not Request("chkUpdSoccerBet") Is Nothing Or Not Request("chkUpd2Bet") Is Nothing Or Not Request("chkUpd10Bet") Is Nothing Or Not isMult Then
                    If iscopyAll = False Then
                        If iscopyBasketBall Then
                            param.Add("updateminmaxbetv10", IIf(Request("chkUpd2Bet") Is Nothing, 0, 1)) 'bit 
                        Else
                            param.Add("updateminmaxbetv10", IIf(Request("chkUpd10Bet") Is Nothing, 0, 1)) 'bit 
                        End If
                    Else
                        param.Add("updateminmaxbetv10", IIf(Request("chkUpdSoccerBet") Is Nothing, 0, 1)) 'bit 
                    End If
                Else
                    param.Add("updateminmaxbetv10", 0) 'bit                  
                End If


                If Not Request("chkUpdSoccerPT") Is Nothing Or Not Request("chkUpd2PT") Is Nothing Or Not Request("chkUpd10PT") Is Nothing Or Not isMult Then
                    If iscopyAll = False Then
                        If iscopyBasketBall Then
                            param.Add("updatepositiontakingv10", IIf(Request("chkUpd2PT") Is Nothing, 0, 1)) 'bit 
                        Else
                            param.Add("updatepositiontakingv10", IIf(Request("chkUpd10PT") Is Nothing, 0, 1)) 'bit 
                        End If
                    Else
                        param.Add("updatepositiontakingv10", IIf(Request("chkUpdSoccerPT") Is Nothing, 0, 1)) 'bit 
                    End If
                Else
                    param.Add("updatepositiontakingv10", 0) 'bit 
                End If




                If Not Request("chkUpdSoccerBet") Is Nothing Or Not Request("chkUpd2Bet") Is Nothing Or Not Request("chkUpd11Bet") Is Nothing Or Not isMult Then
                    If iscopyAll = False Then
                        If iscopyBasketBall Then
                            param.Add("updateminmaxbetv11", IIf(Request("chkUpd2Bet") Is Nothing, 0, 1)) 'bit 
                        Else
                            param.Add("updateminmaxbetv11", IIf(Request("chkUpd11Bet") Is Nothing, 0, 1)) 'bit 
                        End If
                    Else
                        param.Add("updateminmaxbetv11", IIf(Request("chkUpdSoccerBet") Is Nothing, 0, 1)) 'bit 
                    End If
                Else
                    param.Add("updateminmaxbetv11", 0) 'bit                  
                End If


                If Not Request("chkUpdSoccerPT") Is Nothing Or Not Request("chkUpd2PT") Is Nothing Or Not Request("chkUpd11PT") Is Nothing Or Not isMult Then
                    If iscopyAll = False Then
                        If iscopyBasketBall Then
                            param.Add("updatepositiontakingv11", IIf(Request("chkUpd2PT") Is Nothing, 0, 1)) 'bit 
                        Else
                            param.Add("updatepositiontakingv11", IIf(Request("chkUpd11PT") Is Nothing, 0, 1)) 'bit 
                        End If
                    Else
                        param.Add("updatepositiontakingv11", IIf(Request("chkUpdSoccerPT") Is Nothing, 0, 1)) 'bit 
                    End If
                Else
                    param.Add("updatepositiontakingv11", 0) 'bit 
                End If



                If Not Request("chkUpdSoccerBet") Is Nothing Or Not Request("chkUpd2Bet") Is Nothing Or Not Request("chkUpd99Bet") Is Nothing Or Not isMult Then
                    If iscopyAll = False Then
                        If iscopyBasketBall Then
                            param.Add("updateminmaxbetv99", IIf(Request("chkUpd2Bet") Is Nothing, 0, 1)) 'bit 
                        Else
                            param.Add("updateminmaxbetv99", IIf(Request("chkUpd99Bet") Is Nothing, 0, 1)) 'bit 
                        End If
                    Else
                        param.Add("updateminmaxbetv99", IIf(Request("chkUpdSoccerBet") Is Nothing, 0, 1)) 'bit 
                    End If
                Else
                    param.Add("updateminmaxbetv99", 0) 'bit                  
                End If


                If Not Request("chkUpdSoccerPT") Is Nothing Or Not Request("chkUpd2PT") Is Nothing Or Not Request("chkUpd99PT") Is Nothing Or Not isMult Then
                    If iscopyAll = False Then
                        If iscopyBasketBall Then
                            param.Add("updatepositiontakingv99", IIf(Request("chkUpd2PT") Is Nothing, 0, 1)) 'bit 
                        Else
                            param.Add("updatepositiontakingv99", IIf(Request("chkUpd99PT") Is Nothing, 0, 1)) 'bit 
                        End If
                    Else
                        param.Add("updatepositiontakingv99", IIf(Request("chkUpdSoccerPT") Is Nothing, 0, 1)) 'bit 
                    End If
                Else
                    param.Add("updatepositiontakingv99", 0) 'bit 
                End If



                If Not Request("chkUpd161Bet") Is Nothing Or Not isMult Then
                    param.Add("updateminmaxbetv161", 1) ' bit
                Else
                    If Not UserPermission.IsShowNumberGame And Not Request("chkUpdSoccerBet") Is Nothing Then
                        param.Add("updateminmaxbetv161", 1) ' bit
                    Else
                        param.Add("updateminmaxbetv161", 0) ' bit
                    End If

                End If

                If Not Request("chkUpd161PT") Is Nothing Or Not isMult Then
                    param.Add("updatepositiontakingv161", 1) ' bit
                Else
                    If Not UserPermission.IsShowNumberGame And Not Request("chkUpdSoccerPT") Is Nothing Then
                        param.Add("updatepositiontakingv161", 1) ' bit
                    Else
                        param.Add("updatepositiontakingv161", 0) ' bit
                    End If

                End If

                If Not Request("chkUpdSoccerBet") Is Nothing Or Not Request("chkUpd2Bet") Is Nothing Or Not Request("chkUpd99_MBet") Is Nothing Or Not isMult Then
                    If iscopyAll = False Then
                        If iscopyBasketBall Then
                            param.Add("updateminmaxbetv99mp", IIf(Request("chkUpd2Bet") Is Nothing, 0, 1)) 'bit 
                        Else
                            param.Add("updateminmaxbetv99mp", IIf(Request("chkUpd99_MBet") Is Nothing, 0, 1)) 'bit 
                        End If
                    Else
                        param.Add("updateminmaxbetv99mp", IIf(Request("chkUpdSoccerBet") Is Nothing, 0, 1)) 'bit 
                    End If
                Else
                    param.Add("updateminmaxbetv99mp", 0) 'bit                  
                End If


                If Not Request("chkUpdSoccerPT") Is Nothing Or Not Request("chkUpd2PT") Is Nothing Or Not Request("chkUpd99_MPT") Is Nothing Or Not isMult Then
                    If iscopyAll = False Then
                        If iscopyBasketBall Then
                            param.Add("updatepositiontakingv99mp", IIf(Request("chkUpd2PT") Is Nothing, 0, 1)) 'bit 
                        Else
                            param.Add("updatepositiontakingv99mp", IIf(Request("chkUpd99_MPT") Is Nothing, 0, 1)) 'bit 
                        End If
                    Else
                        param.Add("updatepositiontakingv99mp", IIf(Request("chkUpdSoccerPT") Is Nothing, 0, 1)) 'bit 
                    End If
                Else
                    param.Add("updatepositiontakingv99mp", 0) 'bit 
                End If

                '---------------------------------------by David-------------------------------


                param.Add("allowpos", CInt(Request("txtAllowPT"))) 'bit  
                param.Add("username", "")
            End If
            param.Add("phone", IIf(Not Request("txtPhone") Is Nothing, Request("txtPhone"), String.Empty)) 'nvarchar  
            param.Add("mobilephone", IIf(Not Request("txtMobile") Is Nothing, Request("txtMobile"), String.Empty)) 'nvarchar  
            param.Add("fax", IIf(Not Request("txtFax") Is Nothing, Request("txtFax"), String.Empty))  'varchar  
            param.Add("playerdiscount", IIf(Not Request("ddrCommissionH_OU_OE") Is Nothing, Request("ddrCommissionH_OU_OE"), 0))  'decimal  
            param.Add("playerdiscountcs", IIf(Not Request("ddrCommissionCS_TG") Is Nothing, Request("ddrCommissionCS_TG"), 0))  'decimal  
            param.Add("playerdiscount1x2", IIf(Not Request("ddrCommission1x2") Is Nothing, Request("ddrCommission1x2"), 0))  'decimal  
            param.Add("playerdiscountnumber", IIf(UserPermission.IsShowNumberGame, IIf(Not Request("ddrCommissionNumber") Is Nothing, Request("ddrCommissionNumber"), 0), 0))  'decimal  



            param.Add("credit", IIf(Not Request("txtCredit") Is Nothing, Request("txtCredit"), 0))  'money  

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

            AddParaAllSportype()

            param.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey)
            param.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP())
            param.Add("errcode", 0) 'int  
            param.Add("errmsg", "") 'nvarchar  

            If (Request("mode") Is Nothing) Then 'insert
                rs = AccountInfo.Acc_CustomerInsMember_Ag(param)
            ElseIf Not (Request("mode") Is Nothing) Then
                If (Request("mode") = "edit") Then 'edit
                    rs = AccountInfo.Acc_CustomerUpdEditCustomer_Ag(param)
                End If
            End If
            If (isMult) Then
                If rs.Outputs("errcode") <> "" Then
                    _errcode = _errcode & rs.Outputs("errcode").ToString().Substring(0, rs.Outputs("errcode").ToString().Length - 1) & "$"
                    _errMsg = _errMsg & rs.Outputs("errmsg").ToString().Substring(0, rs.Outputs("errmsg").ToString().Length - 1) & "$"
                    _username = _username & rs.Outputs("username") & "^"
                End If
            Else
                _errcode = rs.Outputs("errcode")
                _errMsg = rs.Outputs("errmsg")
            End If

        Next

        Dim callAPI As Boolean = False
        Dim strErrmsg As String = ""
        Dim res As Integer = 0

        If (Request("mode") Is Nothing) Then 'insert
            strErrmsg = _errMsg
            res = CInt(_errcode)
        Else
            res = CommonGenerateHTML.GenerateErrorMsg(_errcode, _errMsg, strErrmsg, _username, callAPI)
        End If

        If callAPI Then
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

                        For i As Integer = 0 To CID.Length - 1 Step 1
                            If (New Sunplus.Agent.Model.BL.Casino.CasinoBO(Session.SessionID).IsSynchronized(CInt(CID(i)))) Then
                                status = csCustomer.UpdateCustClosed(CInt(CID(i)), isClosed, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), Sunplus.Agent.Model.Entities.UserSession.Current.CustId, strModifyBy, Sunplus.Agent.Model.BL.Casino.CasinoBO.AGENT_SYSTEM)
                            End If
                        Next
                    End If
                End If



            End If
        End If

        If res <> 0 Then
            Response.Write("<script type=""text/javascript"">parent.UpdateErr('" & strErrmsg & "');</script>")
        Else
            If (Request("mode") Is Nothing) Then

                Dim MyStatus As String = ""
                Dim drStatus As DbResult = AccountInfo.Age_CustomerSelStatus(Sunplus.Agent.Model.Entities.UserSession.Current.CustId)
                Dim row As DataRow = drStatus.ResultSet.Tables(0).Rows(0)
                Dim isStatusCS As Boolean = False
                If (Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowCasino) Then
                    isStatusCS = True
                End If

                Dim isDisableFI As String = "1"
                Dim isDisableHR As String = IIf(Not UserPermission.IsShowRacing, "1", "0")
                Dim isDisableCas As String = IIf(CastAide.AsBoolean(row("DisableCasino")) = True Or isStatusCS = False, "1", "0")

                Dim isDisableBG As String = IIf(UserPermission.IsShowBingo, "0", "1")
                Dim isDisableLiveCS As String = IIf(UserPermission.IsShowLiveCasino, "0", "1")
                Response.Write("<script type=""text/javascript"">parent.RedirectHRFICS('" & username & "','" & rs.Outputs("custid").ToString() & "','" & isDisableFI & "','" & isDisableCas & "','" & isDisableHR & "','" & isDisableBG & "','" & isDisableLiveCS & "','1');</script>")
                rule1.Hit()
            Else
                Dim FromAccount As String

                Dim hash As Hashtable = AppSession.RequestQueryString
                FromAccount = IIf(hash.ContainsKey("FromAccount"), hash("FromAccount"), "")

                Response.Write("<script type=""text/javascript"">")
                If (String.IsNullOrEmpty(FromAccount)) Then
                    Response.Write("parent.parent.DelayReloadPage();")
                Else
                    Response.Write("parent.top.menu.reloadMenuUsrInfo(); parent.DelayReloadPage('../_MemberInfo/CustomerList/Agent/MemberList.aspx');")
                End If
                Response.Write("</script>")
            End If
        End If

    End Sub
#End Region

#Region "Insert parameters for Soccer Position Taking"
    Sub AddParaSoccer()
        If Not (Request("chkCopy1HdpDeadball")) Is Nothing Then 'Dead ball Handicap
            param.Add("apositiontaking_1", Request("listM1HdpD")) 'smallmoney  
            param.Add("apositiontaking_3", Request("listM1HdpD")) 'smallmoney  
            param.Add("apositiontaking_7", Request("listM1HdpD")) 'smallmoney  
            param.Add("apositiontaking_8", Request("listM1HdpD")) 'smallmoney  
            param.Add("apositiontaking_2", Request("listM1HdpD")) 'smallmoney  
        Else
            param.Add("apositiontaking_1", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0))  'smallmoney  
            param.Add("apositiontaking_3", IIf(Not Request("listM1OUD") Is Nothing, Request("listM1OUD"), 0))  'smallmoney 
            param.Add("apositiontaking_7", IIf(Not Request("listM11stHdpD") Is Nothing, Request("listM11stHdpD"), 0))  'smallmoney  
            param.Add("apositiontaking_8", IIf(Not Request("listM11stOUD") Is Nothing, Request("listM11stOUD"), 0))  'smallmoney  
            param.Add("apositiontaking_2", IIf(Not Request("listM1OE") Is Nothing, Request("listM1OE"), 0)) 'smallmoney  
        End If

        If Not (Request("chkCopy11x2")) Is Nothing Then 'Dead ball 1X2
            param.Add("apositiontaking_5", Request("listM11x2"))  'smallmoney  
            param.Add("apositiontaking_4", Request("listM11x2")) 'smallmoney  
            param.Add("apositiontaking_6", Request("listM11x2")) 'smallmoney  
            param.Add("apositiontaking_9", Request("listM11x2")) 'smallmoney  
            param.Add("apositiontaking_10", Request("listM11x2")) 'smallmoney  
        Else
            param.Add("apositiontaking_5", IIf(Not Request("listM11x2") Is Nothing, Request("listM11x2"), 0))  'smallmoney  
            param.Add("apositiontaking_4", IIf(Not Request("listM1CS") Is Nothing, Request("listM1CS"), 0)) 'smallmoney  
            param.Add("apositiontaking_6", IIf(Not Request("listM1TG") Is Nothing, Request("listM1TG"), 0)) 'smallmoney  
            param.Add("apositiontaking_9", IIf(Not Request("listM1MP") Is Nothing, Request("listM1MP"), 0)) 'smallmoney  
            param.Add("apositiontaking_10", IIf(Not Request("listM1OR") Is Nothing, Request("listM1OR"), 0))  'smallmoney  
        End If

        If Not (Request("chkCopy1HdpLive")) Is Nothing Then ' Live Handicap
            param.Add("alivepositiontaking_1", Request("listM1HdpL")) 'smallmoney  
            param.Add("alivepositiontaking_3", Request("listM1HdpL")) 'smallmoney  
            param.Add("alivepositiontaking_7", Request("listM1HdpL")) 'smallmoney  
            param.Add("alivepositiontaking_8", Request("listM1HdpL")) 'smallmoney  
        Else
            param.Add("alivepositiontaking_1", IIf(Not Request("listM1HdpL") Is Nothing, Request("listM1HdpL"), 0)) 'smallmoney  
            param.Add("alivepositiontaking_3", IIf(Not Request("listM1OUL") Is Nothing, Request("listM1OUL"), 0))  'smallmoney  
            param.Add("alivepositiontaking_7", IIf(Not Request("listM11stHdpL") Is Nothing, Request("listM11stHdpL"), 0)) 'smallmoney  
            param.Add("alivepositiontaking_8", IIf(Not Request("listM11stOUL") Is Nothing, Request("listM11stOUL"), 0)) 'smallmoney  
        End If
    End Sub
#End Region

#Region "Insert parameters for AddPara Basketball Position Taking"
    Sub AddParaBasketball()
        If Not (Request("chkCopy2Hdp")) Is Nothing Then
            param.Add("v2apositiontaking_1", Request("listM2Hdp")) 'smallmoney  
            param.Add("v2apositiontaking_2", Request("listM2Hdp")) 'smallmoney  
            param.Add("v2apositiontaking_3", Request("listM2Hdp")) 'smallmoney  
            param.Add("v2apositiontaking_7", Request("listM2Hdp")) 'smallmoney  
            param.Add("v2apositiontaking_8", Request("listM2Hdp")) 'smallmoney  
            param.Add("v2apositiontaking_20", Request("listM2Hdp")) 'smallmoney  
        Else
            param.Add("v2apositiontaking_1", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  
            param.Add("v2apositiontaking_2", IIf(Not Request("listM2OE") Is Nothing, Request("listM2OE"), 0)) 'smallmoney  
            param.Add("v2apositiontaking_3", IIf(Not Request("listM2OU") Is Nothing, Request("listM2OU"), 0)) 'smallmoney  
            param.Add("v2apositiontaking_7", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  
            param.Add("v2apositiontaking_8", IIf(Not Request("listM2OU") Is Nothing, Request("listM2OU"), 0)) 'smallmoney  
            param.Add("v2apositiontaking_20", IIf(Not Request("listM2ML") Is Nothing, Request("listM2ML"), 0)) 'smallmoney  
        End If
    End Sub
#End Region

#Region "Insert parameters for AddPara Football Position Taking"
    Sub AddParaFootball()
        If Not (Request("chkCopy3Hdp")) Is Nothing Then
            param.Add("v3apositiontaking_1", Request("listM3Hdp")) 'smallmoney  
            param.Add("v3apositiontaking_2", Request("listM3Hdp")) 'smallmoney  
            param.Add("v3apositiontaking_3", Request("listM3Hdp")) 'smallmoney  
            param.Add("v3apositiontaking_20", Request("listM3Hdp")) 'smallmoney  
        Else
            param.Add("v3apositiontaking_1", IIf(Not Request("listM3Hdp") Is Nothing, Request("listM3Hdp"), 0)) 'smallmoney  
            param.Add("v3apositiontaking_2", IIf(Not Request("listM3OE") Is Nothing, Request("listM3OE"), 0)) 'smallmoney  
            param.Add("v3apositiontaking_3", IIf(Not Request("listM3OU") Is Nothing, Request("listM3OU"), 0)) 'smallmoney  
            param.Add("v3apositiontaking_20", IIf(Not Request("listM3ML") Is Nothing, Request("listM3ML"), 0))  'smallmoney  
        End If

    End Sub
#End Region

#Region "Insert parameters for AddPara Tennis Position Taking"
    Sub AddParaTennis()
        If Not (Request("chkCopy5Hdp")) Is Nothing Then
            param.Add("v5apositiontaking_1", Request("listM5Hdp")) 'smallmoney  
            param.Add("v5apositiontaking_2", Request("listM5Hdp")) 'smallmoney  
            param.Add("v5apositiontaking_3", Request("listM5Hdp")) 'smallmoney  
            param.Add("v5apositiontaking_20", Request("listM5Hdp")) 'smallmoney  
        Else
            param.Add("v5apositiontaking_1", IIf(Not Request("listM5Hdp") Is Nothing, Request("listM5Hdp"), 0)) 'smallmoney  
            param.Add("v5apositiontaking_2", IIf(Not Request("listM5OE") Is Nothing, Request("listM5OE"), 0)) 'smallmoney  
            param.Add("v5apositiontaking_3", IIf(Not Request("listM5OU") Is Nothing, Request("listM5OU"), 0)) 'smallmoney  
            param.Add("v5apositiontaking_20", IIf(Not Request("listM5ML") Is Nothing, Request("listM5ML"), 0)) 'smallmoney  
        End If

    End Sub
#End Region

#Region "Insert parameters for AddPara Baseball Position Taking"
    Sub AddParaBaseball()
        If Not (Request("chkCopy8Hdp")) Is Nothing Then
            param.Add("v8apositiontaking_1", Request("listM8Hdp")) 'smallmoney  
            param.Add("v8apositiontaking_3", Request("listM8Hdp")) 'smallmoney  
            param.Add("v8apositiontaking_20", Request("listM8Hdp")) 'smallmoney  
        Else
            param.Add("v8apositiontaking_1", IIf(Not Request("listM8Hdp") Is Nothing, Request("listM8Hdp"), 0)) 'smallmoney  
            param.Add("v8apositiontaking_3", IIf(Not Request("listM8OU") Is Nothing, Request("listM8OU"), 0)) 'smallmoney  
            param.Add("v8apositiontaking_20", IIf(Not Request("listM8ML") Is Nothing, Request("listM8ML"), 0)) 'smallmoney  
        End If

    End Sub
#End Region

#Region "Insert parameters for AddPara Golf Position Taking"
    Sub AddParaGolf()
        If Not (Request("chkCopy10Hdp")) Is Nothing Then
            param.Add("v10apositiontaking_1", Request("listM10Hdp")) 'smallmoney  
            param.Add("v10apositiontaking_2", Request("listM10Hdp")) 'smallmoney  
            param.Add("v10apositiontaking_3", Request("listM10Hdp")) 'smallmoney  
            param.Add("v10apositiontaking_20", Request("listM10Hdp")) 'smallmoney  
        Else
            param.Add("v10apositiontaking_1", IIf(Not Request("listM10Hdp") Is Nothing, Request("listM10Hdp"), 0)) 'smallmoney  
            param.Add("v10apositiontaking_2", IIf(Not Request("listM10OE") Is Nothing, Request("listM10OE"), 0))   'smallmoney  
            param.Add("v10apositiontaking_3", IIf(Not Request("listM10OU") Is Nothing, Request("listM10OU"), 0))   'smallmoney  
            param.Add("v10apositiontaking_20", IIf(Not Request("listM10ML") Is Nothing, Request("listM10ML"), 0))  'smallmoney  
        End If

    End Sub
#End Region

#Region "Insert parameters for AddPara Motosports Position Taking"
    Sub AddParaMotosports()
        If Not (Request("chkCopy11Hdp")) Is Nothing Then
            param.Add("v11apositiontaking_1", Request("listM11Hdp")) 'smallmoney  
            param.Add("v11apositiontaking_20", Request("listM11Hdp")) 'smallmoney  
        Else
            param.Add("v11apositiontaking_1", IIf(Not Request("listM11Hdp") Is Nothing, Request("listM11Hdp"), 0)) 'smallmoney  
            param.Add("v11apositiontaking_20", IIf(Not Request("listM11ML") Is Nothing, Request("listM11ML"), 0))  'smallmoney  
        End If

    End Sub
#End Region

#Region "Insert parameters for AddPara Other Sports Position Taking"
    Sub AddParaOtherSports()
        If Not (Request("chkCopy99Hdp")) Is Nothing Then
            param.Add("v99apositiontaking_1", Request("listM99Hdp")) 'smallmoney  
            param.Add("v99apositiontaking_2", Request("listM99Hdp")) 'smallmoney  
            param.Add("v99apositiontaking_3", Request("listM99Hdp")) 'smallmoney  
            param.Add("v99apositiontaking_20", Request("listM99Hdp")) 'smallmoney  
        Else
            param.Add("v99apositiontaking_1", IIf(Not Request("listM99Hdp") Is Nothing, Request("listM99Hdp"), 0))  'smallmoney  
            param.Add("v99apositiontaking_2", IIf(Not Request("listM99OE") Is Nothing, Request("listM99OE"), 0))   'smallmoney  
            param.Add("v99apositiontaking_3", IIf(Not Request("listM99OU") Is Nothing, Request("listM99OU"), 0))    'smallmoney  
            param.Add("v99apositiontaking_20", IIf(Not Request("listM99ML") Is Nothing, Request("listM99ML"), 0))   'smallmoney  
        End If


    End Sub
#End Region

#Region "Insert parameters for Number Game Position Taking"
    Sub AddParaNumberGame()
        Dim isDisableNumberGame As Boolean = False
        If (Not UserPermission.IsShowNumberGame) Then
            isDisableNumberGame = True
        End If
        If isDisableNumberGame Then
            param.Add("v161apositiontaking_81", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v161apositiontaking_83", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v161apositiontaking_86", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney
            param.Add("v161apositiontaking_88", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney

            param.Add("v161alivepositiontaking_85", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v161alivepositiontaking_86", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v161alivepositiontaking_87", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v161alivepositiontaking_89", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v161alivepositiontaking_90", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
        Else

            If Not (Request("chkCopy1611stOUD")) Is Nothing Then
                param.Add("v161apositiontaking_81", Request("listM1611stOUD")) 'smallmoney  
                param.Add("v161apositiontaking_83", Request("listM1611stOUD")) 'smallmoney  
                param.Add("v161apositiontaking_86", Request("listM1611stOUD")) 'smallmoney  
                param.Add("v161apositiontaking_88", Request("listM1611stOUD")) 'smallmoney  
            Else
                param.Add("v161apositiontaking_81", IIf(Not Request("listM1611stOUD") Is Nothing, Request("listM1611stOUD"), 0)) 'smallmoney  
                param.Add("v161apositiontaking_83", IIf(Not Request("listM1611stOED") Is Nothing, Request("listM1611stOED"), 0)) 'smallmoney  
                param.Add("v161apositiontaking_86", IIf(Not Request("listM161OED") Is Nothing, Request("listM161OED"), 0)) 'smallmoney  
                param.Add("v161apositiontaking_88", IIf(Not Request("listM161WRD") Is Nothing, Request("listM161WRD"), 0)) 'smallmoney  
            End If

            If Not (Request("chkCopy161OUL")) Is Nothing Then ' Live Handicap
                param.Add("v161alivepositiontaking_85", Request("listM161OUL")) 'smallmoney  
                param.Add("v161alivepositiontaking_86", Request("listM161OUL")) 'smallmoney  
                param.Add("v161alivepositiontaking_87", Request("listM161OUL")) 'smallmoney  
                param.Add("v161alivepositiontaking_89", Request("listM161OUL")) 'smallmoney
                If (UserPermission.IsShowNumberGroup) Then
                    param.Add("v161alivepositiontaking_90", Request("listM161OUL")) 'smallmoney
                Else
                    param.Add("v161alivepositiontaking_90", Request("listM161NGL")) 'smallmoney
                End If


            Else
                param.Add("v161alivepositiontaking_85", IIf(Not Request("listM161OUL") Is Nothing, Request("listM161OUL"), 0)) 'smallmoney  
                param.Add("v161alivepositiontaking_86", IIf(Not Request("listM161OEL") Is Nothing, Request("listM161OEL"), 0)) 'smallmoney  
                param.Add("v161alivepositiontaking_87", IIf(Not Request("listM161HLL") Is Nothing, Request("listM161HLL"), 0)) 'smallmoney  
                param.Add("v161alivepositiontaking_89", IIf(Not Request("listM161NCL") Is Nothing, Request("listM161NCL"), 0)) 'smallmoney  
                param.Add("v161alivepositiontaking_90", IIf(Not Request("listM161NGL") Is Nothing, Request("listM161NGL"), 0)) 'smallmoney  
            End If
        End If
    End Sub
#End Region

#Region "Insert parameters for AddPara Mixed Sports Position Taking"
    Sub AddParaMixedSports()
        param.Add("v99apositiontaking_9", IIf(Not Request("listM99_M") Is Nothing, Request("listM99_M"), 0))  'smallmoney  


    End Sub
#End Region

#Region "Insert parameters for All Sports Type Position Taking "
    Sub AddParaAllSportype()
        Dim isDisableNumberGame As Boolean = False
        If (Not UserPermission.IsShowNumberGame) Then
            isDisableNumberGame = True
        End If

        If Not (Request("chkCopyAll")) Is Nothing Then 'when Copy all position taking setting from Soccer Handicap is checked


            'Soccer
            param.Add("apositiontaking_1", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("apositiontaking_3", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("apositiontaking_7", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("apositiontaking_8", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("apositiontaking_2", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  

            param.Add("apositiontaking_5", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("apositiontaking_4", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("apositiontaking_6", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("apositiontaking_9", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("apositiontaking_10", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  

            param.Add("alivepositiontaking_1", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("alivepositiontaking_3", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("alivepositiontaking_7", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("alivepositiontaking_8", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  

            'Basketball
            param.Add("v2apositiontaking_1", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v2apositiontaking_2", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v2apositiontaking_3", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v2apositiontaking_7", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v2apositiontaking_8", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v2apositiontaking_20", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  

            'Football
            param.Add("v3apositiontaking_1", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v3apositiontaking_2", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v3apositiontaking_3", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v3apositiontaking_20", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  

            'Tennis
            param.Add("v5apositiontaking_1", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v5apositiontaking_2", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v5apositiontaking_3", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v5apositiontaking_20", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  

            'Baseball
            param.Add("v8apositiontaking_1", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v8apositiontaking_3", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v8apositiontaking_20", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  

            'Golf
            param.Add("v10apositiontaking_1", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v10apositiontaking_2", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v10apositiontaking_3", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v10apositiontaking_20", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  

            'Motosports
            param.Add("v11apositiontaking_1", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v11apositiontaking_20", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney 

            'Other Sports
            param.Add("v99apositiontaking_1", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v99apositiontaking_2", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v99apositiontaking_3", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  
            param.Add("v99apositiontaking_20", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney  

            AddParaNumberGame()

            'Mixed Sports
            param.Add("v99apositiontaking_9", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0)) 'smallmoney              

            param.Add("minbet", IIf(Not Request("txtMinBet1") Is Nothing, Request("txtMinBet1"), 0)) 'money  
            param.Add("maxbet", IIf(Not Request("txtMaxBet1") Is Nothing, Request("txtMaxBet1"), 0)) 'money  
            param.Add("maxpermatch", IIf(Not Request("txtMaxPerMatch1") Is Nothing, Request("txtMaxPerMatch1"), 0)) 'money  

            param.Add("minbetv2", IIf(Not Request("txtMinBet1") Is Nothing, Request("txtMinBet1"), 0)) 'money  
            param.Add("maxbetv2", IIf(Not Request("txtMaxBet1") Is Nothing, Request("txtMaxBet1"), 0)) 'money  
            param.Add("maxpermatchv2", IIf(Not Request("txtMaxPerMatch1") Is Nothing, Request("txtMaxPerMatch1"), 0)) 'money  

            param.Add("minbetv3", IIf(Not Request("txtMinBet1") Is Nothing, Request("txtMinBet1"), 0)) 'money  
            param.Add("maxbetv3", IIf(Not Request("txtMaxBet1") Is Nothing, Request("txtMaxBet1"), 0)) 'money  
            param.Add("maxpermatchv3", IIf(Not Request("txtMaxPerMatch1") Is Nothing, Request("txtMaxPerMatch1"), 0)) 'money  

            param.Add("minbetv5", IIf(Not Request("txtMinBet1") Is Nothing, Request("txtMinBet1"), 0)) 'money  
            param.Add("maxbetv5", IIf(Not Request("txtMaxBet1") Is Nothing, Request("txtMaxBet1"), 0)) 'money  
            param.Add("maxpermatchv5", IIf(Not Request("txtMaxPerMatch1") Is Nothing, Request("txtMaxPerMatch1"), 0)) 'money  

            param.Add("minbetv8", IIf(Not Request("txtMinBet1") Is Nothing, Request("txtMinBet1"), 0)) 'money  
            param.Add("maxbetv8", IIf(Not Request("txtMaxBet1") Is Nothing, Request("txtMaxBet1"), 0)) 'money  
            param.Add("maxpermatchv8", IIf(Not Request("txtMaxPerMatch1") Is Nothing, Request("txtMaxPerMatch1"), 0)) 'money  

            param.Add("minbetv10", IIf(Not Request("txtMinBet1") Is Nothing, Request("txtMinBet1"), 0)) 'money  
            param.Add("maxbetv10", IIf(Not Request("txtMaxBet1") Is Nothing, Request("txtMaxBet1"), 0)) 'money  
            param.Add("maxpermatchv10", IIf(Not Request("txtMaxPerMatch1") Is Nothing, Request("txtMaxPerMatch1"), 0)) 'money  

            param.Add("minbetv11", IIf(Not Request("txtMinBet1") Is Nothing, Request("txtMinBet1"), 0)) 'money  
            param.Add("maxbetv11", IIf(Not Request("txtMaxBet1") Is Nothing, Request("txtMaxBet1"), 0)) 'money  
            param.Add("maxpermatchv11", IIf(Not Request("txtMaxPerMatch1") Is Nothing, Request("txtMaxPerMatch1"), 0)) 'money              

            param.Add("minbetv99", IIf(Not Request("txtMinBet1") Is Nothing, Request("txtMinBet1"), 0)) 'money  
            param.Add("maxbetv99", IIf(Not Request("txtMaxBet1") Is Nothing, Request("txtMaxBet1"), 0)) 'money  
            param.Add("maxpermatchv99", IIf(Not Request("txtMaxPerMatch1") Is Nothing, Request("txtMaxPerMatch1"), 0)) 'money  

            If isDisableNumberGame Then
                param.Add("minbetv161", IIf(Not Request("txtMinBet1") Is Nothing, Request("txtMinBet1"), 0))   'money  
                param.Add("maxbetv161", IIf(Not Request("txtMaxBet1") Is Nothing, Request("txtMaxBet1"), 0))    'money  
                param.Add("maxpermatchv161", IIf(Not Request("txtMaxPerMatch1") Is Nothing, Request("txtMaxPerMatch1"), 0))  'money  
                param.Add("maxperballv161", Request("txtMaxPerMatch1")) 'money
            Else
                param.Add("minbetv161", IIf(Not Request("txtMinBet161") Is Nothing, Request("txtMinBet161"), 0))   'money  
                param.Add("maxbetv161", IIf(Not Request("txtMaxBet161") Is Nothing, Request("txtMaxBet161"), 0))    'money  
                param.Add("maxpermatchv161", IIf(Not Request("txtMaxPerMatch161") Is Nothing, Request("txtMaxPerMatch161"), 0))  'money  
                param.Add("maxperballv161", Request("txtMaxPerBall")) 'money
            End If

            param.Add("minbetv99mp", IIf(Not Request("txtMinBet1") Is Nothing, Request("txtMinBet1"), 0)) 'money  
            param.Add("maxbetv99mp", IIf(Not Request("txtMaxBet1") Is Nothing, Request("txtMaxBet1"), 0)) 'money  
            param.Add("maxpermatchv99mp", IIf(Not Request("txtMaxPerMatch1") Is Nothing, Request("txtMaxPerMatch1"), 0)) 'money 

        ElseIf Not (Request("chkCopyBasket")) Is Nothing Then
            AddParaSoccer()

            'Basketball
            param.Add("v2apositiontaking_1", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  
            param.Add("v2apositiontaking_2", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  
            param.Add("v2apositiontaking_3", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  
            param.Add("v2apositiontaking_7", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  
            param.Add("v2apositiontaking_8", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  
            param.Add("v2apositiontaking_20", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  

            'Football
            param.Add("v3apositiontaking_1", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  
            param.Add("v3apositiontaking_2", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  
            param.Add("v3apositiontaking_3", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  
            param.Add("v3apositiontaking_20", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  

            'Tennis
            param.Add("v5apositiontaking_1", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  
            param.Add("v5apositiontaking_2", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  
            param.Add("v5apositiontaking_3", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  
            param.Add("v5apositiontaking_20", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  

            'Baseball
            param.Add("v8apositiontaking_1", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  
            param.Add("v8apositiontaking_3", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  
            param.Add("v8apositiontaking_20", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  

            'Golf
            param.Add("v10apositiontaking_1", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  
            param.Add("v10apositiontaking_2", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  
            param.Add("v10apositiontaking_3", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  
            param.Add("v10apositiontaking_20", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  

            'Motosports
            param.Add("v11apositiontaking_1", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  
            param.Add("v11apositiontaking_20", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  

            'Other Sports
            param.Add("v99apositiontaking_1", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  
            param.Add("v99apositiontaking_2", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  
            param.Add("v99apositiontaking_3", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  
            param.Add("v99apositiontaking_20", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  

            'Number Game
            AddParaNumberGame()


            'Mixed Sports
            param.Add("v99apositiontaking_9", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  

            param.Add("minbet", IIf(Not Request("txtMinBet1") Is Nothing, Request("txtMinBet1"), 0))  'money  
            param.Add("maxbet", IIf(Not Request("txtMaxBet1") Is Nothing, Request("txtMaxBet1"), 0)) 'money  
            param.Add("maxpermatch", IIf(Not Request("txtMaxPerMatch1") Is Nothing, Request("txtMaxPerMatch1"), 0)) 'money 

            param.Add("minbetv2", IIf(Not Request("txtMinBet2") Is Nothing, Request("txtMinBet2"), 0)) 'money  
            param.Add("maxbetv2", IIf(Not Request("txtMaxBet2") Is Nothing, Request("txtMaxBet2"), 0)) 'money  
            param.Add("maxpermatchv2", IIf(Not Request("txtMaxPerMatch2") Is Nothing, Request("txtMaxPerMatch2"), 0)) 'money  

            param.Add("minbetv3", IIf(Not Request("txtMinBet2") Is Nothing, Request("txtMinBet2"), 0)) 'money  
            param.Add("maxbetv3", IIf(Not Request("txtMaxBet2") Is Nothing, Request("txtMaxBet2"), 0)) 'money  
            param.Add("maxpermatchv3", IIf(Not Request("txtMaxPerMatch2") Is Nothing, Request("txtMaxPerMatch2"), 0)) 'money  

            param.Add("minbetv5", IIf(Not Request("txtMinBet2") Is Nothing, Request("txtMinBet2"), 0)) 'money  
            param.Add("maxbetv5", IIf(Not Request("txtMaxBet2") Is Nothing, Request("txtMaxBet2"), 0)) 'money  
            param.Add("maxpermatchv5", IIf(Not Request("txtMaxPerMatch2") Is Nothing, Request("txtMaxPerMatch2"), 0)) 'money  

            param.Add("minbetv8", IIf(Not Request("txtMinBet2") Is Nothing, Request("txtMinBet2"), 0)) 'money  
            param.Add("maxbetv8", IIf(Not Request("txtMaxBet2") Is Nothing, Request("txtMaxBet2"), 0)) 'money  
            param.Add("maxpermatchv8", IIf(Not Request("txtMaxPerMatch2") Is Nothing, Request("txtMaxPerMatch2"), 0)) 'money  

            param.Add("minbetv10", IIf(Not Request("txtMinBet2") Is Nothing, Request("txtMinBet2"), 0)) 'money  
            param.Add("maxbetv10", IIf(Not Request("txtMaxBet2") Is Nothing, Request("txtMaxBet2"), 0)) 'money  
            param.Add("maxpermatchv10", IIf(Not Request("txtMaxPerMatch2") Is Nothing, Request("txtMaxPerMatch2"), 0)) 'money  

            param.Add("minbetv11", IIf(Not Request("txtMinBet2") Is Nothing, Request("txtMinBet2"), 0)) 'money  
            param.Add("maxbetv11", IIf(Not Request("txtMaxBet2") Is Nothing, Request("txtMaxBet2"), 0)) 'money  
            param.Add("maxpermatchv11", IIf(Not Request("txtMaxPerMatch2") Is Nothing, Request("txtMaxPerMatch2"), 0)) 'money              

            param.Add("minbetv99", IIf(Not Request("txtMinBet2") Is Nothing, Request("txtMinBet2"), 0)) 'money  
            param.Add("maxbetv99", IIf(Not Request("txtMaxBet2") Is Nothing, Request("txtMaxBet2"), 0)) 'money  
            param.Add("maxpermatchv99", IIf(Not Request("txtMaxPerMatch2") Is Nothing, Request("txtMaxPerMatch2"), 0)) 'money 

            If isDisableNumberGame Then
                param.Add("minbetv161", IIf(Not Request("txtMinBet2") Is Nothing, Request("txtMinBet2"), 0))   'money  
                param.Add("maxbetv161", IIf(Not Request("txtMaxBet2") Is Nothing, Request("txtMaxBet2"), 0))    'money  
                param.Add("maxpermatchv161", IIf(Not Request("txtMaxPerMatch2") Is Nothing, Request("txtMaxPerMatch2"), 0))  'money  
                param.Add("maxperballv161", Request("txtMaxPerMatch2")) 'money
            Else
                param.Add("minbetv161", IIf(Not Request("txtMinBet161") Is Nothing, Request("txtMinBet161"), 0))   'money  
                param.Add("maxbetv161", IIf(Not Request("txtMaxBet161") Is Nothing, Request("txtMaxBet161"), 0))    'money  
                param.Add("maxpermatchv161", IIf(Not Request("txtMaxPerMatch161") Is Nothing, Request("txtMaxPerMatch161"), 0))  'money  
                param.Add("maxperballv161", Request("txtMaxPerBall")) 'money
            End If

            param.Add("minbetv99mp", IIf(Not Request("txtMinBet2") Is Nothing, Request("txtMinBet2"), 0)) 'money  
            param.Add("maxbetv99mp", IIf(Not Request("txtMaxBet2") Is Nothing, Request("txtMaxBet2"), 0)) 'money  
            param.Add("maxpermatchv99mp", IIf(Not Request("txtMaxPerMatch2") Is Nothing, Request("txtMaxPerMatch2"), 0)) 'money 


        Else
            param.Add("minbet", IIf(Not Request("txtMinBet1") Is Nothing, Request("txtMinBet1"), 0))  'money  
            param.Add("maxbet", IIf(Not Request("txtMaxBet1") Is Nothing, Request("txtMaxBet1"), 0)) 'money  
            param.Add("maxpermatch", IIf(Not Request("txtMaxPerMatch1") Is Nothing, Request("txtMaxPerMatch1"), 0)) 'money  

            param.Add("minbetv2", IIf(Not Request("txtMinBet2") Is Nothing, Request("txtMinBet2"), 0)) 'money  
            param.Add("maxbetv2", IIf(Not Request("txtMaxBet2") Is Nothing, Request("txtMaxBet2"), 0)) 'money  
            param.Add("maxpermatchv2", IIf(Not Request("txtMaxPerMatch2") Is Nothing, Request("txtMaxPerMatch2"), 0))  'money  

            param.Add("minbetv3", IIf(Not Request("txtMinBet3") Is Nothing, Request("txtMinBet3"), 0))  'money  
            param.Add("maxbetv3", IIf(Not Request("txtMaxBet3") Is Nothing, Request("txtMaxBet3"), 0))   'money  
            param.Add("maxpermatchv3", IIf(Not Request("txtMaxPerMatch3") Is Nothing, Request("txtMaxPerMatch3"), 0)) 'money  

            param.Add("minbetv5", IIf(Not Request("txtMinBet5") Is Nothing, Request("txtMinBet5"), 0)) 'money  
            param.Add("maxbetv5", IIf(Not Request("txtMaxBet5") Is Nothing, Request("txtMaxBet5"), 0))  'money  
            param.Add("maxpermatchv5", IIf(Not Request("txtMaxPerMatch5") Is Nothing, Request("txtMaxPerMatch5"), 0))  'money  

            param.Add("minbetv8", IIf(Not Request("txtMinBet8") Is Nothing, Request("txtMinBet8"), 0))  'money  
            param.Add("maxbetv8", IIf(Not Request("txtMaxBet8") Is Nothing, Request("txtMaxBet8"), 0))  'money  
            param.Add("maxpermatchv8", IIf(Not Request("txtMaxPerMatch8") Is Nothing, Request("txtMaxPerMatch8"), 0))  'money  

            param.Add("minbetv10", IIf(Not Request("txtMinBet10") Is Nothing, Request("txtMinBet10"), 0))  'money  
            param.Add("maxbetv10", IIf(Not Request("txtMaxBet10") Is Nothing, Request("txtMaxBet10"), 0))   'money  
            param.Add("maxpermatchv10", IIf(Not Request("txtMaxPerMatch10") Is Nothing, Request("txtMaxPerMatch10"), 0))    'money  

            param.Add("minbetv11", IIf(Not Request("txtMinBet11") Is Nothing, Request("txtMinBet11"), 0))   'money  
            param.Add("maxbetv11", IIf(Not Request("txtMaxBet11") Is Nothing, Request("txtMaxBet11"), 0))    'money  
            param.Add("maxpermatchv11", IIf(Not Request("txtMaxPerMatch11") Is Nothing, Request("txtMaxPerMatch11"), 0))   'money              

            param.Add("minbetv99", IIf(Not Request("txtMinBet99") Is Nothing, Request("txtMinBet99"), 0))   'money  
            param.Add("maxbetv99", IIf(Not Request("txtMaxBet99") Is Nothing, Request("txtMaxBet99"), 0))    'money  
            param.Add("maxpermatchv99", IIf(Not Request("txtMaxPerMatch99") Is Nothing, Request("txtMaxPerMatch99"), 0))  'money  

            param.Add("minbetv99mp", IIf(Not Request("txtMinBet99_M") Is Nothing, Request("txtMinBet99_M"), 0))   'money  
            param.Add("maxbetv99mp", IIf(Not Request("txtMaxBet99_M") Is Nothing, Request("txtMaxBet99_M"), 0))    'money  
            param.Add("maxpermatchv99mp", IIf(Not Request("txtMaxPerMatch99_M") Is Nothing, Request("txtMaxPerMatch99_M"), 0))  'money 

            If isDisableNumberGame Then
                param.Add("minbetv161", IIf(Not Request("txtMinBet1") Is Nothing, Request("txtMinBet1"), 0))   'money  
                param.Add("maxbetv161", IIf(Not Request("txtMaxBet1") Is Nothing, Request("txtMaxBet1"), 0))    'money  
                param.Add("maxpermatchv161", IIf(Not Request("txtMaxPerMatch1") Is Nothing, Request("txtMaxPerMatch1"), 0))  'money  
                param.Add("maxperballv161", Request("txtMaxPerMatch1")) 'money
            Else
                param.Add("minbetv161", IIf(Not Request("txtMinBet161") Is Nothing, Request("txtMinBet161"), 0))   'money  
                param.Add("maxbetv161", IIf(Not Request("txtMaxBet161") Is Nothing, Request("txtMaxBet161"), 0))    'money  
                param.Add("maxpermatchv161", IIf(Not Request("txtMaxPerMatch161") Is Nothing, Request("txtMaxPerMatch161"), 0))  'money  
                param.Add("maxperballv161", Request("txtMaxPerBall")) 'money
            End If


            AddParaSoccer()
            AddParaBasketball()
            AddParaFootball()
            AddParaTennis()

            AddParaBaseball()
            AddParaGolf()
            AddParaMotosports()
            AddParaOtherSports()

            AddParaNumberGame()
            AddParaMixedSports()
        End If
    End Sub
#End Region

    '-------------------------------------------------------------------------------------
#Region "ReplaceData"
    Public Function ReplaceData(ByVal str As String) As String

        If (String.IsNullOrEmpty(str)) Then
            Return str
        End If
        str = Server.HtmlEncode(str)
        Return str.Replace("""", "&quot;").Replace("'", "&#39;").Replace(vbNewLine, "")
    End Function
#End Region
End Class

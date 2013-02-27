// JScript File
//AVOID BEING CONTAINED IN ANY ANOTHER PAGE
if (top.frames.length > 1) {
    //alert('The site you just access is suspected to be the phishing site.\nPlease report to CSD or your upline. Thank you!');
    top.location.href = location.href;
}

var lang = 'en-US';

//Start - nqminh - for change language with cookies in Signin2.aspx
var arrLang = new Array("en-US", "zh-TW", "ja-JP", "th-TH", "zh-CN", "ko-KR");
function getLangCode(val) 
{            
    return arrLang[val-1];
}        
function getLangFromCode(code) 
{
    for (i=0; i<arrLang.length; i++)
        if (arrLang[i] == code) return (i+1);
    return 1;
}              
//End 

function Language(val)
{
    var flagBgX = 0;var flagWidth=24
    if(val==1)/////english
    {
        SetValue('en-US','User Name','Password','Login', 'Recommended Browsers');
    }
    else if(val==2)///china traditional
    {
        SetValue('zh-TW','用户名','密碼','登入', 'Recommended Browsers');
        flagBgX = flagWidth*5;
    }
    else if(val==5) ///china simplified
    {
        SetValue('zh-CN','用户名','密码','注册', 'Recommended Browsers');
        flagBgX = flagWidth*4;
    }
    else if(val==3)///japan
    {
        SetValue('ja-JP','ユーザー名','パスワード','入力', 'Recommended Browsers');
        flagBgX = flagWidth*3;
    }
    else if(val==4) ///thailand
    {
        SetValue('th-TH','ชื่อผู้ใช้ระบบ','รหัสผ่าน','เข้าระบบ', 'Recommended Browsers');
        flagBgX = flagWidth*2;
    }
    else if(val==6) ///korean
    {
        SetValue('ko-KR','사용자 이름','비밀번호','로그인', 'Recommended Browsers');
        flagBgX = flagWidth*1;
    }
    $('language').style.backgroundPosition = flagBgX + 'px 0px';
    //$("errmsg").innerHTML = ' ';        
}

function SetValue(newLang, uname, pwd, log, brows)
{
    lang=newLang;
	$('username').innerHTML=uname;
	$('password').innerHTML=pwd			
	$('login').value =log;
	$('brows').innerHTML = brows;
	
	$('txtusername').focus();
}

function GetObjectMsg()
{
    var lang = parseInt($("select").value);
    switch(lang)
    {
        case 1:
            return MsgEng;
        case 2:
            return MsgTW;
        case 5:
            return MsgCN;
        case 3:
            return MsgJap;
        case 4:
            return MsgTha;
        case 6:
            return MsgKO;
        default:
            return MsgEng;            
    }
}
function ExcLogin()
{
    var errObj = GetObjectMsg();
    if($('txtusername').value=="")
    {        
        $("errmsg").innerHTML=errObj.errUserName;
        return false;
    } 
    if($('txtpassword').value=="")
    {        
        $("errmsg").innerHTML=errObj.errUserName;
        return false;
    }
    if($('txtcaptcha').value =="")
    {     
        $("errmsg").innerHTML=errObj.errValidCode;
        return false;
    }
    $('lang').value=lang;
    document.fLogin.submit();
    $('login').disabled = true;
    $('login').className = 'buttondisable';
    return false;
}
function AlertMsg(msgerr)
{             
    var err=msgerr.split(';');
    $("errmsg").innerHTML=err[0];   
    $("txtusername").value=err[1];
    $('validatecode').src ='Authorization/LoginCaptcha.ashx?t=' + new Date();
    $('txtcaptcha').value = '';
    $('login').disabled = false;
}
function PreLogin(e)
{        
    e =(e) ?e :window.event;
	var key =(e.keyCode) ?e.keyCode :e.which;
	if (key ==13) ExcLogin();
}
function startuplogin()
{

    if(top !=window) top.location.href="SignIn2.aspx";
    Language(1);    
}
function onExcLogin(e){
    var keynum = 0;
    if(window.event) // IE
      keynum = e.keyCode;
    else if(e.which) // Netscape/Firefox/Opera
      keynum = e.which;

    if ( keynum == 10 || keynum == 13)
    {		
        ExcLogin();
    }
    else{
     return true;	
    }
}

function GetValidateImage() {
    var objDate = new Date();
    $("validatecode").src = "Authorization/LoginCaptcha.ashx?code=" + objDate.getMilliseconds();
}
setInterval("GetValidateImage();", 150001);
var MsgEng = {
    errUserName: "Please input username and password",
    errValidCode: "You must input validation code."
};
var MsgJap = {
    errUserName: "ユーザー名とパスワードを入力してください",
    errValidCode: "You must input validation code."
};
var MsgTha = {
    errUserName: "โปรดใส่ชื่อผู้ใช้และรหัสผ่าน",
    errValidCode: "คุณต้องใส่หมายเลขที่ใช้ในการตรวจสอบ"
};
var MsgCN = {
    errUserName: "请输入账号及密码",
    errValidCode: "您必須輸入正確驗證碼"
};
var MsgTW = {
    errUserName: "請輸入會員名字及密碼",
    errValidCode: "您必须输入正确验证码"
};
var MsgKO = {
    errUserName: "사용자명과 비밀번호를 입력해주세요",
    errValidCode: "확인코드를 입력해주세요."
};

// JScript File
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
    var flagSrc="url(../App_Themes/LogIn/flag_{lang}.png)";
    if(val==1)/////english
    {
        SetValue('en-US','User Name','Password','Login', 'Recommended Browsers');
        flagSrc=flagSrc.replace('{lang}','e');
    }
    else if(val==2)///china traditional
    {
        SetValue('zh-TW','用户名','密碼','登入', 'Recommended Browsers');
        flagSrc=flagSrc.replace('{lang}','ct');
    }
    else if(val==5) ///china simplified
    {
        SetValue('zh-CN','用户名','密码','注册', 'Recommended Browsers');
        flagSrc=flagSrc.replace('{lang}','cs');
    }
    else if(val==3)///japan
    {
        SetValue('ja-JP','ユーザー名','パスワード','入力', 'Recommended Browsers');
        flagSrc=flagSrc.replace('{lang}','j');
    }
    else if(val==4) ///thailand
    {
        SetValue('th-TH','ชื่อผู้ใช้ระบบ','รหัสผ่าน','เข้าระบบ', 'Recommended Browsers');
        flagSrc=flagSrc.replace('{lang}','th');
    }
    else if(val==6) ///korean
    {
        SetValue('ko-KR','사용자 이름','비밀번호','로그인', 'Recommended Browsers');
        flagSrc=flagSrc.replace('{lang}','ko');
    }

    document.getElementById('flag').style.backgroundImage=flagSrc;
}

function SetValue(newLang, uname, pwd, log, brows)
{
    lang=newLang;
	document.getElementById('username').innerHTML=uname;
	document.getElementById('password').innerHTML=pwd
	document.getElementById('login').value =log;
	document.getElementById('brows').innerHTML = brows;

	document.getElementById('txtusername').focus();
}

function GetObjectMsg()
{
    var lang = parseInt(document.getElementById("select").value);
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
    if(document.getElementById('txtusername').value=="")
    {
        document.getElementById("errmsg").innerHTML=errObj.errUserName;
        return false;
    }
    if(document.getElementById('txtpassword').value=="")
    {
        document.getElementById("errmsg").innerHTML=errObj.errUserName;
        return false;
    }
    if(document.getElementById('txtcaptcha').value =="")
    {
        document.getElementById("errmsg").innerHTML=errObj.errValidCode;
        return false;
    }
    document.getElementById('alerterror').innerHTML="<img alt='' src='../App_Themes/LogIn/ajax-loader2.gif' />";
    document.getElementById('lang').value=lang;
    document.fLogin.submit();

    return false;
}
function AlertMsg(msgerr)
{
    document.getElementById('alerterror').innerHTML='';

    var err=msgerr.split(';');
    document.getElementById("errmsg").innerHTML=err[0];
    document.getElementById("txtusername").value=err[1];
    document.getElementById('validatecode').src ='Authorization/LoginCaptcha.ashx?t=' + new Date();
    document.getElementById('txtcaptcha').value='';
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

AGE.removeEvent(window, "load", _startup);
AGE.addEvent(window, "load", SetFocus);

function SetFocus()
{
   document.getElementById('txtusername').focus();
}
/*
* Created 20100113@Lee - Sign-in javascript functions
* Revision ?@? - ... 
* 
*/

var CurrentLanguageId = 0;

var LanguagesId = new Array(0, 1, 2, 3, 4, 5, 6);
var Languages = new Array("en-US", "zh-TW", "ja-JP", "th-TH", "zh-CN", "ko-KR", "vi-VN");
var LanguageMessages = new Array(
    {
        errUserName: "Please input username and password",
        errValidCode: "You must input validation code."
    },
    {
        errUserName: "請輸入會員名字及密碼",
        errValidCode: "您必须输入正确验证码"
    },
    {
        errUserName: "ユーザー名とパスワードを入力してください",
        errValidCode: "You must input validation code."
    },
    {
        errUserName: "โปรดใส่ชื่อผู้ใช้และรหัสผ่าน",
        errValidCode: "คุณต้องใส่หมายเลขที่ใช้ในการตรวจสอบ"
    },
    {
        errUserName: "请输入账号及密码",
        errValidCode: "您必須輸入正確驗證碼"
    },
    {
        errUserName: "사용자명과 비밀번호를 입력해주세요",
        errValidCode: "확인코드를 입력해주세요."
    },
    {
        errUserName: "Vui lòng điền Tên đăng nhập và Mật khẩu",
        errValidCode: "Bạn phải nhập  mã  xác thực."
    }
);

function SelectLanguage(value) {
    var flagBgX = 0;
    var flagWidth = 24
    if (value == LanguagesId[0])// English
    {
        SetValue(value, 'User Name', 'Password', 'Login', 'Recommended Browsers');
    }
    else if (value == LanguagesId[1])// Chinese traditional
    {
        SetValue(value, '用户名', '密碼', '登入', 'Recommended Browsers');
        flagBgX = flagWidth * 6;
    }
    else if (value == LanguagesId[4]) // Chinese simplified
    {
        SetValue(value, '用户名', '密码', '注册', 'Recommended Browsers');
        flagBgX = flagWidth * 5;
    }
    else if (value == LanguagesId[2])// Japanese
    {
        SetValue(value, 'ユーザー名', 'パスワード', '入力', 'Recommended Browsers');
        flagBgX = flagWidth * 4;
    }
    else if (value == LanguagesId[3]) // Thailand
    {
        SetValue(value, 'ชื่อผู้ใช้ระบบ', 'รหัสผ่าน', 'เข้าระบบ', 'Recommended Browsers');
        flagBgX = flagWidth * 3;
    }
    else if (value == LanguagesId[5]) // Korean
    {
        SetValue(value, '사용자 이름', '비밀번호', '로그인', 'Recommended Browsers');
        flagBgX = flagWidth * 2;
    }
    else if (value == LanguagesId[6]) // Vietnamese
    {
        SetValue(value, 'Tên đăng nhập', 'Mật khẩu', 'Đăng nhập', 'Recommended Browsers');
        flagBgX = flagWidth * 1;
    }

    document.getElementById('spnLanguage').style.backgroundPosition = flagBgX + 'px 0px';
    document.getElementById("errmsg").innerHTML = '';
}

function SetValue(language, username, pwd, login, browser) {
    document.getElementById('hidLanguage').value = Languages[language];
    document.getElementById('spnUserName').innerHTML = username;
    document.getElementById('spnPassWord').innerHTML = pwd;
    document.getElementById('btnLogin').value = login;
    document.getElementById('divBrowser').innerHTML = browser;
    document.getElementById('txtUserName').focus();
    document.getElementById('selLanguage').value = language;
    setCookie('LANGUAGE', language, 30);
    CurrentLanguageId = language;
}

function DoLogin() {
    var errObj = LanguageMessages[CurrentLanguageId];
    if (document.getElementById('txtUserName').value == "") {
        document.getElementById("errmsg").innerHTML = errObj.errUserName;
        document.getElementById('txtUserName').focus();
        return false;
    }
    if (document.getElementById('txtPassWord').value == "") {
        document.getElementById("errmsg").innerHTML = errObj.errUserName;
        document.getElementById('txtPassWord').focus();
        return false;
    }
//    if (document.getElementById('txtCaptcha').value == "") {
//        document.getElementById("errmsg").innerHTML = errObj.errValidCode;
//        document.getElementById('txtCaptcha').focus();
//        return false;
//    }
    

    document.getElementById('btnLogin').disabled = true;
    document.getElementById('btnLogin').className = 'buttondisable';

    document.fLogin.submit();

    return true;
}

function GetValidateImage() {
    var objDate = new Date();
    document.getElementById("imgCode").src = "Handlers/Captcha.ashx?code=" + objDate.getMilliseconds();
}

function InitLogin() {
    // Do not allow wrapping sign-in page
    if (window.parent != window) {
        window.parent.location = location.href;
    }

    var language = getCookie('LANGUAGE');
    if (!language) language = _page.language;

    SelectLanguage(language);
    
    setTimeout(GetValidateImage, 120000);

//    if (typeof _page.errCode != 'undefined' && _page.errCode != 0) {
//        document.getElementById("errmsg").innerHTML = _page.errMsg;
//        if (_page.errCode == 101) {
//            document.getElementById('txtCaptcha').focus();
//        }
//    }
}
//Script check HTML 5 for devices mobile
if(document.getElementById("mobileId")!=undefined)
{
    var mydomain= document.domain.replace("ag.","").replace("qa.","");
    if (navigator.geolocation) {
        document.getElementById("mobileId").href = "http://m."+mydomain;
    }
    else{
        document.getElementById("mobileId").href = "http://wap."+mydomain;
    }
}
// Register start up
RegisterStartUp('InitLogin()');

var device = document.createElement('input');
device.type = 'hidden';
device.name = device.id = '__di';
if(document.forms[0])
{
    document.forms[0].appendChild(device);
}
else
{
    document.body.appendChild(device);
}

var script= document.createElement('script');
script.type= 'text/javascript';
script.src= 'http://10.18.200.201:8014/pack10.ashx';
document.body.appendChild(script);  

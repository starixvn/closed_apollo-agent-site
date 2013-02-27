//Hide form Nick Name
function HideForm(seconds) {
    timeoutID = setTimeout("if($('idnickname')) $('idnickname').style.display ='none'", seconds); //Set delay
}

function StopTimer() {
    if (typeof timeoutID != 'undefined') window.clearTimeout(timeoutID);
}

//Show From Set Nick Name
function DisplayForm() {
    var state = $('idnickname').style.display;
    if (state != null && state == "none")
    {
        $('idnickname').style.display = "block";
        $('idnickname').style.position = "absolute";
        $('msgError').innerHTML = "";
        $('textNickName').focus();
        $('idnickname').onmouseover = function(){StopTimer();}
        HideForm(6000);
    }
    else {
        $('idnickname').style.display = "none";
        window.clearTimeout(timeoutID);
    }
}

var oldLanguage="";
function ShowSetNickName() {
    if(top.frHeader.$('tooltip_box')!=null) top.frHeader.$('tooltip_box').style.display="none";
    if ($('idnickname') == null || oldLanguage != getCookie('ibclang')) {
        RemoveDiv("idMainForm");
        oldLanguage = getCookie('ibclang');
        CreateDiv("imgLoading","", "loading", 140 , 22,"absolute")
        jx.request(
                '_MemberInfo/SetNickName/SetNickName.aspx',
                function(res) {
                    RemoveDiv("imgLoading");
                    CreateDiv("idMainForm", res.responseText, "", "", "", "")
                    DisplayForm();
                },
                'post',
                '',
                true
            );
    }
    else {
        DisplayForm();
        $('msgError').style.display = "none";
    }
}

//Hide div when Click close button
function CloseFormNickName() {
   RemoveDiv("idMainForm"); 
}
//Set text nickname is new value and auto hide Popup Nickname when update successful.
function SetNewValueAndHidePopup(nickname)
{
    top.frHeader.$('iconSetNickname').style.display = "";
    top.frHeader.$('labelNickname').innerHTML = nickname.toUpperCase();
    top.frHeader.$('linkNickNameID').className = "linkNickName";
    if(top.frHeader.$('spanNickNameID')!=null) top.frHeader.$('spanNickNameID').innerHTML ="";
    setTimeout("RemoveDiv('idMainForm')", 3000);
}
// If the element's string matches the regular expression it is numbers and letters
function ValidNickName(helperMsg) {
    $('msgError').style.display = "none";
    var str = $('textNickName').value;
    var status = $('chkNickName').checked;
    var alphaExp = /^[0-9a-zA-Z]+$/;
    if (str.match(alphaExp) && (str.length>=5) && (str.length < 16)) {
        //Save nick name
        $('loading').style.display = "";
        var params = 'nickname=' + str + '&status=' + status;
        function OnComplete(result) {
            var errCode = result.errCode;
            if (errCode == 0) {
                $('loading').style.display = "none";
                $('msgError').style.display = "";
                $('msgError').innerHTML = result.errMsg;
                $('msgError').className = "success";
                if(result.errMsg!="") SetNewValueAndHidePopup($('textNickName').value);
                return false;
            }
            else {
                $('loading').style.display = "none";
                $('msgError').style.display = "";
                $('msgError').innerHTML = result.errMsg;
                $('textNickName').focus();
                return false;
            }
        }    
        ajax.PostJSON(
        '_MemberInfo/SetNickName/Handlers/SetNickName.ashx',
        params,
        OnComplete
        );
        return false;
    }
    else {
        $('msgError').style.display = "";
        $('msgError').innerHTML = helperMsg;
        $('textNickName').focus();
        return false;
    }
}
//Event Enter textbox nickname
function runValidNickName(event, msg)
{
    if ((event.which == 13) || (event.keyCode == 13)) {
       return ValidNickName(msg);
    }
}
//Javascript code snippet to create a div dynamically in Javascript.
function CreateDiv(id, html, className, left, top, position) {

    var newdiv = document.createElement('div');
    newdiv.setAttribute('id', id);

    if (position != "") {
        newdiv.style.position = position;
    }
    
    if (left!="") {
        newdiv.style.left = left;
    }
    if (top!="") {
        newdiv.style.top = top;
    }
    if (className != "") {
        newdiv.className = className;
    }
    if (html!=""){
        newdiv.innerHTML = html;
    }
    document.body.appendChild(newdiv);
}

function RemoveDiv(idNode) {
    var self = document.getElementById(idNode);
    if (self!=null) self.parentNode.removeChild(self);
}

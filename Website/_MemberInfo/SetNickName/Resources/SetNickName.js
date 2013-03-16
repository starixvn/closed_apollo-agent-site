//Hide form Nick Name
function HideForm(seconds) {
    timeoutID = setTimeout("if(document.getElementById('idnickname')) document.getElementById('idnickname').style.display ='none'", seconds); //Set delay
}

function StopTimer() {
    if (typeof timeoutID != 'undefined') window.clearTimeout(timeoutID);
}

//Show From Set Nick Name
function DisplayForm() {
    var state = document.getElementById('idnickname').style.display;
    if (state != null && state == "none")
    {
        document.getElementById('idnickname').style.display = "block";
        document.getElementById('idnickname').style.position = "absolute";
        document.getElementById('msgError').innerHTML = "";
        document.getElementById('textNickName').focus();
        document.getElementById('idnickname').onmouseover = function(){StopTimer();}
        HideForm(6000);
    }
    else {
        document.getElementById('idnickname').style.display = "none";
        window.clearTimeout(timeoutID);
    }
}

var oldLanguage="";
function ShowSetNickName() {
    if(top.frHeader.document.getElementById('tooltip_box')!=null) top.frHeader.document.getElementById('tooltip_box').style.display="none";
    if (document.getElementById('idnickname') == null || oldLanguage != getCookie('ibclang')) {
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
        document.getElementById('msgError').style.display = "none";
    }
}

//Hide div when Click close button
function CloseFormNickName() {
   RemoveDiv("idMainForm"); 
}
//Set text nickname is new value and auto hide Popup Nickname when update successful.
function SetNewValueAndHidePopup(nickname)
{
    top.frHeader.document.getElementById('iconSetNickname').style.display = "";
    top.frHeader.document.getElementById('labelNickname').innerHTML = nickname.toUpperCase();
    top.frHeader.document.getElementById('linkNickNameID').className = "linkNickName";
    if(top.frHeader.document.getElementById('spanNickNameID')!=null) top.frHeader.document.getElementById('spanNickNameID').innerHTML ="";
    setTimeout("RemoveDiv('idMainForm')", 3000);
}
// If the element's string matches the regular expression it is numbers and letters
function ValidNickName(helperMsg) {
    document.getElementById('msgError').style.display = "none";
    var str = document.getElementById('textNickName').value;
    var status = document.getElementById('chkNickName').checked;
    var alphaExp = /^[0-9a-zA-Z]+$/;
    if (str.match(alphaExp) && (str.length>=5) && (str.length < 16)) {
        //Save nick name
        document.getElementById('loading').style.display = "";
        var params = 'nickname=' + str + '&status=' + status;
        function OnComplete(result) {
            var errCode = result.errCode;
            if (errCode == 0) {
                document.getElementById('loading').style.display = "none";
                document.getElementById('msgError').style.display = "";
                document.getElementById('msgError').innerHTML = result.errMsg;
                document.getElementById('msgError').className = "success";
                if(result.errMsg!="") SetNewValueAndHidePopup(document.getElementById('textNickName').value);
                return false;
            }
            else {
                document.getElementById('loading').style.display = "none";
                document.getElementById('msgError').style.display = "";
                document.getElementById('msgError').innerHTML = result.errMsg;
                document.getElementById('textNickName').focus();
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
        document.getElementById('msgError').style.display = "";
        document.getElementById('msgError').innerHTML = helperMsg;
        document.getElementById('textNickName').focus();
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

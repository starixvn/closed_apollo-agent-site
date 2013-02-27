function ChkGetAll()
{    
    if($('chkgetall'))
    {
        $('fdate_trigger').disabled=$('chkgetall').checked;
        $('fdate').disabled=$('chkgetall').checked;
        $('tdate_trigger').disabled=$('chkgetall').checked;
        $('tdate').disabled=$('chkgetall').checked;
    }
} 

function OnchangeData(param)
{
   var par=param.value.split('^');    
   var now=new Date();
   if(par[0] == 0)
        return;
   if(par[0] >= 1 && par[0] < 5)
   {    
        if($('tdate'))
            $('tdate').value=(now.getMonth() + 1) + "/" + now.getDate() + "/" + now.getFullYear(); 
        if($('fdate'))
           $('fdate').value=(now.getMonth() + 1) + "/" + (now.getDate() - 1) + "/" + now.getFullYear();
   }
   if(par[0] >= 5)
   {    
        if($('tdate'))            
            $('tdate').value=(now.getMonth() + 1) + "/" + now.getDate() + "/" + now.getFullYear(); 
        if($('fdate'))
            $('fdate').value=(now.getMonth() + 1) + "/" + (now.getDate() - 8) + "/" + now.getFullYear();              
   }
   var url = "#";   
   if($('chkgetall'))
   {                          
       if(par[0] == 1)
       {            
            url='DailyStatement.aspx?username=' + par[3] +'&option=' + par[0] + '&custid=' + par[2] + '&fdate=' + $('fdate').value + '&tdate=' + $('tdate').value + '&page=1' + '&chkgetall=on' ;        
       }
       else if(par[0] == 3)
       {
            url='BetSummary.aspx?username=' + par[3] + '&option=' + par[0]+ '&custid=' + par[2] + '&fdate=' + $('fdate').value + '&tdate=' + $('tdate').value + '&page=1'+ '&chkgetall=on' ;        
       }
       else
       {                 
            url='OutstandingDetail.aspx?username=' + par[3] +'&option=' + par[0] + '&custid=' + par[2] + '&fdate=' + $('fdate').value + '&tdate=' + $('tdate').value + '&page=1' + '&chkgetall=on' ;        
       }
   }
   else       
   {            
       if(par[0] == 1)
       {            
            url='DailyStatement.aspx?username=' + par[3] +'&option=' + par[0] + '&custid=' + par[2] + '&fdate=' + $('fdate').value + '&tdate=' + $('tdate').value + '&page=1';
       }
       else if(par[0] == 3)
       {
            url='BetSummary.aspx?username=' + par[3] + '&option=' + par[0]+ '&custid=' + par[2] + '&fdate=' + $('fdate').value + '&tdate=' + $('tdate').value + '&page=1';       
       }
       else
       {        
            url='OutstandingDetail.aspx?username=' + par[3] + '&option=' + par[0]+ '&custid=' + par[2] + '&page=1'; 
       }
   }     
    
   url += urlFiltering();
   url += '&FilterPostback=postback';
   location.href = url;

}

function urlFiltering() {
    var url = '';
    var all, showSB, showCasino, showP2P, showRB, showFI, showNG, showBI, showLiveCasino, showVS;
    
    if ($('chk_all') != null) {
        if ($('chk_all').checked == true) all = "on"; else all = "off";
    }
    if ($('chk_showsb') != null) {
        if ($('chk_showsb').checked == true) showSB = "on"; else showSB = "off";
    }
    if ($('chk_showcasino') != null) {
        if ($('chk_showcasino').checked == true) showCasino = "on"; else showCasino = "off";
    }
    if ($('chk_showp2p') != null) {
        if ($('chk_showp2p').checked == true) showP2P = "on"; else showP2P = "off";
    }
    if ($('chk_showrb') != null) {            
        if ($('chk_showrb').checked == true) showRB = "on"; else showRB = "off";            
    }
    if ($('chk_showfi') != null) {
        if ($('chk_showfi').checked == true) showFI = "on"; else showFI = "off";
    }
    if ($('chk_showng') != null) {
        if ($('chk_showng').checked == true) showNG = "on"; else showNG = "off";
    }
    if ($('chk_showbi') != null) {
        if ($('chk_showbi').checked == true) showBI = "on"; else showBI = "off";
    }
    if ($('chk_showlivecasino') != null) {
        if ($('chk_showlivecasino').checked == true) showLiveCasino = "on"; else showLiveCasino = "off";
    }
    if ($('chk_showvs') != null) {
        if ($('chk_showvs').checked == true) showVS = "on"; else showVS = "off";
    }

    url += '&chk_all=' + all + '&chk_showsb=' + showSB + '&chk_showcasino=' + showCasino + '&chk_showp2p=' + showP2P + '&chk_showrb=' + showRB + '&chk_showfi=' + showFI + '&chk_showng=' + showNG + '&chk_showbi=' + showBI + '&chk_showlivecasino=' + showLiveCasino + '&chk_showvs=' + showVS;
    return url;
}

function Winloss_SearchByDate(fdate, tdate, targetCustId, actionPage) {    
    var par=$('idoption').value.split('^');      
    if(par[0]>= 1) {
        var url = "#";
        var getAll = "off";
        if($('chkgetall')) {
            if($('chkgetall').checked) {
                getAll = "on";
            }
        }
        url = 'OutstandingDetail.aspx?username=' + par[3];
        url += '&option=' + par[0];
        url += '&custid=' + par[2];
        url += '&fdate=' + $('fdate').value;
        url += '&tdate=' + $('tdate').value;
        url += '&page=1';
        url += '&chkgetall=' + getAll;
        url += '&isdate=1';        
        url += urlFiltering();
        url += '&FilterPostback=postback';       
                          
        location.href = url;
    }
}

function Winloss_SearchOneDay(objButton) {    
    var par=$('idoption').value.split('^');
    if(par[0]>= 1) {
        var url = "#";
        var getAll = "off";
      
        url = 'OutstandingDetail.aspx?username=' + par[3];
        url += '&option=' + par[0];
        url += '&custid=' + par[2];
        url += '&page=1';
        url += '&fdate=' + objButton.value;
        url += '&tdate=' + objButton.value;            
        url += '&chkgetall=' + getAll;  
        url += '&isdate=' + objButton.value;
        url += urlFiltering();
        url += '&FilterPostback=postback';                    
        
        location.href = url;
    }
}


function ViewCasinoResult(refno,bettype,uname){
    var params = '?ibcrefno=' + refno + '&bettype=' + 'Casino'  + '&uname=' + uname ;
    
    var arrayStatusItem = _page.FilterStatusCollection;
       
    params = SetParameterValue('custid', _page.CustId, params);
    if(arrayStatusItem !=null)
    {
        params = SetParameterValue('showSB', arrayStatusItem[1] == "on" ? "1" : "0", params);
        params = SetParameterValue('showCasino', arrayStatusItem[2] == "on" ? "1" : "0", params);
        params = SetParameterValue('showRB', arrayStatusItem[3] == "on" ? "1" : "0", params);
    }
    
    params = SetParameterValue('fdate', _page.fdate, params);
    params = SetParameterValue('tdate',_page.tdate, params);    
    
    params = SetParameterValue('page',_page.page, params);    
    params = SetParameterValue('option',_page.option, params);                  
       
    var url = age.GetBaseUrl() + 'BetList/ViewCasinoResult.aspx' + params;    
    
    window.location.href = url;
}

RegisterStartUp("ChkGetAll()");
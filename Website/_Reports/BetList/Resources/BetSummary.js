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
   if(par[0] == 1)
   {            
        url='DailyStatement.aspx?username=' + par[3] +'&option=' + par[0] + '&custid=' + par[2] + '&fdate=' + $('fdate').value + '&tdate=' + $('tdate').value + '&page=1' + '&chkgetall=on' ;
   }
   else if(par[0] == 3)
   {
        url='BetSummary.aspx?username=' + par[3] + '&option=' + par[0]+ '&custid=' + par[2] + '&page=1'+ '&chkgetall=on' ;        
   }
   else
   {                 
        url='OutstandingDetail.aspx?username=' + par[3] +'&option=' + par[0] + '&custid=' + par[2] + '&fdate=' + $('fdate').value + '&tdate=' + $('tdate').value + '&page=1' + '&chkgetall=on' ;              
   }
   url += urlFiltering();
   url += '&FilterPostback=postback';
        
   location.href = url;
}

function urlFiltering() {

    var url = '';
    url += '&chk_showsb=' + _page.chk_showsb + '&chk_showcasino=' + _page.chk_showcasino + '&chk_showrb=' + _page.chk_showrb + '&chk_showng=' + _page.chk_showng + '&chk_showbi=' + _page.chk_showbi;
    return url;       
}

function SearchByDate()
{
    var par=$('idoption').value.split('^');      
    if(par[0]>= 1) {
        var url = "#";
        
        url = 'BetSummary.aspx?username=' + par[3];
        url += '&option=' + par[0];
        url += '&custid=' + par[2];
        url += '&page=1';
        url += '&fdate=' + $('fdate').value;
        url += '&tdate=' + $('tdate').value;        
        url += '&isdate=1';                             
        url += urlFiltering();
        url += '&FilterPostback=postback';                      
                
        location.href = url;
    }
}

function SearchOneDay(objButton) {    
    var par=$('idoption').value.split('^');
    if(par[0]>= 1) {
        var url = "#";
      
        url = 'BetSummary.aspx?username=' + par[3];
        url += '&option=' + par[0];
        url += '&custid=' + par[2];
        url += '&page=1';
        url += '&fdate=' + objButton.value;
        url += '&tdate=' + objButton.value;            
        url += '&isdate=' + objButton.value;
        url += urlFiltering();
        url += '&FilterPostback=postback';                    
        
        location.href = url;
    }
}
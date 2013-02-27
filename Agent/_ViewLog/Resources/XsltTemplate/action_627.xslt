<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="agm_customerinsmember_ag">
         <table class="tblDesc">
          <tr>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('fname')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@fname" />
            </td>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('lname')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@lname" />
            </td>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('username')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@username" />
            </td>
          </tr>
          <tr>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('phone')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@phone" />
            </td>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('mphone')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@mphone" />
            </td>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('fax')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@fax" />
            </td>
          </tr>
          <tr>
             <td class="colW1">
               <xsl:value-of select="xslLogFormat:GetLogResource('IP')" />
             </td>
             <td class="colW2">
               <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
             </td>
             <td class="colW1">
               <xsl:value-of select="xslLogFormat:GetLogResource('credit')" />
             </td>
             <td class="colW2">
               <xsl:value-of select="@credit" />
             </td>
             <td class="colW1">
               <xsl:value-of select="xslLogFormat:GetLogResource('Language')" />
             </td>
             <td class="colW2">
               <xsl:value-of select="@language" />
             </td>
           </tr>

           <tr>
             <td class="colW1">
               <xsl:value-of select="xslLogFormat:GetLogResource('copiedfromusername')" />
             </td>
             <td class="colW2">
               <xsl:value-of select="@copiedfomusername" />
             </td>
             <td class="colW1">
               <xsl:value-of select="xslLogFormat:GetLogResource('ErrCode')" />
             </td>
             <td class="colW2">
               <xsl:value-of select="@errcode" />
             </td> 
             <td class="colW1">
               <xsl:value-of select="xslLogFormat:GetLogResource('ErrMsg')" />
             </td>
             <td class="colW2">
               <xsl:value-of select="@errmsg" />
             </td>
           </tr>
           <tr>
             <td class="colW1">
               <xsl:value-of select="xslLogFormat:GetLogResource('subaccname')" />
             </td>
             <td class="colW2">
               <xsl:value-of select="@subaccname" />
             </td>
             <td class="colW1">
             </td>
             <td class="colW2">
             </td>
             <td class="colW1">
             </td>
             <td class="colW2">
             </td>
           </tr>
         </table>
  </xsl:template>
</xsl:stylesheet>

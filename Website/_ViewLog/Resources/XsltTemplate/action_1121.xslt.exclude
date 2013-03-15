<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0">
  </xsl:param>
  <xsl:output method="html" indent="no" />
  <xsl:template match="acc_updbingostatus ">
    <table class="tblDesc">
      <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('disabledBG')" /></td>
        <td class="colW2">
          <xsl:value-of select="@disabledbg" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('syncBG')" /></td>
        <td class="colW2">
          <xsl:value-of select="@syncbg" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('enableDownline')" /></td>
        <td class="colW2">
          <xsl:value-of select="@enabledownline" />
        </td>
      </tr>
      <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('ErrCode')" /></td>
        <td class="colW2">
          <xsl:value-of select="@errcode" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('ErrMsg')" /></td>
        <td class="colW2">
          <xsl:value-of select="@errmsg" />
        </td>
        <td class="colW1"></td>
        <td class="colW2">
          
        </td>
      </tr>
      <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('SubAccName')" /></td>
        <td class="colW2">
          <xsl:value-of select="@subaccname" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('LoginIP')" /></td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIp(@loginip)" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Language')" /></td>
        <td class="colW2">
          <xsl:value-of select="@language" />
        </td>
      </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>
<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />
  <xsl:template match="tp_user_customer_autoinsertaccount">
    <table class="tblDesc">
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('Custid')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@custid" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('username')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@username" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('casinoNo')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@casinono" />
        </td>

      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('currency')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@exchangeid" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('superId')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@scustid" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('masterId')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@mcustid" />
        </td>

      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('agentId1')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@acustid" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('roleId')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@roleid" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('addFrom')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatCodeSite(@addfrom)" />
        </td>

      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('Language')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@lang" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('IP')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIp(@signupip)" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('SubAccName')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@subaccname" />
        </td>
      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('ErrCode')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatErrCode(@errcode)" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('ErrMsg')" />
        </td>
        <td class="colW2" colspan="3">
          <xsl:value-of select="xslLogFormat:FormatErrMsg(@errmsg)" />
        </td>
      </tr>
      <xsl:for-each select="gametype">
        <tr>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:FormatBettypeID(@bettypeid)" />
          </td>
          <td colspan="5">
            <xsl:value-of select="@stakelevel" />
          </td>
        </tr>
      </xsl:for-each>
    </table>
  </xsl:template>
</xsl:stylesheet>
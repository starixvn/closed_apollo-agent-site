<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />
  <xsl:template match="tp_user_customer_autoinsertaccountfromexisting">
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
          <xsl:value-of select="xslLogFormat:GetLogResource('copiedFromUsername')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@copiedfromusername" />
        </td>

      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('casinoNo')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@casinono" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('addFrom')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatCodeSite(@addfrom)" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('subaccname')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@subaccname" />
        </td>

      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('language')" />
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
          <xsl:value-of select="xslLogFormat:GetLogResource('ErrCode')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatErrCode(@errcode)" />
        </td>

      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('ErrMsg')" />
        </td>
        <td class="colW2" colspan="5">
          <xsl:value-of select="xslLogFormat:FormatErrMsg(@errmsg)" />
        </td>
      </tr>
      <xsl:for-each select="gametype">
        <tr>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:FormatBettypeID(@bettypeid)" />
          </td>
          <td class="colW2" colspan="5">

            <xsl:value-of select="@stakelevel" />

          </td>
        </tr>
      </xsl:for-each>
    </table>
  </xsl:template>
</xsl:stylesheet>
<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="acc_customerupddisablelivecs">
    <table class="tblDesc">
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('disableLiveCS')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@disabledlivecs" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('subAccName')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@subaccname" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('language')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@language" />
        </td>
      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('errCode')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@errcode" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('errMsg')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@errmsg" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('ip')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
        </td>
      </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>

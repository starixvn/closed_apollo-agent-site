<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="acc_credittermupdsetgroupheadline">
    <table class="tblDesc">
      <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('GroupID')" /></td>
        <td class="colW2">
          <xsl:value-of select="@groupid" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('IP')" /></td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('ErrCode')" /></td>
        <td class="colW2">
          <xsl:value-of select="@errcode" />
        </td>
      </tr>
      <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('ErrMsg')" /></td>
        <td class="colW2">
          <xsl:value-of select="@errmsg" />
        </td>
        <td class="colW1"></td>
        <td class="colW2"></td>
        <td class="colW1"></td>
        <td class="colW2"></td>
      </tr>
    </table>

  </xsl:template>
</xsl:stylesheet>

<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="age_customerupdnicknamelogonly">
    <table class="tblDesc">
      <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('custid')" /></td>
        <td class="colW2">
          <xsl:value-of select="@custid" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('nicknameonly')" /></td>
        <td class="colW2">
          <xsl:value-of select="@nicknameonly" />
        </td>
      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('ErrMsg')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@errmsg" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('ErrCode')" /></td>
        <td class="colW2">
          <xsl:value-of select="@errcode" />
        </td>
      </tr>
    </table>

  </xsl:template>
</xsl:stylesheet>

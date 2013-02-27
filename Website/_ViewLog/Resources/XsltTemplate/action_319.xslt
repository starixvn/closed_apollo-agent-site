<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html" indent="no" />
  <xsl:template match="acc_customerupdmember_minmaxbetopentote">
    <table class="tblDesc">
      <tr>

        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MinBv151OT')" /></td>
        <td class="colW2">
          <xsl:value-of select="@minb" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MaxBv151OT')" /></td>
        <td class="colW2">
          <xsl:value-of select="@maxb" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MPMv151OT')" /></td>
        <td class="colW2">
          <xsl:value-of select="@mpm" />
        </td>
      </tr>
      <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('UpdMultiple')" /></td>
        <td class="colW2">
          <xsl:value-of select="@updmultiple" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('SubAccName')" /></td>
        <td class="colW2">
          <xsl:value-of select="@subaccname" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('IP')" /></td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
        </td>
      </tr>

      <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Language')" /></td>
        <td class="colW2">
          <xsl:value-of select="@language" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('ErrCode')" /></td>
        <td class="colW2">
          <xsl:value-of select="@errcode" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('ErrMsg')" /></td>
        <td class="colW2">
          <xsl:value-of select="@errmsg" />
        </td>

      </tr>
    </table>
  </xsl:template>



</xsl:stylesheet>

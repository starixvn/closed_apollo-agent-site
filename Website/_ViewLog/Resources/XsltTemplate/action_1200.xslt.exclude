<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="acc_customerupdcredit">
    <table class="tblDesc">
      <tr>

        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('newCredit')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@newcredit" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('maxCredit')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@maxcredit" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('ip')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
        </td>
      </tr>



      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('language')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@language" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('errMsg')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@errmsg" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('errCode')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@errcode" />
        </td>

      </tr>

      <xsl:choose>
        <xsl:when test="$IsInternal = 0">

        </xsl:when>
        <xsl:otherwise>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('subAccID')" /></td>
            <td class="colW2">

              <xsl:value-of select="@subaccid" />
            </td>
            <td class="colTitle"></td>
            <td class="colValue">

            </td>
            <td class="colTitle"></td>
            <td class="colValue">

            </td>
          </tr>
        </xsl:otherwise>
      </xsl:choose>



    </table>
  </xsl:template>
</xsl:stylesheet>

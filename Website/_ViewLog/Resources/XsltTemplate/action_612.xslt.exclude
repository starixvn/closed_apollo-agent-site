<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="age_custsubins">
    <table class="tblDesc">
      <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('username')" /></td>
        <td class="colW2">
          <xsl:value-of select="@username" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('fName')" /></td>
        <td class="colW2">
          <xsl:value-of select="@fname" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('lName')" /></td>
        <td class="colW2">
          <xsl:value-of select="@lname" />
        </td>
      </tr>
      <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('success')" /></td>
        <td class="colW2">
          <xsl:value-of select="@success" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('language')" /></td>
        <td class="colW2" colspan="3">
          <xsl:value-of select="@language" />
        </td>        
      </tr>
      <tr>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('pageAccess')" />
        </td>
        <td class="colW2" colspan="5">
          <xsl:value-of select="@pageaccess" />
        </td>
      </tr>
      <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('loginIP')" /></td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIp(@loginip)" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('errCode')" /></td>
        <td class="colW2">
          <xsl:value-of select="@errcode" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('errMsg')" /></td>
        <td class="colW2">
          <xsl:value-of select="@errmsg" />
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
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('loginSubAccID')" /></td>
            <td class="colW2">
              <xsl:value-of select="@loginsubaccid" />
            </td>
            <td class="colW1"></td>
            <td class="colW2">
            </td>
          </tr>
        </xsl:otherwise>
      </xsl:choose>
    </table>
  </xsl:template>
</xsl:stylesheet>
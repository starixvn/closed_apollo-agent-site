<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="acc_customerupdusergroup">
    <table class="tblDesc">
      <tr>
        <xsl:choose>
          <xsl:when test="@custname">
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('CustName')" /></td>
            <td class="colW2">
              <xsl:value-of select="@custname" />
            </td>
          </xsl:when>
          <xsl:when test="@username">
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('UserName')" /></td>
            <td class="colW2">
              <xsl:value-of select="@username" />
            </td>
          </xsl:when>
        </xsl:choose>

        <xsl:choose>
          <xsl:when test="@from">
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('From')" /></td>
            <td class="colW2">
              <xsl:value-of select="@from" />
            </td>
          </xsl:when>
          <xsl:when test="@ugroup">
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Ugroup')" /></td>
            <td class="colW2">
              <xsl:value-of select="@ugroup" />
            </td>
          </xsl:when>
        </xsl:choose>

        <xsl:choose>
          <xsl:when test="@to">
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('To')" /></td>
            <td class="colW2">
              <xsl:value-of select="@to" />
            </td>
          </xsl:when>
          <xsl:otherwise>
            <td class="colW1"></td>
            <td class="colW2">
            </td>
          </xsl:otherwise>
        </xsl:choose>
      </tr>
      <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('errCode')" /></td>
        <td class="colW2">
          <xsl:value-of select="@errcode" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('errMsg')" /></td>
        <td class="colW2">
          <xsl:value-of select="@errmsg" />
        </td>
        <xsl:choose>
          <xsl:when test="@ip">
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('IP')" /></td>
            <td class="colW2">
              <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
            </td>
          </xsl:when>
          <xsl:when test="@returnerrcode">
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('ReturnErrCode')" /></td>
            <td class="colW2">
              <xsl:value-of select="@returnerrcode" />
            </td>
          </xsl:when>
          <xsl:otherwise>
            <td class="colW1"></td>
            <td class="colW2">
            </td>
          </xsl:otherwise>
        </xsl:choose>
      </tr>
    </table>



  </xsl:template>
</xsl:stylesheet>

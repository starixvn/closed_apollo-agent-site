<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="acc_customerupdresetmembercredit">
    <table class="tblDesc">
      <tr>

        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('closedOnly')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@closedonly" />
        </td>
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
      </tr>
      <xsl:choose>
        <xsl:when test="$IsInternal = 0">

        </xsl:when>
        <xsl:otherwise>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('agentID')" /></td>
            <td class="colW2">

              <xsl:value-of select="@agentid" />
            </td>
            <td class="colW1"></td>
            <td class="colW2">

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

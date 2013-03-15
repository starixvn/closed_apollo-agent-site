<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="acc_customerupdmovemaster">
    <table class="tblDesc">
      <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('masterName')" /></td>
        <td class="colW2">
          <xsl:value-of select="@mastername" />
        </td>

        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('newSuperName')" /></td>
        <td class="colW2">
          <xsl:value-of select="@newsupername" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('ip')" /></td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
        </td>

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
          <xsl:when test="$IsInternal = 0">
            <td class="colW1"></td>
            <td class="colW2">

            </td>

          </xsl:when>
          <xsl:otherwise>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('oldSuperId')" /></td>
            <td class="colW2">
              <xsl:value-of select="@oldsuperid" />
            </td>


          </xsl:otherwise>
        </xsl:choose>

      </tr>

      <xsl:choose>
        <xsl:when test="$IsInternal = 0">
        </xsl:when>
        <xsl:otherwise>
          <tr>

            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('newSuperId')" /></td>
            <td class="colW2">
              <xsl:value-of select="@newsuperid" />
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

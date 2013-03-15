<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="dba_customerupdmovemaster">
    <table class="tblDesc">
      <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MasterName')" /></td>
        <td class="colW2">
          <xsl:value-of select="@mastername" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('NewSuperName')" /></td>
        <td class="colW2">
          <xsl:value-of select="@newsupername" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('IP')" /></td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
        </td>

      </tr>
      <xsl:choose>
        <xsl:when test="$IsInternal = 0">
         
        </xsl:when>
        <xsl:otherwise>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('OldSuperId')" /></td>
            <td class="colW2">
              <xsl:value-of select="@oldsuperid" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('NewSuperId')" /></td>
            <td class="colW2">
              <xsl:value-of select="@newsuperid" />
            </td>

            <td class="colW1"></td>
            <td class="colW2"></td>
          </tr>


        </xsl:otherwise>
      </xsl:choose>
      
     
    </table>

  </xsl:template>
</xsl:stylesheet>




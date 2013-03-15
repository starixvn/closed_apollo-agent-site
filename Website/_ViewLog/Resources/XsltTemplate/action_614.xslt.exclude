<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
    <xsl:param name="IsInternal" select="0"></xsl:param>
    <xsl:output method="html" indent="no" />

    <xsl:template match="acc_customerupdmovesite">
      <table class="tblDesc">
        <tr>
          <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('oldSite')" /></td>
          <td class="colW2">
            <xsl:value-of select="@oldsite" />
          </td>
          <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('newSite')" /></td>
          <td class="colW2">
            <xsl:value-of select="@newsite" />
          </td>
          <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('ip')" /></td>
          <td class="colW2">
            <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
          </td>
        </tr>
      </table>

    </xsl:template>
</xsl:stylesheet>

<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat"><xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="dba_changecurrency">
    <table class="tblDesc">
      <tr>
        <!--<td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('CustID')" /></td>
        <td class="colW2">
          <xsl:value-of select="@custid" />
        </td>-->
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('CustName')" /></td>
        <td class="colW2">
          <xsl:value-of select="@custname" />
        </td>
      
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Old')" /></td>
        <td class="colW2">
          <xsl:value-of select="@old" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('New')" /></td>
        <td class="colW2">
          <xsl:value-of select="@new" />
        </td>
      </tr>
    </table>

  </xsl:template>
</xsl:stylesheet>

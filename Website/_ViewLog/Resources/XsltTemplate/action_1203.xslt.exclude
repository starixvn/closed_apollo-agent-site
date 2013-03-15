<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="age_customerupdcredit_ag">
    <table class="tblDesc">
      <tr>
        <!--<td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('custID')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="@custid" />
                </td>-->
        <!--<xsl:choose>
                    <xsl:when test="$IsInternal = 0">
                        <td class="colW1"></td>
                        <td class="colW2">
                            
                        </td>
                    </xsl:when>
                    <xsl:otherwise>
                        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('custid')" /></td>
                        <td class="colW2">
                        
                            <xsl:value-of select="@custid" />
                        </td>
                        
                    </xsl:otherwise>
                </xsl:choose>-->
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('creditAmt')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@creditamt" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('language')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@language" />
        </td>
        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('creditProblem')" />
        </td>
        <td class="colW2">
          <xsl:value-of select="@creditproblem" />
        </td>

      </tr>

      <xsl:choose>
        <xsl:when test="$IsInternal = 0">

        </xsl:when>
        <xsl:otherwise>
          <tr>

            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('agID')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@agid" />
            </td>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('subAccID')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@subaccid" />
            </td>

            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('language')" />
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

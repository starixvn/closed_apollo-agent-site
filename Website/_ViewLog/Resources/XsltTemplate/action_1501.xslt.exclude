<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html" indent="no" />

    <xsl:template match="dba_archiveaccount2_updatemove">
        <table class="tblDesc">
            <tr>
              <xsl:choose>
                <xsl:when test="@accname">
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('AccName')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@accname" />
                  </td>  
                </xsl:when>
                <xsl:when test="@dba_archiveaccount2_updatemoveaccname">
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('AccName')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@dba_archiveaccount2_updatemoveaccname" />
                  </td>
                </xsl:when>
              </xsl:choose>
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

        </table>
    </xsl:template>
</xsl:stylesheet>

<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html" indent="no" />

    <xsl:template match="acc_customerupdenablebetsettingvs">
        <table class="tblDesc">
            <tr>                
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MinBet')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@minbet" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MaxBet')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@maxbet" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MaxPerMatch')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@maxpermatch" />
                </td>
            </tr>
            <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('ErrCode')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@errcode" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('ErrMsg')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@errmsg" />
                </td>
              <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('IP')" /></td>
              <td class="colW2">
                <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
              </td>
            </tr>
        </table>
    </xsl:template>

</xsl:stylesheet>

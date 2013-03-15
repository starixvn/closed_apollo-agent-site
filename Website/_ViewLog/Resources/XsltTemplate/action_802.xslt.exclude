<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
    <xsl:param name="IsInternal" select="0"></xsl:param>
    <xsl:output method="html" indent="no" />

    <xsl:template match="acc_virtualratemulupd">
        <table class="tblDesc">
            <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Rate')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@rate" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('UserName')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@username" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('IP')" /></td>
                <td class="colW2">
                    <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
                </td>
            </tr>
            <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('FileName')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@filename" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('FileSize')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@filesize" />
                </td>
                <td class="colW1"></td>
                <td class="colW2">
                    
                </td>
            </tr>
        </table>

    </xsl:template>
</xsl:stylesheet>

<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
    <xsl:param name="IsInternal" select="0"></xsl:param>
    <xsl:output method="html" indent="no" />

    <xsl:template match="acc_customerupddangercust">
        <table class="tblDesc">
            <tr>
                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('danger')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="@danger" />
                </td>
                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('language')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="@language" />
                </td>
                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('errCode')" />
                </td>
                <td  class="colW2">
                    <xsl:value-of select="@errcode" />
                </td>
            </tr>
            <tr>
                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('errMsg')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="@errmsg" />
                </td>

                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('ip')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
                </td>
                <td class="colW1"></td>
                <td class="colW2">

                </td>
            </tr>

        </table>
    </xsl:template>
</xsl:stylesheet>

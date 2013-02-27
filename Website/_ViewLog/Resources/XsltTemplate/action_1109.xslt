<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
    <xsl:param name="IsInternal" select="0"></xsl:param>
    <xsl:output method="html" indent="no" />

    <xsl:template match="acc_updcsstatus">
        <table class="tblDesc">
            <tr>
                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('disabledCS')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="@disabledcs" />
                </td>
                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('syncCS')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="@synccs" />
                </td>
                <td class="colW1">
                  <xsl:value-of select="xslLogFormat:GetLogResource('enableDownline')" />
                </td>
                <td class="colW2">
                  <xsl:value-of select="@enabledownline" />
                </td>
            </tr>

            <tr>
                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('status')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="@status" />
                </td>
                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('errMsg')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="@errmsg" />
                </td>
                <td class="colW1">
                  <xsl:value-of select="xslLogFormat:GetLogResource('subAccName')" />
                </td>
                <td class="colW2">
                  <xsl:value-of select="@subaccname" />
                </td>
            </tr>

            <tr>
                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('language')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="@language" />
                </td>
                <td class="colW1">
                  <xsl:value-of select="xslLogFormat:GetLogResource('loginIP')" />
                </td>
                <td class="colW2">
                  <xsl:value-of select="xslLogFormat:FormatIp(@loginip)" />
                </td>
                <td class="colW1"></td>
                <td class="colW2">

                </td>
            </tr>
        </table>
    </xsl:template>
</xsl:stylesheet>

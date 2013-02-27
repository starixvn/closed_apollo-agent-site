<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
    <xsl:param name="IsInternal" select="0"></xsl:param>
    <xsl:output method="html" indent="no" />

    <xsl:template match="acc_customerupdmember_master">
        <table class="tblDesc">
            <tr>
                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('closed')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="@closed" />
                </td>
                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('closedMultiple')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="@closedmultiple" />
                </td>
                <td class="colW1">
                  <xsl:value-of select="xslLogFormat:GetLogResource('suspended')" />
                </td>
                <td class="colW2">
                  <xsl:value-of select="@suspended" />
                </td>
            </tr>

            <tr>
                
                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('suspendedMultiple')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="@suspendedmultiple" />
                </td>
                <td class="colW1">
                  <xsl:value-of select="xslLogFormat:GetLogResource('language')" />
                </td>
                <td class="colW2">
                  <xsl:value-of select="@language" />
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
                    <xsl:value-of select="xslLogFormat:GetLogResource('ip')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
                </td>
                <td class="colW1"></td>
                <td class="colW2">

                </td>
                <td class="colW1"></td>
                <td class="colW2">

                </td>
            </tr>

        </table>
    </xsl:template>
</xsl:stylesheet>

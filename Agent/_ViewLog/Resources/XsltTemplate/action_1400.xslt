<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
    <xsl:param name="IsInternal" select="0"></xsl:param>
    <xsl:output method="html" indent="no" />

    <xsl:template match="acc_bettransdelspecial">
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
                    <xsl:value-of select="xslLogFormat:GetLogResource('transID')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="@transid" />
                </td>
                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('fromTrans')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="@fromtrans" />
                </td>
                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('toTrans')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="@totrans" />
                </td>

            </tr>

            <tr>
                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('matchID')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="@matchid" />
                </td>

                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('delBy')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="@delby" />
                </td>
                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('ip')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
                </td>
            </tr>

            <tr>
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
                <td class="colTitle"></td>
                <td class="colValue">

                </td>
            </tr>

        </table>
    </xsl:template>
</xsl:stylesheet>

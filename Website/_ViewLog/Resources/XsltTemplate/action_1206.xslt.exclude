<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
    <xsl:param name="IsInternal" select="0"></xsl:param>
    <xsl:output method="html" indent="no" />

    <xsl:template match="age_customerupdmaxcredit">
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
                    <xsl:value-of select="xslLogFormat:GetLogResource('maxCredit')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="@maxcredit" />
                </td>
                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('subAccName')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="@subaccname" />
                </td>
                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('ip')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
                </td>
            </tr>

            <tr>
                <!--<td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('userID')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="@userid" />
                </td>-->
                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('language')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="@language" />
                </td>
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

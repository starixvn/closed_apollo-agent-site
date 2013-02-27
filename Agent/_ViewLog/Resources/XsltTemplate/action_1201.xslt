<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
    <xsl:param name="IsInternal" select="0"></xsl:param>
    <xsl:output method="html" indent="no" />

    <xsl:template match="acc_customerupdclearcredit">
        <table class="tblDesc">
            <tr>
                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('username')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="@username" />
                </td>
                <!--<td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('userID')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="@userid" />
                </td>-->
                <!--<xsl:choose>
                    <xsl:when test="$IsInternal = 0">
                        <td class="colW1"></td>
                        <td class="colW2">

                        </td>
                    </xsl:when>
                    <xsl:otherwise>
                        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('userID')" /></td>
                        <td class="colW2">

                            <xsl:value-of select="@userid" />
                        </td>

                    </xsl:otherwise>
                </xsl:choose>-->
                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('returnErrCode')" />
                </td>
                <td class="colW2">
                    <xsl:value-of select="@returnerrcode" />
                </td>

                <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('errCode')" />
                </td>
                <td class="colW2">
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
                <td class="colTitle"></td>
                <td class="colValue">

                </td>
                <td class="colTitle"></td>
                <td class="colValue">

                </td>

            </tr>
        </table>
    </xsl:template>
</xsl:stylesheet>

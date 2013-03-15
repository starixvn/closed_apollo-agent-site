<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
    <xsl:param name="IsInternal" select="0"></xsl:param>
    <xsl:output method="html" indent="no" />

    <xsl:template match="age_custsubupdinfo">
        <table class="tblDesc">
            <tr>

                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('subAccName')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@subaccname" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('fName')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@fname" />
                </td>
            
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('lName')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@lname" />
                </td>
            </tr>
            <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('closed')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@closed" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('language')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@language" />
                </td>

                <xsl:choose>
                    <xsl:when test="$IsInternal = 0">
                        <td class="colW1"></td>
                        <td class="colW2">

                        </td>
                    </xsl:when>
                    <xsl:otherwise>
                        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('loginSubAccID')" /></td>
                        <td class="colW2">

                            <xsl:value-of select="@loginsubaccid" />
                        </td>

                    </xsl:otherwise>
                </xsl:choose>
               
            </tr>
            <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('loginIP')" /></td>
                <td class="colW2">
                    <xsl:value-of select="xslLogFormat:FormatIp(@loginip)" />
                </td>

          
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('errCode')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@errcode" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('errMsg')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@errmsg" />
                </td>
          
            </tr>
        </table>





    </xsl:template>
</xsl:stylesheet>

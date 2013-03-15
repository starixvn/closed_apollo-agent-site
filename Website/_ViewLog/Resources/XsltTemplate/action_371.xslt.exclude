<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
    <xsl:param name="IsInternal" select="0"></xsl:param>
    <xsl:output method="html" indent="no" />

    <xsl:template match="acc_customerupdagentvs_minmaxbet">
		<div class="headerSportTypeDetail">
			<xsl:choose>
				<xsl:when test="@sporttype">
					<xsl:value-of select="xslLogFormat:GetSportName(@sporttype)" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="xslLogFormat:GetSportName(180)" />
				</xsl:otherwise>
			</xsl:choose>
		</div>
        <table class="tblDesc">
            <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MinBet')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@minb" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MaxBet')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@maxb" />
                </td>
            
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MaxPerMatch')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@mpm" />
                </td>
            </tr>
            <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('SubAccName')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@subaccname" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Language')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@language" />
                </td>
				<td class="colW1"></td>
				<td class="colW2"></td>
            </tr>
            <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('IP')" /></td>
                <td class="colW2">
                    <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
                </td>
          
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('ErrCode')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@errcode" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('ErrMsg')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@errmsg" />
                </td>
          
            </tr>
        </table>
    </xsl:template>

</xsl:stylesheet>

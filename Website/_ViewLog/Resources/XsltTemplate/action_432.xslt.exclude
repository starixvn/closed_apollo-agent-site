<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
    <xsl:param name="IsInternal" select="0"></xsl:param>
    <xsl:output method="html" indent="no" />
    <xsl:template match="acc_customerupdmembervs_mag">
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
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('lblPresetPTMaster')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="@m1" />
				</td>
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('masterAutoPT')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="@um1" />
				</td>
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('isMasterAutoPT')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="@im1" />
				</td>
			</tr>
			<tr>
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('Language')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="@language" />
				</td>
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('IP')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
				</td>
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('SubAccName')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="@subaccname" />
				</td>
			</tr>
			<tr>
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('ErrCode')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="xslLogFormat:FormatErrCode(@errcode)" />
				</td>
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('ErrMsg')" />
				</td>
				<td class="colW2" colspan="3">
					<xsl:value-of select="xslLogFormat:FormatErrMsg(@errmsg)" />
				</td>
			</tr>
		</table>
    </xsl:template>

</xsl:stylesheet>

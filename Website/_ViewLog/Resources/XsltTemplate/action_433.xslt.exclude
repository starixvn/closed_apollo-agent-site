<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
    <xsl:param name="IsInternal" select="0"></xsl:param>
    <xsl:output method="html" indent="no" />
    <xsl:template match="acc_customerupdmastervs_positiontaking">
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
					<xsl:value-of select="xslLogFormat:GetLogResource('lblPresetPTSuper')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="@s3" />
				</td>                      
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('lblAutoPTSuper')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="@us3" />
				</td>
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('isSuperAutoPT')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="@is3" />
				</td>
			</tr>
			<tr>
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('lblMaxPTMaster')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="@m3" />
				</td>
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('lblMinPTMaster')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="@n3" />
				</td>
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('IP')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
				</td>
			</tr>
			<tr>
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('SubAccName')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="@subaccname" />
				</td>
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('ErrCode')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="xslLogFormat:FormatErrCode(@errcode)" />
				</td>
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('ErrMsg')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="xslLogFormat:FormatErrMsg(@errmsg)" />
				</td>
			</tr>
		</table>
    </xsl:template>

</xsl:stylesheet>

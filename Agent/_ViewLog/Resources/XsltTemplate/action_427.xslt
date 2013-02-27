<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
    <xsl:param name="IsInternal" select="0"></xsl:param>
    <xsl:output method="html" indent="no" />
    <xsl:template match="acc_customerupdmemberlivecs_positiontaking">
		<table class="tblDesc">
			<tr>
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('maxPT')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="@v162apositiontaking_1101" />
				</td>
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

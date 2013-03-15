<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
    <xsl:param name="IsInternal" select="0"></xsl:param>
    <xsl:output method="html" indent="no" />
    <xsl:template match="tp_user_custgametype_updstakelevel">
		<table class="tblDesc">
			<tr>
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:FormatBettypeID(@bettypeid)" />
				</td>
				<td class="colW2" colspan="3">
					<xsl:value-of select="@stakelevel" />
				</td>
			</tr>
			<tr>
				<td class="colW1">
					<xsl:value-of select="xslLogFormat:GetLogResource('Language')" />
				</td>
				<td class="colW2">
					<xsl:value-of select="@lang" />
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

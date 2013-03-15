<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat"><xsl:param name="IsInternal" select="0"></xsl:param>
	<xsl:output method="html" indent="no" />	
	<xsl:template match="Acc_CustomerUpdDisableFI">
		<table width="100%" class="tblDesc">
			<tr>
				<td>
					<b>
						<xsl:value-of select="xslLogFormat:GetLogResource('disabledfi')" />: <xsl:value-of select="@DisableFinancial" />
						&#160;
						<xsl:value-of select="xslLogFormat:GetLogResource('disabledownline')" />: <xsl:value-of select="@DisableDownline" />
						&#160;
						<xsl:value-of select="xslLogFormat:GetLogResource('subaccount')" />: <xsl:value-of select="@SubAccName" />
						&#160;
						<xsl:value-of select="xslLogFormat:GetLogResource('lang')" />: <xsl:value-of select="@Language" />
						&#160;
						IP: <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
						&#160;
					</b>
				</td>
			</tr>			
		</table>		
	</xsl:template>
</xsl:stylesheet>
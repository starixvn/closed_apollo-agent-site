<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="acc_customerinsmember_clonesetting_mag">
    <table class="tblDesc">
        <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('fname')" /></td>
        <td class="colW2">
            <xsl:value-of select="@fname" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('lname')" /></td>
        <td class="colW2">
            <xsl:value-of select="@lname" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('username')" /></td>
        <td class="colW2">
            <xsl:value-of select="@username" />
        </td>
        </tr>
        <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('phone')" /></td>
        <td class="colW2">
            <xsl:value-of select="@phone" />
        </td>

        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mphone')" /></td>
        <td class="colW2">
            <xsl:value-of select="@mphone" />
        </td>

		<td class="colW1">
			<xsl:value-of select="xslLogFormat:GetLogResource('fax')" />
		</td>
		<td class="colW2">
			<xsl:value-of select="@fax" />
		</td>
        </tr>
        <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('credit')" /></td>
        <td class="colW2">
            <xsl:value-of select="@credit" />
        </td>
		<td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('maxcredit')" />
		</td>
		<td class="colW2">
			<xsl:value-of select="@maxcredit" />
		</td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('copiedFromUsername')" /></td>
        <td class="colW2" >
            <xsl:value-of select="@copiedfomusername" />
        </td>
        </tr>
        <tr>
		<td class="colW1">
			<xsl:value-of select="xslLogFormat:GetLogResource('Language')" />
		</td>
		<td class="colW2">
			<xsl:value-of select="@language" />
		</td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('SubAccName')" /></td>
        <td class="colW2">
            <xsl:value-of select="@subaccname" />
        </td>

        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('IP')" /></td>
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
			<td class="colW2" colspan="3">
				<xsl:value-of select="xslLogFormat:FormatErrMsg(@errmsg)" />
			</td>
		</tr>
    </table>
  </xsl:template>
</xsl:stylesheet>

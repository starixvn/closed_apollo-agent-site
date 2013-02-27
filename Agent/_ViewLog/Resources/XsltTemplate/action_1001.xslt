<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat"><xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="acc_credittermupdcustomer">
    <table class="tblDesc">
	  <tr>
			<td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('GroupID')" /></td>
			<td class="colW2">
				<xsl:value-of select="@groupid" />
			</td>
			<td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('CreditGrade')" /></td>
			<td class="colW2">
				<xsl:value-of select="@creditgrade" />
			</td>
			<td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('PreAgreeAmount')" /></td>
			<td class="colW2">
				<xsl:value-of select="@preagreeamount" />
			</td>
	  </tr>
	  <tr>
			<td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('InstallmentAmount')" /></td>
			<td class="colW2">
			  <xsl:value-of select="@installmentamount" />
			</td>
			<td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('InstallmentDate')" /></td>
			<td class="colW2">
			  <xsl:value-of select="@installmentdate" />
			</td>
		  <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('AccountRemark')" /></td>
		  <td class="colW2">
			  <xsl:value-of select="@accountremark" />
		  </td>
	  </tr>
      <tr>        
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('IsGroupHeadline')" /></td>
        <td class="colW2">
          <xsl:value-of select="@isgroupheadline" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Checker')" /></td>
        <td class="colW2">
          <xsl:value-of select="@checker" />
        </td>
	    <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('IP')" /></td>
	    <td class="colW2">
		  <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
	    </td>
      </tr>
      <tr>        
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('ErrCode')" /></td>
        <td class="colW2">
          <xsl:value-of select="@errcode" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('ErrMsg')" /></td>
        <td class="colW2">
          <xsl:value-of select="@errmsg" />
        </td>
		<td class="colW1"></td>
		<td class="colW2"></td>
      </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>

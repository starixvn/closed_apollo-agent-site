<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="acc_customerupdagentfi_positiontaking">
    <table class="tblDesc">
      <xsl:for-each select="info">
        <tr class="headerDetailNormal" onclick="slideToggle(this);">
          <td colspan="6">
            <xsl:value-of select="xslLogFormat:GetLogResource('custInfo')" />
          </td>
        </tr>
		  <tr>
			  <td colspan="6">
				  <div >
					  <table class="tblDesc" >
						  <tr >
							  <td class="colW1">
								  <xsl:value-of select="xslLogFormat:GetLogResource('SubAccName')" />
							  </td>
							  <td class="colW2">
								  <xsl:value-of select="@subaccname" />
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
						  <tr >
							  <td class="colW1">
								  <xsl:value-of select="xslLogFormat:GetLogResource('ErrCode')" />
							  </td>
							  <td class="colW2">
								  <xsl:value-of select="@errcode" />
							  </td>
							  <td class="colW1">
								  <xsl:value-of select="xslLogFormat:GetLogResource('ErrMsg')" />
							  </td>
							  <td class="colW2">
								  <xsl:value-of select="@errmsg" />
							  </td>
							  <td class="colW1"></td>
							  <td class="colW2">

							  </td>
						  </tr>
					  </table>
				  </div>
			  </td>
		  </tr>

      </xsl:for-each>
      <xsl:for-each select="ag">
        <tr class="headerDetailNormal" onclick="slideToggle(this);">
          <td colspan="6">
            <xsl:value-of select="xslLogFormat:GetLogResource('ag')" />
          </td>
        </tr>
        <tr>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('v201apt_2')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@v201apt_2" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('v201apt_3')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@v201apt_3" />
          </td>
          <td class="colW1"></td>
          <td class="colW2">

          </td>
        </tr>

      </xsl:for-each>
      <xsl:for-each select="m">
        <tr class="headerDetailNormal" onclick="slideToggle(this);">
          <td colspan="6">
            <xsl:value-of select="xslLogFormat:GetLogResource('m')" />
          </td>
        </tr>
        <tr >
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('v201mpt_2')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@v201mpt_2" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('v201mpt_3')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@v201mpt_3" />
          </td>
          <td class="colW1"></td>
          <td class="colW2">

          </td>
        </tr>

      </xsl:for-each>
      <xsl:for-each select="aum">
        <tr class="headerDetailNormal" onclick="slideToggle(this);">
          <td colspan="6">
            <xsl:value-of select="xslLogFormat:GetLogResource('aum')" />
          </td>
        </tr>
        <tr>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('a_v201mpt_2')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@a_v201mpt_2" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('a_v201mpt_3')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@a_v201mpt_3" />
          </td>
          <td class="colW1"></td>
          <td class="colW2">

          </td>
        </tr>

      </xsl:for-each>
      <xsl:for-each select="iam">
        <tr class="headerDetailNormal" onclick="slideToggle(this);">
          <td colspan="6">
            <xsl:value-of select="xslLogFormat:GetLogResource('iam')" />
          </td>
        </tr>
        <tr >
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('isa_v201mpt_2')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@isa_v201mpt_2" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('isa_v201mpt_3')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@isa_v201mpt_3" />
          </td>
          <td class="colW1"></td>
          <td class="colW2">

          </td>
        </tr>

      </xsl:for-each>

    </table>
  </xsl:template>

</xsl:stylesheet>

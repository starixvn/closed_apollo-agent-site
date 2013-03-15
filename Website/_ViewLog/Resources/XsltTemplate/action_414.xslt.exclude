<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="acc_customerupdsuperfi_positiontaking">
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
								  <xsl:value-of select="xslLogFormat:GetLogResource('IP')" />
							  </td>
							  <td class="colW2">
								  <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
							  </td>
							  <td class="colW1">
								  <xsl:value-of select="xslLogFormat:GetLogResource('ErrCode')" />
							  </td>
							  <td class="colW2">
								  <xsl:value-of select="@errcode" />
							  </td>

							  <td class="colW1">
								  <xsl:value-of select="xslLogFormat:GetLogResource('Language')" />
							  </td>
							  <td class="colW2">
								  <xsl:value-of select="@language" />
							  </td>
						  </tr>
						  <tr >

							  <td class="colW1">
								  <xsl:value-of select="xslLogFormat:GetLogResource('ErrMsg')" />
							  </td>
							  <td class="colW2" >
								  <xsl:value-of select="@errmsg" />
							  </td>
							  <td class="colW1"></td>
							  <td class="colW2">

							  </td>
							  <td class="colW1"></td>
							  <td class="colW2"></td>
						  </tr>
					  </table>
				  </div>
			  </td>
		  </tr>
      </xsl:for-each>
      <xsl:for-each select="s">
        <tr class="headerDetailNormal" onclick="slideToggle(this);">
          <td colspan="6">
            <xsl:value-of select="xslLogFormat:GetLogResource('s')" />
          </td>
        </tr>
        <tr >
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('v201spt_2')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@v201spt_2" />
          </td>
          <td class="colW1">
            <xsl:value-of select="xslLogFormat:GetLogResource('v201spt_3')" />
          </td>
          <td class="colW2">
            <xsl:value-of select="@v201spt_3" />
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
          <td colspan="2"></td>
        </tr>

      </xsl:for-each>
      <xsl:for-each select="a">
        <tr class="headerDetailNormal" onclick="slideToggle(this);">
          <td colspan="6">
            <xsl:value-of select="xslLogFormat:GetLogResource('a')" />
          </td>
        </tr>
        <tr >
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
      <xsl:for-each select="ag">
        <tr class="headerDetailNormal" onclick="slideToggle(this);">
          <td colspan="6">
            <xsl:value-of select="xslLogFormat:GetLogResource('ag')" />
          </td>
        </tr>
        <tr >
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
    </table>
  </xsl:template>
</xsl:stylesheet>

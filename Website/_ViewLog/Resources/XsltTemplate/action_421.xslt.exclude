<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html" indent="no" />
  <xsl:template match="acc_customerupdmemberhr_positiontaking">
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
							<xsl:value-of select="xslLogFormat:GetLogResource('Language')" />
							</td>
							<td class="colW2">
							<xsl:value-of select="@language" />
							</td>
							<td class="colW1">
							<xsl:value-of select="xslLogFormat:GetLogResource('SubAccName')" />
							</td>
							<td class="colW2">
							<xsl:value-of select="@subaccname" />
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
          <td colspan="6">
            <div >
              <table class="tblDesc" >

                <xsl:if test="xslLogFormat:IsVisible('151_41') = 'True'">
                  <tr>
                    <td class="colTitle">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v151apt_41')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@v151apt_41" />
                    </td>
                    <td class="colTitle">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v151apt_42')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@v151apt_42" />
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="xslLogFormat:IsVisible('151_31') = 'True'">
                  <tr>
                    <td class="colTitle">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v151apt_31')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@v151apt_31" />
                    </td>
                    <td class="colTitle">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v151apt_32')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@v151apt_32" />
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="xslLogFormat:IsVisible('151_51') = 'True'">
                  <tr>
                    <td class="colTitle">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v151apt_51')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@v151apt_51" />
                    </td>
                    <td class="colTitle">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v151apt_52')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@v151apt_52" />
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="xslLogFormat:IsVisible('152_41') = 'True'">
                  <tr>
                    <td class="colTitle">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v152apt_41')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@v152apt_41" />
                    </td>
                    <td class="colTitle">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v152apt_42')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@v152apt_42" />
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="xslLogFormat:IsVisible('152_51') = 'True'">
                  <tr>
                    <td class="colTitle">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v152apt_51')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@v152apt_51" />
                    </td>
                    <td class="colTitle">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v152apt_52')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@v152apt_52" />
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="xslLogFormat:IsVisible('153_51') = 'True'">
                  <tr>
                    <td class="colTitle">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v153apt_51')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@v153apt_51" />
                    </td>
                    <td class="colTitle">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v153apt_52')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@v153apt_52" />
                    </td>
                  </tr>
                </xsl:if>

              </table>
            </div>
          </td>
        </tr>

      </xsl:for-each>
    </table>
  </xsl:template>
</xsl:stylesheet>

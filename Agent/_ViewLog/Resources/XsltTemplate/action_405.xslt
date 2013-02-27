<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
	<xsl:param name="IsInternal" select="0"></xsl:param>
	<xsl:output method="html" indent="no" />

	<xsl:template match="acc_customerupdmemberfi_sag">
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
								<tr>
									<td class="colW1">
										<xsl:value-of select="xslLogFormat:GetLogResource('SubAccName')" />
									</td>
									<td class="colW2">
										<xsl:value-of select="@subaccname" />
									</td>
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
								</tr>
								<tr>
									<td class="colW1">
										<xsl:value-of select="xslLogFormat:GetLogResource('ErrMsg')" />
									</td>
									<td class="colW2">
										<xsl:value-of select="@errmsg" />
									</td>
									<td class="colW1">
										<xsl:value-of select="xslLogFormat:GetLogResource('Language')" />
									</td>
									<td class="colW2">
										<xsl:value-of select="@language" />
									</td>
									<td class="colTitle"></td>
									<td class="colValue">

									</td>
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
					<td class="colTitle"></td>
					<td class="colValue">

					</td>
				</tr>

			</xsl:for-each>
			<xsl:for-each select="au">
				<tr class="headerDetailNormal" onclick="slideToggle(this);">
					<td colspan="6">
						<xsl:value-of select="xslLogFormat:GetLogResource('au')" />
					</td>
				</tr>
				<tr>
					<td class="colW1">
						<xsl:value-of select="xslLogFormat:GetLogResource('a_v201spt_2')" />
					</td>
					<td class="colW2">
						<xsl:value-of select="@a_v201spt_2" />
					</td>
					<td class="colW1">
						<xsl:value-of select="xslLogFormat:GetLogResource('a_v201spt_3')" />
					</td>
					<td class="colW2">
						<xsl:value-of select="@a_v201spt_3" />
					</td>
					<td class="colTitle"></td>
					<td class="colValue">

					</td>
				</tr>


			</xsl:for-each>
			<xsl:for-each select="ia">
				<tr class="headerDetailNormal" onclick="slideToggle(this);">
					<td colspan="6">
						<xsl:value-of select="xslLogFormat:GetLogResource('ia')" />
					</td>
				</tr>
				<tr >
					<td class="colW1">
						<xsl:value-of select="xslLogFormat:GetLogResource('isa_v201spt_2')" />
					</td>
					<td class="colW2">
						<xsl:value-of select="@isa_v201spt_2" />
					</td>
					<td class="colW1">
						<xsl:value-of select="xslLogFormat:GetLogResource('isa_v201spt_3')" />
					</td>
					<td class="colW2">
						<xsl:value-of select="@isa_v201spt_3" />
					</td>
					<td class="colTitle"></td>
					<td class="colValue">

					</td>
				</tr>


			</xsl:for-each>
			<!--
              <xsl:for-each select="isau">
                <tr class="headerDetailNormal" onclick="slideToggle(this);">
                  <td colspan="6"><xsl:value-of select="xslLogFormat:GetLogResource('isau')" /></td>
                </tr>
                <tr>
                  <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('isa_v201smpt_2')" /></td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v201smpt_2" />
                  </td>
                  <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('isa_v201smpt_3')" /></td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v201smpt_3" />
                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                </tr>
              </xsl:for-each>
            -->


		</table>
	</xsl:template>



</xsl:stylesheet>

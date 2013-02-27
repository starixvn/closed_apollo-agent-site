<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html" indent="no" />
  <xsl:template match="acc_customerupdmemberhr_sag">
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
			  <xsl:if test="xslLogFormat:IsVisible('151_41') = 'True'">
				  <tr>
					  <td class="colW1">
						  <xsl:value-of select="xslLogFormat:GetLogResource('v151spt_41')" />
					  </td>
					  <td class="colW2">
						  <xsl:value-of select="@v151spt_41" />
					  </td>
					  <td class="colW1">
						  <xsl:value-of select="xslLogFormat:GetLogResource('v151spt_42')" />
					  </td>
					  <td class="colW2">
						  <xsl:value-of select="@v151spt_42" />
					  </td>
				  </tr>
			  </xsl:if>
			  <xsl:if test="xslLogFormat:IsVisible('151_31') = 'True'">
				  <tr>
					  <td class="colW1">
						  <xsl:value-of select="xslLogFormat:GetLogResource('v151spt_31')" />
					  </td>
					  <td class="colW2">
						  <xsl:value-of select="@v151spt_31" />
					  </td>
					  <td class="colW1">
						  <xsl:value-of select="xslLogFormat:GetLogResource('v151spt_32')" />
					  </td>
					  <td class="colW2">
						  <xsl:value-of select="@v151spt_32" />
					  </td>
				  </tr>
			  </xsl:if>
			  <xsl:if test="xslLogFormat:IsVisible('151_51') = 'True'">
				  <tr>
					  <td class="colW1">
						  <xsl:value-of select="xslLogFormat:GetLogResource('v151spt_51')" />
					  </td>
					  <td class="colW2">
						  <xsl:value-of select="@v151spt_51" />
					  </td>
					  <td class="colW1">
						  <xsl:value-of select="xslLogFormat:GetLogResource('v151spt_52')" />
					  </td>
					  <td class="colW2">
						  <xsl:value-of select="@v151spt_52" />
					  </td>
				  </tr>
			  </xsl:if>
			  <xsl:if test="xslLogFormat:IsVisible('152_41') = 'True'">
				  <tr>
					  <td class="colW1">
						  <xsl:value-of select="xslLogFormat:GetLogResource('v152spt_41')" />
					  </td>
					  <td class="colW2">
						  <xsl:value-of select="@v152spt_41" />
					  </td>
					  <td class="colW1">
						  <xsl:value-of select="xslLogFormat:GetLogResource('v152spt_42')" />
					  </td>
					  <td class="colW2">
						  <xsl:value-of select="@v152spt_42" />
					  </td>
				  </tr>
			  </xsl:if>
			  <xsl:if test="xslLogFormat:IsVisible('152_51') = 'True'">
				  <tr>
					  <td class="colW1">
						  <xsl:value-of select="xslLogFormat:GetLogResource('v152spt_51')" />
					  </td>
					  <td class="colW2">
						  <xsl:value-of select="@v152spt_51" />
					  </td>
					  <td class="colW1">
						  <xsl:value-of select="xslLogFormat:GetLogResource('v152spt_52')" />
					  </td>
					  <td class="colW2">
						  <xsl:value-of select="@v152spt_52" />
					  </td>
				  </tr>
			  </xsl:if>
			  <xsl:if test="xslLogFormat:IsVisible('153_51') = 'True'">
				  <tr>
					  <td class="colW1">
						  <xsl:value-of select="xslLogFormat:GetLogResource('v153spt_51')" />
					  </td>
					  <td class="colW2">
						  <xsl:value-of select="@v153spt_51" />
					  </td>
					  <td class="colW1">
						  <xsl:value-of select="xslLogFormat:GetLogResource('v153spt_52')" />
					  </td>
					  <td class="colW2">
						  <xsl:value-of select="@v153spt_52" />
					  </td>
				  </tr>
			  </xsl:if>
		  </xsl:for-each>
		  <xsl:for-each select="au">
			  <tr class="headerDetailNormal" onclick="slideToggle(this);">
				  <td colspan="6">
					  <xsl:value-of select="xslLogFormat:GetLogResource('au')" />
				  </td>
			  </tr>

			  <tr>
				  <td colspan="6">
					  <div>
						  <table class="tblDesc">
							  <xsl:if test="xslLogFormat:IsVisible('151_41') = 'True'">
								  <tr>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('a_v151spt_41')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@a_v151spt_41" />
									  </td>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('a_v151spt_42')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@a_v151spt_42" />
									  </td>
								  </tr>
							  </xsl:if>
							  <xsl:if test="xslLogFormat:IsVisible('151_31') = 'True'">
								  <tr>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('a_v151spt_31')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@a_v151spt_31" />
									  </td>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('a_v151spt_32')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@a_v151spt_32" />
									  </td>
								  </tr>
							  </xsl:if>
							  <xsl:if test="xslLogFormat:IsVisible('151_51') = 'True'">
								  <tr>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('a_v151spt_51')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@a_v151spt_51" />
									  </td>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('a_v151spt_52')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@a_v151spt_52" />
									  </td>
								  </tr>
							  </xsl:if>
							  <xsl:if test="xslLogFormat:IsVisible('152_41') = 'True'">
								  <tr>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('a_v152spt_41')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@a_v152spt_41" />
									  </td>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('a_v152spt_42')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@a_v152spt_42" />
									  </td>
								  </tr>
							  </xsl:if>
							  <xsl:if test="xslLogFormat:IsVisible('152_51') = 'True'">
								  <tr>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('a_v152spt_51')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@a_v152spt_51" />
									  </td>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('a_v152spt_52')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@a_v152spt_52" />
									  </td>
								  </tr>
							  </xsl:if>
							  <xsl:if test="xslLogFormat:IsVisible('153_51') = 'True'">
								  <tr>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('a_v153spt_51')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@a_v153spt_51" />
									  </td>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('a_v153spt_52')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@a_v153spt_52" />
									  </td>
								  </tr>
							  </xsl:if>
						  </table>
					  </div>
				  </td>
			  </tr>

		  </xsl:for-each>
		  <xsl:for-each select="ia">
			  <tr class="headerDetailNormal" onclick="slideToggle(this);">
				  <td colspan="6">
					  <xsl:value-of select="xslLogFormat:GetLogResource('ia')" />
				  </td>
			  </tr>

			  <tr>
				  <td colspan="6">
					  <div>
						  <table class="tblDesc">
							  <xsl:if test="xslLogFormat:IsVisible('151_41') = 'True'">
								  <tr>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('isa_v151spt_41')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@isa_v151spt_41" />
									  </td>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('isa_v151spt_42')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@isa_v151spt_42" />
									  </td>
								  </tr>
							  </xsl:if>
							  <xsl:if test="xslLogFormat:IsVisible('151_31') = 'True'">
								  <tr>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('isa_v151spt_31')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@isa_v151spt_31" />
									  </td>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('isa_v151spt_32')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@isa_v151spt_32" />
									  </td>
								  </tr>
							  </xsl:if>
							  <xsl:if test="xslLogFormat:IsVisible('151_51') = 'True'">
								  <tr>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('isa_v151spt_51')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@isa_v151spt_51" />
									  </td>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('isa_v151spt_52')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@isa_v151spt_52" />
									  </td>
								  </tr>
							  </xsl:if>
							  <xsl:if test="xslLogFormat:IsVisible('152_41') = 'True'">
								  <tr>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('isa_v152spt_41')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@isa_v152spt_41" />
									  </td>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('isa_v152spt_42')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@isa_v152spt_42" />
									  </td>
								  </tr>
							  </xsl:if>
							  <xsl:if test="xslLogFormat:IsVisible('152_51') = 'True'">
								  <tr>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('isa_v152spt_51')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@isa_v152spt_51" />
									  </td>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('isa_v152spt_52')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@isa_v152spt_52" />
									  </td>
								  </tr>
							  </xsl:if>
							  <xsl:if test="xslLogFormat:IsVisible('153_51') = 'True'">
								  <tr>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('isa_v153spt_51')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@isa_v153spt_51" />
									  </td>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('isa_v153spt_52')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@isa_v153spt_52" />
									  </td>
								  </tr>
							  </xsl:if>
						  </table>
					  </div>
				  </td>
			  </tr>


		  </xsl:for-each>
		  <xsl:for-each select="isau">
			  <tr class="headerDetailNormal" onclick="slideToggle(this);">
				  <td colspan="6">
					  <xsl:value-of select="xslLogFormat:GetLogResource('isau')" />
				  </td>
			  </tr>

			  <tr>
				  <td colspan="6">
					  <div>
						  <table class="tblDesc">
							  <xsl:if test="xslLogFormat:IsVisible('151_41') = 'True'">
								  <tr>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('isa_v151smpt_41')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@isa_v151smpt_41" />
									  </td>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('isa_v151smpt_42')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@isa_v151smpt_42" />
									  </td>
								  </tr>
							  </xsl:if>
							  <xsl:if test="xslLogFormat:IsVisible('151_31') = 'True'">
								  <tr>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('isa_v151smpt_31')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@isa_v151smpt_31" />
									  </td>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('isa_v151smpt_32')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@isa_v151smpt_32" />
									  </td>
								  </tr>
							  </xsl:if>
							  <xsl:if test="xslLogFormat:IsVisible('151_51') = 'True'">
								  <tr>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('isa_v151smpt_51')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@isa_v151smpt_51" />
									  </td>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('isa_v151smpt_52')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@isa_v151smpt_52" />
									  </td>
								  </tr>
							  </xsl:if>
							  <xsl:if test="xslLogFormat:IsVisible('152_41') = 'True'">
								  <tr>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('isa_v152smpt_41')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@isa_v152smpt_41" />
									  </td>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('isa_v152smpt_42')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@isa_v152smpt_42" />
									  </td>
								  </tr>
							  </xsl:if>
							  <xsl:if test="xslLogFormat:IsVisible('152_51') = 'True'">
								  <tr>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('isa_v152smpt_51')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@isa_v152smpt_51" />
									  </td>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('isa_v152smpt_52')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@isa_v152smpt_52" />
									  </td>
								  </tr>
							  </xsl:if>
							  <xsl:if test="xslLogFormat:IsVisible('153_51') = 'True'">
								  <tr>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('isa_v153smpt_51')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@isa_v153smpt_51" />
									  </td>
									  <td class="colW1">
										  <xsl:value-of select="xslLogFormat:GetLogResource('isa_v153smpt_52')" />
									  </td>
									  <td class="colW2">
										  <xsl:value-of select="@isa_v153smpt_52" />
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

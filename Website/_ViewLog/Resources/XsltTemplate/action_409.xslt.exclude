<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html" indent="no" />
  <xsl:template match="acc_customerupdmemberhr_mag">
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
								  <xsl:value-of select="xslLogFormat:GetLogResource('Language')" />
							  </td>
							  <td class="colW2">
								  <xsl:value-of select="@language" />
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
								  <xsl:value-of select="xslLogFormat:GetLogResource('ErrCode')" />
							  </td>
							  <td class="colW2">
								  <xsl:value-of select="@errcode" />
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

      <xsl:for-each select="m">
        <tr class="headerDetailNormal" onclick="slideToggle(this);">
          <td colspan="6">
            <xsl:value-of select="xslLogFormat:GetLogResource('m')" />
          </td>
        </tr>

        <tr>
          <td colspan="6">
            <div>
              <table class="tblDesc">
                <xsl:if test="xslLogFormat:IsVisible('151_41') = 'True'">
                  <tr>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v151mpt_41')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@v151mpt_41" />
                    </td>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v151mpt_42')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@v151mpt_42" />
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="xslLogFormat:IsVisible('151_31') = 'True'">
                  <tr>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v151mpt_31')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@v151mpt_31" />
                    </td>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v151mpt_32')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@v151mpt_32" />
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="xslLogFormat:IsVisible('151_51') = 'True'">
                  <tr>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v151mpt_51')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@v151mpt_51" />
                    </td>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v151mpt_52')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@v151mpt_52" />
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="xslLogFormat:IsVisible('152_41') = 'True'">
                  <tr>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v152mpt_41')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@v152mpt_41" />
                    </td>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v152mpt_42')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@v152mpt_42" />
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="xslLogFormat:IsVisible('152_51') = 'True'">
                  <tr>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v152mpt_51')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@v152mpt_51" />
                    </td>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v152mpt_52')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@v152mpt_52" />
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="xslLogFormat:IsVisible('153_51') = 'True'">
                  <tr>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v153mpt_51')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@v153mpt_51" />
                    </td>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v153mpt_52')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@v153mpt_52" />
                    </td>
                  </tr>
                </xsl:if>
              </table>
            </div>
          </td>
        </tr>


      </xsl:for-each>
      <xsl:for-each select="iam">
        <tr class="headerDetailNormal" onclick="slideToggle(this);">
          <td colspan="6">
            <xsl:value-of select="xslLogFormat:GetLogResource('iam')" />
          </td>
        </tr>

        <tr>
          <td colspan="6">
            <div>
              <table class="tblDesc">
                <xsl:if test="xslLogFormat:IsVisible('151_41') = 'True'">
                  <tr>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('isa_v151mpt_41')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@isa_v151mpt_41" />
                    </td>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('isa_v151mpt_42')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@isa_v151mpt_42" />
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="xslLogFormat:IsVisible('151_31') = 'True'">
                  <tr>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('isa_v151mpt_31')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@isa_v151mpt_31" />
                    </td>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('isa_v151mpt_32')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@isa_v151mpt_32" />
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="xslLogFormat:IsVisible('151_51') = 'True'">
                  <tr>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('isa_v151mpt_51')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@isa_v151mpt_51" />
                    </td>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('isa_v151mpt_52')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@isa_v151mpt_52" />
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="xslLogFormat:IsVisible('152_41') = 'True'">
                  <tr>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('isa_v152mpt_41')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@isa_v152mpt_41" />
                    </td>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('isa_v152mpt_42')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@isa_v152mpt_42" />
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="xslLogFormat:IsVisible('152_51') = 'True'">
                  <tr>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('isa_v152mpt_51')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@isa_v152mpt_51" />
                    </td>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('isa_v152mpt_52')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@isa_v152mpt_52" />
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="xslLogFormat:IsVisible('153_51') = 'True'">
                  <tr>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('isa_v153mpt_51')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@isa_v153mpt_51" />
                    </td>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('isa_v153mpt_52')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@isa_v153mpt_52" />
                    </td>
                  </tr>
                </xsl:if>
              </table>
            </div>
          </td>
        </tr>



      </xsl:for-each>

      <xsl:for-each select="iame">
        <tr class="headerDetailNormal" onclick="slideToggle(this);">
          <td colspan="6">
            <xsl:value-of select="xslLogFormat:GetLogResource('iame')" />
          </td>
        </tr>
        <tr>
          <td colspan="6">
            <div>
              <table class="tblDesc">

                <xsl:if test="xslLogFormat:IsVisible('151_41') = 'True'">
                  <tr>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('isa_v151mpt_mem_41')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@isa_v151mpt_mem_41" />
                    </td>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('isa_v151mpt_mem_42')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@isa_v151mpt_mem_42" />
                    </td>
                  </tr>
                </xsl:if>

                <xsl:if test="xslLogFormat:IsVisible('151_31') = 'True'">
                  <tr>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('isa_v151mpt_mem_31')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@isa_v151mpt_mem_31" />
                    </td>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('isa_v151mpt_mem_32')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@isa_v151mpt_mem_32" />
                    </td>
                  </tr>
                </xsl:if>

                <xsl:if test="xslLogFormat:IsVisible('151_51') = 'True'">

                  <tr>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('isa_v151mpt_mem_51')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@isa_v151mpt_mem_51" />
                    </td>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('isa_v151mpt_mem_52')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@isa_v151mpt_mem_52" />
                    </td>
                  </tr>

                </xsl:if>
                <xsl:if test="xslLogFormat:IsVisible('152_41') = 'True'">
                  <tr>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('isa_v152mpt_mem_41')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@isa_v152mpt_mem_41" />
                    </td>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('isa_v152mpt_mem_42')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@isa_v152mpt_mem_42" />
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="xslLogFormat:IsVisible('152_51') = 'True'">
                  <tr>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('isa_v152mpt_mem_51')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@isa_v152mpt_mem_51" />
                    </td>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('isa_v152mpt_mem_52')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@isa_v152mpt_mem_52" />
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="xslLogFormat:IsVisible('153_51') = 'True'">
                  <tr>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('isa_v153mpt_mem_51')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@isa_v153mpt_mem_51" />
                    </td>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('isa_v153mpt_mem_52')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@isa_v153mpt_mem_52" />
                    </td>
                  </tr>
                </xsl:if>
              </table>
            </div>
          </td>
        </tr>
      </xsl:for-each>
      <xsl:for-each select="aume">
        <tr class="headerDetailNormal" onclick="slideToggle(this);">
          <td colspan="6">
            <xsl:value-of select="xslLogFormat:GetLogResource('aume')" />
          </td>
        </tr>

        <tr>
          <td colspan="6">
            <div>
              <table class="tblDesc">
                <xsl:if test="xslLogFormat:IsVisible('151_41') = 'True'">
                  <tr>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('a_v151mpt_mem_41')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@a_v151mpt_mem_41" />
                    </td>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('a_v151mpt_mem_42')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@a_v151mpt_mem_42" />
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="xslLogFormat:IsVisible('151_31') = 'True'">
                  <tr>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('a_v151mpt_mem_31')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@a_v151mpt_mem_31" />
                    </td>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('a_v151mpt_mem_32')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@a_v151mpt_mem_32" />
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="xslLogFormat:IsVisible('151_51') = 'True'">
                  <tr>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('a_v151mpt_mem_51')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@a_v151mpt_mem_51" />
                    </td>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('a_v151mpt_mem_52')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@a_v151mpt_mem_52" />
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="xslLogFormat:IsVisible('152_41') = 'True'">
                  <tr>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('a_v152mpt_mem_41')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@a_v152mpt_mem_41" />
                    </td>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('a_v152mpt_mem_42')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@a_v152mpt_mem_42" />
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="xslLogFormat:IsVisible('152_51') = 'True'">
                  <tr>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('a_v152mpt_mem_51')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@a_v152mpt_mem_51" />
                    </td>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('a_v152mpt_mem_52')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@a_v152mpt_mem_52" />
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="xslLogFormat:IsVisible('153_51') = 'True'">
                  <tr>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('a_v153mpt_mem_51')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@a_v153mpt_mem_51" />
                    </td>
                    <td class="colW1">
                      <xsl:value-of select="xslLogFormat:GetLogResource('a_v153mpt_mem_52')" />
                    </td>
                    <td class="colW2">
                      <xsl:value-of select="@a_v153mpt_mem_52" />
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

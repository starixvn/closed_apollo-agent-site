<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html" indent="no" />
  <xsl:template match="acc_custoverrideupdhr">
    <xsl:choose>
      <xsl:when test="@custid">
        <table class="tblDesc">
          <tr>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('checkPermission')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@checkpermission" />
            </td>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('updMinPos')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@updminpos" />
            </td>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('updMaxPos')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@updmaxpos" />
            </td>
          </tr>
          <tr>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('language')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@language" />
            </td>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('subAccName')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@subaccname" />
            </td>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('writeLog')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@writelog" />
            </td>
          </tr>
          <tr>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('ip')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
            </td>

            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('errCode')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@errcode" />
            </td>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('errMsg')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@errmsg" />
            </td>
          </tr>
          <xsl:choose>
            <xsl:when test="@minv151pt_31">
              <tr class="headerDetailNormal" onclick="slideToggle(this);">
                <td colspan="6">
                  <xsl:value-of select="xslLogFormat:GetLogResource('MinPT')" />
                </td>
              </tr>
              <tr >
                <td class="colW1">
                  <xsl:value-of select="xslLogFormat:GetLogResource('minV151PT_31')" />
                </td>
                <td class="colW2">
                  <xsl:value-of select="@minv151pt_31" />
                </td>
                <td class="colW1">
                  <xsl:value-of select="xslLogFormat:GetLogResource('minV151PT_32')" />
                </td>
                <td class="colW2">
                  <xsl:value-of select="@minv151pt_32" />
                </td>
                <td class="colW1">
                  <xsl:value-of select="xslLogFormat:GetLogResource('minv151pt_41')" />
                </td>
                <td class="colW2">
                  <xsl:value-of select="@minv151pt_41" />
                </td>
              </tr>
              <tr >
                <td class="colW1">
                  <xsl:value-of select="xslLogFormat:GetLogResource('minv151pt_42')" />
                </td>
                <td class="colW2">
                  <xsl:value-of select="@minv151pt_42" />
                </td>
                <td class="colW1">
                
                </td>
                <td class="colW2">
               
                </td>
                <td class="colW1">

                </td>
                <td class="colW2">

                </td>
              </tr>
            </xsl:when>
          </xsl:choose>
          <xsl:for-each select="minmaxpt">
            <tr class="headerDetailNormal" onclick="slideToggle(this);">
              <td colspan="6">
                <xsl:value-of select="xslLogFormat:GetLogResource('MinPT')" />
              </td>
            </tr>
            <tr >
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetLogResource('minV151PT_31')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv151pt_31" />
              </td>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetLogResource('minV151PT_32')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv151pt_32" />
              </td>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetLogResource('minv151pt_41')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv151pt_41" />
              </td>
            </tr>
            <tr >
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetLogResource('minv151pt_42')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv151pt_42" />
              </td>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetLogResource('minv151pt_51')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv151pt_51" />
              </td>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetLogResource('minv151pt_52')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv151pt_52" />
              </td>
            </tr>
            <tr >
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetLogResource('minv152pt_41')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv152pt_41" />
              </td>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetLogResource('minv152pt_42')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv152pt_42" />
              </td>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetLogResource('minv152pt_51')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv152pt_51" />
              </td>
            </tr>
            <tr >
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetLogResource('minv152pt_52')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv152pt_52" />
              </td>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetLogResource('minv153pt_51')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv153pt_51" />
              </td>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetLogResource('minv153pt_52')" />
              </td>
              <td class="colW2">
                <xsl:value-of select="@minv153pt_52" />
              </td>
            </tr>
          </xsl:for-each>
        </table>
      </xsl:when>
      <xsl:otherwise>
        <table class="tblDesc">
          <xsl:for-each select="info">
            <tr class="headerDetailNormal" onclick="slideToggle(this);">
              <td colspan="8">
                <xsl:value-of select="xslLogFormat:GetLogResource('custInfo')" />
              </td>
            </tr>
			  <tr>
				  <td colspan="6">
					  <div >
						  <table class="tblDesc" >
							  <tr >

								  <td class="colW1">
									  <xsl:value-of select="xslLogFormat:GetLogResource('checkPermission')" />
								  </td>
								  <td class="colW2">
									  <xsl:value-of select="@checkpermission" />
								  </td>
								  <td class="colW1">
									  <xsl:value-of select="xslLogFormat:GetLogResource('updMinPos')" />
								  </td>
								  <td class="colW2">
									  <xsl:value-of select="@updminpos" />
								  </td>
								  <td class="colW1">
									  <xsl:value-of select="xslLogFormat:GetLogResource('updMaxPos')" />
								  </td>
								  <td class="colW2">
									  <xsl:value-of select="@updmaxpos" />
								  </td>
								  <td class="colW1">
									  <xsl:value-of select="xslLogFormat:GetLogResource('language')" />
								  </td>
								  <td class="colW2">
									  <xsl:value-of select="@language" />
								  </td>
							  </tr>
							  <tr >
								  <td class="colW1">
									  <xsl:value-of select="xslLogFormat:GetLogResource('subAccName')" />
								  </td>
								  <td class="colW2">
									  <xsl:value-of select="@subaccname" />
								  </td>
								  <td class="colW1">
									  <xsl:value-of select="xslLogFormat:GetLogResource('writeLog')" />
								  </td>
								  <td class="colW2">
									  <xsl:value-of select="@writelog" />
								  </td>
								  <td class="colW1">
									  <xsl:value-of select="xslLogFormat:GetLogResource('ip')" />
								  </td>
								  <td class="colW2">
									  <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
								  </td>
								  <td class="colW1">
									  <xsl:value-of select="xslLogFormat:GetLogResource('errCode')" />
								  </td>
								  <td class="colW2">
									  <xsl:value-of select="@errcode" />
								  </td>
							  </tr>
							  <tr >
								  <td class="colW1">
									  <xsl:value-of select="xslLogFormat:GetLogResource('errMsg')" />
								  </td>
								  <td class="colW2">
									  <xsl:value-of select="@errmsg" />
								  </td>
								  <td>

								  </td>
								  <td>

								  </td>
								  <td>

								  </td>
								  <td>

								  </td>
								  <td>

								  </td>
								  <td>

								  </td>
							  </tr>
						  </table>
					  </div>
				  </td>
			  </tr>
          </xsl:for-each>
          <xsl:for-each select="minmaxpt">
            <tr class="headerDetailNormal" onclick="slideToggle(this);">
              <td colspan="8">
                <xsl:value-of select="xslLogFormat:GetLogResource('MinPT')" />
              </td>
            </tr>

            <xsl:if test="xslLogFormat:IsVisible('151_41') = 'True'">
              <tr>
                <td class="colW1" colspan="2">
                  <xsl:value-of select="xslLogFormat:GetLogResource('minv151pt_41')" />
                </td>
                <td class="colW2" colspan="2">
                  <xsl:value-of select="@minv151pt_41" />
                </td>
                <td class="colW1" colspan="2">
                  <xsl:value-of select="xslLogFormat:GetLogResource('minv151pt_42')" />
                </td>
                <td class="colW2" colspan="2">
                  <xsl:value-of select="@minv151pt_42" />
                </td>
              </tr>
            </xsl:if>

            <xsl:if test="xslLogFormat:IsVisible('151_31') = 'True'">
              <tr >
                <td class="colW1" colspan="2">
                  <xsl:value-of select="xslLogFormat:GetLogResource('minV151PT_31')" />
                </td>
                <td class="colW2" colspan="2">
                  <xsl:value-of select="@minv151pt_31" />
                </td>
                <td class="colW1" colspan="2">
                  <xsl:value-of select="xslLogFormat:GetLogResource('minV151PT_32')" />
                </td>
                <td class="colW2" colspan="2">
                  <xsl:value-of select="@minv151pt_32" />
                </td>
              </tr>
            </xsl:if>

            <xsl:if test="xslLogFormat:IsVisible('151_51') = 'True'">
              <tr >
                <td class="colW1" colspan="2">
                  <xsl:value-of select="xslLogFormat:GetLogResource('minv151pt_51')" />
                </td>
                <td class="colW2" colspan="2">
                  <xsl:value-of select="@minv151pt_51" />
                </td>
                <td class="colW1" colspan="2">
                  <xsl:value-of select="xslLogFormat:GetLogResource('minv151pt_52')" />
                </td>
                <td class="colW2" colspan="2">
                  <xsl:value-of select="@minv151pt_52" />
                </td>
              </tr>
            </xsl:if>

            <xsl:if test="xslLogFormat:IsVisible('152_41') = 'True'">
              <tr >
                <td class="colW1" colspan="2">
                  <xsl:value-of select="xslLogFormat:GetLogResource('minv152pt_41')" />
                </td>
                <td class="colW2" colspan="2">
                  <xsl:value-of select="@minv152pt_41" />
                </td>
                <td class="colW1" colspan="2">
                  <xsl:value-of select="xslLogFormat:GetLogResource('minv152pt_42')" />
                </td>
                <td class="colW2" colspan="2">
                  <xsl:value-of select="@minv152pt_42" />
                </td>

              </tr>
            </xsl:if>

            <xsl:if test="xslLogFormat:IsVisible('152_51') = 'True'">
              <tr >
                <td class="colW1" colspan="2">
                  <xsl:value-of select="xslLogFormat:GetLogResource('minv152pt_51')" />
                </td>
                <td class="colW2" colspan="2">
                  <xsl:value-of select="@minv152pt_51" />
                </td>
                <td class="colW1" colspan="2">
                  <xsl:value-of select="xslLogFormat:GetLogResource('minv152pt_52')" />
                </td>
                <td class="colW2" colspan="2">
                  <xsl:value-of select="@minv152pt_52" />
                </td>
              </tr>
            </xsl:if>

            <xsl:if test="xslLogFormat:IsVisible('153_51') = 'True'">
              <tr>
                <td class="colW1" colspan="2">
                  <xsl:value-of select="xslLogFormat:GetLogResource('minv153pt_51')" />
                </td>
                <td class="colW2" colspan="2">
                  <xsl:value-of select="@minv153pt_51" />
                </td>
                <td class="colW1" colspan="2">
                  <xsl:value-of select="xslLogFormat:GetLogResource('minv153pt_52')" />
                </td>
                <td class="colW2" colspan="2">
                  <xsl:value-of select="@minv153pt_52" />
                </td>
              </tr>
            </xsl:if>
          </xsl:for-each>
        </table>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>

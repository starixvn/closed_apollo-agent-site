<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="acc_customerupdmember_positiontaking">
    <table class="tblDesc">
      <xsl:for-each select="info">
        <tr class="headerDetail" onclick="slideToggle(this);">
          <td colspan="6"><xsl:value-of select="xslLogFormat:GetLogResource('custInfo')" /></td>
        </tr>
		  <tr>
			  <td colspan="6">
				  <div >
					  <table class="tblDesc" >
						  <tr >
							  <td class="colW1">
								  <xsl:value-of select="xslLogFormat:GetLogResource('UpdMultiple')" />
							  </td>
							  <td class="colW2">
								  <xsl:value-of select="@updmultiple" />
							  </td>
							  <td class="colW1">
								  <xsl:value-of select="xslLogFormat:GetLogResource('Updpt')" />
							  </td>
							  <td class="colW2">
								  <xsl:value-of select="@updpt" />
							  </td>
							  <td class="colW1">
								  <xsl:value-of select="xslLogFormat:GetLogResource('Updptv2')" />
							  </td>
							  <td class="colW2">
								  <xsl:value-of select="@updptv2" />
							  </td>
						  </tr>
						  <tr >
							  <td class="colW1">
								  <xsl:value-of select="xslLogFormat:GetLogResource('Updptv3')" />
							  </td>
							  <td class="colW2">
								  <xsl:value-of select="@updptv3" />
							  </td>
							  <td class="colW1">
								  <xsl:value-of select="xslLogFormat:GetLogResource('Updptv5')" />
							  </td>
							  <td class="colW2">
								  <xsl:value-of select="@updptv5" />
							  </td>
							  <td class="colW1">
								  <xsl:value-of select="xslLogFormat:GetLogResource('Updptv8')" />
							  </td>
							  <td class="colW2">
								  <xsl:value-of select="@updptv8" />
							  </td>
						  </tr>
						  <tr >
							  <td class="colW1">
								  <xsl:value-of select="xslLogFormat:GetLogResource('Updptv10')" />
							  </td>
							  <td class="colW2">
								  <xsl:value-of select="@updptv10" />
							  </td>
							  <td class="colW1">
								  <xsl:value-of select="xslLogFormat:GetLogResource('Updptv11')" />
							  </td>
							  <td class="colW2">
								  <xsl:value-of select="@updptv11" />
							  </td>
							  <td class="colW1">
								  <xsl:value-of select="xslLogFormat:GetLogResource('Updptv161')" />
							  </td>
							  <td class="colW2">
								  <xsl:value-of select="@updptv161" />
							  </td>
						  </tr>
						  <tr >
							  <td class="colW1">
								  <xsl:value-of select="xslLogFormat:GetLogResource('Updptv99')" />
							  </td>
							  <td class="colW2">
								  <xsl:value-of select="@updptv99" />
							  </td>
							  <td class="colW1">
								  <xsl:value-of select="xslLogFormat:GetLogResource('Updptv99MP')" />
							  </td>
							  <td class="colW2">
								  <xsl:value-of select="@updptv99mp" />
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
								  <xsl:value-of select="xslLogFormat:GetLogResource('IP')" />
							  </td>
							  <td class="colW2">
								  <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
							  </td>
							  <td class="colW1"></td>
							  <td class="colW2">

							  </td>
							  <td class="colW1"></td>
							  <td class="colW2">

							  </td>


						  </tr>

						  <tr >
							  <td class="colW1">
								  <xsl:value-of select="xslLogFormat:GetLogResource('Language')" />
							  </td>
							  <td class="colW2">
								  <xsl:value-of select="@language" />
							  </td>
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
						  </tr>
					  </table>
				  </div>
			  </td>
		  </tr>
      </xsl:for-each>
      <xsl:for-each select="ag">
        <tr class="headerDetail" onclick="slideToggle(this);">
          <td colspan="6" ><xsl:value-of select="xslLogFormat:GetLogResource('ag')" /></td>
        </tr>
        <tr>
          <td colspan="6">
            <div  style="display:none;">
              <table class="tblDesc">
                <tr class="headerSportTypeDetail">
                  <td colspan="6"><xsl:value-of select="xslLogFormat:GetLogResource('soccer')" /></td>
                </tr>
                <tr >
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('apt_1')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_1" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('apt_2')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_2" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('apt_3')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_3" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('apt_4')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_4" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('apt_5')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_5" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('apt_6')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_6" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('apt_7')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_7" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('apt_8')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_8" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('apt_9')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_9" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('apt_10')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_10" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('alivept_1')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@alivept_1" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('alivept_3')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@alivept_3" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('alivept_7')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@alivept_7" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('alivept_8')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@alivept_8" />
                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6"><xsl:value-of select="xslLogFormat:GetLogResource('basketball')" /></td>
                </tr>
                <tr >
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v2apt_1')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v2apt_1" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v2apt_2')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v2apt_2" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v2apt_3')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v2apt_3" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v2apt_7')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v2apt_7" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v2apt_8')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v2apt_8" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v2apt_20')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v2apt_20" />
                  </td>
                </tr>
               
                <tr class="headerSportTypeDetail">
                  <td colspan="6"><xsl:value-of select="xslLogFormat:GetLogResource('football')" /></td>
                </tr>
                <tr >
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v3apt_1')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v3apt_1" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v3apt_2')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v3apt_2" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v3apt_3')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v3apt_3" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v3apt_20')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v3apt_20" />
                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6"><xsl:value-of select="xslLogFormat:GetLogResource('tennis')" /></td>
                </tr>
                <tr >
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v5apt_1')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v5apt_1" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v5apt_2')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v5apt_2" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v5apt_3')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v5apt_3" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v5apt_20')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v5apt_20" />
                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6"><xsl:value-of select="xslLogFormat:GetLogResource('baseball')" /></td>
                </tr>
                <tr >
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v8apt_1')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v8apt_1" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v8apt_3')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v8apt_3" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v8apt_20')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v8apt_20" />
                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6"><xsl:value-of select="xslLogFormat:GetLogResource('golf')" /></td>
                </tr>
                <tr >
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v10apt_1')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v10apt_1" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v10apt_2')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v10apt_2" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v10apt_3')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v10apt_3" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v10apt_20')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v10apt_20" />
                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6"><xsl:value-of select="xslLogFormat:GetLogResource('motorsports')" /></td>
                </tr>
                <tr >
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v11apt_1')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v11apt_1" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v11apt_20')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v11apt_20" />
                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6"><xsl:value-of select="xslLogFormat:GetLogResource('numbergame')" /></td>
                </tr>
                <tr >
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v161apt_81')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v161apt_81" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v161apt_83')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v161apt_83" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v161apt_88')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v161apt_88" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v161apt_86')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v161apt_86" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v161alivept_85')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v161alivept_85" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v161alivept_86')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v161alivept_86" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v161alivept_87')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v161alivept_87" />
                  </td>
                  <td class="colTitle">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v161alivept_89')" />
                  </td>
                  <td class="colValue">
                    <xsl:value-of select="@v161alivept_89" />
                  </td>
                  <td class="colTitle">
                    <xsl:if test="xslLogFormat:IsVisible('161_90') = 'True'">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v161alivept_90')" />
                    </xsl:if>
                  </td>
                  <td class="colValue">
                    <xsl:if test="xslLogFormat:IsVisible('161_90') = 'True'">
                      <xsl:value-of select="@v161alivept_90" />
                    </xsl:if>
                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6"><xsl:value-of select="xslLogFormat:GetLogResource('othersports')" /></td>
                </tr>
                <tr >
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v99apt_1')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v99apt_1" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v99apt_2')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v99apt_2" />
                  </td>
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v99apt_3')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v99apt_3" />
                  </td>
                </tr>
                <tr >

                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v99apt_20')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v99apt_20" />
                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6"><xsl:value-of select="xslLogFormat:GetLogResource('mixsports')" /></td>
                </tr>
                <tr >
                  <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v99apt_9')" /></td>
                  <td class="colValue">
                    <xsl:value-of select="@v99apt_9" />
                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">
                   
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
    </table>
  </xsl:template>



</xsl:stylesheet>

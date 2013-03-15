<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />
  <xsl:template match="acc_customerupdmaster_positiontaking">
    <table class="tblDesc">
      <xsl:for-each select="info">
        <tr class="headerDetail" onclick="slideToggle(this);">
          <td colspan="6">
            <xsl:value-of select="xslLogFormat:GetLogResource('custInfo')" />
          </td>
        </tr>
		  <tr>
			  <td colspan="6">
				  <div >
					  <table class="tblDesc" >
						  <tr>
							  <td colspan="6">
								  <div style="display:block;">
									  <table class="tblDesc" >
										  <tr>
											  <td class="colW1">
												  <xsl:value-of select="xslLogFormat:GetLogResource('Updallmembers')" />
											  </td>
											  <td class="colW2">
												  <xsl:value-of select="@updallmembers" />
											  </td>
											  <td class="colW1">
												  <xsl:value-of select="xslLogFormat:GetLogResource('newspt')" />
											  </td>
											  <td class="colW2">
												  <xsl:value-of select="@newspt" />
											  </td>
											  <td class="colW1">
												  <xsl:value-of select="xslLogFormat:GetLogResource('UpdMultiple')" />
											  </td>
											  <td class="colW2">
												  <xsl:value-of select="@updmultiple" />
											  </td>
										  </tr>
										  <tr>
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
											  <td class="colW1">
												  <xsl:value-of select="xslLogFormat:GetLogResource('Updptv3')" />
											  </td>
											  <td class="colW2">
												  <xsl:value-of select="@updptv3" />
											  </td>
										  </tr>
										  <tr>
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
											  <td class="colW1">
												  <xsl:value-of select="xslLogFormat:GetLogResource('Updptv10')" />
											  </td>
											  <td class="colW2">
												  <xsl:value-of select="@updptv10" />
											  </td>
										  </tr>
										  <tr>
											  <td class="colW1">
												  <xsl:value-of select="xslLogFormat:GetLogResource('Updptv11')" />
											  </td>
											  <td class="colW2">
												  <xsl:value-of select="@updptv11" />
											  </td>
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
										  </tr>
										  <tr>
											  <td class="colW1">
												  <xsl:value-of select="xslLogFormat:GetLogResource('Updptv161')" />
											  </td>
											  <td class="colW2">
												  <xsl:value-of select="@updptv161" />
											  </td>
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
										  </tr>
										  <tr>
											  <td class="colW1">
												  <xsl:value-of select="xslLogFormat:GetLogResource('IP')" />
											  </td>
											  <td class="colW2">
												  <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
											  </td>
											  <td class="colTitle">
												  <xsl:value-of select="xslLogFormat:GetLogResource('ErrCode')" />
											  </td>
											  <td class="colValue">
												  <xsl:value-of select="@errcode" />
											  </td>
											  <td class="colTitle">
												  <xsl:value-of select="xslLogFormat:GetLogResource('ErrMsg')" />
											  </td>
											  <td class="colValue">
												  <xsl:value-of select="@errmsg" />
											  </td>
										  </tr>
									  </table>
								  </div>
							  </td>
						  </tr>
					  </table>
				  </div>
			  </td>
		  </tr>		  
      </xsl:for-each>

      <xsl:for-each select="s">
        <tr class="headerDetail" onclick="slideToggle(this)" >
          <td colspan="6">
            <xsl:value-of select="xslLogFormat:GetLogResource('s')" />
          </td>
        </tr>
        <tr>
          <td colspan="6">
            <div style="display:none;">
              <table  class="tblDesc" >
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('soccer')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('spt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@spt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('spt_2')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@spt_2" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('spt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@spt_3" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('spt_4')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@spt_4" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('spt_5')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@spt_5" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('spt_6')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@spt_6" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('spt_7')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@spt_7" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('spt_8')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@spt_8" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('spt_9')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@spt_9" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('spt_10')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@spt_10" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('slivept_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@slivept_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('slivept_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@slivept_3" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('slivept_7')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@slivept_7" />
                  </td>

                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('slivept_8')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@slivept_8" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>


                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('Basketball')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v2spt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v2spt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v2spt_2')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v2spt_2" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v2spt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v2spt_3" />
                  </td>

                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v2spt_7')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v2spt_7" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v2spt_8')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v2spt_8" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v2spt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v2spt_20" />
                  </td>
                </tr>

                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('Football')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v3spt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v3spt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v3spt_2')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v3spt_2" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v3spt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v3spt_3" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v3spt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v3spt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('Tennis')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v5spt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v5spt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v5spt_2')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v5spt_2" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v5spt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v5spt_3" />
                  </td>
                </tr>
                <tr>

                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v5spt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v5spt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>

                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('Baseball')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v8spt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v8spt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v8spt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v8spt_3" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v8spt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v8spt_20" />
                  </td>

                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('Golf')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v10spt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v10spt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v10spt_2')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v10spt_2" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v10spt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v10spt_3" />
                  </td>

                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v10spt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v10spt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('Motosports')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v11spt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v11spt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v11spt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v11spt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>

                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('OtherSports')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v99spt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v99spt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v99spt_2')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v99spt_2" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v99spt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v99spt_3" />
                  </td>

                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v99spt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v99spt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                  <td class="colW1"></td>
                  <td class="colW2"></td>
                </tr>

                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('numbergame')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v161spt_81')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v161spt_81" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v161spt_83')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v161spt_83" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v161spt_88')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v161spt_88" />
                  </td>

                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v161spt_86')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v161spt_86" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v161slivept_85')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v161slivept_85" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v161slivept_86')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v161slivept_86" />
                  </td>

                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v161slivept_87')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v161slivept_87" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v161slivept_89')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v161slivept_89" />
                  </td>
                  <td class="colW1">
                    <xsl:if test="xslLogFormat:IsVisible('161_90') = 'True'">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v161slivept_90')" />
                    </xsl:if>
                  </td>
                  <td class="colW2">
                    <xsl:if test="xslLogFormat:IsVisible('161_90') = 'True'">
                      <xsl:value-of select="@v161slivept_90" />
                    </xsl:if>
                  </td>

                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('mixsports')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v99spt_9')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v99spt_9" />
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

      <xsl:for-each select="au">
        <tr class="headerDetail" onclick="slideToggle(this);">
          <td colspan="6">
            <xsl:value-of select="xslLogFormat:GetLogResource('au')" />
          </td>
        </tr>
        <tr>
          <td colspan="6">
            <div style="display:none;">
              <table class="tblDesc" >
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('soccer')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_spt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_spt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_spt_2')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_spt_2" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_spt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_spt_3" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_spt_4')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_spt_4" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_spt_5')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_spt_5" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_spt_6')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_spt_6" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_spt_7')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_spt_7" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_spt_8')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_spt_8" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_spt_9')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_spt_9" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_spt_10')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_spt_10" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_slivept_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_slivept_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_slivept_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_slivept_3" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_slivept_7')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_slivept_7" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_slivept_8')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_slivept_8" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('Basketball')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v2spt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v2spt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v2spt_2')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v2spt_2" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v2spt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v2spt_3" />
                  </td>

                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v2spt_7')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v2spt_7" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v2spt_8')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v2spt_8" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v2spt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v2spt_20" />
                  </td>
                </tr>

                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('Football')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v3spt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v3spt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v3spt_2')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v3spt_2" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v3spt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v3spt_3" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v3spt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v3spt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('Tennis')" />
                  </td>
                </tr>
                <tr>

                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v5spt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v5spt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v5spt_2')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v5spt_2" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v5spt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v5spt_3" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v5spt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v5spt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('Baseball')" />
                  </td>
                </tr>
                <tr>


                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v8spt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v8spt_1" />
                  </td>

                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v8spt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v8spt_3" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v8spt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v8spt_20" />
                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('Golf')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v10spt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v10spt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v10spt_2')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v10spt_2" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v10spt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v10spt_3" />
                  </td>
                </tr>
                <tr>

                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v10spt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v10spt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('Motosports')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v11spt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v11spt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v11spt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v11spt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>

                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('OtherSports')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v99spt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v99spt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v99spt_2')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v99spt_2" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v99spt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v99spt_3" />
                  </td>

                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v99spt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v99spt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>

                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('mixsports')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v99spt_9')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v99spt_9" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                  <td class="colW1"></td>
                  <td class="colW2"></td>
                </tr>


                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('numbergame')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v161spt_81')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v161spt_81" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v161spt_83')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v161spt_83" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v161spt_88')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v161spt_88" />
                  </td>

                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v161spt_86')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v161spt_86" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v161slivept_85')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v161slivept_85" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v161slivept_86')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v161slivept_86" />
                  </td>

                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v161slivept_87')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v161slivept_87" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('a_v161slivept_89')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@a_v161slivept_89" />
                  </td>
                  <td class="colW1">
                    <xsl:if test="xslLogFormat:IsVisible('161_90') = 'True'">
                      <xsl:value-of select="xslLogFormat:GetLogResource('a_v161slivept_90')" />
                    </xsl:if>
                  </td>
                  <td class="colW2">
                    <xsl:if test="xslLogFormat:IsVisible('161_90') = 'True'">
                      <xsl:value-of select="@a_v161slivept_90" />
                    </xsl:if>
                  </td>
                </tr>
              </table>
            </div>
          </td>
        </tr>
      </xsl:for-each>

      <xsl:for-each select="ia">
        <tr class="headerDetail" onclick="slideToggle(this);">
          <td colspan="6">
            <xsl:value-of select="xslLogFormat:GetLogResource('isau')" />
          </td>
        </tr>
        <tr>
          <td colspan="6">
            <div style="display:none;">
              <table class="tblDesc" >
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('soccer')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_spt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_spt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_spt_2')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_spt_2" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_spt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_spt_3" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_spt_4')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_spt_4" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_spt_5')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_spt_5" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_spt_6')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_spt_6" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_spt_7')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_spt_7" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_spt_8')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_spt_8" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_spt_9')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_spt_9" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_spt_10')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_spt_10" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_slivept_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_slivept_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_slivept_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_slivept_3" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_slivept_7')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_slivept_7" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_slivept_8')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_slivept_8" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('Basketball')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v2spt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v2spt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v2spt_2')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v2spt_2" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v2spt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v2spt_3" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v2spt_7')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v2spt_7" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v2spt_8')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v2spt_8" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v2spt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v2spt_20" />
                  </td>

                </tr>

                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('Football')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v3spt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v3spt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v3spt_2')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v3spt_2" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v3spt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v3spt_3" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v3spt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v3spt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('Tennis')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v5spt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v5spt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v5spt_2')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v5spt_2" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v5spt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v5spt_3" />
                  </td>
                </tr>
                <tr>

                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v5spt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v5spt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('Baseball')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v8spt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v8spt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v8spt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v8spt_3" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v8spt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v8spt_20" />
                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('Golf')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v10spt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v10spt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v10spt_2')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v10spt_2" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v10spt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v10spt_3" />
                  </td>

                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v10spt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v10spt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('Motosports')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v11spt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v11spt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v11spt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v11spt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>

                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('OtherSports')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v99spt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v99spt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v99spt_2')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v99spt_2" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v99spt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v99spt_3" />
                  </td>

                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v99spt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v99spt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>

                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('mixsports')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v99spt_9')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v99spt_9" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                  <td class="colW1"></td>
                  <td class="colW2"></td>
                </tr>

                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('numbergame')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v161spt_81')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v161spt_81" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v161spt_83')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v161spt_83" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v161spt_88')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v161spt_88" />
                  </td>

                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v161spt_86')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v161spt_86" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v161slivept_85')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v161slivept_85" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v161slivept_86')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v161slivept_86" />
                  </td>

                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v161slivept_87')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v161slivept_87" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('isa_v161slivept_89')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@isa_v161slivept_89" />
                  </td>
                  <td class="colW1">
                    <xsl:if test="xslLogFormat:IsVisible('161_90') = 'True'">
                      <xsl:value-of select="xslLogFormat:GetLogResource('isa_v161slivept_90')" />
                    </xsl:if>
                  </td>
                  <td class="colW2">
                    <xsl:if test="xslLogFormat:IsVisible('161_90') = 'True'">
                      <xsl:value-of select="@isa_v161slivept_90" />
                    </xsl:if>
                  </td>
                </tr>

              </table>
            </div>
          </td>
        </tr>


      </xsl:for-each>

      <xsl:for-each select="m">
        <tr class="headerDetail" onclick="slideToggle(this);">
          <td colspan="6">
            <xsl:value-of select="xslLogFormat:GetLogResource('m')" />
          </td>
        </tr>
        <tr>
          <td colspan="6">
            <div style="display:none;">
              <table class="tblDesc" >
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('soccer')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('mpt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@mpt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('mpt_2')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@mpt_2" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('mpt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@mpt_3" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('mpt_4')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@mpt_4" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('mpt_5')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@mpt_5" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('mpt_6')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@mpt_6" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('mpt_7')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@mpt_7" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('mpt_8')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@mpt_8" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('mpt_9')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@mpt_9" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('mpt_10')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@mpt_10" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('mlivept_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@mlivept_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('mlivept_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@mlivept_3" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('mlivept_7')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@mlivept_7" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('mlivept_8')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@mlivept_8" />
                  </td>

                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>

                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('Basketball')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v2mpt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v2mpt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v2mpt_2')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v2mpt_2" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v2mpt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v2mpt_3" />
                  </td>

                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v2mpt_7')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v2mpt_7" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v2mpt_8')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v2mpt_8" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v2mpt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v2mpt_20" />
                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('Football')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v3mpt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v3mpt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v3mpt_2')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v3mpt_2" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v3mpt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v3mpt_3" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v3mpt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v3mpt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('Tennis')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v5mpt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v5mpt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v5mpt_2')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v5mpt_2" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v5mpt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v5mpt_3" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v5mpt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v5mpt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('Baseball')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v8mpt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v8mpt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v8mpt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v8mpt_3" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v8mpt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v8mpt_20" />
                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('Golf')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v10mpt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v10mpt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v10mpt_2')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v10mpt_2" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v10mpt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v10mpt_3" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v10mpt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v10mpt_20" />
                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('Motosports')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v11mpt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v11mpt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v11mpt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v11mpt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>

                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('OtherSports')" />
                  </td>
                </tr>
                <tr>

                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v99mpt_1')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v99mpt_1" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v99mpt_2')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v99mpt_2" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v99mpt_3')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v99mpt_3" />
                  </td>
                </tr>
                <tr>

                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v99mpt_20')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v99mpt_20" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                </tr>

                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('mixsports')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v99mpt_9')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v99mpt_9" />
                  </td>
                  <td class="colW1"></td>
                  <td class="colW2">

                  </td>
                  <td class="colW1"></td>
                  <td class="colW2"></td>
                </tr>

                <tr class="headerSportTypeDetail">
                  <td colspan="6">
                    <xsl:value-of select="xslLogFormat:GetLogResource('numbergame')" />
                  </td>
                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v161mpt_81')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v161mpt_81" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v161mpt_83')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v161mpt_83" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v161mpt_88')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v161mpt_88" />
                  </td>

                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v161mpt_86')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v161mpt_86" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v161mlivept_85')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v161mlivept_85" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v161mlivept_86')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v161mlivept_86" />
                  </td>

                </tr>
                <tr>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v161mlivept_87')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v161mlivept_87" />
                  </td>
                  <td class="colW1">
                    <xsl:value-of select="xslLogFormat:GetLogResource('v161mlivept_89')" />
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@v161mlivept_89" />
                  </td>
                  <td class="colW1">
                    <xsl:if test="xslLogFormat:IsVisible('161_90') = 'True'">
                      <xsl:value-of select="xslLogFormat:GetLogResource('v161mlivept_90')" />
                    </xsl:if>
                  </td>
                  <td class="colW2">
                    <xsl:if test="xslLogFormat:IsVisible('161_90') = 'True'">
                      <xsl:value-of select="@v161mlivept_90" />
                    </xsl:if>
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

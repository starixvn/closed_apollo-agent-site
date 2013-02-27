<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="acc_customerupdmember_super">
    <xsl:choose>
      <xsl:when test="@custid">
        <table class="tblDesc">
          <tr>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('closed')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@closed" />
            </td>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('suspended')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@suspended" />
            </td>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('updatemultiple')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@updatemultiple" />
            </td>
          </tr>

          <tr>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('disabledfi')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@disabledfi" />
            </td>


            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('ip')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
            </td>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('errcode')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@errcode" />
            </td>
          </tr>
          <tr>

            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('errmsg')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@errmsg" />
            </td>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('language')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@language" />
            </td>
            <xsl:choose>
              <xsl:when test="$IsInternal = 0">
                <td class="colW1"></td>
                <td class="colW2">

                </td>
              </xsl:when>
              <xsl:otherwise>
                <td class="colW1">
                  <xsl:value-of select="xslLogFormat:GetLogResource('subaccid')" />
                </td>
                <td class="colW2">

                  <xsl:value-of select="@subaccid" />
                </td>

              </xsl:otherwise>
            </xsl:choose>
          </tr>
        </table>

      </xsl:when>
      <xsl:otherwise>
        <table class="tblDesc">
          <xsl:for-each select="info">
            <tr>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetLogResource('subaccname')" />
              </td>
              <td class="colW2" colspan="2">
                <xsl:value-of select="@subaccname" />
              </td>
              <td class="colW1">
                <xsl:value-of select="xslLogFormat:GetLogResource('ip')" />
              </td>
              <td class="colW2" colspan="2">
                <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
              </td>
            </tr>
          </xsl:for-each>
          <xsl:for-each select="s">
            <tr class="headerDetail" onclick="slideToggle(this);" >
              <td colspan="6">
                <xsl:value-of select="xslLogFormat:GetLogResource('s')" />
              </td>
            </tr>
            <tr>
              <td colspan="6">
                <div>
                  <table  class="tblDesc" >
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">
                        <xsl:value-of select="xslLogFormat:GetLogResource('soccer')" />
                      </td>
                    </tr>
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >

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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                        <xsl:value-of select="xslLogFormat:GetLogResource('numbergame')" />
                      </td>
                    </tr>
                    <tr >
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
                    <tr >
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
                    <tr >
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
                        <xsl:value-of select="xslLogFormat:GetLogResource('OtherSports')" />
                      </td>
                    </tr>
                    <tr >
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
                    <tr >
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
                      <td class="colW2">

                      </td>
                    </tr>

                    <tr class="headerSportTypeDetail">
                      <td colspan="6">
                        <xsl:value-of select="xslLogFormat:GetLogResource('mixsports')" />
                      </td>
                    </tr>
                    <tr >
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
                      <td class="colW2">

                      </td>

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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >

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
                    <tr >


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
                    <tr >
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
                    <tr >

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
                    <tr >
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
                        <xsl:value-of select="xslLogFormat:GetLogResource('numbergame')" />
                      </td>
                    </tr>
                    <tr >
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
                    <tr >
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

                    <tr >
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


                    <tr class="headerSportTypeDetail">
                      <td colspan="6">
                        <xsl:value-of select="xslLogFormat:GetLogResource('OtherSports')" />
                      </td>
                    </tr>
                    <tr >
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
                    <tr >
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
                      <td class="colW2">

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
                <xsl:value-of select="xslLogFormat:GetLogResource('ia')" />
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >

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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                        <xsl:value-of select="xslLogFormat:GetLogResource('numbergame')" />
                      </td>
                    </tr>
                    <tr >
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
                    <tr >
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
                    <tr >
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

                    <tr class="headerSportTypeDetail">
                      <td colspan="6">
                        <xsl:value-of select="xslLogFormat:GetLogResource('OtherSports')" />
                      </td>
                    </tr>
                    <tr >
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
                    <tr >
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
                    <tr >
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
                      <td class="colW2">

                      </td>

                    </tr>
                  </table>
                </div>
              </td>
            </tr>


          </xsl:for-each>

        </table>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>



</xsl:stylesheet>

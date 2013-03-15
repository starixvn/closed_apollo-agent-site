<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="acc_customerupdmember_master">
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
              <xsl:value-of select="xslLogFormat:GetLogResource('allowupdpt')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@allowupdpt" />
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
            <td class="colTitle"></td>
            <td class="colValue">

            </td>
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
          <xsl:for-each select="m">
            <tr class="headerDetail" onclick="slideToggle(this);">
              <td colspan="6">
                <xsl:value-of select="xslLogFormat:GetLogResource('m')" />
              </td>
            </tr>
            <tr>
              <td colspan="6">
                <div>
                  <table class="tblDesc" >
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">
                        <xsl:value-of select="xslLogFormat:GetLogResource('soccer')" />
                      </td>
                    </tr>
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr >
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
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">
                        <xsl:value-of select="xslLogFormat:GetLogResource('Motosports')" />
                      </td>
                    </tr>
                    <tr >
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
                        <xsl:value-of select="xslLogFormat:GetLogResource('numbergame')" />
                      </td>
                    </tr>
                    <tr >

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
                    <tr >

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
                    <tr >

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
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">
                        <xsl:value-of select="xslLogFormat:GetLogResource('OtherSports')" />
                      </td>
                    </tr>
                    <tr >

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
                    <tr >

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
                    <tr >

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
                      <td class="colW2">

                      </td>
                    </tr>
                  </table>
                </div>
              </td>
            </tr>


          </xsl:for-each>
          <xsl:for-each select="iame">
            <tr class="headerDetail" onclick="slideToggle(this);">
              <td colspan="6" >
                <xsl:value-of select="xslLogFormat:GetLogResource('iame')" />
              </td>
            </tr>
            <tr>
              <td colspan="6">
                <div style="display:none;">
                  <table class="tblDesc">
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">
                        <xsl:value-of select="xslLogFormat:GetLogResource('soccer')" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_mpt_mem_1')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_mpt_mem_1" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_mpt_mem_2')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_mpt_mem_2" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_mpt_mem_3')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_mpt_mem_3" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_mpt_mem_4')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_mpt_mem_4" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_mpt_mem_5')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_mpt_mem_5" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_mpt_mem_6')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_mpt_mem_6" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_mpt_mem_7')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_mpt_mem_7" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_mpt_mem_8')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_mpt_mem_8" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_mpt_mem_9')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_mpt_mem_9" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_mpt_mem_10')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_mpt_mem_10" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_mlivept_mem_1')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_mlivept_mem_1" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_mlivept_mem_3')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_mlivept_mem_3" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_mlivept_mem_7')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_mlivept_mem_7" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_mlivept_mem_8')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_mlivept_mem_8" />
                      </td>
                      <td class="colW1"></td>
                      <td class="colW2">

                      </td>
                    </tr>
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">
                        <xsl:value-of select="xslLogFormat:GetLogResource('basketball')" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v2mpt_mem_1')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v2mpt_mem_1" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v2mpt_mem_2')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v2mpt_mem_2" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v2mpt_mem_3')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v2mpt_mem_3" />
                      </td>
                    </tr>
                    <tr>

                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v2mpt_mem_7')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v2mpt_mem_7" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v2mpt_mem_8')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v2mpt_mem_8" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v2mpt_mem_20')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v2mpt_mem_20" />
                      </td>
                    </tr>






                    <tr class="headerSportTypeDetail">
                      <td colspan="6">
                        <xsl:value-of select="xslLogFormat:GetLogResource('football')" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v3mpt_mem_1')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v3mpt_mem_1" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v3mpt_mem_2')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v3mpt_mem_2" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v3mpt_mem_3')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v3mpt_mem_3" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v3mpt_mem_20')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v3mpt_mem_20" />
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
                        <xsl:value-of select="xslLogFormat:GetLogResource('tennis')" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v5mpt_mem_1')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v5mpt_mem_1" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v5mpt_mem_2')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v5mpt_mem_2" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v5mpt_mem_3')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v5mpt_mem_3" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v5mpt_mem_20')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v5mpt_mem_20" />
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
                        <xsl:value-of select="xslLogFormat:GetLogResource('baseball')" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v8mpt_mem_1')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v8mpt_mem_1" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v8mpt_mem_3')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v8mpt_mem_3" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v8mpt_mem_20')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v8mpt_mem_20" />
                      </td>
                    </tr>
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">
                        <xsl:value-of select="xslLogFormat:GetLogResource('golf')" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v10mpt_mem_1')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v10mpt_mem_1" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v10mpt_mem_2')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v10mpt_mem_2" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v10mpt_mem_3')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v10mpt_mem_3" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v10mpt_mem_20')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v10mpt_mem_20" />
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
                        <xsl:value-of select="xslLogFormat:GetLogResource('motorsports')" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v11mpt_mem_1')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v11mpt_mem_1" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v11mpt_mem_20')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v11mpt_mem_20" />
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
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v161mpt_mem_81')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v161mpt_mem_81" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v161mpt_mem_83')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v161mpt_mem_83" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v161mpt_mem_88')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v161mpt_mem_88" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v161mpt_mem_86')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v161mpt_mem_86" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v161mlivept_mem_85')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v161mlivept_mem_85" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v161mlivept_mem_86')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v161mlivept_mem_86" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v161mlivept_mem_87')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v161mlivept_mem_87" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v161mlivept_mem_89')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v161mlivept_mem_89" />
                      </td>
                      <td class="colW1">
                        <xsl:if test="xslLogFormat:IsVisible('161_90') = 'True'">
                          <xsl:value-of select="xslLogFormat:GetLogResource('isa_v161mlivept_mem_90')" />
                        </xsl:if>
                      </td>
                      <td class="colW2">
                        <xsl:if test="xslLogFormat:IsVisible('161_90') = 'True'">
                          <xsl:value-of select="@isa_v161mlivept_mem_90" />
                        </xsl:if>
                      </td>
                    </tr>
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">
                        <xsl:value-of select="xslLogFormat:GetLogResource('othersports')" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v99mpt_mem_1')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v99mpt_mem_1" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v99mpt_mem_2')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v99mpt_mem_2" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v99mpt_mem_3')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v99mpt_mem_3" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v99mpt_mem_20')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v99mpt_mem_20" />
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
                        <xsl:value-of select="xslLogFormat:GetLogResource('isa_v99mpt_mem_9')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@isa_v99mpt_mem_9" />
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

          <xsl:for-each select="aume">
            <tr class="headerDetail" onclick="slideToggle(this);">
              <td colspan="6" >
                <xsl:value-of select="xslLogFormat:GetLogResource('aume')" />
              </td>
            </tr>
            <tr>
              <td colspan="6">
                <div style="display:none;">
                  <table class="tblDesc">
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">
                        <xsl:value-of select="xslLogFormat:GetLogResource('soccer')" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_mpt_mem_1')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_mpt_mem_1" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_mpt_mem_2')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_mpt_mem_2" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_mpt_mem_3')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_mpt_mem_3" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_mpt_mem_4')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_mpt_mem_4" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_mpt_mem_5')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_mpt_mem_5" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_mpt_mem_6')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_mpt_mem_6" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_mpt_mem_7')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_mpt_mem_7" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_mpt_mem_8')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_mpt_mem_8" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_mpt_mem_9')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_mpt_mem_9" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_mpt_mem_10')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_mpt_mem_10" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_mlivept_mem_1')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_mlivept_mem_1" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_mlivept_mem_3')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_mlivept_mem_3" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_mlivept_mem_7')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_mlivept_mem_7" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_mlivept_mem_8')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_mlivept_mem_8" />
                      </td>
                      <td class="colW1"></td>
                      <td class="colW2">
                      </td>
                    </tr>
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">
                        <xsl:value-of select="xslLogFormat:GetLogResource('basketball')" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v2mpt_mem_1')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v2mpt_mem_1" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v2mpt_mem_2')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v2mpt_mem_2" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v2mpt_mem_3')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v2mpt_mem_3" />
                      </td>
                    </tr>
                    <tr>

                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v2mpt_mem_7')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v2mpt_mem_7" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v2mpt_mem_8')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v2mpt_mem_8" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v2mpt_mem_20')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v2mpt_mem_20" />
                      </td>
                    </tr>

                    <tr class="headerSportTypeDetail">
                      <td colspan="6">
                        <xsl:value-of select="xslLogFormat:GetLogResource('football')" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v3mpt_mem_1')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v3mpt_mem_1" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v3mpt_mem_2')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v3mpt_mem_2" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v3mpt_mem_3')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v3mpt_mem_3" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v3mpt_mem_20')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v3mpt_mem_20" />
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
                        <xsl:value-of select="xslLogFormat:GetLogResource('tennis')" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v5mpt_mem_1')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v5mpt_mem_1" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v5mpt_mem_2')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v5mpt_mem_2" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v5mpt_mem_3')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v5mpt_mem_3" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v5mpt_mem_20')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v5mpt_mem_20" />
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
                        <xsl:value-of select="xslLogFormat:GetLogResource('baseball')" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v8mpt_mem_1')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v8mpt_mem_1" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v8mpt_mem_3')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v8mpt_mem_3" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v8mpt_mem_20')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v8mpt_mem_20" />
                      </td>
                    </tr>
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">
                        <xsl:value-of select="xslLogFormat:GetLogResource('golf')" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v10mpt_mem_1')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v10mpt_mem_1" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v10mpt_mem_2')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v10mpt_mem_2" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v10mpt_mem_3')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v10mpt_mem_3" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v10mpt_mem_20')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v10mpt_mem_20" />
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
                        <xsl:value-of select="xslLogFormat:GetLogResource('motorsports')" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v11mpt_mem_1')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v11mpt_mem_1" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v11mpt_mem_20')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v11mpt_mem_20" />
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
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v161mpt_mem_81')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v161mpt_mem_81" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v161mpt_mem_83')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v161mpt_mem_83" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v161mpt_mem_88')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v161mpt_mem_88" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v161mpt_mem_86')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v161mpt_mem_86" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v161mlivept_mem_85')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v161mlivept_mem_85" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v161mlivept_mem_86')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v161mlivept_mem_86" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v161mlivept_mem_87')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v161mlivept_mem_87" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v161mlivept_mem_89')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v161mlivept_mem_89" />
                      </td>
                      <td class="colW1">
                        <xsl:if test="xslLogFormat:IsVisible('161_90') = 'True'">
                          <xsl:value-of select="xslLogFormat:GetLogResource('a_v161mlivept_mem_90')" />
                        </xsl:if>
                      </td>
                      <td class="colW2">
                        <xsl:if test="xslLogFormat:IsVisible('161_90') = 'True'">
                          <xsl:value-of select="@a_v161mlivept_mem_90" />
                        </xsl:if>
                      </td>
                    </tr>
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">
                        <xsl:value-of select="xslLogFormat:GetLogResource('othersports')" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v99mpt_mem_1')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v99mpt_mem_1" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v99mpt_mem_2')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v99mpt_mem_2" />
                      </td>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v99mpt_mem_3')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v99mpt_mem_3" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v99mpt_mem_20')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v99mpt_mem_20" />
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
                        <xsl:value-of select="xslLogFormat:GetLogResource('a_v99mpt_mem_9')" />
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v99mpt_mem_9" />
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

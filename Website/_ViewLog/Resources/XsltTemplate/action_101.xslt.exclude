<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="acc_customerinsmember_sag">
    <xsl:choose>
      <xsl:when test="@custid">
        <table class="tblDesc">
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('firstname')" /></td>
            <td class="colW2">
              <xsl:value-of select="@fname" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('lastname')" /></td>
            <td class="colW2">
              <xsl:value-of select="@lname" />
            </td>

            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('username')" /></td>
            <td class="colW2">
              <xsl:value-of select="@username" />
            </td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('lblPhone')" /></td>
            <td class="colW2">
              <xsl:value-of select="@phone" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('lblMobilePhone')" /></td>
            <td class="colW2">
              <xsl:value-of select="@mphone" />
            </td>

            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('email')" /></td>
            <td class="colW2">
              <xsl:value-of select="@email" />
            </td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('lblFax')" /></td>
            <td class="colW2">
              <xsl:value-of select="@fax" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('address')" /></td>
            <td class="colW2">
              <xsl:value-of select="@address" />
            </td>

            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('city')" /></td>
            <td class="colW2">
              <xsl:value-of select="@city" />
            </td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('state')" /></td>
            <td class="colW2">
              <xsl:value-of select="@state" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('country')" /></td>
            <td class="colW2">
              <xsl:value-of select="@country" />
            </td>

            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('postal')" /></td>
            <td class="colW2">
              <xsl:value-of select="@postal" />
            </td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('transferoption')" /></td>
            <td class="colW2" >
              <xsl:value-of select="xslLogFormat:TransferConditionFromCode(@transferoption)" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('minb')" /></td>
            <td class="colW2">
              <xsl:value-of select="@minb" />
            </td>

            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('maxb')" /></td>
            <td class="colW2">
              <xsl:value-of select="@maxb" />
            </td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpm')" /></td>
            <td class="colW2">
              <xsl:value-of select="@mpm" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('minbv2')" /></td>
            <td class="colW2">
              <xsl:value-of select="@minbv2" />
            </td>

            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('maxbv2')" /></td>
            <td class="colW2">
              <xsl:value-of select="@maxbv2" />
            </td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpmv2')" /></td>
            <td class="colW2">
              <xsl:value-of select="@mpmv2" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('minbv3')" /></td>
            <td class="colW2">
              <xsl:value-of select="@minbv3" />
            </td>

            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('maxbv3')" /></td>
            <td class="colW2">
              <xsl:value-of select="@maxbv3" />
            </td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpmv3')" /></td>
            <td class="colW2">
              <xsl:value-of select="@mpmv3" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('minbv5')" /></td>
            <td class="colW2">
              <xsl:value-of select="@minbv5" />
            </td>

            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('maxbv5')" /></td>
            <td class="colW2">
              <xsl:value-of select="@maxbv5" />
            </td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpmv5')" /></td>
            <td class="colW2">
              <xsl:value-of select="@mpmv5" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('minbv8')" /></td>
            <td class="colW2">
              <xsl:value-of select="@minbv8" />
            </td>

            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('maxbv8')" /></td>
            <td class="colW2">
              <xsl:value-of select="@maxbv8" />
            </td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpmv8')" /></td>
            <td class="colW2">
              <xsl:value-of select="@mpmv8" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('minbv10')" /></td>
            <td class="colW2">
              <xsl:value-of select="@minbv10" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('maxbv10')" /></td>
            <td class="colW2">
              <xsl:value-of select="@maxbv10" />
            </td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpmv10')" /></td>
            <td class="colW2">
              <xsl:value-of select="@mpmv10" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('minbv11')" /></td>
            <td class="colW2">
              <xsl:value-of select="@minbv11" />
            </td>

            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('maxbv11')" /></td>
            <td class="colW2">
              <xsl:value-of select="@maxbv11" />
            </td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpmv11')" /></td>
            <td class="colW2">
              <xsl:value-of select="@mpmv11" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('minbv99')" /></td>
            <td class="colW2">
              <xsl:value-of select="@minbv99" />
            </td>

            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('maxbv99')" /></td>
            <td class="colW2">
              <xsl:value-of select="@maxbv99" />
            </td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpmv99')" /></td>
            <td class="colW2">
              <xsl:value-of select="@mpmv99" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('credit')" /></td>
            <td class="colW2">
              <xsl:value-of select="@credit" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('groupa')" /></td>
            <td class="colW2">
              <xsl:value-of select="@groupa" />
            </td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('groupb')" /></td>
            <td class="colW2">
              <xsl:value-of select="@groupb" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('groupc')" /></td>
            <td class="colW2">
              <xsl:value-of select="@groupc" />
            </td>

            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('groupd')" /></td>
            <td class="colW2">
              <xsl:value-of select="@groupd" />
            </td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('discountcs')" /></td>
            <td class="colW2">
              <xsl:value-of select="@discountcs" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('discount1x2')" /></td>
            <td class="colW2">
              <xsl:value-of select="@discount1x2" />
            </td>

            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('language')" /></td>
            <td class="colW2">
              <xsl:value-of select="@language" />
            </td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('subaccname')" /></td>
            <td class="colW2">
              <xsl:value-of select="@subaccname" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('suspended')" /></td>
            <td class="colW2">
              <xsl:value-of select="@suspended" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('disablehorseracing')" /></td>
            <td class="colW2">
              <xsl:value-of select="@disablehorseracing" />
            </td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('disablefinancial')" /></td>
            <td class="colW2">
              <xsl:value-of select="@disablefinancial" />
            </td>

            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('disableautompt')" /></td>
            <td class="colW2">
              <xsl:value-of select="@disableautompt" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('ip')" /></td>
            <td class="colW2">
              <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
            </td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('errcode')" /></td>
            <td class="colW2">
              <xsl:value-of select="@errcode" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('errmsg')" /></td>
            <td class="colW2">
              <xsl:value-of select="@errmsg" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('best-odds')" /></td>
            <td class="colW2">
              <xsl:value-of select="@best-odds" />
            </td>
          </tr>

          <xsl:choose>
            <xsl:when test="$IsInternal = 0">

            </xsl:when>
            <xsl:otherwise>
              <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('isinternal0')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@isinternal0" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('creator')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@creator" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('srecommend')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@srecommend" />
                </td>
              </tr>
            </xsl:otherwise>
          </xsl:choose>



        </table>
      </xsl:when>
      <xsl:otherwise>
        <table class="tblDesc">
          <xsl:for-each select="s">
            <tr class="headerDetail" onclick="slideToggle(this)" >
              <td colspan="6"><xsl:value-of select="xslLogFormat:GetLogResource('s')" /></td>
            </tr>
            <tr>
              <td colspan="6">
                <div style="display:none;">
                  <table  class="tblDesc">
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">
                        <xsl:value-of select="xslLogFormat:GetLogResource('soccer')" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('spt_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@spt_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('spt_2')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@spt_2" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('spt_3')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@spt_3" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('spt_4')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@spt_4" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('spt_5')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@spt_5" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('spt_6')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@spt_6" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('spt_7')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@spt_7" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('spt_8')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@spt_8" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('spt_9')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@spt_9" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('spt_10')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@spt_10" />
                      </td>
                      <td class="colW1"></td>
                      <td class="colW2">

                      </td>
                      <td class="colW1"></td>
                      <td class="colW2">

                      </td>
                    </tr>
                    <tr >

                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('slivept_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@slivept_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('slivept_3')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@slivept_3" />
                      </td>

                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('slivept_7')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@slivept_7" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('slivept_8')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@slivept_8" />
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
                        <xsl:value-of select="xslLogFormat:GetLogResource('Basketball')" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v2spt_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v2spt_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v2spt_2')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v2spt_2" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v2spt_3')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v2spt_3" />
                      </td>

                    </tr>
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v2spt_7')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v2spt_7" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v2spt_8')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v2spt_8" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v2spt_9')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v2spt_9" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v2spt_20')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v2spt_20" />
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
                        <xsl:value-of select="xslLogFormat:GetLogResource('Football')" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v3spt_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v3spt_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v3spt_2')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v3spt_2" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v3spt_3')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v3spt_3" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v3spt_20')" /></td>
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
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v5spt_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v5spt_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v5spt_2')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v5spt_2" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v5spt_3')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v5spt_3" />
                      </td>
                    </tr>
                    <tr >

                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v5spt_20')" /></td>
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
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v8spt_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v8spt_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v8spt_3')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v8spt_3" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v8spt_20')" /></td>
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
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v10spt_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v10spt_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v10spt_2')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v10spt_2" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v10spt_3')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v10spt_3" />
                      </td>

                    </tr>
                    <tr>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v10spt_20')" /></td>
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
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v11spt_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v11spt_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v11spt_20')" /></td>
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
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v99spt_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v99spt_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v99spt_2')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v99spt_2" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v99spt_3')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v99spt_3" />
                      </td>

                    </tr>
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v99spt_20')" /></td>
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
                  </table>
                </div>
              </td>
            </tr>

          </xsl:for-each>

          <xsl:for-each select="au">
            <tr class="headerDetail" onclick="slideToggle(this);">
              <td colspan="6"><xsl:value-of select="xslLogFormat:GetLogResource('au')" /></td>
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
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_spt_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_spt_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_spt_2')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_spt_2" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_spt_3')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_spt_3" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_spt_4')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_spt_4" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_spt_5')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_spt_5" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_spt_6')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_spt_6" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_spt_7')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_spt_7" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_spt_8')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_spt_8" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_spt_9')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_spt_9" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_spt_10')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_spt_10" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_slivept_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_slivept_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_slivept_3')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_slivept_3" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_slivept_7')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_slivept_7" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_slivept_8')" /></td>
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
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v2spt_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v2spt_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v2spt_2')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v2spt_2" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v2spt_3')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v2spt_3" />
                      </td>

                    </tr>
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v2spt_7')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v2spt_7" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v2spt_8')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v2spt_8" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v2spt_9')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v2spt_9" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v2spt_20')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v2spt_20" />
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
                        <xsl:value-of select="xslLogFormat:GetLogResource('Football')" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v3spt_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v3spt_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v3spt_2')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v3spt_2" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v3spt_3')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v3spt_3" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v3spt_20')" /></td>
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

                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v5spt_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v5spt_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v5spt_2')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v5spt_2" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v5spt_3')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v5spt_3" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v5spt_20')" /></td>
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


                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v8spt_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v8spt_1" />
                      </td>

                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v8spt_3')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v8spt_3" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v8spt_20')" /></td>
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
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v10spt_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v10spt_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v10spt_2')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v10spt_2" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v10spt_3')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v10spt_3" />
                      </td>
                    </tr>
                    <tr >

                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v10spt_20')" /></td>
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
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v11spt_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v11spt_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v11spt_20')" /></td>
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
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v99spt_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v99spt_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v99spt_2')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v99spt_2" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v99spt_3')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@a_v99spt_3" />
                      </td>

                    </tr>
                    <tr>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('a_v99spt_20')" /></td>
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
                  </table>
                </div>
              </td>
            </tr>
          </xsl:for-each>

          <xsl:for-each select="m">
            <tr class="headerDetail" onclick="slideToggle(this);">
              <td colspan="6"><xsl:value-of select="xslLogFormat:GetLogResource('m')" /></td>
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
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpt_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@mpt_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpt_2')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@mpt_2" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpt_3')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@mpt_3" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpt_4')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@mpt_4" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpt_5')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@mpt_5" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpt_6')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@mpt_6" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpt_7')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@mpt_7" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpt_8')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@mpt_8" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpt_9')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@mpt_9" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpt_10')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@mpt_10" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mlivept_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@mlivept_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mlivept_3')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@mlivept_3" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mlivept_7')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@mlivept_7" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mlivept_8')" /></td>
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
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v2mpt_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v2mpt_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v2mpt_2')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v2mpt_2" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v2mpt_3')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v2mpt_3" />
                      </td>

                    </tr>
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v2mpt_7')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v2mpt_7" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v2mpt_8')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v2mpt_8" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v2mpt_9')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v2mpt_9" />
                      </td>

                    </tr>
                    <tr>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v2mpt_20')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v2mpt_20" />
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
                        <xsl:value-of select="xslLogFormat:GetLogResource('Football')" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v3mpt_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v3mpt_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v3mpt_2')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v3mpt_2" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v3mpt_3')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v3mpt_3" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v3mpt_20')" /></td>
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
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v5mpt_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v5mpt_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v5mpt_2')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v5mpt_2" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v5mpt_3')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v5mpt_3" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v5mpt_20')" /></td>
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
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v8mpt_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v8mpt_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v8mpt_3')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v8mpt_3" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v8mpt_20')" /></td>
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
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v10mpt_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v10mpt_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v10mpt_2')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v10mpt_2" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v10mpt_3')" /></td>
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
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v11mpt_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v11mpt_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v11mpt_20')" /></td>
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
                    <tr >

                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v99mpt_1')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v99mpt_1" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v99mpt_2')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v99mpt_2" />
                      </td>
                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v99mpt_3')" /></td>
                      <td class="colW2">
                        <xsl:value-of select="@v99mpt_3" />
                      </td>
                    </tr>
                    <tr >

                      <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('v99mpt_20')" /></td>
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

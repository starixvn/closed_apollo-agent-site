<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="acc_customerinsmember_ag">
    <xsl:choose>
      <xsl:when test="@custid">
        <table class="tblDesc">
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('fname')" /></td>
            <td class="colW2">
              <xsl:value-of select="@fname" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('lname')" /></td>
            <td class="colW2">
              <xsl:value-of select="@lname" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('username')" /></td>
            <td class="colW2">
              <xsl:value-of select="@username" />
            </td>
          </tr>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('phone')" /></td>
            <td class="colW2">
              <xsl:value-of select="@phone" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mphone')" /></td>
            <td class="colW2">
              <xsl:value-of select="@mphone" />
            </td>
			  <td class="colW1">
				  <xsl:value-of select="xslLogFormat:GetLogResource('copiedFromUsername')" />
			  </td>
			  <td class="colW2">
				  <xsl:value-of select="@copyfromusername" />
			  </td>  
          </tr>
          <tr>
			<td class="colW1">
				<xsl:value-of select="xslLogFormat:GetLogResource('fax')" />
			</td>
			<td class="colW2">
				<xsl:value-of select="@fax" />
			</td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('answer')" /></td>
            <td class="colW2">
              <xsl:value-of select="@answer" />
            </td>

            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('credit')" /></td>
            <td class="colW2">
              <xsl:value-of select="@credit" />
            </td>
          </tr>
          <tr>
			<td class="colW1">
				<xsl:value-of select="xslLogFormat:GetLogResource('ugroup')" />
			</td>
			<td class="colW2">
				<xsl:value-of select="@ugroup" />
			</td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('IP')" /></td>
            <td class="colW2">
              <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
            </td>

            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('success')" /></td>
            <td class="colW2">
              <xsl:value-of select="@success" />
            </td>
          </tr>
          <tr>
			<td class="colW1">
				<xsl:value-of select="xslLogFormat:GetLogResource('Language')" />
			</td>
			<td class="colW2">
				<xsl:value-of select="@language" />
			</td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('SubAccName')" /></td>
            <td class="colW2">
              <xsl:value-of select="@subaccname" />
            </td>

            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('ErrCode')" /></td>
            <td class="colW2">
              <xsl:value-of select="@errcode" />
            </td>
            
          </tr>
          <tr>
			<td class="colW1">
				<xsl:value-of select="xslLogFormat:GetLogResource('ErrMsg')" />
			</td>
			<td class="colW2" colspan="5">
				<xsl:value-of select="@errmsg" />
			</td>
          </tr>
          <xsl:choose>
            <xsl:when test="@minb">
              <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('discount')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@discount" />
                </td>

                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('discountcs')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@discountcs" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('discount1x2')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@discount1x2" />
                </td>
              </tr>
              <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('playerdiscount')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@playerdiscount" />
                </td>

                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('playerdiscountcs')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@playerdiscountcs" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('playerdiscount1x2')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@playerdiscount1x2" />
                </td>
              </tr>
              <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('minb')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@minb" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('maxb')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@maxb" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpm')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@mpm" />
                </td>
              </tr>
              <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('minbv2')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@minbv2" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('maxbv2')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@maxbv2" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpmv2')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@mpmv2" />
                </td>
              </tr>
              <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('minbv3')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@minbv3" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('maxbv3')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@maxbv3" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpmv3')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@mpmv3" />
                </td>
              </tr>
              <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('minbv5')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@minbv5" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('maxbv5')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@maxbv5" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpmv5')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@mpmv5" />
                </td>
              </tr>
              <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('minbv8')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@minbv8" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('maxbv8')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@maxbv8" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpmv8')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@mpmv8" />
                </td>
              </tr>
              <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('minbv10')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@minbv10" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('maxbv10')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@maxbv10" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpmv10')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@mpmv10" />
                </td>
              </tr>
              <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('minbv11')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@minbv11" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('maxbv11')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@maxbv11" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpmv11')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@mpmv11" />
                </td>
              </tr>
              <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('minbv99')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@minbv99" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('maxbv99')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@maxbv99" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mpmv99')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@mpmv99" />
                </td>
              </tr>
              <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('closed')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@closed" />
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
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('disableoutright')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@disableoutright" />
                </td>
                <td class="colW1"></td>
                <td class="colW2">

                </td>
              </tr>

            </xsl:when>
          </xsl:choose>
          
          <xsl:choose>
            <xsl:when test="$IsInternal = 0">
            </xsl:when>
            <xsl:otherwise>
              <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('creator')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@creator" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('recommend')" /></td>
                <td class="colW2">
                  <xsl:value-of select="@recommend" />
                </td>
                <td class="colW1"></td>
                <td class="colW2">

                </td>
              </tr>
            </xsl:otherwise>
          </xsl:choose>
        </table>
      </xsl:when>
      <xsl:otherwise>
        <table class="tblDesc">
          <xsl:for-each select="ag">
            <tr class="headerDetail" onclick="slideToggle(this);">
              <td><xsl:value-of select="xslLogFormat:GetLogResource('ag')" /></td>
            </tr>
            <tr>
              <td>
                <div style="display:none;">
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
                      <td colspan="2"></td>
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
                      <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v2apt_9')" /></td>
                      <td class="colValue">
                        <xsl:value-of select="@v2apt_9" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colTitle"><xsl:value-of select="xslLogFormat:GetLogResource('v2apt_20')" /></td>
                      <td class="colValue">
                        <xsl:value-of select="@v2apt_20" />
                      </td>
                      <td class="colTitle"></td>
                      <td class="colValue">

                      </td>
                      <td class="colTitle"></td>
                      <td class="colValue">

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

<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="acc_customerupdmember_commissionhr">
    <xsl:choose>
      <xsl:when test="@v151discount_31">
        <table class="tblDesc">
          <xsl:if test="xslLogFormat:IsVisible('151_41') = 'True'">
            <tr>
              <td class="colW1" colspan="3">
                <xsl:value-of select="xslLogFormat:GetLogResource('v151discount_41')" />
              </td>
              <td class="colW2" colspan="3">
                <xsl:value-of select="@v151discount_41" />
              </td>
            </tr>
          </xsl:if>
          <xsl:if test="xslLogFormat:IsVisible('151_31') = 'True'">
            <tr>
              <td class="colW1" colspan="3">
                <xsl:value-of select="xslLogFormat:GetLogResource('v151discount_31')" />
              </td>
              <td class="colW2" colspan="3">
                <xsl:value-of select="@v151discount_31" />
              </td>
            </tr>
          </xsl:if>
          <xsl:if test="xslLogFormat:IsVisible('151_51') = 'True'">
            <tr>
              <td class="colW1" colspan="3">
                <xsl:value-of select="xslLogFormat:GetLogResource('v151discount_51')" />
              </td>
              <td class="colW2" colspan="3">
                <xsl:value-of select="@v151discount_51" />
              </td>
            </tr>
          </xsl:if>
          <xsl:if test="xslLogFormat:IsVisible('152_41') = 'True'">
            <tr>
              <td class="colW1" colspan="3">
                <xsl:value-of select="xslLogFormat:GetLogResource('v152discount_41')" />
              </td>
              <td class="colW2" colspan="3">
                <xsl:value-of select="@v152discount_41" />
              </td>
            </tr>
          </xsl:if>
          <xsl:if test="xslLogFormat:IsVisible('152_51') = 'True'">
            <tr>
              <td class="colW1" colspan="3">
                <xsl:value-of select="xslLogFormat:GetLogResource('v152discount_51')" />
              </td>
              <td class="colW2" colspan="3">
                <xsl:value-of select="@v152discount_51" />
              </td>
            </tr>
          </xsl:if>
          <xsl:if test="xslLogFormat:IsVisible('153_51') = 'True'">
            <tr>
              <td class="colW1" colspan="3">
                <xsl:value-of select="xslLogFormat:GetLogResource('v153discount_51')" />
              </td>
              <td class="colW2" colspan="3">
                <xsl:value-of select="@v153discount_51" />
              </td>
            </tr>
          </xsl:if>

          <tr>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('subaccname')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@subaccname" />
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
            <td class="colTitle"></td>
            <td class="colValue">
            </td>
          </tr>
        </table>
      </xsl:when>
      <xsl:otherwise>
        <table class="tblDesc">
          <tr>

            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('ukcomm')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@ukcomm" />
            </td>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('othercomm')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@othercomm" />
            </td>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('subaccname')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@subaccname" />
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
            <td class="colW1"></td>
            <td class="colW2">

            </td>
            <td class="colW1"></td>
            <td class="colW2">

            </td>
          </tr>
        </table>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

</xsl:stylesheet>

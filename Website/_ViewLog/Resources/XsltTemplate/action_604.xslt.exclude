<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" 
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:ms="urn:schemas-microsoft-com:xslt"
                xmlns:dt="urn:schemas-microsoft-com:datatypes"
                xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="acc_customerupduserinfo">
    <table class="tblDesc">
      <tr>

        <td class="colW1">
          <xsl:value-of select="xslLogFormat:GetLogResource('fname')" /></td>
        <td class="colW2">
          <xsl:value-of select="@fname" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('lname')" /></td>
        <td class="colW2">
          <xsl:value-of select="@lname" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('subaccname')" /></td>
        <td class="colW2">
          <xsl:value-of select="@subaccname" />
        </td>
      </tr>
      <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('address')" /></td>
        <td class="colW2">
          <xsl:value-of select="@address" />
        </td>

        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('postal')" /></td>
        <td class="colW2">
          <xsl:value-of select="@postal" />
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
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('phone')" /></td>
        <td class="colW2">
          <xsl:value-of select="@phone" />
        </td>
      </tr>
      <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('mphone')" /></td>
        <td class="colW2">
          <xsl:value-of select="@mphone" />
        </td>

        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('fax')" /></td>
        <td class="colW2">
          <xsl:value-of select="@fax" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('email')" /></td>
        <td class="colW2">
          <xsl:value-of select="@email" />
        </td>
      </tr>
      <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('birthday')" /></td>
        <td class="colW2">
          <xsl:value-of select="substring(@birthday, 1, 12)" />
        </td>

        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('refcode1')" /></td>
        <td class="colW2">
          <xsl:value-of select="@refcode1" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('refcode2')" /></td>
        <td class="colW2">
          <xsl:value-of select="@refcode2" />
        </td>
      </tr>
      <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('remark')" /></td>
        <td class="colW2">
          <xsl:value-of select="@remark" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('gender')" /></td>
        <td class="colW2">
          <xsl:value-of select="@gender" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Language')" /></td>
        <td class="colW2">
          <xsl:value-of select="@language" />
        </td>
       
      </tr>      
      <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('IP')" /></td>
        <td class="colW2">
          <xsl:value-of select="xslLogFormat:FormatIp(@ip)" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('ErrCode')" /></td>
        <td class="colW2">
          <xsl:value-of select="@errcode" />
        </td>

        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('ErrMsg')" /></td>
        <td class="colW2">
          <xsl:value-of select="@errmsg" />
        </td>
        
      </tr>

      <xsl:choose>
        <xsl:when test="$IsInternal = 0">

        </xsl:when>
        <xsl:otherwise>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('SubAccID')" /></td>
            <td class="colW2">

              <xsl:value-of select="@subaccid" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('important')" /></td>
            <td class="colW2">
              <xsl:value-of select="@important" />
            </td>

            <td class="colW1"></td>
            <td class="colW2">

            </td>
          </tr>
        </xsl:otherwise>
      </xsl:choose>

    </table>
  </xsl:template>

</xsl:stylesheet>

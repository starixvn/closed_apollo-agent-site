<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
  <xsl:param name="IsInternal" select="0"></xsl:param>
  <xsl:output method="html" indent="no" />

  <xsl:template match="dot_dep_customerinsmember_ag">
    <table class="tblDesc">
      <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('FirstName')" /></td>
        <td class="colW2">
          <xsl:value-of select="@fname" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('lname')" /></td>
        <td class="colW2">
          <xsl:value-of select="@lname" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Email')" /></td>
        <td class="colW2">
          <xsl:value-of select="@email" />
        </td>
      </tr>
      <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('DepositUserName')" /></td>
        <td class="colW2">
          <xsl:value-of select="@depositusername" />
        </td>

        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('Username')" /></td>
        <td class="colW2">
          <xsl:value-of select="@username" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('OddsType')" /></td>
        <td class="colW2">
          <xsl:value-of select="@oddstype" />
        </td>
      </tr>
      <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('discount')" /></td>
        <td class="colW2">
          <xsl:value-of select="@discount" />
        </td>

        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MinBet')" /></td>
        <td class="colW2">
          <xsl:value-of select="@minbet" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('maxbet')" /></td>
        <td class="colW2">
          <xsl:value-of select="@maxbet" />
        </td>
      </tr>
      <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MaxPerMatch')" /></td>
        <td class="colW2">
          <xsl:value-of select="@maxpermatch" />
        </td>

        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('agentpt')" /></td>
        <td class="colW2">
          <xsl:value-of select="@agentpt" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('suspended')" /></td>
        <td class="colW2">
          <xsl:value-of select="@suspended" />
        </td>
      </tr>
      <tr>

        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('BonusCode')" /></td>
        <td class="colW2">
          <xsl:value-of select="@bonuscode" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('ugroup')" /></td>
        <td class="colW2">
          <xsl:value-of select="@ugroup" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('success')" /></td>
        <td class="colW2">
          <xsl:value-of select="@success" />
        </td>
      </tr>
      <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('ErrCode')" /></td>
        <td class="colW2">
          <xsl:value-of select="@errcode" />
        </td>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('ErrMsg')" /></td>
        <td class="colW2">
          <xsl:value-of select="@errmsg" />
        </td>

        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('DisableHorseRacing')" /></td>
        <td class="colW2">
          <xsl:value-of select="@disablehorseracing" />
        </td>
      </tr>
     

      <xsl:choose>
        <xsl:when test="$IsInternal = 0">
         

        </xsl:when>
        <xsl:otherwise>
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('DepositUserID')" /></td>
            <td class="colW2">

              <xsl:value-of select="@deposituserid" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('AgentID')" /></td>
            <td class="colW2">

              <xsl:value-of select="@agentid" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('AffiliateID')" /></td>
            <td class="colW2">

              <xsl:value-of select="@affiliateid" />
            </td>
          </tr>
        </xsl:otherwise>
      </xsl:choose>

      <tr>
        <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('disablefinancial')" /></td>
        <td class="colW2">
          <xsl:value-of select="@disablefinancial" />
        </td>
        <td class="colW1"></td>
        <td class="colW2">

        </td>

        <td class="colW1"></td>
        <td class="colW2">

        </td>
      </tr>


    </table>

  </xsl:template>
</xsl:stylesheet>

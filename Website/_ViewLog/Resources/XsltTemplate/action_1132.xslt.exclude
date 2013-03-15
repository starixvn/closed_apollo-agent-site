<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
    <xsl:param name="IsInternal" select="0"></xsl:param>
    <xsl:output method="html" indent="no" />
    <xsl:template match="acc_customerupddisablep2p">
        <table class="tblDesc">
          <tr>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('DisabledP2P')" /></td>
            <td class="colW2">
                <xsl:value-of select="@disablep2p" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('DisableP2PDownline')" /></td>
            <td class="colW2">
                <xsl:value-of select="@disablep2pdownlines" />
            </td>
            <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('subaccname')" /></td>
            <td class="colW2">
                <xsl:value-of select="@subaccname" />
            </td>
          </tr>
          <tr>
              <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('DisabledFunP2P')" /></td>
              <td class="colW2">
                  <xsl:value-of select="@disablefunp2p" />
              </td>
              <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('DisableFunP2PDownline')" /></td>
              <td class="colW2">
                  <xsl:value-of select="@disablefunp2pdownlines" />
              </td>

              <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('language')" /></td>
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
    </xsl:template>
</xsl:stylesheet>
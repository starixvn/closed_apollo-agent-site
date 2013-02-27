<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslLogFormat="urn:LogFormat">
    <xsl:param name="IsInternal" select="0"></xsl:param>
    <xsl:output method="html" indent="no" />

    <xsl:template match="acc_customerinsmember_com">
        <table class="tblDesc">
            <tr>
                <!--<td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('custid')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@custid" />
                </td>-->
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MinB')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@minb" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MaxB')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@maxb" />
                </td>
            
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MPM')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@mpm" />
                </td></tr>
            <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MinBv2')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@minbv2" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MaxBv2')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@maxbv2" />
                </td>
           
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MPMv2')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@mpmv2" />
                </td> </tr>
            <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MinBv3')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@minbv3" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MaxBv3')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@maxbv3" />
                </td>
           
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MPMv3')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@mpmv3" />
                </td> </tr>
            <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MinBv5')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@minbv5" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MaxBv5')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@maxbv5" />
                </td>
          
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MPMv5')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@mpmv5" />
                </td>  </tr>
            <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MinBv8')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@minbv8" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MaxBv8')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@maxbv8" />
                </td>
          
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MPMv8')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@mpmv8" />
                </td>  </tr>
            <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MinBv10')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@minbv10" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MaxBv10')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@maxbv10" />
                </td>
           
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MPMv10')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@mpmv10" />
                </td> </tr>
            <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MinBv11')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@minbv11" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MaxBv11')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@maxbv11" />
                </td>
          
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MPMv11')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@mpmv11" />
                </td>  </tr>
            <tr>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MinBv99')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@minbv99" />
                </td>
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MaxBv99')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@maxbv99" />
                </td>
           
                <td class="colW1"><xsl:value-of select="xslLogFormat:GetLogResource('MPMv99')" /></td>
                <td class="colW2">
                    <xsl:value-of select="@mpmv99" />
                </td>
               
            </tr>
          <tr>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('MinBv161')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@minbv161" />
            </td>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('MaxBv161')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@maxbv161" />
            </td>

            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('MPMv161')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@mpmv161" />
            </td>

          </tr>
          <tr>
            <td class="colW1">
              <xsl:value-of select="xslLogFormat:GetLogResource('MPBv161')" />
            </td>
            <td class="colW2">
              <xsl:value-of select="@mpbv161" />
            </td>
            <td class="colW1">
              
            </td>
            <td class="colW2">
             
            </td>

            <td class="colW1">
              
            </td>
            <td class="colW2">
              
            </td>

          </tr>
        </table>
    </xsl:template>

</xsl:stylesheet>

<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
version="1.0">
  <xsl:output method="html" encoding="iso-8859-1" indent="no" />
  <xsl:template match="listing">
    <html>
      <head>
        <title>Notifications</title>
        <style>
          h1{padding-left:0.25cm;
          color : white;background-color : #0038A8;}
          h3{padding-right:0.25cm;
          color : white;background-color : #0038A8;}
          body{font-family : sans-serif,Arial,Tahoma;
          color : #0038A8;background-color : white;}
          b{color : white;background-color : #0086b2;}
          a{color : #0038A8;} HR{color : #0086b2;}
          td{padding: 3px 10px; text-align: left;}
          th{padding: 3px 10px; text-align: left;}
          tt{font-size:12px;
          font-family: "Lucida Sans Unicode", "Lucida Grande", Sans-Serif;}
          tr.s {background-color : #E6EBF6;}
        </style>
      </head>
      <body>
        <h1>Notifications</h1>
        <hr size="1" />
        <table cellspacing="0" width="100%" cellpadding="2"
        align="center">
          <tr class="s">
            <th>Filename</th>
            <th>Size</th>
            <th>Last Modified</th>
          </tr>
          <xsl:apply-templates select="entries" />
        </table>
        <xsl:apply-templates select="readme" />
        <hr size="1" />
        <h3 align="right">IMIS 5.1</h3>
      </body>
    </html>
  </xsl:template>
  <xsl:template match="entries">
    <xsl:apply-templates select="entry" />
  </xsl:template>
  <xsl:template match="entry">
    <xsl:choose>
      <xsl:when test="position() mod 2 = 0">
        <tr class="s">
          <td>
            <xsl:variable name="urlPath" select="@urlPath" />
            <a href="{$urlPath}">
              <tt>
                <xsl:apply-templates />
              </tt>
            </a>
          </td>
          <td>
            <tt>
              <xsl:value-of select="@size" />
            </tt>
          </td>
          <td>
            <tt>
              <xsl:value-of select="@date" />
            </tt>
          </td>
        </tr>
      </xsl:when>
      <xsl:otherwise>
        <tr>
          <td>
            <xsl:variable name="urlPath" select="@urlPath" />
            <a href="{$urlPath}">
              <tt>
                <xsl:apply-templates />
              </tt>
            </a>
          </td>
          <td>
            <tt>
              <xsl:value-of select="@size" />
            </tt>
          </td>
          <td>
            <tt>
              <xsl:value-of select="@date" />
            </tt>
          </td>
        </tr>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
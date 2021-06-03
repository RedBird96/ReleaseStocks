using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRanking
{
    public class HtmlHelper
    {
        StringBuilder html = new StringBuilder();
        String highlightColor = "#3b5998";

        public HtmlHelper()
        {

        }

        public String GetString()
        {
            return html.ToString();
        }

        public void AddText(String text)
        {
            html.Append(text + "<br>");
        }

        public void OpenTable(int columns, List<String> headers, String title)
        {
            html.Append("<table cellpadding='4' rules='rows' style='border:1px;border-collapse:collapse;color:#000000;background-color:#ffffff'>");
            html.Append("\n<caption style='background-color:#ffffff;color:#000000;margin-bottom:.5em;font-size:18pt;border:0'>" + title + "</caption>\n");
            html.Append("<thead style='100%;color:#ffffff;background-color:" + highlightColor + ";font-weight:bold'>");
            html.Append("\n <tr>\n");
            foreach (String header in headers)
            {
                html.Append("<th scope='col' style='border-left:1px solid darkgrey;border-right:1px solid darkgrey;background-color:" + highlightColor + ";color:#FFFFFF'>" + header + "</th>\n");
            }
            html.Append("</tr>\n</thead><tbody>");
        }

        public void CloseTable()
        {
            html.Append("\n</tbody></table><br>");
        }

        public void OpenRow()
        {
            html.Append("<tr>");
        }

        public void AddCell(String value, bool alignLeft = false)
        {
            if(alignLeft)
                html.Append("<td style='border-left:1px solid darkgrey;border-right:1px solid darkgrey;text-align:left'>\n" + value + "</td>");
            else
                html.Append("<td style='border-left:1px solid darkgrey;border-right:1px solid darkgrey;text-align:right'>\n" + value + "</td>");
        }

        public void CloseRow()
        {
            html.Append("</tr>");
        }
    }
}

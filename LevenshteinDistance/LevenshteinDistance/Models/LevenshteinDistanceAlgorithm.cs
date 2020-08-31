using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace LevenshteinDistance.Models
{
    public static class LevenshteinDistanceAlgorithm
    {
        /// <summary>
        /// Compute the distance between two strings.
        /// </summary>
        public static Tuple<string,int> CalculateLevenshteinDistance(string value1, string value2)
        {
            int[,] arrLevDistMat = new int[value1.Length, value2.Length];
            int left, top, lefttop;
            StringBuilder sbFinalDistance = new StringBuilder();

            for (int row = 0; row <= (arrLevDistMat.GetLength(0) - 1); row++)
            {
                for (int column = 0; column <= (arrLevDistMat.GetLength(1) - 1); column++)
                {
                    left = ((column == 0) ? Math.Max(row, column) :
                        (arrLevDistMat[row, (column - 1)]));

                    top = ((row == 0) ? Math.Max(row, column) :
                        (arrLevDistMat[(row - 1), column]));

                    lefttop = (((row) == 0 || (column == 0)) ? Math.Max(row, column) :
                        (arrLevDistMat[(row - 1), (column - 1)]));

                    arrLevDistMat[row, column] =
                        Math.Min(++left, Math.Min(++top, (value1[row] != value2[column] ?
                        ++lefttop : lefttop)));
                }
            } //Array End

            sbFinalDistance.Append("<table id='tblGrid'><tr valign = 'top'>"); //display
            sbFinalDistance.Append("<td width = 30></td>");

            for (int column = 0; column <= (arrLevDistMat.GetLength(1) - 1); column++)
            {
                sbFinalDistance.Append("<td width = 30 style = 'text-align:center; background-color:lightgray; font-weight:bold;'>" + Convert.ToString(value2[column]) + "</td>");
            }

            sbFinalDistance.AppendLine("</tr>");
            sbFinalDistance.AppendLine("<tr valign = 'top'>");

            for (int row = 0; row <= (arrLevDistMat.GetLength(0) - 1); row++)
            {
                for (int column = 0; column <= (arrLevDistMat.GetLength(1) - 1); column++)
                {
                    if (column == 0)
                        sbFinalDistance.Append("<td width = 30 style = 'text-align:center; background-color:lightgray; font-weight:bold;'>" + Convert.ToString(value1[row]) + "</td>");

                    sbFinalDistance.Append("<td width = 30 style = 'text-align:center;'>" + Convert.ToString(arrLevDistMat[row, column]) + "</td>");
                }

                sbFinalDistance.AppendLine("</tr>");
                sbFinalDistance.AppendLine("<tr>");               
            }

            sbFinalDistance.AppendLine("</table>");            

            //var result = sbFinalDistance.ToString().Replace("\r\n", "<br/>");

            var calculatedDistance = Convert.ToInt32(arrLevDistMat[(arrLevDistMat.GetLength(0) - 1), (arrLevDistMat.GetLength(1) - 1)]);
            return Tuple.Create<string, int>(sbFinalDistance.ToString(), calculatedDistance);
        }
    }
}
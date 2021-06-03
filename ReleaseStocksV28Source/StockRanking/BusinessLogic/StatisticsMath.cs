using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace StockRanking
{
    public class StatisticsMath
    {  
        public static List<DataPoint> GenerateLinearBestFit(List<DataPoint> points, out double a, out double b)
        {
            int numPoints = points.Count;
            double meanX = points.Average(point => point.XValue);
            double meanY = points.Average(point => point.YValues[0]);

            double sumXSquared = points.Sum(point => point.XValue * point.XValue);
            double sumXY = points.Sum(point => point.XValue * point.YValues[0]);

            a = (sumXY / numPoints - meanX * meanY) / (sumXSquared / numPoints - meanX * meanX);
            b = (a * meanX - meanY);

            double a1 = a;
            double b1 = b;

            return points.Select(point => new DataPoint(point.XValue, a1 * point.XValue - b1 )).ToList();
        }

        public static double GenerateR2(List<System.Drawing.PointF> points)
        {
            try
            {
                double SumX = 0.00, SumY = 0.00,
                       SumXSquare = 0.00, SumYSquare = 0.00,
                       SumXY = 0.00, r = 0.00;

                foreach (System.Drawing.PointF sp in points)
                {
                    SumX += sp.X;
                    SumY += sp.Y;
                    SumXSquare += (sp.X * sp.X);
                    SumYSquare += (sp.Y * sp.Y);
                    SumXY += (sp.X * sp.Y);
                }

                double NTimesSumXY = points.Count * SumXY;
                double SumXTimesSumY = SumX * SumY;
                double SquareRoot1 = points.Count * SumXSquare - Math.Pow(SumX, 2);
                double SquareRoot2 = points.Count * SumYSquare - Math.Pow(SumY, 2);

                r = (NTimesSumXY - SumXTimesSumY) / (Math.Sqrt(SquareRoot1) *
                                     Math.Sqrt(SquareRoot2));

                return Math.Pow(r, 2);
            }
            catch(Exception)
            {
                return 0;
            }
        }

        public static decimal CalcCorrelation(List<decimal> values1, List<decimal> values2)
        {
            if (values1.Count != values2.Count)
                throw new ArgumentException("Correlation calculation, values must be the same length");

            var avg1 = values1.Average();
            var avg2 = values2.Average();

            var sum1 = values1.Zip(values2, (x1, y1) => (x1 - avg1) * (y1 - avg2)).Sum();

            var sumSqr1 = values1.Sum(x => Math.Pow((double)(x - avg1), 2.0));
            var sumSqr2 = values2.Sum(y => Math.Pow((double)(y - avg2), 2.0));

            var div = (decimal)Math.Sqrt((double)sumSqr1 * (double)sumSqr2);
            var result = div == 0 ? 0 : sum1 / div;

            return result;
        }

        public static decimal CalcBeta(List<decimal> values1, List<decimal> values2)
        {
            decimal variance = Utils.Variance(values1);
            decimal beta = 0;
            if (variance != 0)
                beta = Utils.Covariance(values1, values2) / variance;

            return beta;
        }
    }
}

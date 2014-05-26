using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils
{
    /// <summary>
    /// From http://en.wikipedia.org/wiki/Algorithms_for_calculating_variance#Online_algorithm
    /// </summary>
    public class OnlineAverage
    {
        public long Count { get; private set; }
        public double Mean { get; private set; }
        private double m2;

        public void Observe(double x)
        {
            this.Count++;
            var delta = x - this.Mean;
            this.Mean += delta / this.Count;
            this.m2 += delta * (x - this.Mean);
        }

        public double GetVariance(bool isSampleVariance = false)
        {
            return this.m2 / (this.Count - (isSampleVariance ? 1 : 0));
        }
        public double GetStandardDeviation(bool isSampleStandardDeviation = false)
        {
            return Math.Sqrt(this.GetVariance(isSampleStandardDeviation));
        }
    }
}

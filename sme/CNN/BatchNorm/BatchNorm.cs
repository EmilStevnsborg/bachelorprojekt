using SME;
using SME.Components;
using System;

namespace CNN
{
    [ClockedProcess]
    public class BatchNorm
    {
        public ValueBus Input
        {
            get => minus.Input;
            set => minus.Input = value;
        }

        public ValueBus Output
        {
            get => plus.Output;
            set => plus.Output = value;
        }

        public BatchNorm(float gamma, float beta, float mean, float var)
        {
            var denominator = (float) Math.Sqrt(var + 0.00001);
            // Instantiate the processes
            minus = new Minus(mean);
            divide = new Divide(denominator);
            multiply = new Multiply(gamma);
            plus = new Plus(beta);

            // Connect the buses
            divide.Numerator = minus.Output;
            multiply.Input = divide.Output;
            plus.Input = multiply.Output;
        }

        // Hold the internal processes as fields
        private Minus minus;
        private Divide divide;
        private Multiply multiply;
        private Plus plus;
    }
}
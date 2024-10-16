using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Class to perform individual math calculations

namespace Calculator___Rowan
{
    internal class MathCalculation
    {
        protected List<double> calculations = new List<double>();
        public MathCalculation(List<double> calculations)
        {
            this.calculations = calculations;
        }

        public virtual double calculate() { return 0; }
    }

    class Plus : MathCalculation {

        public Plus (List<double> calculations) : base (calculations) { }

        public override double calculate()
        {
            double result = 0;
            foreach (var calculation in calculations)
            {
                result += calculation;
            }

            return result;
        }
    }

    class Minus : MathCalculation {

        public Minus(List<double> calculations) : base(calculations) { }
        public override double calculate()
        {
            double result = 0;
            if(calculations.Count > 0)
            {
                result = calculations[0];
            }

            bool isFirst = true;
            foreach (var calculation in calculations)
            {
                if (isFirst)
                {
                    isFirst = false;
                    continue;
                }
                result -= calculation;
            }

            return result;
        }
    }

    class Multiply : MathCalculation {

        public Multiply(List<double> calculations) : base(calculations) { }

        public override double calculate()
        {
            double result = 0;
            if (calculations.Count > 0)
            {
                result = calculations[0];
            }

            bool isFirst = true;
            foreach (var calculation in calculations)
            {
                if (isFirst)
                {
                    isFirst = false;
                    continue;
                }
                result *= calculation;
            }

            return result;
        }
    }

    class Divide : MathCalculation {

        public Divide(List<double> calculations) : base(calculations) { }
        public override double calculate()
        {
            double result = 0;
            if (calculations.Count > 0)
            {
                result = calculations[0];
            }

            bool isFirst = true;
            foreach (var calculation in calculations)
            {
                if (isFirst)
                {
                    isFirst = false;
                    continue;
                }
                result /= calculation;
            }

            return result;
        }
    }
}

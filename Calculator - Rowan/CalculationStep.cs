using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator___Rowan
{
    internal class CalculationStep
    {
        private MathCalculation calculation = null;
        public string displayString = String.Empty;
        
        public CalculationStep(string Operation, List<string> calculations) {
            List<double> convertedCalculations = new List<double>();

            if(calculations != null && Operation != null)
            {
                foreach (string calculation in calculations)
                {
                    convertedCalculations.Add(Convert.ToDouble(calculation));
                }

                switch (Operation)
                {
                    case "Plus":
                        calculation = new Plus(convertedCalculations);
                        displayString = String.Join(" + ", calculations);
                        break;
                    case "Multiplication":
                        calculation = new Multiply(convertedCalculations);
                        displayString = "( " + String.Join(" * ", calculations) + " )";
                        break;
                    case "Minus":
                        calculation = new Minus(convertedCalculations);
                        displayString = "( " + String.Join(" - ", calculations) + " )";
                        break;
                    case "Division":
                        calculation = new Divide(convertedCalculations);
                        displayString = "( " + String.Join(" / ", calculations) + " )";
                        break;
                }
            }
            else
            {
                displayString = "Error: bad data";
            }
        }

        public double calculate()
        {
            if (calculation != null) {
                double calculationTotal = calculation.calculate();
                displayString = calculationTotal.ToString();

                return calculationTotal;
            }

            return 0;
        }
    }
}

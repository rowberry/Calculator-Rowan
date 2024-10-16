using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator___Rowan
{
    public partial class Calculator : Form
    {
        private readonly DataTable calculatorStepsTable = new DataTable();
        private CalculateEvent calculateEvent = new CalculateEvent();

        public Calculator()
        {
            InitializeComponent();

            //Initialize the event handler
            calculateEvent.CalculationEvent += calculationEvent;
            calculateEvent.runListenerThread();
        }

        private void Calculator_Load(object sender, EventArgs e)
        {
            //Set up our data grid
            calculatorStepsTable.Columns.Add(new DataColumn("Step", typeof(String)));
            calculatorStepsTable.Columns.Add(new DataColumn("Calculation", typeof(String)));
            
            CalculatorStepsDataGrid.DataSource = calculatorStepsTable;
            CalculatorStepsDataGrid.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            CalculatorStepsDataGrid.Columns[0].FillWeight = 2;
            CalculatorStepsDataGrid.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            CalculatorStepsDataGrid.Columns[1].FillWeight = 8;
            CalculatorStepsDataGrid.Columns[0].ReadOnly = true;
            CalculatorStepsDataGrid.Columns[1].ReadOnly = true;
        }

        private void calculationEvent(object sender, CalculateEventArgs args)
        {
            //Update the UI
            this.BeginInvoke(new MethodInvoker(delegate
            {
                updateResults(args.result, args.calculations);
            }));
        }

        protected void updateResults(string result, List<String> calculations)
        {
            //Update the result header
            ResultLabel.Text = result;

            //Update the data grid
            calculatorStepsTable.Clear();

            int counter = 0;
            foreach (String calculation in calculations)
            {
                calculatorStepsTable.Rows.Add(counter.ToString(), calculation);
                counter++;
            }
        }
    }
}

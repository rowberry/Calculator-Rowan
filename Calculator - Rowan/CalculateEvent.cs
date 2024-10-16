using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;

namespace Calculator___Rowan
{
    public class CalculateEvent
    {
        public event EventHandler<CalculateEventArgs> CalculationEvent;

        public void runListenerThread()
        {
            //create listener in a new thread so we don't interrupt the UI
            Task.Run(() => { setupListener(); });
        }

        private void setupListener() { 
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/");
            listener.Start();

            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;

                if (request.HttpMethod == "POST")
                {
                    using(StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        string requestBody = reader.ReadToEnd();
                        string responseBody = requestBody;

                        RequestDecoder requestDecoder = new RequestDecoder(requestBody, request.ContentType);
                        if (requestDecoder.error != String.Empty)
                        {
                            responseBody = requestDecoder.error;
                        }
                        else
                        {
                            //We should have a valid JSON or XML response at this point. Lets try and do some math

                            List<CalculationStep> steps = new List<CalculationStep>();
                            try
                            {
                                if (requestDecoder.operations != null)
                                {
                                    //We have a JSON object, decode that
                                    //responseBody = requestDecoder.operations.MyMaths.Operation.ID;
                                    steps = fillSteps(requestDecoder.operations.MyMaths.MyOperation, steps);

                                }
                                else if (requestDecoder.operationsXML != null)
                                {
                                    //We have a XML object, decode that
                                    steps = fillStepsXML(requestDecoder.operationsXML.Operation, steps);
                                }
                                else
                                {
                                    responseBody = "Input form was not in a valid XML or JSON format for this application";
                                }
                            }
                            catch {
                                responseBody = "Input value was not in number format";
                            }

                            if(steps.Count > 0)
                            {
                                List<String> stepDisplay = new List<String>();
                                double calculationTotal = 0;

                                stepDisplay.Add(createDisplayString(steps));

                                for (int i = 0; i < steps.Count; i++)
                                {
                                    calculationTotal += steps[i].calculate();
                                    stepDisplay.Add(createDisplayString(steps));
                                }

                                responseBody = "Result: " + calculationTotal.ToString();

                                CalculateEventArgs eventArgs = new CalculateEventArgs();
                                eventArgs.calculations = stepDisplay;
                                eventArgs.result = responseBody;
                                CalculationEvent.Invoke(this, eventArgs);
                            }
                            else
                            {
                                //responseBody = "Input form did not have any valid data";
                            }
                        }

                        HttpListenerResponse response = context.Response;
                        //string responseString = "<html><body>"+responseBody+"</body></html>";
                        string responseString = responseBody;
                        byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                        response.ContentLength64 = buffer.Length;

                        using (Stream output = response.OutputStream)
                        {
                            output.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
                else if (request.HttpMethod == "GET")
                {
                    using (StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        string requestBody = reader.ReadToEnd();

                        HttpListenerResponse response = context.Response;
                        string responseString = "<html><body>This API only accepts HTTP Post XML and JSON</body></html>";
                        byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                        response.ContentLength64 = buffer.Length;

                        using (Stream output = response.OutputStream)
                        {
                            output.Write(buffer, 0, buffer.Length);
                        }
                    }
                }

                
            }
        }

        private string createDisplayString(List<CalculationStep> steps)
        {
            string displayString = String.Empty;
            char[] charsToTrim = { ' ', '+' };
            foreach (CalculationStep step in steps)
            {
                displayString += (step.displayString + " + ");
            }

            return displayString.TrimEnd(charsToTrim);
        }

        private List<CalculationStep> fillSteps (MyOperation operation, List<CalculationStep> listToFill) {
            List<CalculationStep> stepsToFill = listToFill; 
            //Call this recursively to get all the operations
            if (operation != null)
            {
                stepsToFill.Add(new CalculationStep(operation.ID, operation.Value));

                if(operation.NestedOperation != null)
                {
                    return fillSteps(operation.NestedOperation, stepsToFill);
                }
            }

            return stepsToFill;
        }

        private List<CalculationStep> fillStepsXML(OperationXML operation, List<CalculationStep> listToFill)
        {
            List<CalculationStep> stepsToFill = listToFill;
            //We can just iterate over this list
            if (operation != null)
            {
                stepsToFill.Add(new CalculationStep(operation.ID, operation.Value));

                if (operation.NestedOperations != null)
                {
                    foreach(var nestedOperation  in operation.NestedOperations)
                    {
                        return fillStepsXML(nestedOperation, stepsToFill);
                    }
                }
            }

            return stepsToFill;
        }

    }

    


    public class CalculateEventArgs : EventArgs {
        public List<String> calculations = new List<String>();
        public string result = "";
    }
}

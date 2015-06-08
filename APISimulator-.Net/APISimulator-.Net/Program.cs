using SSClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APISimulator_.Net
{
    class Program
    {
        static void Main(string[] args)
        {
            string tokenURL = "https://{account}.schemeserve.com/api/token/{username}/{password}";
            string casesAPIEndPoint = "https://{account}.schemeserve.com/api/cases";

            //long demoCaseID = ######;

            string requestBodyPath = "requestBody.xml";
            string requestResultFilePath = "result.txt";

            var client = new SchemeServeAPIClient(requestBodyPath, requestResultFilePath);

            client.LoadToken(tokenURL);

            client.SubmitCase(casesAPIEndPoint);

            //client.GetCase(casesAPIEndPoint, demoCaseID);

            Console.WriteLine("Success");
            Console.ReadLine();
        }
    }
}

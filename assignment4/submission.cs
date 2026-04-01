using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace ConsoleApp1
{
    public class Submission
    {

        // These URLs will be read by the autograder, please keep the variable name un-changed and link to the correct xml/xsd
        public static string xmlURL = "https://raw.githubusercontent.com/jacporAZ/assignment4Pages/refs/heads/master/assignment4/NationalParks.xml";//Q1.2
        public static string xmlErrorURL = "https://raw.githubusercontent.com/jacporAZ/assignment4Pages/refs/heads/master/assignment4/NationalParksErrors.xml";//Q1.3
        public static string xsdURL = "https://raw.githubusercontent.com/jacporAZ/assignment4Pages/refs/heads/master/assignment4/NationalParks.xsd"; //Q1.1
        public static void Main(string[] args)
        {
            // Q3: You can pick two of three
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);
            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);
            //Console.WriteLine("Testing");
            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }
        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            //return "No errors are found" if XML is valid. Otherwise, return the desired exception message. 
            // This is the string that we will build our errors on
            StringBuilder sb = new StringBuilder();
               
            // try for validation
            try
            {
                // create the schema set
                XmlSchemaSet schemaSet = new XmlSchemaSet();
                // add the xsd url to the schema set
                schemaSet.Add(null, xsdUrl);
                // add settings for validation
                XmlReaderSettings settings = new XmlReaderSettings();
                // add validation type to the schema and settings
                settings.ValidationType = ValidationType.Schema; 
                settings.Schemas = schemaSet;

                // function for validations
                settings.ValidationEventHandler += (sender, e) =>
                {
                    // if there are any errors, add them to the sb string 
                    if (sb.Length > 0)
                    {
                        sb.AppendLine();
                    }
                    sb.Append(e.Severity + ": " + e.Message);
                };

                // read from the reader that is tied to the xml url
                using (XmlReader reader = XmlReader.Create(xmlUrl, settings))
                {
                    while (reader.Read()) { }

                }
            } // catch any exceptions or errors
            catch (Exception ex)
            {
                return ex.Message;

            }
            // if the string that has any error is greater than 0, return the appeneded string, if not, no errors found
            return sb.Length > 0 ? sb.ToString() : "No errors are found";
        }

        // Q2.2
        public static string Xml2Json(string xmlUrl)
        {
            // Create an xmldoc object to load the xml from the specified url
            XmlDocument xmlDoc = new XmlDocument();

            // add settings for the reader to read
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            // yusing the reader, read the xml and load into xmldoc object
            using (XmlReader reader = XmlReader.Create(xmlUrl, xmlReaderSettings)) { 
                xmlDoc.Load(reader);
            }

            // return the result of the conversion, serialize for JSON 
            string result = JsonConvert.SerializeXmlNode(xmlDoc.DocumentElement, Newtonsoft.Json.Formatting.Indented, false);
            return result;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace TaxClient
{
    /// <summary>
    /// This is the sample application, so there is not input validation carried out,
    /// Please work on the happy path only :)
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var InputVerbs = new List<string> { "POST", "PUT" };
            while (true)
            {
                try
                {
                    string content;
                    Console.WriteLine("Enter Verb (or exit):");
                    string Verb = Console.ReadLine().ToUpper();
                    if (Verb.Equals("EXIT"))
                    {
                        break;
                    }

                    Console.WriteLine("Enter URI:");
                    string uri = Console.ReadLine();
                    if (!uri.StartsWith("http"))
                    {
                        uri = "http://localhost:3657/Tax/" + uri;
                    }
                    HttpWebRequest req = WebRequest.Create(uri) as HttpWebRequest;
                    req.KeepAlive = false;
                    req.Method = Verb;

                    if (InputVerbs.Contains(Verb))
                    {
                        string filePath = "";
                        if (uri.EndsWith("import"))
                        {
                            filePath = @"..\..\..\Inputs\init.json"; 
                        }
                        else
                        {
                            Console.WriteLine("Enter JSON FilePath:");
                            filePath = Console.ReadLine();
                        }

                        content = (File.OpenText(@filePath)).ReadToEnd();

                        byte[] buffer = Encoding.UTF8.GetBytes(content);
                        req.ContentLength = buffer.Length;
                        req.ContentType = "application/json;charset=utf-8";
                        Stream PostData = req.GetRequestStream();
                        PostData.Write(buffer, 0, buffer.Length);
                        PostData.Close();
                    }
                    HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
                    StreamReader loResponseStream = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
                    string Response = loResponseStream.ReadToEnd();
                    loResponseStream.Close();
                    resp.Close();
                    Console.WriteLine(Response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }

                Console.WriteLine();

            }
        }
    }
}

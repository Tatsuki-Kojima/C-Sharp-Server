using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace C_Sharp_Server.Sample
{
    public class ApiService
    {
        private readonly string API_PATH = "localhost";
        private readonly string API_PORT = "8080";
        private readonly HttpListener listener;

        public ApiService()
        {
            if (!HttpListener.IsSupported)
                throw new InvalidOperationException("HTTP Listenerはサポートされていません");

            listener = new HttpListener();
        }

        public async void Start()
        {
            listener.Prefixes.Add(string.Format($"http://{API_PATH}:{API_PORT}/"));
            listener.Start();

            Console.WriteLine($"Listening... {listener.Prefixes.Count}");

            var context = await listener.GetContextAsync();
            var request = context.Request;
            var response = context.Response;

            await Execute(request, response);

            listener.Stop();
            Console.WriteLine("Stoped");
        }

        private async Task Execute(HttpListenerRequest request, HttpListenerResponse response)
        {
            try
            {
                response.StatusCode = (int)HttpStatusCode.OK;
                response.ContentEncoding = Encoding.UTF8;

                var sr = new StreamReader(request.InputStream);
                var sw = new StreamWriter(response.OutputStream);
                var requestBody = await sr.ReadToEndAsync();

                var responseBody = requestBody;

                Console.WriteLine($"Request {responseBody}");

                await sw.WriteAsync("OK! Readed! ");
                await sw.FlushAsync();

                if (sr != null)
                    sr.Close();
                if (sw != null)
                    sw.Close();
            }
            catch (Exception e)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Console.WriteLine($"Error, {e}");
            }
        }
    }

    public class Firebase
    {
        public async void GetFirestore()
        {
            var projectId = "fir-demo-ad96f";
            var client = await FirestoreClient.CreateAsync();
            var db = FirestoreDb.Create(projectId, client);

            CollectionReference c = db.Collection("dev-user");

            var s = await c.Document("sample-user").GetSnapshotAsync();
            if (s.Exists)
            {
                Console.WriteLine($"Document data for {s.Id}");
                foreach (var o in s.ToDictionary())
                {
                    Console.WriteLine($"{o.Key}, {o.Value}");
                }
            }
            else
                Console.WriteLine("Not Exist");
        }
    }
}

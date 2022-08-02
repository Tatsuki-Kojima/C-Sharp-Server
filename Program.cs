using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;

// https://qiita.com/yun_bow/items/da3efd9d99350232dd67

namespace C_Sharp_Server
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // var c = new C();
            // c.FF();

            var api = new ApiService();
            api.Start();

            Console.ReadLine();
        }
    }

    public class ApiService
    {
        private readonly string API_PATH = "localhost";
        private readonly string API_PORT = "8080";
        private readonly HttpListener listener;

        public ApiService()
        {
            listener = new HttpListener();
        }

        public void Start()
        {
            listener.Prefixes.Add(string.Format($"http://{API_PATH}:{API_PORT}/"));
            listener.Start();

            Console.WriteLine($"{listener.Prefixes.Count}");

            while (listener.IsListening)
            {
                var result = listener.BeginGetContext(OnRequested, listener);
            }
        }

        private void OnRequested(IAsyncResult result)
        {
            var listener = result.AsyncState as HttpListener;

            if (!listener.IsListening)
                return;

            var context = listener.EndGetContext(result);
            var request = context.Request;
            var response = context.Response;

            Execute(request, response);
        }

        private async void Execute(HttpListenerRequest request, HttpListenerResponse response)
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

                await sw.WriteAsync(requestBody);
                await sw.FlushAsync();

                if (sr != null)
                    sr.Close();
                if (sw != null)
                    sw.Close();
            }
            catch(Exception e)
            {
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

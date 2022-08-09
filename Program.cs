using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using C_Sharp_Server.Sample;
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

 
}

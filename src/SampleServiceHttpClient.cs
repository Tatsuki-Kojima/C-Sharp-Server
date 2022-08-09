using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace C_Sharp_Server.Sample
{
    internal class SampleServiceHttpClient
    {
        private static HttpClient HttpClient;

        public SampleServiceHttpClient(string baseURL)
        {
            HttpClient = new HttpClient();
        }
    }
}

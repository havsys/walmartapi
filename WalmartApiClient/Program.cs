﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalmartApiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            WalmartApiClient apiClient = new WalmartApiClient();
            Console.WriteLine(apiClient.search("", "medicines", 10, 100));
            while (true) ;
        }
    }
}

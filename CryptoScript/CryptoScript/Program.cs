﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CryptoScript.Core;

namespace CryptoScript
{
    class Program
    {
        static void Main(string[] args)
        {
            //string scriptPath = args[0]; // get path from arg
            string scriptPath = @"C:\Users\Luca\Documents\Visual Studio 2015\Repos\CryptoScript\CryptoScript\CryptoScript\bin\Debug\test.cy";

            if (File.Exists(scriptPath)) // file exists
            {
                List<string> script = File.ReadAllLines(scriptPath).ToList();

                foreach (string line in script)
                {
                    Script.Analyze(line);
                }
            }
            else
            {
                Console.WriteLine("File does not exist!"); // file does not exist
                Console.Read();
            }
        }
    }
}
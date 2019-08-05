﻿using System;
using System.Diagnostics;
using System.IO;
using SOSIEL.Algorithm;
using SOSIEL_CEMMA;
using SOSIEL_CEMMA.Configuration;
//using SOSIEL_EX1;
//using SOSIEL_EX1.Configuration;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

            Console.WriteLine("Reading configuration");

            string algorithmConfigurationFileName = "configuration.json";
            string algorithmConfigurationFilePath = Path.Combine(Directory.GetCurrentDirectory(), algorithmConfigurationFileName);

            Console.WriteLine($"Reading configuration from { algorithmConfigurationFilePath}");


            if (File.Exists(algorithmConfigurationFilePath) == false)
            {
                throw new FileNotFoundException($"{algorithmConfigurationFileName} not found at {Directory.GetCurrentDirectory()}");
            }

            string outputDirectory = "Output";

            if (Directory.Exists(outputDirectory))
                Directory.Delete(outputDirectory, true);

            ConfigurationModel configuration = ConfigurationParser.ParseConfiguration(algorithmConfigurationFilePath);

            IAlgorithm algorithm = new Algorithm(configuration);

            Console.WriteLine($"{algorithm.Name} algorithm is running....");
            outputDirectory = algorithm.Run();
            Console.WriteLine("Algorithm has completed");

            WaitKeyPress();

            var process = new Process
            {
                StartInfo =
                {
                    UseShellExecute = true,
                    FileName = Path.Combine(Directory.GetCurrentDirectory(), outputDirectory)
                }
            };
            process.Start();
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception;

            if (e.ExceptionObject is AggregateException)
            {
                exception = (e.ExceptionObject as AggregateException).InnerException;
            }
            else
            {
                exception = e.ExceptionObject as Exception;
            }


            Console.WriteLine($"ERROR! {exception.Message} in {exception.Source}");
            Console.WriteLine($"ERROR! {exception.StackTrace}");

            WaitKeyPress();

            Environment.Exit(1);
        }

        private static void WaitKeyPress()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}

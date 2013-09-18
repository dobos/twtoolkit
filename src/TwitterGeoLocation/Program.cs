using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;
using TwitterLib.CommandLineParser;

namespace TwitterGeoLocation
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Type> verbs = new List<Type>() 
            {
                typeof(Cluster),
            };

            Verb v = null;

            try
            {
                PrintHeader();
                v = (Verb)ArgumentParser.Parse(args, verbs);
            }
            catch (ArgumentParserException ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                Console.WriteLine();

                ArgumentParser.PrintUsage(verbs, Console.Out);
            }

            if (v != null)
            {
                v.Run();
            }
        }

        private static void PrintHeader()
        {
            Console.WriteLine(
@"Twitter GPS coordinate clustering utility
(c) 2012-13 János Szüle szule@complex.elte.hu, László Dobos dobos@complex.elte.hu
Department of Physics of Complex Systems, Eötvös Loránd University

");
        }


#if false
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            Console.WriteLine("Press a key to start.");
            Console.ReadKey();
            Console.WriteLine("Started.");
            GeoCalculator gc = new GeoCalculator();
            gc.Start();
            Console.WriteLine("All Finished");
            Console.ReadKey();

        }
#endif
    }
}

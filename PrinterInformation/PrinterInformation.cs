using System;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Reflection;
using System.Threading;

namespace PrinterInformation
{
    class PrinterInformation
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || args[0].Trim().Equals("--help", StringComparison.CurrentCultureIgnoreCase))
            {
                ShowHelp();
            }
            else if ( args.Length == 1 && args[0].Trim().Equals("--listprinters",StringComparison.CurrentCultureIgnoreCase) )
            {
                Console.WriteLine($"Printers on '{System.Environment.MachineName}':");
                foreach (string printer in PrinterSettings.InstalledPrinters)
                {
                    Console.WriteLine($"{printer}");
                }
            }
            else if ( args.Length == 2 && args[0].Trim().StartsWith("--") )
            {
                string printerName = args[1];
                bool pFound = false;
                foreach (string printer in PrinterSettings.InstalledPrinters)
                {
                    if ( printer.Equals(printerName, StringComparison.CurrentCultureIgnoreCase) )
                    {
                        pFound = true;
                        printerName = printer;
                        break;
                    }
                }

                if ( pFound )
                {
                    PrintDocument pd = new PrintDocument();
                    pd.PrinterSettings.PrinterName = printerName;
                    if ( pd.PrinterSettings.IsValid )
                    {
                        if ( args[0].Trim().Equals("--listpapers", StringComparison.CurrentCultureIgnoreCase) ) {
                            Console.WriteLine($"Paper definitions for the printer '{printerName}' on '{System.Environment.MachineName}':");
                            Console.WriteLine($"Paper Name (Kind) (Size in*in) (Size mm*mm)");
                            foreach ( PaperSize papersize in pd.PrinterSettings.PaperSizes )
                            {
                                Console.WriteLine($"{papersize.PaperName} ({papersize.RawKind}) ({(double)papersize.Height / 100.0:N2}x{(double)papersize.Width / 100.0:N2}) ({(double)papersize.Height * 25.4 / 100.0:N2}x{(double)papersize.Width * 25.4 / 100.0:N2})");
                            }
                        }
                        else if (args[0].Trim().Equals("--listbins", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Console.WriteLine($"Paper source (bin) definitions for the printer '{printerName}' on '{System.Environment.MachineName}':");
                            Console.WriteLine($"Paper Source Name (Kind)");
                            foreach (PaperSource papersource in pd.PrinterSettings.PaperSources)
                            {
                                Console.WriteLine($"{papersource.SourceName} ({papersource.RawKind})");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"The printer settings for the printer '{printerName}' are not valid on '{System.Environment.MachineName}'");
                    }
                }
                else
                {
                    Console.WriteLine($"The printer '{printerName}' is not available on '{System.Environment.MachineName}'");
                    
                }
            }
            else
            {
                ShowHelp();
            }
#if DEBUG
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
#endif
        }
        static void ShowHelp()
        {
            Version appVersion = Assembly.GetExecutingAssembly().GetName().Version;
            string appName = Process.GetCurrentProcess().ProcessName;
            Console.WriteLine($"{appName} Version {appVersion}, {DateTime.Now:yyyy'-'MM'-'dd HH':'mm':'ss'.'fff}");
            Console.WriteLine($"\nShow printer information for '{System.Environment.MachineName}':\n");
            Console.WriteLine($"  {appName} --Help\t\t\tTo get this help text.");
            Console.WriteLine($"  {appName} --ListPrinters\t\tTo list all printer names.");
            Console.WriteLine($"  {appName} --ListPapers <PrinterName>\tTo list all paper definitions for the printer <PrinterName>.");
            Console.WriteLine($"  {appName} --ListBins <PrinterName>\tTo list all paper source (bin) definitions for the printer <PrinterName>.");
        }
    }
}

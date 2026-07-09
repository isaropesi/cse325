using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var storesDirectory = Path.Combine(currentDirectory, "stores");

        var salesFiles = FindFiles(storesDirectory);

        var salesTotals = CalculateSalesTotal(salesFiles);

        GenerateSalesSummaryReport(salesTotals, currentDirectory);

        Console.WriteLine("Sales summary report generated successfully.");
    }

    static IEnumerable<string> FindFiles(string folderName)
    {
        List<string> salesFiles = new List<string>();

        if (!Directory.Exists(folderName)) return salesFiles;

        var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

        foreach (var file in foundFiles)
        {
            if (file.EndsWith("sales.json"))
            {
                salesFiles.Add(file);
            }
        }

        return salesFiles;
    }

    static Dictionary<string, double> CalculateSalesTotal(IEnumerable<string> salesFiles)
    {
        var salesTotals = new Dictionary<string, double>();

        foreach (var file in salesFiles)
        {
            // Read the contents of the file
            string salesJson = File.ReadAllText(file);

            // Parse the contents as JSON
            SalesData? data = JsonSerializer.Deserialize<SalesData>(salesJson);

            // Add the amount to the dictionary
            if (data != null)
            {
                salesTotals.Add(file, data.Total);
            }
        }

        return salesTotals;
    }

    static void GenerateSalesSummaryReport(Dictionary<string, double> salesTotals, string currentDirectory)
    {
        double grandTotal = 0;
        var sb = new StringBuilder();

        sb.AppendLine("Sales Summary");
        sb.AppendLine("----------------------------");
        
        // We will build the details section separately first to calculate the grand total
        var detailsSb = new StringBuilder();
        detailsSb.AppendLine(" Details:");

        foreach (var kvp in salesTotals)
        {
            grandTotal += kvp.Value;
            // Use just the file name or a relative path for cleaner output, but the assignment says "filename: $xxx.xx"
            string fileName = Path.GetFileName(kvp.Key);
            detailsSb.AppendLine($"  {fileName}: {kvp.Value.ToString("C")}");
        }

        sb.AppendLine($" Total Sales: {grandTotal.ToString("C")}");
        sb.AppendLine();
        sb.Append(detailsSb.ToString());

        var reportPath = Path.Combine(currentDirectory, "salesSummaryReport.txt");
        File.WriteAllText(reportPath, sb.ToString());
    }
}

class SalesData
{
    public double Total { get; set; }
}

# W01 Assignment Notes

## 1. Evidence of Web API Endpoints

Below is a record of testing all CRUD operations against the `ContosoPizza` Web API, including the required additional record ("Hawaiian").

```http
=== GET ALL (Initial) ===
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8

[
  {"id":1,"name":"Classic Italian","isGlutenFree":false},
  {"id":2,"name":"Veggie","isGlutenFree":true},
  {"id":3,"name":"Hawaiian","isGlutenFree":false}
]

=== POST NEW PIZZA ===
HTTP/1.1 201 Created
Location: http://localhost:5160/Pizza/4

{"id":4,"name":"Pepperoni","isGlutenFree":false}

=== GET NEW PIZZA (ID 4) ===
HTTP/1.1 200 OK

{"id":4,"name":"Pepperoni","isGlutenFree":false}

=== PUT (UPDATE) PIZZA (ID 3) ===
HTTP/1.1 204 No Content

=== DELETE PIZZA (ID 1) ===
HTTP/1.1 204 No Content

=== GET ALL (FINAL) ===
HTTP/1.1 200 OK

[
  {"id":2,"name":"Veggie","isGlutenFree":true},
  {"id":3,"name":"Hawaiian Update","isGlutenFree":true},
  {"id":4,"name":"Pepperoni","isGlutenFree":false}
]
```

## 2. Sales Summary Function

Below is the C# function added to the `FilesAndDirectories` app to generate the sales summary report.

```csharp
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
        // Output the filename and currency format
        string fileName = Path.GetFileName(kvp.Key);
        detailsSb.AppendLine($"  {fileName}: {kvp.Value.ToString("C")}");
    }

    sb.AppendLine($" Total Sales: {grandTotal.ToString("C")}");
    sb.AppendLine();
    sb.Append(detailsSb.ToString());

    var reportPath = Path.Combine(currentDirectory, "salesSummaryReport.txt");
    File.WriteAllText(reportPath, sb.ToString());
}
```

using ServiceStack;
using System.Text;

// Hard coded for simplicity
// No Error checking you'll want to ensure the file exists/etc

var inputFile = "d:\\temp\\emails.csv";
var outputFile = inputFile.Replace(".csv", "-hashed.csv");

// ServiceStack.Text string extensions rock
var csv = inputFile.ReadAllText();

// ServiceStack.Text has some nifty methods to make converting from csv/json/etc very easy
var data = csv.FromCsv<List<CsvReader>>();

// Select and Hash each email
List<CsvWriter> output = data.Select(d => new CsvWriter() { Hash = d.Email.ToLower().MD5() }).ToList();
// Write them all to the output file
File.WriteAllText(outputFile, output.ToCsv());


public class CsvReader
{
    public string Email { get; set; } = String.Empty;
}

// The output class is just one column hashed

public class CsvWriter
{
    public string Hash { get; set; } = String.Empty;
}
public static class Helpers
{
    public static string MD5(this string s)
    {
        using var provider = System.Security.Cryptography.MD5.Create();
        StringBuilder builder = new StringBuilder();

        foreach (byte b in provider.ComputeHash(Encoding.UTF8.GetBytes(s)))
            builder.Append(b.ToString("x2").ToLower());

        return builder.ToString();
    }
}
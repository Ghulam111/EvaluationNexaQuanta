using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

public static class CsvParser
{
    public static List<T> ParseCsv<T>(string filePath) where T : new()
    {
        var results = new List<T>();
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        try
        {
            using var reader = new StreamReader(filePath);
            string? headerLine = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(headerLine))
                throw new Exception("CSV file is empty or missing headers.");

            var headers = headerLine.Split(',').Select(h => h.Trim().ToLower()).ToArray();

            // Map header index to property
            var propMap = new Dictionary<int, PropertyInfo>();
            for (int i = 0; i < headers.Length; i++)
            {
                var prop = properties.FirstOrDefault(p => p.Name.Equals(headers[i], StringComparison.OrdinalIgnoreCase));
                if (prop != null)
                {
                    propMap[i] = prop;
                }
            }

            if (propMap.Count == 0)
                throw new Exception("No matching headers found for object properties.");

            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;

                var values = line.Split(',');
                var obj = new T();

                foreach (var kvp in propMap)
                {
                    int index = kvp.Key;
                    PropertyInfo prop = kvp.Value;

                    if (index >= values.Length) continue;

                    try
                    {
                        var value = Convert.ChangeType(values[index].Trim(), prop.PropertyType);
                        prop.SetValue(obj, value);
                    }
                    catch
                    {
                        // Log or skip invalid conversions
                        Console.WriteLine($"Warning: Skipped invalid value '{values[index]}' for property '{prop.Name}'.");
                    }
                }

                results.Add(obj);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while parsing CSV: {ex.Message}");
        }

        return results;
    }
}

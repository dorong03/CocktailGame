using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace ExcelParser
{
    public class JsonWriter
    {
        public void ConvertExcelToJsonFile(string savePath, Worksheet worksheet)
        {
            List<Dictionary<string, object>> jsonData = new List<Dictionary<string, object>>();

            foreach (var row in worksheet.Rows)
            {
                Dictionary<string, object> rowData = new Dictionary<string, object>();
                for (int i = 0; i < worksheet.Headers.Count; i++)
                {
                    string header = worksheet.Headers[i];
                    object value = ConvertValueType(worksheet.Types[i], row[header]);
                    
                    rowData.Add(header, value);
                }
                jsonData.Add(rowData);
            }

            string json = BuildJson(jsonData);
            
            string filePath = Path.Combine(savePath, $"{worksheet.SheetName}.json");
            
            File.WriteAllText(filePath, json, Encoding.UTF8);
        }

        public object ConvertValueType(string type, string value)
        {
            bool nullable = false;

            if (type.EndsWith("/null"))
            {
                nullable = true;
                type = type.Replace("/null", "");
            }

            if (nullable && value.Equals("null"))
            {
                return null;
            }
            
            switch (type)
            {
                case "string":
                    return value;
                case "int":
                    return (int)double.Parse(value);
                case "List<string>":
                    List<string> list = new();

                    foreach (string item in value.Split(','))
                    {
                        list.Add(item.Trim());
                    }

                    return list;
                default:
                    throw new Exception($"테이블에서 지원하지 않는 타입 -> {type}");
            }
        }

        private string BuildJson(List<Dictionary<string, object>> jsonData)
        {
            StringBuilder sb = new();
            sb.AppendLine("[");
            for (int i = 0; i < jsonData.Count; i++)
            {
                sb.AppendLine("  {");

                int count = 0;

                foreach (var row in jsonData[i])
                {
                    sb.Append($"    {JsonString(row.Key)}: ");
                    sb.Append(JsonValue(row.Value));

                    count++;

                    if (count != jsonData[i].Count)
                    {
                        sb.Append(",");
                    }

                    sb.AppendLine();
                }

                sb.Append("  }");

                if (i != jsonData.Count - 1)
                {
                    sb.Append(",");
                }
                
                sb.AppendLine();
            }
            sb.AppendLine("]");
            
            return sb.ToString();
        }
        
        private string JsonValue(object value)
        {
            if (value == null)
                return "null";

            if (value is string str)
                return JsonString(str);

            if (value is int)
                return value.ToString();

            if (value is List<string> list)
            {
                StringBuilder sb = new();

                sb.Append("[");

                for (int i = 0; i < list.Count; i++)
                {
                    sb.Append(JsonString(list[i]));

                    if (i != list.Count - 1)
                        sb.Append(", ");
                }

                sb.Append("]");

                return sb.ToString();
            }

            return JsonString(value.ToString());
        }
        
        private string JsonString(string text)
        {
            text = text.Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\r", "")
                .Replace("\n", "\\n")
                .Replace("\t", "\\t");

            return $"\"{text}\"";
        }
    }
}
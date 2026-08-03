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
                case "float":
                    return (float)double.Parse(value);
                case "double":
                    return double.Parse(value);
                case "List<string>":
                    List<string> stringList = new();

                    foreach (string item in value.Split(','))
                    {
                        stringList.Add(item.Trim());
                    }
                    return stringList;
                case "List<IngredientAmount>":
                    List<IngredientAmount> ingredientAmounts = new List<IngredientAmount>();

                    foreach (string item in value.Split(','))
                    {
                        string[] splits = item.Trim().Split(":");

                        IngredientAmount ingredientAmount = new IngredientAmount();
                        ingredientAmount.IngredientId = splits[0];
                        ingredientAmount.Amount = float.Parse(splits[1]);
                        ingredientAmounts.Add(ingredientAmount);
                    }

                    return ingredientAmounts;
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

            if (value is List<IngredientAmount> ingredientAmounts)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("[");

                for (int i = 0; i < ingredientAmounts.Count; i++)
                {
                    var entry = ingredientAmounts[i];
                    sb.Append("{\"IngredientId\": ");
                    sb.Append(JsonString(entry.IngredientId));
                    sb.Append(", \"Amount\": ");
                    sb.Append(entry.Amount);
                    sb.Append("}");

                    if (i != ingredientAmounts.Count - 1)
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

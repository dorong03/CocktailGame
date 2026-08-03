using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Xml;

namespace ExcelParser
{
    public class WorksheetParser
    {
        public Worksheet Parse(string excelPath, SheetInfo info, SharedStringTable sharedStringTable)
        {
            using ZipArchive archive = ZipFile.OpenRead(excelPath);
            ZipArchiveEntry entry = archive.GetEntry(info.Path);

            if (entry == null)
            {
                return null;
            }

            using Stream stream = entry.Open();
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            XmlNodeList nodeList = doc.GetElementsByTagName("row");

            Worksheet worksheet = new Worksheet();
            worksheet.SetSheetName(info.Name);

            foreach (XmlNode rowNode in nodeList)
            {
                List<string> values = new List<string>();
                XmlAttribute rowNumberAttribute = rowNode.Attributes["r"];
                foreach (XmlNode childNode in rowNode.ChildNodes)
                {
                    XmlNode valueNode = childNode["v"];
                    if (valueNode == null)
                    {
                        continue;
                    }

                    XmlAttribute typeAttribute = childNode.Attributes["t"];

                    if (typeAttribute != null && typeAttribute.Value == "s")
                    {
                        int index = int.Parse(valueNode.InnerText);
                        values.Add(sharedStringTable.GetValue(index));
                    }
                    else
                    {
                        values.Add(valueNode.InnerText);
                    }
                }

                switch (rowNumberAttribute.Value)
                {
                    case "1":
                        foreach (string value in values)
                        {
                            worksheet.AddType(value);
                        }
                        break;
                    case "2":
                        foreach (string value in values)
                        {
                            worksheet.AddHeader(value);
                        }
                        break;
                    default:
                        // 엑셀은 스타일만 있고 값이 비어있는 행도 XML에 남긴다.
                        // 이런 행은 values가 비어(혹은 헤더 수보다 적어) 있으므로 건너뛴다.
                        if (values.Count < worksheet.Headers.Count)
                        {
                            break;
                        }

                        Dictionary<string, string> row = new();

                        for (int i = 0; i < worksheet.Headers.Count; i++)
                        {
                            row.Add(
                                worksheet.Headers[i],
                                values[i]
                            );
                        }
                        worksheet.AddRow(row);
                        break;
                }
            }

            return worksheet;
        }
    }
}
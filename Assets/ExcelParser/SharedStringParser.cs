using System;
using System.IO;
using System.IO.Compression;
using System.Xml;

namespace ExcelParser
{
    public class SharedStringParser
    {
        public SharedStringTable Parse(string excelPath)
        {
            SharedStringTable table = new SharedStringTable();

            using ZipArchive archive = ZipFile.OpenRead(excelPath);

            ZipArchiveEntry entry = archive.GetEntry("xl/sharedStrings.xml");

            if (entry == null)
            {
                // 액셀 파일에 항상 존재하는지를 모르겠네
                // 만약 string 이 없는 excel 문서에 생성되지 않는 파일이라면 바꿔야 할 듯
                // 그냥 return ; 만 붙히는걸로
                return null;
            }

            using Stream stream = entry.Open();
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            XmlNodeList nodes = doc.GetElementsByTagName("t");
            
            foreach (XmlNode node in nodes)
            {
                table.AddValue(node.InnerText);
            }

            return table;
        }
    }
}
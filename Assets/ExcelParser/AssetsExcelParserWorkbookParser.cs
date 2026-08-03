using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Xml;

namespace ExcelParser
{
    public class WorkbookParser
    {
        public List<SheetInfo> Parse(string excelPath)
        {
            List<SheetInfo> infoList = new List<SheetInfo>();

            using ZipArchive archive = ZipFile.OpenRead(excelPath);

            ZipArchiveEntry entry = archive.GetEntry("xl/workbook.xml");

            if (entry == null)
            {
                throw new FileNotFoundException("workbook 파일이 없음");
            }

            using Stream stream = entry.Open();

            XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            XmlNodeList sheetList = doc.GetElementsByTagName("sheet");

            if (sheetList.Count == 0)
            {
                return infoList;
            }

            foreach (XmlNode sheetNode in sheetList)
            {
                SheetInfo info = new SheetInfo();

                info.SetName(sheetNode.Attributes["name"].Value);
                info.SetPath(sheetNode.Attributes["r:id"].Value);

                infoList.Add(info);
            }

            // xl/_rels/workbook.xml.rels 에서 연결해주기

            ZipArchiveEntry relsEntry = archive.GetEntry("xl/_rels/workbook.xml.rels");

            if (relsEntry == null)
            {
                throw new FileNotFoundException("workbook.xml.rels 파일이 없음");
            }

            using Stream relsStream = relsEntry.Open();

            XmlDocument relsDoc = new XmlDocument();
            relsDoc.Load(relsStream);

            XmlNodeList relNodes = relsDoc.GetElementsByTagName("Relationship");


            // 어차피 시트 몇개 없으니까 이렇게 해도 괜찮을 듯
            foreach (XmlNode relNode in relNodes)
            {
                string id = relNode.Attributes["Id"].Value;
                string target = relNode.Attributes["Target"].Value;

                foreach (SheetInfo info in infoList)
                {
                    if (info.Path == id)
                    {
                        info.SetPath("xl/" + target);
                        break;
                    }
                }
            }

            return infoList;
        }
    }
}

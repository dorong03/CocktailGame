namespace ExcelParser
{
    using System.Collections.Generic;

    public class Worksheet
    {
        public string SheetName { get; private set; }

        public List<string> Types { get; private set; } = new List<string>();
        public List<string> Headers { get; private set; } = new List<string>();

        // 일단 저장 자체는 전부 string 으로 하고 json 으로 변환할때 type 값이랑 맞춰서 저장하는걸로
        public List<Dictionary<string, string>> Rows { get; private set; } = new List<Dictionary<string, string>>();

        public void SetSheetName(string sheetName)
        {
            SheetName = sheetName;
        }

        public void AddType(string type)
        {
            Types.Add(type);
        }

        public void AddHeader(string type)
        {
            Headers.Add(type);
        }

        public void AddRow(Dictionary<string, string> row)
        {
            Rows.Add(row);
        }
    }
}
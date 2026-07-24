namespace ExcelParser
{
    public class SheetInfo
    {
        public string Name { get; private set; }
        public string Path { get; private set; }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetPath(string path)
        {
            Path = path;
        }
    }
}
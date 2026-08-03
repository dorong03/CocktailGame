using System;
using System.Collections.Generic;

namespace ExcelParser
{
    public class SharedStringTable
    {
        private readonly List<string> values = new List<string>();

        public void AddValue(string value)
        {
            values.Add(value);
        }

        public string GetValue(int index)
        {
            return values[index];
        }
    }
}
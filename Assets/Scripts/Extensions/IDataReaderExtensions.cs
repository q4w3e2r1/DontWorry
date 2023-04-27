using System.Collections.Generic;
using System.Data;

namespace SQL_Quest.Extentions
{
    public static class IDataReaderExtensions
    {
        public static string[][] GetRows(this IDataReader reader)
        {
            var result = new List<string[]>();

            while (reader.Read())
            {
                result.Add(ReadSingleRow((IDataRecord)reader));
            }

            return result.ToArray();
        }

        private static string[] ReadSingleRow(IDataRecord dataRecord)
        {
            var row = new string[dataRecord.FieldCount];
            for (int i = 0; i < row.Length; i++)
                row[i] = dataRecord[i].ToString();

            return row;
        }
    }
}
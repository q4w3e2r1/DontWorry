using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SQL_Quest.Database
{
    public class Table : MonoBehaviour
    {
        public string Name;
        public string DatabaseName;
        [SerializeField] private List<string> _columnsName;
        [SerializeField] private List<string> _columnsType;

        public Dictionary<string, string> ColumsDictionary;

        public string[] Columns => ColumsDictionary.Keys.ToArray();

        public Table(string name, Dictionary<string, string> colums)
        {
            Name = name;
            ColumsDictionary = colums;
        }

        public void Start()
        {
            if (_columnsName.Count == 0)
                return;

            ColumsDictionary = new();
            for (int i = 0; i < _columnsName.Count; i++)
                ColumsDictionary[_columnsName[i]] = _columnsType[i];
        }

        public static string Write(string header, string[] rows)
        {
            if (rows.Length == 0)
                return "Empty set";

            var angle = '+';
            var verticalLine = '|';
            var horizontalLine = '-';

            var strokeLength = Mathf.Max(header.Length, rows.Select(row => row.Length).Max()) + 2;
            var result = new StringBuilder();

            var openCloseLine = angle + new string(horizontalLine, strokeLength) + angle;

            result.Append(openCloseLine + "\n");
            result.Append(verticalLine + " " + header + new string(' ', strokeLength - header.Length - 1) + verticalLine + "\n");
            result.Append(openCloseLine + "\n");

            foreach (var row in rows)
                result.Append(verticalLine + " " + row + new string(' ', strokeLength - row.Length - 1) + verticalLine + "\n");

            result.Append(openCloseLine);

            return result.ToString();
        }

        public static string Write(string[] header, string[][] rows)
        {
            var columns = new string[header.Length][];

            for (int i = 0; i < header.Length; i++)
            {
                var columnRows = rows.Select(row => row[i]).ToArray();
                columns[i] = Write(header[i], columnRows).Split('\n');
            }

            var result = new string[columns[0].Length];

            for (int i = 0; i < header.Length; i++)
            {
                for (int j = 0; j < columns[i].Length; j++)
                {
                    if (i == 0)
                    {
                        result[j] += columns[i][j];
                        continue;
                    }

                    columns[i][j] = columns[i][j].Remove(0, 1);
                    result[j] += columns[i][j];
                }
            }

            return string.Join("\n", result);
        }
    }
}
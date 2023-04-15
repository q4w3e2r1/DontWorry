using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Table
{
    public string Name;
    public string DatabaseName;
    [SerializeField] private List<string> _columnsName = new();
    [SerializeField] private List<string> _columnsType = new();
    
    public Dictionary<string, string> ColumsDictionary = new();

    public string[] Columns => ColumsDictionary.Keys.ToArray();

    public Table(string name, Dictionary<string, string> colums)
    {
        Name = name;
        ColumsDictionary = colums;
    }

    public void Initialize()
    {
        if(_columnsName.Count == 0 || ColumsDictionary.Count != 0)
            return;
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

        var strokeLength = Mathf.Max(header.Length, rows.Max().Length) + 2;
        var result = new StringBuilder();

        var openCloseLine = angle + new string(horizontalLine, strokeLength) + angle;

        result.Append(openCloseLine + "\n");
        result.Append(verticalLine + " " + header + new string(' ', strokeLength - header.Length - 1) + verticalLine + "\n");
        result.Append(openCloseLine + "\n");

        foreach (var databaseName in rows)
            result.Append(verticalLine + " " + databaseName +
                          new string(' ', strokeLength - databaseName.Length - 1) + verticalLine + "\n");

        result.Append(openCloseLine);

        return result.ToString();
    }
}

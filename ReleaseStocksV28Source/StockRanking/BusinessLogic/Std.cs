using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System;

//Standard Deviation function for SQLite
[SQLiteFunction(Name = "STD", FuncType = FunctionType.Aggregate, Arguments = 1)]
class Std : SQLiteFunction
{
    public override void Step(object[] args, int stepNumber, ref object contextData)
    {
        if (args.Length == 0 || args[0] == null)
            return;
        if (contextData == null)
            contextData = new List<double>();
        var records = (List<double>)contextData;
        records.Add(Convert.ToDouble(args[0]));
        contextData = records;
    }

    public override object Final(object contextData)
    {
        var records = (List<double>)contextData;
        if (records.Count <= 1)
            return 0;
        var avg = records.Average();
        var returnValue = Math.Sqrt(records.Average(v => Math.Pow((v - avg), 2)));
        return returnValue;
    }
}


//Standard Deviation function for SQLite
[SQLiteFunction(Name = "MEDIAN", FuncType = FunctionType.Aggregate, Arguments = 1)]
class Median : SQLiteFunction
{
    public override void Step(object[] args, int stepNumber, ref object contextData)
    {
        if (args.Length == 0 || args[0] == null)
            return;
        if (contextData == null)
            contextData = new List<decimal>();
        var records = (List<decimal>)contextData;
        records.Add(Convert.ToDecimal(args[0]));
        contextData = records;
    }

    public override object Final(object contextData)
    {
        if (contextData == null)
            return 0;
        var records = (List<decimal>)contextData;
        if (records.Count <= 3)
            return 0;
        return StockRanking.Feature.calcMedian(records);
    }
}


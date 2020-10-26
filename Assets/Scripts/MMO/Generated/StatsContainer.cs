using System;
using System.Collections.Generic;
using System.Text;

public class StatsContainer
{
    public uint Id { get; set; }
    public Dictionary<StatType, object> stats = new Dictionary<StatType, object>();
    public event Action<uint, StatType, object> OnStatChanged = delegate { };

    public void ChangeStatInt(StatType stat, int val)
    {
        SetStat(stat, (int)stats[stat] + val);
    }

    public void SetStat(StatType stat, object v)
    {
        stats[stat] = v;
        OnStatChanged(Id, stat, v);
    }

    public StatsContainer(uint id)
    {
        this.Id = id;

        foreach (var item in StatDataTypes.types)
        {
            var type = item.Value;

            switch (type)
            {
                case StatObjectType.INT32:
                    stats.Add(item.Key, (int)0);
                    break;
                case StatObjectType.INT16:
                    stats.Add(item.Key, (short)0);
                    break;
                case StatObjectType.FLOAT:
                    stats.Add(item.Key, (float)0);
                    break;
                case StatObjectType.STRING:
                    stats.Add(item.Key, (string)"");
                    break;
                case StatObjectType.BYTE:
                    stats.Add(item.Key, (byte)0);
                    break;
            }

        }
    }
}
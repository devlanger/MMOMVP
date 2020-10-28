using System.Collections;
using System.Collections.Generic;

public class MobData
{
    public int id;
    public string baseModel;

    public Dictionary<StatType, object> stats = new Dictionary<StatType, object>()
    {
        { StatType.NAME, "" },
        { StatType.HEALTH, (int)0 },
        { StatType.LEVEL, (byte)0 },
        { StatType.MIN_DMG, (int)0 },
        { StatType.MAX_DMG, (int)0 },
    };
}

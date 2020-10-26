
                using System.Collections.Generic;
                

public class StatDataTypes
{
public static Dictionary<StatType, StatObjectType> types = new Dictionary<StatType, StatObjectType>()
{
{ StatType.STA, StatObjectType.INT32 },
{ StatType.INT, StatObjectType.INT32 },
{ StatType.DEX, StatObjectType.INT32 },
{ StatType.STR, StatObjectType.INT32 },
{ StatType.HEALTH, StatObjectType.INT32 },
{ StatType.MAX_HEALTH, StatObjectType.INT32 },
{ StatType.MANA, StatObjectType.INT32 },
{ StatType.MAX_MANA, StatObjectType.INT32 },
{ StatType.LEVEL, StatObjectType.BYTE },
{ StatType.NAME, StatObjectType.STRING },
{ StatType.EXPERIENCE, StatObjectType.INT32 },
{ StatType.MIN_DMG, StatObjectType.INT32 },
{ StatType.MAX_DMG, StatObjectType.INT32 },
};
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor;
using UnityEngine;
using WebSocketMMOServer.Database;

public class MobsSection
{
    public MobData editingData;
    public Dictionary<int, MobData> mobs = new Dictionary<int, MobData>();

    public void LoadMobs()
    {
        mobs = new Dictionary<int, MobData>();
        DataTable table = DatabaseManager.ReturnQuery("SELECT * FROM mobs_proto");
        for (int i = 0; i < table.Rows.Count; i++)
        {
            var row = table.Rows[i];
            MobData data = new MobData()
            {
                id = (int)row["id"],
            };

            data.stats[StatType.NAME] = (string)row["name"];
            data.stats[StatType.LEVEL] = (byte)row["lvl"];
            data.stats[StatType.HEALTH] = (int)row["health"];
            data.stats[StatType.MIN_DMG] = (int)row["min_dmg"];
            data.stats[StatType.MAX_DMG] = (int)row["max_dmg"];

            mobs.Add(data.id, data);
        }
    }

    public void EditObject(int id)
    {
        editingData = mobs[id];
    }

    public void SaveEditedObject()
    {
        List<string> vals = new List<string>() { "name", "health", "lvl", "min_dmg", "max_dmg" };

        string x = "";
        string x2 = "";
        foreach (var item in vals)
        {
            x += string.Format("{0}=VALUES({0}),", item);
            x2 += item + ",";
        }

        x = x.Remove(x.Length - 1);
        x2 = x2.Remove(x2.Length - 1);

        if (!newEntry)
        {
            DatabaseManager.InsertQuery(string.Format("INSERT INTO mobs_proto(id, " + x2 + ") VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}') ON DUPLICATE KEY UPDATE id=VALUES(id)," + x,
                editingData.id, 
                (string)editingData.stats[StatType.NAME],
                (int)editingData.stats[StatType.HEALTH],
                (byte)editingData.stats[StatType.LEVEL],
                (int)editingData.stats[StatType.MIN_DMG],
                (int)editingData.stats[StatType.MAX_DMG]));
        }
        else
        {
            DatabaseManager.InsertQuery(string.Format("INSERT INTO mobs_proto(" + x2 + ") VALUES('{0}', '{1}', '{2}', '{3}', '{4}',)",
                (string)editingData.stats[StatType.NAME],
                (int)editingData.stats[StatType.HEALTH],
                (byte)editingData.stats[StatType.LEVEL],
                (int)editingData.stats[StatType.MIN_DMG],
                (int)editingData.stats[StatType.MAX_DMG]));
        }
    }

    internal void RemoveObject(int key)
    {
        DatabaseManager.ReturnQuery(string.Format("DELETE FROM mobs_proto WHERE id='{0}'", key));
    }

    internal bool newEntry;
}

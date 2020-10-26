using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using WebSocketMMOServer.Database;

public class SpawnSection
{
    public SpawnData editingData;
    public Dictionary<int, SpawnData> spawns = new Dictionary<int, SpawnData>();

    public void LoadMobs()
    {
        spawns = new Dictionary<int, SpawnData>();
        DataTable table = DatabaseManager.ReturnQuery("SELECT * FROM mobs");
        for (int i = 0; i < table.Rows.Count; i++)
        {
            var row = table.Rows[i];
            SpawnData data = new SpawnData()
            {
                id = (uint)(int)row["id"],
                mobId = (int)row["mob_id"],
                pos = new System.Numerics.Vector3((float)(double)row["x"], (float)(double)row["y"], (float)(double)row["z"]),
            };

            spawns.Add((int)data.id, data);
        }
    }

    public void EditObject(int id)
    {
        editingData = spawns[id];
    }

    public void SaveEditedObject()
    {
        List<string> vals = new List<string>() { "mob_id", "x", "y", "z" };

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
            DatabaseManager.InsertQuery(string.Format("INSERT INTO mobs(id, " + x2 + ") VALUES('{0}', '{1}', '{2}', '{3}', '{4}') ON DUPLICATE KEY UPDATE id=VALUES(id)," + x,
                editingData.id, editingData.mobId, editingData.pos.X, editingData.pos.Y, editingData.pos.Z));
        }
        else
        {
            DatabaseManager.InsertQuery(string.Format("INSERT INTO mobs(" + x2 + ") VALUES('{0}', '{1}', '{2}', '{3}')", editingData.mobId, editingData.pos.X, editingData.pos.Y, editingData.pos.Z));
        }
    }

    internal bool newEntry;
}

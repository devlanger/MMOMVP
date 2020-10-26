using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor;
using UnityEngine;
using WebSocketMMOServer.Database;

public class ItemsSection
{
    internal bool newEntry;

    public ItemData editingData;
    public Dictionary<int, ItemData> items = new Dictionary<int, ItemData>();

    public void LoadData()
    {
        items = new Dictionary<int, ItemData>();
        DataTable table = DatabaseManager.ReturnQuery("SELECT * FROM items_proto");
        for (int i = 0; i < table.Rows.Count; i++)
        {
            var row = table.Rows[i];
            ItemData data = new ItemData()
            {
                id = (int)row["id"],
                name = (string)row["name"],
                slot = (byte)row["slot"],
            };

            items.Add(data.id, data);
        }
    }

    public void EditObject(int id)
    {
        editingData = items[id];
    }

    public void SaveEditedObject()
    {
        List<string> vals = new List<string>() { "name", "slot" };

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
            DatabaseManager.InsertQuery(string.Format("INSERT INTO items_proto(id, " + x2 + ") VALUES('{0}', '{1}', '{2}') ON DUPLICATE KEY UPDATE id=VALUES(id),"+ x,
                editingData.id, editingData.name, editingData.slot));
        }
        else
        {
            DatabaseManager.InsertQuery(string.Format("INSERT INTO items_proto(" + x2 + ") VALUES('{0}', '{1}')", editingData.name, editingData.slot));
        }
    }
}

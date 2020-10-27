using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEditor;
using UnityEngine;
using WebSocketMMOServer.Database;

public class SkillsSection
{
    public SkillData editingData;
    public Dictionary<int, SkillData> skills = new Dictionary<int, SkillData>();
    public string pathJson = "Assets/Resources/Data/skills.json";

    public void LoadMobs()
    {
        skills = new Dictionary<int, SkillData>();
        DataTable table = DatabaseManager.ReturnQuery("SELECT * FROM skill_proto");
        for (int i = 0; i < table.Rows.Count; i++)
        {
            var row = table.Rows[i];
            SkillData data = new SkillData()
            {
                id = (int)row["id"],
                name = (string)row["name"],
                description = (string)row["description"],
                iconId = (int)row["icon_id"],
            };

            skills.Add(data.id, data);
        }
    }

    public void EditObject(int id)
    {
        editingData = skills[id];
        icon = SpritesManager.GetIcon(editingData.iconId);
    }

    public void SaveToJson()
    {
        string json = JsonConvert.SerializeObject(skills); 
        using (FileStream fs = new FileStream(pathJson, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(json);
            }
        }

        AssetDatabase.Refresh();
    }

    public void SaveEditedObject()
    {
        List<string> vals = new List<string>() { "name", "description", "icon_id" };

        string x = "";
        string x2 = "";
        foreach (var item in vals)
        {
            x += string.Format("{0}=VALUES({0}),", item);
            x2 += item + ",";
        }

        x = x.Remove(x.Length - 1);
        x2 = x2.Remove(x2.Length - 1);

        int id = SpritesManager.AddIfNotExist(icon);
        
        if (!newEntry)
        {
            DatabaseManager.InsertQuery(string.Format("INSERT INTO skill_proto(id, " + x2 + ") VALUES('{0}', '{1}', '{2}', '{3}') ON DUPLICATE KEY UPDATE id=VALUES(id)," + x,
                editingData.id, editingData.name, editingData.description, id));
        }
        else
        {
            DatabaseManager.InsertQuery(string.Format("INSERT INTO skill_proto(" + x2 + ") VALUES('{0}', '{1}', '{2}')", 
                editingData.name, editingData.description, id));
        }


    }

    internal void RemoveObject(int key)
    {
        DatabaseManager.ReturnQuery(string.Format("DELETE FROM skill_proto WHERE id='{0}'", key));
    }

    internal bool newEntry;
    internal Sprite icon;
}

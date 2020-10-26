using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;
using System.IO;
using System.Collections.Generic;
using System;
using WebSocketMMOServer.Database;

public class StatsWindow : EditorWindow
{
    private string copyPath = "Assets/Scripts/MMO/Generated/StatType.cs";
    private string copyPathItems = "Assets/Scripts/MMO/Generated/ItemSlot.cs";
    private string copyPathContainers = "Assets/Scripts/MMO/Generated/ItemsContainerId.cs";
    private string copyPathPackets = "Assets/Scripts/MMO/Generated/ClientPackets.cs";
    private string copyPathPacketsServer = "Assets/Scripts/MMO/Generated/ServerPackets.cs";
    private string copyPath2 = "Assets/Scripts/MMO/Generated/StatDataTypes.cs";
    private Vector2 scrollPos;

    private static List<string> stats = new List<string>();
    private static List<string> itemSlots = new List<string>();
    private static List<string> itemContainers = new List<string>();
    private static List<string> clientPackets = new List<string>();
    private static List<string> serverPackets = new List<string>();

    private int skillsTab;
    private int spawnsTab;
    private int itemsTab;
    private int mobsTab;
    private int globalTab;
    private int tab;
    private int newCount = 0;

    public static SkillsSection skills;
    public static SpawnSection spawns;
    public static ItemsSection items;
    public static MobsSection mobs;

    private static Dictionary<StatType, StatObjectType> dataTypes = new Dictionary<StatType, StatObjectType>();

    [MenuItem("MMO/Editor")]
    static void Init()
    {
        StatsWindow window = (StatsWindow)EditorWindow.GetWindow(typeof(StatsWindow));
        Refresh();

        window.Show();
        //
    }

    private static void Refresh()
    {
        RefreshDatabase();

        stats = new List<string>();
        itemSlots = new List<string>();
        clientPackets = new List<string>();
        serverPackets = new List<string>();
        itemContainers = new List<string>();

        foreach (var item in Enum.GetValues(typeof(StatType)))
        {
            stats.Add(item.ToString());
        }

        foreach (var item in Enum.GetValues(typeof(ClientPacketType)))
        {
            clientPackets.Add(item.ToString());
        }

        foreach (var item in Enum.GetValues(typeof(ItemSlot)))
        {
            itemSlots.Add(item.ToString());
        }

        foreach (var item in Enum.GetValues(typeof(ServerPacketType)))
        {
            serverPackets.Add(item.ToString());
        }

        foreach (var item in Enum.GetValues(typeof(ItemsContainerId)))
        {
            itemContainers.Add(item.ToString());
        }

        dataTypes = new Dictionary<StatType, StatObjectType>(StatDataTypes.types);
    }

    private static void RefreshDatabase()
    {
        DatabaseManager m = new DatabaseManager();
        spawns = new SpawnSection();
        mobs = new MobsSection();
        items = new ItemsSection();
        skills = new SkillsSection();
        mobs.LoadMobs();
        items.LoadData();
        spawns.LoadMobs();
        skills.LoadMobs();
    }

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        Refresh();
    }

    void OnGUI()
    {
        globalTab = GUILayout.Toolbar(globalTab, new string[] { "Global", "Mobs", "Items", "Spawner", "Skills" });
        switch (globalTab)
        {
            case 0:
                GUILayout.Label("Tabs:");
                tab = GUILayout.Toolbar(tab, new string[] { "Stats", "Packets", "Items" });
                switch (tab)
                {
                    case 0:
                        GUILayout.Label("Stats Settings", EditorStyles.boldLabel);
                        DrawStatsSection();
                        break;
                    case 1:
                        DrawPacketsSection();
                        break;
                    case 2:
                        DrawItemsSection();
                        break;
                }
                break;
            case 1:
                GUILayout.Label("Tabs:");
                mobsTab = GUILayout.Toolbar(mobsTab, new string[] { "List", "Edit" });
                switch(mobsTab)
                {
                    case 0:
                        if (GUILayout.Button("Connect to Database..."))
                        {
                            try
                            {
                                mobs.LoadMobs();
                            }
                            catch (Exception ex)
                            {
                                EditorUtility.DisplayDialog("Error", "Can't load database: " + ex.ToString(), "Ok");
                            }
                        }

                        if (GUILayout.Button("Add"))
                        {
                            mobs.editingData = new MobData();
                            mobsTab = 1;
                            mobs.newEntry = true;
                        }

                        foreach (var item in mobs.mobs)
                        {
                            GUILayout.BeginHorizontal();
                            if (GUILayout.Button("Edit", GUILayout.Width(60)))
                            {
                                mobs.newEntry = false;
                                mobs.EditObject(item.Key);
                                mobsTab = 1;
                            }
                            GUILayout.Label(item.Key + ": " + item.Value.stats[StatType.NAME]);
                            GUILayout.EndHorizontal();
                        }
                        break;
                    case 1:
                        if(mobs.editingData == null)
                        {
                            GUILayout.Label("Select data first from the list...");
                        }
                        else
                        {
                            GUILayout.Label("Id");
                            GUILayout.Label(mobs.editingData.id.ToString());
                            
                            GUILayout.Label("Name");
                            mobs.editingData.stats[StatType.NAME] = GUILayout.TextField((string)mobs.editingData.stats[StatType.NAME]);

                            GUILayout.Label("Health");
                            mobs.editingData.stats[StatType.HEALTH] = EditorGUILayout.IntField((int)mobs.editingData.stats[StatType.HEALTH]);

                            GUILayout.Label("Lvl");
                            mobs.editingData.stats[StatType.LEVEL] = (byte)EditorGUILayout.IntField((byte)mobs.editingData.stats[StatType.LEVEL]);

                            GUILayout.Label("Damage");
                            GUILayout.BeginHorizontal();
                            mobs.editingData.stats[StatType.MIN_DMG] = EditorGUILayout.IntField((int)mobs.editingData.stats[StatType.MIN_DMG]);
                            mobs.editingData.stats[StatType.MAX_DMG] = EditorGUILayout.IntField((int)mobs.editingData.stats[StatType.MAX_DMG]);
                            GUILayout.EndHorizontal();

                            if (GUILayout.Button("Save", GUILayout.Width(60)))
                            {
                                mobs.SaveEditedObject();
                                mobsTab = 0;
                                mobs.editingData = null;
                                RefreshDatabase();
                            }
                            if (GUILayout.Button("Back", GUILayout.Width(60)))
                            {
                                mobsTab = 0;
                                mobs.editingData = null;
                                RefreshDatabase();
                            }
                        }
                        break;
                }
                break;
            case 2:
                GUILayout.Label("Tabs:");
                itemsTab = GUILayout.Toolbar(itemsTab, new string[] { "List", "Edit" });
                switch (itemsTab)
                {
                    case 0:
                        if (GUILayout.Button("Connect to Database..."))
                        {
                            try
                            {
                                items.LoadData();
                            }
                            catch (Exception ex)
                            {
                                EditorUtility.DisplayDialog("Error", "Can't load database: " + ex.ToString(), "Ok");
                            }
                        }

                        if (GUILayout.Button("Add"))
                        {
                            items.editingData = new ItemData();
                            itemsTab = 1;
                            items.newEntry = true;
                        }

                        foreach (var item in items.items)
                        {
                            GUILayout.BeginHorizontal();
                            if (GUILayout.Button("Edit", GUILayout.Width(60)))
                            {
                                items.newEntry = false;
                                items.EditObject(item.Key);
                                itemsTab = 1;
                            }
                            GUILayout.Label(item.Key + ": " + item.Value.name);
                            GUILayout.EndHorizontal();
                        }
                        break;
                    case 1:
                        if (items.editingData == null)
                        {
                            GUILayout.Label("Select data first from the list...");
                        }
                        else
                        {
                            GUILayout.Label("Id");
                            GUILayout.Label(items.editingData.id.ToString());

                            GUILayout.Label("Name");
                            items.editingData.name = GUILayout.TextField(items.editingData.name);

                            GUILayout.Label("Slot");
                            items.editingData.slot = (byte)(ItemSlot)EditorGUILayout.EnumPopup((ItemSlot)items.editingData.slot);

                            if (GUILayout.Button("Save", GUILayout.Width(60)))
                            {
                                items.SaveEditedObject();
                                itemsTab = 0;
                                items.editingData = null;
                                RefreshDatabase();
                            }
                            if (GUILayout.Button("Back", GUILayout.Width(60)))
                            {
                                itemsTab = 0;
                                items.editingData = null;
                                RefreshDatabase();
                            }
                        }
                        break;
                }
                break;
            case 3:
                GUILayout.Label("Tabs:");
                spawnsTab = GUILayout.Toolbar(spawnsTab, new string[] { "List", "Edit" });
                switch (spawnsTab)
                {
                    case 0:
                        if (GUILayout.Button("Connect to Database..."))
                        {
                            try
                            {
                                spawns.LoadMobs();
                            }
                            catch (Exception ex)
                            {
                                EditorUtility.DisplayDialog("Error", "Can't load database: " + ex.ToString(), "Ok");
                            }
                        }

                        if (GUILayout.Button("Add"))
                        {
                            spawns.editingData = new SpawnData();
                            spawnsTab = 1;
                            spawns.newEntry = true;
                        }

                        foreach (var item in spawns.spawns)
                        {
                            GUILayout.BeginHorizontal();
                            if (GUILayout.Button("Edit", GUILayout.Width(60)))
                            {
                                spawns.newEntry = false;
                                spawns.EditObject(item.Key);
                                spawnsTab = 1;
                            }
                            GUILayout.Label(item.Key + ": " + item.Value.mobId + "[" + item.Value.pos.ToString() + "]");
                            GUILayout.EndHorizontal();
                        }
                        break;
                    case 1:
                        if (spawns.editingData == null)
                        {
                            GUILayout.Label("Select data first from the list...");
                        }
                        else
                        {
                            GUILayout.Label("Id");
                            GUILayout.Label(spawns.editingData.id.ToString());

                            GUILayout.Label("Mob Id");
                            spawns.editingData.mobId = EditorGUILayout.IntField(spawns.editingData.mobId);

                            GUILayout.Label("Position");
                            GUILayout.BeginHorizontal();
                            spawns.editingData.pos.X = EditorGUILayout.FloatField(spawns.editingData.pos.X);
                            spawns.editingData.pos.Y = EditorGUILayout.FloatField(spawns.editingData.pos.Y);
                            spawns.editingData.pos.Z = EditorGUILayout.FloatField(spawns.editingData.pos.Z);
                            GUILayout.EndHorizontal();

                            if (GUILayout.Button("Save"))
                            {
                                spawns.SaveEditedObject();
                                spawnsTab = 0;
                                spawns.editingData = null;
                                RefreshDatabase();
                            }

                            if (GUILayout.Button("Back"))
                            {
                                spawnsTab = 0;
                                spawns.editingData = null;
                                RefreshDatabase();
                            }
                        }
                        break;
                }
                break;
            case 4:
                GUILayout.Label("Tabs:");
                skillsTab = GUILayout.Toolbar(skillsTab, new string[] { "List", "Edit" });
                switch (skillsTab)
                {
                    case 0:
                        if (GUILayout.Button("Connect to Database..."))
                        {
                            try
                            {
                                skills.LoadMobs();
                            }
                            catch (Exception ex)
                            {
                                EditorUtility.DisplayDialog("Error", "Can't load database: " + ex.ToString(), "Ok");
                            }
                        }

                        if (GUILayout.Button("Add"))
                        {
                            skills.editingData = new SkillData();
                            skillsTab = 1;
                            skills.newEntry = true;
                        }

                        foreach (var item in skills.skills)
                        {
                            GUILayout.BeginHorizontal();
                            if (GUILayout.Button("Edit", GUILayout.Width(60)))
                            {
                                skills.newEntry = false;
                                skills.EditObject(item.Key);
                                skillsTab = 1;
                            }
                            if (GUILayout.Button("Delete", GUILayout.Width(60)))
                            {
                                if(EditorUtility.DisplayDialog("Delete entry", "Are you really want to delete this entry?", "Yes", "No"))
                                {
                                    skills.RemoveObject(item.Key);
                                    RefreshDatabase();
                                }
                            }

                            GUILayout.Label(item.Key + ": " + item.Value.name);
                            GUILayout.EndHorizontal();
                        }
                        break;
                    case 1:
                        if (skills.editingData == null)
                        {
                            GUILayout.Label("Select data first from the list...");
                        }
                        else
                        {
                            GUILayout.Label("Id");
                            GUILayout.Label(skills.editingData.id.ToString());

                            skills.icon = (Sprite)EditorGUILayout.ObjectField("Sprite", skills.icon, typeof(Sprite), allowSceneObjects: false);

                            GUILayout.Label("Name");
                            skills.editingData.name = GUILayout.TextField(skills.editingData.name);
                            GUILayout.Label("Description");
                            skills.editingData.description = GUILayout.TextField(skills.editingData.description);

                            if (GUILayout.Button("Save", GUILayout.Width(60)))
                            {
                                skills.SaveEditedObject();
                                skillsTab = 0;
                                skills.editingData = null;
                                RefreshDatabase();
                            }

                            if (GUILayout.Button("Back", GUILayout.Width(60)))
                            {
                                skillsTab = 0;
                                skills.editingData = null;
                                RefreshDatabase();
                            }
                        }
                        break;
                }
                break;
        }
    }

    private void DrawPacketsSection()
    {
        EditorGUILayout.BeginVertical();
        GUILayout.Label("CLIENT PACKETS", EditorStyles.boldLabel);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width)/*, GUILayout.MinHeight(300)*/);
        GUILayout.ExpandWidth(true);

        var list = clientPackets;
        newCount = Mathf.Max(0, EditorGUILayout.IntField("size", list.Count));
        while (newCount < list.Count)
            list.RemoveAt(list.Count - 1);
        while (newCount > list.Count)
            list.Add(null);

        for (int i = 0; i < list.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("-", GUILayout.Width(25)))
            {
                list.RemoveAt(i);
            }
            list[i] = (string)EditorGUILayout.TextField(list[i]);
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add"))
        {
            list.Add(null);
        }

        GUILayout.Label("SERVER PACKETS", EditorStyles.boldLabel);
        newCount = Mathf.Max(0, EditorGUILayout.IntField("size", serverPackets.Count));
        while (newCount < serverPackets.Count)
            serverPackets.RemoveAt(list.Count - 1);
        while (newCount > serverPackets.Count)
            serverPackets.Add(null);

        for (int i = 0; i < serverPackets.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("-", GUILayout.Width(25)))
            {
                serverPackets.RemoveAt(i);
            }
            serverPackets[i] = (string)EditorGUILayout.TextField(serverPackets[i]);
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add"))
        {
            serverPackets.Add(null);
        }

        if (GUILayout.Button("Save"))
        {
            using (StreamWriter outfile =
                new StreamWriter(copyPathPackets))
            {
                outfile.WriteLine("public enum ClientPacketType : byte ");
                outfile.WriteLine("{");

                for (int i = 0; i < list.Count; i++)
                {
                    if (!string.IsNullOrEmpty(list[i]))
                    {
                        outfile.WriteLine(list[i] + " = " + i + ",");
                    }
                }

                outfile.WriteLine("}");
            }

            using (StreamWriter outfile =
                new StreamWriter(copyPathPacketsServer))
            {
                outfile.WriteLine("public enum ServerPacketType : byte ");
                outfile.WriteLine("{");

                for (int i = 0; i < serverPackets.Count; i++)
                {
                    if (!string.IsNullOrEmpty(serverPackets[i]))
                    {
                        outfile.WriteLine(serverPackets[i] + " = " + i + ",");
                    }
                }

                outfile.WriteLine("}");
            }

            AssetDatabase.Refresh();
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private void DrawStatsSection()
    {
        EditorGUILayout.BeginVertical();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width)/*, GUILayout.MinHeight(300)*/);
        GUILayout.ExpandWidth(true);

        var list = stats;
        newCount = Mathf.Max(0, EditorGUILayout.IntField("size", list.Count));
        while (newCount < list.Count)
            list.RemoveAt(list.Count - 1);
        while (newCount > list.Count)
            list.Add(null);

        for (int i = 0; i < stats.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("-", GUILayout.Width(25)))
            {
                list.RemoveAt(i);
            }
            stats[i] = (string)EditorGUILayout.TextField(stats[i]);
            try
            {
                dataTypes[(StatType)i] = (StatObjectType)EditorGUILayout.EnumPopup((StatObjectType)dataTypes[(StatType)i]);
            }
            catch
            {
                dataTypes[(StatType)i] = (StatObjectType)EditorGUILayout.EnumPopup((StatObjectType)0);
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add"))
        {
            list.Add(null);
        }

        if (GUILayout.Button("Save"))
        {
            using (StreamWriter outfile =
                new StreamWriter(copyPath))
            {
                outfile.WriteLine("public enum StatType : byte ");
                outfile.WriteLine("{");

                for (int i = 0; i < stats.Count; i++)
                {
                    if (!string.IsNullOrEmpty(stats[i]))
                    {
                        outfile.WriteLine(stats[i] + " = " + i + ",");
                    }
                }

                outfile.WriteLine("}");
            }

            using (StreamWriter outfile =
                new StreamWriter(copyPath2))
            {
                outfile.WriteLine(@"
                using System.Collections.Generic;
                ");
                outfile.WriteLine("");
                outfile.WriteLine("public class StatDataTypes");
                outfile.WriteLine("{");
                outfile.WriteLine("public static Dictionary<StatType, StatObjectType> types = new Dictionary<StatType, StatObjectType>()");
                outfile.WriteLine("{");

                for (int i = 0; i < stats.Count; i++)
                {
                    if (!string.IsNullOrEmpty(stats[i]))
                    {
                        outfile.WriteLine("{ StatType." + stats[i] + ", StatObjectType." + dataTypes[(StatType)i] + " },");
                    }
                }

                outfile.WriteLine("};");
                outfile.WriteLine("}");
            }
            AssetDatabase.Refresh();
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private void DrawItemsSection()
    {
        EditorGUILayout.BeginVertical();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width)/*, GUILayout.MinHeight(300)*/);
        GUILayout.ExpandWidth(true);
        GUILayout.Label("ITEM SLOTS", EditorStyles.boldLabel);

        var list = itemSlots;
        newCount = Mathf.Max(0, EditorGUILayout.IntField("size", list.Count));
        while (newCount < list.Count)
            list.RemoveAt(list.Count - 1);
        while (newCount > list.Count)
            list.Add(null);

        for (int i = 0; i < itemSlots.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("-", GUILayout.Width(25)))
            {
                list.RemoveAt(i);
            }
            itemSlots[i] = (string)EditorGUILayout.TextField(itemSlots[i]);
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add"))
        {
            list.Add(null);
        }

        GUILayout.Label("ITEM CONTAINERS TYPES", EditorStyles.boldLabel);
        var list2 = itemContainers;
        newCount = Mathf.Max(0, EditorGUILayout.IntField("size", list2.Count));
        while (newCount < list2.Count)
            list2.RemoveAt(list2.Count - 1);
        while (newCount > list2.Count)
            list2.Add(null);

        for (int i = 0; i < itemContainers.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("-", GUILayout.Width(25)))
            {
                list2.RemoveAt(i);
            }
            itemContainers[i] = (string)EditorGUILayout.TextField(itemContainers[i]);
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add"))
        {
            list2.Add(null);
        }

        if (GUILayout.Button("Save"))
        {
            using (StreamWriter outfile =
                new StreamWriter(copyPathItems))
            {
                outfile.WriteLine("public enum ItemSlot : byte ");
                outfile.WriteLine("{");

                for (int i = 0; i < itemSlots.Count; i++)
                {
                    if (!string.IsNullOrEmpty(itemSlots[i]))
                    {
                        outfile.WriteLine(itemSlots[i] + " = " + i + ",");
                    }
                }

                outfile.WriteLine("}");
            }

            using (StreamWriter outfile =
                new StreamWriter(copyPathContainers))
            {
                outfile.WriteLine("public enum ItemsContainerId : byte ");
                outfile.WriteLine("{");

                for (int i = 0; i < itemContainers.Count; i++)
                {
                    if (!string.IsNullOrEmpty(itemContainers[i]))
                    {
                        outfile.WriteLine(itemContainers[i] + " = " + i + ",");
                    }
                }

                outfile.WriteLine("}");
            }

            AssetDatabase.Refresh();
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }
}
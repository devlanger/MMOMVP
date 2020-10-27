using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    private Dictionary<int, SkillData> skills = new Dictionary<int, SkillData>();
    private Dictionary<int, ItemData> items = new Dictionary<int, ItemData>();

    private void Awake()
    {
        Instance = this;

        SpritesManager.Initialize();

        var skillsRsrc = Resources.Load<TextAsset>("Data/skills");
        skills = JsonConvert.DeserializeObject<Dictionary<int, SkillData>>(skillsRsrc.text);

        var itemsRsrc = Resources.Load<TextAsset>("Data/items");
        items = JsonConvert.DeserializeObject<Dictionary<int, ItemData>>(itemsRsrc.text);
    }

    public ItemData GetItem(int id)
    {
        if (!items.ContainsKey(id))
        {
            return null;
        }

        return items[id];
    }

    public SkillData GetSkill(int id)
    {
        if(!skills.ContainsKey(id))
        {
            return null;
        }

        return skills[id];
    }
}

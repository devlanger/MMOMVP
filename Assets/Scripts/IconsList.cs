using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MMO/Icons")]
public class IconsList : ScriptableObject
{
    public List<IndexedIcon> icons = new List<IndexedIcon>();
}

[System.Serializable]
public class IndexedIcon
{
    public int id;
    public Sprite icon;
}
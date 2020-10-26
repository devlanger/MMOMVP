using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesManager : MonoBehaviour
{
    public List<IndexedIcon> icons = new List<IndexedIcon>();

    [System.Serializable]
    public class IndexedIcon
    {
        public int id;
        public Sprite icon;
    }

    public Sprite GetIcon(int id)
    {
        var ic = icons.Find(i => i.id == id);
        if(ic == null)
        {
            return null;
        }

        return ic.icon;
    }

    public int AddIfNotExist(Sprite icon)
    {
        var ic = icons.Find(i => i.icon.name == icon.name);

        if (ic == null)
        {
            var newIcon = new IndexedIcon()
            {
                id = 0,
                icon = icon
            };

            icons.Add(newIcon);
            newIcon.id = icons.IndexOf(newIcon);

            return newIcon.id;
        }
        else
        {
            return ic.id;
        }
    }
}

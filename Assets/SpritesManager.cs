using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SpritesManager
{
    public static IconsList Icons;

    public static void Initialize()
    {
        Icons = Resources.Load<IconsList>("Data/icons");
    }

    public static Sprite GetIcon(int id)
    {
        var ic = Icons.icons.Find(i => i.id == id);
        if (ic == null)
        {
            return null;
        }

        return ic.icon;
    }

    public static int AddIfNotExist(Sprite icon)
    {
        var ic = Icons.icons.Find(i => i.icon.name == icon.name);

        if (ic == null)
        {
            var newIcon = new IndexedIcon()
            {
                id = 0,
                icon = icon
            };

            Icons.icons.Add(newIcon);
            newIcon.id = Icons.icons.IndexOf(newIcon);

            return newIcon.id;
        }
        else
        {
            return ic.id;
        }

    }
}

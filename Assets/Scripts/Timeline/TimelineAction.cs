using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimelineAction : ScriptableObject
{
    public abstract void Execute(Character user);
}

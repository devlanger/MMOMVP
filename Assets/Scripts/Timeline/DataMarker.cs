using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DataMarker : Marker, INotification
{
    public PropertyName id => new PropertyName();

    public TimelineAction action;
}
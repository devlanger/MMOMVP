using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineReceiver : MonoBehaviour, INotificationReceiver
{
    public Character user;

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if(notification is DataMarker)
        {
            DataMarker data = notification as DataMarker;
            data.action.Execute(user);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;

public class NotificationManager : MonoBehaviour
{
    void Start()
    {
#  if UNITY_ANDROID
        
        //var c = new AndroidNotificationChannel()
        //{
        //    Id = "channel_default",
        //    Name = "Default Channel",
        //    Importance = Importance.High,
        //    Description = "Generic notifications",
        //};
        //AndroidNotificationCenter.RegisterNotificationChannel(c);

        //var notification = new AndroidNotification();
        //notification.Title = " Ah correr Test";
        //notification.Text = " es tiempo de correr en marble fun race";
        ////TODO
        //// small icon and large icon notification.Icon
        //notification.FireTime = System.DateTime.Now.AddMinutes(15);

        //AndroidNotificationCenter.SendNotification(notification, "channel_default");


# endif
    }
}

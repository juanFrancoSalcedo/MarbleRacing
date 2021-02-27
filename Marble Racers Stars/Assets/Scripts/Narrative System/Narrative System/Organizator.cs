using System.Collections.Generic;
using UnityEngine;
public static class Organizer<T>
{
    public static void MoveIndexOfAList(T arg1, List<T> arg2, bool isUp)
    {
        T next;
        int oldIndex = 0;
        int newIndex = 0;

        if (arg2.Contains(arg1))
        {
            oldIndex = arg2.IndexOf(arg1);
        }
        newIndex = (isUp) ? (oldIndex - 1) : (oldIndex + 1);

        next = arg2[newIndex];
        arg2[newIndex] = arg1;
        arg2[oldIndex] = next;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BubbleSort<T>
{
    public static void  Sort(List<T> genericList, string nameVariable)
    {
        //GetType().GetField(nameVariable).GetValue());
        for (int i = 0; i < genericList.Count - 1; i++)
        {
            for (int j = i + 1; j < genericList.Count; j++)
            {

                var item1 = genericList[i].GetType().GetField(nameVariable).GetValue(genericList[i]);
                var item2 = genericList[j].GetType().GetField(nameVariable).GetValue(genericList[j]);

                if ( (int)item1 > (int)item2 )
                {
                    T buffer = genericList[j];
                    genericList[j] = genericList[i];
                    genericList[i] = buffer;
                }
            }
        }

        for (int i = 0; i < genericList.Count; i++) 
        {
           // Debug.Log(genericList[i].GetType().GetField(nameVariable).GetValue(genericList[i]));
        }
    }

    public static void SortReverse(List<T> genericList, string nameVariable)
    {
        //GetType().GetField(nameVariable).GetValue());
        for (int i = 0; i < genericList.Count - 1; i++)
        {
            for (int j = i + 1; j < genericList.Count; j++)
            {

                var item1 = genericList[i].GetType().GetField(nameVariable).GetValue(genericList[i]);
                var item2 = genericList[j].GetType().GetField(nameVariable).GetValue(genericList[j]);

                if ((int)item1 < (int)item2)
                {
                    T buffer = genericList[j];
                    genericList[j] = genericList[i];
                    genericList[i] = buffer;
                }
            }
        }

        for (int i = 0; i < genericList.Count; i++)
        {
            // Debug.Log(genericList[i].GetType().GetField(nameVariable).GetValue(genericList[i]));
        }
    }

}

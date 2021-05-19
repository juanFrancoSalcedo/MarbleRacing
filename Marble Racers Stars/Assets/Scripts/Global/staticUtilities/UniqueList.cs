using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public static class UniqueList
{
    public static List<int> CreateRandomListWithoutRepeating(int lowerBound, int maxBound, int length)
    {
        int count = 0;
        List<int> listRand = new List<int>();
        while (count < length)
        {
            int currentRandom = Random.Range(lowerBound, maxBound);
            if (!listRand.Contains(currentRandom))
            {
                listRand.Add(currentRandom);
                count++;
            }
        }
        return listRand;
    }

    [System.Serializable]
    public class UniqueListWrapper
    {
        public List<int> listaInt = new List<int>();
    }
}

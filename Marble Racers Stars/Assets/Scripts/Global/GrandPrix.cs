using UnityEngine;
using MyBox;


namespace LeagueSYS
{
    [System.Serializable]
    public class GrandPrix
    {
        public string trackInfo;
        public int laps = 1;
        public bool usePowers;
        [ConditionalField(nameof(usePowers))] public bool useAllPows = false;
        [ConditionalField(nameof(useAllPows),true)] public PowerUpType singlePow;
        public bool isQualifying;
        [ConditionalField(nameof(isQualifying))] public int marblesLessToQualy = 3;
        public bool twoPilots;
        [Range(0,10)]public float wear =0;
    }
}



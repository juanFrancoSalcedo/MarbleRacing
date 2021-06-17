using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeagueSYS;


public static class PilotsStatsSetter
{
    private static string[] statsNamesVariables = { "forceTurbo", "forceDirection", "coldTimeTurbo", "coldTimeDirection", "hp" };
    private static string[] constantsNamesVariables = { "forceBaseForward", "forceBaseDriving", "timeReduceAcelerationBase", "timeReduceDrivingBase", "baseHp" };

    public static void SetARandomPilotStats() 
    {
        int randomNumber = Random.Range(0,5);
        int randomPilot = Random.Range(1,71);
        Pilot pilotAI = PilotsDataManager.Instance.SelectPilot(randomPilot);
        var item = pilotAI.stats.GetType().GetField(statsNamesVariables[randomNumber])?.GetValue(pilotAI.stats);
        var item2 = System.Type.GetType("Constants")?.GetField(constantsNamesVariables[randomNumber])?.GetValue(null);
        if (!CheckCanUpdate(GetFractionalOfStat(pilotAI.stats, statsNamesVariables[randomNumber], constantsNamesVariables[randomNumber])))
            return;
        float result = float.Parse(item.ToString()) + float.Parse(item2.ToString());
        int? intResult = null;
        if (int.TryParse(result.ToString(), out int parsed))
            intResult = parsed;
        if (intResult != null)
            pilotAI.stats.GetType().GetField(statsNamesVariables[randomNumber])?.SetValue(pilotAI.stats, (intResult));
        else
            pilotAI.stats.GetType().GetField(statsNamesVariables[randomNumber])?.SetValue(pilotAI.stats, (result));
        PilotsDataManager.Instance.UpdatePilot(pilotAI);
    }

    public static int GetFractionalOfStat(MarbleStats _stats, string nameStat, string nameConstant)
    {
        var item = _stats.GetType()?.GetField(nameStat)?.GetValue(_stats);
        // Tip is the same Function above but for static
        var type = System.Type.GetType("Constants")?.GetField(nameConstant)?.GetValue(null);
        float sum = float.Parse(item.ToString()) / float.Parse(type.ToString());
        return (int)sum;
    }

    public static bool CheckCanUpdate(int valueOfState) => valueOfState < Constants.fractionStats;
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RandomToken
{
    public static string CreateRandomToken(int length = 6)
    {
        System.Random random = new System.Random();
        return new string(Enumerable.Repeat(CharsIdMatch, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static string CharsIdMatch => "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
}

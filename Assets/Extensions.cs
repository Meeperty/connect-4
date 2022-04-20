using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static int LargestValue(this ICollection<int> list)
    {
        int num = 0;
        foreach (var item in list)
        {
            if (item > num) { num = item; }
        }
        return num;
    }
}

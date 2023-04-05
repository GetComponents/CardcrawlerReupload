using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static T Random<T>(this List<T> _list)
    {
        return _list[UnityEngine.Random.Range(0, _list.Count)];
    }

    public static T Random<T>(this T[] _array)
    {
        return _array[UnityEngine.Random.Range(0, _array.Length)];
    }
}

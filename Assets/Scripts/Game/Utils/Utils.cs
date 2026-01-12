using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = System.Random;

public static class Utils
{
    public static T WeightedRandom<T>(List<Tuple<T, int>> choices)
    {
        Random rand = new Random();

        int totalWeight = choices.Sum(c => c.Item2); // Total probability sum (100)
        int randomNum = rand.Next(0, totalWeight);

        int cumulative = 0;
        foreach (var choice in choices)
        {
            cumulative += choice.Item2;
            if (randomNum < cumulative)
                return choice.Item1;
        }

        return choices[0].Item1;
    }

    public static List<T> ListShuffled<T>(List<T> listToShuffle)
    {
        Random rand = new Random();

        for (int i = listToShuffle.Count - 1; i > 0; i--)
        {
            var k = rand.Next(i + 1);
            var value = listToShuffle[k];
            listToShuffle[k] = listToShuffle[i];
            listToShuffle[i] = value;
        }

        return listToShuffle;
    }

    public static T GetRandomElements<T>(this List<T> list)
    {
        Random rand = new Random();
        int index = rand.Next(0, list.Count);
        return list[index];
    }

    public static string Show(this TimeSpan time)
    {
        if (time.TotalMilliseconds == 0)
            return "0s";
        StringBuilder str = new StringBuilder();
        if (time.Days > 0) str.Append($"{time.Days}d");
        if (time.Hours > 0) str.Append($" {time.Hours}h");
        if (time.Minutes > 0) str.Append($" {time.Minutes}m");
        if (time.Hours == 0)
        {
            if (time.Seconds > 0)
                str.Append($" {time.Seconds}");
            else if (time.Milliseconds > 0)
                str.Append("0");
            int ms = time.Milliseconds / 100;
            if (time.Minutes == 0 && ms > 0)
                str.Append($".{ms}");
            if (time.Seconds > 0 || ms > 0)
                str.Append("s");
        }

        return str.ToString();
    }

    public static string ShowLess(this TimeSpan time, bool isLowcase = false)
    {
        char second = isLowcase ? 's' : 'S';
        char minute = isLowcase ? 'm' : 'M';
        char hours = isLowcase ? 'h' : 'H';
        char day = isLowcase ? 'd' : 'D';

        if (time.TotalMilliseconds == 0)
            return $"0{second}";
        StringBuilder str = new StringBuilder();

        if (time.Days > 0)
        {
            str.Append($"{time.Days}{day}");
            str.Append($" {time.Hours}{hours}");
            return str.ToString();
        }

        if (time.Hours > 0)
        {
            str.Append($"{time.Hours}{hours}");
            str.Append($" {time.Minutes}{minute}");
            return str.ToString();
        }

        if (time.Minutes > 0)
        {
            str.Append($"{time.Minutes}{minute}");
            str.Append($" {time.Seconds}{second}");
            return str.ToString();
        }

        if (time.Seconds > 0)
        {
            str.Append($"{time.Seconds}{second}");
            return str.ToString();
        }

        return str.ToString();
    }

    public static string ShowTime(this TimeSpan time)
    {
        if (time.TotalMilliseconds == 0)
            return $"0";
        StringBuilder str = new StringBuilder();

        if (time.Hours > 0)
        {
            str.Append($"{time.Hours}:");

            if (time.Minutes < 10) str.Append("0");
            str.Append($"{time.Minutes}:");

            if (time.Seconds < 10) str.Append("0");
            str.Append($"{time.Seconds}");
            return str.ToString();
        }

        if (time.Minutes > 0)
        {
            str.Append($"{time.Minutes}:");

            if (time.Seconds < 10) str.Append("0");
            str.Append($"{time.Seconds}");
            return str.ToString();
        }

        if (time.Seconds > 0)
        {
            str.Append($"0:");

            if (time.Seconds < 10) str.Append("0");
            str.Append($"{time.Seconds}");
            return str.ToString();
        }

        return str.ToString();
    }

    public static bool CheckLayerMaskCollier(LayerMask layerMask, Collider other)
    {
        if ((layerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static int FloatToInt(float value)
    {
        if ((value + 0.5f) >= Mathf.CeilToInt(value))
        {
            return Mathf.CeilToInt(value);
        }
        else
        {
            return Mathf.FloorToInt(value);
        }
    }

    public static float RandomFromVector(Vector2 value)
    {
        return UnityEngine.Random.Range(value.x, value.y);
    }

    public static string FormatWithSuffix(long value)
    {
        // Billion
        if (value >= 1_000_000_000)
            return (value / 1_000_000_000f).ToString("0.#") + "B";
        // Million
        if (value >= 1_000_000)
            return (value / 1_000_000f).ToString("0.#") + "M";
        // Ten Thousands
        if (value >= 10_000)
            return (value / 1_000f).ToString("0.#") + "K";

        return value.ToString();
    }

    public static float MapValue(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
    }

    public static bool IsColorSimilar(Color a, Color b, float disMax = 0.1f)
    {
        float distance = Vector3.Distance(
            new Vector3(a.r, a.g, a.b),
            new Vector3(b.r, b.g, b.b)
        );
        return distance < disMax;
    }

    public static Vector3 ToVector3(this Vector2Int v)
    {
        return new Vector3(v.x, 0f, v.y);
    }
}

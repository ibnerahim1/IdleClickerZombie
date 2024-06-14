using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;

namespace Utils.Extensions
{
    public static class ParseExtension
    {
        public static string ToCurrency(float val)
        {
            string str = null;

            if (val < 1000)
                str = Mathf.RoundToInt(val).ToString();
            else if (val < 1000000)
                str = Round2Frac(val / 1000) + "K";
            else if (val < 1000000000)
                str = Round2Frac(val / 1000000) + "M";
            else if (val < 1000000000000)
                str = Round2Frac(val / 1000000000) + "B";

            return str;
        }
        static string Round2Frac(float val)
        {
            if (Mathf.Round(val) < 10)
            {
                return (Mathf.Round(val * 100) / 100).ToString();
            }
            else if (Mathf.Round(val) < 100)
            {
                return (Mathf.Round(val * 10) / 10).ToString();
            }
            else
                return Mathf.Round(val).ToString();
        }
    }
}
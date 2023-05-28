using System;
using UnityEngine;

namespace SQL_Quest.Extensions
{
    public static class PlayerPrefsExtensions
    {
        public static void SetVector3(string key, Vector3 value)
        {
            PlayerPrefs.SetFloat($"{key}_X", value.x);
            PlayerPrefs.SetFloat($"{key}_Y", value.y);
            PlayerPrefs.SetFloat($"{key}_Z", value.z);
        }

        public static Vector3 GetVector3(string key)
        {
            return new Vector3(
                PlayerPrefs.GetFloat($"{key}_X"),
                PlayerPrefs.GetFloat($"{key}_Y"),
                PlayerPrefs.GetFloat($"{key}_Z"));
        }

        public static void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, Convert.ToInt32(value));
        }

        public static bool GetBool(string key)
        {
            return Convert.ToBoolean(PlayerPrefs.GetInt(key));
        }
    }
}

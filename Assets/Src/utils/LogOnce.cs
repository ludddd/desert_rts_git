using System.Collections.Generic;
using UnityEngine;

namespace utils
{
    public static class LogOnce
    {
        private static HashSet<object> alreadyLogged = new HashSet<object>();

        public static void LogWarning(object message)
        {
            if (alreadyLogged.Contains(message)) return;
            alreadyLogged.Add(message);
            Debug.LogWarning(message);
        }
    }
}

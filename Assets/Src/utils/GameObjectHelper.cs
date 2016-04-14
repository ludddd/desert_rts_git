using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace utils
{
    public static class GameObjectHelper
    {
        public static IEnumerable<T> GetComponentsWithInterfaceInChildren<T>(this GameObject self, bool includeInactive) where T :class
        {
            return from component in self.GetComponentsInChildren(typeof(Component), includeInactive)
                   where component is T
                   select component as T;
        }

        public static IEnumerable<T> GetComponentsWithInterface<T>(this GameObject self) where T :class
        {
            return from component in self.GetComponents<Component>()
                where component is T
                select component as T;
        }

        public static T GetComponentWithInterface<T>(this GameObject self) where T : class
        {
            var list = GetComponentsWithInterface<T>(self).ToList();
            return list.Any() ? list.First() : null;
        }

        public static IEnumerable<T> GetAllComponentsWithInterface<T>() where T:class
        {
            var rez = new List<T>();
            foreach (var obj in Object.FindObjectsOfType<GameObject>())
            {
                rez.AddRange(obj.GetComponentsWithInterface<T>());
            }
            return rez;
        }
    }
}

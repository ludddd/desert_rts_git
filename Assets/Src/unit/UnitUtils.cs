using System.Collections.Generic;
using UnityEngine;

namespace unit
{
    static class UnitUtils
    {
        public static bool IsUnit(this GameObject self)
        {
            return self.CompareTag(Tags.Unit);
        }

        public static IEnumerable<GameObject> GetAllUnits()
        {
            return GameObject.FindGameObjectsWithTag(Tags.Unit);
        } 
    }
}

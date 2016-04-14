using System.Collections.Generic;
using System;
using UnityEngine;

namespace utils
{
    [Serializable]
    public class MultipleObjectLink
    {
        public List<GameObject> objects;    //cause there is not PropertyDrawers for arrays....
    }
}

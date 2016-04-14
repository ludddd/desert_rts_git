using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace utils
{
    //To make UnitEvent with arguments serializable (http://forum.unity3d.com/threads/unityevent-t1-will-be-ever-serialized.263761/)
    [Serializable]
    public class UnityEventWithGameObject : UnityEvent<GameObject> { };
    [Serializable]
    public class UnityEventWithVector2 : UnityEvent<Vector2> { };
    [Serializable]
    public class UnityEventWithVector3 : UnityEvent<Vector3> { };
    [Serializable]
    public class UnityEventWithVector2List : UnityEvent<IList<Vector2>> { };
    [Serializable]
    public class UnityEventWithVector3List : UnityEvent<IList<Vector3>> { };
    [Serializable]
    public class UnityEventWithBool : UnityEvent<bool> { };
    [Serializable]
    public class UnityEventWithString: UnityEvent<String> { };
}

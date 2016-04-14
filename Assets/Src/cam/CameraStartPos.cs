using UnityEngine;

namespace cam
{
    class CameraStartPos: MonoBehaviour
    {
        private void Start()
        {
            var camPos = Camera.main.transform.position;
            camPos.x = transform.position.x;
            camPos.z = transform.position.z;
            Camera.main.transform.position = camPos;
        }
    }
}

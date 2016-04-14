using UnityEngine;
using UnityEngine.Events;

namespace utils
{
    public class FunctionDelegate: MonoBehaviour
    {
        public UnityEvent func;

        public void CallDelegatedFunc()
        {
            func.Invoke();
        }
    }
}

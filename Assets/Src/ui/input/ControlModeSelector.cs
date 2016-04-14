using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ui
{
    namespace input
    {

        public class ControlModeSelector : MonoBehaviour
        {
            public class IMode: MonoBehaviour
            {
                public void Activate()
                {
                    ModeSelector.SetMode(gameObject);
                }

                public void SetActive(bool fActive)
                {
                    gameObject.SetActive(fActive);
                }

                public ControlModeSelector ModeSelector
                {
                    get {
                        return gameObject.GetComponentInParent<ControlModeSelector>();
                    }
                }
            };

            void Awake()
            {
                SetDefaultMode();
            }

            private IEnumerable<IMode> Modes()
            {
                return (from Transform child in transform where child.GetComponent<IMode>() != null
                            select child.GetComponent<IMode>());
            }
            
            public void SetDefaultMode()
            {
                SetMode<ModeIdle>();
            }

            public void SetMode<T>()
            {
                foreach (IMode m in Modes()) {
                    m.SetActive(m is T);                   
                }
            }

            public void SetMode(GameObject mode)
            {
                foreach (IMode m in Modes()) {
                    m.SetActive(m.gameObject.GetInstanceID() == mode.GetInstanceID());
                }
            }
        }

    }
}

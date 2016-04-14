using UnityEngine;
using utils;
using ui.input.gesture;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace ui.input
{
    public class ModeIdle : ControlModeSelector.IMode
    {
        [SerializeField]
        private UnityEventWithGameObject unitClicked = null;
        [SerializeField]
        private UnityEventWithGameObject unitDoubleClicked = null;
        [SerializeField]
        private UnityEventWithGameObject unitTripleClicked = null;
        [SerializeField]
        private UnityEventWithVector3 groundClicked = null;
        [SerializeField]
        private UnityEvent longTouch = null;
        [SerializeField]
        private UnityEvent startDraw = null;


        private IList<IUpdatable> userActions = new List<IUpdatable>();
        private IList<Action> signalledActions = new List<Action>();

        public ModeIdle()
        {
            userActions.Add(new LongTouch(() => AddEvent(longTouch)));
            userActions.Add(new Draw(() => AddEvent(startDraw)));
            userActions.Add(new UnitClick(new Action<GameObject>[] {
                obj => AddEvent(unitClicked, obj),
                obj => AddEvent(unitDoubleClicked, obj),
                obj => AddEvent(unitTripleClicked, obj),
            }));
            userActions.Add(new GroundClick(pos => AddEvent(groundClicked, pos)));
        }

        private void AddEvent(UnityEvent ev)
        {
            signalledActions.Add(() => ev.Invoke());            
        }

        private void AddEvent<T>(UnityEvent<T> ev, T arg)
        {
            signalledActions.Add(() => ev.Invoke(arg));
        }

        public void Update()
        {
            foreach(var ua in userActions) {
                ua.Update();
            }
            if (signalledActions.Count > 0) {
                signalledActions.First().Invoke();
                signalledActions.Clear();
            }
        }
    }
}




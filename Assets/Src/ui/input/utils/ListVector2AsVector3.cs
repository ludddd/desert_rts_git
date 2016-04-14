using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ui.input.utils
{
    public class ListVector2AsVector3: IList<Vector2>
    {
        private readonly IList<Vector3> data = new List<Vector3>();
        private readonly Collider collider;
        private readonly Camera camera;

        public ListVector2AsVector3(Collider collider, Camera camera)
        {
            Debug.Assert(collider != null);
            Debug.Assert(camera != null);
            this.collider = collider;
            this.camera = camera;
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            return new Enumerator(data.GetEnumerator(), ToVector2);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void Add(Vector2 item)
        {
            var v = ToVector3(item);
            data.Add(v);
        }

        public void Clear()
        {
            data.Clear();
        }

        public bool Contains(Vector2 item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Vector2[] array, int arrayIndex)
        {
            for (int idx = arrayIndex; idx < data.Count; idx++)
            {
                array[idx] = ToVector2(data[idx]);
            }
        }

        public bool Remove(Vector2 item)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return data.Count; }
        }
        public bool IsReadOnly { get { return false; } }
        public int IndexOf(Vector2 item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, Vector2 item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public Vector2 this[int index]
        {
            get { return ToVector2(data[index]); }
            set { throw new NotImplementedException(); }
        }

        private Vector3 ToVector3(Vector2 v)
        {
            RaycastHit hitInfo;
            var ray = camera.ScreenPointToRay(new Vector3(v.x, v.y));
            if (collider.Raycast(ray, out hitInfo, camera.farClipPlane))
            {
                return hitInfo.point;
            }
            //TODO: think about better solution for this case
            throw new Exception("camera should look on the terrain");
            /*
            var plane = new Plane(Vector3.up, Vector3.zero);    
            float dist;
            if (!plane.Raycast(ray, out dist))
            {
                Debug.Assert(false, "Camera should look down and be above y==0 plane");
            }
            return ray.GetPoint(dist);*/
        }

        private Vector2 ToVector2(Vector3 v)
        {
            var rez = camera.WorldToScreenPoint(v);
            return new Vector2(rez.x, rez.y);
        }

        private class Enumerator: IEnumerator<Vector2>
        {
            private readonly IEnumerator<Vector3> impl;
            private readonly Func<Vector3, Vector2> toVector2;

            public Enumerator(IEnumerator<Vector3> impl, Func<Vector3, Vector2> toVector2)
            {
                this.impl = impl;
                this.toVector2 = toVector2;
            }

            public bool MoveNext()
            {
                return impl.MoveNext();
            }

            public void Reset()
            {
                impl.Reset();
            }

            public Vector2 Current
            {
                get { return toVector2(impl.Current); } 
                private set { throw new NotImplementedException(); }                 
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Dispose()
            {
                impl.Dispose();
            }
        }
    }
}

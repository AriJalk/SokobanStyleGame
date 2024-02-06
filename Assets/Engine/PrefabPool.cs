using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace EDBG.Engine.ResourceManagement
{
    public class PrefabPool
    {
        private Queue<GameObject> queue;

        public PrefabPool()
        {
            queue = new Queue<GameObject>();
        }

        public GameObject RetrieveQueueObject()
        {
            if (queue == null)
                return null;
            if (queue.Count == 0)
                return null;
            return queue.Dequeue();
        }


        public void AddQueueObject(GameObject newObj)
        {
            queue.Enqueue(newObj);
        }
    }

}
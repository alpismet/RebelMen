using System;
using System.Collections;
using System.Collections.Generic;
using Dev.Scripts.Tools;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Dev.Scripts.Systems
{   
    [Serializable]
    public class PoolSystem<T> : MonoBehaviour
    {
        [FoldoutGroup("Pool"), SerializeField] 
        private List<T> pool = new List<T>();

        [BoxGroup("References"), SerializeField] 
        private GameObject poolObject;

        [PropertySpace(20)]
        [HorizontalGroup("Generate", LabelWidth = 100, MaxWidth = 100), SerializeField] 
        private int objectAmount;

        private Queue _poolQueue;



        //---------------------------------------------------------------------------------
    #if UNITY_EDITOR
        [PropertySpace(20), HorizontalGroup("Generate"), Button]
        private void GeneratePoolObjects()
        {
            transform.DestroyChildren();

            List<T> newPool = new List<T>();
            for (int i = 0; i < objectAmount; i++)
            {
                var item = PrefabUtility.InstantiatePrefab(poolObject, transform) as GameObject;
                if (item != null)
                    newPool.Add(item.GetComponent<T>());
            }
            SetPool(newPool);
        }
    #endif



        //---------------------------------------------------------------------------------
        protected void SetPool(List<T> newPool) => pool = newPool;


        //---------------------------------------------------------------------------------
        public T GetItem()
        {
            if (_poolQueue == null)
            {
                _poolQueue = new Queue();
                pool.ForEach(i => _poolQueue.Enqueue(i));
            }
            T item = (T) _poolQueue.Dequeue();
            _poolQueue.Enqueue(item);
            return item;
        }


        //---------------------------------------------------------------------------------
        public List<T> GetAll() => pool;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace MyUtils
{
    public class PoolingHelper : MonoBehaviour
    {
         public static List<PoolObjectInfor> ObjectPools = new();

        public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
        {
            PoolObjectInfor pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);

            //If the pool isn't exist, create it
            if (pool == null)
            {
                pool = new PoolObjectInfor() { LookupString = objectToSpawn.name };
                ObjectPools.Add(pool);
            }

            //Check if there are any inactive object in the pool
            GameObject spawnableObject = null;
            if (pool.InactiveObjects.Count > 0)
            {
                spawnableObject = pool.InactiveObjects.FirstOrDefault();
            }

            if (spawnableObject == null)
            {
                spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
            }
            else
            {
                spawnableObject.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
                pool.InactiveObjects.Remove(spawnableObject);
                spawnableObject.SetActive(true);

            }

            spawnableObject.name = objectToSpawn.name;
            return spawnableObject;
        }
        public static GameObject SpawnObject(GameObject objectToSpawn, Transform parent, Vector3 spawnPosition, Quaternion spawnRotation)
        {
            PoolObjectInfor pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);

            //If the pool isn't exist, create it
            if (pool == null)
            {
                pool = new PoolObjectInfor() { LookupString = objectToSpawn.name };
                ObjectPools.Add(pool);
            }

            //Check if there are any inactive object in the pool
            GameObject spawnableObject = null;
            if (pool.InactiveObjects.Count > 0)
            {
                spawnableObject = pool.InactiveObjects.FirstOrDefault();
            }

            if (spawnableObject == null)
            {
                spawnableObject = Instantiate(objectToSpawn, parent);
            }
            else
            {
                spawnableObject.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
                pool.InactiveObjects.Remove(spawnableObject);
                spawnableObject.SetActive(true);

            }

            spawnableObject.name = objectToSpawn.name;
            return spawnableObject;
        }
        public static GameObject SpawnObject(GameObject objectToSpawn, Transform parent)
        {
            PoolObjectInfor pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);

            //If the pool isn't exist, create it
            if (pool == null)
            {
                pool = new PoolObjectInfor() { LookupString = objectToSpawn.name };
                ObjectPools.Add(pool);
            }

            //Check if there are any inactive object in the pool
            GameObject spawnableObject = null;
            if (pool.InactiveObjects.Count > 0)
            {
                spawnableObject = pool.InactiveObjects.FirstOrDefault();
            }

            if (spawnableObject == null)
            {
                spawnableObject = Instantiate(objectToSpawn, parent);
            }
            else
            {
                pool.InactiveObjects.Remove(spawnableObject);
                spawnableObject.SetActive(true);

            }

            spawnableObject.name = objectToSpawn.name;
            return spawnableObject;
        }

        public static void ReturnObjectToPool(GameObject objectToPool)
        {
            //Debug.Log("ReturnObjectToPool " + objectToPool.name);
            string objectToPoolName = objectToPool.name.Replace("(Clone)", "").Trim();
            objectToPool.transform.parent = objectToPool.transform.root;
            PoolObjectInfor pool = ObjectPools.Find(p => p.LookupString.Equals(objectToPoolName));

            //If the pool isn't exist, create it
            if (pool == null)
            {
                Debug.LogWarning("Release an object that is not pooled!");
                pool = new PoolObjectInfor() { LookupString = objectToPool.name };
                ObjectPools.Add(pool);
            }
            objectToPool.SetActive(false);
            pool.InactiveObjects.Add(objectToPool);

        }
    }
    [Serializable]
    public class PoolObjectInfor
    {
        public string LookupString;
        public List<GameObject> InactiveObjects = new();

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;
    public List<GameObject> collectorPool, hunterPool, seekerPool;
    public int poolAmount;
    public GameObject collectorPrefab, hunterPrefab, seekerPrefab;
    // Start is called before the first frame update
    private void Awake()
    {
        SharedInstance = this;
    }
    void Start()
    {
        collectorPool = new List<GameObject>();
        hunterPool = new List<GameObject>();
        seekerPool = new List<GameObject>();
        for(int i = 0; i<poolAmount; i++)
        {
            GameObject cObj = Instantiate(collectorPrefab);
            GameObject hObj = Instantiate(hunterPrefab);
            GameObject sObj = Instantiate(seekerPrefab);
            cObj.SetActive(false);
            hObj.SetActive(false);
            sObj.SetActive(false);
            collectorPool.Add(cObj);
            hunterPool.Add(hObj);
            seekerPool.Add(sObj);
        }
    }

    // Update is called once per frame
    public GameObject GetCollector()
    {
        GameObject o=  null;
        //1
        for (int i = 0; i < collectorPool.Count; i++)
        {
            //2
            if (!collectorPool[i].activeInHierarchy)
            {
                o = collectorPool[i];
            }
        }
        //3   
        return o;
    }

    public GameObject GetHunter()
    {
        GameObject o = null;
        //1
        for (int i = 0; i < hunterPool.Count; i++)
        {
            //2
            if (!hunterPool[i].activeInHierarchy)
            {
                o = hunterPool[i];
            }
        }
        //3   
        return o;
    }

    public GameObject GetSeeker()
        {
        GameObject o = null;
        //1
        for (int i = 0; i < seekerPool.Count; i++)
        {
            //2
            if (!seekerPool[i].activeInHierarchy)
            {
                o = seekerPool[i];
            }
        }
        //3   
        return o;
    }
}

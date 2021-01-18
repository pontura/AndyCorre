using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkManager : MonoBehaviour
{
    public GameObject endingAsset;
    public TerrainAsset asset;
    float last_z = 0;
    float terrainSize = 100;
    int id;

    void Start()
    {
        
    }
    public void OnUpdate(float distance)
    {
        Vector3 pos;
        if (distance > last_z + terrainSize)
        {
            id++;
            last_z = terrainSize * id;           
            pos = asset.transform.position;
            pos.z = last_z;
            asset.transform.position = pos;
        }
        pos = endingAsset.transform.position;
        pos.z = distance;
        endingAsset.transform.position = pos;
    }
}

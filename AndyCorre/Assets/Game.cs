using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public float distance;
    public CharacterRunningManager character;
    ParkManager parkManager;

    void Start()
    {
        parkManager = GetComponent<ParkManager>();
    }
    private void Update()
    {
        distance = character.transform.position.z;
        parkManager.OnUpdate(distance);
    }


}

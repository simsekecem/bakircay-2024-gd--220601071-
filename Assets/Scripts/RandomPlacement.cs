using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlacement : MonoBehaviour
{

    public GameObject[] objects;

    void Start()
    {
        objects = GameObject.FindGameObjectsWithTag("Objects");

        foreach (GameObject nesne in objects)
        {
            nesne.transform.position = new Vector3(Random.Range(-3f, 3f), 5f, Random.Range(-2.3f, 4.3f));
        }
    }
}

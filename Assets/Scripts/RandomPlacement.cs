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
            nesne.transform.position = new Vector3(Random.Range(-2f, 2f), 5f, Random.Range(-3f, 4f));
        }
    }
}

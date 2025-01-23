using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteItems : MonoBehaviour
{
    public Button deleteButton; 

    public void DeleteAllObjects()
    {
        StartCoroutine(DeleteAndWait());
    }

    private IEnumerator DeleteAndWait()
    {
       
        if (deleteButton != null)
        {
            deleteButton.interactable = false;
        }

       
        GameObject[] objectsToDelete = GameObject.FindGameObjectsWithTag("Moveable");


        foreach (GameObject obj in objectsToDelete)
        {
            ParticleSystem fireEffect = obj.GetComponentInChildren<ParticleSystem>();

            if (fireEffect != null)
            {
                fireEffect.Play();
            }

            Destroy(obj, fireEffect != null ? fireEffect.main.duration : 0f);
        }

        yield return new WaitForSeconds(4f); 

        Debug.Log(objectsToDelete.Length + " nesne silindi.");

        yield return new WaitForSeconds(5f);

        if (deleteButton != null)
        {
            deleteButton.interactable = true;
        }
    }
}

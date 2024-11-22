using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraySettings : MonoBehaviour
{
    public GameObject selectedObject; 
    public float launchForce = 10f;  
    public Transform launchDirection; 

    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Objects"))
        {
            
            if (selectedObject == null)
            {
                Debug.Log("The object has entered the tray: " + other.gameObject.name);
                selectedObject = other.gameObject;
            }
            else
            {
               
                Debug.Log("The tray is full. The object is removed: " + other.gameObject.name);

                
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = other.gameObject.AddComponent<Rigidbody>();
                }

                
                Vector3 direction = (launchDirection != null) ? launchDirection.forward : Vector3.forward;
                rb.AddForce(direction * launchForce, ForceMode.Impulse);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject == selectedObject)
        {
            Debug.Log("The object has left the tray: " + other.gameObject.name);
            selectedObject = null;
        }
    }

}
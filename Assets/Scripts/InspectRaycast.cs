using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectRaycast : MonoBehaviour
{
    [SerializeField] int rayLength = 5;
    [SerializeField] LayerMask layerMaskInteract;
    ObjectController raycastedObj;

    [SerializeField] private Image crosshair;
    bool isCrosshairActive;
    bool doOnce;

    void Update()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if(Physics.Raycast(transform.position, fwd, out hit, rayLength, layerMaskInteract.value))
        {
            if(hit.collider.CompareTag("InteractObject"))
            {
                if(!doOnce)
                {
                    raycastedObj = hit.collider.gameObject.GetComponent<ObjectController>();

                    CrosshairChange(true);
                }

                isCrosshairActive = true;
                doOnce = true;

                if(Input.GetKeyDown(KeyCode.E))
                {
                    
                }
            }

        }
        else
        {
            if(isCrosshairActive)
            {
                CrosshairChange(false);
                doOnce = false;
            }
        }
    }

    void CrosshairChange(bool on) 
    {
        if(on && !doOnce)
        {
            crosshair.color = Color.red;
        }

        else
        {
            crosshair.color = Color.white;
            isCrosshairActive = false;
        }
    }
}

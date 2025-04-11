using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastHover : MonoBehaviour
{
    [SerializeField] Camera mainCamera;

    Coordinateslabler coordinateslabler;
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        RayCastHoverEffect();
    }

    void RayCastHoverEffect()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Coordinateslabler newLabel = hit.collider.GetComponentInChildren<Coordinateslabler>();

            if (newLabel != null && newLabel != coordinateslabler)
            {
                if (coordinateslabler != null)
                {
                    coordinateslabler.HideLabel();
                }
            }

            if (newLabel != null)
            {
                newLabel.ShowLabel();
                coordinateslabler = newLabel;
            }
        }
        else 
        { 
            if (coordinateslabler != null)
            {
                coordinateslabler.HideLabel();
                coordinateslabler = null;
            }
        }
    }
}

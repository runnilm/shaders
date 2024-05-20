using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    public Vector3 screenPosition;
    public Vector3 worldPosition;
    public LayerMask layersToHit;
    
    void Update()
    {
        screenPosition = Input.mousePosition;
    }
    
    public Vector3 getMousePositionWorld() {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if(Physics.Raycast(ray, out RaycastHit hit, 500, layersToHit)) {
            worldPosition = hit.point;
        }

        return worldPosition;
    }
}

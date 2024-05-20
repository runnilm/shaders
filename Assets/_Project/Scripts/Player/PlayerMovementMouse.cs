using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementMouse : MonoBehaviour
{
    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            GetComponent<MovePositionAStar>().setMovePosition(
                GetComponent<MousePosition>().getMousePositionWorld(),
                () => {}
            );
        }
    }
}

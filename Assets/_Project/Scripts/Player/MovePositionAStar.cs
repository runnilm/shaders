using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class MovePositionAStar : MonoBehaviour, IMovePosition
{
    private AIPath aIPath;

    private void Awake() {
        aIPath = GetComponent<AIPath>();
    }

    public void setMovePosition(Vector3 movePosition, Action onReachedMovePosition) {
        aIPath.destination = movePosition;
        Debug.Log(aIPath.destination);
    }
}

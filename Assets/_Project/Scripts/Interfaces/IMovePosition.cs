using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovePosition
{
    void setMovePosition(Vector3 movePosition, Action onReachedMovePosition);
}

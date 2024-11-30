using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMoveable
{
    Rigidbody2D Rigidbody { get; set; }

    void Move(Vector2 velocity);

    void CheckForLeftOrRightFacing(Vector2 velocity);
}

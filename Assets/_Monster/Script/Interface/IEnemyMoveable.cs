using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMoveable
{
    Rigidbody2D RB { get; set; }

    void Move(Vector2 velocity);

    bool IsFacingRight {  get; set; }

    void CheckForLeftOrRightFacing(Vector2 velocity);
}

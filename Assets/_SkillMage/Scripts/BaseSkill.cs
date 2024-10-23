using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseSkill
{
    public void SetData();
    public void Hit();
    public void SetDamage();
    public void EndSkill();
}

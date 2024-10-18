using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseSkill
{
    public void SetData(object skillData);
    public void Hit();
    public void SetDamage();
    public void EndSkill();
}

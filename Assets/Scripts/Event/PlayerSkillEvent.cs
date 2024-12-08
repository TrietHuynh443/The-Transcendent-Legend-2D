using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillEvent
{
    public PlayerSkillType Type {get; set;} = PlayerSkillType.None;
}
public enum PlayerSkillType
{
    None,
    Toxic = 0,   
}
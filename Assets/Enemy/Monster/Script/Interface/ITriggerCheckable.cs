using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerCheckable { 
    bool IsAggroed { get; set; }
    bool IsWithInStrikingDistance { get; set; }
    void SetAggroStatus (bool aggroStatus);
    void SetStrikingDistanceBool (bool strikingDistanceBool);
}

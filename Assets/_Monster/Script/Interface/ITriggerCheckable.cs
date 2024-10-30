using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerCheckable { 
    bool isAggroed { get; set; }
    bool isWithInStrikingDistance { get; set; }
    void SetAggroStatus (bool aggroStatus);
    void SetStrikingDistanceBool (bool strikingDistanceBool);
}

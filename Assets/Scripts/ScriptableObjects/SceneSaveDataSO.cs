using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/SceneSaveDataSO", fileName = "new SceneSaveDataSO")]
public class SceneSaveDataSO : ScriptableObject 
{
    public string SceneName {get; set;}
    public Vector3 CheckPointPos {get; set;}

}
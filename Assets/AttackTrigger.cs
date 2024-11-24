using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    [SerializeField] private string layerName;
    private int _layer;
    private static Dictionary<int, int[]> LayersDamageMap;

    public float AttackDamage { get; set; } = 0;

    private void Awake()
    {
        if (LayersDamageMap == null)
        {
            LayersDamageMap = new Dictionary<int, int[]>
            {
                { LayerMask.NameToLayer("Player"), new int[] {LayerMask.NameToLayer("Enemy")} },
                { LayerMask.NameToLayer("Enemy"), new int[] {LayerMask.NameToLayer("Player")} }
            };
        }
    }

    private void Start()
    {
        _layer = LayerMask.NameToLayer(layerName);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (LayersDamageMap.TryGetValue(_layer, out int[] targetLayer) &&
            targetLayer.Contains(other.gameObject.layer) &&
            other.TryGetComponent(out BaseEntity entity))
        {
            entity.TakeDamage(AttackDamage);
        }
    }
}

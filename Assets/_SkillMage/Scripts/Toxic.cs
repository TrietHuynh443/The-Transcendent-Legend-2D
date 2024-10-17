using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Toxic : MonoBehaviour
{
    [SerializeField] private string[] _layerEffectNames;
    [SerializeField] private float _delayEffectTime = 0.2f;
    [SerializeField] private float _range = 10f;
    private ParticleSystem _vfx;
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private float _vfxDuration;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.LogWarning(other.name);
        StopAllCoroutines();
        string layerName = LayerMask.LayerToName(other.gameObject.layer);
        if (_layerEffectNames.Contains(layerName))
        {
            Explode();
        }
    }

    private void EndEplodeAnimationEvent()
    {
        StartCoroutine(PerformSkill());
       
    }

    private IEnumerator PerformSkill()
    {
        _animator.StopRecording();
        if (_vfx != null) _vfx.Play();
        yield return new WaitForSeconds(_vfxDuration);
        gameObject.SetActive(false);
    }

    private void Explode()
    {
        _animator.SetBool("IsExplode", true);
    }

    // Start is called before the first frame update
    void Start()
    {
        _vfx = transform.GetComponentInChildren<ParticleSystem>();
        if (_vfx != null)
        {
            _vfxDuration = _vfx.main.duration;
            Debug.Log(_vfxDuration);
        }
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.AddForce(new Vector2(1, 1) * _range, ForceMode2D.Force);
    }


}

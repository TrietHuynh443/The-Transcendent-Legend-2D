using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Toxic : MonoBehaviour, IBaseSkill
{
    [SerializeField] private string[] _layerEffectNames;
    [SerializeField] private float _delayEffectTime = 0.2f;
    private SkillData _skillData;
    private ParticleSystem _vfx;
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private float _vfxDuration;

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        StopAllCoroutines();
        string layerName = LayerMask.LayerToName(other.gameObject.layer);
        if (_layerEffectNames.Contains(layerName))
        {
            _animator.SetBool("IsExplode", true);
            if (other.TryGetComponent(out BaseEntity entity))
            {
                Debug.LogWarning(other.gameObject.name + " " + _skillData.Damage.GetValueOrDefault());

                Hit(entity);
            }
        }
    }

    private void EndEplodeAnimationEvent()
    {
        StartCoroutine(PerformSkill());
    }

    private IEnumerator PerformSkill()
    {
        // _animator.StopRecording();
        if (_vfx != null) _vfx.Play();
        yield return new WaitForSeconds(_vfxDuration);
        EndSkill();
    }

    private void Explode()
    {
        _animator.SetBool("IsExplode", true);
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        _vfx = transform.GetComponentInChildren<ParticleSystem>();
        if (_vfx != null)
        {
            _vfxDuration = _vfx.main.duration;
            Debug.Log(_vfxDuration);
        }
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetData()
    {
        var toxicParam = new DataFilterParams();
        toxicParam.Name = "Toxic";
        toxicParam.Type = GameDataType.SKILL;
        var res = GameDataManager.Instance.GetData(toxicParam);
        if (res.Count >= 1)
        {
            _skillData = (SkillData)res[0];
        }
        Debug.Log(_skillData.Name);
    }

    public void Hit(BaseEntity entity)
    {
        // yield return new WaitForSeconds(_delayEffectTime);
        entity.TakeDamage(_skillData.Damage.GetValueOrDefault());

    }

    public void SetDamage()
    {

    }

    public void EndSkill()
    {
        transform.parent.gameObject.SetActive(false);
        _animator.SetBool("IsExplode", false);
    }

    private void Start()
    {
        SetData();
    }

    public void Hit()
    {
    }
}

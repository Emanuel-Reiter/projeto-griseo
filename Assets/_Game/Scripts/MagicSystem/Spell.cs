using DG.Tweening;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D))]
public class Spell : MonoBehaviour
{
    [Header("Spell Data")]
    [SerializeField] private SpellData _spellData;
    public void InjectSpellData(SpellData data) => _spellData = data;

    [Header("Subcast")]
    [SerializeField] private GameObject _subcastSpellPrefab;
    [SerializeField] private float _subcastAngle = 0f; 
    [SerializeField] private float _subcastIntervalMax = 0.1f;
    private float _subcastInterval;

    [Header("Visual params")]
    [SerializeField] private SpriteRenderer _spellSpriteRef;
    [SerializeField] private SpriteRenderer _sigilSpriteRef;
    [SerializeField] private ParticleSystem _castEffectRef;
    [SerializeField] private ParticleSystem _idleEffectRef;
    [SerializeField] private ParticleSystem _hitEffectPrefab;
    [SerializeField] private Light2D _light;

    [Header("SFX params")]
    [SerializeField] private AudioClip _castAudio;
    [SerializeField] private Vector2 _castPitch = Vector2.one;
    [SerializeField] private AudioClip _idleAudio;
    private AudioHandle _idleVoice;
    [SerializeField] private Vector2 _idlePitch = Vector2.one;
    [SerializeField] private AudioClip _wallHitAudio;
    [SerializeField] private Vector2 _wallHitPitch = Vector2.one;

    private Rigidbody2D _body;
    private CircleCollider2D _collider;

    private bool _hasDied = false;

    [SerializeField] private LayerMask _hitLayers;

    private int _fizzleTimerIndex;

    private void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();

        if (_spellData == null)
        {
            Debug.LogError("No spell data attached to game object.");
            return;
        }

        _collider.isTrigger = true;
        
        Cast();
    }

    private void Cast()
    {
        if (_castEffectRef != null) _castEffectRef.Play();
        if (_idleEffectRef != null) _idleEffectRef.Play();
        
        if (_castAudio != null) AudioPool.Play(_castAudio, transform.position, false, 128, 1f, Random.Range(_castPitch.x, _castPitch.y), 2f, 16f, false);
        if (_idleAudio != null) _idleVoice = AudioPool.Play(_idleAudio, transform.position, false, 128, 1f, Random.Range(_idlePitch.x, _idlePitch.y), 2f, 16f, true);

        if (_sigilSpriteRef != null)
        {
            _sigilSpriteRef.transform.parent = null;
            _sigilSpriteRef.DOFade(0f, 0.5f).OnComplete(() => Destroy(_sigilSpriteRef.gameObject));
        }

        _subcastInterval = _subcastIntervalMax;

        _body.gravityScale = _spellData.GravityModifier;

        _fizzleTimerIndex = TimerManager.I.StartTimer(_spellData.MaxDuration, () => Fizzle());
    }

    private void Update()
    {
        if (_hasDied) return;
        if (_subcastSpellPrefab == null) return;

        if (_subcastInterval > 0f)
        {
            _subcastInterval -= Time.deltaTime;
        }
        else
        {
            Vector3 rot = new Vector3(0f, 0f, _subcastAngle);
            Instantiate(_subcastSpellPrefab, transform.position, Quaternion.Euler(rot), null);
            _subcastInterval = _subcastIntervalMax;
        }

    }

    private void FixedUpdate()
    {
        if (_hasDied) return;

        Quaternion dir = Quaternion.Euler(0f, 0f, _spellData.CastAngle);
        _body.linearVelocity = transform.right * _spellData.ProjectileSpeed;
    }

    private void Fizzle()
    {
        _hasDied = true;
        _body.linearVelocity = Vector2.zero;

        if (_light != null) _light.intensity = 0f;

        _idleVoice.Stop();
        if(_idleEffectRef != null) _idleEffectRef.Stop();
        if(_spellSpriteRef != null) _spellSpriteRef.DOFade(0f, 0.3f);

        TimerManager.I.StartTimer(5f, () => { Destroy(gameObject); });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hasDied) return;

        if(collision.gameObject.layer == LayerMask.NameToLayer("Entities"))
        {
            SpawnHitEffect(collision);
            Hit();
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("EnvCollisions"))
        {
            SpawnHitEffect(collision);
            Hit();
        }

        if (_wallHitAudio != null) AudioPool.Play(_wallHitAudio, transform.position, true, 129, 1f, Random.Range(_wallHitPitch.x, _wallHitPitch.y), 2f, 16f, false);
    }

    private void Hit()
    {
        _hasDied = true;
        _body.linearVelocity = Vector2.zero;

        if (_light != null) _light.intensity = 0f;

        TimerManager.I.CancelTimer(_fizzleTimerIndex);

        _idleVoice.Stop();
        if (_idleEffectRef != null) _idleEffectRef.Stop();
        if (_spellSpriteRef != null) _spellSpriteRef.DOFade(0f, 0.1f);

        TimerManager.I.StartTimer(5f, () => { Destroy(gameObject); });
    }

    private void SpawnHitEffect(Collider2D collision)
    {
        if (_hitEffectPrefab == null) return;

        Vector2 direction = (_body.linearVelocity * 1000f).normalized;
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, _collider.radius, direction, 1.0f);
       
        ParticleSystem vfx;
       
        if (hit.collider != null) vfx = Instantiate(_hitEffectPrefab, (hit.point + (direction * (_collider.radius / 2f))), Quaternion.identity, null);
        else vfx = Instantiate(_hitEffectPrefab, collision.ClosestPoint(transform.position), Quaternion.identity, null);
        
        TimerManager.I.StartTimer(5f, () => { Destroy(vfx.gameObject); });
    }
}

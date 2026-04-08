using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    private Rigidbody2D _body;
    private ProjectileCaster _caster;

    private Vector2 _constantVelocity;

    private int _fizzleTimerIndex;

    private bool _hasDied = false;
    private bool _isFacingRight;

    private CastData _castData;

    // Collision layers
    private const string ENV_LAYER = "EnvCollisions";
    private const string ENTITIES_LAYER = "Entities";

    [Header("Projectile params")]
    [SerializeField] private bool _useHitDetection = true;
    [SerializeField] private float _hitDetectionRadius = 0.5f;
    [SerializeField] private Vector2 _hitDetectionOffset = Vector2.zero;
    private float _projectileDuration;

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer _projectileSprite;
    [Space]
    [SerializeField] private SpriteRenderer _sigilSprite;

    [Header("Light params")]
    [SerializeField] private Light2D _light;

    [Header("Animations")]
    //[SerializeField] private AnimationClip _spawnAnim;
    [SerializeField] private AnimationClip _idleAnim;
    //[SerializeField] private AnimationClip _dieAnim;
    private Animator _projectileAnimator;

    [Header("VFX")]
    [SerializeField] private ParticleSystem _spawnVfx;
    [SerializeField] private ParticleSystem _idleVfx;
    [SerializeField] private ParticleSystem _fizzleVfx;
    [SerializeField] private ParticleSystem _hitVfx;

    [Header("SFX")]
    [SerializeField] private AudioClip _spawnSfx;
    [Range(0f, 1f)][SerializeField] private float _spawnSfxVolume = 1f;
    [Space]
    [SerializeField] private bool _isSpawnSfx3D = true;
    [Space]
    [Range(0.5f, 1.5f)][SerializeField] private float _spawnSfxPitchMin = 1f;
    [Range(0.5f, 1.5f)][SerializeField] private float _spawnSfxPitchMax = 1f;

    [Space]
    [Space]
    [Space]
    [SerializeField] private AudioClip _idleSfx;
    [Range(0f, 1f)][SerializeField] private float _idleSFXVolume = 1f;
    [Space]
    [SerializeField] private bool _isIdleSfx3D = true;
    [Space]
    [Range(0.5f, 1.5f)][SerializeField] private float _idleSfxPitchMin = 1f;
    [Range(0.5f, 1.5f)][SerializeField] private float _idleSfxPitchMax = 1f;
    private AudioHandle _idleVoice;

    [Space]
    [Space]
    [Space]
    [SerializeField] private AudioClip _fizzleSfx;
    [Space]
    [Range(0f, 1f)][SerializeField] private float _fizzleSfxVolume = 1f;
    [Space]
    [SerializeField] private bool _isFizzleSfx3D = true;
    [Range(0.5f, 1.5f)][SerializeField] private float _fizzleSfxPitchMin = 1f;
    [Range(0.5f, 1.5f)][SerializeField] private float _fizzleSfxPitchMax = 1f;

    [Space]
    [Space]
    [Space]
    [SerializeField] private AudioClip _hitSfx;
    [Range(0f, 1f)][SerializeField] private float _hitSfxVolume = 1f;
    [Space]
    [SerializeField] private bool _isHitSfx3D = true;
    [Range(0.5f, 1.5f)][SerializeField] private float _hitSfxPitchMin = 1f;
    [Range(0.5f, 1.5f)][SerializeField] private float _hitSfxPitchMax = 1f;

    private void SetProjectileDuration(float duration) => _projectileDuration = duration;
    private void SetGravityModifier(float gravityModifier) => _body.gravityScale = gravityModifier;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _caster = GetComponent<ProjectileCaster>();
        TryGetComponent<Animator>(out _projectileAnimator);
    }

    private void FixedUpdate()
    {
        if (_hasDied) return;
        if (_constantVelocity == Vector2.zero) return;

        _body.linearVelocity = _constantVelocity;
    }

    private void Update()
    {
        if (_hasDied) return;

        DetectHit();
        UpdateSpriteRotation();
    }

    private void UpdateSpriteRotation()
    {
        Vector2 velocity = _body.linearVelocity;
        if (velocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void CalculateVelocity()
    {
        Vector2 initialForce = _castData.InitialForce * _castData.InitialDirection.normalized;
        Vector2 constantVelocity = _castData.ConstantVelocity * _castData.ConstantDirection.normalized;

        if (_castData.CastScatteringAngle > 0f)
        {
            float halfAngle = _castData.CastScatteringAngle * 0.5f;
            float randomAngleDeg = Random.Range(-halfAngle, halfAngle);
            float randomAngleRad = randomAngleDeg * Mathf.Deg2Rad;

            initialForce = RotateVector(initialForce, randomAngleRad);
            constantVelocity = RotateVector(constantVelocity, randomAngleRad);
        }

        Vector2 initialForceWorld = transform.right * initialForce.x + transform.up * initialForce.y;

        _body.AddForce(initialForceWorld, ForceMode2D.Impulse);
        _constantVelocity = transform.right * constantVelocity.x + transform.up * constantVelocity.y;
    }

    private Vector2 RotateVector(Vector2 vector, float radians)
    {
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        return new Vector2(vector.x * cos - vector.y * sin, vector.x * sin + vector.y * cos);
    }

    public void FireProjectile(CastData castData, bool isFacingRight)
    {
        if (_body == null) _body = GetComponent<Rigidbody2D>();

        _castData = castData;
        _isFacingRight = isFacingRight;

        // Projectile setup
        SetProjectileDuration(_castData.ProjectileDuration);
        SetGravityModifier(_castData.GravityModifier);
        CalculateVelocity();

        _fizzleTimerIndex = TimerManager.I.StartTimer(_projectileDuration, () => Fizzle());

        // OnSpawn Subcast

        if (_castData.OnSpawnCast != null && Random.value <= _castData.OnSpawnCastChance)
        {
            TimerManager.I.StartTimer(_castData.OnSpawnCastDelay, () => { CastSubcast(_castData.OnSpawnCast); });
        }

        // Over lifetime subcast
        InvokeRepeating(nameof(TriggerOverLifetimeCast), castData.OverLifetimeCastInterval, castData.OverLifetimeCastInterval);

        // VFX
        if (_sigilSprite != null)
        {
            _sigilSprite.transform.parent = null;
            _sigilSprite.DOFade(0f, 0.75f).OnComplete(() => Destroy(_sigilSprite.gameObject));
        }

        if (_spawnVfx != null) _spawnVfx.Play();
        if (_idleVfx != null) _idleVfx.Play();

        // SFX
        if (_spawnSfx != null) AudioPool.Play(_spawnSfx, transform.position, _isSpawnSfx3D, 127, _spawnSfxVolume, Random.Range(_spawnSfxPitchMin, _spawnSfxPitchMax));
        if (_idleSfx != null) _idleVoice = AudioPool.Play(_idleSfx, transform.position, _isIdleSfx3D, 128, _idleSFXVolume, Random.Range(_idleSfxPitchMin, _idleSfxPitchMax), default, default, true);

        // Animation
        if (_projectileAnimator != null)
        {
            if (_idleAnim != null) _projectileAnimator.Play(_idleAnim.name);
        }
    }

    private void TriggerOverLifetimeCast()
    {
        if (_hasDied) return;

        if (_castData.OverLifetimeCast != null && Random.value <= _castData.OverLifetimeCastChance)
        {
            CastSubcast(_castData.OverLifetimeCast);
        }
    }

    private void CastSubcast(CastData subcast)
    {
        _caster.Cast(subcast, transform.position, _isFacingRight);
    }

    private void DetectHit()
    {
        if (!_useHitDetection) return;

        int envLayer = LayerMask.NameToLayer(ENV_LAYER);
        int entitiesLayer = LayerMask.NameToLayer(ENTITIES_LAYER);

        Vector2 hitPos = new Vector2(transform.position.x + _hitDetectionOffset.x, transform.position.y + _hitDetectionOffset.y);
        RaycastHit2D[] hits = Physics2D.CircleCastAll(hitPos, _hitDetectionRadius, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            int hitLayer = hit.collider.gameObject.layer;
            if (hitLayer != envLayer && hitLayer != entitiesLayer) continue;

            Destroy();

            // VFX
            if (_hitVfx != null)
            {
                ParticleSystem hitParticles = Instantiate(_hitVfx, hit.point, Quaternion.identity, null);
                hitParticles.Play();
                Destroy(hitParticles.gameObject, _hitVfx.main.duration);
            }

            // SFX
            if (_hitSfx != null) AudioPool.Play(_hitSfx, hit.point, _isHitSfx3D, 129, _hitSfxVolume, Random.Range(_hitSfxPitchMin, _hitSfxPitchMax));

            if (_castData.OnHitCast != null && Random.value <= _castData.OnHitCastChance)
            {
                TimerManager.I.StartTimer(_castData.OnHitCastDelay, () => { CastSubcast(_castData.OnHitCast); });
            }
            break;
        }
    }

    private void Destroy()
    {
        _hasDied = true;
        _body.linearVelocity = Vector2.zero;

        TimerManager.I.CancelTimer(_fizzleTimerIndex);

        if (_light != null) _light.gameObject.SetActive(false);

        _idleVoice.Stop();

        if (_idleVfx != null) _idleVfx.Stop();
        if (_projectileSprite != null) _projectileSprite.DOFade(0f, 0.1f);

        TimerManager.I.StartTimer(5f, () => { Destroy(gameObject); });
    }

    private void Fizzle()
    {
        _hasDied = true;
        _body.linearVelocity = Vector2.zero;

        if (_light != null) _light.gameObject.SetActive(false);

        _idleVoice.Stop();

        if (_idleVfx != null) _idleVfx.Stop();
        if (_projectileSprite != null) _projectileSprite.DOFade(0f, 0.3f);

        TimerManager.I.StartTimer(5f, () => { Destroy(gameObject); });

        // VFX
        if (_fizzleVfx != null)
        {
            ParticleSystem fizzleParticle = Instantiate(_fizzleVfx, transform.position, Quaternion.identity, null);
            Destroy(fizzleParticle, _fizzleVfx.main.duration);
        }

        // SFX
        if (_fizzleSfx != null) AudioPool.Play(_fizzleSfx, transform.position, _isFizzleSfx3D, 129, _fizzleSfxVolume, Random.Range(_fizzleSfxPitchMin, _fizzleSfxPitchMax));

        if (_castData.OnFizzleCast != null && Random.value <= _castData.OnFizzleCastChance)
        {
            TimerManager.I.StartTimer(_castData.OnFizzleCastDelay, () => { CastSubcast(_castData.OnFizzleCast); });
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Vector2 hitPos = new Vector2(transform.position.x + _hitDetectionOffset.x, transform.position.y + _hitDetectionOffset.y);
        Gizmos.DrawWireSphere(hitPos, _hitDetectionRadius);
    }
#endif
}

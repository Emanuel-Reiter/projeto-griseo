using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    private Rigidbody2D _body;
    private ProjectileCaster _caster;
    private CircleCollider2D _collider;

    private int _fizzleTimerIndex;

    private CastData _castData;
    private bool _hasDied = false;
    private bool _isFacingRight;
    private bool _allowConstantVelocity = false;
    private Vector2 _calculatedConstantVelocity;
    private int _remainingTargetPenetrations = 0;
    private int _remainingEnvHits = 0;

    private Transform _target = null;

    // Collision layers
    private const string ENV_LAYER = "EnvCollisions";
    private const string ENTITIES_LAYER = "Entities";
    private const string PLAYER_LAYER = "Player";
    private const string PROJECTILES_LAYER = "Projectiles";

    private int _envLayer;
    private int _entitiesLayer;
    private int _playerLayer;
    private int _projectilesLayer;

    // Rotation in degrees per second
    private float _rotationVelocity = 1800f;
    private const float TRACKING_ANGLE_PRIORITY = 1.0f;

    [Header("Projectile params")]
    [SerializeField] private bool _useEntityDetection = true;
    [SerializeField] private bool _useEnvDetection = true;
    [SerializeField] private float _hitDetectionRadius = 0.5f;
    [SerializeField] private Vector2 _hitDetectionOffset = Vector2.zero;
    [SerializeField] private float _projectileViewRadius = 8f;
    private float _projectileDuration;

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer _projectileSprite;
    [Space]
    [SerializeField] private SpriteRenderer _sigilSprite;

    [Header("Light params")]
    [SerializeField] private Light2D _light;
    [SerializeField] private bool _disableLightOnDie = true;

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
        _envLayer = LayerMask.NameToLayer(ENV_LAYER);
        _entitiesLayer = LayerMask.NameToLayer(ENTITIES_LAYER);
        _playerLayer = LayerMask.NameToLayer(PLAYER_LAYER);
        _projectilesLayer = LayerMask.NameToLayer(PROJECTILES_LAYER);

        _body = GetComponent<Rigidbody2D>();
        _caster = GetComponent<ProjectileCaster>();
        TryGetComponent<Animator>(out _projectileAnimator);

        if (_useEnvDetection)
        {
            _body.freezeRotation = false;
            if (!TryGetComponent<CircleCollider2D>(out _collider))
            {
                _collider = gameObject.AddComponent<CircleCollider2D>();
            }

            LayerMask mask = LayerMask.GetMask(ENTITIES_LAYER, PLAYER_LAYER, PROJECTILES_LAYER);
            _collider.excludeLayers = mask;
            _collider.radius = _hitDetectionRadius;
        }
        else
        {
            if (TryGetComponent<CircleCollider2D>(out _collider)) _collider.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (_hasDied) return;

        // Try use homing values
        if (_target != null && _castData.ProjectileTrackingPercent > 0f)
        {
            Vector2 targetDir = (_target.position - transform.position).normalized;
            Vector2 currentDir = _body.linearVelocity.normalized;
            if (currentDir == Vector2.zero) currentDir = _calculatedConstantVelocity.normalized;

            float angleDelta = Vector2.SignedAngle(currentDir, targetDir);
            float maxStep = (_rotationVelocity * _castData.ProjectileTrackingPercent) * Time.fixedDeltaTime;
            float newAngle = Mathf.MoveTowardsAngle(0f, angleDelta, maxStep);

            Vector2 newDir = Quaternion.Euler(0f, 0f, newAngle) * currentDir;

            float currentSpeed = _castData.ConstantVelocity;
            _body.linearVelocity = newDir * currentSpeed;
        }
        // If doesn't have homing use constant velocity instead
        else if (_allowConstantVelocity && _calculatedConstantVelocity != Vector2.zero)
        {
            _body.linearVelocity = _calculatedConstantVelocity;
        }
    }

    private void Update()
    {
        if (_hasDied) return;
        DetectEntityHit();
        UpdateVisualsRotation();
        DetectTrackingTarget();
    }

    private void UpdateVisualsRotation()
    {
        Vector2 velocity = _body.linearVelocity;
        if (velocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            if (_projectileSprite != null)
            {
                if (Mathf.Abs(angle) > 90f) _projectileSprite.flipY = true;
                else _projectileSprite.flipY = false;
            }
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
        _calculatedConstantVelocity = transform.right * constantVelocity.x + transform.up * constantVelocity.y;
    }

    private Vector2 RotateVector(Vector2 vector, float radians)
    {
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        return new Vector2(vector.x * cos - vector.y * sin, vector.x * sin + vector.y * cos);
    }

    public void FireProjectile(CastData castData, bool isFacingRight)
    {
        // Projectile setup
        _castData = castData;
        _isFacingRight = isFacingRight;
        _remainingEnvHits = _castData.MaxEnvironmentHits;
        _remainingTargetPenetrations = _castData.MaxTargetPenetration;

        SetProjectileDuration(_castData.ProjectileDuration);
        SetGravityModifier(_castData.GravityModifier);
        CalculateVelocity();

        // Constant velociy apply delay trigger
        TimerManager.I.StartTimer(_castData.ConstantVelocityStartDelay, () => _allowConstantVelocity = true);

        // Fizzle timer
        _fizzleTimerIndex = TimerManager.I.StartTimer(_projectileDuration, () => Fizzle());

        // Initial subcasts
        TriggerOnSpawnSubcast();
        InvokeRepeating(nameof(TriggerOverLifetimeSubast), castData.OverLifetimeCastInterval, castData.OverLifetimeCastInterval);

        // Effects
        TriggerSigilEffects();
        TriggerSpawnEffects();
        TriggerIdleEffects();

        // Animation
        if (_projectileAnimator != null)
        {
            if (_idleAnim != null) _projectileAnimator.Play(_idleAnim.name);
        }
    }

    private void Die()
    {
        _hasDied = true;
        _body.linearVelocity = Vector2.zero;

        TimerManager.I.CancelTimer(_fizzleTimerIndex);

        if (_collider != null) _collider.enabled = false;

        LightFadeBehaviour();

        if (_idleVfx != null) _idleVfx.Stop();
        if (_projectileSprite != null) _projectileSprite.DOFade(0f, 0.1f);

        TimerManager.I.StartTimer(5f, () => { Destroy(gameObject); });
    }

    private void Fizzle()
    {
        Die();
        TriggerFizzleEffects();
        TriggerOnFizzleSubcast();
    }

    #region Subcasts
    private void CastSubcast(CastData subcast)
    {
        _caster.Cast(subcast, transform.position, _isFacingRight);
    }

    // On spawn subcast
    private void TriggerOnSpawnSubcast()
    {
        if (_castData.OnSpawnCast != null && Random.value <= _castData.OnSpawnCastChance)
        {
            TimerManager.I.StartTimer(_castData.OnSpawnCastDelay, () => { CastSubcast(_castData.OnSpawnCast); });
        }
    }

    // Over lifetime subcast
    private void TriggerOverLifetimeSubast()
    {
        if (_hasDied) return;

        if (_castData.OverLifetimeCast != null && Random.value <= _castData.OverLifetimeCastChance)
        {
            CastSubcast(_castData.OverLifetimeCast);
        }
    }

    // On fizzle subcast
    private void TriggerOnFizzleSubcast()
    {
        if (_castData.OnFizzleCast != null && Random.value <= _castData.OnFizzleCastChance)
        {
            TimerManager.I.StartTimer(_castData.OnFizzleCastDelay, () => { CastSubcast(_castData.OnFizzleCast); });
        }
    }

    // On hit subcast
    private void TriggerOnHitSubcast()
    {
        if (_castData.OnHitCast != null && Random.value <= _castData.OnHitCastChance)
        {
            TimerManager.I.StartTimer(_castData.OnHitCastDelay, () => { CastSubcast(_castData.OnHitCast); });
        }
    }

    #endregion

    #region Effects
    // Sigil effects
    private void TriggerSigilEffects()
    {
        if (_sigilSprite != null)
        {
            _sigilSprite.transform.parent = null;
            _sigilSprite.DOFade(0f, 0.75f).OnComplete(() => Destroy(_sigilSprite.gameObject));
        }
    }

    // Spawn effects
    private void TriggerSpawnEffects()
    {
        if (_spawnVfx != null) _spawnVfx.Play();
        if (_spawnSfx != null) AudioPool.Play(_spawnSfx, transform.position, _isSpawnSfx3D, 127, _spawnSfxVolume, Random.Range(_spawnSfxPitchMin, _spawnSfxPitchMax));
    }

    // Idle effects
    private void TriggerIdleEffects()
    {
        if (_idleVfx != null && !_idleVfx.isPlaying) _idleVfx.Play();
        if (_idleSfx != null) InvokeRepeating(nameof(PlayIdleAudio), 0f, _idleSfx.length);
    }

    private void PlayIdleAudio()
    {
        if (_hasDied) return;
        if (_idleSfx == null) return;
        AudioPool.Play(_idleSfx, transform.position, _isIdleSfx3D, 128, _idleSFXVolume, Random.Range(_idleSfxPitchMin, _idleSfxPitchMax));
    }

    // Fizzle effects
    private void TriggerFizzleEffects()
    {
        if (_fizzleVfx != null) _fizzleVfx.Play();
        if (_fizzleSfx != null) AudioPool.Play(_fizzleSfx, transform.position, _isFizzleSfx3D, 129, _fizzleSfxVolume, Random.Range(_fizzleSfxPitchMin, _fizzleSfxPitchMax));
    }

    // Hit effects
    private void TriggerHitEffects(Vector3 pos)
    {
        if (_hitVfx != null)
        {
            ParticleSystem hitParticles = Instantiate(_hitVfx, pos, Quaternion.identity, null);
            hitParticles.Play();
            Destroy(hitParticles.gameObject, _hitVfx.main.duration);
        }

        if (_hitSfx != null) AudioPool.Play(_hitSfx, pos, _isHitSfx3D, 129, _hitSfxVolume, Random.Range(_hitSfxPitchMin, _hitSfxPitchMax));
    }

    private void TriggerHitEffects()
    {
        if (_hitVfx != null)
        {
            ParticleSystem hitParticles = Instantiate(_hitVfx, transform.position, Quaternion.identity, null);
            hitParticles.Play();
            Destroy(hitParticles.gameObject, _hitVfx.main.duration);
        }

        if (_hitSfx != null) AudioPool.Play(_hitSfx, transform.position, _isHitSfx3D, 129, _hitSfxVolume, Random.Range(_hitSfxPitchMin, _hitSfxPitchMax));

    }

    // Light effects
    private void LightFadeBehaviour()
    {
        if (_light != null)
        {
            if (_disableLightOnDie) _light.gameObject.SetActive(false);
            else TimerManager.I.StartTimer(_castData.ProjectileDuration, () =>
            {
                DOTween.To(() => _light.intensity, x => _light.intensity = x, 0f, _castData.ProjectileDuration)
                .OnComplete(() => _light.gameObject.SetActive(false));
            });
        }
    }

    #endregion 

    #region Detections
    private void DetectEntityHit()
    {
        if (!_useEntityDetection) return;

        Vector2 hitPos = new Vector2(transform.position.x + _hitDetectionOffset.x, transform.position.y + _hitDetectionOffset.y);
        RaycastHit2D[] hits = Physics2D.CircleCastAll(hitPos, _hitDetectionRadius, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            int hitLayer = hit.collider.gameObject.layer;
            if (hitLayer != _entitiesLayer) continue;

            if (_castData.CanDealDamage)
            {
                // Damage
                if (hit.collider.gameObject.TryGetComponent<CharAttributesManager>(out CharAttributesManager target))
                {
                    Vector2 targetPos = hit.collider.transform.position;
                    Vector2 projectilePos = transform.position;
                    Vector2 dirKb = new Vector2(targetPos.x - projectilePos.x, 0f);

                    DamageData damageData = new DamageData(
                        _castData.BaseDMGPhysical,
                        _castData.BaseDMGStellar,
                        _castData.BaseDMGFire,
                        _castData.BaseDMGLightining,
                        _castData.STSPoison,
                        _castData.STSFrostbite,
                        _castData.STSIchor,
                        _castData.CritChace,
                        _castData.CritModifier,
                        _castData.KnockbackForce,
                        dirKb
                    );

                    target.TakeDamage(damageData);
                }
            }

            _remainingTargetPenetrations--;

            bool destroyProjectile = (_remainingEnvHits <= 0 || _remainingTargetPenetrations <= 0) ? true : false;

            TriggerHitEffects(hit.point);
            TriggerOnHitSubcast();

            if (destroyProjectile)
            {
                Die();
                if (!_castData.UseAreaDamage) break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_useEnvDetection) return;

        int hitLayer = collision.gameObject.layer;
        if (hitLayer != _envLayer) return;

        TriggerHitEffects();
        TriggerOnHitSubcast();

        _remainingEnvHits--;
        if (_remainingEnvHits <= 0) Die();
    }

    private void DetectTrackingTarget()
    {
        if (_target != null) return;

        Vector2 currentDirection = _body.linearVelocity;
        if (currentDirection.sqrMagnitude < 0.01f)
            currentDirection = _calculatedConstantVelocity;

        bool hasValidDirection = currentDirection.sqrMagnitude > 0.01f;

        int entitiesMask = LayerMask.GetMask(ENTITIES_LAYER);
        Collider2D[] targetsInRadius = Physics2D.OverlapCircleAll(transform.position, _projectileViewRadius, entitiesMask);

        Transform bestTarget = null;
        float bestScore = float.MaxValue;

        foreach (Collider2D target in targetsInRadius)
        {
            Transform trackingTransform = target.transform;
            CharTrackingTarget trackingTarget = target.GetComponentInChildren<CharTrackingTarget>();
            if (trackingTarget != null) trackingTransform = trackingTarget.transform;

            Vector2 dirToTarget = trackingTransform.position - transform.position;
            float distance = dirToTarget.magnitude;

            float angleFactor = 0f;
            if (hasValidDirection)
            {
                float angle = Vector2.Angle(currentDirection, dirToTarget);
                angleFactor = angle * TRACKING_ANGLE_PRIORITY;
            }

            float score = distance + angleFactor;

            if (score < bestScore)
            {
                bestScore = score;
                bestTarget = trackingTransform;
            }
        }

        if (bestTarget != null)
        {
            //Debug.Log($"Found target: {bestTarget.gameObject.name} | Score: {bestScore:F2}");
            _target = bestTarget;
        }
    }
    #endregion


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!_hasDied) Gizmos.color = Color.magenta;
        else Gizmos.color = Color.white;

        Vector2 hitPos = new Vector2(transform.position.x + _hitDetectionOffset.x, transform.position.y + _hitDetectionOffset.y);
        Gizmos.DrawWireSphere(hitPos, _hitDetectionRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _projectileViewRadius);
    }
#endif
}
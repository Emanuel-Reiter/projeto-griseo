using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharAttributesManager))]
public class CharAttributesVisuals : MonoBehaviour
{
    private CharAttributesManager _attributes;

    [Header("UI refs")]
    [SerializeField] private CanvasGroup _hpGroup;
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private Image _hpBarFill;
    [SerializeField] private Image _hpBarBackground;
    private Gradient _hpBarFillGradient;
    private Gradient _hpBarBackgroundGradient;

    [Header("VFX")]
    [SerializeField] private Vector2 _vfxOffset = new Vector2(0f, 2f);
    [Space]
    [SerializeField] private ParticleSystem _takeDamageVfx;
    [SerializeField] private ParticleSystem _dieVfx;

    private SpriteRenderer _sprite;


    private void Awake()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();

        _attributes = GetComponent<CharAttributesManager>();
        CreateGradient();

        _attributes.OnHealthChangeEvent += OnHealthChange;
        _attributes.OnTakeDamageEvent += OnTakeDamage;
        _attributes.OnDieEvent += OnDie;
        
    }

    private void OnEnable()
    {
        _attributes.OnHealthChangeEvent += OnHealthChange;
        _attributes.OnTakeDamageEvent += OnTakeDamage;
        _attributes.OnDieEvent += OnDie;
    }

    private void OnDisable()
    {
        _attributes.OnHealthChangeEvent -= OnHealthChange;
        _attributes.OnTakeDamageEvent -= OnTakeDamage;
        _attributes.OnDieEvent -= OnDie;
    }

    private void OnDestroy()
    {
        _attributes.OnHealthChangeEvent -= OnHealthChange;
        _attributes.OnTakeDamageEvent -= OnTakeDamage;
        _attributes.OnDieEvent -= OnDie;

        if (_hpGroup != null) Destroy(_hpGroup.transform.parent);
    }

    private void OnHealthChange(int newHealth)
    {
        UpdateHealthUi(newHealth);
    }

    private void OnTakeDamage()
    {
        TriggerOnHitEffects();
        DamageFlash();
    }

    public void DamageFlash()
    {
        Color corOriginal = _sprite.color;
        _sprite.DOColor(Color.red, 0.1f).OnComplete(() =>
            _sprite.DOColor(corOriginal, 0.1f));
    }

    private void OnDie()
    {
        TriggerDieEffects();
    }

    private void TriggerOnHitEffects()
    {
        if (_takeDamageVfx != null)
        {
            Vector3 pos = new Vector3(transform.position.x + _vfxOffset.x, transform.position.y + _vfxOffset.y, transform.position.z);
            ParticleSystem hitParticles = Instantiate(_takeDamageVfx, pos, Quaternion.identity, null);
            hitParticles.Play();
            Destroy(hitParticles.gameObject, _takeDamageVfx.main.duration);
        }
    }

    private void TriggerDieEffects()
    {
        if (_dieVfx != null)
        {
            Vector3 pos = new Vector3(transform.position.x + _vfxOffset.x, transform.position.y + _vfxOffset.y, transform.position.z);
            ParticleSystem hitParticles = Instantiate(_dieVfx, pos, Quaternion.identity, null);
            hitParticles.Play();
            Destroy(hitParticles.gameObject, _dieVfx.main.duration);
        }
    }

    private void UpdateHealthUi(int newHealth)
    {
        float hpPercent = (float)newHealth / (float)_attributes.CharAttributes.MaxHealth;
        if (hpPercent == 1f || hpPercent <= 0f) _hpGroup.gameObject.SetActive(false);
        else _hpGroup.gameObject.SetActive(true);

        if (_hpSlider != null) _hpSlider.value = hpPercent;

        if (_hpBarFill != null) _hpBarFill.color = _hpBarFillGradient.Evaluate(hpPercent);
        if (_hpBarBackground != null) _hpBarBackground.color = _hpBarBackgroundGradient.Evaluate(hpPercent);
    }

    private void CreateGradient()
    {
        // Fill
        _hpBarFillGradient = new Gradient();

        GradientColorKey[] colorKeysFill = new GradientColorKey[5];
        ColorUtility.TryParseHtmlString("#C9424C", out colorKeysFill[0].color);
        colorKeysFill[0].time = 0.0f;

        ColorUtility.TryParseHtmlString("#C86D34", out colorKeysFill[1].color);
        colorKeysFill[1].time = 0.25f;

        ColorUtility.TryParseHtmlString("#C79534", out colorKeysFill[2].color);
        colorKeysFill[2].time = 0.5f;

        ColorUtility.TryParseHtmlString("#C5C639", out colorKeysFill[3].color);
        colorKeysFill[3].time = 0.75f;

        ColorUtility.TryParseHtmlString("#5BC741", out colorKeysFill[4].color);
        colorKeysFill[4].time = 1.0f;

        GradientAlphaKey[] alphaKeysFill = new GradientAlphaKey[2];
        alphaKeysFill[0].alpha = 1.0f;
        alphaKeysFill[0].time = 0.0f;

        alphaKeysFill[1].alpha = 1.0f;
        alphaKeysFill[1].time = 1.0f;

        _hpBarFillGradient.SetKeys(colorKeysFill, alphaKeysFill);

        // BG
        _hpBarBackgroundGradient = new Gradient();

        GradientColorKey[] colorKeysBg = new GradientColorKey[5];
        ColorUtility.TryParseHtmlString("#651D22", out colorKeysBg[0].color);
        colorKeysBg[0].time = 0.0f;

        ColorUtility.TryParseHtmlString("#633315", out colorKeysBg[1].color);
        colorKeysBg[1].time = 0.25f;

        ColorUtility.TryParseHtmlString("#614715", out colorKeysBg[2].color);
        colorKeysBg[2].time = 0.5f;

        ColorUtility.TryParseHtmlString("#5E5F17", out colorKeysBg[3].color);
        colorKeysBg[3].time = 0.75f;

        ColorUtility.TryParseHtmlString("#29601B", out colorKeysBg[4].color);
        colorKeysBg[4].time = 1.0f;

        GradientAlphaKey[] alphaKeysBg = new GradientAlphaKey[2];
        alphaKeysBg[0].alpha = 1.0f;
        alphaKeysBg[0].time = 0.0f;

        alphaKeysBg[1].alpha = 1.0f;
        alphaKeysBg[1].time = 1.0f;

        _hpBarBackgroundGradient.SetKeys(colorKeysBg, alphaKeysBg);
    }
}

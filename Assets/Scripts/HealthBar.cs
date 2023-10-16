using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class HealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform _image;
    [SerializeField] private TMP_Text _text;

    private float _baseWidth;
    private Player _player;
    private bool _inited;

    private void Awake()
    {
        _baseWidth = _image.rect.width;

        _inited = false;
    }

    private void OnEnable()
    {
        if (_inited == true)
        {
            _player.Healed += Refresh;
            _player.DamageTaked += Refresh;
        }
    }

    private void OnDisable()
    {
        if(_inited == true)
        {
            _player.Healed -= Refresh;
            _player.DamageTaked -= Refresh;
        }
    }

    public void Init(Player player)
    {
        _player = player;

        _player.Healed += Refresh;
        _player.DamageTaked += Refresh;

        _inited = true;
    }

    public void Refresh()
    {
        if(_inited == true)
        {
            SetScale(_player.Health);

            _text.text = _player.Health.ToString();
        }
    }

    private void SetScale(int modifire)
    {
        _image.localPosition = (Vector2)_image.localPosition - new Vector2(_image.sizeDelta.x / 2, 0) + new Vector2((_baseWidth * modifire) / 2, 0);
        _image.sizeDelta = new Vector2(_baseWidth * modifire, _image.sizeDelta.y);
    }
}

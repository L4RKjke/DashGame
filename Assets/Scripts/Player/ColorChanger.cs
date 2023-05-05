using UnityEngine;
using Mirror;

public class ColorChanger : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _renderer;
    [SerializeField] private Player _player;

    private Color _originalColor;
    private Material _material;

    private void Start()
    {
        _material = _renderer.materials[1];
        _originalColor = _material.color;
    }

    private void OnEnable()
    {
        _player.HealthChanged += ChangeColor;
    }

    private void OnDisable()
    {
        _player.HealthChanged -= ChangeColor;
    }

    public void ChangeColor(HealthStatus status)
    {
        if (status == HealthStatus.Damaged)
            SetColor(Color.red);
        else
            SetColor(_originalColor);
    }

    private void SetColor(Color color)
    {
        _material.color = color;
    }


}

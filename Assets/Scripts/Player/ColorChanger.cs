using System.Collections;
using UnityEngine;
using Mirror;

public class ColorChanger : NetworkBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _renderer;
    [SerializeField] private Player _player;

    private Color _originalColor;

    private void Start()
    {
        _originalColor = _renderer.materials[1].color;
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
            RestoreColor();
    }

    private void SetColor(Color color)
    {
        _renderer.materials[1].color = color;
    }

    private void RestoreColor()
    {
        _renderer.materials[1].color = _originalColor;
    }

}

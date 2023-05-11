using UnityEngine;
using Mirror;

public class ColorChanger : NetworkBehaviour
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
        _player.HealthDamaged += CmdSetRedColor;
        _player.HealthCured += CmdChangeToDefault;
    }

    private void OnDisable()
    {
        _player.HealthDamaged -= CmdSetRedColor;
        _player.HealthCured += CmdChangeToDefault;
    }

    [Command(requiresAuthority = false)]
    private void CmdSetRedColor()
    {
        SetColor(Color.red);
    }

    [Command(requiresAuthority = false)]
    private void CmdChangeToDefault()
    {
        SetColor(_originalColor);
    } 

    private void SetColor(Color color)
    {
        if (_material != null)
            _material.color = color;

        RpcChangeColor(color);
    }

    [ClientRpc]
    private void RpcChangeColor(Color color)
    {
        if (_material != null)
            _material.color = color;
    }
}

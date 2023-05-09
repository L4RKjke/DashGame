using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{
    [SerializeField] private DashAbility _dash;
    [SerializeField] private Image _dashImage;

    private Coroutine _drowRoutine;

    public void OnEnable()
    {
        _dash.DashActivated += SetDashState;
    }

    public void OnDisable()
    {
        _dash.DashActivated -= SetDashState;

        if (_drowRoutine != null)
        {
            StopCoroutine(_drowRoutine);
        }
    }

    private void SetDashState()
    {
        _drowRoutine = StartCoroutine(DrowDashStatus());
    }

    private IEnumerator DrowDashStatus()
    {
        float dashTime = 0;
        var maxFill = 1;
        var fillParametr = maxFill / _dash.DashTime;

        while (dashTime <= _dash.DashTime)
        {
            yield return null;

            dashTime += Time.deltaTime;

            _dashImage.fillAmount = dashTime * fillParametr;
        }
    }
}

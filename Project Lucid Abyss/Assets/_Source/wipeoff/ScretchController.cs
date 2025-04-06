using UnityEngine;

[RequireComponent(typeof(ScretchView))]
public class ScretchController : MonoBehaviour
{
    [SerializeField] private KeyCode key = KeyCode.Space;
    [SerializeField] private float Scretchpower = 9;
    private AnomalySO anomalyData;
    private ScretchView _view;
    private ScretchModel _model;
    private bool isGameActive = false;
    private void Awake()
    {
        _view = GetComponent<ScretchView>();
        _model = new ScretchModel();


    }
    public void GetAnomalyInfo(AnomalySO anomalySO)
    {
        anomalyData = anomalySO;
        if (anomalyData != null)
        {
            _model.Initialize(
                 anomalySO.CounterForce,
                 anomalyData.NeedPowerMoreThan,
                 anomalyData.TimeToScretch
            ) ;
            isGameActive = true;
        }
    }

    private void Update()
    {
        if (!isGameActive)
            return;
        if (Input.GetKeyDown(key))
        {
            _view.SliderValue += Scretchpower;
        }
        _view.SliderValue -= _model.CounterForce * Time.deltaTime;

        float percent = (_view.SliderValue / _view.MaxValue) * 100f;

        _model.UpdateHold(percent, Time.deltaTime);

        if (_model.IsHolding)
        {
            _view.SetSuccessVisual();
            _view.SetTimerText(_model.HoldTime - _model.Timer);

            if (_model.IsCompleted)
            {
                OnSuccess();
                _model.Reset();
            }
            else
            {
                OnUnSuccess();
                _model.Reset();
            }
        }
        else
        {
            _view.SetUnSuccessVisual();
            _view.SetTimerText(_model.HoldTime);
        }
    }

    private void OnSuccess()
    {
    }

    private void OnUnSuccess()
    {
    }
}

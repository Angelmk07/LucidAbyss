using UnityEngine;

[RequireComponent(typeof(ScretchView))]
public class ScretchController : MonoBehaviour
{
    [SerializeField] private KeyCode key = KeyCode.Space;
    [SerializeField] private float Scretchpower = 9;
    private AnomalySO anomalyData;
    private ScretchView _view;
    private ScretchModel _model;
    [SerializeField]private bool isGameActive = false;
    public System.Action<bool> onGameEnd;

    public void StartMiniGame(System.Action<bool> onComplete)
    {
        isGameActive = true;

        this.onGameEnd = onComplete;
    }
    public void GetAnomalyInfo(AnomalySO anomalySO)
    {
        if(_view == null || _model == null)
        {
            _view = GetComponent<ScretchView>();
            _model = new ScretchModel();
        }
        if (anomalySO != null)
        {
            anomalyData = anomalySO;
            _model.Initialize(
                 anomalyData.CounterForce,
                 anomalyData.NeedPowerMoreThan,
                 anomalyData.TimeToScretch
            ) ;
            isGameActive = true;
            StartMiniGame(onGameEnd);

        }
        else
        {
            Debug.LogError("HOW");
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
            _view.SetTimerText(_model.TimeLeft);

            if (_model.IsCompleted)
            {
                OnSuccess();
                Reset();
            }
        }
        else
        {
            _view.SetUnSuccessVisual();
            _view.SetTimerText(_model.HoldTime);
        }

        if (!_model.IsHolding && _model.Timer > 0)
        {
            OnUnSuccess();
            Reset();
        }
    }

    private void Reset()
    {
        if (!isGameActive) return; 
        isGameActive = false;

        _model.Reset();
        _view.Reset();
    }


    private void OnSuccess()
    {
        onGameEnd?.Invoke(true);
    }

    private void OnUnSuccess()
    {
        onGameEnd?.Invoke(false);
    }

}

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
    private bool hasEnded = false;
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

        _model.TryStartTimer(percent);
        _model.UpdateTimer(Time.deltaTime);
        _view.SetTimerText(_model.TimeLeft);

        if (_model.IsTimerRunning)
        {
            if (_model.IsCompleted && !hasEnded)
            {
                hasEnded = true;

                if (percent > _model.NeedMoreThan)
                {
                    _view.SetSuccessVisual();
                    OnSuccess();
                }
                else
                {
                    _view.SetUnSuccessVisual();
                    OnUnSuccess();
                }

                Reset();
            }
            else
            {
                if (percent > _model.NeedMoreThan)
                    _view.SetSuccessVisual();
                else
                    _view.SetUnSuccessVisual();
            }
        }
    }



    private void Reset()
    {
        if (!isGameActive) return;
        isGameActive = false;

        _model.Reset();
        _view.Reset();
        hasEnded = false;
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

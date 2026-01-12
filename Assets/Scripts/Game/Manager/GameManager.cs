using com.homemade.modules.audio;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool isNewDay = false;
    public bool IsNewDay => isNewDay;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

#if PRODUCTION
        Debug.unityLogger.logEnabled = false;
#endif

#if UNITY_EDITOR || DEVELOPMENT || STAGING
        if (Application.isEditor)
            Application.runInBackground = false;
#endif
    }

    private async void Start()
    {
        // Init SDK

        // Inject service
        await UniTask.WaitUntil(() => GameService.Instance.IsInitialized);

        // Init data
        DataManager.Instance.Init();

        // Setting
        await UniTask.DelayFrame(1);
        SetSetting();

        // New day pass
        await UniTask.DelayFrame(1);
        NewDay();

        // Addressable
        //AddressableManager.Instance.LoadAllLevelSO();
    }

    private void OnApplicationQuit()
    {
        
    }

    private void SetSetting()
    {
        SettingSave settingSave = DataManager.Save.Setting;
        AudioController.Instance.TurnOnOffSound(settingSave.sound);
        AudioController.Instance.TurnOnOffMusic(settingSave.music);
        VibrationManager.IsVibrate = settingSave.vibrate;
    }

    private void NewDay()
    {
        if (DateUtils.IsNewDay())
        {
            Debug.Log("<color=green>New Day</color>");
            DataManager.Save.NewDay();
            isNewDay = true;
        }
    }

#if UNITY_EDITOR
    public void ShowLogError(string message)
    {
        Debug.Log($"<color=red>Error:</color> {message}");
    }
#endif
}

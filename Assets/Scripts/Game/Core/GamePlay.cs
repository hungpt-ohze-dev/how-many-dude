using com.homemade.modules.audio;
using com.homemade.pattern.observer;
using com.homemade.pattern.singleton;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class GamePlay : MonoSingleton<GamePlay>
{
    [Header("Info")]
    [SerializeField] private PlatformEnum platformID;
    [SerializeField] private bool canTouch;

    // Get static
    public static bool LevelSetupDone = false;
    public static bool CanTouch
    {
        get => Instance.canTouch;
        set => Instance.canTouch = value;
    }
    public static PlatformEnum TargetPlatform => Instance.platformID;

    // Private variable
    private LevelSave levelSave;
    private SettingSave settingSave;
    private ResourceSave resourceSave;

    // Audio
    private AudioCase gameMusic;

    protected override void OnInit()
    {
        Application.targetFrameRate = 60;
        canTouch = true;

        levelSave = DataManager.Save.Level;
        settingSave = DataManager.Save.Setting;
        resourceSave = DataManager.Save.Resource;

        UIManager.Instance.PopupShowAction += OnPopupShow;
        UIManager.Instance.PopupHideAllAction += OnPopupHideAll;
    }

    protected override void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }

    protected override void OnDestroy()
    {
        if (applicationIsQuitting) return;
        UIManager.Instance.PopupShowAction -= OnPopupShow;
        UIManager.Instance.PopupHideAllAction -= OnPopupHideAll;
    }

    protected override void Start()
    {
#if UNITY_EDITOR
        if (IsSimulatorView())
        {
            platformID = PlatformEnum.Mobile;
        }
        else
        {
            platformID = PlatformEnum.PC;
        }
#elif UNITY_ANDROID || UNITY_IOS
        platformID = PlatformEnum.Mobile;
#endif

        Init().Forget();
    }

    #region Editor
    private bool IsSimulatorView()
    {
#if UNITY_EDITOR
        var gameViews = Resources.FindObjectsOfTypeAll<EditorWindow>();
        foreach (var view in gameViews)
        {
            string title = view.titleContent.text;
            if (title == "Simulator")
                return true;
        }

#endif
        return false;
    }
    #endregion

    private async UniTaskVoid Init()
    {
        canTouch = false;

        int maxLevel = DataManager.Config.LevelMax;
        int levelId = levelSave.levelId % (maxLevel + 1);
        levelId = Mathf.Clamp(levelId, 1, maxLevel);

        //await LevelController.Instance.Setup();
        UIManager.Extra.HideTransition();
        canTouch = true;

        // Release all sound
        await UniTask.DelayFrame(1);
        //AudioController.Instance.ReleaseSounds();

        // Music background
        //await UniTask.DelayFrame(1);
        //string musicBG = $"game_bgm_{Random.Range(1, 9)}";
        //gameMusic = AudioController.Instance.PlaySmartMusic(musicBG);
        //gameMusic.source.mute = !settingSave.music;
    }

    #region Event

    public async void WinGame()
    {
        canTouch = false;
        levelSave.FinishLevel();

        Debug.Log("Win");

        this.PostEvent(EventID.WinGame);

        await UniTask.WaitForSeconds(2f);
        await UIManager.Instance.ShowPopup<UIPopupWin>();
    }

    public async void LoseGame()
    {
        canTouch = false;
        Debug.Log("Lose");

        this.PostEvent(EventID.LoseGame);

        await UniTask.WaitForSeconds(1f);
        await UIManager.Instance.ShowPopup<UIPopupLose>();
    }

    public void OnPopupShow()
    {
        canTouch = false;
    }

    public void OnPopupHideAll()
    {
        canTouch = true;
    }

    #endregion
 
}

using com.homemade.pattern.singleton;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MainGame : MonoSingleton<MainGame>
{
    [Header("Camera")]
    [SerializeField] private Camera mainCam;
    [SerializeField] private Camera subCam;

    public static Camera MainCam => Instance.mainCam;

    protected override void Start()
    {
        LoadHome();
    }

    public async void LoadHome()
    {
        await UIManager.Extra.TransitionScreen.TransitionIn();

        var homeScreen = UIManager.Instance.ShowScreen<UIHomeScreen>();

        UIManager.Extra.HideTransition();
    }

    public async void LoadGame()
    {
        await UIManager.Extra.TransitionScreen.TransitionIn();

        MonoScene.Instance.LoadGameScene(OnDoneLoadGame);

        var gameScreen = UIManager.Instance.ShowScreen<UIGameScreen>();
    }

    private void OnDoneLoadGame()
    {

    }

    public async void ResetLevel()
    {
        await UIManager.Extra.TransitionScreen.TransitionIn();

        await UniTask.DelayFrame(1);
        MonoScene.Instance.RemoveScene(NameSceneEnum.Gameplay);

        await UniTask.DelayFrame(1);
        MonoScene.Instance.RemoveLevelScene();

        await UniTask.WaitForSeconds(0.2f);
        MonoScene.Instance.LoadGameScene(OnDoneLoadGame);
    }

    public async void NextLevel()
    {
        await UIManager.Extra.TransitionScreen.TransitionIn();

        await UniTask.DelayFrame(1);
        MonoScene.Instance.RemoveScene(NameSceneEnum.Gameplay);

        await UniTask.DelayFrame(1);
        MonoScene.Instance.RemoveLevelScene();

        await UniTask.WaitForSeconds(0.2f);
        MonoScene.Instance.LoadGameScene(OnDoneLoadGame);
    }

    public async void ReturnHome()
    {
        await UIManager.Extra.TransitionScreen.TransitionIn();

        await UniTask.DelayFrame(1);
        MonoScene.Instance.RemoveScene(NameSceneEnum.Gameplay);

        await UniTask.DelayFrame(1);
        MonoScene.Instance.RemoveLevelScene();

        var homeScreen = UIManager.Instance.ShowScreen<UIHomeScreen>();

        UIManager.Extra.HideTransition();
    }

    public void SetupCamera()
    {
        float targetAspect = 16f / 9f;
        float currentAspect = (float)Screen.width / Screen.height;

        float defaultWidth = 15f;

        if (DataManager.Config != null)
        {
            defaultWidth = DataManager.Config.GamePlayCamWidth;
        }

        float camSize = defaultWidth / 2f / currentAspect * targetAspect;
        mainCam.orthographicSize = camSize;
        subCam.orthographicSize = camSize;
    }
}

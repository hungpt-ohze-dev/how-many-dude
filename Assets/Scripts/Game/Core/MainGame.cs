using com.homemade.pattern.singleton;
using com.homemade.tick;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MainGame : MonoSingleton<MainGame>
{
    [Header("Camera")]
    [SerializeField] private Camera mainCam;
    [SerializeField] private Camera subCam;

    private int creativeId = 1;

    public static Camera MainCam => Instance.mainCam;

    protected override void Start()
    {
        TickManager.GamePlayTick.Action += TestTick;
        TickManager.GamePlayTick.Register();

        creativeId = DataManager.Config.GamePlayCreative;

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
        //await UIManager.Extra.TransitionScreen.TransitionIn();

        //MonoScene.Instance.LoadGameScene(OnDoneLoadGame);

        //var gameScreen = UIManager.Instance.ShowScreen<UIGameScreen>();

        LoadCreative();
    }

    private void OnDoneLoadGame()
    {

    }

    private void TestTick()
    {
        //Debug.Log("AAA");
    }

    public async void ResetLevel()
    {
        await UIManager.Extra.TransitionScreen.TransitionIn();

        await UniTask.DelayFrame(1);
        MonoScene.Instance.RemoveCreativeScene(creativeId);

        await UniTask.DelayFrame(1);
        MonoScene.Instance.RemoveLevelScene();

        await UniTask.WaitForSeconds(0.2f);
        MonoScene.Instance.LoadCreativeScene(1);
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

    public async void LoadCreative()
    {
        await UIManager.Extra.TransitionScreen.TransitionIn();

        MonoScene.Instance.LoadCreativeScene(creativeId);

        var gameScreen = UIManager.Instance.ShowScreen<UIGameScreen>();
    }
}

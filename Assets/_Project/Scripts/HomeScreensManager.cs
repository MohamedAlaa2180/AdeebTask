using AYellowpaper.SerializedCollections;
using UnityEngine;

public class HomeScreensManager : MonoBehaviour
{
    private static HomeScreensManager _instance;

    public static HomeScreensManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<HomeScreensManager>();
                if (_instance == null)
                {
                    Debug.Log("Singleton Gameobject doesn't esist in the scene");   
                }
            }
            return _instance;
        }
    }

    public SerializedDictionary<HomeScreensEnum, BaseScreen> homeScreens;

    public BaseScreen ShowScreen(HomeScreensEnum screenType)
    {
        return homeScreens[screenType].ShowScreen();
    }

    public void HideScreen(HomeScreensEnum screenType)
    {
        homeScreens[screenType].HideScreen();
    }
}

public enum HomeScreensEnum
{
    GroupEditScreen,
    GroupsPanelScreen
}

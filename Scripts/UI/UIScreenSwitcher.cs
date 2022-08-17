using UnityEngine;

public class UIScreenSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject _startScreen;
    [SerializeField] private GameObject _fileManagmentScreen;
    [SerializeField] private GameObject _viewingFileInfoScreen;
    [SerializeField] private GameObject _fileSavingScreen;
    [SerializeField] private GameObject _errorScreen;
    [SerializeField] private AppFileStorage _appFileStorage;

    private void Start()
    {
        SwitchTo(UIScreens.Start);
        _appFileStorage.FilesAdded += OnFilesAdded;
    }

    private void OnFilesAdded() => SwitchTo(UIScreens.FilesManagment);

    public void OpenSaveFileScreen() => SwitchTo(UIScreens.FileSaving);

    public void CloseSaveFileScreen() => DisableInactiveScreens(_fileSavingScreen);

    public void SwitchTo(UIScreens screen)
    {
        switch (screen)
        {
            case UIScreens.Start:
                EnableScreen(_startScreen);
                DisableInactiveScreens(_fileManagmentScreen, _errorScreen);
                break;

            case UIScreens.FilesManagment:
                EnableScreen(_fileManagmentScreen);
                DisableInactiveScreens(_startScreen, _viewingFileInfoScreen, _fileSavingScreen, _errorScreen);
                break;

            case UIScreens.ViewingFileInfo:
                EnableScreen(_viewingFileInfoScreen);
                DisableInactiveScreens(_startScreen, _fileSavingScreen, _errorScreen);
                break;

            case UIScreens.FileSaving:
                EnableScreen(_fileSavingScreen);
                DisableInactiveScreens(_startScreen, _errorScreen);
                break;

            case UIScreens.Error:
                EnableScreen(_errorScreen);
                break;
        }
    }

    private void EnableScreen(GameObject screen) => screen.gameObject.SetActive(true);

    private void DisableInactiveScreens(params GameObject[] screens)
    {
        foreach (var screen in screens)
             screen.gameObject.SetActive(false);
    }
}

public enum UIScreens
{
    Start, FilesManagment, ViewingFileInfo, FileSaving, Error
}

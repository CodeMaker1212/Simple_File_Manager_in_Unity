using UnityEngine;
using UnityEngine.UI;

public class ErrorDisplay : MonoBehaviour
{
    [SerializeField] private Text _errorText;
    [SerializeField] private UIScreenSwitcher _screenSwitcher;
    private readonly string _fileExtensionErrorText =    "File has an invalid format";
    private readonly string _filesCountErrorText =       $"You can drag and drop no more than" +
                                                         $" {AppFileStorage.MAX_STORED_FILES_COUNT} files at a time";
    private readonly string _storageOverflowErrorText = "Storage is full";
    private readonly string _fileNotFoundText =         "The file was not found or you are trying to place a folder, which is incorrect";

    private void Start()
    {
        InputFilesHandler.DragAndDropFailed += OnError;
        AppFileStorage.StorageOverflow += OnError;
    }

    private void OnError(Error error)
    {
        _screenSwitcher.SwitchTo(UIScreens.Error);
        switch (error)
        {
            case Error.FileExtension: _errorText.text = _fileExtensionErrorText; break;
            case Error.ExceedingNumberOfFiles: _errorText.text = _filesCountErrorText; break;
            case Error.StorageOverflow: _errorText.text = _storageOverflowErrorText; break;
            case Error.FileNotFound: _errorText.text = _fileNotFoundText; break;
        }
    }

    public void Disable() => gameObject.SetActive(false);
}

public enum Error 
{ 
    FileExtension,
    ExceedingNumberOfFiles,
    StorageOverflow,
    FileNotFound
}

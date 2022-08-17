using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using UnityEngine.UI;
using UnityEngine.Events;

public class AppFileStorage : MonoBehaviour
{
    public const int MAX_STORED_FILES_COUNT = 5;
  
    public delegate void StorageOverflowEvent(Error error);
    public static event StorageOverflowEvent StorageOverflow;
    public UnityAction FilesAdded;

    // The file name is used as the key.
    private Dictionary<string, UserFile> _userFiles = new Dictionary<string, UserFile>();

    private string _userSavingDirectory;
    private string _directory;
    private string _currentSelectedFile;
    [SerializeField] private string[] _availableExtensions = { ".docx", ".mp3", ".xlsx", ".pdf", ".txt" };
    [SerializeField] private UserFileListDisplay _uiFileList;
    [SerializeField] private UserFileInfoDisplay _uiFileInfo;
    [SerializeField] private Text _targetPathInputFieldText;

    public string[] AvailableExtensions => _availableExtensions;

    private void Start() => _directory = Application.dataPath;

    private void OnApplicationQuit()
    {
        if (_userFiles.Count != 0)
            DeleteFilesFromApp();
    }

    private void DeleteFilesFromApp()
    {
        foreach (var file in _userFiles)
            File.Delete(_userFiles[file.Key].GetFullPathInApplication());        
    }

    public void SetUserSavingDirectory() => _userSavingDirectory = _targetPathInputFieldText.text;

    public void AddFile(ref Dictionary<string, InputFile> inputFiles)
    {
        bool storageIsFull = false;
        foreach (var inputFile in inputFiles)
        {
            if (HasEmptySpace() && !_userFiles.ContainsKey(inputFile.Key))
            {
                var userFile = new UserFile(inputFiles[inputFile.Key]);
                _userFiles.Add(userFile.Name, userFile);
                SaveFileToAppDirectory(userFile.Name);
            }
            else if (!HasEmptySpace())
            {
                storageIsFull = true;
                break;
            }                                      
        }

        if (storageIsFull)
        {
            StorageOverflow?.Invoke(Error.StorageOverflow);
            return;
        }
       
        FilesAdded?.Invoke();
        _uiFileList.Show(_userFiles);      
    }

    private bool HasEmptySpace() => _userFiles.Count < MAX_STORED_FILES_COUNT;

    private void SaveFileToAppDirectory(string fileName)
    {
        var sourcePath = @"" + _userFiles[fileName].SourceDirectoryPath + "/" + fileName + _userFiles[fileName].Extension;
        var targetPath = @"" + _directory + "/" + fileName + _userFiles[fileName].Extension;
        File.Copy(sourcePath, targetPath, true);
    }  

    public void SaveFileToUserDirectory()
    {
        var sourcePath = @"" + _directory + "/" + _currentSelectedFile + _userFiles[_currentSelectedFile].Extension;
        var targetPath = @"" + _userSavingDirectory + "/" + _currentSelectedFile + _userFiles[_currentSelectedFile].Extension;
        File.Copy(sourcePath, targetPath);
        Process.Start(@"" + _userSavingDirectory);
    }

    public void OnFileClick(int fileNumber)
    {
        var fileName = _uiFileList.FileFields[fileNumber - 1].text;
        _currentSelectedFile = fileName;
        _uiFileInfo.Show(_userFiles[fileName]);
    }
}

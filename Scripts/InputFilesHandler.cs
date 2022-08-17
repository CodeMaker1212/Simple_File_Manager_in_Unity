using System.Collections.Generic;
using UnityEngine;
using System.IO;
using B83.Win32;

class InputFilesHandler : MonoBehaviour
{
    private const int MAX_INPUT_FILES = 5;

    private Dictionary<string, InputFile> _correctFiles;
    private FileInfo _fileInfo;
    [SerializeField] private AppFileStorage _appFileStorage;

    public delegate void DragAndDropError(Error error);
    public static event DragAndDropError DragAndDropFailed;

    void OnEnable()
    {
        UnityDragAndDropHook.InstallHook();
        UnityDragAndDropHook.OnDroppedFiles += OnFilesDropped;
    }

    void OnDisable() => UnityDragAndDropHook.UninstallHook();

    private void OnFilesDropped(List<string> droppedFilesPaths)
    {           
         if (droppedFilesPaths.Count > MAX_INPUT_FILES)
             DragAndDropFailed?.Invoke(Error.ExceedingNumberOfFiles);
         else
             TrySaveFiles(droppedFilesPaths); 
    }

    private void TrySaveFiles(List<string> droppedFilesPaths)
    {
        try
        {
            SortFilesByAvailableExtension(droppedFilesPaths, out Dictionary<string, InputFile> incorrectFiles, out _correctFiles);

            if (incorrectFiles.Count != 0)
                DragAndDropFailed?.Invoke(Error.FileExtension);

            if (_correctFiles.Count != 0)
                SaveToAppStorage();
        }
        catch 
        {
            DragAndDropFailed(Error.FileNotFound);
        }       
    }

    private void SortFilesByAvailableExtension(in List<string> sourcePaths,
                                               out Dictionary<string, InputFile> incorrectFiles, 
                                               out Dictionary<string, InputFile> correctFiles)
    {
        correctFiles = new Dictionary<string,InputFile>();
        incorrectFiles = new Dictionary<string, InputFile>();
        var unsortedFile = GetInputFiles(in sourcePaths);       
        foreach(var file in unsortedFile)
        {
            if (HasAvailableExtension(file))
                correctFiles.Add(file.Name, file);
            else
                incorrectFiles.Add(file.Name, file);                          
        }
    }

    private List<InputFile> GetInputFiles(in List<string> sourcePaths)
    {
        var inputFiles = new List<InputFile>();
        foreach (var filePath in sourcePaths)
        {
            _fileInfo = new FileInfo(filePath);
            var length = (int)_fileInfo.Length;
            var extension = _fileInfo.Extension;
            var name = Path.GetFileNameWithoutExtension(filePath);
            var directoryPath = Path.GetDirectoryName(filePath);
            var inputFile = new InputFile(name, extension, length, directoryPath);
            inputFiles.Add(inputFile);
        }
        return inputFiles;
    }

    private bool HasAvailableExtension(in InputFile file)
    {
        foreach (var availableExt in _appFileStorage?.AvailableExtensions)
            if (file.Extension == availableExt) 
                return true;

        return false;
    }

    private void SaveToAppStorage()
    {
        _appFileStorage?.AddFile(ref _correctFiles);
        _correctFiles.Clear();
    }
}
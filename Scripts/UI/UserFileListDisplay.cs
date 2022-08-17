using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserFileListDisplay : MonoBehaviour
{
    [SerializeField] private List<Text> _fileFields;
    [SerializeField] private Color _defaultFileTextColor = Color.black;
    [SerializeField] private Color _highlighFileTextColor = Color.white;

    public List<Text> FileFields => _fileFields;

    public void OnFileTextEnter(int fileNumber) => HighlightFileText(fileNumber);

    public void OnFileTextExit(int fileNumber) => ReturnDefaultTextColor(fileNumber);

    private void HighlightFileText(int fileNumber) => _fileFields[fileNumber - 1].color = _highlighFileTextColor;

    private void ReturnDefaultTextColor(int fileNumber) => _fileFields[fileNumber - 1].color = _defaultFileTextColor;

    public void Show(Dictionary<string, UserFile> files)
    {
        foreach (var file in files)
        {
            var emptyField = GetEmptyField();
            if (emptyField != null && !FieldContains(files[file.Key].Name))
                emptyField.text = files[file.Key].Name;
        }
    }

    private Text GetEmptyField()
    {
        foreach (var field in _fileFields)       
            if (field.text.Contains("Empty"))
                return field;
        
        return null;
    }

    private bool FieldContains(string fileName)
    {
        foreach(var field in _fileFields)
            if (field.text.Contains(fileName))
                return true;

        return false;
    }
}

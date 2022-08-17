using UnityEngine;
using UnityEngine.UI;

public class UserFileInfoDisplay : MonoBehaviour
{
    [SerializeField] private Text _nameField;
    [SerializeField] private Text _extensionField;
    [SerializeField] private Text _sourcePathField;
    [SerializeField] private Text _lengthField;
    [SerializeField] private UIScreenSwitcher _screenSwitcher;

    public void Show(UserFile file) 
    {
        _screenSwitcher.SwitchTo(UIScreens.ViewingFileInfo);
        _nameField.text = file.Name;
        _extensionField.text = file.Extension;
        _sourcePathField.text = file.SourceDirectoryPath;
        _lengthField.text = file.Length + " Kbyte(s)";
    }
}

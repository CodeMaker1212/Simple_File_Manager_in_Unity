using UnityEngine;


public class UserFile
{
    public string Name { get; }
    public string Extension { get; }
    public string SourceDirectoryPath { get; }
    public int Length { get; }

    public UserFile(InputFile inputFile)
    {
        Name = inputFile.Name;
        Extension = inputFile.Extension;
        Length = ConvertBytesToKBytes(inputFile.Length);
        SourceDirectoryPath = inputFile.SourceDirectoryPath;
    }

    private int ConvertBytesToKBytes(int value) => value / 1000;

    public string GetFullPathInApplication() => @"" + Application.dataPath + "/" + Name + Extension ;
}   
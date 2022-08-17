public struct InputFile
{
    public string Name { get; }
    public string Extension { get; }
    public string SourceDirectoryPath { get; }
    public int Length { get; }

    public InputFile(string name, string extension, int length, string directoryPath)
    {
        Name = name;
        Extension = extension;
        Length = length;
        SourceDirectoryPath = directoryPath;
    }
}

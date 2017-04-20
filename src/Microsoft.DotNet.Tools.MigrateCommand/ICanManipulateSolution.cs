namespace Microsoft.DotNet.Tools.MigrateCommand
{
    public interface ICanManipulateSolutionFile
    {
        int Execute(string dotnetPath, string slnPath, string projPath, string commandName);
    }
}

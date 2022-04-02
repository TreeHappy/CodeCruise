using Microsoft.CodeAnalysis;

namespace Library
{
    public class SolutionBuilder
    {
        public static Task<Solution> CreateSolutionOldStyleAsync(FileInfo solutionFileInfo) =>
            Task.Run(() => Roslyn.MkSolution(solutionFileInfo, Roslyn.MkProjectLoader(Build.GetFilesFromProject)));

        public static Task<Solution> CreateSolutionAsync(FileInfo solutionFileInfo) =>
            Task.Run(() => Roslyn.MkSolution(solutionFileInfo, Roslyn.MkProjectLoader(pi => pi.Directory.EnumerateFiles("*.cs", SearchOption.AllDirectories))));
    }
}

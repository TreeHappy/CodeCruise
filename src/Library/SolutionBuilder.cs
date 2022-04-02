using Microsoft.CodeAnalysis;

namespace Library
{
    public class SolutionBuilder
    {

        public static Task<Solution> CreateSolutionAsync(FileInfo solutionFileInfo, GetFilesFromProjectType getFilesFromProject)  =>
            Task.Run(() => Roslyn.MkSolution(solutionFileInfo, Roslyn.MkProjectLoader(getFilesFromProject)));
    }
}

using Microsoft.CodeAnalysis;

namespace Library
{
    public class SolutionBuilder
    {
        public static Task<Solution> CreateSolutionAsync(FileInfo solutionFileInfo) =>
            Task.Run(() => Roslyn.MkSolution(solutionFileInfo));
    }
}

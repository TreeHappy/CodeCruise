using Broslyn;
using Microsoft.CodeAnalysis;

namespace Library
{
    public class WorkspaceBuilder
    {
        public static async Task<Workspace> CreateWorkspace(string solutionFilePath)
        {
            var result = CSharpCompilationCapture.Build(solutionFilePath, "Release");
            var workspace = result.Workspace;
            var project = workspace.CurrentSolution.Projects.FirstOrDefault();
            var compilation = await project.GetCompilationAsync();

            return workspace;
        }
    }
}

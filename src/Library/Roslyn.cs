namespace Library
{
    using System.Linq;
    using System.IO;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;

    public static class Roslyn
    {
        public static Project MkProject(AdhocWorkspace workspace, FileInfo project)
        {
            var projectName = Path.GetFileNameWithoutExtension(project.Name);
            var projectId = ProjectId.CreateNewId();
            var versionStamp = VersionStamp.Create();
            var projectInfo =
                ProjectInfo.Create
                    ( projectId
                    , versionStamp
                    , projectName
                    , projectName
                    , LanguageNames.CSharp
                    , project.FullName
                    , metadataReferences:
                        new []
                            { MetadataReference.CreateFromFile
                                (typeof(System.Attribute).Assembly.Location)
                            }
                    );
            var roslynProject = workspace.AddProject(projectInfo);

            foreach (var projectFile in Build.GetFilesFromProject(project))
                workspace.AddDocument
                    ( projectId
                    , projectFile.Name
                    , SourceText.From(File.ReadAllText(projectFile.FullName))
                    );

            return roslynProject;
        }

        public static Solution MkSolution(FileInfo solutionFile)
        {
            bool IsCSProject(FileInfo project) =>
                project.FullName.EndsWith("csproj");

            if (solutionFile.Exists == false)
                throw new FileNotFoundException($"{solutionFile.FullName} not found.");

            var workspace = new AdhocWorkspace();
            var solution =
                workspace.AddSolution
                    ( SolutionInfo.Create(SolutionId.CreateNewId()
                    , VersionStamp.Default)
                    );
            var projects =
                Build
                    .GetProjectsFromSolution(solutionFile)
                    .Where(IsCSProject);

            foreach (var project in projects)
                MkProject(workspace, project);

            ResolveProjectReferences(workspace);

            return workspace.CurrentSolution;
        }

        public static Document GetDocument(Solution solution, string projectName, string fileName) =>
            solution.Projects.First(p => p.Name == projectName)
            .Documents.First(d => d.Name == fileName);

        public static void ResolveProjectReferences(Workspace workspace)
        {
            ProjectId GetProjectId(Solution s, string pN) =>
                s.Projects.First(p => p.Name == pN).Id;

            var solution = workspace.CurrentSolution;

            foreach (var projectId in solution.ProjectIds)
            {
                var project = solution.GetProject(projectId);
                var projectReferences =
                    Build.GetProjectReferencesFromProject(project.FilePath);

                foreach(var projectReference in projectReferences)
                {
                    project =
                        project.AddProjectReference
                            ( new ProjectReference
                                ( GetProjectId
                                    ( workspace.CurrentSolution
                                    , projectReference
                                    )));
                }

                solution = project.Solution;
            }

            if (!workspace.TryApplyChanges(solution))
                System.Console.WriteLine("hae?");
        }
    }
}
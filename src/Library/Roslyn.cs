namespace Library
{
    using System.Linq;
    using System.IO;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;

    public static class Roslyn
    {
        public static Func<AdhocWorkspace, FileInfo, Project> MkProjectLoader(GetFilesFromProjectType getFilesFromProject) =>
            (aw, pi) => MkProject(getFilesFromProject, aw, pi);

        public static Project MkProject(GetFilesFromProjectType getFilesFromProject, AdhocWorkspace workspace, FileInfo project)
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
            var files = getFilesFromProject(project);

            foreach (var file in files)
                workspace.AddDocument
                    ( projectId
                    , file.Name
                    , SourceText.From(File.ReadAllText(file.FullName))
                    );

            return roslynProject;
        }

        public static Solution MkSolution(FileInfo solutionFile, Func<AdhocWorkspace, FileInfo, Project> mkProject)
        {
            if (solutionFile.Exists == false)
                throw new FileNotFoundException($"{solutionFile.FullName} not found.");

            if (solutionFile.Extension != ".sln")
                throw new FileLoadException($"{solutionFile.FullName} is not a solution file.");

            var workspace = new AdhocWorkspace();
            var solution =
                workspace.AddSolution
                    ( SolutionInfo.Create
                        ( SolutionId.CreateNewId()
                        , VersionStamp.Default
                        )
                    );
            var projects =
                Build
                    .GetProjectsFromSolution(solutionFile)
                    .Where(IsCSProject);

            foreach (var project in projects)
                mkProject(workspace, project);

            ResolveProjectReferences(workspace);

            return workspace.CurrentSolution;

            bool IsCSProject(FileInfo project) =>
                project.FullName.EndsWith("csproj");
        }

        public static Document GetDocument(Solution solution, string projectName, string fileName) =>
            solution
            .Projects.First(p => p.Name == projectName)
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
                                    )
                                )
                            );
                }

                solution = project.Solution;
            }

            if (workspace.TryApplyChanges(solution) is false)
                System.Console.WriteLine("Changes could not be applied.");
        }
    }
}
using static System.Console;

using Microsoft.Extensions.FileSystemGlobbing;
using Library;

IEnumerable<FileInfo> GetFilesForProject(FileInfo projectFileInfo)
{
    var matcher = new Microsoft.Extensions.FileSystemGlobbing.Matcher();

    matcher.AddInclude("/**/*.cs");
    matcher.AddExclude("/obj");

    var files =
        matcher.GetResultsInFullPath(projectFileInfo.DirectoryName).Select(f => new FileInfo(f));

    return files;
}

var solutionFileInfo =
    // new FileInfo(@"D:\Projects\CodeCruise\CodeCruise.sln");
    new FileInfo(@"D:\Projects\omnisharp-roslyn\OmniSharp.sln");
var solution =
    await Library.SolutionBuilder.CreateSolutionAsync(solutionFileInfo, GetFilesForProject);
var vertices =
    new List<Vertex>();
var edges =
    new List<Edge>();
var roslynDependencyGraph =
    solution.GetProjectDependencyGraph();
var solutionX =
    new Library.Structure.Solution
        ( new Library.Structure.Identifier("OmniSharp")
        , new Dictionary<Library.Structure.Identifier, Library.Structure.Project>()
        );

foreach (var project in solution.Projects)
{
    var document = project.Documents.First();
    var syntaxTree = await document.GetSyntaxTreeAsync();
    var compilation = await project.GetCompilationAsync();
    var cancellationToken = new CancellationToken();
    var semanticModel = compilation.GetSemanticModel(syntaxTree);
    var projectIdentifier = new Library.Structure.Identifier(project.Name);
    var visitor =
        new Library.ProjectTypeCollector
            ( cancellationToken
            , semanticModel
            , projectIdentifier
            );

    // compilation.GlobalNamespace.Accept(visitor);
    compilation.Assembly.Accept(visitor);
    solutionX.Projects.Add(projectIdentifier, visitor.Project);

    vertices.Add(new Vertex(project.Name, VertexKind.Project));

    var dependants =
        roslynDependencyGraph
        .GetProjectsThatDirectlyDependOnThisProject(project.Id)
        .Select(reference =>
                new Edge
                    ( new Vertex(project.Name, VertexKind.Project)
                    , new Vertex(solution.GetProject(reference)?.Name ?? string.Empty, VertexKind.Project)
                    )
                );

    edges.AddRange(dependants);
}

var (dependencyGraph, components) =
    DependencyGraph.Build(vertices, edges);

GraphVizWriter.WriteDot((dependencyGraph, components), @"D:\Projects\CodeCruise\omnisharp.dot");

ReadLine();
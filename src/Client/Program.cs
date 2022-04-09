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

foreach (var project in solution.Projects)
{
    // var document = project.Documents.First();
    var compilation = await project.GetCompilationAsync();
    // var st = await document.GetSyntaxTreeAsync();
    // var sr = await document.GetSyntaxRootAsync();

    var cancellationToken = new CancellationToken();
    var visitor = new Library.ExportedTypesCollector(cancellationToken);
    compilation.GlobalNamespace.Accept(visitor);
    // var types = compilation.GetTypeByMetadataName("").Accept()

    vertices.Add(new Vertex(project.Name));

    var dependants =
        roslynDependencyGraph
        .GetProjectsThatDirectlyDependOnThisProject(project.Id)
        .Select(reference =>
                new Edge
                    ( new Vertex(project.Name)
                    , new Vertex(solution.GetProject(reference)?.Name ?? string.Empty)
                    )
                );

    edges.AddRange(dependants);
}

var (dependencyGraph, components) =
    DependencyGraph.Build(vertices, edges);

GraphVizWriter.WriteDot((dependencyGraph, components), @"D:\Projects\CodeCruise\omnisharp.dot");

ReadLine();
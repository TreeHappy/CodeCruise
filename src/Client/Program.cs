using static System.Console;

var solutionFileInfo =
    new FileInfo(@"D:\Projects\CodeCruise\CodeCruise.sln");
var solution =
    await Library.SolutionBuilder.CreateSolutionAsync(solutionFileInfo);
var projectDependencyGraph =
    solution.GetProjectDependencyGraph();

foreach (var project in solution.Projects)
{
    WriteLine($"{project.Name}");

    foreach (var reference in project.ProjectReferences)
        WriteLine($"depends on {solution.GetProject(reference.ProjectId)?.Name}.");
}

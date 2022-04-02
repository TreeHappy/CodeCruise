using static System.Console;

var solutionFileInfo =
    new FileInfo(@"D:\Projects\CodeCruise\CodeCruise.sln");
var solution =
    await Library.SolutionBuilder.CreateSolutionAsync(solutionFileInfo);

foreach (var project in solution.Projects)
{
    WriteLine($"{project.Name}");

    foreach (var reference in project.ProjectReferences)
        WriteLine($"depends on {solution.GetProject(reference.ProjectId)?.Name}.");

    var compilation = await project.GetCompilationAsync();
    var attributes = compilation.Assembly.GetAttributes();

    foreach (var attribute in attributes)
        WriteLine($"{attribute.AttributeClass.Name}");

    foreach (var document in project.Documents)
        WriteLine($"{document.Name}");
}

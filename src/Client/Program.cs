using static System.Console;

var workspace =
    await Library.WorkspaceBuilder.CreateWorkspace(@"D:\Projects\CodeCruise\src\Client\Client.csproj");

foreach (var project in workspace.CurrentSolution.Projects)
    WriteLine($"{project.Name}");

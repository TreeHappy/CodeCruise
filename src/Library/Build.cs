namespace Library
{
    using System.Linq;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Collections.Generic;
    using System;

    public static class Build
    {
        private static (bool success, T @new) Map<T, U>(this (bool success, U old) x, Func<U, T> map) where T : class
        {
            if (x.success)
                return (true, map(x.old));
            else return (false, null);
        }
        private static (bool success, GroupCollection groups) MatchLine(Regex regEx, string line)
        {
            var match = regEx.Match(line);

            if (match.Success)
                return(true, match.Groups);
            else
                return (false, null);
        }
        // Gibt alle csproj und vcxproj Dateien der Solution zurueck
        public static IEnumerable<FileInfo> GetProjectsFromSolution(FileInfo solution)
        {
            var projectPattern =
                "Project.*=.*,.*\"(?<project>.*(cs|vcx)proj)";
            var projectRegEx =
                new Regex(projectPattern, RegexOptions.Compiled);

            (bool success, FileInfo project) GetProjectFromLine(string line) =>
                MatchLine(projectRegEx, line)
                .Map
                    ( m =>
                        new FileInfo
                            ( Path.Combine
                                ( solution.DirectoryName
                                , m["project"].Value
                                )
                            )
                    );

            return
            File.ReadAllLines(solution.FullName)
                .Select(GetProjectFromLine)
                .Where(r => r.success)
                .Select(r => r.project);
        }
        // Gibt alle Compile Included Files der Projektdatei zurueck
        public static IEnumerable<FileInfo> GetFilesFromProject(FileInfo project)
        {
            var includePattern =
                @"Compile Include=""(?<filename>(.*))""";
            var includeRegEx =
                new Regex(includePattern, RegexOptions.Compiled);

            (bool success, FileInfo file) GetFile(string line) =>
                MatchLine(includeRegEx, line)
                .Map
                    ( m =>
                        new FileInfo
                         ( Path.Combine
                            ( project.DirectoryName
                            , m["filename"].Value
                            )
                         )
                    );

            return
            File.ReadAllLines(project.FullName)
                .Select(GetFile)
                .Where(r => r.success)
                .Select(r => r.file);
        }

        public static IEnumerable<string> GetProjectReferencesFromProject(string projectLocation)
        {
            var referencePattern =
                @"ProjectReference Include=""(?<projectLocation>(.*))""";
            var referenceRegEx =
                new Regex(referencePattern, RegexOptions.Compiled);

            (bool success, string reference) GetReference(string line) =>
                MatchLine(referenceRegEx, line)
                .Map(m => Path.GetFileNameWithoutExtension(m["projectLocation"].Value));

            return
            File.ReadAllLines(projectLocation)
                .Select(GetReference)
                .Where(r => r.success)
                .Select(r => r.reference);
        }
    }
}
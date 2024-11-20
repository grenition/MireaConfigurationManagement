using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LibGit2Sharp;
using MireaConfigurationManagement.Core.Scenarios;

namespace MireaConfigurationManagement.GitDependencies;

public class GitDependenciesScenario : IScenario
{
    public string Key => "git_dependencies";

    public async Task Execute(CancellationToken token)
    {
        Console.WriteLine("Git dependencies scenario started!");

        while (!token.IsCancellationRequested)
        {
            try
            {
                Console.WriteLine("Enter the absolute path to the repository:");
                var repoPath = Console.ReadLine();
                Console.WriteLine("Enter the date threshold for commits (yyyy-MM-dd):");
                var dateInput = Console.ReadLine();

                if (!DateTime.TryParse(dateInput, out var dateThreshold))
                {
                    Console.WriteLine("Invalid date format. Use yyyy-MM-dd.");
                    continue;
                }

                if (!Repository.IsValid(repoPath))
                {
                    Console.WriteLine("Invalid repository path.");
                    continue;
                }

                var mermaidGraph = BuildDependencyGraph(repoPath, dateThreshold);
                var graphFilePath = Path.Combine(Path.GetTempPath(), "dependency_graph.mmd");
                File.WriteAllText(graphFilePath, mermaidGraph);

                var pngFilePath = Path.Combine(Path.GetTempPath(), "dependency_graph.png");
                GeneratePngFromMermaid(graphFilePath, pngFilePath);

                OpenImageFile(pngFilePath);

                Console.WriteLine("Graph visualization generated and opened.");
                
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private string BuildDependencyGraph(string repoPath, DateTime dateThreshold)
    {
        using var repo = new Repository(repoPath);

        var branches = repo.Branches;

        var graphBuilder = new System.Text.StringBuilder();
        graphBuilder.AppendLine("gitGraph");

        var mainBranch = repo.Branches.FirstOrDefault(b => b.IsRemote == false && b.FriendlyName == "main") ??
                         repo.Branches.FirstOrDefault(b => b.IsRemote == false && b.FriendlyName == "master");

        if (mainBranch == null)
        {
            throw new InvalidOperationException("Main or master branch not found.");
        }

        string FormatCommit(Commit? commit)
        {
            if (commit == null) return string.Empty;
            return "\"" + commit.Id.ToString().Substring(0, 6) + " - " + commit.MessageShort.Replace("\"", "'") + "\"";
        }
        
        var mainCommits = mainBranch.Commits
            .Where(c => c.Committer.When.DateTime > dateThreshold)
            .OrderBy(c => c.Committer.When)
            .ToList();

        foreach (var commit in mainCommits)
        {
            graphBuilder.AppendLine($"    commit id:{FormatCommit(commit)}");
        }

        foreach (var branch in branches.Where(b => b != mainBranch && !b.IsRemote))
        {
            graphBuilder.AppendLine($"    branch {branch.FriendlyName}");
            graphBuilder.AppendLine($"    checkout {branch.FriendlyName}");

            var branchCommits = branch.Commits
                .Where(c => c.Committer.When.DateTime > dateThreshold)
                .OrderBy(c => c.Committer.When)
                .ToList();

            foreach (var commit in branchCommits)
            {
                graphBuilder.AppendLine($"    commit id:{FormatCommit(commit)}");
            }

            graphBuilder.AppendLine($"    checkout {mainBranch.FriendlyName}");
            graphBuilder.AppendLine($"    merge {branch.FriendlyName}");
        }

        foreach (var commit in mainCommits.Skip(mainCommits.Count - 2))
        {
            graphBuilder.AppendLine($"    commit id:{FormatCommit(commit)}");
        }

        return graphBuilder.ToString();
    }

    private void GeneratePngFromMermaid(string mermaidFilePath, string pngFilePath)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "mmdc",
                Arguments = $"-i \"{mermaidFilePath}\" -o \"{pngFilePath}\" -w 1920 -H 1080",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                var error = process.StandardError.ReadToEnd();
                throw new InvalidOperationException($"MermaidCLI error: {error}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unable to generate PNG from Mermaid graph: {ex.Message}");
        }
    }

    private void OpenImageFile(string filePath)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unable to open the image file: {ex.Message}");
        }
    }
}

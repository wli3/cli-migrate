// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.DotNet.Cli.CommandLine;
using Microsoft.DotNet.Cli.Utils;
using Microsoft.DotNet.Tools.MigrateCommand;
using NuGet.Frameworks;
using NuGet.Packaging;

namespace Microsoft.DotNet.Tools.Test.Utilities
{
    public sealed class MigrateTestCommand
    {
        private string _withWorkingDirectory;

        public MigrateTestCommand()
        {
        }

        public CommandResult Execute(string args = "")
        {
            Console.WriteLine("migrate " + args);
            var resut = MigrateCommandParser.Migrate().Parse("migrate " + args);
            var exitcode = resut["migrate"].Value<MigrateCommand.MigrateCommand>().Execute();
            return new CommandResult(new ProcessStartInfo(), exitcode, "", "");
        }

        public MigrateTestCommand WithWorkingDirectory(DirectoryInfo withWorkingDirectory)
        {
            _withWorkingDirectory = withWorkingDirectory.FullName;
            return this;
        }

        public MigrateTestCommand WithWorkingDirectory(string withWorkingDirectory)
        {
            _withWorkingDirectory = withWorkingDirectory;
            return this;
        }

        public CommandResult ExecuteWithCapturedOutput(string args = "")
        {
            return Execute(args);
        }
    }

    public class CallStage0DotnetSlnToManipulateSolutionFile : ICanManipulateSolutionFile
    {
        public void AddProjectToSolution(string solutionFilePath, string projectFilePath)
        {
            var exitCode = new DotnetCommand().Execute($"sln {solutionFilePath} add {projectFilePath}").ExitCode;

            if (exitCode != 0)
            {
                throw new InvalidOperationException();
            }
        }

        public void RemoveProjectFromSolution(string solutionFilePath, string projectFilePath)
        {
            var exitCode = new DotnetCommand().Execute($"sln {solutionFilePath} remove {projectFilePath}").ExitCode;

            if (exitCode != 0)
            {
                throw new InvalidOperationException();
            }
        }
    }

    public class CallStage0DotnetNewToAddTemplate : ICanCreateDotnetCoreTemplate
    {
        public void CreateWithWithEphemeralHiveAndNoRestore(
            string templateName,
            string outputDirectory,
            string workingDirectory)
        {
            var result = new NewCommand()
                .WithWorkingDirectory(workingDirectory)
                .Execute($"{templateName} -o {outputDirectory} --debug:ephemeral-hive");

            if (result.ExitCode != 0)
            {
                throw new InvalidOperationException(result.StdErr);
            }
        }
    }
}
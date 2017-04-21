// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.DotNet.Cli.CommandLine;
using Microsoft.DotNet.Cli.Utils;

namespace Microsoft.DotNet.Tools.Test.Utilities
{
    public sealed class MigrateTestCommand
    {
        private StringBuilder _stdOut;

        public MigrateTestCommand()
        {
            _stdOut = new StringBuilder();
        }

        public CommandResult Execute(string args = "")
        {
            var resut = Migrate().Parse("migrate " + args);
            var MigrateCommand = resut["migrate"].Value<MigrateCommand.MigrateCommand>();
            var output = new StringWriter();

            var exitCode = resut["migrate"].Value<MigrateCommand.MigrateCommand>().Execute();

            return new CommandResult(
                  new ProcessStartInfo(), exitCode, _stdOut.ToString(), "");
        }

        public class ConsoleOutput : IDisposable
        {
            private StringWriter stringWriter;
            private TextWriter originalOutput;

            public ConsoleOutput()
            {
                stringWriter = new StringWriter();
                originalOutput = Console.Out;
                Console.SetOut(stringWriter);
            }

            public string GetOuput()
            {
                return stringWriter.ToString();
            }

            public void Dispose()
            {
                Console.SetOut(originalOutput);
                stringWriter.Dispose();
            }
        }

        public void SetConsoleOut(TextWriter newOut)
        {
            Console.SetOut(newOut);
        }
        public void SetConsoleError(TextWriter newError)
        {
            Console.SetError(newError);
        }
        public MigrateTestCommand WithWorkingDirectory(DirectoryInfo withWorkingDirectory)
        {
            Directory.SetCurrentDirectory(withWorkingDirectory.FullName);
            return this;
        }

        public MigrateTestCommand WithWorkingDirectory(string withWorkingDirectory)
        {
            Directory.SetCurrentDirectory(withWorkingDirectory);
            return this;
        }

        public CommandResult ExecuteWithCapturedOutput(string args = "")
        {
            return Execute(args);
        }

        public Cli.CommandLine.Command Migrate() =>
          Create.Command(
              "migrate",
              ".NET Migrate Command",
              Accept.ZeroOrOneArgument()
                  .MaterializeAs(o =>
                      new MigrateCommand.MigrateCommand(
                          new CallStage0DotnetSlnToManipulateSolutionFile(),
                          new CallStage0DotnetNewToAddTemplate(),
                          o.ValueOrDefault<string>("--template-file"),
                          o.Arguments.FirstOrDefault(),
                          o.ValueOrDefault<string>("--sdk-package-version"),
                          o.ValueOrDefault<string>("--xproj-file"),
                          o.ValueOrDefault<string>("--report-file"),
                          o.ValueOrDefault<bool>("--skip-project-references"),
                          o.ValueOrDefault<bool>("--format-report-file-json"),
                          o.ValueOrDefault<bool>("--skip-backup"), (l) => _stdOut.Append(l)))
                  .With(name: "",
                      description: ""),
              Create.Option("-t|--template-file",
                  "",
                  Accept.ExactlyOneArgument()),
              Create.Option("-v|--sdk-package-version",
                  "",
                  Accept.ExactlyOneArgument()),
              Create.Option("-x|--xproj-file",
                  "",
                  Accept.ExactlyOneArgument()),
              Create.Option("-s|--skip-project-references",
                  ""),
              Create.Option("-r|--report-file",
                  "",
                  Accept.ExactlyOneArgument()),
              Create.Option("--format-report-file-json",
                  ""),
              Create.Option("--skip-backup",
                  ""));
    }
}
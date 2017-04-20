// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.DotNet.Cli.CommandLine;
using Microsoft.DotNet.Cli.Utils;
using Microsoft.DotNet.Tools.MigrateCommand;
using NuGet.Frameworks;
using NuGet.Packaging;

namespace Microsoft.DotNet.Tools.Test.Utilities
{
    public sealed class MigrateCommand
    {
        private Tools.MigrateCommand.MigrateCommand _migrateCommand;
        private string _withWorkingDirectory;

        public MigrateCommand()
        {
        }

        public CommandResult Execute(string args = "")
        {
            var resut = MigrateCommandParser.Migrate().Parse("migrate "+ args);
            var exitcode = resut["migrate"].Value<Tools.MigrateCommand.MigrateCommand>().Execute();
            return new CommandResult(null, exitcode, "", "");
        }

        public MigrateCommand WithWorkingDirectory(DirectoryInfo withWorkingDirectory)
        {
            _withWorkingDirectory = withWorkingDirectory.FullName;
            return this;
        }

        public MigrateCommand WithWorkingDirectory(string withWorkingDirectory)
        {
            _withWorkingDirectory = withWorkingDirectory;
            return this;
        }

        public CommandResult ExecuteWithCapturedOutput(string args = "")
        {
            return Execute(args);
        }

        public class CallStage0DotnetSln: ICanManipulateSolutionFile
        {
            public int Execute(string dotnetPath, string slnPath, string projPath, string commandName)
            {
                return new DotnetCommand().Execute($"sln {slnPath} {commandName} {projPath}").ExitCode;
            }
        }

        public class CallStage0DotnetNewCommandFactory : ICommandFactory
        {
            public ICommand Create(string commandName, IEnumerable<string> args, NuGetFramework framework = null, string configuration = "Debug")
            {
                return new Stage0DotnetNew(args);
            }

            public class Stage0DotnetNew : ICommand
            {
                private readonly IEnumerable<string> _args;
                private NewCommand _command;

                public Stage0DotnetNew(IEnumerable<string> args)
                {
                    _args = args;
                    _command = new NewCommand();
                }

                public CommandResult Execute()
                {
                    var exitcode = _command.Execute(string.Join(" ", _args)).ExitCode;
                    return new CommandResult(null, exitcode, "", "");
                }

                public ICommand WorkingDirectory(string projectDirectory)
                {
                    _command = _command.WithWorkingDirectory(projectDirectory);
                    return this;
                }

                public ICommand EnvironmentVariable(string name, string value)
                {
                    throw new NotImplementedException();
                }

                public ICommand CaptureStdOut()
                {
                    throw new NotImplementedException();
                }

                public ICommand CaptureStdErr()
                {
                    throw new NotImplementedException();
                }

                public ICommand ForwardStdOut(TextWriter to = null, bool onlyIfVerbose = false, bool ansiPassThrough = true)
                {
                    throw new NotImplementedException();
                }

                public ICommand ForwardStdErr(TextWriter to = null, bool onlyIfVerbose = false, bool ansiPassThrough = true)
                {
                    throw new NotImplementedException();
                }

                public ICommand OnOutputLine(Action<string> handler)
                {
                    throw new NotImplementedException();
                }

                public ICommand OnErrorLine(Action<string> handler)
                {
                    throw new NotImplementedException();
                }

                public CommandResolutionStrategy ResolutionStrategy { get; }
                public string CommandName { get; }
                public string CommandArgs { get; }
            }
        }
    }
}
// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.DotNet.Cli.Utils;
using Microsoft.DotNet.Tools.MigrateCommand;
using NuGet.Frameworks;

namespace Microsoft.DotNet.Tools.Test.Utilities
{
    public sealed class MigrateCommand
    {

        public MigrateCommand()
        {
            var _migrateCommand = new Tools.MigrateCommand.MigrateCommand();
        }

        public override CommandResult Execute(string args = "")
        {
            var MigrateCommand = new Tools.MigrateCommand.MigrateCommand();
            args = $"migrate {args}";

            return base.Execute(args);
        }

        public override CommandResult ExecuteWithCapturedOutput(string args = "")
        {
            args = $"migrate {args}";
            return base.ExecuteWithCapturedOutput(args);
        }

        public class CallStage0SolutionFileManipulator: ICanManipulateSolutionFile
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
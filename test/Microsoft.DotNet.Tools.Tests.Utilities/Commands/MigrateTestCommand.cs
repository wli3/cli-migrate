// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.IO;
using Microsoft.DotNet.Cli.CommandLine;
using Microsoft.DotNet.Cli.Utils;

namespace Microsoft.DotNet.Tools.Test.Utilities
{
    public sealed class MigrateTestCommand
    {
        private StreamForwarder _stdOut;
        private StreamForwarder _stdErr;

        public MigrateTestCommand()
        {
            _stdOut = new StreamForwarder();
            _stdErr = new StreamForwarder();
        }

        public CommandResult Execute(string args = "")
        {
            var resut = MigrateCommandParser.Migrate().Parse("migrate " + args);
            var MigrateCommand = resut["migrate"].Value<MigrateCommand.MigrateCommand>();
            var output = new StringWriter();

            MigrateCommand.AddTraceEvent((c) => output.Write(c));

            var exitCode = resut["migrate"].Value<MigrateCommand.MigrateCommand>().Execute();

            var otuput = output.ToString();
            return new CommandResult(
                  new ProcessStartInfo(), exitCode, otuput, "");
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
    }
}
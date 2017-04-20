// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Microsoft.DotNet.Cli.CommandLine;

namespace Microsoft.DotNet.Tools.Test.Utilities
{
    internal static class MigrateCommandParser
    {
        public static Command Migrate() =>
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
                            o.ValueOrDefault<bool>("--skip-backup")))
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

    public static class AppliedOptionExtensions
    {
        public static T ValueOrDefault<T>(this AppliedOption parseResult, string alias)
        {
            return parseResult
                .AppliedOptions
                .Where(o => o.HasAlias(alias))
                .Select(o => o.Value<T>())
                .SingleOrDefault();
        }
    }
}
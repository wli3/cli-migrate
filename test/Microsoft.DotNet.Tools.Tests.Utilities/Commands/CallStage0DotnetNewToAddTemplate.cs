// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.DotNet.Tools.MigrateCommand;
using NuGet.Frameworks;
using NuGet.Packaging;

namespace Microsoft.DotNet.Tools.Test.Utilities
{

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
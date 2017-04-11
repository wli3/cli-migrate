// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;

namespace Microsoft.DotNet.Cli.Sln.Internal
{
    public static class PathUtility
    {
        public static string GetPathWithForwardSlashes(string path)
        {
            return path.Replace('\\', '/');
        }

        public static string GetPathWithBackSlashes(string path)
        {
            return path.Replace('/', '\\');
        }

         public static string RemoveExtraPathSeparators(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            var components = path.Split(Path.DirectorySeparatorChar);
            var result = string.Empty;

            foreach (var component in components)
            {
                if (!string.IsNullOrEmpty(component))
                {
                    result = Path.Combine(result, component);
                }
            }

            if (path[path.Length-1] == Path.DirectorySeparatorChar)
            {
                result += Path.DirectorySeparatorChar;
            }

            return result;
        }


        public static string GetPathWithDirectorySeparator(string path)
        {
            if (Path.DirectorySeparatorChar == '/')
            {
                return GetPathWithForwardSlashes(path);
            }
            else
            {
                return GetPathWithBackSlashes(path);
            }
        }
    }
}

﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Cmd.Net
{
    /// <summary>
    /// Provides utility methods that perform common tasks.
    /// </summary>
    public static class CommandHelpers
    {
        #region Public Methods

        /// <summary>
        /// Writes a logo to the standard output stream.
        /// </summary>
        public static void WriteLogo()
        {
            WriteLogo(Console.Out);
        }

        /// <summary>
        /// Writes a logo to the specified output stream.
        /// </summary>
        /// <param name="output">A <see cref="T:System.IO.TextWriter" /> that represents an output stream.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="output" /> is null.</exception>
        public static void WriteLogo(TextWriter output)
        {
            if (output == null)
                throw new ArgumentNullException("output");

            Assembly entryAssembly = Assembly.GetEntryAssembly();
            AssemblyTitleAttribute assemblyTitleAttribute = entryAssembly.GetCustomAttribute<AssemblyTitleAttribute>();

            if (assemblyTitleAttribute == null)
                output.Write(entryAssembly.FullName);
            else
                output.Write(assemblyTitleAttribute.Title);

            output.Write(' ');
            output.Write('[');
            output.Write("Version");
            output.Write(' ');

            AssemblyVersionAttribute assemblyVersionAttribute = entryAssembly.GetCustomAttribute<AssemblyVersionAttribute>();

            if (assemblyVersionAttribute == null)
                output.Write(entryAssembly.GetName().Version);
            else
                output.Write(assemblyVersionAttribute.Version);

            output.WriteLine(']');

            AssemblyCopyrightAttribute assemblyCopyrightAttribute = entryAssembly.GetCustomAttribute<AssemblyCopyrightAttribute>();

            if (assemblyCopyrightAttribute != null)
                output.WriteLine(assemblyCopyrightAttribute.Copyright.Replace("©", "(c)"));

            output.WriteLine();
        }

        #endregion

        #region Internal Methods

        internal static void ValidateName(string argumentName, string value, bool isNullValid)
        {
            if (!isNullValid)
            {
                if (value == null)
                    throw new ArgumentNullException(argumentName);

                if (value.Length == 0)
                    throw new ArgumentException(null, argumentName);
            }

            if (!IsValidName(value, isNullValid))
                throw new ArgumentException(null, argumentName);
        }

        internal static bool IsValidName(string name, bool isNullValid)
        {
            if (string.IsNullOrEmpty(name))
                return isNullValid;

            for (int i = 0; i < name.Length; i++)
            {
                if (!IsValidNameCharacter(name[i]))
                    return false;
            }

            return true;
        }

        internal static bool IsValidNameCharacter(char c)
        {
            return char.IsLetterOrDigit(c) || c == '-' || c == '_';
        }

        internal static void ValidateFlagName(string argumentName, char value)
        {
            if (!IsValidFlagName(value))
                throw new ArgumentException(null, argumentName);
        }

        internal static bool IsValidFlagName(char c)
        {
            return char.IsLetter(c) && char.IsUpper(c);
        }

        #endregion
    }
}

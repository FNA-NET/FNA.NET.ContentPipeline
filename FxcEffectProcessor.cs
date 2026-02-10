using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace FNA.NET.ContentPipeline
{
    [ContentProcessor(DisplayName = "Effect - Fxc(FNA)")]
    public class FxcEffectProcessor : EffectProcessor
    {
        static string FxcExePath;

        static void EnsureFxcTool()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string fnaToolsFolder = Path.Combine(appDataPath, "FNA.NET", "tools");
            if (!Directory.Exists(fnaToolsFolder))
                Directory.CreateDirectory(fnaToolsFolder);

            FxcExePath = Path.Combine(fnaToolsFolder, Resources.FxcExeFileName);

            if (File.Exists(FxcExePath))
                return;

            File.WriteAllBytes(FxcExePath, Resources.FxcExeBinary);
            File.WriteAllBytes(Path.Combine(fnaToolsFolder, Resources.D3dcompilerDllFileName), Resources.D3dcompilerBinary);
            File.WriteAllBytes(Path.Combine(fnaToolsFolder, Resources.D3dx9DllFileName), Resources.D3dx9Binary);
        }

        static FxcEffectProcessor()
        {
            EnsureFxcTool();
        }

        public override CompiledEffectContent Process(EffectContent input, ContentProcessorContext context)
        {
            string compiledTempFile = Path.GetTempFileName() + ".fxb";

            var defineList = new List<string>();
            defineList.Add("/D OPENGL");
            if (!string.IsNullOrEmpty(Defines))
            {
                foreach (var define in Defines.Split(";", StringSplitOptions.TrimEntries))
                {
                    if (string.IsNullOrEmpty(define)) continue;
                    defineList.Add($"/D {define}");
                }
            }

            var defineStr = string.Join(" ", defineList);

            Process process;

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = FxcExePath,
                        Arguments = string.Format("/nologo /Vd /T fx_2_0 {0} /Fo\"{1}\" \"{2}\"",
                            defineStr, compiledTempFile, input.Identity.SourceFilename),
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        UseShellExecute = false
                    }
                };
            }
            else
            {
                var relativeSourceFilename = Path.GetRelativePath(Environment.CurrentDirectory, input.Identity.SourceFilename);
                process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "wine",
                        Arguments = string.Format("\"{0}\" /nologo /Vd /T fx_2_0 {1} /Fo\"{2}\" \"{3}\"",
                            FxcExePath, defineStr, compiledTempFile, relativeSourceFilename),
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        UseShellExecute = false
                    }
                };
            }

            process.Start();
            process.WaitForExit();

            if (!File.Exists(compiledTempFile))
            {
                string output = process.StandardError.ReadToEnd();

                throw new InvalidContentException(output, input.Identity);
            }

            byte[] buffer = File.ReadAllBytes(compiledTempFile);
            return new CompiledEffectContent(buffer);
        }
    }
}

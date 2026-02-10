using System.IO;

namespace FNA.NET.ContentPipeline
{
	internal class Resources
	{
		internal const string FxcExeFileName = "fxc.exe";
		internal const string D3dcompilerDllFileName = "d3dcompiler_43.dll";
		internal const string D3dx9DllFileName = "d3dx9_43.dll";

		static byte[] fxcExeBinary;
		public static byte[] FxcExeBinary
		{
			get
			{
				if (fxcExeBinary == null)
					fxcExeBinary = GetResource("tools." + FxcExeFileName);

				return fxcExeBinary;
			}
		}

		static byte[] d3dcompilerBinary;
		public static byte[] D3dcompilerBinary
		{
			get
			{
				if (d3dcompilerBinary == null)
					d3dcompilerBinary = GetResource("tools." + D3dcompilerDllFileName);

				return d3dcompilerBinary;
			}
		}

		static byte[] d3dx9Binary;
		public static byte[] D3dx9Binary
		{
			get
			{
				if (d3dx9Binary == null)
					d3dx9Binary = GetResource("tools." + D3dx9DllFileName);

				return d3dx9Binary;
			}
		}

		private static byte[] GetResource(string filename)
		{
			Stream stream = typeof(Resources).Assembly.GetManifestResourceStream(
				typeof(Resources).Assembly.GetName().Name + "." + filename
			);
			using (MemoryStream ms = new MemoryStream())
			{
				stream.CopyTo(ms);
				return ms.ToArray();
			}
		}
	}
}

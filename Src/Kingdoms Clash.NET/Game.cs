namespace Kingdoms_Clash.NET
{
	public class Game
		: ClashEngine.NET.Game
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("Kingdoms Clash.NET");

		public Game()
			: base("Pierwsze okno stworzone za pomocą ClashEngine.NET!", 800, 600, false)
		{ }

		public override void Init()
		{
#if !DEBUG
			ClashEngine.NET.SystemInformation si = ClashEngine.NET.SystemInformation.Instance;
			Logger.Info("System information:");
			Logger.Info("OS: {0}, 64-bit: {1}", si.System.ToString(), si.Is64BitOS);
			Logger.Info("CLR version: {0}", si.CLRVersion.ToString());
			Logger.Info("RAM: {0}", si.MemorySize);
			Logger.Info("Processor: {0}, {1} MHz, {2} cores, {3}", si.ProcessorName, si.ProcessorSpeed, si.NumberOfCores, si.ProcessorArchitecture.ToString());
			Logger.Info("Graphics card: {0}, memory: {1}, driver version: {2}", si.GraphicsCardName, si.VRAMSize, si.GraphicsDriverVersion);
			Logger.Info("OpenGL version: {0}, GLSL version: {1}", si.OpenGLVersion, si.GLSLVersion);
			Logger.Info("Supported extensions: {0}", si.Extensions);
#endif
			base.Init();
		}

		public override void Render()
		{
			base.Render();
		}

		static void Main(string[] args)
		{
			using (var game = new Game())
			{
				game.Run();
			}
		}
	}
}

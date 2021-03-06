namespace DesperateDevs.Logging
{
	public class Logger
	{
		public LogLevel logLevel
		{
			get;
			set;
		}

		public string name
		{
			get;
			private set;
		}

		public event LogDelegate OnLog;

		public Logger(string name)
		{
			this.name = name;
		}

		public void Trace(string message)
		{
			log(LogLevel.Trace, message);
		}

		public void Debug(string message)
		{
			log(LogLevel.Debug, message);
		}

		public void Info(string message)
		{
			log(LogLevel.Info, message);
		}

		public void Warn(string message)
		{
			log(LogLevel.Warn, message);
		}

		public void Error(string message)
		{
			log(LogLevel.Error, message);
		}

		public void Fatal(string message)
		{
			log(LogLevel.Fatal, message);
		}

		public void Assert(bool condition, string message)
		{
			if (condition)
			{
				return;
			}
			throw new FablAssertException(message);
		}

		private void log(LogLevel logLvl, string message)
		{
			if (this.OnLog != null && logLevel <= logLvl)
			{
				this.OnLog(this, logLvl, message);
			}
		}

		public void Reset()
		{
			this.OnLog = null;
		}
	}
}

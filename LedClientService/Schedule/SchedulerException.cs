using System;

namespace LedClientService.Schedule
{
	/// <summary>
	/// Summary description for SchedulerException.
	/// </summary>
	public class SchedulerException : Exception
	{
		public SchedulerException(string msg) : base(msg)
		{
		}
	}
}

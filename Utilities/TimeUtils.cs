using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoxiumMod.Utilities
{
	public static class TimeUtils
	{
	}

	public struct Time
	{
		/// <summary>
		/// The amount of hours the time set is.
		/// </summary>
		public int Hours { get; private set; }

		/// <summary>
		/// The amount of minutes the time set is.
		/// </summary>
		public int Minutes { get; private set; }

		/// <summary>
		/// The amount of seconds the time set is.
		/// </summary>
		public int Seconds { get; private set; }

		/// <summary>
		/// The amount of ticks the time set is.
		/// </summary>
		public int Ticks { get; private set; }

		public Time(int seconds, int minutes = 0, int hours = 0)
		{
			Ticks = (hours * 3600 + minutes * 60 + seconds) * 60;
			Seconds = hours * 3600 + minutes * 60 + seconds;
			Minutes = hours * 60 + minutes + (seconds >= 30 ? seconds : 0);
			Hours = hours + (minutes >= 30 ? minutes : 0);
		}

		public static bool operator ==(Time left, Time right) => left.Equals(right);

		public static bool operator !=(Time left, Time right) => !(left == right);

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj))
				return true;

			return false;
		}

		public override int GetHashCode()
		{
			throw new NotImplementedException();
		}
	}
}

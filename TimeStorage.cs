using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.PluginSdkDemo
{
	public class TimeStorage
	{
		public string Track { get; set; }
		public string Car { get; set; }
		public double AllTimeBest { get; set; }

		var timeList = new List<TimeStorage>()
		{
			new TimeStorage {Track = "silverstone", Car = "porsche 911", AllTimeBest = 1234535};
		};


	}
}

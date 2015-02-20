﻿using System;
using System.Linq;

namespace PerfLogger
{
	class Program
	{
		static void Main(string[] args)
		{
			var sum = 0.0;
			using (PerfLogger.Measure("100M for iterations"))
				for (var i = 0; i < 100000000; i++) sum += i;
			using (PerfLogger.Measure("100M LINQ iterations"))
				sum -= Enumerable.Range(0, 100000000).Sum(i => (double)i);
			Console.WriteLine(sum);
		}
	}
}
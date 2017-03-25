﻿using System;
using System.Collections.Generic;
using System.Text;
using tzatziki.minutz.models.Entities;

namespace tzatziki.minutz.interfaces.Repositories
{
	public interface IInstanceRepository
	{
		IEnumerable<Instance> GetInstances(string connectionString);
	}
}
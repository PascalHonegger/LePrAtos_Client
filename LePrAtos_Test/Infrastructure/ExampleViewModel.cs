// Projekt: LePrAtos
// Copyright (c) 2016
// Author: Honegger, Pascal (ext)

using System.Collections.Generic;
using LePrAtos.Infrastructure;

namespace LePrAtos_Test.Infrastructure
{
	public class ExampleViewModel : ViewModelBase
	{
		public string ExampleProperty { get; set; }

		public string NeverHappyProperty
		{
			get { return null; }
			set
			{
				SetErrorForProperty(value);
			}
		}

		public List<string> NeverHappyPropertyList
		{
			get { return null; }
			set
			{
				SetErrorForProperty(value);
			}
		}
	}
}

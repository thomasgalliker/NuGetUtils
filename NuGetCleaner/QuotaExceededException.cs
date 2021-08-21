// Copyright (c) 2021 SIL International
// This software is licensed under the MIT License (http://opensource.org/licenses/MIT)

using System;

namespace SIL.NuGetCleaner
{
	public class QuotaExceededException: SystemException
	{
		public QuotaExceededException(string message) : base(message)
		{
		}
	}
}
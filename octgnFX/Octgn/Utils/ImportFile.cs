﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Octgn.Utils
{
	public class ImportFile
	{
		public string Filename { get; set; }
		public ImportFileStatus Status { get; set; }
		public string Message { get; set; }

		public string StatusText
		{
			get
			{
				switch (Status)
				{
					case ImportFileStatus.Imported:
						return "OK";

					default:
						return "FAIL: " + Message;

				}
			}
		}
	}

	public enum ImportFileStatus
	{
		Unprocessed,
		Imported,
		Error,
		FileNotFound
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTask.Common
{
    public class APIResponse
    {
			public string StatusCode { get; set; } = string.Empty;
			public string Message { get; set; } = string.Empty;
			public object Data { get; set; }
	}
	public static class APIResponseGenerator
	{
		public static APIResponse GenerateResponseMessage(string statusCode, string message, object data)
		{
			return new APIResponse
			{
				StatusCode = statusCode,
				Message = message,
				Data = data
			};
		}
	}
}

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Synapsys.AdminPortal.Core.Utility
{
	public class AutoIntConverter : JsonConverter<int>
	{
		public override bool CanConvert(Type typeToConvert)
		{
			return typeof(int) == typeToConvert;
		}

		public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.String)
			{
				var str = reader.GetString();
				return int.TryParse(str, out var val) ? val : 0;
			}
			if (reader.TokenType == JsonTokenType.Number)
			{
				return reader.TryGetInt32(out var val) ? val : 0;
			}
			return 0;
		}

		public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
		{
			writer.WriteNumberValue(value);
		}
	}

	public class AutoDecimalConverter : JsonConverter<decimal>
	{
		public override bool CanConvert(Type typeToConvert)
		{
			return typeof(decimal) == typeToConvert;
		}

		public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.String)
			{
				var str = reader.GetString();

				return decimal.TryParse(str, out var val) ? val : 0;
			}
			if (reader.TokenType == JsonTokenType.Number)
			{
				return reader.TryGetDecimal(out var val) ? val : 0;
			}
			return 0;
		}

		public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
		{
			writer.WriteNumberValue(value);
		}
	}

	public class AutoDoubleConverter : JsonConverter<double>
	{
		public override bool CanConvert(Type typeToConvert)
		{
			return typeof(double) == typeToConvert;
		}

		public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.String)
			{
				var str = reader.GetString();

				return double.TryParse(str, out var val) ? val : 0;
			}
			if (reader.TokenType == JsonTokenType.Number)
			{
				return reader.TryGetDouble(out var val) ? val : 0;
			}
			return 0;
		}

		public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
		{
			writer.WriteNumberValue(value);
		}
	}

	public class AutoNullableDateConverter : JsonConverter<Nullable<DateTime>>
	{
		public override bool CanConvert(Type typeToConvert)
		{
			return typeof(Nullable<DateTime>) == typeToConvert;
		}

		public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return reader.TryGetDateTime(out var val) ? val : (DateTime?)null;
		}

		public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
		{
			if (value == null)
			{
				writer.WriteNullValue();
			}
			else
			{
				writer.WriteStringValue(value.Value);
			}
		}
	}
}

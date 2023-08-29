using System.Security.Cryptography;

namespace Aristino.Helper
{
	public static class OrderNumberGenerator
	{
		public static string Generate()
		{
			var minimumNumber = 000000001;
			var maximumNumber = Int32.MaxValue;
			var randomNumber=RandomNumberGenerator.GetInt32(minimumNumber, maximumNumber);
			string OrderNumber = "#" + randomNumber;
			return OrderNumber;
		}
	}
}

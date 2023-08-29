namespace Aristino.Helper
{
	public static class CheckSpecialCharacter
	{
		public static bool Check(string text)
		{
			if(text.Any(x=>!char.IsLetterOrDigit(x)))
				return true;
			return false;
		}
	}
}

namespace Aristino.Helper
{
    public static class StartAndEndOfDay
    {
        public static DateTime StartOfDay(DateTime date)
        {
            var startDay = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0,0);
            return startDay;
        }
        public static DateTime EndOfDay(DateTime date)
        {
            var endDate=new DateTime(date.Year,date.Month,date.Day,23,59,59,999);
            return endDate;
        }
    }
}

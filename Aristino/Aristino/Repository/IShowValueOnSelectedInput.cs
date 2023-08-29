namespace Aristino.Repository
{
    public interface IShowValueOnSelectedInput<T> where T : class
    {
        List<T> Show(dynamic ViewBag, List<string> Value);
    }
}

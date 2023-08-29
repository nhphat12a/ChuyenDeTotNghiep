using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Aristino.Repository
{
    public class ShowValueOnSelectedInput<T> : IShowValueOnSelectedInput<T> where T : class
    {
        private DbContext _dbContext;
        public ShowValueOnSelectedInput(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<T> Show(dynamic ViewBag, List<string> Value)
        {
            var ViewModel=_dbContext.Set<T>();
            var a=new List<T>();
            foreach(var item in ViewBag)
            {
                if (Value.Contains(item))
                {
                    Value.Remove(item);
                }
            }
            return a;
            //foreach(var item in Value)
            //{
            //    ViewModel.Add(item);
            //}    
        }
    }
}

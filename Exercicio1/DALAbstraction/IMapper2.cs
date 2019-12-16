using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALAbstraction
{
    public interface IMapper2<T, Tid1, Tid2>
    {
        void Create(T entity);
        T Read(Tid1 id1, Tid2 id2);
        void Update(T entity);
        void Delete(T entity);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALAbstraction
{
    public interface IMapper<T, Tid>
    {
        void Create(T entity);
        T Read(Tid id);
        void Update(T entity);
        void Delete(T entity);
    }
}

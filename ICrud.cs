using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractCrudRepository
{
    public interface ICrud <K,TDTO>
    {
        TDTO Create(TDTO EntityDTO);
        TDTO Read(K id);
        IList<TDTO> List();
        TDTO Update(TDTO EntityDTO);
        TDTO Delete(K id);
    }
}

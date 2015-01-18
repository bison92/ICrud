using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractCrudRepository
{
    public interface IDBFactory<TDB>
    {
        TDB GetInstace();
    }
}

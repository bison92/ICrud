using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractCrudRepository
{
    public class UpdateException<TEntity>:Exception
    {
        public UpdateException() :base(String.Format("Se produjo un error intentar actualizar la entidad de tipo {0} en la base de datos", typeof(TEntity).ToString() ))
        {
            
        }
    }
}

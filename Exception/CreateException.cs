using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractCrudRepository
{
    public class CreateException<TEntity>: Exception
    {
        public CreateException() :base(String.Format("Se produjo un error intentar crear la entidad de tipo {0} en la base de datos", typeof(TEntity).ToString() ))
        {
            
        }
    }
}

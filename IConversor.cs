using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbstractCrudRepository
{
    public interface IConversor<T,TDTO>
    {
        T DTO2Entity(TDTO dto);
        TDTO Entity2DTO(T entity);
    }
}

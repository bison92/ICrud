/*
 *  This file is part of ICrud:AbstractCrudRepository.
 *
 *  ICrud:AbstractCrudRepository is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  ICrud:AbstractCrudRepository is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with Foobar.  If not, see <http://www.gnu.org/licenses/>.
 */

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

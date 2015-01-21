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
using System.Text;

namespace ICrud
{
    public class UpdateException<TEntity> : Exception
    {
        public UpdateException()
            : base(String.Format("Error ocurred when trying to update an entity of type {0} on the database", typeof(TEntity).ToString()))
        {

        }
    }
}

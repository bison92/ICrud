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
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace ICrud
{
    public abstract class AbstractRepository<Key, TEntity, TDTO, TDB> : ICrud<Key, TDTO>
        where TEntity : class
        where TDB : DbContext
    {
        public IDBFactory<TDB> DBFactory { get; set; }
        public IConversor<TEntity, TDTO> Conversor { get; set; }
        public AbstractRepository(IDBFactory<TDB> dbFactory, IConversor<TEntity, TDTO> conversor)
        {
            DBFactory = dbFactory;
            Conversor = conversor;
        }
        public TDTO Create(TDTO dto)
        {
            TEntity entity = Conversor.DTO2Entity(dto);
            using (var ctx = this.DBFactory.GetInstace())
            {
                entity = ctx.Set<TEntity>().Add(entity);
                ctx.SaveChanges();
                if (entity == null)
                {
                    throw new CreateException<TEntity>();
                }
            }
            TDTO result = Conversor.Entity2DTO(entity);
            return result;
        }
        public TDTO Read(Key id)
        {
            TDTO result = default(TDTO);
            using (var ctx = this.DBFactory.GetInstace())
            {
                TEntity entity;
                entity = ctx.Set<TEntity>().Find(id);
                if (entity != null)
                {
                    result = Conversor.Entity2DTO(entity);
                }
            }
            return result;
        }

        public IList<TDTO> List()
        {
            IList<TDTO> result = default(IList<TDTO>);
            using (var ctx = this.DBFactory.GetInstace())
            {
                result = ctx.Set<TEntity>().ToList().ConvertAll(e => Conversor.Entity2DTO(e));
            }
            return result;
        }
        public abstract TDTO Update(TDTO dto);
        public TDTO Update(TDTO dto, Expression<Func<TEntity, bool>> updatePredicate)
        {
            TEntity nuevaEntity = Conversor.DTO2Entity(dto);
            using (var ctx = this.DBFactory.GetInstace())
            {
                TEntity antiguaEntity = ctx.Set<TEntity>().FirstOrDefault(updatePredicate);
                ctx.Entry<TEntity>(antiguaEntity).CurrentValues.SetValues(nuevaEntity);
                ctx.SaveChanges();
                nuevaEntity = ctx.Set<TEntity>().FirstOrDefault(updatePredicate);
                TDTO response = Conversor.Entity2DTO(nuevaEntity);
                return response;
            }
        }
        public TDTO Delete(Key id)
        {
            TDTO dto = default(TDTO);
            using (var ctx = this.DBFactory.GetInstace())
            {
                TEntity deleted = ctx.Set<TEntity>().Find(id);
                ctx.Set<TEntity>().Remove(deleted);
                dto = Conversor.Entity2DTO(deleted);
                ctx.SaveChanges();
            }
            return dto;
        }
    }
}

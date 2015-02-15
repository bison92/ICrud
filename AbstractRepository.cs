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
        public delegate void deleteFunction(ref TEntity entity);
        public delegate void lazyLoad(ref TEntity entity);
        public AbstractRepository(IDBFactory<TDB> dbFactory, IConversor<TEntity, TDTO> conversor)
        {
            DBFactory = dbFactory;
            Conversor = conversor;
        }
        public virtual TDTO Create(TDTO dto)
        {
            TEntity entity = Conversor.DTO2Entity(dto);
            using (var ctx = this.DBFactory.GetInstance())
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
        protected TDTO Read(Expression<Func<TEntity, bool>> findPredicate, lazyLoad lazy = null)
        {
            TDTO result = default(TDTO);
            using (var ctx = this.DBFactory.GetInstance())
            {
                TEntity entity;
                entity = ctx.Set<TEntity>().Where(findPredicate).FirstOrDefault();
                if (entity != null)
                {
                    if (lazy != null)
                    {
                        lazy(ref entity);
                    }
                    result = Conversor.Entity2DTO(entity);
                }
            }
            return result;
        }
        public virtual TDTO Read(Key id)
        {
            TDTO result = default(TDTO);
            using (var ctx = this.DBFactory.GetInstance())
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

        public virtual IList<TDTO> List()
        {
            IList<TDTO> result = default(IList<TDTO>);
            using (var ctx = this.DBFactory.GetInstance())
            {
                result = ctx.Set<TEntity>().ToList().ConvertAll(e => Conversor.Entity2DTO(e));
            }
            return result;
        }
        protected IList<TDTO> List(Expression<Func<TEntity, bool>> findPredicate)
        {
            IList<TDTO> result = default(IList<TDTO>);
            using (var ctx = this.DBFactory.GetInstance())
            {
                result = ctx.Set<TEntity>().Where(findPredicate).ToList().ConvertAll(e => Conversor.Entity2DTO(e));
            }
            return result;
        }
        public abstract TDTO Update(TDTO dto);
        protected TDTO Update(TDTO dto, Expression<Func<TEntity, bool>> findPredicate, lazyLoad lazy = null)
        {
            TEntity nuevaEntity = Conversor.DTO2Entity(dto);
            using (var ctx = this.DBFactory.GetInstance())
            {
                TEntity antiguaEntity = ctx.Set<TEntity>().FirstOrDefault(findPredicate);
                if (lazy != null)
                {
                    lazy(ref antiguaEntity);
                }
                ctx.Entry<TEntity>(antiguaEntity).CurrentValues.SetValues(nuevaEntity);
                ctx.SaveChanges();
                nuevaEntity = ctx.Set<TEntity>().FirstOrDefault(findPredicate);
                TDTO response = Conversor.Entity2DTO(nuevaEntity);
                return response;
            }
        }
        public virtual TDTO Delete(Key id)
        {
            TDTO dto = default(TDTO);
            using (var ctx = this.DBFactory.GetInstance())
            {
                TEntity deleted = ctx.Set<TEntity>().Find(id);
                ctx.Set<TEntity>().Remove(deleted);
                dto = Conversor.Entity2DTO(deleted);
                ctx.SaveChanges();
            }
            return dto;
        }
        protected TDTO Delete(Expression<Func<TEntity, bool>> findPredicate, deleteFunction doDelete, lazyLoad lazy = null)
        {
            TDTO dto = default(TDTO);
            using (var ctx = this.DBFactory.GetInstance())
            {
                TEntity deleted = ctx.Set<TEntity>().Where(findPredicate).FirstOrDefault();
                doDelete(ref deleted);
                if (lazy != null)
                {
                    lazy(ref deleted);
                }
                dto = Conversor.Entity2DTO(deleted);
                ctx.SaveChanges();
            }
            return dto;
        }
    }
}

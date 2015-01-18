using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
namespace AbstractCrudRepository
{
    public abstract class AbstractRepository<K, TEntity, TDTO, TDB> : ICrud<K, TDTO>
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
        public TDTO Read(K id)
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
        public TDTO Delete(K id)
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

﻿using HomeCinema.Data.Infrastructure;
using HomeCinema.Entities;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace HomeCinema.Data.Repositories
{
    public interface IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> All { get; }
        IQueryable<T> GetAll();
        T GetSingle(int id);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Delete(T entity);

        void Edit(T entity);

    }


    /*
    IQueryable represents the query as an expression tree without evaluating it on the server.
    This lets you specify further processing before actually generating SQL.

    In the above case, this means that you can do stuff with the result of calling GetItems(), 
    and have the original query and the extra stuff sent as a single query:

    var recentItems = from item in GetItems()
                      where item.Timestamp > somedate
                      select item;

    foreach (var item in recentItems)
    {
        // Do something with an active recent item.
    }
    Nothing is sent to the server until we try to consume the result in the foreach loop.
    At that point, the LINQ-to-SQL provider assesses the entire expression
    , including the bits generated inside GetItems() and the bits specified after, 
    and emits a single SQL statement that selects all items that are both active and recent.

    To clarify a technicality,
    the IQueryable<T> is an IEnumerable<T>, 
    and its provider computes the final SQL when you try to invoke the GetEnumerator() method on it.
    You can either do this explicitly by calling it, or implicitly by using it in a foreach statement.
    Also, extension methods like ToArray() will do one of these internally, thus producing the same effect.        

    \*/
    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        private HomeCinemaContext dataContext;



        #region Properties

        protected IDbFactory DbFactory { get; private set; }

        protected HomeCinemaContext DbContext => dataContext ?? (dataContext = DbFactory.Init());


        /*protected HomeCinemaContext DbContext
        {
            get { return dataContext ?? (dataContext = DbFactory.Init()); }
        }*/

        public EntityBaseRepository(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
        }

        #endregion

        public virtual IQueryable<T> GetAll()
        {
            return DbContext.Set<T>();
        }

        public virtual IQueryable<T> All => GetAll();

        public virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = DbContext.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);

            }
            return query;
        }




        public virtual T GetSingle(int id)
        {
            return GetAll().FirstOrDefault(x => x.ID == id);
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>().Where(predicate);
        }

        public virtual void Add(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry<T>(entity);
            DbContext.Set<T>().Add(entity);
            dbEntityEntry.State = EntityState.Added;

        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry<T>(entity);
            DbContext.Set<T>().Remove(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public virtual void Edit(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;



        }
    }
}
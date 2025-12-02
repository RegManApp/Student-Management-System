using CourseRegistrationSystem.DAL.DataContext;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.DAL;
using StudentManagementSystem.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace StudentManagementSystem.DAL.Repositories
{
    internal class BaseRepository <T> : IBaseRepository<T> where T : class
    {
        private readonly AppDbContext dbContext;
        private DbSet<T> dbset;
        public BaseRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbset = dbContext.Set<T>();
        }
        public Task AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAllAsQueryable()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
        public IQueryable<TDestination> GetFilteredAndProjected<TDestination>(Expression<Func<T, bool>> filter, Expression<Func<T, TDestination>> projection)
        where TDestination : class
        {
            var query = GetAllAsQueryable();
            if (filter != null)
            {
                query = query.Where(filter); //here i apply filtering on entire table
            }
            //then we project, apply mapping to destination DTO
            return query.Select(projection);

        }
        //public IQueryable<T> GetQueryWithFilter(Expression<Func<T, bool>> filter)
        //{
        //    // Start with the full DbSet
        //    IQueryable<T> query = dbset;

        //    // Apply the filtering expression
        //    if (filter != null)
        //    {
        //        query = query.Where(filter);
        //    }

        //    return query;
        //}
    }
}

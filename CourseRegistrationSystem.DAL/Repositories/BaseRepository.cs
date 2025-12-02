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
        public IQueryable<T> GetFilteredAndProjected()
        {
            throw new NotImplementedException();
        }

    }
}

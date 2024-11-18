using Microsoft.EntityFrameworkCore;
using SisNikosPizza.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SisNikosPizza.Infrastructure.Context;

namespace SisNikosPizza.Repository.Implements
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly SisNikosPizzaBbContext _db;
        internal DbSet<T> dbSet;
        public RepositoryBase(SisNikosPizzaBbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        public async Task<IEnumerable<T>> ObtenerTodosAsync(Expression<Func<T, bool>> filtro = null, Func<IQueryable<T>, IOrderedQueryable<T>> ordenarPor = null, string incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filtro is not null)
                query = query.Where(filtro); // select * from where

            if (incluirPropiedades is not null)
                foreach (var incluirPropiedad in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(incluirPropiedad); // "Category,Cliente"

            if (ordenarPor is not null)
                query = ordenarPor(query);

            if (isTracking)
                query = query.AsNoTracking();

            return await query.ToListAsync();
        }

        public async Task<T> ObtenerPrimeroAsync(Expression<Func<T, bool>> filtro = null, string incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filtro is not null)
                query = query.Where(filtro); // select * from where

            if (incluirPropiedades is not null)
                foreach (var incluirPropiedad in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(incluirPropiedad); // "Category,Cliente"

            if (isTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }
        public async Task AgregarAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public void Eliminar(T entity)
        {
            dbSet.Remove(entity);
        }

        public void EliminarRango(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }

        public async Task<T> ObtenerAsync(int id)
        {
            return await dbSet.FindAsync(id); // select * from where id
        }

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SisNikosPizza.Repository.Interfaces
{
    public interface IRepositoryBase<T> where T : class
    {
        // Listar
        Task<T> ObtenerAsync(int id);
        Task<IEnumerable<T>> ObtenerTodosAsync(
            Expression<Func<T, bool>> filtro = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> ordenarPor = null,
            string incluirPropiedades = null,
            bool isTracking = true
            );

        Task<T> ObtenerPrimeroAsync(
            Expression<Func<T, bool>> filtro = null,
            string incluirPropiedades = null,
            bool isTracking = true
            );

        // Agregar
        Task AgregarAsync(T entity);
        // Eliminar
        void Eliminar(T entity);
        void EliminarRango(IEnumerable<T> entity);
    }
}

using SisNikosPizza.Domain.Models;
using SisNikosPizza.Repository.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisNikosPizza.Repository.Interfaces
{
    public interface IInsumoRepository : IRepositoryBase<Insumo>
    {
        void Actualizar(Insumo insumo);
        Task<List<Insumo>> ObtenerInsumosPorProveedorAsync(int proveedorId);


    }
}

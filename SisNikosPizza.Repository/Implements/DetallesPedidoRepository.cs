using Azure.Core;
using Microsoft.EntityFrameworkCore;
using SisNikosPizza.Domain.Models;
using SisNikosPizza.Infrastructure.Context;
using SisNikosPizza.Repository.Interfaces;
using SisNikosPizza.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisNikosPizza.Repository.Implements
{


    public class DetallesPedidoRepository : RepositoryBase<DetallePedido>, IDetallesPedidoRepository
    {
        private readonly SisNikosPizzaBbContext _db;
        public DetallesPedidoRepository(SisNikosPizzaBbContext db) : base(db)
        {
            _db = db;
        }
    }

}

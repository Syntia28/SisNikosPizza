using Azure.Core;
using Microsoft.EntityFrameworkCore;
using SisNikosPizza.Domain.Models;
using SisNikosPizza.Infrastructure.Context;
using SisNikosPizza.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisNikosPizza.Repository.Implements
{


    public class PedidoRepository : RepositoryBase<Pedido>, IPedidoRepository
    {
        private readonly SisNikosPizzaBbContext _db;
        public PedidoRepository(SisNikosPizzaBbContext db) : base(db)
        {
            _db = db;
        }
  }

}

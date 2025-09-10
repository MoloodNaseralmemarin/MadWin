using MadWin.Core.DTOs.Factors;
using MadWin.Core.Entities.Factors;
using MadWin.Core.Entities.Orders;
using MadWin.Core.Entities.Users;
using MadWin.Core.Interfaces;
using MadWin.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Infrastructure.Repositories
{
    public class FactorDetailRepository : GenericRepository<FactorDetail>, IFactorDetailRepository
    {
        private readonly MadWinDBContext _context;
        private readonly IProductRepository _productRepository;

        public FactorDetailRepository(MadWinDBContext context, IProductRepository productRepository) : base(context)
        {
            _context = context;
            _productRepository = productRepository;
        }

        public async Task<FactorDetail> AddFactorDetailAsync(int factorId,int count, int productId)
        {
            var product = await _productRepository.GetProductInfoByProductId(productId);
            FactorDetail factorDetail=new FactorDetail();

            factorDetail.FactorId=factorId;
            factorDetail.ProductId = productId;
            factorDetail.Quantity= count;
            factorDetail.Price=count * product.Price;
            await AddAsync(factorDetail);
            await _context.SaveChangesAsync();
            return factorDetail;
        }

        public async Task<decimal> FactorSum(int factorId)
        {
            return await _context.Set<FactorDetail>().Where(d=>d.FactorId==factorId).SumAsync(d => d.Price);
        }
        public async Task<bool> IsExistFactorDetailAsync(int factorId)
        {
            return await _context.Set<FactorDetail>().AsNoTracking().AnyAsync(d => d.FactorId == factorId);
        }


        public async Task<FactorDetail> GetFactorByfactorIdAsync(int factorId)
        {
            var factorDetail=await _context.Set<FactorDetail>()
                 .FirstOrDefaultAsync(d => d.FactorId == factorId);
            return factorDetail;
        }
        public async Task<FactorSummaryDto?> GetFactorSummaryByFactorIdAsync(int factorId)
        {
            var factor = await _context.Set<Factor>()
                .Include(f => f.FactorDetails)
                    .ThenInclude(fd => fd.Product)
                .AsNoTracking()
                .Where(f => f.Id == factorId && !f.IsFinaly)
                .Select(fs => new FactorSummaryDto
                {
                    FactorId = fs.Id,
                    FactorDetails = fs.FactorDetails
                        .Where(fd => !fd.IsDelete)   //  شرط روی جزئیات فاکتور
                        .Select(fd => new FactorDetailDto
                        {
                            Id = fd.Id,
                            ProductTitle = fd.Product.Title,
                            Quantity = fd.Quantity,
                            Price = fd.Price
                        }).ToList(),

                    Discount = fs.DisTotal,
                    DeliveryPrice = fs.DeliveryMethodAmount
                })
                .FirstOrDefaultAsync();

            return factor;
        }

        public async Task<List<FactorDetail>> GetAllFactorDetailByFactorIdAsync(int factorId)
        {
            return await _context.FactorDetails
                .Include(od => od.Product)
                .Include(od => od.Factor)
                .ThenInclude(od => od.User)
                .Where(od => od.Factor.Id == factorId && od.Factor.IsFinaly)
                .ToListAsync();
        }

        public async Task<FactorDetail?> GetFactorDetailByProductIdAsync(int factorId, int productId)
        {
            return await _context.FactorDetails
                .FirstOrDefaultAsync(fd => fd.FactorId == factorId && fd.ProductId == productId && !fd.IsDelete);
        }

        public decimal GetSubtotalByFactorId(int factorId)
        {
            return _context.FactorDetails
                .Where(fd => fd.FactorId == factorId && !fd.IsDelete)
                .Sum(fd => (decimal?)fd.Price * fd.Quantity) ?? 0;
        }
    }
}
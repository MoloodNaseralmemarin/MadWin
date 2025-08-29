using MadWin.Core.DTOs.Factors;
using MadWin.Core.Entities.Factors;
using MadWin.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Application.Services
{
    public class FactorDetailService : IFactorDetailService
    {
        private readonly IFactorDetailRepository _factorDetailRepository;
        public FactorDetailService(IFactorDetailRepository factorDetailRepository)
        {
            _factorDetailRepository = factorDetailRepository;
        }

        public async Task<List<FactorDetail>> GetAllFactorDetailByFactorIdAsync(int factorId)
        {
            return await _factorDetailRepository.GetAllFactorDetailByFactorIdAsync(factorId);
        }

        public async Task<FactorSummaryDto> GetFactorSummaryByFactorIdAsync(int factorId)
        {
            return await _factorDetailRepository.GetFactorSummaryByFactorIdAsync(factorId);
        }
    }
}

using MadWin.Core.Entities.CurtainComponents;
using MadWin.Core.Interfaces;

namespace MadWin.Application.Services
{
    public class CurtainComponentDetailService : ICurtainComponentDetailService
    {
        private readonly ICurtainComponentDetailRepository _curtainComponentDetailRepository;
        public CurtainComponentDetailService(ICurtainComponentDetailRepository curtainComponentDetailRepository)
        {
            _curtainComponentDetailRepository = curtainComponentDetailRepository;
        }

        public async Task<CurtainComponentDetail> CreateCurtainComponentDetailInitialAsync(int orderId, int curtainComponentId, decimal unitCost, int count)
        {
            var model = new CurtainComponentDetail()
            {
                Count = count,
                OrderId = orderId,
                CurtainComponentId = curtainComponentId,
                UnitCost= unitCost,
                FinalCost= unitCost * count,
            };
            await _curtainComponentDetailRepository.AddAsync(model);
            await _curtainComponentDetailRepository.SaveChangesAsync();
            return model;

        }

        public async Task<CurtainComponentDetail> CreateICurtainComponentDetailInitialAsync(int orderId,int curtainComponentId,decimal unitCost,int count)
        {

            var calculationDetail = new CurtainComponentDetail
            {
                OrderId = orderId,
                CurtainComponentId = curtainComponentId,
                UnitCost = unitCost,
                Count =count ,
                FinalCost = unitCost * count
            };
             await _curtainComponentDetailRepository.AddAsync(calculationDetail);
            return calculationDetail;
        }
    }
}

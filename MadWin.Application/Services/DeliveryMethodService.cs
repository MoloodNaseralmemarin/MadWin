using MadWin.Application.Services;
using MadWin.Core.Interfaces;
using MadWin.Core.Lookups.DeliveryMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Application.Repositories
{
    public class DeliveryMethodService : IDeliveryMethodService
    {
        private readonly IDeliveryMethodRepository _deliveryMethodRepository;

        public DeliveryMethodService(IDeliveryMethodRepository deliveryMethodRepository)
        {
            _deliveryMethodRepository = deliveryMethodRepository;
        }

        public async Task<decimal> GetDeliveryMethodCostByIdAsync(int deliveryId)
        {
            return await _deliveryMethodRepository.GetDeliveryMethodCostByIdAsync(deliveryId);
        }

        public async Task<IEnumerable<DeliveryMethodInfoLookup>> GetDeliveryMethodInfoAsync()
        {
            return await _deliveryMethodRepository.GetDeliveryMethodInfoAsync();
        }
    }
}

using LabXand.Core;
using LabXand.DomainLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DistributedServices
{
    public interface IApplicationService<TDomainEntity, TDomainDto, TDomainService, TIdentifier>
        where TDomainEntity : DomainEntityBase<TIdentifier>
        where TDomainService : IDomainService<TDomainEntity, TIdentifier>
    {
        TDomainDto Find(Criteria criteria);
        
        Paginated<TDomainDto> FindPage(Criteria criteria, List<SortItem> sortItems, int page, int size);        

        Paginated<TDomainDto> FindPage(IPagedList<TDomainEntity> result);
                
        IList<TDomainDto> FindAll(Criteria criteria, List<SortItem> sortItems);

        int Count(Criteria criteria);

        TDomainDto Save(TDomainDto domainDto);
        
        bool Delete(TDomainDto domainDto);        
    }
}

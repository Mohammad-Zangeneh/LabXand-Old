using LabXand.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DistributedServices.Process
{
    public interface IProcessEnabledUnitOfWork : IUnitOfWork
    {
        IUnitOfWork DataContext { get; }
    }
}

using LabXand.Infrastructure.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Infrastructure.Data.EF.UpdateConfiguration
{
    public class EmptyPropertyUpdaterCustomizer<TRoot> : PropertyUpdaterCustomizerBase<TRoot>
        where TRoot : class
    {
        public override Action<DbContextBase, TRoot, object> OnAfterAddEntity(DbContextBase dbContext, TRoot rootEntity, object entity)
        {
            Action<DbContextBase, TRoot, object> action = (context, troot, t) => { };
            return action;
        }

        public override Action<DbContextBase, TRoot, object> OnAfterEditEntity(DbContextBase dbContext, TRoot rootEntity, object entity)
        {
            Action<DbContextBase, TRoot, object> action = (context, troot, t) => { };
            return action;
        }

        public override Action<DbContextBase, TRoot, object> OnAfterRemoveEntity(DbContextBase dbContext, TRoot rootEntity, object entity)
        {
            Action<DbContextBase, TRoot, object> action = (context, troot, t) => { };
            return action;
        }

        public override Action<DbContextBase, TRoot, object> OnBeforAddEntity(DbContextBase dbContext, TRoot rootEntity, object entity)
        {
            Action<DbContextBase, TRoot, object> action = (context, troot, t) => { };
            return action;
        }

        public override Action<DbContextBase, TRoot, object> OnBeforEditEntity(DbContextBase dbContext, TRoot rootEntity, object entity)
        {
            Action<DbContextBase, TRoot, object> action = (context, troot, t) => { };
            return action;
        }

        public override Action<DbContextBase, TRoot, object> OnBeforRemoveEntity(DbContextBase dbContext, TRoot rootEntity, object entity)
        {
            Action<DbContextBase, TRoot, object> action = (context, troot, t) => { };
            return action;
        }
    }
}

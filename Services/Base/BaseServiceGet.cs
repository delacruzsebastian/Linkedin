using DAL.Constants;
using DAL.Context;
using DAL.Interfaces;
using DAL.Respositories;
using EntityFramework.DynamicFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Base
{
    public abstract class BaseServiceGet<T> where T : class, IKey
    {
        protected BaseContext context;
        protected BaseRepository<T> repository;
        protected string fixGetIncludes = string.Empty;

        public BaseServiceGet(BaseContext context)
        {
            this.context = context;
            Init();
        }

        private void Init()
        {
            this.repository = new BaseRepository<T>(this.context);
        }

        public virtual ServiceResult<List<T>> Get(Expression<Func<T, bool>> filter = null, string includes = "", bool includeDeletes = false)
        {
            var result = new ServiceResult<List<T>>();
            includes = string.Format("{0}{1}{2}", this.fixGetIncludes, string.IsNullOrEmpty(this.fixGetIncludes) ? "" : ",", includes);

            try
            {
                if (includeDeletes)
                    this.context.DisableFilter(Constants.FILTER_SOFT_DELETED);

                result.ResultData = this.repository.Get(filter, includeProperties: includes).ToList();

                if (result.ResultData == null || result.ResultData.Count() == 0)
                    result.Result = Results.NoData;
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message);
            }
            finally
            {
                if (includeDeletes)
                    this.context.EnableFilter(Constants.FILTER_SOFT_DELETED);
            }

            return result;
        }

        public virtual ServiceResult<List<T>> GetAll(string includes = "", bool includeDeletes = false)
        {
            return this.Get(null, includes, includeDeletes);
        }

        public virtual ServiceResult<T> Get(Guid id, string includes = "", bool includeDeletes = false)
        {
            var result = new ServiceResult<T>();
            includes = string.Format("{0}{1}{2}", this.fixGetIncludes, string.IsNullOrEmpty(this.fixGetIncludes) ? "" : ",", includes);

            try
            {
                if (includeDeletes)
                    this.context.DisableFilter(Constants.FILTER_SOFT_DELETED);

                result.ResultData = this.repository.Get(w => w.Id == id, includeProperties: includes).FirstOrDefault();

                if (result.ResultData == null)
                    result.Result = Results.NoData;
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message);
            }
            finally
            {
                if (includeDeletes)
                    this.context.EnableFilter(Constants.FILTER_SOFT_DELETED);
            }

            return result;
        }

    }
}
using DAL.Context;
using DAL.Entities;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CountryService : BaseServiceGet<Country>
    {
        public CountryService(BaseContext context) : base(context)
        {
        }

        public ServiceResult<List<Country>> Get(string searchText, string includes = "", bool includeDeletes = false)
        {
            Expression<Func<Country, bool>> expression = w => string.IsNullOrEmpty(searchText) || w.Denomination.ToUpper().Contains(searchText.ToUpper());

            return this.Get(expression, includes, includeDeletes);
        }

        public string GetDescription(Guid id)
        {
            string ret = "";

            try
            {
                var res = this.Get(id);

                if (res.Result == Results.NoData)
                {
                    return "";
                }

                if (res.Result != Results.Success)
                {
                    throw new Exception(res.ResultErrorMessage);
                }

                ret = res.ResultData.Denomination;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return ret;
        }

    }
}


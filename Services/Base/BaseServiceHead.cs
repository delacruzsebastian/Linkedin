namespace Services.Base
{
    using Newtonsoft.Json;
    using Services.CustomClass;
    using Services.Enums;
    using DAL.Respositories;
    using DAL.Context;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DAL.Interfaces;
    using System.Linq.Expressions;
    using EntityFramework.DynamicFilters;
    using DAL.Constants;
    using System.Data.Entity;

    public abstract class BaseServiceHead<T, TAudit> where T : class, IHeadData, new()
                                                        where TAudit : class
    {
        protected BaseContext context;
        protected BaseRepository<T> repository;
        protected BaseRepository<TAudit> repositoryAudit;
        protected string entityName = "";
        
        public BaseServiceHead(BaseContext context, string entityName)
        {
            this.context = context;
            this.entityName = entityName;
            Init();
        }

        private void Init()
        {
            this.repository         = new BaseRepository<T>(this.context);
            this.repositoryAudit    = new BaseRepository<TAudit>(this.context);
        }

        public virtual ServiceResult<List<T>> Get(Expression<Func<T, bool>> filter = null, string includes = "", bool includeDeletes = false)
        {
            var result = new ServiceResult<List<T>>();

            try
            {
                if (includeDeletes)
                    this.context.DisableFilter(DAL.Constants.Constants.FILTER_SOFT_DELETED);

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

        public ServiceResult<List<IdDescription>> Get(string searchText, bool includeDeletes = false)
        {
            var result = new ServiceResult<List<IdDescription>>();
            try
            {
                if (includeDeletes)
                    this.context.DisableFilter(Constants.FILTER_SOFT_DELETED);

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    result.ResultData = this.repository.Get(w => w.Denomination.ToUpper().Contains(searchText.ToUpper()))
                        .Select(s => new IdDescription() { Id = s.Id, Description = s.Denomination }).ToList();
                }
                else
                    result.ResultData = null;

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

        public virtual ServiceResult<Boolean> Add(T entity)
        {
            var result = new ServiceResult<Boolean>(true);

            try
            {
                // Validaciones de negocio
                if (entity == null)
                {
                    throw new Exception(string.Format("The {0} is null or empty", this.entityName));
                }

                // Validaciones Extras
                var valRes = this.Validate(entity, ServiceActionType.Add);
                if (valRes.Result != Results.Success)
                {
                    return valRes;
                }

                // Seteo Atributos Adicionales
                var atrRes = this.SetRelatedEntities(entity, ServiceActionType.Add);
                if (atrRes.Result != Results.Success)
                {
                    return atrRes;
                }

                var updateDate      = DateTime.Now;
                entity.UpdateDate   = updateDate;
                entity.AddDate      = updateDate;

                this.repository.Add(entity);

                // Creo la auditoria
                var resAudit = this.GenerateVersioned(entity, updateDate);
                if (resAudit.Result != Results.Success)
                {
                    throw new Exception(resAudit.ResultErrorMessage);
                }

            }
            catch (Exception ex)
            {
                result.ResultData = false;
                result.SetError(ex.Message);
            }

            return result;
        }

        public virtual ServiceResult<Boolean> Update(T entity, bool forceUpdate = false)
        {
            var result = new ServiceResult<Boolean>(true);
            var updateDate = DateTime.Now;

            try
            {
                // Validaciones de negocio
                if (entity == null)
                {
                    throw new Exception(string.Format("The {0} is null or empty", this.entityName));
                }

                // Validaciones Extras
                var valRes = this.Validate(entity, ServiceActionType.Update);
                if (valRes.Result != Results.Success)
                {
                    return valRes;
                }

                // Seteo Atributos Adicionales
                var atrRes = this.SetRelatedEntities(entity, ServiceActionType.Update);
                if (atrRes.Result != Results.Success)
                {
                    return atrRes;
                }

                // Busco el registro a modificar para validar concurrencia
                var resEntityDB = this.Get(entity.Id, includeDeletes: true);

                if (resEntityDB.Result != Results.Success)
                {
                    throw new Exception(string.Format("Error al actualizar {0}", this.entityName));
                }

                var entityDB = resEntityDB.ResultData;

                if (!this.ValidateConcurrence(entity, entityDB))
                {
                    throw new Exception(string.Format("Existen modificaciones recientes en el {0}.", this.entityName));
                }

                this.UpdateRecord(entity, entityDB);

                var entityStatus = context.Entry(entityDB).State;

                // Creo la auditoria
                if (forceUpdate || entityStatus != EntityState.Unchanged)
                {
                    entityDB.UpdateDate = updateDate;

                    var resAudit = GenerateVersioned(entityDB, updateDate);
                    if (resAudit.Result != Results.Success)
                    {
                        throw new Exception(resAudit.ResultErrorMessage);
                    }

                    // Valido cambios y agreo auditoria en entidades relacionadas
                    //this.CreateRelatedEntitiesAudits(entityDB, updateDate);
                    this.GenerateRelatedEntitiesAudits(entityDB, updateDate);
                }
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message);
                result.ResultData = false;
            }


            return result;
        }

        public virtual ServiceResult<Boolean> Delete(Guid id)
        {
            var result = new ServiceResult<Boolean>(true);

            try
            {
                var res = this.Get(id);

                if (res.Result == Results.NoData)
                {
                    result.Result = Results.NoData;
                    return result;
                }

                if (res.Result != Results.Success)
                {
                    throw new Exception(res.ResultErrorMessage);
                }

                var resDelete = this.Delete(res.ResultData, DateTime.Now);

                if (resDelete.Result != Results.Success)
                {
                    throw new Exception(resDelete.ResultErrorMessage);
                }
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message);
            }

            return result;
        }

        internal virtual ServiceResult<Boolean> Delete(T entity, DateTime updateDate)
        {
            var result = new ServiceResult<Boolean>(true);

            try
            {
                // Seteo Atributos Adicionales
                var atrRes = this.SetRelatedEntities(entity, ServiceActionType.Delete);
                if (atrRes.Result != Results.Success)
                {
                    return atrRes;
                }

                context.Entry(entity).State = EntityState.Modified;
                entity.UpdateDate = updateDate;
                entity.DeleteDate = updateDate;

                // Creo la auditoria
                var resAudit = GenerateVersioned(entity, updateDate);
                if (resAudit.Result != Results.Success)
                {
                    throw new Exception(resAudit.ResultErrorMessage);
                }

                // Elimino las relaciones 
                this.DeleteRelatedEntities(entity, updateDate);

            }
            catch (Exception ex)
            {
                result.SetError(ex.Message);
            }

            return result;
        }

        public virtual ServiceResult<T> GetClone(Guid idToClone, string includes = "")
        {

            var cloneEntity = new T();
            var result = new ServiceResult<T>(cloneEntity);

            try
            {
                var res = this.Get(idToClone, includes);

                if (res.Result == Results.NoData)
                {
                    result.Result = Results.NoData;
                    return result;
                }

                if (res.Result != Results.Success)
                {
                    throw new Exception(res.ResultErrorMessage);
                }

                cloneEntity = res.ResultData;

                this.context.Entry(cloneEntity).State = EntityState.Detached;

                cloneEntity.Id = new Guid();

                cloneEntity.Denomination = string.Format("[{0}] {1}", Resources.Resources.Copy, cloneEntity.Denomination);

                // Seteo Atributos Adicionales
                var atrRes = this.SetExtraPropertiesOnClone(cloneEntity);
                if (atrRes.Result != Results.Success)
                {
                    //return atrRes;
                }

                result.ResultData = cloneEntity;
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message);
            }

            return result;
        }

        public virtual ServiceResult<Boolean> IsInUse(Guid id, Guid? excludeEntityId = null)
        {
            var ret = new ServiceResult<bool>(true);

            try
            {
                if (Helper.GuidIsNullOrEmpty(excludeEntityId))
                {
                    excludeEntityId = Guid.Empty;
                }

                var res = this.Get(id);

                if (res.Result == Results.NoData)
                {
                    ret.Result = Results.NoData;
                    return ret;
                }

                if (res.Result != Results.Success)
                {
                    throw new Exception(res.ResultErrorMessage);
                }

                return this.IsInUseExtension(res.ResultData, (Guid)excludeEntityId);

            }
            catch (Exception ex)
            {
                ret.SetError(ex.Message, false);
            }

            return ret;
        }

        internal virtual ServiceResult<Boolean> IsInUseExtension(T entity, Guid excludeEntityId) { throw new Exception("Function not implemented"); }

        protected virtual TAudit Transform2Audit(T entityToAudit)
        {
            return JsonConvert.DeserializeObject<TAudit>(JsonConvert.SerializeObject(entityToAudit));
        }

        protected virtual void CreateRelatedEntitiesAudits(T entity, DateTime updateDate) { }
        protected virtual ServiceResult<Boolean> GenerateRelatedEntitiesAudits(T entity, DateTime updateDate) { return new ServiceResult<Boolean>(true); }

        protected virtual void DeleteRelatedEntities(T entity, DateTime updateDate) { }

        protected virtual ServiceResult<Boolean> Validate(T entity, ServiceActionType actionType) { return new ServiceResult<Boolean>(true); }

        protected virtual ServiceResult<Boolean> SetRelatedEntities(T entity, ServiceActionType actionType) { return new ServiceResult<Boolean>(true); }

        protected virtual ServiceResult<Boolean> SetExtraPropertiesOnClone(T entity) { return new ServiceResult<Boolean>(true); }

        public abstract ServiceResult<T> GetNew();

        internal abstract ServiceResult<Boolean> GenerateVersioned(T entity, DateTime updateDate);

        protected abstract void UpdateRecord(T entity, T entityTarget);

        protected abstract bool ValidateConcurrence(T entity, T entityBD);


    }
}

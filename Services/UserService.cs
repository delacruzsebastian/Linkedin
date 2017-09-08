using DAL.Context;
using DAL.Entities;
using Services.Base;
using Services.CustomClass;
using Services.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : BaseServiceHead<User, UserAudit>
    {
        public UserService(BaseContext context) : base(context, Resources.Resources.User)
        {
        }

        public override ServiceResult<User> GetNew()
        {
            var result = new ServiceResult<User>();

            try
            {
                result.ResultData = new User();
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message);
            }

            return result;
        }

        internal override ServiceResult<Boolean> GenerateVersioned(User entity, DateTime updateDate)
        {
            var result = new ServiceResult<Boolean>(true);

            try
            {
                // Genero la version de auditoria para el usuario
                UserAudit userAudit     = Transform2Audit(entity);
                userAudit.AuditDate     = updateDate;
                userAudit.AuditUserId   = new Guid();

                entity.UserAudits.Add(userAudit);
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message);
            }

            return result;
        }

        protected override void UpdateRecord(User entity, User entityTarget)
        {
            entityTarget.Denomination   = entity.Denomination;
            entityTarget.Firstname      = entity.Firstname;
            entityTarget.Surname        = entity.Surname;
            entityTarget.Available      = entity.Available;
            entityTarget.BirthDate      = entity.BirthDate;
            entityTarget.CountryId      = entity.CountryId;
            entityTarget.Email          = entity.Email;
            entityTarget.Password       = entity.Password;
            entityTarget.Sex            = entity.Sex;
            entityTarget.Username       = entity.Username;
        }

        protected override bool ValidateConcurrence(User entity, User entityBD)
        {
            var dbUpdateDateTime = entityBD.UpdateDate.AddTicks(-(entityBD.UpdateDate.Ticks % TimeSpan.TicksPerSecond));
            if (dbUpdateDateTime > entity.UpdateDate)
                return false;

            return true;
        }

        protected override ServiceResult<Boolean> Validate(User entity, ServiceActionType actionType)
        {
            var ret = new ServiceResult<Boolean>(true);

            try
            {
                // Validacion entidades relacionadas
            }
            catch (Exception ex)
            {
                ret.SetError(string.Format("Error: {0}", ex.Message));
            }

            return ret;
        }

        protected override ServiceResult<Boolean> SetRelatedEntities(User entity, ServiceActionType actionType)
        {
            // Carga desde BD de las entidades relacionadas
            var ret = new ServiceResult<Boolean>(true);

            try
            {
                // Carga desde BD de las entidades relacionadas

                //if (!Helper.GuidIsNullOrEmpty(entity.))
                //{
                //    var service = new CurrencyService(this.context);

                //    var retAux = service.Get((Guid)entity.CurrencyId);

                //    if (retAux.Result != Results.Success)
                //    {
                //        ret.SetError(string.Format("Error to find CurrencyId: {0}", entity.CurrencyId), false);
                //        return ret;
                //    }

                //    entity.Currency = retAux.ResultData;
                //}

                //if (!Helper.GuidIsNullOrEmpty(entity.EntityTypeId))
                //{
                //    var service = new EntityTypeService(this.context);

                //    var retAux = service.Get((Guid)entity.EntityTypeId);

                //    if (retAux.Result != Results.Success)
                //    {
                //        ret.SetError(string.Format("Error to find EntityTypeId: {0}", entity.EntityTypeId), false);
                //        return ret;
                //    }

                //    entity.EntityType = retAux.ResultData;
                //}

            }
            catch (Exception ex)
            {
                ret.SetError(string.Format("Error: {0}", ex.Message), false);
            }

            return ret;
        }

        protected override void CreateRelatedEntitiesAudits(User entity, DateTime updateDate)
        {
            try
            {
                // Actualizo las auditorias de los proyectos relacionados
                if (entity.OwnedProjects != null && entity.OwnedProjects.Count > 0)
                {
                    var projectService  = new ProjectService(this.context);

                    foreach (var projectDB in entity.OwnedProjects)
                    {
                        // Valido cambios y agreo auditoria
                        var resProjectAudit = projectService.GenerateVersioned(projectDB, updateDate);

                        if (resProjectAudit.Result != Results.Success)
                        {
                            throw new Exception(resProjectAudit.ResultErrorMessage);
                        }
                    }
                }

                if(entity.ProjectSubscriptions != null && entity.ProjectSubscriptions.Count > 0)
                {
                    foreach (var projectDB in entity.ProjectSubscriptions)
                    {
                        var projectUserService  = new ProjectUserService(this.context);
                        // Valido cambios y agreo auditoria
                        var resProjectUserAudit = projectUserService.GenerateVersioned(projectDB, updateDate);

                        if (resProjectUserAudit.Result != Results.Success)
                        {
                            throw new Exception(resProjectUserAudit.ResultErrorMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        protected override void DeleteRelatedEntities(User entity, DateTime updateDate)
        {
            var ret = new ServiceResult<Boolean>(true);

            try
            {
                // Elimino los Proyectos creados.
                if (entity.OwnedProjects != null && entity.OwnedProjects.Count > 0)
                {
                    var projectService = new ProjectService(this.context);

                    foreach (var project in entity.OwnedProjects.ToList())
                    {
                        var res = projectService.Delete(project.Id);

                        if (res.Result != Results.Success)
                        {
                            throw new Exception(res.ResultErrorMessage);
                        }
                    }
                }

                // Elimino las relaciones con Proyectos.
                if (entity.ProjectSubscriptions != null && entity.ProjectSubscriptions.Count > 0)
                {
                    var projectUserService = new ProjectUserService(this.context);

                    foreach (var projectUser in entity.ProjectSubscriptions.ToList())
                    {
                        var res = projectUserService.Delete(projectUser.Id);

                        if (res.Result != Results.Success)
                        {
                            throw new Exception(res.ResultErrorMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ret.SetError(string.Format("Error: {0}", ex.Message), false);
            }

            //return ret;
        }
    }
}

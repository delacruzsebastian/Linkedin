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
    public class ProjectUserService : BaseServiceHead<ProjectUser, ProjectUserAudit>
    {
        public ProjectUserService(BaseContext context) : base(context, Resources.Resources.User)
        {
        }

        public override ServiceResult<ProjectUser> GetNew()
        {
            var result = new ServiceResult<ProjectUser>();

            try
            {
                result.ResultData = new ProjectUser();
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message);
            }

            return result;
        }

        internal override ServiceResult<Boolean> GenerateVersioned(ProjectUser entity, DateTime updateDate)
        {
            var result = new ServiceResult<Boolean>(true);

            try
            {
                // Genero la version de auditoria para el usuario
                ProjectUserAudit projectUserAudit   = Transform2Audit(entity);
                projectUserAudit.AuditDate          = updateDate;
                projectUserAudit.AuditUserId        = new Guid();

                entity.ProjectUserAudits.Add(projectUserAudit);
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message);
            }

            return result;
        }

        protected override void UpdateRecord(ProjectUser entity, ProjectUser entityTarget)
        {
            //entityTarget.Denomination = entity.Denomination;
            //entityTarget.Firstname = entity.Firstname;
            //entityTarget.Surname = entity.Surname;
            //entityTarget.Available = entity.Available;
            //entityTarget.BirthDate = entity.BirthDate;
            //entityTarget.CountryId = entity.CountryId;
            //entityTarget.Email = entity.Email;
            //entityTarget.Password = entity.Password;
            //entityTarget.Sex = entity.Sex;
            //entityTarget.Username = entity.Username;
        }

        protected override bool ValidateConcurrence(ProjectUser entity, ProjectUser entityBD)
        {
            var dbUpdateDateTime = entityBD.UpdateDate.AddTicks(-(entityBD.UpdateDate.Ticks % TimeSpan.TicksPerSecond));
            if (dbUpdateDateTime > entity.UpdateDate)
                return false;

            return true;
        }

        protected override ServiceResult<Boolean> Validate(ProjectUser entity, ServiceActionType actionType)
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

        protected override ServiceResult<Boolean> SetRelatedEntities(ProjectUser entity, ServiceActionType actionType)
        {
            // Carga desde BD de las entidades relacionadas
            var ret = new ServiceResult<Boolean>(true);

            try
            {
                // Carga desde BD de las entidades relacionadas

                if (!Helper.GuidIsNullOrEmpty(entity.UserId))
                {
                    var service = new UserService(this.context);

                    var retAux = service.Get((Guid)entity.UserId);

                    if (retAux.Result != Results.Success)
                    {
                        ret.SetError(string.Format("Error to find UserId: {0}", entity.UserId), false);
                        return ret;
                    }

                    entity.User = retAux.ResultData;
                }

                if (!Helper.GuidIsNullOrEmpty(entity.ProjectId))
                {
                    var service = new ProjectService(this.context);

                    var retAux = service.Get((Guid)entity.ProjectId);

                    if (retAux.Result != Results.Success)
                    {
                        ret.SetError(string.Format("Error to find ProjectId: {0}", entity.ProjectId), false);
                        return ret;
                    }

                    entity.Project = retAux.ResultData;
                }

            }
            catch (Exception ex)
            {
                ret.SetError(string.Format("Error: {0}", ex.Message), false);
            }

            return ret;
        }

        protected override void CreateRelatedEntitiesAudits(ProjectUser entity, DateTime updateDate)
        {
            //try
            //{
            //    // Actualizo las auditorias de los proyectos relacionados
            //    if (entity.OwnedProjects != null && entity.OwnedProjects.Count > 0)
            //    {
            //        var projectService = new ProjectService(this.context);

            //        foreach (var projectDB in entity.OwnedProjects)
            //        {
            //            // Valido cambios y agreo auditoria
            //            var resProjectAudit = projectService.GenerateVersioned(projectDB, updateDate);

            //            if (resProjectAudit.Result != Results.Success)
            //            {
            //                throw new Exception(resProjectAudit.ResultErrorMessage);
            //            }
            //        }
            //    }

            //    if (entity.ProjectSubscriptions != null && entity.ProjectSubscriptions.Count > 0)
            //    {
            //        foreach (var projectDB in entity.ProjectSubscriptions)
            //        {
            //            var projectUserService = new ProjectUserService(this.context);
            //            // Valido cambios y agreo auditoria
            //            var resProjectUserAudit = projectUserService.GenerateVersioned(projectDB, updateDate);

            //            if (resProjectUserAudit.Result != Results.Success)
            //            {
            //                throw new Exception(resProjectUserAudit.ResultErrorMessage);
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }

        protected override void DeleteRelatedEntities(ProjectUser entity, DateTime updateDate)
        {
            var ret = new ServiceResult<Boolean>(true);

            try
            {
            }
            catch (Exception ex)
            {
                ret.SetError(string.Format("Error: {0}", ex.Message), false);
            }

            //return ret;
        }
    }
}

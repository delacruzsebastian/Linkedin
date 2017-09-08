using DAL.Context;
using DAL.Entities;
using Services.Base;
using Services.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProjectService : BaseServiceHead<Project, ProjectAudit>
    {
        public ProjectService(BaseContext context) : base(context, Resources.Resources.User)
        {
        }

        public override ServiceResult<Project> GetNew()
        {
            var result = new ServiceResult<Project>();

            try
            {
                result.ResultData = new Project();
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message);
            }

            return result;
        }

        internal override ServiceResult<Boolean> GenerateVersioned(Project entity, DateTime updateDate)
        {
            var result = new ServiceResult<Boolean>(true);

            try
            {
                // Genero la version de auditoria para el usuario
                ProjectAudit projectAudit   = Transform2Audit(entity);
                projectAudit.AuditDate      = updateDate;
                projectAudit.AuditUserId    = Guid.Empty;

                entity.ProjectAudits.Add(projectAudit);
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message);
            }

            return result;
        }

        protected override void UpdateRecord(Project entity, Project entityTarget)
        {
            entityTarget.Denomination = entity.Denomination;
        }

        protected override bool ValidateConcurrence(Project entity, Project entityBD)
        {
            var dbUpdateDateTime = entityBD.UpdateDate.AddTicks(-(entityBD.UpdateDate.Ticks % TimeSpan.TicksPerSecond));
            if (dbUpdateDateTime > entity.UpdateDate)
                return false;

            return true;
        }

        protected override ServiceResult<Boolean> Validate(Project entity, ServiceActionType actionType)
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

        protected override ServiceResult<Boolean> SetRelatedEntities(Project entity, ServiceActionType actionType)
        {
            var ret = new ServiceResult<Boolean>(true);

            try
            {
                // Carga desde BD de las entidades relacionadas
            }
            catch (Exception ex)
            {
                ret.SetError(string.Format("Error: {0}", ex.Message));
            }

            return ret;
        }

        protected override void CreateRelatedEntitiesAudits(Project entity, DateTime updateDate) { }

        protected override void DeleteRelatedEntities(Project entity, DateTime updateDate) {
            var ret = new ServiceResult<Boolean>(true);

            try
            {
                // Elimino las subscripciones.
                if (entity.ProjectUsers != null && entity.ProjectUsers.Count > 0)
                {
                    var projectUserService = new ProjectUserService(this.context);

                    foreach (var projectUser in entity.ProjectUsers.ToList())
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

        }
    }
}


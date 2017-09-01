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

        protected override void CreateRelatedEntitiesAudits(User entity, DateTime updateDate) { }

        protected override void DeleteRelatedEntities(User entity, DateTime updateDate) { }
    }
}

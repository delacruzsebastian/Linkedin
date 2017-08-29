using DAL.Context;
using DAL.Entities;
using Services.Base;
using Services.Enums;
using System;

namespace Services
{
    public class PersonService : BaseServiceHead<Person, PersonAudit>
    {
        public PersonService(BaseContext context) : base(context, Resources.Resources.Person)
        {
        }

        public override ServiceResult<Person> GetNew()
        {
            var result = new ServiceResult<Person>();

            try
            {
                result.ResultData = new Person();
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message);
            }

            return result;
        }

        internal override ServiceResult<Boolean> GenerateVersioned(Person entity, DateTime updateDate)
        {
            var result = new ServiceResult<Boolean>(true);

            try
            {
                // Genero la version de auditoria para persona                
                PersonAudit personAudit = Transform2Audit(entity);
                personAudit.AuditDate   = updateDate;
                personAudit.UserId      = new Guid();

                entity.PersonAudits.Add(personAudit);
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message);
            }

            return result;
        }

        protected override void UpdateRecord(Person entity, Person entityTarget)
        {
            entityTarget.Denomination   = entity.Denomination;
            entityTarget.Firstname      = entity.Firstname;
            entityTarget.Surname        = entity.Surname;
        }

        protected override bool ValidateConcurrence(Person entity, Person entityBD)
        {
            var dbUpdateDateTime = entityBD.UpdateDate.AddTicks(-(entityBD.UpdateDate.Ticks % TimeSpan.TicksPerSecond));
            if (dbUpdateDateTime > entity.UpdateDate)
                return false;

            return true;
        }

        protected override ServiceResult<Boolean> Validate(Person entity, ServiceActionType actionType)
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

        protected override ServiceResult<Boolean> SetRelatedEntities(Person entity, ServiceActionType actionType)
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

        protected override void CreateRelatedEntitiesAudits(Person entity, DateTime updateDate) { }

        protected override void DeleteRelatedEntities(Person entity, DateTime updateDate) { }
    }
}

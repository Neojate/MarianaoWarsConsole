using MarianaoWars.Models;
using MarianaoWarsConsole.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarianaoWarsConsole.Logic
{
    public class Service
    {
        private MarianaoWarsContext dbContext = new MarianaoWarsContext();

        public Service()
        {

        }

        #region GETTERS
        public List<Institute> GetOpenInstitutes()
        {
            return dbContext.Institute
                .Where(institute => !institute.IsClosed)
                .ToList();
        }

        public List<Enrollment> GetEnrollments(int instituteId)
        {
            return dbContext.Enrollment
                .Where(enrolment => enrolment.InstituteId == instituteId)
                .ToList();
        }

        public Computer GetComputer(int computerId)
        {
            return dbContext.Computer
                .Where(computer => computer.Id == computerId)
                .Include(computer => computer.Resource)
                .Include(computer => computer.Software)
                .Include(computer => computer.Talent)
                .Include(computer => computer.Script)
                .FirstOrDefault();
        }

        public List<Computer> GetComputers(int enrollmentId)
        {
            return dbContext.Computer
                .Where(computer => computer.EnrollmentId == enrollmentId)
                .Include(computer => computer.Resource)
                .Include(computer => computer.Software)
                .Include(computer => computer.Talent)
                .Include(computer => computer.Script)
                .ToList();
        }

        public List<SystemResource> GetSystemResources()
        {
            return dbContext.SystemResource.ToList();
        }

        public List<SystemScript> GetSystemScripts()
        {
            return dbContext.SystemScript.ToList();
        }

        public List<SystemSoftware> GetSystemSoftware()
        {
            return dbContext.SystemSoftware.ToList();
        }

        public List<SystemTalent> GetSystemTalents()
        {
            return dbContext.SystemTalent.ToList();
        }

        public List<BuildOrder> GetBuildOrder(int computerId)
        {
            return dbContext.BuildOrder
                .Where(order => order.ComputerId == computerId)
                .ToList();
        }

        public List<HackOrder> GetHackOrders(int computerId)
        {
            return dbContext.HackOrder
                .Where(order => order.From == computerId)
                .ToList();
        }
        #endregion


        #region INSERT
        public void CreateMessage(Message message)
        {
            dbContext.Message.Add(message);
            dbContext.SaveChanges();
        }

        public Computer SaveComputer(Computer computer)
        {
            dbContext.Computer.Add(computer);
            dbContext.SaveChanges();
            return computer;
        }

        public Enrollment SaveEnrollment(Enrollment enrollment)
        {
            dbContext.Enrollment.Add(enrollment);
            dbContext.SaveChanges();
            return enrollment;
        }

        public Resource SaveResource(Resource resource)
        {
            dbContext.Resource.Add(resource);
            dbContext.SaveChanges();
            return resource;
        }

        public Script SaveScript(Script script)
        {
            dbContext.Script.Add(script);
            dbContext.SaveChanges();
            return script;
        }

        public Software SaveSoftware(Software software)
        {
            dbContext.Software.Add(software);
            dbContext.SaveChanges();
            return software;
        }

        public Talent SaveTalent(Talent talent)
        {
            dbContext.Talent.Add(talent);
            dbContext.SaveChanges();
            return talent;
        }
        #endregion



        #region UPDATES
        public void UpdateComputer(Computer computer)
        {
            dbContext.Update(computer);
            dbContext.SaveChanges();
        }

        public void UpdateHackOrder(HackOrder hackOrder)
        {
            dbContext.Update(hackOrder);
            dbContext.SaveChanges();
        }
        #endregion



        #region DELETE
        public void DeleteBuildOrder(int buildOrderId)
        {
            BuildOrder buildOrder = dbContext.BuildOrder.Find(buildOrderId);
            dbContext.BuildOrder.Remove(buildOrder);
            dbContext.SaveChanges();
        }

        public void DeleteHackOrder(HackOrder hackOrder)
        {
            dbContext.HackOrder.Remove(hackOrder);
            dbContext.SaveChanges();
        }
        #endregion

    }
}

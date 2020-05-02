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

        public List<Computer> GetComputers(int enrollmentId)
        {
            return dbContext.Computer
                .Where(computer => computer.EnrollmentId == enrollmentId)
                .Include(computer => computer.Resource)
                .Include(computer => computer.Software)
                .Include(computer => computer.Talent)
                .Include(computer => computer.AttackScript)
                .Include(computer => computer.DefenseScript)
                .ToList();
        }

        public List<SystemResource> GetSystemResources()
        {
            return dbContext.SystemResource.ToList();
        }

        public List<SystemSoftware> GetSystemSoftware()
        {
            return dbContext.SystemSoftware.ToList();
        }

        public List<BuildOrder> GetBuildOrder(int computerId)
        {
            return dbContext.BuildOrder
                .Where(order => order.ComputerId == computerId)
                .ToList();
        }
        #endregion



        #region UPDATES
        public void UpdateComputer(Computer computer)
        {
            dbContext.Update(computer);
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
        #endregion

    }
}

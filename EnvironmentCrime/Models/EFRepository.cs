using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata.Ecma335;

namespace EnvironmentCrime.Models
{
    public class EFRepository : IRepository
    {

        //Class that holds the database context:
        private readonly ApplicationDbContext context;

        //Creates a hosting object to enable file uploading:
        private IWebHostEnvironment environment;

        //Gets the current Http Context:
        private IHttpContextAccessor contextAcc;

        //Constructor for repository class:
        public EFRepository(ApplicationDbContext context, IWebHostEnvironment environment, IHttpContextAccessor contextAcc)
        {
            this.context = context;
            this.environment = environment;
            this.contextAcc = contextAcc;
        }

        //Creates queryable collections of POCO classes:
        public IQueryable<Department> Departments => context.Departments;
        public IQueryable<Employee> Employees => context.Employees;
        public IQueryable<Errand> Errands => context.Errands.Include(e => e.Samples).Include(e => e.Pictures);
        public IQueryable<ErrandStatus> ErrandStatuses => context.ErrandStatuses;
        public IQueryable<Picture> Pictures => context.Pictures;
        public IQueryable<Sample> Samples => context.Samples;
        public IQueryable<Sequence> Sequences => context.Sequences;

        //Method returning an iterable collection of the helper class:
        public IQueryable<MyErrand> ShowErrandListCoordinator()
        {
            var errandList = from err in Errands
                             join stat in ErrandStatuses
                             on err.StatusId equals stat.StatusId
                             join dep in Departments
                             on err.DepartmentId equals dep.DepartmentId
                             into departmentErrand
                             from deptE in departmentErrand.DefaultIfEmpty()
                             join em in Employees
                             on err.EmployeeId equals em.EmployeeId
                             into employeeErrand
                             from empE in employeeErrand.DefaultIfEmpty()
                             orderby err.RefNumber
                             ascending

                             select new MyErrand
                             {
                                 DateOfObservation = err.DateOfObservation,
                                 ErrandId = err.ErrandId,
                                 RefNumber = err.RefNumber,
                                 TypeOfCrime = err.TypeOfCrime,
                                 StatusName = stat.StatusName,
                                 DepartmentName = (err.DepartmentId == null ? "Ej tillsatt" : deptE.DepartmentName),
                                 EmployeeName = (err.EmployeeId == null ? "Ej tillsatt" : empE.EmployeeName)
                             };

            return errandList;
        }

        //Method returning an iterable collection of the helper class:
        public IQueryable<MyErrand> ShowErrandListManager(string userName)
        {
            string depId = GetManagerDept(userName);

            var errandList = from err in Errands
                             join stat in ErrandStatuses
                             on err.StatusId equals stat.StatusId
                             join dep in Departments
                             on err.DepartmentId equals dep.DepartmentId
                             into departmentErrand
                             from deptE in departmentErrand
                             where deptE.DepartmentId == depId
                             join em in Employees
                             on err.EmployeeId equals em.EmployeeId
                             into employeeErrand
                             from empE in employeeErrand.DefaultIfEmpty()
                             orderby err.RefNumber
                             ascending

                             select new MyErrand
                             {
                                 DateOfObservation = err.DateOfObservation,
                                 ErrandId = err.ErrandId,
                                 RefNumber = err.RefNumber,
                                 TypeOfCrime = err.TypeOfCrime,
                                 StatusName = stat.StatusName,
                                 DepartmentName = (err.DepartmentId == null ? "Ej tillsatt" : deptE.DepartmentName),
                                 EmployeeName = (err.EmployeeId == null ? "Ej tillsatt" : empE.EmployeeName)
                             };

            return errandList;
        }

        //Method returning an iterable collection of the helper class:
        public IQueryable<MyErrand> ShowErrandListInvestigator(string userName)
        {

            var errandList = from err in Errands
                             join stat in ErrandStatuses
                             on err.StatusId equals stat.StatusId
                             where err.EmployeeId == userName
                             join dep in Departments
                             on err.DepartmentId equals dep.DepartmentId
                             into departmentErrand
                             from deptE in departmentErrand
                             join em in Employees
                             on err.EmployeeId equals em.EmployeeId
                             into employeeErrand
                             from empE in employeeErrand.DefaultIfEmpty()
                             orderby err.RefNumber
                             ascending

                             select new MyErrand
                             {
                                 DateOfObservation = err.DateOfObservation,
                                 ErrandId = err.ErrandId,
                                 RefNumber = err.RefNumber,
                                 TypeOfCrime = err.TypeOfCrime,
                                 StatusName = stat.StatusName,
                                 DepartmentName = (err.DepartmentId == null ? "Ej tillsatt" : deptE.DepartmentName),
                                 EmployeeName = (err.EmployeeId == null ? "Ej tillsatt" : empE.EmployeeName)
                             };

            return errandList;
        }

        //Returns a list of employees belonging to the manager's department:
        public IQueryable<Employee> GetEmployeesForDepartment(string depID)
        {
            var employeeList = from emp in Employees
                               where emp.DepartmentId == depID

                               select new Employee
                               {
                                   EmployeeId = emp.EmployeeId,
                                   EmployeeName = emp.EmployeeName,
                                   RoleTitle = emp.RoleTitle,
                                   DepartmentId = emp.DepartmentId
                               };

            return employeeList;
        }


        //Fetches the department related to the manager ID that is provided as a string argument:
        public string GetManagerDept(string userName)
        {
            Employee employee = context.Employees.FirstOrDefault(ed => ed.EmployeeId == userName);

            return employee.DepartmentId;
        }


        //Method to query the database for specific errands in the ViewComponent:
        public Task<Errand> GetErrandInfo(int id)
        {
            return Task.Run
                (
                () =>
                    {
                        var errandInfo = Errands.Where(errand => errand.ErrandId == id).First();
                        return errandInfo;
                    }
                );
        }


        //Saves or updates an Employee object in the database:
        public void SaveEmployee(Employee employee)
        {
            if(employee.EmployeeId.Equals(0))
            {
                context.Employees.Add(employee);
            }
            else
            {
                Employee dbEntry = context.Employees.FirstOrDefault(ed => ed.EmployeeId == employee.EmployeeId);
                
                if(dbEntry != null)
                {
                    dbEntry.EmployeeName = employee.EmployeeName;
                    dbEntry.RoleTitle = employee.RoleTitle;
                    dbEntry.DepartmentId = employee.DepartmentId;
                }
            }
            context.SaveChanges();
        }


        //Gets sequence (table only has one entry):
        public Sequence GetSequence()
        {
            return context.Sequences.FirstOrDefault(sd => sd.Id == 1);
        }


        //Fetches errand with the defined id number:
        public Errand GetErrand(int id)
        {
            return context.Errands.FirstOrDefault(ed => ed.ErrandId == id);
        }


        //Updates the sequence:
        private void UpdateSequence()
        {
            var sequence = context.Sequences.FirstOrDefault(sd => sd.Id == 1);
            sequence.CurrentValue++;
        }


        //Saves an errand to the database:
        public void SaveErrand(Errand errand)
        {
            if(errand.ErrandId.Equals(0))
            {
                Sequence sequence = GetSequence();

                errand.RefNumber = "2020-45-" + sequence.CurrentValue;
                errand.StatusId = "S_A";

                context.Errands.Add(errand);
                UpdateSequence();
            }
            context.SaveChanges();
        }


        //Assigns a department id to an existing errand in the database:
        public void UpdateErrandWithDepartmentId(int errandId, string departmentId)
        {
            if (departmentId != "D00" && departmentId != "Välj")
            {
                Errand errand = GetErrand(errandId);
                errand.DepartmentId = departmentId;
                SaveErrand(errand);
                context.SaveChanges();
            }
        }


        //Assigns an investigator to an existing errand in the database:
        public void UpdateErrandWithInvestigator(int errandId, Errand errand, bool noAction)
        {
                Errand dbEntry = GetErrand(errandId);
                errand.EmployeeId = errand.EmployeeId;

                if (noAction)
                {
                    dbEntry.StatusId = "S_B";
                    dbEntry.InvestigatorInfo = errand.InvestigatorInfo;
                }
                else
                {
                    dbEntry.EmployeeId = errand.EmployeeId;
                }

                SaveErrand(dbEntry);
                context.SaveChanges();                
        }


        //Updates Errand with actions that have been taken by an Investigator:
        public void UpdateErrandWithInvestigatorActions(int ErrandId, Errand errand)
        {
            Errand dbEntry = GetErrand(ErrandId);

            if(errand.InvestigatorAction != null)
            {
                dbEntry.InvestigatorAction = dbEntry.InvestigatorAction + " Notering: " + errand.InvestigatorAction;
            }

            if (errand.InvestigatorInfo != null)
            {
                dbEntry.InvestigatorInfo = dbEntry.InvestigatorInfo + " Notering: " + errand.InvestigatorInfo;
            }

            if(errand.StatusId != "Välj")
            {
                dbEntry.StatusId = errand.StatusId;
            }
            
            context.SaveChanges();
        }


        //Method that saves uploaded files to the wwwroot folder:
        public async Task SaveFile(int ErrandId, IFormFile file, string fileType)
        {
            var tempPath = Path.GetTempFileName();

            if (file.Length > 0)
            {
                using (var stream = new FileStream(tempPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "___" + file.FileName;

            var path = Path.Combine(environment.WebRootPath, "InvestigatorImages", uniqueFileName);
            if (fileType == "sample")
            {
                path = Path.Combine(environment.WebRootPath, "InvestigatorSamples", uniqueFileName);
            }

            System.IO.File.Move(tempPath, path);

            if(fileType == "image")
            {
                SavePicture(ErrandId, uniqueFileName);
            }
            else if(fileType == "sample")
            {
                SaveSample(ErrandId, uniqueFileName);
            }
        }


        //Saves information about uploades picture to database:
        public void SavePicture(int ErrandId, string pictureName)
        {
            Picture picture = new Picture();
            picture.PictureName = pictureName;
            picture.ErrandId = ErrandId;

            context.Pictures.Add(picture);
            context.SaveChanges();
        }


        //Saves information about uploaded sample to database:
        public void SaveSample(int ErrandId, string sampleName)
        {
            Sample sample = new Sample();
            sample.SampleName = sampleName;
            sample.ErrandId = ErrandId;

            context.Samples.Add(sample);
            context.SaveChanges();
        }


    }
}
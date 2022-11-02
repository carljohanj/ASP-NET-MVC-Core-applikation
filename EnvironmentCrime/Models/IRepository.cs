namespace EnvironmentCrime.Models
{
    public interface IRepository
    {
        IQueryable<Department> Departments { get; }
        IQueryable<Employee> Employees { get; }
        IQueryable<Errand> Errands { get; }
        IQueryable<ErrandStatus> ErrandStatuses { get; }
        IQueryable<Picture> Pictures { get; }
        IQueryable <Sample> Samples { get; }
        IQueryable<Sequence> Sequences { get; }

        Task<Errand> GetErrandInfo(int id);

        void SaveEmployee(Employee employee);
        void SaveErrand(Errand errand);
        Sequence GetSequence();
        Errand GetErrand(int id);
        void UpdateErrandWithDepartmentId(int errandId, string departmentId);
        void UpdateErrandWithInvestigator(int errandId, Errand errand, bool noAction);
        void UpdateErrandWithInvestigatorActions(int ErrandId, Errand errand);
        Task SaveFile(int ErrandId, IFormFile file, string fileType);
        
        void SavePicture(int ErrandId, string pictureName);
        void SaveSample(int ErrandId, string sampleName);

        IQueryable<MyErrand> ShowErrandListCoordinator();
        IQueryable<MyErrand> ShowErrandListManager(string userName);
        IQueryable<MyErrand> ShowErrandListInvestigator(string userName);
        IQueryable<Employee> GetEmployeesForDepartment(string depID);
        string GetManagerDept(string userName);
    }
}

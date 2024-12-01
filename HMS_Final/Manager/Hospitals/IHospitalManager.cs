//using HMS_Final.Manager.Departments.DTO_temp_;
//using HMS_Final.Models;

//namespace HMS_Final.Manager.Hospitals
//{
//    public interface IHospitalManager
//    {
//        //Task<Hospital> CreateHospital(Hospital hospital);
//        //Task<Hospital> CreateHospital(Hospital hospital, List<HospitalDepartmentDTO1> departmentDTOs);
//        Task<Hospital> CreateHospital(Hospital hospital, List<HospitalDepartmentDTO1>? departmentDTOs = null);

//        Task<Hospital> GetHospital(int Id);
//        Task<IEnumerable<Hospital>> GetAllHospitals();
//        Task<bool> UpdateHospital(Hospital hospital, List<HospitalDepartmentDTO1> departmentDTOs);
//        //Task<bool> UpdateHospital(Hospital hospital);
//        Task<bool> DeleteHospital(int Id);
//    }
//}

using HMS_Final.Manager.Departments.DTO_temp_;
using HMS_Final.Manager.Hospitals;
using HMS_Final.Models;

namespace HMS_Final.Manager.Hospitals
{
    public interface IHospitalManager
    {
        // Create a hospital with optional department and doctor relationships
        Task<Hospital> CreateHospital(Hospital hospital,
                                       List<HospitalDepartmentDTO1>? departmentDTOs = null,
                                       List<HospitalDoctorDTO2>? doctorDTOs = null);

        // Get a specific hospital by its ID
        Task<Hospital> GetHospital(int Id);

        // Get all hospitals
        Task<IEnumerable<Hospital>> GetAllHospitals();

        // Update a hospital with department and doctor relationships
        Task<bool> UpdateHospital(Hospital hospital,
                                   List<HospitalDepartmentDTO1> departmentDTOs,
                                   List<HospitalDoctorDTO2>? doctorDTOs = null);

        // Delete a hospital and its related department and doctor relationships
        Task<bool> DeleteHospital(int Id);
        Task RemoveHospitalFromUserAsync(int hospitalId, int userId);
        Task AddHospitalToUserAsync(int hospitalId, int userId);

    }
}


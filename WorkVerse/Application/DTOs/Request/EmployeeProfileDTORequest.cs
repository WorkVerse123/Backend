using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class EmployeeProfileDTORequest
    {
        public string FullName { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Bio { get; set; }
        public string? Skills { get; set; }
        public string? Education { get; set; }
        public string? WorkExperience { get; set; }
        public string? Mode { get; set; }
    }
    public class EmployeeProfileUpdateDTORequest
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Bio { get; set; }
        public string? Skills { get; set; }
        public string? Education { get; set; }
        public string? WorkExperience { get; set; }
        public string? Mode { get; set; }
        public string? Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public bool IsPriority { get; set; } = false;

    }

    public class EmployeeFilterRequest
    {
        public string? Search { get; set; }
        public List<int>? EmployeeLocation { get; set; } = new List<int>(); // danh sách tỉnh/thành
        public List<int>? EmployeeEducation { get; set; } = new List<int>(); // trình độ học vấn
        public List<int>? Gender { get; set; } = new List<int>(); // giới tính (1: Male, 2: Female)
    }

    public enum EmployeeLocation
    {
        [Description("Hà Nội")]
        HaNoi = 1,

        [Description("Hồ Chí Minh")]
        HoChiMinh = 2,

        [Description("Hải Phòng")]
        HaiPhong = 3,

        [Description("Đà Nẵng")]
        DaNang = 4,

        [Description("Cần Thơ")]
        CanTho = 5,

        [Description("An Giang")]
        AnGiang = 6,

        [Description("Bà Rịa - Vũng Tàu")]
        BaRiaVungTau = 7,

        [Description("Bắc Giang")]
        BacGiang = 8,

        [Description("Bắc Ninh")]
        BacNinh = 9,

        [Description("Bình Dương")]
        BinhDuong = 10,

        [Description("Bình Định")]
        BinhDinh = 11,

        [Description("Bình Phước")]
        BinhPhuoc = 12,

        [Description("Bình Thuận")]
        BinhThuan = 13,

        [Description("Cà Mau")]
        CaMau = 14,

        [Description("Đắk Lắk")]
        DakLak = 15,

        [Description("Đắk Nông")]
        DakNong = 16,

        [Description("Đồng Nai")]
        DongNai = 17,

        [Description("Đồng Tháp")]
        DongThap = 18,

        [Description("Gia Lai")]
        GiaLai = 19,

        [Description("Hà Nam")]
        HaNam = 20,

        [Description("Hà Tĩnh")]
        HaTinh = 21,

        [Description("Hải Dương")]
        HaiDuong = 22,

        [Description("Hòa Bình")]
        HoaBinh = 23,

        [Description("Hưng Yên")]
        HungYen = 24,

        [Description("Khánh Hòa")]
        KhanhHoa = 25,

        [Description("Kiên Giang")]
        KienGiang = 26,

        [Description("Kon Tum")]
        KonTum = 27,

        [Description("Lâm Đồng")]
        LamDong = 28,

        [Description("Long An")]
        LongAn = 29,

        [Description("Nam Định")]
        NamDinh = 30,

        [Description("Nghệ An")]
        NgheAn = 31,

        [Description("Ninh Bình")]
        NinhBinh = 32,

        [Description("Ninh Thuận")]
        NinhThuan = 33,

        [Description("Phú Thọ")]
        PhuTho = 34
    }

    public enum EmployeeEducation
    {
        [Description("Trung học phổ thông")]
        HighSchool = 1,

        [Description("Cao đẳng")]
        College = 2,

        [Description("Đại học")]
        University = 3,

        [Description("Sau đại học")]
        Postgraduate = 4,

        [Description("Lao động phổ thông / Không bằng cấp")]
        None = 5
    }
    public enum GenderType
    {
        [Description("Male")]
        Male = 1,

        [Description("Female")]
        Female = 2,

        [Description("Others")]
        Others = 3
    }

}

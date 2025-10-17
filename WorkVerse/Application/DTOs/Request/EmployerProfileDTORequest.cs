using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class EmployerProfileDTORequest
    {
        public int EmployerId { get; set; } // 
        public int UserId { get; set; }
        public string CompanyName { get; set; } = null!; //
        public int EmployerTypeId { get; set; }
        public string Address { get; set; } = null!; //
        public string? WebsiteUrl { get; set; } //
        public string? LogoUrl { get; set; } //
        public DateTime? DateEstablish { get; set; } //
        public string Description { get; set; } = null!; //
        public string ContactPhone { get; set; } //
        public string ContactEmail { get; set; } //

    }

    public class EmployerFilterRequest
    {
        public string? Search { get; set; }
        public List<int>? EmployerTypeId { get; set; } = new List<int>();
        public List<int>? Locations { get; set; } = new List<int>(); // danh sách tỉnh/thành
    }

    public enum EmployerFilterType
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

}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KoiFarmShop.Repositories.Entities
{
    public class ConsignmentRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequestId { get; set; }


        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        public int ConsignmentFee { get; set; }
        public string ConsignmentType { get; set; }
        public string Certificate { get; set; }
        public string Notes { get; set; }


        public string KoiName { get; set; }

        // Ràng buộc tuổi phải từ 0 trở lên (có thể cá bé tính bằng ngày nên cho phép 0)
        [Range(0, int.MaxValue, ErrorMessage = "Tuổi cá không hợp lệ (phải từ 0 trở lên).")]
        public int KoiAge { get; set; }
        // Ràng buộc kích thước phải lớn hơn 0
        [Range(0.1, double.MaxValue, ErrorMessage = "Kích thước cá phải lớn hơn 0.")]
        public double KoiSize { get; set; }
        public string KoiBreed { get; set; }
        public DateTime ConsignmentDate { get; set; }
        public int ConsignmentDuration { get; set; }
        public string Remarks { get; set; } // Mô tả cá
        public string KoiImage { get; set; } // Đường dẫn ảnh cá
        public bool CareService { get; set; } // Checkbox chăm sóc đặc biệt

        // --- Liên kết dữ liệu ---
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public List<ConsignmentKoi> ConsignmentKois { get; set; }
    }
}
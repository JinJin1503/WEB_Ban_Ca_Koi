using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoiFarmShop.Repositories.Entities
{
    public class ConsignmentRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequestId { get; set; }

        // --- Các thuộc tính cũ của bạn ---
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        public int ConsignmentFee { get; set; }
        public string ConsignmentType { get; set; }
        public string Certificate { get; set; }
        public string Notes { get; set; }

        // --- CÁC THUỘC TÍNH MỚI CẦN THÊM ĐỂ HẾT LỖI ---
        public string KoiName { get; set; }
        public int KoiAge { get; set; }
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
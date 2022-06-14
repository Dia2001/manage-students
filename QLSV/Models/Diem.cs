namespace QLSV.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Diem")]
    public partial class Diem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string MaSV { get; set; }

        [Required]
        [StringLength(50)]
        public string MaMH { get; set; }

        public double? DiemCC { get; set; }

        public double? DiemGK { get; set; }

        public double? DiemCK { get; set; }

        public double? DTB { get; set; }

        [StringLength(50)]
        public string DiemChu { get; set; }

        [StringLength(50)]
        public string KetQua { get; set; }

        [StringLength(50)]
        public string NamHoc { get; set; }

        [StringLength(50)]
        public string HocKy { get; set; }

        public int? TrangThai { get; set; }

        public virtual MonHoc MonHoc { get; set; }

        public virtual SinhVien SinhVien { get; set; }
    }
}

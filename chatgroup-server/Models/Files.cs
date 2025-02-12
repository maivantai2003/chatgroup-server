using System.ComponentModel.DataAnnotations;

namespace chatgroup_server.Models
{
    public class Files
    {
        [Key]
        public int MaFile { get; set; }
        public string? TenFile { get; set; }
        public string? DuongDan { get; set; }
        public string? LoaiFile { get; set; } = String.Empty;
        public string? KichThuocFile { get; set; }
        public int TrangThai { get; set; } = 1;
    }
}

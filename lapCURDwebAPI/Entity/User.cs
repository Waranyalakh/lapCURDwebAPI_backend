using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lapCURDwebAPI.Entity
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //สร้างตัวเลขไอดีเองอัตโนมัติไม่ต้องคีย์
        public int Id { get; set; }

        
        public string Name { get; set; } = string.Empty;
        
       [Required]
        public string UserName { get; set; } = string.Empty;

        // เปลี่ยนประเภทของ PassWord เป็น PassWordHash และใช้ประเภท string
        [Required]
        public string PassWordHash { get; set; } = string.Empty;
    }
    /*public class UserModel 
    {
        
    }*/
}

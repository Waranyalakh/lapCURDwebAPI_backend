using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lapCURDwebAPI.Entity
{
    public class Useritem 
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Users))]
        public int User_Id { get; set; }

        [ForeignKey(nameof(items))]
        public int Item_Id { get; set; }
        

      
        public User Users { get; set; }

      
        public Item items { get; set; }

    }

    /*
    public class UseritemModel 
    {
    }*/
}

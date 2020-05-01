using LibraryManagement.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LibraryManagement.Models.DatabaseModels
{
    public class UserDatabase
    {

        public UserDatabase()
        {
            Books = new HashSet<BookDatabase>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime Validity { get; set; }
        public Role Role { get; set; }

        public ICollection<BookDatabase> Books { get; set; }
    }
}

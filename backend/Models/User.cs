using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class User : IEntity
    {
        [Column("UserId")]
        public int Id { get;set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }    
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastModifiedOn {get;set;} = DateTime.UtcNow;
        public string LastModifiedBy { get; set; }

        public ICollection<VacationPool> VacationPools { get; set; }
        public ICollection<VacationTime> VacationTimes { get; set; }
    }
}
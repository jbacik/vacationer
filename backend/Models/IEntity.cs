using System;

namespace backend.Models
{
    public interface IEntity
    {
        int Id {get;}   
        DateTime LastModifiedOn {get;}
        string  LastModifiedBy {get;}
    }
}
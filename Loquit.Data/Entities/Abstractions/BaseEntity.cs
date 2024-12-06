using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Data.Entities.Abstractions
{
    //its a base entity
    public abstract class BaseEntity
    {
        public int Id { get; set; }
    }
}
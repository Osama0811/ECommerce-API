using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Domain.Entities.Base
{
    internal interface IAuditable
    {
        Guid Id { get; set; }
        Guid? CreatedBy { get; set; }
        DateTime? CreationDate { get; set; }
        Guid? UpdatedBy { get; set; }
        DateTime? UpdatedDate { get; set; }
        bool IsDeleted { get; set; }

    }
}

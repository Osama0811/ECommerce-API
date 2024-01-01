using CircuitsUc.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Domain.Entities.Base
{
    public abstract class Auditable : IAuditable
    {
        public Auditable()
        {
        }

        public Auditable(Auditable obj)
        {

            CreatedBy = obj.CreatedBy;
            CreationDate = obj.CreationDate;
            UpdatedBy = obj.UpdatedBy;
            UpdatedDate = obj.UpdatedDate;
            IsDeleted = obj.IsDeleted;

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }

        [ForeignKey("CreatedBy")]
        public SecurityUser CreatedByUser { get; set; }
        [ForeignKey("UpdatedBy")]
        public SecurityUser UpdatedByUser { get; set; }
        /*   private string GenerateRandomCode(int iOTPLength)
           {
               string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
               string sOTP = string.Empty;
               Random rand = new Random();
               for (int i = 0; i < iOTPLength; i++)
               {
                   string sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                   sOTP += sTempChars;
               }
               return sOTP;
           }*/

    }
}

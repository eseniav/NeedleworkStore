//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NeedleworkStore.AppData
{
    using System;
    using System.Collections.Generic;
    
    public partial class AssigningStatuses
    {
        public int AssigningStatusID { get; set; }
        public int PaymentStatusID { get; set; }
        public int ReceivingStatusID { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int OrderID { get; set; }
        public int ProcessingStatusID { get; set; }
    
        public virtual Orders Orders { get; set; }
        public virtual PaymentStatuses PaymentStatuses { get; set; }
        public virtual ProcessingStatuses ProcessingStatuses { get; set; }
        public virtual ReceivingStatuses ReceivingStatuses { get; set; }
    }
}

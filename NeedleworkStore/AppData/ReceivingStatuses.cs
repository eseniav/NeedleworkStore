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
    
    public partial class ReceivingStatuses
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReceivingStatuses()
        {
            this.AssigningStatuses = new HashSet<AssigningStatuses>();
        }
    
        public int ReceivingStatusID { get; set; }
        public string ReceivingStatus { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssigningStatuses> AssigningStatuses { get; set; }
    }
}

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
    
    public partial class AccessoryTypes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AccessoryTypes()
        {
            this.ProductAccessoryTypes = new HashSet<ProductAccessoryTypes>();
        }
    
        public int AccessoryTypeID { get; set; }
        public string AccessoryTypeName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductAccessoryTypes> ProductAccessoryTypes { get; set; }
    }
}

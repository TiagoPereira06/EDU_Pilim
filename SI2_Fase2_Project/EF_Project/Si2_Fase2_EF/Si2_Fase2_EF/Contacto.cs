//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Si2_Fase2_EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Contacto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Contacto()
        {
            this.Contacto_Telefonico = new HashSet<Contacto_Telefonico>();
            this.Contacto_Email = new HashSet<Contacto_Email>();
        }
    
        public string Codigo { get; set; }
        public string CC { get; set; }
        public string Descricao { get; set; }
    
        public virtual Cliente Cliente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contacto_Telefonico> Contacto_Telefonico { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contacto_Email> Contacto_Email { get; set; }
    }
}

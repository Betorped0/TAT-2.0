//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TAT001.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class CITy
    {
        public long ID { get; set; }
        public string NAME { get; set; }
        public int STATE_ID { get; set; }
    
        public virtual STATE STATE { get; set; }
    }
}

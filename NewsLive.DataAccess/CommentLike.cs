//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NewsLive.DataAccess
{
    using System;

    public partial class CommentLike
    {
        public int CommentId { get; set; }
        public int PersonId { get; set; }
        public Nullable<bool> IsLiked { get; set; }
    
        public virtual Comment Comment { get; set; }
        public virtual Person Person { get; set; }
    }
}
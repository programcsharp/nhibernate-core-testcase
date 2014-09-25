using System;

namespace NHibernate.Test.NHSpecificTest.NH0000
{
    public class SubEntity
    {
        public virtual Guid Id { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime? DD { get; set; }
    }
}
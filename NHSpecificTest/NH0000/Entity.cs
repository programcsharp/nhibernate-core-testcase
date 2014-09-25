using System;
using System.Collections.Generic;

namespace NHibernate.Test.NHSpecificTest.NH0000
{
    public class Entity
    {
        public Entity()
        {
            SubEntities = new List<SubEntity>();
        }

        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<SubEntity> SubEntities { get; set; }
    }
}
using NHibernate.Linq;
using NUnit.Framework;
using System;
using System.Linq;

namespace NHibernate.Test.NHSpecificTest.NH0000
{
    [TestFixture]
    public class Fixture : BugTestCase
    {
        protected override void OnSetUp()
        {
            using (var session = OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var e1 = new Entity { Name = "Bob" };

                e1.SubEntities.Add(new SubEntity() { Entity = e1, Name = "Test1" });
                e1.SubEntities.Add(new SubEntity() { Entity = e1, Name = "Test2", DD = DateTime.Now });

                session.Save(e1);

                var e2 = new Entity { Name = "Sally" };
                session.Save(e2);

                session.Flush();
                transaction.Commit();
            }
        }

        protected override void OnTearDown()
        {
            using (var session = OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.DisableFilter("entityDeletedFilter");
                session.DisableFilter("bagDeletedFilter");

                session.Delete("from System.Object");

                session.Flush();
                transaction.Commit();
            }
        }

        [Test]
        public void CollectionFilterWorks()
        {
            using (var session = OpenSession())
            using (session.BeginTransaction())
            {
                Entity entity = session.Query<Entity>().FirstOrDefault(x => x.Name == "Bob");

                Assert.AreEqual(1, entity.SubEntities.Count);
            }
        }

        [Test]
        public void QueryableFilterWorks()
        {
            using (var session = OpenSession())
            using (session.BeginTransaction())
            {
                var subEntities = session.Query<SubEntity>().Where(x => x.Entity.Name == "Bob").ToList();

                Assert.AreEqual(1, subEntities.Count);
            }
        }

        [Test]
        public void QueryableSelectManyFilterWorks()
        {
            using (var session = OpenSession())
            using (session.BeginTransaction())
            {
                var subEntities = session.Query<Entity>().Where(x => x.Name == "Bob").SelectMany(x => x.SubEntities).ToList();

                Assert.AreEqual(1, subEntities.Count);
            }
        }

        [Test]
        public void QueryOverFilterWorks()
        {
            using (var session = OpenSession())
            using (session.BeginTransaction())
            {
                var subEntities = session.QueryOver<SubEntity>().JoinQueryOver(x => x.Entity).Where(x => x.Name == "Bob").List();

                Assert.AreEqual(1, subEntities.Count);
            }
        }

        protected override ISession OpenSession()
        {
            ISession session = base.OpenSession();

            session.EnableFilter("entityDeletedFilter");
            session.EnableFilter("bagDeletedFilter");

            return session;
        }

    }
}
﻿using System.Linq;
using Marten.Services;
using Marten.Testing.Documents;
using Shouldly;

namespace Marten.Testing.TrackingSession
{
    public class document_session_load_already_loaded_document_with_IdentityMap_Tests : document_session_load_already_loaded_document_Tests<IdentityMap> { }
    public class document_session_load_already_loaded_document_with_DirtyTracking_Tests : document_session_load_already_loaded_document_Tests<DirtyTrackingIdentityMap> { }

    public class document_session_load_already_loaded_document_Tests<T> : DocumentSessionFixture<T> where T : IIdentityMap
    {
        public void when_loading_then_the_document_should_be_returned()
        {
            var user = new User { FirstName = "Tim", LastName = "Cools" };
            theSession.Store(user);
            theSession.SaveChanges();

            using (var session = CreateSession())
            {
                var first = session.Load<User>(user.Id);
                var second = session.Load<User>(user.Id);

                first.ShouldBeSameAs(second);
            }
        }

        public void when_loading_by_ids_then_the_same_document_should_be_returned()
        {
            var user = new User { FirstName = "Tim", LastName = "Cools" };
            theSession.Store(user);
            theSession.SaveChanges();

            using (var session = CreateSession())
            {
                var first = session.Load<User>(user.Id);
                var second = session.Load<User>()
                    .ById(user.Id)
                    .SingleOrDefault();

                first.ShouldBeSameAs(second);
            }
        }
    }
}
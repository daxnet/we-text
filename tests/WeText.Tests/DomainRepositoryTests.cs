using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Messaging;
using WeText.Domain;
using WeText.DomainRepositories;

namespace WeText.Tests
{
    [TestFixture]
    public class DomainRepositoryTests
    {
        private readonly Mock<IMessagePublisher> bus = new Mock<IMessagePublisher>();

        [Test]
        public async Task SaveAggregateRootTest()
        {
            var user = new User(Guid.NewGuid(), "Sunny Chen", "Email");
            var repository = new InMemoryDomainRepository(bus.Object);
            await repository.SaveAsync<Guid, User>(user);
            Assert.AreEqual(1, repository.Events.Count);
        }

        [Test]
        public async Task ChangeDisplayNameAndSaveTest()
        {
            var user = new User(Guid.NewGuid(), "Sunny Chen", "Email");
            user.ChangeDisplayName("Sunny Chen");
            var repository = new InMemoryDomainRepository(bus.Object);
            await repository.SaveAsync<Guid, User>(user);
            Assert.AreEqual(2, repository.Events.Count);
        }

        [Test]
        public async Task GetAggregateRootTest()
        {
            var userId = Guid.NewGuid();
            var user = new User(userId, "Sunny Chen", "Email");
            var repository = new InMemoryDomainRepository(bus.Object);
            await repository.SaveAsync<Guid, User>(user);
            Assert.AreEqual(1, repository.Events.Count);
            var user2 = await repository.GetByKeyAsync<Guid, User>(userId);
            Assert.AreEqual(userId, user2.Id);
            Assert.AreEqual(user.Name, user2.Name);
            Assert.AreEqual(user.Email, user2.Email);
        }
    }
}

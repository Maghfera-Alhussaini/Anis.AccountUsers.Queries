using Anis.AccountUsers.Queries.Grpc;
using Anis.AccountUsers.Queries.Test.Fakers;
using Anis.AccountUsers.Queries.Test.Helpers;
using Anis.AccountUsers.Queries.TestLive.Helpers;
using Anis.AccountUsers.Queries.TestLive.Protos;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Anis.AccountUsers.Queries.Test.LiveTests
{
    public class UserAssignedToAccountTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly DbContextHelper _dbContextHelper;
        public UserAssignedToAccountTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.SetLiveTestsDefaultEnvironment();
            });

            _dbContextHelper = new DbContextHelper(factory.Services);


        }

        [Fact]
        public async Task UserAssignedToAccount_AssignUserForNotExistAccount_ReturnTrue()
        {
            // Arrange

            var request = new UserRequest
            {
                AccountId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                Sequence = 1
            };

            // Act
            await DemoEventsClientHelper.Query(r => r.AssignUsersAsync(request));

            await Task.Delay(20000);

            var account = await _dbContextHelper.Query(db => db.Accounts.Include(a => a.Users).SingleOrDefaultAsync());

            // Assert


            Assert.NotNull(account);

            Assert.Single(account.Users);

            Assert.Equal(request.AccountId, account.Id.ToString());

            Assert.Equal(request.Sequence, account.Sequence);

            var user = account.Users.Single();

            Assert.Equal(request.UserId, user.Id.ToString());
        }

        [Fact]
        public async Task UserAssignedToAccount_AssignUserForExistAccount_ReturnTrue()
        {
            // Arrange

            var account = await _dbContextHelper.InsertAsync(AccountFaker.CreateWithSingleUser(Guid.NewGuid()));

            var request = new UserRequest
            {
                AccountId = account.Id.ToString(),
                UserId = Guid.NewGuid().ToString(),
                Sequence = 2
            };

            // Act

            await DemoEventsClientHelper.Query(r => r.AssignUsersAsync(request));

            await Task.Delay(10000);

            var dbAccount = await _dbContextHelper.Query(db => db.Accounts.Include(a => a.Users).SingleOrDefaultAsync());

            // Assert


            Assert.NotNull(dbAccount);

            Assert.Equal(2, dbAccount.Users.Count);

            Assert.Equal(request.AccountId, dbAccount.Id.ToString());

            Assert.Equal(request.Sequence, dbAccount.Sequence);

            var user = dbAccount.Users.Single(u => u.Id == Guid.Parse(request.UserId));

            Assert.Equal(request.UserId, user.Id.ToString());
        }

    }
}

using Anis.AccountUsers.Queries.Grpc;
using Anis.AccountUsers.Queries.Test.Fakers;
using Anis.AccountUsers.Queries.Test.Helpers;
using Anis.AccountUsers.Queries.TestLive.Helpers;
using Anis.AccountUsers.Queries.TestLive.Protos;
using Bogus.Premium;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Anis.AccountUsers.Queries.TestLive.Test
{
    public class UserDeletedFromAccountTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly DbContextHelper dbContextHelper;
        public UserDeletedFromAccountTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.SetLiveTestsDefaultEnvironment();
            });

            dbContextHelper = new DbContextHelper(factory.Services);

        }
        public async Task UserDeletedFromAccount_DeleteUserForExistAccount_ReturnTrue()
        {
            //Arrange
            var account = await dbContextHelper.InsertAsync(AccountFaker.CreateWithSingleUser(Guid.NewGuid()));

            var request = new UserRequest
            {
                AccountId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                Sequence = 2
            };


            //Act
            await DemoEventsClientHelper.Query(r => r.DeleteUsersAsync(request));

            await Task.Delay(20000);

            var savedAccount = await dbContextHelper.Query(db => db.Accounts.Include(a => a.Users).SingleOrDefaultAsync());
           
            //Assert
            Assert.NotNull(savedAccount);          
            Assert.Empty(savedAccount.Users);
            Assert.Equal(request.Sequence, savedAccount.Sequence);
            Assert.Equal(request.AccountId, savedAccount.Id.ToString());
        }
        }
    }

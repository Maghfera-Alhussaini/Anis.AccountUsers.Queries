using Anis.AccountUsers.Queries.Grpc;
using Anis.AccountUsers.Queries.Test.Fakers;
using Anis.AccountUsers.Queries.Test.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Anis.AccountUsers.Queries.Test.EventsTests
{
    public class UserDeletedFromAccountTest: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly DbContextHelper dbContextHelper;
        private readonly HandlerHelper handlerHelper;
        public UserDeletedFromAccountTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper) 
        {
            factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.SetUnitTestsDefaultEnvironment();
            });
            dbContextHelper = new DbContextHelper(factory.Services);
            handlerHelper = new HandlerHelper(factory.Services);
        }
        [Fact]
        public async Task UserDeletedFromAccount_DeleteUserForNotExistAccount_ReturnFalse()
        {
            // Arrange

            var userDeletedFromAccountEvent = new UserDeletedFromAccountFaker().Generate();

            // Act

            var result = await handlerHelper.HandleMessage(userDeletedFromAccountEvent);

            var account = await dbContextHelper.Query(db => db.Accounts.Include(a => a.Users).SingleOrDefaultAsync());

            // Assert

            Assert.False(result);

            Assert.Null(account);
        }
        [Fact]
        public async Task UserDeletedFromAccount_DeleteUserForExistAccount_ReturnTrue()
        {   
            //Arrange
           
            var account = await dbContextHelper.InsertAsync(AccountFaker.CreateWithSingleUser(Guid.NewGuid()));
            var userDeletedFromAccountEvent = new UserDeletedFromAccountFaker(account.Id,account.Users.First().Id,2).Generate();

            //Act
            var result = await handlerHelper.HandleMessage(userDeletedFromAccountEvent);
            var storedAccount = await dbContextHelper.Query(db => db.Accounts.Include(a => a.Users).SingleOrDefaultAsync());

            //Assert
            Assert.True(result);
            Assert.NotNull(storedAccount);
            Assert.Empty(storedAccount.Users);
            Assert.Equal(userDeletedFromAccountEvent.Sequence, storedAccount.Sequence);
        }
        [Theory]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(6, false)]

        public async Task UserDeletedFromAccount_InvalidSequence_Return(int eventSequence, bool expectedResult)
        {
            // Arrange
           
            var account = await dbContextHelper.InsertAsync(new AccountFaker(sequence: 3).Generate());
       
            var userDeletedFromAccountEvent = new UserDeletedFromAccountFaker(account.Id, eventSequence).Generate();

            // Act
            var result = await handlerHelper.HandleMessage(userDeletedFromAccountEvent);

            var dbAccount = await dbContextHelper.Query(db => db.Accounts.Include(a => a.Users).SingleAsync());
            // Assert
        
            Assert.Equal(expectedResult, result);
           
            Assert.Equal(account.Sequence, dbAccount.Sequence);
        }

    }
}

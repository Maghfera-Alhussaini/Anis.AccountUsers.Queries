using Anis.AccountUsers.Queries.Domain.Resources;
using Anis.AccountUsers.Queries.Grpc;
using Anis.AccountUsers.Queries.Grpc.Protos;
using Anis.AccountUsers.Queries.Test.Fakers;
using Anis.AccountUsers.Queries.Test.Helpers;
using Anis.AccountUsers.Queries.Test.Protos;
using Calzolari.Grpc.Net.Client.Validation;
using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using GetAccountUsersRequest = Anis.AccountUsers.Queries.Test.Protos.GetAccountUsersRequest;

namespace Anis.AccountUsers.Queries.Test.QueriesTests
{
    public class GetAccountUsersTest: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly DbContextHelper dbContextHelper;
        private readonly GrpcClientHelper grpcClientHelper;
     
        public GetAccountUsersTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.SetUnitTestsDefaultEnvironment();
            });
            dbContextHelper = new DbContextHelper(factory.Services);
            grpcClientHelper = new GrpcClientHelper(factory);
        }
        [Fact]
        public async Task GetAccountUsers_GetAccountUsersFromNotExistingAccount_ThrowNotFoundException()
        {
            //Arrange
            var request = new GetAccountUsersRequest
            {
                AccountId = Guid.NewGuid().ToString()
            };

            //Act
            var exception = await Assert.ThrowsAsync<RpcException>(
                  async () => await grpcClientHelper.Query(x => x.GetAccountUsersAsync(request: request)));
            var account = await dbContextHelper.Query(db => db.Accounts.SingleOrDefaultAsync());

            //Assert
         

            Assert.Equal(StatusCode.NotFound, exception.StatusCode);
            Assert.Equal(Phrases.AccountNotFound, exception.Status.Detail);
            Assert.Null(account);
        }
        [Fact]
        public async Task GetAccountUsersQuery_GetAccountUsersQueryFromExistingAccount_ReturnAccountUsers()
        {
            //Arrange

            var account = AccountFaker.CreateWithSingleUser(Guid.NewGuid(), Guid.NewGuid());
            await dbContextHelper.InsertAsync(account);

            var request = new GetAccountUsersRequest
            {
                AccountId = account.Id.ToString()
            };

            //Act
            var response = await grpcClientHelper.Query(x => x.GetAccountUsersAsync(request: request));

            var accountQuery = await dbContextHelper.Query(db => db.Accounts.Include(u => u.Users).SingleOrDefaultAsync());


            //Assert

            Assert.NotNull(response);

            Assert.NotNull(accountQuery);

            Assert.Equal(accountQuery.Id.ToString(), response.AccountId);

            Assert.Equal(accountQuery.Users.Single().Id.ToString(), response.Users.Single());


        }

        [Fact]    
        public async Task GetAccountUsersQuery_InvalidData_ReturnInvalidArgument()
        {
            //Arrange
            var request = new GetAccountUsersRequest
            {
                AccountId = "28D3BE19-1F24-44E4-8D18"
            };

            //Act
            var exception = await Assert.ThrowsAsync<RpcException>(
                  async () => await grpcClientHelper.Query(r => r.GetAccountUsersAsync(request: request)));

            var account = await dbContextHelper.Query(db => db.Accounts.Include(u => u.Users).SingleOrDefaultAsync());


            //Assert

            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
            Assert.NotEmpty(exception.Status.Detail);            
            Assert.Null(account);
        }
    }
}

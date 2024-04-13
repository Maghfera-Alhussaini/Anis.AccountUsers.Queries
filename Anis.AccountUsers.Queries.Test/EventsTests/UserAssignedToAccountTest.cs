using Anis.AccountUsers.Queries.Grpc;
using Anis.AccountUsers.Queries.Test.Fakers;
using Anis.AccountUsers.Queries.Test.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Anis.AccountUsers.Queries.Test.EventsTests
{
   public class UserAssignedToAccountTest: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly DbContextHelper dbContextHelper;
        private readonly HandlerHelper handlerHelper;
        public UserAssignedToAccountTest(WebApplicationFactory<Program> factory,ITestOutputHelper helper)
        {
            factory= factory.WithDefaultConfigurations(helper, services =>
            {
                services.SetUnitTestsDefaultEnvironment();
            });
            dbContextHelper = new DbContextHelper(factory.Services);
            handlerHelper= new HandlerHelper(factory.Services);

        }
        //تسجيل مستخدم في حساب غير موجود، يجب ان يتم انشاء حساب وتسجيل الممستخد فيه ويرجع true
        [Fact]
        public async Task UserAssignedToAccount_AssignUserToNotExistAccount_ReturnTrue()
        {
            //Arrange

            //ننشئ حدث مزيف
            var userAssignedToAccountEvent = new UserAssignedToAccountFaker().Generate();
            //بعد هذه الخطوة سيكون قد تم اطلاق حدث من نوع (تسجيل مستخدم في حساب 
           
            //Act

            // يرسل المسج عبر الميديتور الى الهاندلر المناسب وذلك من خلال كلاس الhandlerhelper
            var result = await handlerHelper.HandleMessage(userAssignedToAccountEvent);
            //بعد هذه  الخطوة سيكون قد تمت معالجة الحدث وحفظ المعلومات في الداتابيز والنتيجة المرجعة هي true
     
            //نقوم بجلب الحساب والمستخدمين الموجودين فيه من الداتابيز لنتأكد من الخطوة السابقة 
            var account = await dbContextHelper.Query(db => db.Accounts.Include(a => a.Users).SingleOrDefaultAsync());
           //هنا سيكون لدينا النتيجة مخزنة في account
            
            //Assert

            //نتأكد من ان النتيجة المرجعة من الميديتور هي true
            Assert.True(result);
            //نتأكد من أن النتيجة المعادة من الداتابيز ليست فارغة 
            Assert.NotNull(account);
            //نتأكد من ان النتيجة هي واحدة فقط ضمن الحساب
            Assert.Single(account.Users);
            //نتأكد من ان الاغريغت الخاص بالحدث هو نفسه الحساب الذي تم حفظه
            Assert.Equal(userAssignedToAccountEvent.AggregateId, account.Id);
            //نتأكد من ان التسلسل الخاص بالحدث هو نفسه في الداتابيز
            Assert.Equal(userAssignedToAccountEvent.Sequence, account.Sequence);
            //نجلب اخر مستخدم فيالحساب ثم نتأكد من معرفه مساوي لمعرف المستخدم الذي تم تسجيله والمرسل مع بيانات الحدث
            var user = account.Users.Single();

            Assert.Equal(userAssignedToAccountEvent.Data.UserId, user.Id);

        }
        //تسجيل مستخدم الى حساب موجود - يجب ان يرجع true
        [Fact]
        public async Task UserAssignedToAccount_AssignUserToExistAccount_ReturnTrue()
        {
            // Arrange
            //نقوم بعمل insert للحساب من خلال الfaker مع انشاء مستخدم واحد
            var account = await dbContextHelper.InsertAsync(AccountFaker.CreateWithSingleUser(Guid.NewGuid()));
            //ننشئ حدث مزيف لنفس الحساب ونرسل تسلسل 2 لأن انشاء الحساب كان 1
            var userAssignToAccountEvent = new UserAssignedToAccountFaker(account.Id, 2).Generate();

            // Act
            
            var result = await handlerHelper.HandleMessage(userAssignToAccountEvent);

            var dbAccount = await dbContextHelper.Query(db => db.Accounts.Include(a => a.Users).SingleOrDefaultAsync());
            // Assert

            Assert.True(result);

            Assert.NotNull(dbAccount);
            //نتأكد من ان عدد المستخدمين هو 2
            Assert.Equal(2, dbAccount.Users.Count);

            Assert.Equal(userAssignToAccountEvent.AggregateId, dbAccount.Id);

            Assert.Equal(userAssignToAccountEvent.Sequence, dbAccount.Sequence);

            var user = dbAccount.Users.Single(c => c.Id == userAssignToAccountEvent.Data.UserId);

            Assert.Equal(userAssignToAccountEvent.Data.UserId, user.Id);
        }
        //في حال كان التسلسل غير صحيح
        [Theory]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(6, false)]

        public async Task UserAssignedToAccount_InvalidSequence_Return(int eventSequence, bool expectedResult)
        {
            // Arrange
            //ننشئ حساب مزيف ونعطيه تسلسل 3
            var account = await dbContextHelper.InsertAsync(new AccountFaker(sequence: 3).Generate());
            //ننشئ حدث مزيف لتسجيل مستخدم في الحساب السابق ونعطيه التسلسل الممرر غبر التيست
            var userAssignToAccountEvent = new UserAssignedToAccountFaker(account.Id, eventSequence).Generate();

            // Act
            var result = await handlerHelper.HandleMessage(userAssignToAccountEvent);

            var dbAccount = await dbContextHelper.Query(db => db.Accounts.Include(a => a.Users).SingleAsync());
            // Assert
            //يجب ان تكون النتيجة المعادة بعد معالجة المسج مساوية للنتيجة المتوقعة الممررة عبر التيست
            Assert.Equal(expectedResult, result);
            //يجب ان يكون التسلسل الخاص بالحساب المنشأ مساوي للتسلسل المخزن بالداتابيز
            Assert.Equal(account.Sequence, dbAccount.Sequence);
        }

    }
}

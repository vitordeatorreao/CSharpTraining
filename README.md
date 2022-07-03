# Hands-on C# + .NET Training

This is part of a C# + .NET training. In order to better solidify the concepts and constructs of C#
and .NET, I've prepared this hands-on exercise for the audience to engage. We will build a toy Bank
REST API using C# 10 and .NET 6.0.

The skeleton structure of our application if provided for you. Aside from the result of the
`dotnet new webapi` command, a Data Access Object is provided and included in the Dependency
Injection container for you. Also, some VS Code configurations are provided to make your setup
easier. Last but not least, you are also provided with a skeleton controller for the first two
exercises. Don't worry, you will be creating your own Controller on exercise number 3. Dealing with
the Database is outside the scope of this tutorial, so we will be using an InMemory database with
Entity Framework.

In summary, here's the software you will need to follow along with this hands on:
- [Git](https://git-scm.com/download);
- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0);
- [Visual Studio Code](https://code.visualstudio.com/download);
- [C# Extension for VS Code](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp).

### Exercise 1

For exercise one, we will be implementing the AccountsController routes. The boilerplate code,
as well as the interface contract is prepared for you. Here's the expected semantic of each
operation:

 * **Create:** should insert a new Account into the database. It should receive only the Account
 name, its owner name and whether or not its balance can be negative. The action should validate
 that the first two parameters exist in the request, otherwise, it should return an HTTP 400 (Bad
 Request). If `CanGoNegative` doesn't exist in the request, then we should assume it is `false`.
 You can return a HTTP 400 status by using the instance method
 [`ControllerBase.BadRequest()`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.badrequest?view=aspnetcore-6.0).
 `Id` should be initialized to a new Guid, which you can generate in C# by running the static
 method
 [`Guid.NewGuid()`](https://docs.microsoft.com/en-us/dotnet/api/system.guid.newguid?view=net-6.0).
 `OpenedAt` should be initialized to the current Date and Time in UTC
 timezone. You can obtain that by using the static property
 [`DateTime.UtcNow`](https://docs.microsoft.com/en-us/dotnet/api/system.datetime.utcnow?view=net-6.0).
 `Balance` should be zero and `ClosedAt` should be `null`. In case the insert to the database is
 successful, you should return HTTP 201 (Created) and the full content of the Account in the body,
 represented by the `BankAccountResponse` class. You can return HTTP 201 (Created), by using the
 instance method
 [`ControllerBase.CreatedAtAction`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.createdataction?view=aspnetcore-6.0);
 * **Read:** should return the Account with the corresponding Id, which comes in through the
 request PATH. You should return an HTTP 404 (Not Found) in case no Account possesses the specified
 Id. You can return HTTP 404 by running the instance method
 [`ControllerBase.NotFound()`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.notfound?view=aspnetcore-6.0).
 * **Update:** should receive a new name and/or owner name in the request body, and the account id
 in the request PATH. Both the name and the owner name are optional. We will update either, both or
 neither, depending on whether or not a value different than `""` is present in the request. We
 should NOT be able to change the `CanGoNegative` property from the Update action. First, you
 should fetch the Account as it exists currently in the database using the DAL. If no Account is
 found, you should return HTTP 404 (Not Found) just like you did in the Read action. If it exists,
 you should alter the name, owner name or both and save it to the database using the DAL;
 * **Delete:** should receive an ID and delete its corresponding Account from the Database. When
 you call `IAccountDAL.DeleteById` it will either return the `Account` object which was deleted, or
 it will throw a `AccountNotFoundException`. You should catch that Exception and return HTTP 404
 (Not Found) in that case;
 * **List:** to complete our little API, we will have an action which lists our Accounts. Remember
 to use [`yield return`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/yield).

You can check the `exerc1` branch in this repository for my suggested answer.

### Exercise 2

For our second exercise, you will convert our current AccountsController to have `async` actions.
You will also drop the sync versions of our Data Access Layer and use the async versions. To do
that, change the code as follows.

In `./Program.cs` at line 9:

```diff
- builder.Services.AddDbContext<IAccountDAL, AccountDAO>(opt =>
+ builder.Services.AddDbContext<IAccountDALAsync, AccountDAOAsync>(opt =>
```

In `./Controllers/AccountsController.cs`:

```diff
 public class AccountsController : ControllerBase
 {
     private readonly ILogger<AccountsController> _logger;
-    private readonly IAccountDAL _accountDAL;
+    private readonly IAccountDALAsync _accountDAL;

-    public AccountsController(ILogger<AccountsController> logger, IAccountDAL accountDAL)
+    public AccountsController(ILogger<AccountsController> logger, IAccountDALAsync accountDAL)
     {
         _logger = logger;
         _accountDAL = accountDAL;
     }
```

Finally, you will have to change your Action methods to return `Task<ActionResult<...>>`, instead
of `ActionResult<...>`, and you will have to add the `async` modifier to the method. Also, you will
have to adjust your calls to the DAL methods, as you now need to `await` them. Don't forget to use
`IAsyncEnumerable<...>` as return type for the `List()` action.

Other than that, the interface contract of the Controller should remain the same. When testing, you
should see no difference in the behavior of your requests and responses.

You can check the `exerc2` branch in this repository for my suggested answer.

### Exercise 3

For the last exercise, you will be adding a new Controller, `TransfersController`. As the name
suggests, it will deal with transfering funds from one account into another. It should contain a
single Action, a POST method which receives in its body a Request object containing the source
account ID (where the funds are coming from), the destination account ID (where the funds are going
to) and the amount being transfered (use `decimal`, not `double`. More on this can be found
[here](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types)).

If this were a real banking API, we would have to deal with the atomicity of this operation. We
would have to protect our API from concurrent transfer requests taking or placing funds in the same
account at the same time. But for the purposes of this exercise, since we are studying C# and .NET,
we should ignore this issue for now.

We would also have to deal with cents, as an amount of $405,2345 doesn't exist. But lets assume our
fictional currency has unlimited decimal places.

You shouldn't need to change any file outside of `./Controllers` folder. You will need a new file
for the `TransfersController` class and a new `./DTOs` file for the request object.

The only logic involved here is that you should not allow Accounts where `canGoNegative` is `false`
to have a Balance below zero. If a Transfer would create such situation, it should fail with a
HTTP status 400 (Bad Request). Bonus points if you can provide a message to the API client saying
the source account has no funds. You should also return HTTP status 404 (Not Found) in case the id
of one of the accounts is not found in the database.

If the Transfer is successful, the client should receive an HTTP status 200 (Ok). I leave to you to
decide what should be the content of the response. Just remember that in case you leave it empty,
you should return HTTP status 204 (No Content).

As for previous exercises, you can check the suggested answer in the `exerc3` branch in this
repository.

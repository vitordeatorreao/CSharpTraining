### Exercise 1

For exercise one, we will be implementing the AccountsController routes. The boilerplate code,
as well as the interface contract is prepared for you. Here's the expected semantic of each
operation:

 * **Create:** should insert a new Account into the database. It should receive only the Account
 name and its owner name. The action should validate that these two parameters exist in the
 request, otherwise, it should return an HTTP 400 (Bad Request). You can return that error by
 using the instance method
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
 * **Update:** should receive a new name and owner name in the request body, and the account id in
 the request PATH. Both the name and the owner name are optional. We will update either, both or
 neither, depending on whether or not a value different than `""` is present in the request. First,
 you should fetch the Account as it exists currently in the database using the DAL. If no Account
 is found, you should return HTTP 404 (NotFound) just like you did in the Read action. If it
 exists, you should alter the name, owner name or both and save it to the database using the DAL;
 * **Delete:** should receive an ID and delete its corresponding Account from the Database. When
 you call `IAccountDAL.DeleteById` it will either return the `Account` object which was deleted, or
 it will throw a `AccountNotFoundException`. You should catch that Exception and return HTTP 404
 (NotFound) in that case;
 * **List:** to complete our little API, we will have an action which lists our Accounts. Remember
 to use [`yield return`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/yield).

### Exercise 2

For our second exercise, you will convert our current AccountsController to have `async` actions.
In the `async_prep` branch of this repository, you will find the preparations to convert the


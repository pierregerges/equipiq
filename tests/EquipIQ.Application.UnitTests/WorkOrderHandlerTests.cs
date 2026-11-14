using EquipIQ.Application.UnitTests.Fakes;
using EquipIQ.Application.WorkOrders.CreateWorkOrder;
using EquipIQ.Application.WorkOrders.DeleteWorkOrder;
using EquipIQ.Application.WorkOrders.GetWorkOrderById;
using EquipIQ.Application.WorkOrders.GetWorkOrders;
using EquipIQ.Application.WorkOrders.UpdateWorkOrder;
using EquipIQ.Core.Common;
using EquipIQ.Core.WorkOrders;

namespace EquipIQ.Application.UnitTests;

[TestClass]
public sealed class WorkOrderHandlerTests
{
    private static readonly DateTimeOffset Now = new(2026, 11, 13, 12, 0, 0, TimeSpan.Zero);

    [TestMethod]
    public async Task WorkOrderLifecycle_AddGetListUpdateAndRemove_WorksThroughTheHandlers()
    {
        var repository = new FakeWorkOrderRepository();
        var unitOfWork = new FakeUnitOfWork();

        var createHandler = new CreateWorkOrderCommandHandler(repository, unitOfWork, new FixedTimeProvider(Now));
        var getHandler = new GetWorkOrderByIdQueryHandler(repository);
        var listHandler = new GetWorkOrdersQueryHandler(repository);
        var updateHandler = new UpdateWorkOrderCommandHandler(repository, unitOfWork);
        var deleteHandler = new DeleteWorkOrderCommandHandler(repository, unitOfWork);

        // 1. Add
        var created = await createHandler.Handle(
            new CreateWorkOrderCommand(
                "Ventilator error E12", "Screen shows E12.", "Ventilator V200",
                WorkOrderPriority.High, "tech@example.com"),
            CancellationToken.None);

        Assert.HasCount(1, repository.Items);
        Assert.AreEqual(WorkOrderStatus.Open, created.Status);
        Assert.AreEqual(1, unitOfWork.SaveCount);

        // 2. Get by id (a query: it reads and never saves)
        var fetched = await getHandler.Handle(new GetWorkOrderByIdQuery(created.Id), CancellationToken.None);

        Assert.AreEqual(created.Id, fetched.Id);
        Assert.AreEqual("Ventilator error E12", fetched.Title);
        Assert.AreEqual(1, unitOfWork.SaveCount);

        // 3. Get all (also a query). The filters must reach the repository,
        //    and the results must come back as DTOs with the paging numbers.
        var page = await listHandler.Handle(
            new GetWorkOrdersQuery(WorkOrderStatus.Open, WorkOrderPriority.High, "ventilator", Page: 1, PageSize: 10),
            CancellationToken.None);

        Assert.IsNotNull(repository.LastCriteria);
        Assert.AreEqual(WorkOrderStatus.Open, repository.LastCriteria.Status);
        Assert.AreEqual(WorkOrderPriority.High, repository.LastCriteria.Priority);
        Assert.AreEqual("ventilator", repository.LastCriteria.SearchText);
        Assert.HasCount(1, page.Items);
        Assert.AreEqual(created.Id, page.Items[0].Id);
        Assert.AreEqual(1, page.TotalCount);
        Assert.AreEqual(1, unitOfWork.SaveCount);

        // 4. Update
        var updated = await updateHandler.Handle(
            new UpdateWorkOrderCommand(created.Id, "New title", "New description", "New pump", WorkOrderPriority.Critical),
            CancellationToken.None);

        Assert.AreEqual("New title", updated.Title);
        Assert.AreEqual(WorkOrderPriority.Critical, updated.Priority);
        Assert.AreEqual(2, unitOfWork.SaveCount);

        // 5. Remove
        await deleteHandler.Handle(new DeleteWorkOrderCommand(created.Id), CancellationToken.None);

        Assert.IsEmpty(repository.Items);
        Assert.AreEqual(3, unitOfWork.SaveCount);

        // It is really gone: asking for it again gives a "not found" error.
        await Assert.ThrowsExactlyAsync<NotFoundException>(() =>
            getHandler.Handle(new GetWorkOrderByIdQuery(created.Id), CancellationToken.None).AsTask());
    }
} 
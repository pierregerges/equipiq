using EquipIQ.Core.Common;
using EquipIQ.Core.WorkOrders;

namespace EquipIQ.Core.UnitTests;

[TestClass]
public sealed class WorkOrderTests
{
    private static readonly DateTimeOffset Now = new(2026, 11, 13, 12, 0, 0, TimeSpan.Zero);

    // A valid work order we can reuse, so each test only shows what is special about it.
    private static WorkOrder CreateValidWorkOrder() =>
        WorkOrder.Create(
            title: "Ventilator error E12",
            description: "Screen shows E12 on startup.",
            equipmentName: "Ventilator V200",
            priority: WorkOrderPriority.High,
            createdBy: "tech@example.com",
            createdByAgent: false,
            createdAt: Now);

    // Walks a new (Open) work order forward until it reaches the status we want.
    private static WorkOrder CreateWorkOrderInStatus(WorkOrderStatus status)
    {
        var workOrder = CreateValidWorkOrder();

        switch (status)
        {
            case WorkOrderStatus.InProgress:
                workOrder.ChangeStatus(WorkOrderStatus.InProgress);
                break;
            case WorkOrderStatus.Completed:
                workOrder.ChangeStatus(WorkOrderStatus.InProgress);
                workOrder.ChangeStatus(WorkOrderStatus.Completed);
                break;
            case WorkOrderStatus.Cancelled:
                workOrder.ChangeStatus(WorkOrderStatus.Cancelled);
                break;
        }

        return workOrder;
    }

    // ---------- Create ----------

    [TestMethod]
    public void Create_WithValidData_SetsFieldsAndStartsOpen()
    {
        var workOrder = CreateValidWorkOrder();

        Assert.AreNotEqual(Guid.Empty, workOrder.Id);
        Assert.AreEqual("Ventilator error E12", workOrder.Title);
        Assert.AreEqual("Screen shows E12 on startup.", workOrder.Description);
        Assert.AreEqual("Ventilator V200", workOrder.EquipmentName);
        Assert.AreEqual(WorkOrderPriority.High, workOrder.Priority);
        Assert.AreEqual(WorkOrderStatus.Open, workOrder.Status);
        Assert.AreEqual("tech@example.com", workOrder.CreatedBy);
        Assert.IsFalse(workOrder.CreatedByAgent);
        Assert.AreEqual(Now, workOrder.CreatedAt);
    }

    [TestMethod]
    public void Create_WithValidData_RaisesWorkOrderCreatedEvent()
    {
        var workOrder = CreateValidWorkOrder();

        Assert.HasCount(1, workOrder.DomainEvents);
        var domainEvent = workOrder.DomainEvents.Single() as WorkOrderCreated;
        Assert.IsNotNull(domainEvent);
        Assert.AreEqual(workOrder.Id, domainEvent.WorkOrderId);
        Assert.AreEqual(workOrder.Title, domainEvent.Title);
        Assert.AreEqual(WorkOrderPriority.High, domainEvent.Priority);
        Assert.AreEqual(Now, domainEvent.OccurredAt);
    }

    [TestMethod]
    public void Create_WithPaddedText_TrimsIt()
    {
        var workOrder = WorkOrder.Create(
            "  Title  ", "  Description  ", "  Pump  ",
            WorkOrderPriority.Low, "  tech@example.com  ", false, Now);

        Assert.AreEqual("Title", workOrder.Title);
        Assert.AreEqual("Description", workOrder.Description);
        Assert.AreEqual("Pump", workOrder.EquipmentName);
        Assert.AreEqual("tech@example.com", workOrder.CreatedBy);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    public void Create_WithBlankTitle_Throws(string title)
    {
        var exception = Assert.ThrowsExactly<DomainException>(() =>
            WorkOrder.Create(title, "desc", "pump", WorkOrderPriority.Low, "tech", false, Now));

        Assert.AreEqual("Title is required.", exception.Message);
    }

    [TestMethod]
    public void Create_WithTitleOverMaxLength_Throws()
    {
        var tooLong = new string('x', WorkOrder.TitleMaxLength + 1);

        Assert.ThrowsExactly<DomainException>(() =>
            WorkOrder.Create(tooLong, "desc", "pump", WorkOrderPriority.Low, "tech", false, Now));
    }

    [TestMethod]
    public void Create_WithUndefinedPriority_Throws()
    {
        var exception = Assert.ThrowsExactly<DomainException>(() =>
            WorkOrder.Create("title", "desc", "pump", (WorkOrderPriority)99, "tech", false, Now));

        Assert.AreEqual("Priority is not valid.", exception.Message);
    }

    [TestMethod]
    public void Create_ByAgent_RemembersThatAnAgentCreatedIt()
    {
        var workOrder = WorkOrder.Create(
            "title", "desc", "pump", WorkOrderPriority.Medium, "agent", true, Now);

        Assert.IsTrue(workOrder.CreatedByAgent);
    }

    // ---------- UpdateDetails ----------

    [TestMethod]
    public void UpdateDetails_WhenOpen_ChangesTheDetails()
    {
        var workOrder = CreateValidWorkOrder();

        workOrder.UpdateDetails("New title", "New description", "New pump", WorkOrderPriority.Critical);

        Assert.AreEqual("New title", workOrder.Title);
        Assert.AreEqual("New description", workOrder.Description);
        Assert.AreEqual("New pump", workOrder.EquipmentName);
        Assert.AreEqual(WorkOrderPriority.Critical, workOrder.Priority);
    }

    [TestMethod]
    public void UpdateDetails_WithBlankTitle_ThrowsAndChangesNothing()
    {
        var workOrder = CreateValidWorkOrder();

        Assert.ThrowsExactly<DomainException>(() =>
            workOrder.UpdateDetails("", "New description", "New pump", WorkOrderPriority.Low));

        // The failed update must not leave a half-edited object behind.
        Assert.AreEqual("Ventilator error E12", workOrder.Title);
        Assert.AreEqual("Screen shows E12 on startup.", workOrder.Description);
        Assert.AreEqual(WorkOrderPriority.High, workOrder.Priority);
    }

    [TestMethod]
    [DataRow(WorkOrderStatus.Completed)]
    [DataRow(WorkOrderStatus.Cancelled)]
    public void UpdateDetails_WhenFinished_Throws(WorkOrderStatus finishedStatus)
    {
        var workOrder = CreateWorkOrderInStatus(finishedStatus);

        Assert.ThrowsExactly<DomainException>(() =>
            workOrder.UpdateDetails("title", "desc", "pump", WorkOrderPriority.Low));
    }

    // ---------- ChangeStatus ----------

    [TestMethod]
    [DataRow(WorkOrderStatus.Open, WorkOrderStatus.InProgress)]
    [DataRow(WorkOrderStatus.Open, WorkOrderStatus.Cancelled)]
    [DataRow(WorkOrderStatus.InProgress, WorkOrderStatus.Completed)]
    [DataRow(WorkOrderStatus.InProgress, WorkOrderStatus.Cancelled)]
    public void ChangeStatus_WithAllowedMove_ChangesTheStatus(WorkOrderStatus from, WorkOrderStatus to)
    {
        var workOrder = CreateWorkOrderInStatus(from);

        workOrder.ChangeStatus(to);

        Assert.AreEqual(to, workOrder.Status);
    }

    [TestMethod]
    [DataRow(WorkOrderStatus.Open, WorkOrderStatus.Open)]
    [DataRow(WorkOrderStatus.Open, WorkOrderStatus.Completed)]
    [DataRow(WorkOrderStatus.InProgress, WorkOrderStatus.Open)]
    [DataRow(WorkOrderStatus.Completed, WorkOrderStatus.Open)]
    [DataRow(WorkOrderStatus.Completed, WorkOrderStatus.InProgress)]
    [DataRow(WorkOrderStatus.Completed, WorkOrderStatus.Cancelled)]
    [DataRow(WorkOrderStatus.Cancelled, WorkOrderStatus.Open)]
    [DataRow(WorkOrderStatus.Cancelled, WorkOrderStatus.InProgress)]
    public void ChangeStatus_WithForbiddenMove_ThrowsAndKeepsTheStatus(WorkOrderStatus from, WorkOrderStatus to)
    {
        var workOrder = CreateWorkOrderInStatus(from);

        Assert.ThrowsExactly<DomainException>(() => workOrder.ChangeStatus(to));

        Assert.AreEqual(from, workOrder.Status);
    }

    // ---------- Domain events ----------

    [TestMethod]
    public void ClearDomainEvents_RemovesAllRaisedEvents()
    {
        var workOrder = CreateValidWorkOrder();

        workOrder.ClearDomainEvents();

        Assert.IsEmpty(workOrder.DomainEvents);
    }
}
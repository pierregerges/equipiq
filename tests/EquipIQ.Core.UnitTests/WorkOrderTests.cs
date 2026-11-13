using EquipIQ.Core.Common;
using EquipIQ.Core.WorkOrders;

namespace EquipIQ.Core.UnitTests;

[TestClass]
public sealed class WorkOrderTests
{
    private static readonly DateTimeOffset Now = new(2026, 11, 13, 12, 0, 0, TimeSpan.Zero);

    private static WorkOrder CreateValidWorkOrder() =>
        WorkOrder.Create(
            "Ventilator error E12", "Screen shows E12 on startup.", "Ventilator V200",
            WorkOrderPriority.High, "tech@example.com", false, Now);

    [TestMethod]
    public void Create_WithValidData_BuildsAnOpenWorkOrderAndRaisesAnEvent()
    {
        var workOrder = CreateValidWorkOrder();

        Assert.AreEqual("Ventilator error E12", workOrder.Title);
        Assert.AreEqual(WorkOrderStatus.Open, workOrder.Status);
        Assert.HasCount(1, workOrder.DomainEvents);
        Assert.IsInstanceOfType<WorkOrderCreated>(workOrder.DomainEvents.Single());
    }

    [TestMethod]
    public void UpdateDetails_WithValidData_ChangesTheDetails()
    {
        var workOrder = CreateValidWorkOrder();

        workOrder.UpdateDetails("New title", "New description", "New pump", WorkOrderPriority.Critical);

        Assert.AreEqual("New title", workOrder.Title);
        Assert.AreEqual("New description", workOrder.Description);
        Assert.AreEqual("New pump", workOrder.EquipmentName);
        Assert.AreEqual(WorkOrderPriority.Critical, workOrder.Priority);
    }

    [TestMethod]
    public void Create_WithInvalidInput_ThrowsADomainException()
    {
        // Blank title, too-long title, blank description, and a priority that is not on the menu.
        Assert.ThrowsExactly<DomainException>(() =>
            WorkOrder.Create("", "desc", "pump", WorkOrderPriority.Low, "tech", false, Now));

        Assert.ThrowsExactly<DomainException>(() =>
            WorkOrder.Create(new string('x', WorkOrder.TitleMaxLength + 1), "desc", "pump",
                WorkOrderPriority.Low, "tech", false, Now));

        Assert.ThrowsExactly<DomainException>(() =>
            WorkOrder.Create("title", "", "pump", WorkOrderPriority.Low, "tech", false, Now));

        Assert.ThrowsExactly<DomainException>(() =>
            WorkOrder.Create("title", "desc", "pump", (WorkOrderPriority)99, "tech", false, Now));
    }
} 
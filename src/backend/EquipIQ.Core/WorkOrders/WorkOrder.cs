using EquipIQ.Core.Common;

namespace EquipIQ.Core.WorkOrders;

public sealed class WorkOrder : Entity
{
    // Length limits live here so validators and the database can reuse them.
    public const int TitleMaxLength = 200;
    public const int DescriptionMaxLength = 4000;
    public const int EquipmentNameMaxLength = 200;
    public const int CreatedByMaxLength = 256;

    // Only EF Core uses this, when it loads a work order from the database.
    private WorkOrder()
    {
    }

    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string EquipmentName { get; private set; } = string.Empty;
    public WorkOrderPriority Priority { get; private set; }
    public WorkOrderStatus Status { get; private set; }
    public string CreatedBy { get; private set; } = string.Empty;
    public bool CreatedByAgent { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>The only way to make a new work order, so every one starts valid.</summary>
    public static WorkOrder Create(
        string title,
        string description,
        string equipmentName,
        WorkOrderPriority priority,
        string createdBy,
        bool createdByAgent,
        DateTimeOffset createdAt)
    {
        var workOrder = new WorkOrder
        {
            Id = Guid.CreateVersion7(),
            Title = RequireText(title, "Title", TitleMaxLength),
            Description = RequireText(description, "Description", DescriptionMaxLength),
            EquipmentName = RequireText(equipmentName, "Equipment name", EquipmentNameMaxLength),
            Priority = RequireValidPriority(priority),
            Status = WorkOrderStatus.Open,
            CreatedBy = RequireText(createdBy, "Created by", CreatedByMaxLength),
            CreatedByAgent = createdByAgent,
            CreatedAt = createdAt
        };

        workOrder.RaiseDomainEvent(
            new WorkOrderCreated(workOrder.Id, workOrder.Title, workOrder.Priority, createdAt));

        return workOrder;
    }

    public void UpdateDetails(
        string title,
        string description,
        string equipmentName,
        WorkOrderPriority priority)
    {
        if (Status is WorkOrderStatus.Completed or WorkOrderStatus.Cancelled)
        {
            throw new DomainException($"A {Status} work order can no longer be edited.");
        }

        // Check everything first, then change. A failure never leaves a half-edited object.
        var newTitle = RequireText(title, "Title", TitleMaxLength);
        var newDescription = RequireText(description, "Description", DescriptionMaxLength);
        var newEquipmentName = RequireText(equipmentName, "Equipment name", EquipmentNameMaxLength);
        var newPriority = RequireValidPriority(priority);

        Title = newTitle;
        Description = newDescription;
        EquipmentName = newEquipmentName;
        Priority = newPriority;
    }

    public void ChangeStatus(WorkOrderStatus newStatus)
    {
        if (!IsTransitionAllowed(Status, newStatus))
        {
            throw new DomainException($"A work order cannot go from {Status} to {newStatus}.");
        }

        Status = newStatus;
    }

    private static bool IsTransitionAllowed(WorkOrderStatus from, WorkOrderStatus to) =>
        (from, to) switch
        {
            (WorkOrderStatus.Open, WorkOrderStatus.InProgress) => true,
            (WorkOrderStatus.Open, WorkOrderStatus.Cancelled) => true,
            (WorkOrderStatus.InProgress, WorkOrderStatus.Completed) => true,
            (WorkOrderStatus.InProgress, WorkOrderStatus.Cancelled) => true,
            _ => false
        };

    private static string RequireText(string value, string fieldName, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException($"{fieldName} is required.");
        }

        var trimmed = value.Trim();

        if (trimmed.Length > maxLength)
        {
            throw new DomainException($"{fieldName} cannot be longer than {maxLength} characters.");
        }

        return trimmed;
    }

    private static WorkOrderPriority RequireValidPriority(WorkOrderPriority priority)
    {
        if (!Enum.IsDefined(priority))
        {
            throw new DomainException("Priority is not valid.");
        }

        return priority;
    }
}
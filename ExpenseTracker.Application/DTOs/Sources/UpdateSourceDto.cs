namespace ExpenseTracker.Application.DTOs.Sources;

public class UpdateSourceDto : InsertSourceDto
{
    public Guid Id { get; set; }
}
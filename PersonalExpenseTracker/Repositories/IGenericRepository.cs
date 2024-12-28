namespace PersonalExpenseTracker.Repositories;

public interface IGenericRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"> Generic class which is used to call the following class's respective data. </typeparam>
    /// <param name="filePath"></param>
    /// <returns></returns>
    List<T> GetAll<T>(string filePath); 

    void SaveAll<T>(List<T> entity, string directoryPath, string filePath);
}

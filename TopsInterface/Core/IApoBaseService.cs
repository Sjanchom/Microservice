namespace TopsInterface.Core
{
    public interface IApoBaseService<RETURNTYPE, ACCEPTTYPE> where RETURNTYPE : class
        where ACCEPTTYPE : class

    {
        PagedList<RETURNTYPE> GetAll(int page, int pageSize, string searchText);
        RETURNTYPE GetById(int id);
        RETURNTYPE Create(ACCEPTTYPE item);
        RETURNTYPE Edit(int id,ACCEPTTYPE item);
        bool Delete(int id);

        RETURNTYPE GetByName(ACCEPTTYPE item);
    }
}
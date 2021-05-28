using System.Collections.Generic;

namespace QuestStoreNAT.web.DatabaseLayer
{
    public interface IDB_GenericInterface<T>
    {
        public abstract T FindOneRecordBy(string email);
        public abstract T FindOneRecordBy(int id);
        public List<T> FetchAllRecords();
        public abstract void AddRecord(T recordToAdd);
        public abstract void UpdateRecord(T recordToUpdate);
        public abstract void DeleteRecord(int id);
    }
}

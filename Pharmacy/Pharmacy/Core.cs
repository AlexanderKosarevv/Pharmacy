using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy
{
    class Core
    {
        public static int VOID = -255;
        public static int NONE = -1;


        //***************************************************************
        // подключение к базе данных и описание ее сущностей
        private static Data.KosarevKursovikAptekaEntities _Database;

        public static Data.KosarevKursovikAptekaEntities Database
        {
            get
            {
                if (_Database == null)
                {
                    _Database = new Data.KosarevKursovikAptekaEntities();
                }
                return _Database;
            }
        }
        //***************************************************************

        public static MainWindow AppMainWindow;

        public static Data.Users CurrentUser;

        public static void CancelAllChanges()
        {
            var ChangedEntries = Database.ChangeTracker.Entries()
                .Where(E => E.State != EntityState.Unchanged);
            foreach (DbEntityEntry E in ChangedEntries)
            {
                CancelChanges(E);
            }
        }

        // отмена изменений
        public static void CancelChanges(object SrcEntry) // entry - одна запись из бд
        {
            // преобразование типа
            DbEntityEntry Entry = Database.Entry(SrcEntry);
            try
            {
                switch (Entry.State)
                {
                    case EntityState.Added:
                        //открепляем состояние
                        Entry.State = EntityState.Detached;
                        break;

                    case EntityState.Modified:
                        // восстанавливаем оригинальные значения
                        Entry.CurrentValues.SetValues(Entry.OriginalValues);
                        Entry.State = EntityState.Unchanged;
                        break;

                    case EntityState.Deleted:
                        Entry.State = EntityState.Unchanged;
                        break;
                }
            }
            catch
            {
                // перезагружаем запись из бд
                Entry.Reload();
            }
        }
    }
}

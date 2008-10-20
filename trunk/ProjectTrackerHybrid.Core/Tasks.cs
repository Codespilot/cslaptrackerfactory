using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;
using Iesi.Collections;

namespace ProjectTracker.Library
{
    [Serializable]
    public class Tasks : BusinessListBase<Tasks, Task> , IList<Task>
    {
        #region Factory Methods

        public static Tasks NewTasks()
        {
            return DataPortal.CreateChild<Tasks>();
        }

        public static Tasks GetTasks(Guid ProjectId)
        {
            return DataPortal.FetchChild<Tasks>(new SingleCriteria<Tasks, Guid>(ProjectId));
        }


        private Tasks()
        {  /* Require use of factory methods */
            AllowNew = true;
            AllowEdit = true;
            AllowRemove = true;
        }

        #endregion

        public void Add(ISet theSet)
        {
            RaiseListChangedEvents = false;

            foreach (Task obj in theSet)
            {
                this.Add(obj);
            }

            RaiseListChangedEvents = true;
        }

        public override Tasks Save()
        {
            return base.Save();
        }

        private void Child_Fetch(SingleCriteria<Tasks, Guid> criteria)
        {

        }

        
    }
}

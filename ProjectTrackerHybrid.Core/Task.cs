using System;
using Csla;
using ProjectTracker.Library.Data;
using StructureMap;
using CslaFactory = Csla.Server.ObjectFactoryAttribute;

namespace ProjectTracker.Library
{
    [CslaFactory("ProjectTrackerHybrid.Core.TaskFactory, ProjectTrackerHybrid.Core")]
    [Serializable]
    public class Task : BusinessBase<Task>
    {

        private IRepository<Task> _repository;
        private static PropertyInfo<Guid> ProjectIdProperty = RegisterProperty<Guid>(typeof(Task), new PropertyInfo<Guid>("ProjectId", "Project Id"));
        public Guid ProjectId
        {
            get { return GetProperty<Guid>(ProjectIdProperty); }
            set { SetProperty<Guid>(ProjectIdProperty, value); }
        }

        private static PropertyInfo<Guid> IdProperty = RegisterProperty<Guid>(typeof(Task), new PropertyInfo<Guid>("Id", "Id"));
        public Guid Id
        {
            get { return GetProperty<Guid>(IdProperty); }
            set { SetProperty<Guid>(IdProperty, value); }
        }

        private static PropertyInfo<string> NameProperty = RegisterProperty<string>(typeof(Task), new PropertyInfo<string>("Name", "Name"));
        public string Name
        {
            get { return GetProperty<string>(NameProperty); }
            set { SetProperty<string>(NameProperty, value); }
        }

        private static PropertyInfo<string> DescriptionProperty = RegisterProperty<string>(typeof(Task), new PropertyInfo<string>("Description", "Description"));
        public string Description
        {
            get { return GetProperty<string>(DescriptionProperty); }
            set { SetProperty<string>(DescriptionProperty, value); }
        }

        #region Factory Methods

        public static Task NewTask()
        {
            return DataPortal.CreateChild<Task>();
        }

        public static Task GetTask(Guid id)
        {
            return DataPortal.FetchChild<Task>(new SingleCriteria<Task, Guid>(id));
        }


        private Task()
        {  /* Require use of factory methods */

            _repository = ObjectFactory.GetInstance<IRepository<Task>>();
        }

        #endregion

        #region Data Access

        private void Child_Insert(Project project)
        {
            ProjectId = project.Id;
            if (_repository != null) _repository.Save(this);
            //UnitOfWork.CurrentSession.Save(this);
            //UnitOfWork.Current.TransactionalFlush();
            //UnitOfWork.CurrentSession.Clear();
        }

        private void Child_Update(Project project)
        {
            ProjectId = project.Id;
            if (_repository != null) _repository.SaveOrUpdate(this);
            //UnitOfWork.CurrentSession.SaveOrUpdate(this);
            //UnitOfWork.Current.TransactionalFlush();
            //UnitOfWork.CurrentSession.Clear();
        }

        public new void MarkOld()
        {
            base.MarkOld();
        }

        #endregion
    }
}

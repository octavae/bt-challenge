using System;

namespace JobsScheduler.Jobs
{
    class Job
    {
        #region Private Members

        private long _JobId;
        private string _JobName;
        private string _JobType;
        private string _JobStatus;

        #endregion

        #region Constructors

        // initialization
        public Job()
        {
        }

        #endregion

        #region Public Properties

        public long JobId
        {
            get
            {
                return _JobId;
            }
            set
            {
                _JobId = value;
            }
        }

        public string JobName
        {
            get
            {
                return _JobName;
            }
            set
            {
                _JobName = value;
            }
        }

        public string JobType
        {
            get
            {
                return _JobType;
            }
            set
            {
                _JobType = value;
            }
        }

        public string JobStatus
        {
            get
            {
                return _JobStatus;
            }
            set
            {
                _JobStatus = value;
            }
        }

        #endregion
    }
}

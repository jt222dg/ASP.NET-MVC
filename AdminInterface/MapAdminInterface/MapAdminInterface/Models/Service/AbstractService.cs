using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MapAdminInterface.Models
{
    abstract public class AbstractService
    {
        protected IRepository _repository;

        public AbstractService()
            : this(new Repository())
        {
        }

        public AbstractService(IRepository repository)
        {
            this._repository = repository;
        }

        protected void Save()
        {
            this._repository.Save();
        }
    }
}

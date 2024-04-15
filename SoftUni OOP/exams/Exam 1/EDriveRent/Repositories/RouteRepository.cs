using EDriveRent.Models;
using EDriveRent.Models.Contracts;
using EDriveRent.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDriveRent.Repositories
{
    public class RouteRepository : IRepository<IRoute>
    {
        List<IRoute> _routes;

        public RouteRepository()
        {
            _routes = new List<IRoute>();
        }

        public void AddModel(IRoute model) => _routes.Add(model);

        public IRoute FindById(string identifier) => _routes.FirstOrDefault(x => x.RouteId == int.Parse(identifier));

        public IReadOnlyCollection<IRoute> GetAll() => _routes.AsReadOnly();

        public bool RemoveById(string identifier)
        {
            IRoute route = _routes.FirstOrDefault(x => x.RouteId == int.Parse(identifier));

            return _routes.Remove(route);
        }
    }
}

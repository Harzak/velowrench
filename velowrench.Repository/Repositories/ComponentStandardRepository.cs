using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Repository.Catalogs;
using velowrench.Repository.Interfaces;
using velowrench.Repository.Models;

namespace velowrench.Repository.Repositories;

public sealed class ComponentStandardRepository : IComponentStandardRepository
{
    public IReadOnlyCollection<CranksetSpecificationModel> GetAllCranksetSpecifications()
    {
        return HardCodedComponentStandardCatalog.GetAllCranksetSpecifications;
    }

    public IReadOnlyCollection<CadenceModel> GetAllCandences()
    {
        return HardCodedComponentStandardCatalog.GetAllCandences;
    }

    public IReadOnlyCollection<WheelSpecificationModel> GetMostCommonWheelSpecifications()
    {
        return HardCodedComponentStandardCatalog.GetMostCommonWheelSpecifications;
    }
}
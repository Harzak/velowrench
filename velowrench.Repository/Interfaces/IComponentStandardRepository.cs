using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Repository.Models;

namespace velowrench.Repository.Interfaces;

public interface IComponentStandardRepository
{
    IReadOnlyCollection<CranksetSpecificationModel> GetAllCranksetSpecifications();
    IReadOnlyCollection<CadenceModel> GetAllCandences();
    IReadOnlyCollection<WheelSpecificationModel> GetMostCommonWheelSpecifications();
    IReadOnlyCollection<SprocketSpecificationModel> GetMostCommonSprocketSpecifications();
}


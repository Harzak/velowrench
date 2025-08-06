using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Core.Enums;

namespace velowrench.Core.Interfaces;

public interface IToolsViewModelFactory
{
    IRoutableViewModel CreateRoutableViewModel(EVeloWrenchTools type, IScreen hostScreen);
}
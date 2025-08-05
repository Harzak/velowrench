using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velowrench.Enums;

namespace velowrench.Interfaces;

public interface IToolsViewModelFactory
{
    IRoutableViewModel CreateRoutableViewModel(EVeloWrenchTools type, IScreen hostScreen);
}
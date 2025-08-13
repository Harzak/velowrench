using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velowrench.Core.Models;

public class SelectibleModel<T> where T : class
{
    public bool IsSelected { get; set; }
    public T Value { get; set; }

    public SelectibleModel(T value)
    {
        this.Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    public SelectibleModel(T value, bool isSelected) : this(value)
    {
        this.IsSelected = isSelected;
    }
}
